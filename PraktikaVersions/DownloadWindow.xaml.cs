using Octokit;
using System;
using System.Net;
using System.Windows;
using System.IO;

namespace PraktikaVersions
{
    public partial class DownloadWindow : Window
    {
        #region Disallow_closing
        // Prep stuff needed to remove close button on window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        #endregion

        Updater updater = null!;
        Release releaseToDownload = null!;

        public DownloadWindow(Updater _updater, Release _releaseToDownload)
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;

            this.updater = _updater;
            this.releaseToDownload = _releaseToDownload;
            TagNameTextBox.Text += releaseToDownload.TagName;
            NoteTextBox.Text = releaseToDownload.Body;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using(WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Bearer {updater.GitClient.Credentials.GetToken()}");
                client.Headers.Add("User-Agent", "smth");

                client.DownloadProgressChanged += (s, e)
                    => ProgressBar.Value = e.ProgressPercentage;
                client.DownloadFileCompleted += (s, e) =>
                { 
                    MessageBox.Show("The download is done!"); 
                    this.Close(); 
                };

                client.DownloadFileAsync(new Uri(releaseToDownload.Assets[0].BrowserDownloadUrl), $"newUpdate.exe");
            }
        }
    }
}
