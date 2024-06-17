// MIT License
// Copyright (c) 2024 Marat

using Client.View;

namespace Client.ViewModel
{
    public class CloudStorageViewModel : ViewModel
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

        public CloudStorageViewModel()
        {
            _currentContent = new CloudFileListView();
        }
    }
}
