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
        private void populateInstallerDLList(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, string> downloadsDictionary = new Dictionary<string, string>();

            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn")))
            {
                Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn"), true);
            }
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn"));
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", "links.txt")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", "links.txt"));
            }
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://raw.githubusercontent.com/Clef-0/FMM-Mods/master/meta/links.txt", Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", "links.txt"));
                }
                catch
                {
                    downloadableModsAlert.Visibility = Visibility.Visible;
                }
            }
            using (var client = new HttpClient())
            {
                try
                {
                    string response = client.GetAsync("https://dev.fractalcore.net/fmm/api/mod/list").Result.Content.ReadAsStringAsync().Result;
                    if ((response.StartsWith("{") && response.EndsWith("}")) || (response.StartsWith("[") && response.EndsWith("]")))
                    {
                        JArray a = JArray.Parse(response);

                        foreach (JObject o in a.Children<JObject>())
                        {
                            downloadsDictionary.Add((string)o.Property("name").Value, (string)o.Property("downloads").Value);
                        }
                    }
                }
                catch { }
            }

            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", "links.txt")))
            {
                IEnumerable<string> lines = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", "links.txt"));
                foreach (string modini in lines)
                {
                    if (modini != "")
                    {
                        Uri moduri = new Uri(modini);
                        using (var client = new System.Net.WebClient())
                        {
                            try
                            {
                                client.DownloadFile(moduri, Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", System.IO.Path.GetFileName(moduri.LocalPath)));
                            }
                            catch { }
                        }
                        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", System.IO.Path.GetFileName(moduri.LocalPath))))
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var parser = new FileIniDataParser();
                                IniData data = parser.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "fmm-svn", System.IO.Path.GetFileName(moduri.LocalPath)));
                                Mod newMod = new Mod();
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
                                string iconUriString = data["FMMInfo"]["Icon"];
                                Uri iconUri = null;
                                Uri.TryCreate(iconUriString, UriKind.Absolute, out iconUri);
                                if (iconUri != null)
                                {
                                    newMod.Icon = new BitmapImage(iconUri);
                                }
                                else
                                {
                                    newMod.Icon = null;
                                }
                                string imageUriString = data["FMMInfo"]["ImageThumb"];
                                Uri imageUri = null;
                                Uri.TryCreate(imageUriString, UriKind.Absolute, out imageUri);
                                if (imageUri != null)
                                {
                                    try
                                    {
                                        newMod.Image = new BitmapImage(imageUri);
                                    }
                                    catch
                                    {
                                        newMod.Image = null;
                                    }
                                }
                                else
                                {
                                    newMod.Image = null;
                                }
                                newMod.Location = modini.Substring(modini.LastIndexOf("/master/")).Replace("/master/", "");
                                dMods.Add(newMod);
                            }));
                        }
                    }
                }
            }
        }
    }
}
