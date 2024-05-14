// MIT License
// Copyright (c) 2024 Marat

using System.Net.Sockets;
using CloudStorageLibrary;
using Server.Session;
using Server.Database;
using CloudStorageLibrary.Commands;
using Server.Configuration;
using Configuration;

namespace Server
{
    internal class Program
    {
        public static Dictionary<long, UserSession> Sessions = new Dictionary<long, UserSession>();
        public static ApplicationContext Database = new ApplicationContext();

        async static Task Main(string[] args)
        {
            Config config = ConfigReader.ReadConfig();
            if (config.Host == null)
            {
                Console.WriteLine("'Host' in appsettings.json is empty");
                
                return;
            }

            Socket mainSocket = SocketBuilder.Build(config.Host, 7070);
            Socket dataTransferSocket = SocketBuilder.Build(config.Host, 7071);

            CheckSessions();
            while (true)
            {
                Socket client = mainSocket.Accept();
                Socket clientDataTransfer = dataTransferSocket.Accept();
                
                await Task.Run(() =>
                {
                    CloudStorageServer server = new CloudStorageServer(client, clientDataTransfer);

                    Request request = server.ReceiveRequest();
                    if (request.SessionId == null || request.SessionId == 0) // The user has not been assigned an ID
                    {
                        UserSession session = CreateSession();
                        Sessions.Add(session.SessionID, session);

                        session.Execute(server, request);
                    }
                    else
                    {
                        try
                        {
                            UserSession session = Sessions[(long) request.SessionId];
                            if (session.Username == request.Username)
                                session.Execute(server, request);
                            else
                                server.SendResponse(new Response(CommandStatus.NotOk), "Your request data is invalid");
                        }
                        catch (KeyNotFoundException)
                        {
                            server.SendResponse(new Response(CommandStatus.NotOk), "Your session id is invalid");
                        }
                    }
                });
            }
        }

        static UserSession CreateSession()
        {
            long id = new Random().NextInt64(1, long.MaxValue);
            var createdTime = DateTime.Parse(DateTime.Now.ToString("G"));

            UserSession session = new UserSession(id, createdTime, new UserRepository(Database));

            return session;
        }

        /// <summary>
        /// Checks each session and delete it if the lifetime has expired
        /// </summary>
        async static void CheckSessions()
        {
            TimeSpan limitOfUnauth = TimeSpan.FromMinutes(10);
            TimeSpan limitOfAuth = TimeSpan.FromHours(2);

            await Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(TimeSpan.FromMinutes(10));
                    foreach (var dict in Sessions)
                    {
                        UserSession session = dict.Value;
                        TimeSpan lifetime = DateTime.Now - session.Created;
                        if ((!session.IsAuthorizied && lifetime > limitOfUnauth) || (session.IsAuthorizied && lifetime > limitOfAuth))
                            Sessions.Remove(dict.Key);

                    }
                }
            });
        }
    }
}
