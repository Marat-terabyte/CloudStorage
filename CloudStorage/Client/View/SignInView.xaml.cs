// MIT License
// Copyright (c) 2024 Marat

using Client.ViewModel;
using System.Windows.Controls;

namespace Client.View
{
    public partial class SignInView : UserControl
    {
        public SignInView(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = new SignInViewModel(viewModel);
        }
    }
}
