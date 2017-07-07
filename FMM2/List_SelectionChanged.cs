using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private void myModsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myModsList.SelectedItems.Count > 0)
            {
                infobarScroll.Visibility = Visibility.Visible;
                Mod item = (Mod)myModsList.SelectedItems[0];

                if (item.Credits != null && item.Credits != "")
                {
                    infobarCredits.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarCredits.Visibility = Visibility.Collapsed;
                }

                if (item.RevisionDate != null && item.RevisionDate != "")
                {
                    infobarRevisionDate.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarRevisionDate.Visibility = Visibility.Collapsed;
                }

                if (item.EDVersion != null && item.EDVersion != "")
                {
                    infobarEDVersion.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarEDVersion.Visibility = Visibility.Collapsed;
                }

                if (item.LongDesc != null && item.LongDesc != "")
                {
                    infobarDescription.Visibility = Visibility.Visible;
                }
                else if (item.Desc != null && item.Desc != "")
                {
                    infobarDescription.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDescription.Visibility = Visibility.Collapsed;
                }

                if (item.LongWarnings != null && item.LongWarnings != "")
                {
                    infobarWarnings.Visibility = Visibility.Visible;
                }
                else if (item.Warnings != null && item.Warnings != "")
                {
                    infobarWarnings.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarWarnings.Visibility = Visibility.Collapsed;
                }

                if (item.Icon != null)
                {
                    infobarIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarIcon.Visibility = Visibility.Collapsed;
                }

                if (item.Image != null)
                {
                    infobarImage.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarImage.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void downloadableModsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (downloadableModsList.SelectedItems.Count > 0)
            {
                infobarDLScroll.Visibility = Visibility.Visible;
                Mod item = (Mod)downloadableModsList.SelectedItems[0];

                if (item.Credits != null && item.Credits != "")
                {
                    infobarDLCredits.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLCredits.Visibility = Visibility.Collapsed;
                }

                if (item.RevisionDate != null && item.RevisionDate != "")
                {
                    infobarDLRevisionDate.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLRevisionDate.Visibility = Visibility.Collapsed;
                }

                if (item.EDVersion != null && item.EDVersion != "")
                {
                    infobarDLEDVersion.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLEDVersion.Visibility = Visibility.Collapsed;
                }

                if (item.LongDesc != null && item.LongDesc != "")
                {
                    infobarDLDescription.Visibility = Visibility.Visible;
                }
                else if (item.Desc != null && item.Desc != "")
                {
                    infobarDLDescription.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLDescription.Visibility = Visibility.Collapsed;
                }

                if (item.LongWarnings != null && item.LongWarnings != "")
                {
                    infobarDLWarnings.Visibility = Visibility.Visible;
                }
                else if (item.Warnings != null && item.Warnings != "")
                {
                    infobarDLWarnings.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLWarnings.Visibility = Visibility.Collapsed;
                }

                if (item.Icon != null)
                {
                    infobarDLIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLIcon.Visibility = Visibility.Collapsed;
                }

                if (item.Image != null)
                {
                    infobarDLImage.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarDLImage.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void myMapsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myMapsList.SelectedItems.Count > 0)
            {
                infobarMMScroll.Visibility = Visibility.Visible;
                Map item = (Map)myMapsList.SelectedItems[0];

                if (item.Credits != null && item.Credits != "")
                {
                    infobarMMCredits.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMMCredits.Visibility = Visibility.Collapsed;
                }

                if (item.RevisionDate != null && item.RevisionDate != "")
                {
                    infobarMMRevisionDate.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMMRevisionDate.Visibility = Visibility.Collapsed;
                }

                if (item.EDVersion != null && item.EDVersion != "")
                {
                    infobarMMEDVersion.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMMEDVersion.Visibility = Visibility.Collapsed;
                }

                if (item.Desc != null && item.Desc != "")
                {
                    infobarMMDescription.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMMDescription.Visibility = Visibility.Collapsed;
                }

                if (item.Icon != null)
                {
                    infobarMMIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMMIcon.Visibility = Visibility.Collapsed;
                }

                if (item.Image != null)
                {
                    infobarMMImage.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMMImage.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void downloadableMapsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (downloadableMapsList.SelectedItems.Count > 0)
            {
                infobarMDLScroll.Visibility = Visibility.Visible;
                Map item = (Map)downloadableMapsList.SelectedItems[0];

                if (item.Submitter != null && item.Submitter != "")
                {
                    infobarMDLSubmitter.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMDLSubmitter.Visibility = Visibility.Collapsed;
                }

                if (item.RevisionDate != null && item.RevisionDate != "")
                {
                    infobarMDLRevisionDate.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMDLRevisionDate.Visibility = Visibility.Collapsed;
                }

                if (item.EDVersion != null && item.EDVersion != "")
                {
                    infobarMDLEDVersion.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMDLEDVersion.Visibility = Visibility.Collapsed;
                }

                if (item.Desc != null && item.Desc != "")
                {
                    infobarMDLDescription.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMDLDescription.Visibility = Visibility.Collapsed;
                }

                if (item.Icon != null)
                {
                    infobarMDLIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMDLIcon.Visibility = Visibility.Collapsed;
                }

                if (item.Image != null)
                {
                    infobarMDLImage.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarMDLImage.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void serverBrowserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serverBrowserList.SelectedItems.Count > 0)
            {
                infobarSBScroll.Visibility = Visibility.Visible;
                Server item = (Server)serverBrowserList.SelectedItems[0];

                if (item.name != null && item.name != "")
                {
                    infobarSBServerName.Visibility = Visibility.Visible;
                }
                else
                {
                    infobarSBServerName.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
