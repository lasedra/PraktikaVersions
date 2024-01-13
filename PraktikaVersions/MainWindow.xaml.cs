using PraktikaVersions.Models;
using Squirrel;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PraktikaVersions
{
    public partial class MainWindow : Window
    {
        UpdateManager updateManager = null!;

        public MainWindow()
        {
            InitializeComponent();
            UserListView.ItemsSource = PracticeDbContext.dbContext.Users.ToList();
            Loaded += Window_Loaded;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                updateManager = await UpdateManager.GitHubUpdateManager(@"https://github.com/lasedra/PraktikaVersions");
                if (updateManager.CurrentlyInstalledVersion() != null)
                    VersionTextBox.Text = updateManager.CurrentlyInstalledVersion().ToString();
                else
                    VersionTextBox.Text = "...";

                var updateInfo = await updateManager.CheckForUpdate();
                if (updateInfo.ReleasesToApply.Count > 0)
                {
                    if (MessageBox.Show("Update the app?", "New version is available!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await updateManager.UpdateApp();
                        MessageBox.Show("Please, restart the application to apply a new version", "Updated successfully!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Net.Http.HttpRequestException)
                {
                    VersionStackPanel.Visibility = Visibility.Hidden;
                    FailTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    // Обработка других типов исключений
                    MessageBox.Show("Unknown error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
