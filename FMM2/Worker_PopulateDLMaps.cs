using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace FMM2
{
    public class MapJson
    {
        public int id { get; set; } = 0;
        public int uid { get; set; } = 0;
        public string title { get; set; } = "";
        public string map { get; set; } = "";
        public string gametype { get; set; } = "";
        public string creator { get; set; } = "";
        public string info { get; set; } = "";
        public string url { get; set; } = "";
        public string directURL { get; set; } = "";
        public int downloads { get; set; } = 0;
        public int replies { get; set; } = 0;
        public int votes { get; set; } = 0;
        public string img { get; set; } = "";
        public string thumbnail { get; set; } = "";
        public string date { get; set; } = "";
        public string updated { get; set; } = "";
        public string edited { get; set; } = "";
        public string @public { get; set; } = "";
        public string submitter { get; set; } = "";
        public string cleanInfo { get; set; } = "";
        public string mapQuote { get; set; } = "";
        public string variantName { get; set; } = "";
        public int views { get; set; } = 0;
    }

    public partial class MainWindow : Window
    {
        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        private static Stream GetStreamFromUrl(string url)
        {
            if (url.StartsWith("//"))
            {
                url = "http:" + url;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return new MemoryStream();
            }

            byte[] imageData = null;
            
            try
            {
                using (var wc = new WebClient())
                    imageData = wc.DownloadData(url);
            }
            catch
            {
                return null;
            }
            

            return new MemoryStream(imageData);
        }

        private void populateDLMapsList(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                dlMapsRefreshButton.Content = "Loading...";
                dlMapsRefreshButton.IsEnabled = false;
            }));
            using (var wc = new WebClient())
            {
                wc.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
                if (repositoryConduit)
                {
                    wc.DownloadStringAsync(new Uri("http://halovau.lt/inc/api/listMaps_7-6-17.json"));
                    //wc.DownloadStringAsync(new Uri("http://halovau.lt/inc/api/listMaps.api?key=ab26e50f274d6ab6122305659c938d99"));
                }
                else
                {
                    //TODO
                }
            }
        }
        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string stringJson = e.Result;

            ObservableCollection<MapJson> oCMJ;
            if (stringJson != string.Empty)
            {
                try
                {
                    oCMJ = JsonConvert.DeserializeObject<ObservableCollection<MapJson>>(stringJson);

                    foreach (MapJson mapJson in oCMJ)
                    {
                        if (mapJson.@public == "y")
                        {
                            taskPopulateDLMaps.Add(new Task(() =>
                            {
                                bitmapInBackground_DoWork(mapJson);
                            }));
                        }
                    }
                    Task[] tasks = taskPopulateDLMaps.ToArray();
                    if (tasks.Length > 0)
                    {
                        Task.Factory.ContinueWhenAll(tasks, bitmapsInBackground_Done);
                        Array.ForEach(tasks, (t) => t.Start());
                    }
                }
                catch
                {
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        downloadableMapsAlert.Visibility = Visibility.Visible;
                    });
                }
            }
            else
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    downloadableMapsAlert.Visibility = Visibility.Visible;
                });
            }
        }
        private void bitmapInBackground_DoWork(MapJson mapJson)
        {
            Uri imageUri = null;
            BitmapImage bmi = new BitmapImage();

            if (mapJson.thumbnail != "" && Uri.TryCreate(mapJson.thumbnail, UriKind.Absolute, out imageUri) && (mapJson.img.EndsWith(".png") || mapJson.img.EndsWith(".jpg") || mapJson.img.EndsWith(".bmp")))
            {
                try
                {
                    var mS = GetStreamFromUrl(imageUri.OriginalString);
                    if (mS != null)
                    {
                        using (WrappingStream wrapper = new WrappingStream(mS))
                        {
                            bmi.BeginInit();
                            bmi.DecodePixelWidth = 200;
                            bmi.CacheOption = BitmapCacheOption.OnLoad;
                            bmi.StreamSource = wrapper;
                            bmi.EndInit();
                            bmi.Freeze();
                        }
                        mS.Dispose();
                    }
                    else
                    {
                        bmi = null;
                    }
                }
                catch
                {
                    bmi = null;
                }
            }
            else if (mapJson.img != "" && Uri.TryCreate(mapJson.img, UriKind.Absolute, out imageUri) && (mapJson.img.EndsWith(".png") || mapJson.img.EndsWith(".jpg") || mapJson.img.EndsWith(".bmp")))
            {
                using (WrappingStream wrapper = new WrappingStream(GetStreamFromUrl(imageUri.OriginalString)))
                {
                    try
                    {
                        bmi.BeginInit();
                        bmi.DecodePixelWidth = 200;
                        bmi.CacheOption = BitmapCacheOption.OnLoad;
                        bmi.StreamSource = wrapper;
                        bmi.EndInit();
                        bmi.Freeze();
                    }
                    catch
                    {
                        // image probably corrupted or intercepted
                        bmi = null;
                    }
                }
            }
            else
            {
                bmi = null;
            }
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Map newMap = new Map();
                newMap.Name = mapJson.title;
                newMap.BaseMap = mapJson.map;
                newMap.Desc = mapJson.cleanInfo.Replace("<br />", "").Trim();
                newMap.Author = mapJson.creator;
                newMap.Submitter = mapJson.submitter;
                newMap.Location = mapJson.directURL;
                newMap.DLs = mapJson.downloads;
                newMap.Icon = null;
                try
                {
                    string[] updatedDateTime = mapJson.edited.Split(' ');
                    string[] updatedDate = updatedDateTime[0].Split('-');
                    if (updatedDate.Count() == 3)
                    {
                        string year = updatedDate[0];
                        int monthint = 0;
                        Int32.TryParse(updatedDate[1], out monthint);
                        string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthint);
                        int dayint = 0;
                        Int32.TryParse(updatedDate[2], out dayint);
                        string day = AddOrdinal(dayint);
                        newMap.RevisionDate = month + " " + day + ", " + year;
                    }
                    else
                    {
                        newMap.RevisionDate = null;
                    }
                }
                catch
                {
                    try
                    {
                        string[] updatedDateTime = mapJson.date.Split(' ');
                        string[] updatedDate = updatedDateTime[0].Split('-');
                        if (updatedDate.Count() == 3)
                        {
                            string year = updatedDate[0];
                        int monthint = 0;
                        Int32.TryParse(updatedDate[1], out monthint);
                        string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthint);
                        int dayint = 0;
                        Int32.TryParse(updatedDate[2], out dayint);
                        string day = AddOrdinal(dayint);
                        newMap.RevisionDate = month + " " + day + ", " + year;
                    }
                    else
                    {
                        newMap.RevisionDate = null;
                    }
                }
                    catch
                    {
                        newMap.RevisionDate = null;
                    }
                }

                try
                {
                    newMap.Image = bmi;
                }
                catch
                {
                    newMap.Image = null;
                }
                dMaps.Add(newMap);
            });
        }
        private void bitmapsInBackground_Done(Task[] tasks)
        {
            taskPopulateDLMaps.Clear();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                dlMapsRefreshButton.Content = "Refresh";
                dlMapsRefreshButton.IsEnabled = true;
            }));
        }
    }
}
