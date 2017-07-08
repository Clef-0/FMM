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

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void populateDLModsList(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                dlModsRefreshButton.Content = "Loading...";
                dlModsRefreshButton.IsEnabled = false;
            }));
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp"));
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "links.txt")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "links.txt"));
            }
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://raw.githubusercontent.com/Clef-0/FMM-Mods/master/meta/links.txt", Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "links.txt"));
                }
                catch
                {
                    downloadableModsAlert.Visibility = Visibility.Visible;
                }
            }

            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "links.txt")))
            {
                IEnumerable<string> lines = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", "links.txt"));
                foreach (string modini in lines)
                {
                    taskPopulateDLMods.Add(new Task(() =>
                    {
                        if (modini != "")
                        {
                            processDLModIni(modini);
                        }
                    }));
                }
            }
            Task[] tasks = taskPopulateDLMods.ToArray();
            if (tasks.Length > 0)
            {
                Task.Factory.ContinueWhenAll(tasks, dlModsBitmapsInBackground_Done);
                Array.ForEach(tasks, (t) => t.Start());
            }
        }

        private void processDLModIni(string modini)
        {
            Uri moduri = new Uri(modini);
            using (var client = new System.Net.WebClient())
            {
                try
                {
                    client.DownloadFile(moduri, Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", System.IO.Path.GetFileName(moduri.LocalPath)));
                }
                catch { }
            }
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", System.IO.Path.GetFileName(moduri.LocalPath))))
            {
                Mod newMod = new Mod();
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp", System.IO.Path.GetFileName(moduri.LocalPath)));

                Uri iconUri = null;
                BitmapImage iconBm = new BitmapImage();

                if ((data["FMMInfo"]["Icon"] != "" && Uri.TryCreate(data["FMMInfo"]["Icon"], UriKind.Absolute, out iconUri) && (data["FMMInfo"]["Icon"].EndsWith(".png") || data["FMMInfo"]["Icon"].EndsWith(".jpg") || data["FMMInfo"]["Icon"].EndsWith(".bmp"))))
                {
                    try
                    {
                        var mS = GetStreamFromUrl(iconUri.OriginalString);
                        if (mS != null)
                        {
                            using (WrappingStream wrapper = new WrappingStream(mS))
                            {
                                iconBm.BeginInit();
                                iconBm.DecodePixelWidth = 200;
                                iconBm.CacheOption = BitmapCacheOption.OnLoad;
                                iconBm.StreamSource = wrapper;
                                iconBm.EndInit();
                                iconBm.Freeze();
                            }
                            mS.Dispose();
                        }
                        else
                        {
                            iconBm = null;
                        }
                    }
                    catch
                    {
                        // image probably corrupted or intercepted
                        iconBm = null;
                    }
                }
                else
                {
                    iconBm = null;
                }

                Uri imageUri = null;
                BitmapImage imageBm = new BitmapImage();
                if ((data["FMMInfo"]["ImageFull"] != "" && Uri.TryCreate(data["FMMInfo"]["ImageFull"], UriKind.Absolute, out imageUri) && (data["FMMInfo"]["ImageFull"].EndsWith(".png") || data["FMMInfo"]["ImageFull"].EndsWith(".jpg") || data["FMMInfo"]["ImageFull"].EndsWith(".bmp")))
                    || (data["FMMInfo"]["ImageThumb"] != "" && Uri.TryCreate(data["FMMInfo"]["ImageThumb"], UriKind.Absolute, out imageUri) && (data["FMMInfo"]["ImageThumb"].EndsWith(".png") || data["FMMInfo"]["ImageThumb"].EndsWith(".jpg") || data["FMMInfo"]["ImageThumb"].EndsWith(".bmp"))))
                {
                    try
                    {
                        var mS = GetStreamFromUrl(imageUri.OriginalString);
                        if (mS != null)
                        {
                            using (WrappingStream wrapper = new WrappingStream(mS))
                            {
                                imageBm.BeginInit();
                                imageBm.DecodePixelWidth = 200;
                                imageBm.CacheOption = BitmapCacheOption.OnLoad;
                                imageBm.StreamSource = wrapper;
                                imageBm.EndInit();
                                imageBm.Freeze();
                            }
                            mS.Dispose();
                        }
                        else
                        {
                            imageBm = null;
                        }
                    }
                    catch
                    {
                        // image probably corrupted or intercepted
                        imageBm = null;
                    }
                }
                else
                {
                    imageBm = null;
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    newMod.Name = data["FMMInfo"]["Name"];
                    newMod.Author = data["FMMInfo"]["Author"];
                    newMod.Version = data["FMMInfo"]["Version"];
                    newMod.Desc = data["FMMInfo"]["Desc"];
                    newMod.LongDesc = data["FMMInfo"]["LongDesc"];
                    newMod.Warnings = data["FMMInfo"]["Warnings"];
                    newMod.LongWarnings = data["FMMInfo"]["LongWarnings"];
                    newMod.Url = data["FMMInfo"]["Url"];
                    newMod.ImageFull = data["FMMInfo"]["ImageFull"];
                    newMod.EDVersion = data["FMMInfo"]["EDVersion"];
                    newMod.RevisionDate = data["FMMInfo"]["RevisionDate"];
                    newMod.Credits = data["FMMInfo"]["Credits"];
                    newMod.Required = data["FMMInfo"]["Required"];
                    newMod.Icon = iconBm;
                    newMod.Image = imageBm;
                    newMod.Location = modini.Substring(modini.LastIndexOf("/master/")).Replace("/master/", "");
                    dMods.Add(newMod);
                }));
            }
        }

        private void dlModsBitmapsInBackground_Done(Task[] tasks)
        {
            taskPopulateDLMods.Clear();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                dlModsRefreshButton.Content = "Refresh";
                dlModsRefreshButton.IsEnabled = true;
            }));
        }
    }
}
