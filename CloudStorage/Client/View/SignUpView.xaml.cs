// MIT License
// Copyright (c) 2024 Marat

using Client.ViewModel;
using System.Windows.Controls;

namespace Client.View
{
    public partial class SignUpView : UserControl
    {
        public SignUpView(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = new SignUpViewModel(viewModel);
        }
    }
}