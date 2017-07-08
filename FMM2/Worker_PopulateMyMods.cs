using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
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
        private void populateMyModsList(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                myModsRefreshButton.Content = "Loading...";
                myModsRefreshButton.IsEnabled = false;
            }));
            BackgroundWorker worker = sender as BackgroundWorker;
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "mods")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "mods"));
            lookModsDirectory(Path.Combine(Environment.CurrentDirectory, "mods"));

            Task[] tasks = taskPopulateMyMods.ToArray();
            if (tasks.Length > 0)
            {
                Task.Factory.ContinueWhenAll(tasks, myModsBitmapsInBackground_Done);
                Array.ForEach(tasks, (t) => t.Start());
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

        private void lookModsFile(string path)
        {
            if (Path.GetExtension(path) == ".ini")
            {
                if (File.Exists(path.Replace(".ini", ".fm")))
                {
                    BitmapImage bmIcon = new BitmapImage();
                    BitmapImage bmImage = new BitmapImage();
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile(path);
                    
                    Uri iconUri = null;
                    if ((data["FMMInfo"]["Icon"] != "" && Uri.TryCreate(data["FMMInfo"]["Icon"], UriKind.Absolute, out iconUri) && (data["FMMInfo"]["Icon"].EndsWith(".png") || data["FMMInfo"]["Icon"].EndsWith(".jpg") || data["FMMInfo"]["Icon"].EndsWith(".bmp"))))
                    {
                        try
                        {
                            var mS = GetStreamFromUrl(iconUri.OriginalString);
                            if (mS != null)
                            {
                                using (WrappingStream wrapper = new WrappingStream(mS))
                                {
                                    bmIcon.BeginInit();
                                    bmIcon.DecodePixelWidth = 200;
                                    bmIcon.CacheOption = BitmapCacheOption.OnLoad;
                                    bmIcon.StreamSource = wrapper;
                                    bmIcon.EndInit();
                                    bmIcon.Freeze();
                                }
                                mS.Dispose();
                            }
                            else
                            {
                                bmIcon = null;
                            }
                        }
                        catch
                        {
                            // image probably corrupted or intercepted
                            bmIcon = null;
                        }
                    }
                    else
                    {
                        bmIcon = null;
                    }

                    Uri imageUri = null;
                    
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
                                    bmImage.BeginInit();
                                    bmImage.DecodePixelWidth = 200;
                                    bmImage.CacheOption = BitmapCacheOption.OnLoad;
                                    bmImage.StreamSource = wrapper;
                                    bmImage.EndInit();
                                    bmImage.Freeze();
                                }
                                mS.Dispose();
                            }
                            else
                            {
                                bmImage = null;
                            }
                        }
                        catch
                        {
                            // image probably corrupted or intercepted
                            bmImage = null;
                        }
                    }
                    else
                    {
                        bmImage = null;
                    }

                    Dispatcher.BeginInvoke(new Action(() => {
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
            Dispatcher.BeginInvoke(new Action(() =>
            {
                myModsRefreshButton.Content = "Refresh";
                myModsRefreshButton.IsEnabled = true;
            }));
        }
    }
}
