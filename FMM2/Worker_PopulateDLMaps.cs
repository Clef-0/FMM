using IniParser;
using IniParser.Model;
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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace FMM2
{
    //    public class MapJson
    //    {
    //        public int id { get; set; } = 0;
    //        public int uid { get; set; } = 0;
    //        public string title { get; set; } = "";
    //        public string map { get; set; } = "";
    //        public string gametype { get; set; } = "";
    //        public string creator { get; set; } = "";
    //        public string info { get; set; } = "";
    //        public string url { get; set; } = "";
    //        public string directURL { get; set; } = "";
    //        public int downloads { get; set; } = 0;
    //        public int replies { get; set; } = 0;
    //        public int votes { get; set; } = 0;
    //        public string img { get; set; } = "";
    //        public string thumbnail { get; set; } = "";
    //        public string date { get; set; } = "";
    //        public string updated { get; set; } = "";
    //        public string edited { get; set; } = "";
    //        public string @public { get; set; } = "";
    //        public string submitter { get; set; } = "";
    //        public string cleanInfo { get; set; } = "";
    //        public string mapQuote { get; set; } = "";
    //        public string variantName { get; set; } = "";
    //        public int views { get; set; } = 0;
    //    }

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

        public class WebClientEx : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebClientEx() : this(60000) { }

        public WebClientEx(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }

    private static Stream GetStreamFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

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
                using (var wc = new WebClientEx(10000))
                    imageData = wc.DownloadData(url);
            }
            catch
            {
                return null;
            }


            return new MemoryStream(imageData);
        }

        //        private void populateDLMapsList(object sender, DoWorkEventArgs e)
        //        {
        //            Dispatcher.BeginInvoke(new Action(() =>
        //            {
        //                dlMapsRefreshButton.Content = "Loading...";
        //                dlMapsRefreshButton.IsEnabled = false;
        //                dlGametypesRefreshButton.Content = "Loading...";
        //                dlGametypesRefreshButton.IsEnabled = false;
        //                //todo
        //                //dlMedalsRefreshButton.Content = "Loading...";
        //                //dlMedalsRefreshButton.IsEnabled = false;
        //            }));
        //            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files"));
        //            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", "links.txt")))
        //            {
        //                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", "links.txt"));
        //            }
        //            using (var client = new WebClient())
        //            {
        //                try
        //                {
        //                    client.DownloadFile("https://raw.githubusercontent.com/Clef-0/FMM-Files/master/meta/links.txt", Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", "links.txt"));
        //                }
        //                catch
        //                {
        //                    Dispatcher.BeginInvoke(new Action(() =>
        //                    {
        //                        downloadableMapsAlert.Visibility = Visibility.Visible;
        //                        downloadableGametypesAlert.Visibility = Visibility.Visible;
        //                        //todo
        //                        //downloadableMedalsAlert.Visibility = Visibility.Visible;
        //                    }));
        //                }
        //            }

        //            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", "links.txt")))
        //            {
        //                IEnumerable<string> lines = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", "links.txt"));
        //                foreach (string fileini in lines)
        //                {
        //                    taskPopulateDLFiles.Add(new Task(() =>
        //                    {
        //                        if (fileini != "")
        //                        {
        //                            processDLFilesIni(fileini);
        //                        }
        //                    }));
        //                }
        //            }
        //            Task[] tasks = taskPopulateDLFiles.ToArray();
        //            if (tasks.Length > 0)
        //            {
        //                Task.Factory.ContinueWhenAll(tasks, dlFilesBitmapsInBackground_Done);
        //                Array.ForEach(tasks, (t) => t.Start());
        //            }
        //        }

        //        private void processDLFilesIni(string fileini)
        //        {
        //            string type = "";
        //            if (fileini.StartsWith("map: "))
        //            {
        //                type = "map";
        //                fileini = fileini.Replace("map: ","");
        //            }
        //            if (fileini.StartsWith("gametype: "))
        //            {
        //                type = "gametype";
        //                fileini = fileini.Replace("gametype: ", "");
        //            }
        //            if (fileini.StartsWith("medals: "))
        //            {
        //                type = "medals";
        //                fileini = fileini.Replace("medals: ", "");
        //            }

        //            Uri fileuri = new Uri(fileini);
        //            using (var client = new System.Net.WebClient())
        //            {
        //                try
        //                {
        //                    client.DownloadFile(fileuri, Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", System.IO.Path.GetFileName(fileuri.LocalPath)));
        //                }
        //                catch { }
        //            }
        //            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", System.IO.Path.GetFileName(fileuri.LocalPath))))
        //            {
        //                FMMFile newFile = new FMMFile();
        //                var parser = new FileIniDataParser();
        //                IniData data = parser.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "files", System.IO.Path.GetFileName(fileuri.LocalPath)));

        //                Uri iconUri = null;
        //                BitmapImage iconBm = new BitmapImage();

        //                if ((data["FMMInfo"]["Icon"] != "" && Uri.TryCreate(data["FMMInfo"]["Icon"], UriKind.Absolute, out iconUri) && (data["FMMInfo"]["Icon"].EndsWith(".png") || data["FMMInfo"]["Icon"].EndsWith(".jpg") || data["FMMInfo"]["Icon"].EndsWith(".bmp"))))
        //                {
        //                    try
        //                    {
        //                        var mS = GetStreamFromUrl(iconUri.OriginalString);
        //                        if (mS != null)
        //                        {
        //                            using (WrappingStream wrapper = new WrappingStream(mS))
        //                            {
        //                                iconBm.BeginInit();
        //                                iconBm.DecodePixelWidth = 200;
        //                                iconBm.CacheOption = BitmapCacheOption.OnLoad;
        //                                iconBm.StreamSource = wrapper;
        //                                iconBm.EndInit();
        //                                iconBm.Freeze();
        //                            }
        //                            mS.Dispose();
        //                        }
        //                        else
        //                        {
        //                            iconBm = null;
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        // image probably corrupted or intercepted
        //                        iconBm = null;
        //                    }
        //                }
        //                else
        //                {
        //                    iconBm = null;
        //                }

        //                Uri imageUri = null;
        //                BitmapImage imageBm = new BitmapImage();
        //                if ((data["FMMInfo"]["ImageFull"] != "" && Uri.TryCreate(data["FMMInfo"]["ImageFull"], UriKind.Absolute, out imageUri) && (data["FMMInfo"]["ImageFull"].EndsWith(".png") || data["FMMInfo"]["ImageFull"].EndsWith(".jpg") || data["FMMInfo"]["ImageFull"].EndsWith(".bmp")))
        //                    || (data["FMMInfo"]["ImageThumb"] != "" && Uri.TryCreate(data["FMMInfo"]["ImageThumb"], UriKind.Absolute, out imageUri) && (data["FMMInfo"]["ImageThumb"].EndsWith(".png") || data["FMMInfo"]["ImageThumb"].EndsWith(".jpg") || data["FMMInfo"]["ImageThumb"].EndsWith(".bmp"))))
        //                {
        //                    try
        //                    {
        //                        var mS = GetStreamFromUrl(imageUri.OriginalString);
        //                        if (mS != null)
        //                        {
        //                            using (WrappingStream wrapper = new WrappingStream(mS))
        //                            {
        //                                imageBm.BeginInit();
        //                                imageBm.DecodePixelWidth = 200;
        //                                imageBm.CacheOption = BitmapCacheOption.OnLoad;
        //                                imageBm.StreamSource = wrapper;
        //                                imageBm.EndInit();
        //                                imageBm.Freeze();
        //                            }
        //                            mS.Dispose();
        //                        }
        //                        else
        //                        {
        //                            imageBm = null;
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        // image probably corrupted or intercepted
        //                        imageBm = null;
        //                    }
        //                }
        //                else
        //                {
        //                    imageBm = null;
        //                }

        //                Dispatcher.BeginInvoke(new Action(() =>
        //                {
        //                    newFile.Name = data["FMMInfo"]["Name"];
        //                    newFile.Author = data["FMMInfo"]["Author"];
        //                    newFile.Version = data["FMMInfo"]["Version"];
        //                    newFile.Desc = data["FMMInfo"]["Desc"];
        //                    newFile.LongDesc = data["FMMInfo"]["LongDesc"];
        //                    newFile.Url = data["FMMInfo"]["Url"];
        //                    newFile.ImageFull = data["FMMInfo"]["ImageFull"];
        //                    newFile.EDVersion = data["FMMInfo"]["EDVersion"];
        //                    newFile.RevisionDate = data["FMMInfo"]["RevisionDate"];
        //                    newFile.Credits = data["FMMInfo"]["Credits"];
        //                    newFile.Icon = iconBm;
        //                    newFile.Image = imageBm;
        //                    newFile.Location = fileini.Substring(fileini.LastIndexOf("/master/")).Replace("/master/", "");
        //                    if (type == "map")
        //                    {
        //                        dMaps.Add(newFile);
        //                    }
        //                    if (type == "gametype")
        //                    {
        //                        dGametypes.Add(newFile);
        //                    }
        //                    if (type == "medals")
        //                    {
        //                        dMedals.Add(newFile);
        //                    }
        //                }));
        //            }
        //        }

        //        private void bitmapInBackground_DoWork(MapJson mapJson)
        //        {
        //            Uri imageUri = null;
        //            BitmapImage bmi = new BitmapImage();

        //            if ((mapJson.img != "" && Uri.TryCreate(mapJson.img, UriKind.Absolute, out imageUri) && (mapJson.img.EndsWith(".png") || mapJson.img.EndsWith(".jpg") || mapJson.img.EndsWith(".bmp")))
        //                || (mapJson.thumbnail != "" && Uri.TryCreate(mapJson.thumbnail, UriKind.Absolute, out imageUri) && (mapJson.thumbnail.EndsWith(".png") || mapJson.thumbnail.EndsWith(".jpg") || mapJson.thumbnail.EndsWith(".bmp"))))
        //            {
        //                try
        //                {
        //                    var mS = GetStreamFromUrl(imageUri.OriginalString);
        //                    if (mS != null)
        //                    {
        //                        using (WrappingStream wrapper = new WrappingStream(mS))
        //                        {
        //                            bmi.BeginInit();
        //                            bmi.DecodePixelWidth = 200;
        //                            bmi.CacheOption = BitmapCacheOption.OnLoad;
        //                            bmi.StreamSource = wrapper;
        //                            bmi.EndInit();
        //                            bmi.Freeze();
        //                        }
        //                        mS.Dispose();
        //                    }
        //                    else
        //                    {
        //                        bmi = null;
        //                    }
        //                }
        //                catch
        //                {
        //                    // image probably corrupted or intercepted
        //                    bmi = null;
        //                }
        //            }
        //            else
        //            {
        //                bmi = null;
        //            }

        //            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
        //            {
        //                FMMFile newMap = new FMMFile();
        //                newMap.Name = mapJson.title;
        //                newMap.BaseMap = mapJson.map;
        //                newMap.Desc = mapJson.cleanInfo.Replace("<br />", "").Trim();
        //                newMap.Author = mapJson.creator;
        //                newMap.Location = mapJson.directURL;
        //                newMap.DLs = mapJson.downloads;
        //                newMap.Icon = null;
        //                if (!string.IsNullOrEmpty(mapJson.url))
        //                {
        //                    if (mapJson.url.StartsWith("//"))
        //                    {
        //                        newMap.Url = "http:" + mapJson.url;
        //                    }
        //                    else
        //                    {
        //                        newMap.Url = mapJson.url;
        //                    }
        //                }

        //                string[] updatedDateTime = mapJson.edited.Split(' ');
        //                string[] updatedDate = updatedDateTime[0].Split('-');
        //                if (updatedDate.Count() == 3)
        //                {
        //                    string year = updatedDate[0];
        //                    int monthint = 0;
        //                    Int32.TryParse(updatedDate[1], out monthint);
        //                    string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthint);
        //                    int dayint = 0;
        //                    Int32.TryParse(updatedDate[2], out dayint);
        //                    string day = AddOrdinal(dayint);
        //                    newMap.RevisionDate = month + " " + day + ", " + year;
        //                }
        //                else
        //                {
        //                    updatedDateTime = mapJson.date.Split(' ');
        //                    updatedDate = updatedDateTime[0].Split('-');
        //                    if (updatedDate.Count() == 3)
        //                    {
        //                        string year = updatedDate[0];
        //                        int monthint = 0;
        //                        Int32.TryParse(updatedDate[1], out monthint);
        //                        string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthint);
        //                        int dayint = 0;
        //                        Int32.TryParse(updatedDate[2], out dayint);
        //                        string day = AddOrdinal(dayint);
        //                        newMap.RevisionDate = month + " " + day + ", " + year;
        //                    }
        //                    else
        //                    {
        //                        newMap.RevisionDate = null;
        //                    }
        //                }

        //                try
        //                {
        //                    newMap.Image = bmi;
        //                }
        //                catch
        //                {
        //                    newMap.Image = null;
        //                }

        //                dMaps.Add(newMap);
        //            });
        //        }

        //        private void dlFilesBitmapsInBackground_Done(Task[] tasks)
        //        {
        //            taskPopulateDLFiles.Clear();
        //            Dispatcher.BeginInvoke(new Action(() =>
        //            {
        //                dlMapsRefreshButton.Content = "Refresh";
        //                dlMapsRefreshButton.IsEnabled = true;
        //                dlGametypesRefreshButton.Content = "Refresh";
        //                dlGametypesRefreshButton.IsEnabled = true;
        //                //todo
        //                //dlMedalsRefreshButton.Content = "Refresh";
        //                //dlMedalsRefreshButton.IsEnabled = true;
        //            }));
        //        }
    }
}
