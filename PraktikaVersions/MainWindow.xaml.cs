using Octokit;
using PraktikaVersions.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Diagnostics;

namespace PraktikaVersions
{
    public partial class MainWindow : Window
    {
        Updater updater = new Updater();
        string currVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        string exeName = AppDomain.CurrentDomain.FriendlyName + ".exe";
        string exePath = Process.GetCurrentProcess().MainModule.FileName;

        public MainWindow()
        {
            InitializeComponent();
            UserListView.ItemsSource = PracticeDbContext.dbContext.Users.ToList();
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

            if (updater != null && updater.IsConnectionOk())
            {
                VersionTextBox.Text += currVer;
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

        private void UpdateBttn_Click(object sender, RoutedEventArgs e)
        {
            if (VersionsComboBox.SelectedItem != null)
            {
                var releaseToDownload = ((Release)VersionsComboBox.SelectedItem);
                if (releaseToDownload.TagName.Trim() != currVer)
                {
                    var result = MessageBox.Show($"Do you wanna update to version: {releaseToDownload.TagName}?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        DownloadWindow downloadWindow = new DownloadWindow(updater, releaseToDownload);
                        downloadWindow.ShowDialog();
                        updater.ApplyNewUpdate();
                    }
                }
                else { MessageBox.Show($"bruh...", "", MessageBoxButton.OK, MessageBoxImage.Question); }
            }
        }

        private void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;

            string exception = ex.InnerException + "\n" + ex.Message + "\n" + ex.StackTrace;
            SendMessage(exception);
            MessageBox.Show("The support is already warned via email.\nWe're gonna fix it up soon.", "An unexpected error occured!", MessageBoxButton.OK, MessageBoxImage.Error);
            try // TODO: сделать откат после ошибки почище
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c taskkill /f /im \"{exeName}\" && timeout /t 1 && {exePath}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                });
            }
            catch (Exception exс) { MessageBox.Show(exс.Message); }
        }

        private static void SendMessage(string exception)
        {
            string smtpServer = "smtp.mail.ru";
            int smtpPort = 587;
            string smtpUsername = "praktikasuppport@mail.ru";
            string smtpPassword = "Zd?Em?qhXZ?tAy?1?hgC?HU?hWg".Replace("?", "");

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add("praktikasuppport@mail.ru");
                    mailMessage.Subject = "В приложении возникла ошибка";
                    mailMessage.Body = exception;

                    try
                    {
                        smtpClient.Send(mailMessage);
                        MessageBox.Show("Message sent successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Message sending error: {ex.Message}");
                    }
                }
            }
        }
    }
}
