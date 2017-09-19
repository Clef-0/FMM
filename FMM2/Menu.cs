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
            try
            {
                try
                {
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile("FMM.ini", Encoding.Unicode);
                    data["FMMPrefs"][property] = value;
                    parser.WriteFile("FMM.ini", data, Encoding.Unicode);
                }
                catch
                {
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile("FMM.ini");
                    data["FMMPrefs"][property] = value;
                    parser.WriteFile("FMM.ini", data);
                }
            }
            catch
            {
            }
        }
        private void writeFMMLang(string property, string value)
        {
            try
            {
                try
                {
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile("FMM_Lang.ini", Encoding.Unicode);
                    data["FMMLang"][property] = value;
                    parser.WriteFile("FMM_Lang.ini", data, Encoding.Unicode);
                }
                catch
                {
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile("FMM_Lang.ini");
                    data["FMMLang"][property] = value;
                    parser.WriteFile("FMM_Lang.ini", data);
                }
            }
            catch
            {

            }
        }

        private void optionsOffline_Click(object sender, RoutedEventArgs e)
        {
            if (offlineMode)
            {
                offlineMode = false;
                writeFMMIni("OfflineMode", "False");
                downloadableModsTab.Visibility = Visibility.Visible;
            }
            else
            {
                offlineMode = true;
                writeFMMIni("OfflineMode", "True");
                downloadableModsTab.Visibility = Visibility.Collapsed;
                myModsTab.IsSelected = true;
            }
        }

        private void optionsDeveloper_Click(object sender, RoutedEventArgs e)
        {
            if (developerMode)
            {
                developerMode = false;
                writeFMMIni("DeveloperMode", "False");
                optionsDeveloperSeparator.Visibility = Visibility.Collapsed;
                optionsDeveloperOptions.Visibility = Visibility.Collapsed;
                optionsDeveloper.IsChecked = false;
            }
            else
            {
                string sMessageBoxText = "Are you sure you want to enable Developer Mode?\nThis will expose settings that can irreversibly modify your game.\n\nDeveloper settings are reset to default every time FMM is opened.";
                string sCaption = this.Title;
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        developerMode = true;
                        writeFMMIni("DeveloperMode", "True");
                        optionsDeveloperSeparator.Visibility = Visibility.Visible;
                        optionsDeveloperOptions.Visibility = Visibility.Visible;
                        optionsDeveloper.IsChecked = true;
                        break;
                    default:
                        optionsDeveloper.IsChecked = false;
                        break;
                }
            }
        }

        private string readFMMIni(string property)
        {
            var parser = new FileIniDataParser();
            string value;
            try
            {
                try
                {
                    IniData data = parser.ReadFile("FMM.ini", Encoding.Unicode);
                    value = data["FMMPrefs"][property];
                }
                catch
                {
                    IniData data = parser.ReadFile("FMM.ini");
                    value = data["FMMPrefs"][property];
                }
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

        private string readFMMLang(string property)
        {
            var parser = new FileIniDataParser();
            string value;
            try
            {
                try
                {
                    IniData data = parser.ReadFile("FMM_Lang.ini", Encoding.Unicode);
                value = data["FMMLang"][property];
                }
                catch
                {
                    IniData data = parser.ReadFile("FMM_Lang.ini");
                    value = data["FMMLang"][property];
                }
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
            switch (readFMMIni("OfflineMode").ToLower())
            {
                case "":
                    writeFMMIni("OfflineMode", "False");
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
                    writeFMMIni("DeveloperMode", "False");
                    optionsDeveloper.IsChecked = false;
                    developerMode = false;
                    optionsDeveloperSeparator.Visibility = Visibility.Collapsed;
                    optionsDeveloperOptions.Visibility = Visibility.Collapsed;
                    break;
                case "false":
                    optionsDeveloper.IsChecked = false;
                    developerMode = false;
                    optionsDeveloperSeparator.Visibility = Visibility.Collapsed;
                    optionsDeveloperOptions.Visibility = Visibility.Collapsed;
                    break;
                case "true":
                    optionsDeveloper.IsChecked = true;
                    developerMode = true;
                    optionsDeveloperSeparator.Visibility = Visibility.Visible;
                    optionsDeveloperOptions.Visibility = Visibility.Visible;
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
            //switch (readFMMIni("Tab").ToLower())
            //{
            //    case "":
            //        writeFMMIni("Tab", "Left");
            //        optionsTabLeft.IsChecked = true;
            //        optionsTabTop.IsChecked = false;
            //        mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Left;
            //        break;
            //    case "left":
            //        optionsTabLeft.IsChecked = true;
            //        optionsTabTop.IsChecked = false;
            //        mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Left;
            //        break;
            //    case "top":
            //        optionsTabLeft.IsChecked = false;
            //        optionsTabTop.IsChecked = true;
            //        mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Top;
            //        break;
            //}
        }

        //private void optionsTabLeft_Click(object sender, RoutedEventArgs e)
        //{
        //    optionsTabLeft.IsChecked = true;
        //    optionsTabTop.IsChecked = false;
        //    mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Left;
        //    writeFMMIni("Tab", "Left");
        //}

        //private void optionsTabTop_Click(object sender, RoutedEventArgs e)
        //{
        //    optionsTabLeft.IsChecked = false;
        //    optionsTabTop.IsChecked = true;
        //    mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Top;
        //    writeFMMIni("Tab", "Top");
        //}

        private void optionsDeveloperBackup_Click(object sender, RoutedEventArgs e)
        {
            if (createBackup)
            {
                createBackup = false;
            }
            else
            {
                createBackup = true;
            }
        }

        private void optionsDeveloperRestore_Click(object sender, RoutedEventArgs e)
        {
            if (restoreBackup)
            {
                restoreBackup = false;
            }
            else
            {
                restoreBackup = true;
            }
        }

        private void optionsDeveloperTagTool_Click(object sender, RoutedEventArgs e)
        {
            if (showTagTool)
            {
                showTagTool = false;
            }
            else
            {
                showTagTool = true;
            }
        }
    }
}
