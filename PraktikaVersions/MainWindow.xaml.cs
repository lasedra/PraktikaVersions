using Octokit;
using PraktikaVersions.Models;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace PraktikaVersions
{
    public partial class MainWindow : Window
    {
        Updater updater = new Updater();

        public MainWindow()
        {
            InitializeComponent();
            UserListView.ItemsSource = PracticeDbContext.dbContext.Users.ToList();

            if (updater != null && updater.IsConnectionOk())
            {
                VersionTextBox.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if(updater.Releases.Result.Any())
                    VersionsComboBox.ItemsSource = updater.Releases.Result;
                else
                {
                    AvailableVersionsTextBlock.Text = "No any available version";
                    VersionsComboBox.IsEnabled = false;
                    UpdateBttn.IsEnabled = false;
                }
            }
            else
            {
                FailTextBox.Visibility = Visibility.Visible;
                VersionsPanel.Visibility = Visibility.Hidden;
            }
        }

        //TODO: Проверятель обновлений

        private void UpdateBttn_Click(object sender, RoutedEventArgs e)
        {
            if (VersionsComboBox.SelectedItem != null)
            {
                var releaseToDownload = ((Release)VersionsComboBox.SelectedItem);
                if (releaseToDownload.TagName.Trim() != VersionTextBox.Text.Trim())
                {
                    var result = MessageBox.Show($"Do you wanna update to version: {releaseToDownload.TagName}?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        DownloadWindow downloadWindow = new DownloadWindow(updater, releaseToDownload);
                        downloadWindow.ShowDialog();
                        updater.ApplyNewUpdate();
                    }
                }
            }
        }
    }
}
