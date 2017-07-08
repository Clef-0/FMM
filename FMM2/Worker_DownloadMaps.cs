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
        private void infobarMDLDownload_Click(object sender, RoutedEventArgs e)
        {
            if (!workerDownloadMaps.IsBusy)
            {
                workerDownloadMaps.RunWorkerAsync();
            }
        }

        private void dlMapWorker(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                mainTabs.IsEnabled = false;
                menu.IsEnabled = false;
                myModsRefreshButton.IsEnabled = false;
                dlModsRefreshButton.IsEnabled = false;
                myMapsRefreshButton.IsEnabled = false;
                dlMapsRefreshButton.IsEnabled = false;
                serversRefreshButton.IsEnabled = false;
            }));

            List<Map> checkedMaps = new List<Map>();

            foreach (Map listedMap in downloadableMapsList.Items)
            {
                if (listedMap.IsChecked == true)
                {
                    checkedMaps.Add(listedMap);
                }
            }
            if (checkedMaps.Count == 0)
            {
                return;
            }

            foreach (Map checkedMap in checkedMaps)
            {
                string remoteLocation = checkedMap.Location;
                string localLocation = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "mods", "maps", new string(checkedMap.Name.Replace(" ","").Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray()).Trim());

                if (remoteLocation.StartsWith("//"))
                {
                    remoteLocation = "http:" + remoteLocation;
                }

                Directory.CreateDirectory(localLocation);

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(remoteLocation, Path.Combine(localLocation, "sandbox.map"));
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                mainTabs.IsEnabled = true;
                menu.IsEnabled = true;
                myModsRefreshButton.IsEnabled = true;
                dlModsRefreshButton.IsEnabled = true;
                myMapsRefreshButton.IsEnabled = true;
                dlMapsRefreshButton.IsEnabled = true;
                serversRefreshButton.IsEnabled = true;
                MessageBox.Show(Application.Current.MainWindow, "Maps downloaded.", "Foundation Mod Manager", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (Map listedMap in downloadableMapsList.Items)
                {
                    if (listedMap.IsChecked == true)
                    {
                        listedMap.IsChecked = false;
                    }
                }

                mMaps.Clear();
                infobarScroll.Visibility = Visibility.Collapsed;
                if (!workerPopulateMyMaps.IsBusy)
                {
                    workerPopulateMyMaps.RunWorkerAsync();
                }
            }));
        }
    }
}
