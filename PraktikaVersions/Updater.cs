using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace PraktikaVersions
{
    public class Updater
    {
        public GitHubClient? GitClient { get; private set; }
        public Task<IReadOnlyList<Release>>? Releases { get; private set; }
        private string? ExeName { get; set; } = AppDomain.CurrentDomain.FriendlyName + ".exe";
        private string? ExePath { get; set; } = Process.GetCurrentProcess().MainModule.FileName;

        public Updater()
        {
            if (IsConnectionOk())
            {
                Directory.SetCurrentDirectory(AppContext.BaseDirectory);

                GitClient = new GitHubClient(new ProductHeaderValue("praktika-versions"))
                    { Credentials = new Credentials("ghp_BHmZn?Xe?QCU1?Il11?s?T0ee?W3H?Ilq7?56r?2?UCU?2O".Replace("?", "")) };
                Releases = GitClient.Repository.Release.GetAll("lasedra", "PraktikaVersions");
            }
        }

        public bool IsConnectionOk()
        {
            try
            {
                Dns.GetHostEntry("github.com");
                return true;
            }
            catch { return false; }
        }

        public void ApplyNewUpdate()
        { Cmd($"taskkill /f /im \"{ExeName}\" && timeout /t 1 && del \"{ExePath}\" && ren newUpdate.exe \"{ExeName}\" && \"{ExePath}\""); }

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
