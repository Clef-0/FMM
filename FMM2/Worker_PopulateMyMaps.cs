using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        Dictionary<int, string> baseMaps = new Dictionary<int, string>()
        {
            { 30, "Last Resort" },
            { 31, "Icebox" },
            { 310, "High Ground" },
            { 320, "Guardian" },
            { 340, "Valhalla" },
            { 380, "Narrows" },
            { 390, "The Pit" },
            { 400, "Sandtrap" },
            { 410, "Standoff" },
            { 700, "Reactor" },
            { 703, "Edge" },
            { 705, "Diamondback" }
        };

        private void populateMyMapsList(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "mods", "maps")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "mods", "maps"));
            lookMapsDirectory(Path.Combine(Environment.CurrentDirectory, "mods", "maps"));
        }

        private void lookMapsDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                lookMapsFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                lookMapsDirectory(subdirectory);
        }

        private async void lookMapsFile(string path)
        {
            if (Path.GetExtension(path) == ".map" || Path.GetExtension(path) == ".bin")
            {
                string mapName = new string(ReadStringFromFile(path, 0x150, 0x20, Encoding.Unicode).Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray()).Trim();
                string mapDesc = new string(ReadStringFromFile(path, 0x170, 0x80, Encoding.ASCII).Where(c => !char.IsControl(c)).ToArray()).Trim();
                string mapAuthor = new string(ReadStringFromFile(path, 0x1f0, 0x17, Encoding.ASCII).Where(c => !char.IsControl(c)).ToArray()).Trim();
                string mapBaseMap = "";
                baseMaps.TryGetValue(LittleEndianByteArrayToInteger(ReadByteArrayFromFile(path, 0x120, 0x4), 0x0), out mapBaseMap);
                await Dispatcher.BeginInvoke(new Action(() => {
                    mMaps.Add(new Map { Name = mapName, Desc = mapDesc, Author = mapAuthor, BaseMap = mapBaseMap, Icon = null, Location = path });
                }));
            }
        }

        private byte[] ReadByteArrayFromFile(string mapFile, int offset, int length)
        {
            byte[] data = new byte[length];
            int actualRead;

            try
            {
                using (FileStream fs = new FileStream(mapFile, FileMode.Open))
                {
                    fs.Position = offset;
                    actualRead = 0;
                    do
                    {
                        actualRead += fs.Read(data, actualRead, length - actualRead);
                    } while (actualRead != length && fs.Position < fs.Length);
                    fs.Close();
                }
            }
            catch
            {

            }

            return data;
        }

        int LittleEndianByteArrayToInteger(byte[] data, int startIndex)
        {
            return (data[startIndex + 3] << 24) | (data[startIndex + 2] << 16) | (data[startIndex + 1] << 8) | data[startIndex];
        }

        private string ReadStringFromFile(string mapFile, int offset, int length, Encoding encoding)
        {
            var data = new byte[length];
            int actualRead;

            try
            {
                using (FileStream fs = new FileStream(mapFile, FileMode.Open))
            {
                fs.Position = offset;
                actualRead = 0;
                do
                {
                    actualRead += fs.Read(data, actualRead, length - actualRead);
                } while (actualRead != length && fs.Position < fs.Length);
                fs.Close();
                }
            }
            catch
            {

            }

            return encoding.GetString(data);
        }
    }
}
