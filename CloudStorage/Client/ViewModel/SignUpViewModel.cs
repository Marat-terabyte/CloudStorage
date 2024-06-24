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
    public class SignUpViewModel : ViewModel
    {
        private MainWindowViewModel _mainWindow;

        public User User { get; set; }

        public SignUpViewModel(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
            User = new User();
        }

        public RelayCommand SignUpCommand
        {
            get
            {
                return new RelayCommand(o => SignUp());   
            }
        }

        public ICommand GoToSignIn
        {
            get
            {
                return new RelayCommand(o => _mainWindow.CurrentContent = new SignInView(_mainWindow));
            }
        }

        public async void SignUp()
        {
            bool successful = false;
            string? message = null;

            await Task.Run(() => successful = new SignUpCommand(User?.Username ?? "", User?.Password ?? "").Execute(out message));

            if (successful)
                _mainWindow.CurrentContent = new SignInView(_mainWindow);
            else
                MessageBox.Show(message);
        }
    }
}
