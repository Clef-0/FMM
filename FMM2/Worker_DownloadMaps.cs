//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using IniParser;
//using IniParser.Model;
//using WPF.JoshSmith.ServiceProviders.UI;
//using System.ComponentModel;
//using System.Net;
//using System.Net.Http;
//using Newtonsoft.Json.Linq;
//using SharpSvn;

//namespace FMM2
//{
//    public partial class MainWindow : Window
//    {
//        private void infobarMDLDownload_Click(object sender, RoutedEventArgs e)
//        {
//            if (!workerDownloadMaps.IsBusy)
//            {
//                workerDownloadMaps.RunWorkerAsync();
//            }
//        }

//        private void dlMapWorker(object sender, DoWorkEventArgs e)
//        {
//            List<FMMFile> checkedMaps = new List<FMMFile>();

//            foreach (FMMFile listedMap in downloadableMapsList.Items)
//            {
//                if (listedMap.IsChecked == true)
//                {
//                    checkedMaps.Add(listedMap);
//                }
//            }
//            if (checkedMaps.Count == 0)
//            {
//                return;
//            }

//            foreach (FMMFile checkedMap in checkedMaps)
//            {
//                Dispatcher.BeginInvoke(new Action(() =>
//                {
//                    mainTabs.IsEnabled = false;
//                    menu.IsEnabled = false;
//                }));

//                SvnClient svnClient = new SvnClient();
//                svnClient.Progress += new EventHandler<SvnProgressEventArgs>(svnProgress);
//                string remoteLocation = filerepository + Path.GetDirectoryName(checkedMap.Location);
//                string localLocation = Path.GetDirectoryName(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "mods", "maps", checkedMap.Location.Replace("/", "\\")));

//                try
//                {
//                    deleteDirectory(localLocation);
//                }
//                catch
//                {
//                    // map doesn't already exist - all fine
//                }

//                if (!Directory.Exists(localLocation))
//                {
//                    Directory.CreateDirectory(localLocation);
//                }

//                if (Directory.Exists(Path.Combine(localLocation, ".svn")))
//                {
//                    svnClient.CleanUp(localLocation);
//                }

//                try
//                {
//                    svnClient.CheckOut(new Uri(remoteLocation), localLocation);
//                }
//                catch { }
//            }

//            Dispatcher.BeginInvoke(new Action(() =>
//            {
//                mainTabs.IsEnabled = true;
//                menu.IsEnabled = true;
//                MessageBox.Show(Application.Current.MainWindow, "Maps downloaded.", "Foundation Mod Manager", MessageBoxButton.OK, MessageBoxImage.Information);

//                foreach (FMMFile listedMap in downloadableMapsList.Items)
//                {
//                    if (listedMap.IsChecked == true)
//                    {
//                        listedMap.IsChecked = false;
//                    }
//                }

//                mMaps.Clear();
//                infobarMMScroll.Visibility = Visibility.Collapsed;
//                workerPopulateMyMaps.RunWorkerAsync();
//            }));
//        }
//    }
//}
