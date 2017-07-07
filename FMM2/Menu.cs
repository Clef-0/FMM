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
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("FMM.ini");
                data["FMMPrefs"][property] = value;
                parser.WriteFile("FMM.ini", data);
            }
            catch
            {

            }
        }

        private void optionsRepoConduit_Click(object sender, RoutedEventArgs e)
        {
            optionsRepoConduit.IsChecked = true;
            optionsRepoMarchi.IsChecked = false;
            repositoryConduit = true;
            writeFMMIni("Repo", "Conduit");
        }

        private void optionsRepoMarchi_Click(object sender, RoutedEventArgs e)
        {
            optionsRepoConduit.IsChecked = false;
            optionsRepoMarchi.IsChecked = true;
            repositoryConduit = false;
            writeFMMIni("Repo", "Marchi");
        }

        private void optionsOffline_Click(object sender, RoutedEventArgs e)
        {
            if (offlineMode)
            {
                offlineMode = false;
                writeFMMIni("OfflineMode", "False");
            }
            else
            {
                offlineMode = true;
                writeFMMIni("OfflineMode", "True");
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
            }
            else
            {
                developerMode = true;
                writeFMMIni("DeveloperMode", "True");
                optionsDeveloperSeparator.Visibility = Visibility.Visible;
                optionsDeveloperOptions.Visibility = Visibility.Visible;
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
            switch (readFMMIni("Repo").ToLower())
            {
                case "":
                    writeFMMIni("Repo", "Conduit");
                    optionsRepoConduit.IsChecked = true;
                    optionsRepoMarchi.IsChecked = false;
                    repositoryConduit = true;
                    break;
                case "conduit":
                    optionsRepoConduit.IsChecked = true;
                    optionsRepoMarchi.IsChecked = false;
                    repositoryConduit = true;
                    break;
                case "marchi":
                    optionsRepoConduit.IsChecked = false;
                    optionsRepoMarchi.IsChecked = true;
                    repositoryConduit = false;
                    break;
            }
            switch (readFMMIni("Tab").ToLower())
            {
                case "":
                    writeFMMIni("Tab", "Left");
                    optionsTabLeft.IsChecked = true;
                    optionsTabTop.IsChecked = false;
                    mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Left;
                    break;
                case "left":
                    optionsTabLeft.IsChecked = true;
                    optionsTabTop.IsChecked = false;
                    mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Left;
                    break;
                case "top":
                    optionsTabLeft.IsChecked = false;
                    optionsTabTop.IsChecked = true;
                    mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Top;
                    break;
            }
            switch (readFMMIni("CreateBackup").ToLower())
            {
                case "":
                    optionsDeveloperBackup.IsChecked = true;
                    createBackup = true;
                    break;
                case "false":
                    optionsDeveloperBackup.IsChecked = false;
                    createBackup = false;
                    break;
                case "true":
                    optionsDeveloperBackup.IsChecked = true;
                    createBackup = true;
                    break;
            }
            switch (readFMMIni("RestoreBackup").ToLower())
            {
                case "":
                    optionsDeveloperRestore.IsChecked = true;
                    restoreBackup = true;
                    break;
                case "false":
                    optionsDeveloperRestore.IsChecked = false;
                    restoreBackup = false;
                    break;
                case "true":
                    optionsDeveloperRestore.IsChecked = true;
                    restoreBackup = true;
                    break;
            }
            switch (readFMMIni("ShowTagTool").ToLower())
            {
                case "":
                    optionsDeveloperTagTool.IsChecked = false;
                    showTagTool = false;
                    break;
                case "false":
                    optionsDeveloperTagTool.IsChecked = false;
                    showTagTool = false;
                    break;
                case "true":
                    optionsDeveloperTagTool.IsChecked = true;
                    showTagTool = true;
                    break;
            }
        }

        private void optionsTabLeft_Click(object sender, RoutedEventArgs e)
        {
            optionsTabLeft.IsChecked = true;
            optionsTabTop.IsChecked = false;
            mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Left;
            writeFMMIni("Tab", "Left");
        }

        private void optionsTabTop_Click(object sender, RoutedEventArgs e)
        {
            optionsTabLeft.IsChecked = false;
            optionsTabTop.IsChecked = true;
            mainTabs.TabStripPlacement = System.Windows.Controls.Dock.Top;
            writeFMMIni("Tab", "Top");
        }

        private void optionsDeveloperBackup_Click(object sender, RoutedEventArgs e)
        {
            if (createBackup)
            {
                createBackup = false;
                writeFMMIni("CreateBackup", "False");
            }
            else
            {
                createBackup = true;
                writeFMMIni("CreateBackup", "True");
            }
        }

        private void optionsDeveloperRestore_Click(object sender, RoutedEventArgs e)
        {
            if (restoreBackup)
            {
                restoreBackup = false;
                writeFMMIni("RestoreBackup", "False");
            }
            else
            {
                restoreBackup = true;
                writeFMMIni("RestoreBackup", "True");
            }
        }

        private void optionsDeveloperTagTool_Click(object sender, RoutedEventArgs e)
        {
            if (showTagTool)
            {
                showTagTool = false;
                writeFMMIni("ShowTagTool", "False");
            }
            else
            {
                showTagTool = true;
                writeFMMIni("ShowTagTool", "True");
            }
        }
    }
}
