using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        //Dictionary<int, string> baseGametypes = new Dictionary<int, string>()
        //{
        //    { 30, "Last Resort" },
        //    { 31, "Icebox" },
        //    { 310, "High Ground" },
        //    { 320, "Guardian" },
        //    { 340, "Valhalla" },
        //    { 380, "Narrows" },
        //    { 390, "The Pit" },
        //    { 400, "Sandtrap" },
        //    { 410, "Standoff" },
        //    { 700, "Reactor" },
        //    { 703, "Edge" },
        //    { 705, "Diamondback" }
        //};

        //private void populateMyGametypesList(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;
        //    if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "mods", "variants")))
        //        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "mods", "variants"));
        //    lookMapsDirectory(Path.Combine(Environment.CurrentDirectory, "mods", "variants"));
        //}

        //private void lookGametypesDirectory(string targetDirectory)
        //{
        //    string[] fileEntries = Directory.GetFiles(targetDirectory);
        //    foreach (string fileName in fileEntries)
        //        lookGametypesFile(fileName);

        //    string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        //    foreach (string subdirectory in subdirectoryEntries)
        //        lookGametypesDirectory(subdirectory);
        //}

        //private async void lookGametypesFile(string path)
        //{
        //    if (Path.GetExtension(path) == ".map" || Path.GetExtension(path) == ".bin" || Path.GetExtension(path) == ".html")
        //    {
        //        if (File.Exists(Path.GetDirectoryName(path) + Path.GetFileName(Path.GetDirectoryName(path)) + ".ini"))
        //        {
        //            if (File.Exists(path.Replace(".ini", ".fm")))
        //            {
        //                BitmapImage bmIcon = new BitmapImage();
        //                BitmapImage bmImage = new BitmapImage();
        //                var parser = new FileIniDataParser();
        //                IniData data = parser.ReadFile(Path.GetDirectoryName(path) + Path.GetFileName(Path.GetDirectoryName(path)) + ".ini");

        //                Uri iconUri = null;
        //                if ((data["FMMInfo"]["Icon"] != "" && Uri.TryCreate(data["FMMInfo"]["Icon"], UriKind.Absolute, out iconUri) && (data["FMMInfo"]["Icon"].EndsWith(".png") || data["FMMInfo"]["Icon"].EndsWith(".jpg") || data["FMMInfo"]["Icon"].EndsWith(".bmp"))))
        //                {
        //                    try
        //                    {
        //                        var mS = GetStreamFromUrl(iconUri.OriginalString);
        //                        if (mS != null)
        //                        {
        //                            using (WrappingStream wrapper = new WrappingStream(mS))
        //                            {
        //                                bmIcon.BeginInit();
        //                                bmIcon.DecodePixelWidth = 200;
        //                                bmIcon.CacheOption = BitmapCacheOption.OnLoad;
        //                                bmIcon.StreamSource = wrapper;
        //                                bmIcon.EndInit();
        //                                bmIcon.Freeze();
        //                            }
        //                            mS.Dispose();
        //                        }
        //                        else
        //                        {
        //                            bmIcon = null;
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        // image probably corrupted or intercepted
        //                        bmIcon = null;
        //                    }
        //                }
        //                else
        //                {
        //                    bmIcon = null;
        //                }

        //                Uri imageUri = null;

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
        //                                bmImage.BeginInit();
        //                                bmImage.DecodePixelWidth = 200;
        //                                bmImage.CacheOption = BitmapCacheOption.OnLoad;
        //                                bmImage.StreamSource = wrapper;
        //                                bmImage.EndInit();
        //                                bmImage.Freeze();
        //                            }
        //                            mS.Dispose();
        //                        }
        //                        else
        //                        {
        //                            bmImage = null;
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        // image probably corrupted or intercepted
        //                        bmImage = null;
        //                    }
        //                }
        //                else
        //                {
        //                    bmImage = null;
        //                }

        //                await Dispatcher.BeginInvoke(new Action(() => {
        //                    FMMFile newMap = new FMMFile();
        //                    newMap.Name = data["FMMInfo"]["Name"];
        //                    newMap.Author = data["FMMInfo"]["Author"];
        //                    newMap.Version = data["FMMInfo"]["Version"];
        //                    newMap.Desc = data["FMMInfo"]["Desc"];
        //                    newMap.LongDesc = data["FMMInfo"]["LongDesc"];
        //                    newMap.Url = data["FMMInfo"]["Url"];
        //                    newMap.ImageFull = data["FMMInfo"]["ImageFull"];
        //                    newMap.EDVersion = data["FMMInfo"]["EDVersion"];
        //                    newMap.RevisionDate = data["FMMInfo"]["RevisionDate"];
        //                    newMap.Credits = data["FMMInfo"]["Credits"];
        //                    newMap.Location = path;

        //                    try
        //                    {
        //                        newMap.Icon = bmIcon;
        //                    }
        //                    catch
        //                    {
        //                        newMap.Icon = null;
        //                    }

        //                    try
        //                    {
        //                        newMap.Image = bmImage;
        //                    }
        //                    catch
        //                    {
        //                        newMap.Image = null;
        //                    }

        //                    mMaps.Add(newMap);
        //                }));
        //            }
        //        }
        //        else
        //        {
        //            string mapName = new string(ReadStringFromFile(path, 0x150, 0x20, Encoding.Unicode).Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray()).Trim();
        //            string mapDesc = new string(ReadStringFromFile(path, 0x170, 0x80, Encoding.ASCII).Where(c => !char.IsControl(c)).ToArray()).Trim();
        //            string mapAuthor = new string(ReadStringFromFile(path, 0x1f0, 0x17, Encoding.ASCII).Where(c => !char.IsControl(c)).ToArray()).Trim();
        //            string mapBaseMap = "";
        //            baseMaps.TryGetValue(LittleEndianByteArrayToInteger(ReadByteArrayFromFile(path, 0x120, 0x4), 0x0), out mapBaseMap);
        //            await Dispatcher.BeginInvoke(new Action(() => {
        //                mMaps.Add(new FMMFile { Name = mapName, Desc = mapDesc, Author = mapAuthor, BaseMap = mapBaseMap, Icon = null, Location = path });
        //            }));
        //        }
        //    }
        //}
    }
}
