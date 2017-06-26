using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void fileExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void optionsOrderList_Click(object sender, RoutedEventArgs e)
        {
            if (installListOrder == false)
            {
                List<Mod> checkedMods = new List<Mod>();
                foreach (Mod item in mMods)
                {
                    if (item.IsChecked)
                    {
                        checkedMods.Add(item);
                    }
                }
                
                foreach (Mod item in checkedMods)
                {
                    Mod itemToMoveUp = mMods[mMods.IndexOf(item)];
                    mMods.RemoveAt(mMods.IndexOf(item));
                    mMods.Insert(0, itemToMoveUp);
                }
            }

            optionsOrderList.IsChecked = true;
            optionsOrderPriority.IsChecked = false;
            installListOrder = true;
            writeFMMIni("Order", "List");
        }

        private void optionsOrderPriority_Click(object sender, RoutedEventArgs e)
        {
            if (installListOrder == true)
            {
                List<Mod> checkedMods = new List<Mod>();
                foreach (Mod item in mMods)
                {
                    if (item.IsChecked)
                    {
                        checkedMods.Add(item);
                    }
                }
                
                foreach (Mod item in checkedMods)
                {
                    Mod itemToMoveUp = mMods[mMods.IndexOf(item)];
                    mMods.RemoveAt(mMods.IndexOf(item));
                    mMods.Insert(0, itemToMoveUp);
                }
            }

            optionsOrderList.IsChecked = false;
            optionsOrderPriority.IsChecked = true;
            installListOrder = false;
            writeFMMIni("Order", "Priority");
        }
        private void writeFMMIni(string property, string value)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("FMM.ini");
            data["FMMPrefs"][property] = value;
            parser.WriteFile("FMM.ini", data);
        }

        private void optionsOffline_Click(object sender, RoutedEventArgs e)
        {
            if (offlineMode)
            {
                offlineMode = false;
                writeFMMIni("OfflineMode", "false");
            }
            else
            {
                offlineMode = true;
                writeFMMIni("OfflineMode", "true");
            }
        }

        private void optionsDeveloper_Click(object sender, RoutedEventArgs e)
        {
            if (developerMode)
            {
                developerMode = false;
                writeFMMIni("DeveloperMode", "false");
            }
            else
            {
                offlineMode = true;
                writeFMMIni("DeveloperMode", "true");
            }
        }

        private string readFMMIni(string property)
        {
            var parser = new FileIniDataParser();
            string value;
            try
            {
                IniData data = parser.ReadFile("FMM.ini");
                value = data["FMMPrefs"][property];
            }
            catch
            {
                value = "";
            }
            if (value == null)
            {
                value = "";
            }
            return value;
        }

        private void loadFMMSettings()
        {
            switch (readFMMIni("OfflineMode").ToLowerInvariant())
            {
                case "":
                    writeFMMIni("OfflineMode", "false");
                    optionsOffline.IsChecked = false;
                    offlineMode = false;
                    break;
                case "false":
                    optionsOffline.IsChecked = false;
                    offlineMode = false;
                    break;
                case "true":
                    optionsOffline.IsChecked = true;
                    offlineMode = true;
                    break;
            }
            switch (readFMMIni("DeveloperMode").ToLower())
            {
                case "":
                    writeFMMIni("DeveloperMode", "false");
                    optionsDeveloper.IsChecked = false;
                    developerMode = false;
                    break;
                case "false":
                    optionsDeveloper.IsChecked = false;
                    developerMode = false;
                    break;
                case "true":
                    optionsDeveloper.IsChecked = true;
                    developerMode = true;
                    break;
            }
            switch (readFMMIni("Order").ToLower())
            {
                case "":
                    writeFMMIni("Order", "List");
                    optionsOrderList.IsChecked = true;
                    optionsOrderPriority.IsChecked = false;
                    installListOrder = true;
                    break;
                case "list":
                    optionsOrderList.IsChecked = true;
                    optionsOrderPriority.IsChecked = false;
                    installListOrder = true;
                    break;
                case "priority":
                    optionsOrderList.IsChecked = false;
                    optionsOrderPriority.IsChecked = true;
                    installListOrder = false;
                    break;
            }
        }
    }
}
