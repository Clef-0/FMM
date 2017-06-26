using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using IniParser;
using IniParser.Model;
using WPF.JoshSmith.ServiceProviders.UI;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using SharpSvn;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void infobarDLDownload_Click(object sender, RoutedEventArgs e)
        {
            if (!workerDownloadMods.IsBusy)
            {
                workerDownloadMods.RunWorkerAsync();
            }
        }

        private void dlModWorker(object sender, DoWorkEventArgs e)
        {
            List<Mod> checkedMods = new List<Mod>();

            foreach (Mod listedMod in downloadableModsList.Items)
            {
                if (listedMod.IsChecked == true)
                {
                    checkedMods.Add(listedMod);
                }
            }
            if (checkedMods.Count == 0)
            {
                return;
            }

            foreach (Mod checkedMod in checkedMods)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    mainTabs.IsEnabled = false;
                    menu.IsEnabled = false;
                    refreshButton.IsEnabled = false;
                }));

                SvnClient svnClient = new SvnClient();
                svnClient.Progress += new EventHandler<SvnProgressEventArgs>(svnProgress);
                string remoteLocation = repository + Path.GetDirectoryName(checkedMod.Location);
                string localLocation = Path.GetDirectoryName(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "mods", "tagmods", checkedMod.Location.Replace("/", "\\")));
                
                try
                {
                    deleteDirectory(localLocation);
                } catch
                {
                    // mod doesn't already exist - all fine
                }

                Directory.CreateDirectory(localLocation);

                if (Directory.Exists(Path.Combine(localLocation, ".svn")))
                {
                    svnClient.CleanUp(localLocation);
                }

                try
                {
                    svnClient.CheckOut(new Uri(remoteLocation), localLocation);
                }
                catch { }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                mainTabs.IsEnabled = true;
                menu.IsEnabled = true;
                refreshButton.IsEnabled = true;
                MessageBox.Show("Mods downloaded.", "Foundation Mod Manager");

                foreach (Mod listedMod in downloadableModsList.Items)
                {
                    if (listedMod.IsChecked == true)
                    {
                        listedMod.IsChecked = false;
                    }
                }

                mMods.Clear();
                lookDirectory(Path.Combine(Environment.CurrentDirectory, "mods"));
            }));
        }

        private void svnProgress(object sender, SvnProgressEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                statusBarText.Content = e.Progress;
            }));
        }

        private void deleteDirectory(string path)
        {
            var directory = new DirectoryInfo(path)
            { Attributes = FileAttributes.Normal };
            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }
            directory.Delete(true);
        }
    }
}
