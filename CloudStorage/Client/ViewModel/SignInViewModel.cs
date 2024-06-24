// MIT License
// Copyright (c) 2024 Marat

using Client.Core;
using Client.Model;
using Client.View;
using ClientLibrary.Commands;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class SignInViewModel : ViewModel
    {
        private MainWindowViewModel _mainWindow;

        public User User { get; set; }

        public SignInViewModel(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
            User = new User();
        }

        public ICommand SignInCommand
        {
            get
            {
                return new RelayCommand(o => SignIn());
            }
        }

        public ICommand GoToSignUp
        {
            get
            {
                return new RelayCommand(o => _mainWindow.CurrentContent = new SignUpView(_mainWindow));
            }
        }

        private async void SignIn()
        {
            string? message = null;
            bool successful = false;

            await Task.Run(() => successful = new SignInCommand(User?.Username ?? "", User?.Password ?? "").Execute(out message));
            if (successful)
                new CloudStorageWindow().Show();
            else
                MessageBox.Show(message);
        }
    }
}
