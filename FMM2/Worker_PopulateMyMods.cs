using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        string loadingString = "Loading...";
        string refreshString = "Refresh";

        private void populateMyModsList(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                myModsRefreshButton.Content = loadingString;
                myModsRefreshButton.IsEnabled = false;
            }));
            BackgroundWorker worker = sender as BackgroundWorker;
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "mods", "tagmods")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "mods", "tagmods"));
            lookModsDirectory(Path.Combine(Environment.CurrentDirectory, "mods", "tagmods"));

            Task[] tasks = taskPopulateMyMods.ToArray();
            if (tasks.Length > 0)
            {
                Task.Factory.ContinueWhenAll(tasks, myModsBitmapsInBackground_Done);
                Array.ForEach(tasks, (t) => t.Start());
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    myModsRefreshButton.Content = refreshString;
                    myModsRefreshButton.IsEnabled = true;
                }));
            }
        }
        
        private void lookModsDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                taskPopulateMyMods.Add(new Task(() => { lookModsFile(fileName); }));

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                lookModsDirectory(subdirectory);
            
        }

        private static bool IsLocalPath(string p)
        {
            return new Uri(p).IsFile;
        }

        private void lookModsFile(string path)
        {
            if (Path.GetExtension(path) == ".ini")
            {
                if (File.Exists(path.Replace(".ini", ".fm")))
                {
                    BitmapImage bmIcon = null;
                    BitmapImage bmImage = null;
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile(path);

                    string iconstring = data["FMMInfo"]["Icon"];
                    string imagefullstring = data["FMMInfo"]["ImageFull"];
                    string imagethumbstring = data["FMMInfo"]["ImageThumb"];

                    //icon loads
                    Uri iconUri = null;

                    if (!string.IsNullOrEmpty(iconstring) && !iconstring.Contains("/"))
                    {
                        if (File.Exists(Path.Combine(Directory.GetParent(path).FullName, iconstring)))
                        {
                            try
                            {
                                var mS = File.OpenRead(Path.Combine(Directory.GetParent(path).FullName, iconstring));
                            if (mS != null)
                            {
                                using (WrappingStream wrapper = new WrappingStream(mS))
                                {
                                    bmIcon = new BitmapImage();
                                    bmIcon.BeginInit();
                                    bmIcon.DecodePixelWidth = 200;
                                    bmIcon.CacheOption = BitmapCacheOption.OnLoad;
                                    bmIcon.StreamSource = wrapper;
                                    bmIcon.EndInit();
                                    bmIcon.Freeze();
                                }
                                mS.Dispose();
                                mS.Close();
                                }
                            }
                            catch
                            {
                                bmIcon = null;
                            }
                        }
                    }
                    else if (!offlineMode && !string.IsNullOrEmpty(iconstring) && Uri.TryCreate(iconstring, UriKind.Absolute, out iconUri))
                    {
                        try
                        {
                            var mS = GetStreamFromUrl(iconstring);
                            if (mS != null)
                            {
                                using (WrappingStream wrapper = new WrappingStream(mS))
                                {
                                    bmIcon = new BitmapImage();
                                    bmIcon.BeginInit();
                                    bmIcon.DecodePixelWidth = 200;
                                    bmIcon.CacheOption = BitmapCacheOption.OnLoad;
                                    bmIcon.StreamSource = wrapper;
                                    bmIcon.EndInit();
                                    bmIcon.Freeze();
                                }
                                mS.Dispose();
                            }
                        }
                        catch
                        {
                            bmIcon = null;
                        }
                    }

                    //image loads
                    Uri imageUri = null;

                    if (!string.IsNullOrEmpty(imagefullstring) && !imagefullstring.Contains("/"))
                    {
                        if (File.Exists(Path.Combine(Directory.GetParent(path).FullName, imagefullstring)))
                        {
                            try
                            {
                                var mS = File.OpenRead(Path.Combine(Directory.GetParent(path).FullName, imagefullstring));
                                if (mS != null)
                                {
                                    using (WrappingStream wrapper = new WrappingStream(mS))
                                    {
                                        bmImage = new BitmapImage();
                                        bmImage.BeginInit();
                                        bmImage.DecodePixelWidth = 200;
                                        bmImage.CacheOption = BitmapCacheOption.OnLoad;
                                        bmImage.StreamSource = wrapper;
                                        bmImage.EndInit();
                                        bmImage.Freeze();
                                    }
                                    mS.Dispose();
                                    mS.Close();
                                }
                            }
                            catch
                            {
                                bmImage = null;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(imagethumbstring) && !imagethumbstring.Contains("/"))
                    {
                        if (File.Exists(Path.Combine(Directory.GetParent(path).FullName, imagethumbstring)))
                        {
                            try
                            {
                                var mS = File.OpenRead(Path.Combine(Directory.GetParent(path).FullName, imagethumbstring));
                                if (mS != null)
                                {
                                    using (WrappingStream wrapper = new WrappingStream(mS))
                                    {
                                        bmImage = new BitmapImage();
                                        bmImage.BeginInit();
                                        bmImage.DecodePixelWidth = 200;
                                        bmImage.CacheOption = BitmapCacheOption.OnLoad;
                                        bmImage.StreamSource = wrapper;
                                        bmImage.EndInit();
                                        bmImage.Freeze();
                                    }
                                    mS.Dispose();
                                    mS.Close();
                                }
                            }
                            catch
                            {
                                bmImage = null;
                            }
                        }
                    }
                    else if (!offlineMode && !string.IsNullOrEmpty(imagethumbstring) && Uri.TryCreate(imagethumbstring, UriKind.Absolute, out imageUri) && (imagethumbstring.EndsWith(".png") || imagethumbstring.EndsWith(".jpg") || imagethumbstring.EndsWith(".bmp")))
                    {
                        try
                        {
                            var mS = GetStreamFromUrl(imagethumbstring);
                            if (mS != null)
                            {
                                using (WrappingStream wrapper = new WrappingStream(mS))
                                {
                                    bmImage = new BitmapImage();
                                    bmImage.BeginInit();
                                    bmImage.DecodePixelWidth = 200;
                                    bmImage.CacheOption = BitmapCacheOption.OnLoad;
                                    bmImage.StreamSource = wrapper;
                                    bmImage.EndInit();
                                    bmImage.Freeze();
                                }
                                mS.Dispose();
                            }
                        }
                        catch
                        {
                            bmImage = null;
                        }
                    }


                    Dispatcher.Invoke(new Action(() => {
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
                        newMod.Location = path.Replace(".ini", ".fm");

                        try
                        {
                            newMod.Icon = bmIcon;
                        }
                        catch
                        {
                            newMod.Icon = null;
                        }

                        try
                        {
                            newMod.Image = bmImage;
                        }
                        catch
                        {
                            newMod.Image = null;
                        }

                        mMods.Add(newMod);
                    }));
                }
            }
        }
        private void myModsBitmapsInBackground_Done(Task[] tasks)
        {
            taskPopulateMyMods.Clear();
            checkFMMInstallerOrder();
            Dispatcher.Invoke(new Action(() =>
            {
                myModsRefreshButton.Content = refreshString;
                myModsRefreshButton.IsEnabled = true;
            }));
        }
        private void checkFMMInstallerOrder()
        {
            string fmmdat = Path.Combine(Directory.GetCurrentDirectory(), "fmm.dat");
            if (File.Exists(fmmdat))
            {
                IEnumerable<string> lines = File.ReadLines(fmmdat);
                IEnumerable<string> linesFor;
                if (installListOrder)
                {
                    linesFor = lines.Reverse();
                }
                else
                {
                    linesFor = lines;
                }
                foreach (string modName in linesFor)
                {
                    foreach (Mod item in mMods.ToList())
                    {
                        if (item.Name == modName)
                        {
                            try
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    mMods.Remove(item);
                                    mMods.Insert(0, item);
                                    item.IsChecked = true;
                                }));
                            }
                            catch { }
                        }
                    }
                }
            }
        }
    }
}
