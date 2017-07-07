using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
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
        private void lookModsDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                lookModsFile(fileName);

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
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile(path);
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
                    newMod.Location = path.Replace(".ini", ".fm");

                    mMods.Add(newMod);
                }
            }
        }
    }
}
