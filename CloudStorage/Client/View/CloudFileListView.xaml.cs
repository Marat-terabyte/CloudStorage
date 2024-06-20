// MIT License
// Copyright (c) 2024 Marat

using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Client.View
{
    public partial class CloudFileListView : UserControl
    {
        public CloudFileListView()
        {
            InitializeComponent();
        }

        private void ListBoxDrop(object sender, System.Windows.DragEventArgs e)
        {
            string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null)
            {
                (DataContext as CloudFileListViewModel)?.SendFileCommand.Execute(files);
            }
        }
    }
}
