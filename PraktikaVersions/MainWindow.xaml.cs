using Octokit;
using PraktikaVersions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace PraktikaVersions
{
    public partial class MainWindow : Window
    {
        GitHubClient gitClient = null!;
        IReadOnlyList<Release> releases = null!;

        public MainWindow()
        {
            InitializeComponent();
            UserListView.ItemsSource = PracticeDbContext.dbContext.Users.ToList();
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateChecker();
            GetReleases();
        }


        private async void UpdateBttn_Click(object sender, RoutedEventArgs e)
        {
            if(VersionsComboBox.SelectedItem != null)
            {
                var releaseToDownload = releases.FirstOrDefault(r => r.TagName == ((Release)VersionsComboBox.SelectedItem).TagName);

                var result = MessageBox.Show($"Are you surer you wanna update to version: {releaseToDownload.TagName}?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (WebClient client = new())
                    {
                        client.Headers.Add("Authorization", $"Bearer {gitClient.Credentials.GetToken()}");
                        client.Headers.Add("User-Agent", "smth");

                        await client.DownloadFileTaskAsync(releaseToDownload.ZipballUrl,
                            $@"{Path.Combine(GetParentDirectory(AppDomain.CurrentDomain.BaseDirectory, 4), @"updates")}\{releaseToDownload.TagName} version.zip");
                        //TODO: Скачивается не полностью и долго. Нет прогресс бара.
                    }
                }
            }
        }

        private async void GetReleases()
        {
            try
            {
                gitClient = new GitHubClient(new ProductHeaderValue("praktika-versions"))
                { Credentials = new Credentials("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") };
                string userName = "lasedra",
                       reposName = "PraktikaVersions";

                releases = await gitClient.Repository.Release.GetAll(userName, reposName);
                if (releases != null)
                    VersionsComboBox.ItemsSource = releases;
                else 
                {
                    //TODO: Отображать "Отсутствие каких-либо версий"
                }

            }
            catch (Exception ex)
            {
                if (ex is System.Net.Http.HttpRequestException)
                {
                    FailTextBox.Visibility = Visibility.Visible;
                    VersionsPanel.Visibility = Visibility.Hidden;
                }
                else
                    MessageBox.Show("Unknown error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateChecker()
        {
            //TODO: Проверятель обновлений
        }

        private static string GetParentDirectory(string path, int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                path = Directory.GetParent(path).FullName;
            }
            return path;
        }
    }
}
