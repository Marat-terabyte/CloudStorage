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

        static void Main(string[] args)
        {
            Config config = ConfigReader.ReadConfig();
            if (config.Host == null || config.MainPort == null || config.DataTransferPort == null)
            {
                Console.WriteLine("'Host' or 'MainPort' or 'DataTransferPort' in appsettings.json are empty");
                
                return;
            }

            CloudStorageServer server = new CloudStorageServer(config.Host, (int) config.MainPort, (int) config.DataTransferPort);

            while (true)
            {
                CloudStorageClient client = server.AcceptClient();
                
                new Thread(() =>
                {
                    try
                    {
                        Request request = client.ReceiveRequest();
                        if (request.SessionId == null || request.SessionId == 0) // The user has not been assigned an ID
                        {
                            UserSession session = CreateSession();
                            Sessions.Add(session.SessionID, session);

                            session.Execute(client, request);
                        }
                        else
                        {
                            try
                            {
                                UserSession session = Sessions[(long)request.SessionId];
                                if (session.Username == request.Username)
                                    session.Execute(client, request);
                                else
                                    client.SendResponse(new Response(CommandStatus.NotOk), "Your request data is invalid");
                            }
                            catch (KeyNotFoundException)
                            {
                                client.SendResponse(new Response(CommandStatus.NotOk), "Your session id is invalid");
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex);
                    }
                }).Start();
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
