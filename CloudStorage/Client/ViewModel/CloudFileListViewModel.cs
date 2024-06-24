// MIT License
// Copyright (c) 2024 Marat

using Client.Core;
using Client.Model;
using ClientLibrary.CloudElements;
using ClientLibrary.Commands;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class CloudFileListViewModel : ViewModel
    {
        private ObservableCollection<ExtendedCloudElement> _cloudElements;
        private string _currentPath = "";

        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                _currentPath = value;
                OnPropertyChanged(nameof(CurrentPath));
            }
        }

        public ObservableCollection<ExtendedCloudElement> CloudElements
        {
            get => _cloudElements;
            set
            {
                _cloudElements = value;
                OnPropertyChanged(nameof(CloudElements));
            }
        }

        public ExtendedCloudElement? SelectedCloudElement { get; set; }

        public ICommand SendFileCommand {  get; set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public CloudFileListViewModel()
        {
            _cloudElements = ReceiveCloudElements();
            SendFileCommand = new RelayCommand(o => SendElement(o));
            DownloadCommand = new RelayCommand(o => DownloadElement());
            OpenCommand = new RelayCommand(o => OpenElement());
            DeleteCommand = new RelayCommand(o => DeleteElement());
        }

        private ObservableCollection<ExtendedCloudElement> ReceiveCloudElements()
        {
            bool isSuccess = new ListCommand(CurrentPath).Execute(out IEnumerable<CloudElement>? elements, out string? message);
            if (isSuccess && elements != null)
            {
                var cloudElements = new ObservableCollection<ExtendedCloudElement>();
                foreach (var item in elements)
                {
                    if (item is CloudFile)
                        cloudElements.Add(new ExtendedCloudFile(item.Path));
                    else
                        cloudElements.Add(new ExtendedCloudFolder(item.Path));
                }

                return cloudElements;
            }

            return new ObservableCollection<ExtendedCloudElement>();
        }

        private void OpenElement()
        {
            if (SelectedCloudElement is ExtendedCloudFolder)
            {
                ChangeDir(SelectedCloudElement.Name);
                CloudElements.Clear();
                CloudElements = ReceiveCloudElements();
            }
        }

        private async void SendElement(object o)
        {
            if (o is string[] paths)
            {
                foreach (var path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        CloudElements.Add(new ExtendedCloudFolder(path));
                    }
                    else if (File.Exists(path))
                    {
                        CloudElements.Add(new ExtendedCloudFile(path));
                    }

                    string? message = null;
                    await Task.Run(() => new UploadCommand(path, CurrentPath).Execute(out message));
                    MessageBox.Show(message);
                }
            }
        }

        private void DownloadElement()
        {
            if (SelectedCloudElement != null)
            {
                bool isSuccess = SelectedCloudElement.Download(out string? message);
                if (isSuccess)
                    MessageBox.Show("Successful downloaded");
                else
                    MessageBox.Show(message);
            }
        }

        private void DeleteElement()
        {
            if (SelectedCloudElement != null)
            {
                List<CloudElement> elements = new List<CloudElement>();
                elements.Add(((CloudElement) SelectedCloudElement));

                CloudElements.Remove(SelectedCloudElement);
                new RemoveCommand(elements).Execute(out string? message);
                MessageBox.Show(message);
            }
        }

        private void ChangeDir(string path)
        {
            if (path == "..")
            {
                int index = CurrentPath.LastIndexOf("\\");
                if (index > 0)
                    CurrentPath = CurrentPath[..index];
                else if (index == -1 && CurrentPath.Length > 0)
                    CurrentPath = "";
            }
            else
                CurrentPath = Path.Combine(CurrentPath, path);
        }
    }
}
