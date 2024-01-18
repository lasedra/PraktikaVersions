using Octokit;
using PraktikaVersions.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;

namespace PraktikaVersions
{
    public partial class MainWindow : Window
    {
        public class Internet
        {
            public static bool IsOk()
            {
                try
                {
                    Dns.GetHostEntry("github.com");
                    return true;
                }
                catch { return false; }
            }
        }
        GitHubClient gitClient = null!;
        IReadOnlyList<Release> releases = null!;
        string exeName = AppDomain.CurrentDomain.FriendlyName + ".exe";
        string exePath = Process.GetCurrentProcess().MainModule.FileName;

        public MainWindow()
        {
            InitializeComponent();
            UserListView.ItemsSource = PracticeDbContext.dbContext.Users.ToList();
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VersionTextBox.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();

            UpdateChecker();
            GetReleases();
        }


        private void UpdateBttn_Click(object sender, RoutedEventArgs e)
        {
            if (VersionsComboBox.SelectedItem != null)
            {
                var releaseToDownload = releases.First(r => r.TagName == ((Release)VersionsComboBox.SelectedItem).TagName);
                if (releaseToDownload.TagName != VersionTextBox.Text) //TODO: Проверка на версию
                {
                    var result = MessageBox.Show($"Are you sure you wanna update to version: {releaseToDownload.TagName}?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        DownloadWindow downloadWindow = new DownloadWindow(gitClient, releaseToDownload);
                        downloadWindow.ShowDialog();
                        // TODO: Нужно сделать либо разархивацию, либо скачивать только .exe, через releaseToDownload.Assets[n] (а архив отдельно)
                        //Cmd($"taskkill /f /im \"{exeName}\" && timeout /t 1 && del \"{exePath}\" && ren \"{downloadedFile}\" \"{exePath}\" && \"{exePath}\"");
                    }
                }
            }
        }

        private async void GetReleases()
        {
            if (Internet.IsOk())
            {
                gitClient = new GitHubClient(new ProductHeaderValue("praktika-versions"))
                { Credentials = new Credentials("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") }; //TODO: Разобраться с токеном
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
            else
            {
                FailTextBox.Visibility = Visibility.Visible;
                VersionsPanel.Visibility = Visibility.Hidden;
            }
        }

        private async void UpdateChecker()
        {
            //TODO: Проверятель обновлений
        }

        private void Cmd(string line)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {line}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                });
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
