using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void itemLocationOpen_Click(object sender, EventArgs e)
        {
            if (mainTabs.SelectedIndex == 0) //mods
            {
                string loc = "";
                if (modsTabs.SelectedIndex == 0)
                {
                    loc = Path.GetDirectoryName(((Mod)myModsList.SelectedItem).Location);
                }
                if (loc != "" && Directory.Exists(loc))
                {
                    Process.Start(loc);
                }
            }
            else if (mainTabs.SelectedIndex == 1) //maps
            {
            }
            else if (mainTabs.SelectedIndex == 2) //gametypes
            {
                //TODO
            }
            else if (mainTabs.SelectedIndex == 3) //medals
            {
                //TODO
            }
            else if (mainTabs.SelectedIndex == 4) //server browser
            {
            }
        }

        private void itemMark_Click(object sender, EventArgs e)
        {
            if (mainTabs.SelectedIndex == 0) //mods
            {
                if (modsTabs.SelectedIndex == 0)
                {
                    ((Mod)myModsList.SelectedItem).IsChecked = !((Mod)myModsList.SelectedItem).IsChecked;
                }
                else if (modsTabs.SelectedIndex == 1)
                {
                    ((Mod)downloadableModsList.SelectedItem).IsChecked = !((Mod)downloadableModsList.SelectedItem).IsChecked;
                }
            }
            else if (mainTabs.SelectedIndex == 1) //maps
            {
            }
            else if (mainTabs.SelectedIndex == 2) //gametypes
            {
                //TODO
            }
            else if (mainTabs.SelectedIndex == 3) //medals
            {
                //TODO
            }
            else if (mainTabs.SelectedIndex == 4) //server browser
            {
            }
        }
    }
}
