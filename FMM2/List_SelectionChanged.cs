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
using System.Diagnostics;

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
                    infobarHeader.Margin = new Thickness(infobarHeader.Margin.Left, infobarHeader.Margin.Top, 28, infobarHeader.Margin.Bottom);
                }
                else
                {
                    infobarIcon.Visibility = Visibility.Collapsed;
                    infobarHeader.Margin = new Thickness(infobarHeader.Margin.Left, infobarHeader.Margin.Top, 3, infobarHeader.Margin.Bottom);
                }

                if (item.Image != null)
                {
                    infobarImage.Visibility = Visibility.Visible;
                    infobarViewGrid.Height = 72;
                    infobarPanel.Margin = new Thickness(0,0,0,0);
                    infobarHeader.Margin = new Thickness(3, 0, infobarHeader.Margin.Right, 3);
                    infobarHeader.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarIcon.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarRect.Fill = this.Resources["infobarGradient"] as LinearGradientBrush;
                    infobarName.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarCredits.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    infobarImage.Visibility = Visibility.Collapsed;
                    infobarViewGrid.Height = Double.NaN;
                    infobarPanel.Margin = new Thickness(0,5,0,0);
                    infobarHeader.Margin = new Thickness(3,0,infobarHeader.Margin.Right,0);
                    infobarHeader.VerticalAlignment = VerticalAlignment.Top;
                    infobarIcon.VerticalAlignment = VerticalAlignment.Top;
                    infobarRect.Fill = new SolidColorBrush(Color.FromArgb(0,0,0,0));
                    infobarName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarCredits.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                }
                if (!string.IsNullOrEmpty(item.ImageFull) || !string.IsNullOrEmpty(item.Url))
                {
                    infobarCon.Cursor = Cursors.Hand;
                }
                else
                {
                    infobarCon.Cursor = null;
                }
            }
            else
            {
                infobarScroll.Visibility = Visibility.Collapsed;
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
                    infobarDLHeader.Margin = new Thickness(infobarDLHeader.Margin.Left, infobarDLHeader.Margin.Top, 28, infobarDLHeader.Margin.Bottom);
                }
                else
                {
                    infobarDLIcon.Visibility = Visibility.Collapsed;
                    infobarDLHeader.Margin = new Thickness(infobarDLHeader.Margin.Left, infobarDLHeader.Margin.Top, 3, infobarDLHeader.Margin.Bottom);
                }

                if (item.Image != null)
                {
                    infobarDLImage.Visibility = Visibility.Visible;
                    infobarDLViewGrid.Height = 72;
                    infobarDLPanel.Margin = new Thickness(0, 0, 0, 0);
                    infobarDLHeader.Margin = new Thickness(3, 0, infobarDLHeader.Margin.Right, 3);
                    infobarDLHeader.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarDLIcon.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarDLRect.Fill = this.Resources["infobarGradient"] as LinearGradientBrush;
                    infobarDLName.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarDLAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarDLCredits.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    infobarDLImage.Visibility = Visibility.Collapsed;
                    infobarDLViewGrid.Height = Double.NaN;
                    infobarDLPanel.Margin = new Thickness(0, 5, 0, 0);
                    infobarDLHeader.Margin = new Thickness(3, 0, infobarDLHeader.Margin.Right, 0);
                    infobarDLHeader.VerticalAlignment = VerticalAlignment.Top;
                    infobarDLIcon.VerticalAlignment = VerticalAlignment.Top;
                    infobarDLRect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    infobarDLName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarDLAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarDLCredits.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                }
                if (!string.IsNullOrEmpty(item.ImageFull) || !string.IsNullOrEmpty(item.Url))
                {
                    infobarDLCon.Cursor = Cursors.Hand;
                }
                else
                {
                    infobarDLCon.Cursor = null;
                }
            }
            else
            {
                infobarDLScroll.Visibility = Visibility.Collapsed;
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
                    infobarMMHeader.Margin = new Thickness(infobarMMHeader.Margin.Left, infobarMMHeader.Margin.Top, 28, infobarMMHeader.Margin.Bottom);
                }
                else
                {
                    infobarMMIcon.Visibility = Visibility.Collapsed;
                    infobarMMHeader.Margin = new Thickness(infobarMMHeader.Margin.Left, infobarMMHeader.Margin.Top, 3, infobarMMHeader.Margin.Bottom);
                }

                if (item.Image != null)
                {
                    infobarMMImage.Visibility = Visibility.Visible;
                    infobarMMViewGrid.Height = 72;
                    infobarMMPanel.Margin = new Thickness(0, 0, 0, 0);
                    infobarMMHeader.Margin = new Thickness(3, 0, infobarMMHeader.Margin.Right, 3);
                    infobarMMHeader.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarMMIcon.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarMMRect.Fill = this.Resources["infobarGradient"] as LinearGradientBrush;
                    infobarMMName.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarMMAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarMMCredits.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    infobarMMImage.Visibility = Visibility.Collapsed;
                    infobarMMViewGrid.Height = Double.NaN;
                    infobarMMPanel.Margin = new Thickness(0, 5, 0, 0);
                    infobarMMHeader.Margin = new Thickness(3, 0, infobarMMHeader.Margin.Right, 0);
                    infobarMMHeader.VerticalAlignment = VerticalAlignment.Top;
                    infobarMMIcon.VerticalAlignment = VerticalAlignment.Top;
                    infobarMMRect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    infobarMMName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarMMAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarMMCredits.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                }
                if (!string.IsNullOrEmpty(item.ImageFull) || !string.IsNullOrEmpty(item.Url))
                {
                    infobarMMCon.Cursor = Cursors.Hand;
                }
                else
                {
                    infobarMMCon.Cursor = null;
                }
            }
            else
            {
                infobarMMScroll.Visibility = Visibility.Collapsed;
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
                    infobarMDLHeader.Margin = new Thickness(infobarDLHeader.Margin.Left, infobarMDLHeader.Margin.Top, 28, infobarDLHeader.Margin.Bottom);
                }
                else
                {
                    infobarMDLIcon.Visibility = Visibility.Collapsed;
                    infobarMDLHeader.Margin = new Thickness(infobarDLHeader.Margin.Left, infobarMDLHeader.Margin.Top, 3, infobarDLHeader.Margin.Bottom);
                }

                if (item.Image != null)
                {
                    infobarMDLImage.Visibility = Visibility.Visible;
                    infobarMDLViewGrid.Height = 72;
                    infobarMDLPanel.Margin = new Thickness(0, 0, 0, 0);
                    infobarMDLHeader.Margin = new Thickness(3, 0, infobarMDLHeader.Margin.Right, 3);
                    infobarMDLHeader.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarMDLIcon.VerticalAlignment = VerticalAlignment.Bottom;
                    infobarMDLRect.Fill = this.Resources["infobarGradient"] as LinearGradientBrush;
                    infobarMDLName.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarMDLAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    infobarMDLSubmitter.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    infobarMDLImage.Visibility = Visibility.Collapsed;
                    infobarMDLViewGrid.Height = Double.NaN;
                    infobarMDLPanel.Margin = new Thickness(0, 5, 0, 0);
                    infobarMDLHeader.Margin = new Thickness(3, 0, infobarMDLHeader.Margin.Right, 0);
                    infobarMDLHeader.VerticalAlignment = VerticalAlignment.Top;
                    infobarMDLIcon.VerticalAlignment = VerticalAlignment.Top;
                    infobarMDLRect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    infobarMDLName.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarMDLAuthor.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    infobarMDLSubmitter.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                }
                if (!string.IsNullOrEmpty(item.ImageFull) || !string.IsNullOrEmpty(item.Url))
                {
                    infobarMDLCon.Cursor = Cursors.Hand;
                }
                else
                {
                    infobarMDLCon.Cursor = null;
                }
            }
            else
            {
                infobarMDLScroll.Visibility = Visibility.Collapsed;
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
                    if (infobarSBServerName.Text.Length > 60)
                    {
                        infobarSBServerName.FontSize = 6;
                        infobarSBServerName.FontWeight = FontWeights.Normal;
                    }
                    else
                    {
                        infobarSBServerName.FontSize = 7;
                        infobarSBServerName.FontWeight = FontWeights.Bold;
                    }
                }
                else
                {
                    infobarSBServerName.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                infobarSBScroll.Visibility = Visibility.Collapsed;
            }
        }
    }
}
