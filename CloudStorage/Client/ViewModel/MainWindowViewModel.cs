// MIT License
// Copyright (c) 2024 Marat

using Client.View;

namespace Client.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        private object _currentContent;

        public object CurrentContent
        {
            get
            { 
                return _currentContent;
            }
            set
            {
                _currentContent = value;
                OnPropertyChanged(nameof(CurrentContent));
            }
        }

        public MainWindowViewModel()
        {
            _currentContent = new SignInView(this);
        }
    }
}
