using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void infobarApply_Click(object sender, RoutedEventArgs e)
        {
            workerInstallMods.RunWorkerAsync();
        }

        private void closeLogButton_Click(object sender, RoutedEventArgs e)
        {
            installLogGrid.Visibility = Visibility.Collapsed;
            mainTabs.IsEnabled = true;
            menu.IsEnabled = true;
            myModsRefreshButton.IsEnabled = true;
            dlModsRefreshButton.IsEnabled = true;
            myMapsRefreshButton.IsEnabled = true;
            dlMapsRefreshButton.IsEnabled = true;
            serversRefreshButton.IsEnabled = true;
        }

        private void installLogScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            bool AutoScroll = true;

            if (e.ExtentHeightChange == 0)
            {
                if (installLogScroll.VerticalOffset == installLogScroll.ScrollableHeight)
                {   
                    AutoScroll = true;
                }
                else
                {
                    AutoScroll = false;
                }
            }
            
            if (AutoScroll && e.ExtentHeightChange != 0)
            {
                installLogScroll.ScrollToVerticalOffset(installLogScroll.ExtentHeight);
            }
        }

        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        private void installModWorker(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                mainTabs.IsEnabled = false;
                menu.IsEnabled = false;
                myModsRefreshButton.IsEnabled = false;
                dlModsRefreshButton.IsEnabled = false;
                myMapsRefreshButton.IsEnabled = false;
                dlMapsRefreshButton.IsEnabled = false;
                serversRefreshButton.IsEnabled = false;
                installLogGrid.Visibility = Visibility.Visible;
                closeLogButton.Visibility = Visibility.Collapsed;
                closeLogButton.Focus();
                installLogBox.Text = "";
                //for (int x = 1; x < 10; x++) {
                    installLogBox.Text += "Foundation Mod Manager [2.00]" + Environment.NewLine +
                    Environment.NewLine +
                    "Please report any bugs and/or feature requests at" + Environment.NewLine +
                    "<http://github.com/Clef-0/FMM2>." + Environment.NewLine + Environment.NewLine;
                //}
            }));

            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 0;

            // Save File Storing Checked Items And Order

            string fmmdat = Path.Combine(Directory.GetCurrentDirectory(), "fmm", "profiles", profile + ".dat");
            FileStream fmmdatWiper = File.Open(fmmdat, FileMode.OpenOrCreate);
            fmmdatWiper.SetLength(0);
            fmmdatWiper.Close();

            StreamWriter fmmdatWriter = new StreamWriter(fmmdat);

            List<Mod> checkedMods = new List<Mod>();

            foreach (Mod listedMod in myModsList.Items)
            {
                if (listedMod.IsChecked == true)
                {
                    checkedMods.Add(listedMod);
                    fmmdatWriter.WriteLine(listedMod.Name);
                }
            }
            fmmdatWriter.Close();

            if (installListOrder == false)
            {
                checkedMods.Reverse();
            }

            Thread.Sleep(2000);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                closeLogButton.Visibility = Visibility.Visible;
            }));

            ////apply mods
            //foreach (Mod item in checkedMods)
            //{
            //    // init variables
            //    string fmFile = Path.Combine(Directory.GetCurrentDirectory(), "mods", item.Location.Replace(".ini",".cfm"));
            //    string batFile = Path.Combine(Path.GetDirectoryName(fmFile), "fm_temp.bat");

            //    try
            //    {
            //        // duplicate .fm as temp .bat installer.
            //        File.Copy(fmFile, batFile, true);

            //        // startInfo for installer
            //        ProcessStartInfo startInfo = new ProcessStartInfo();
            //        if (showInstallers == false)
            //        {
            //            startInfo.CreateNoWindow = true;
            //            startInfo.UseShellExecute = false;
            //            startInfo.RedirectStandardOutput = true;
            //            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //        }
            //        startInfo.FileName = batFile;
            //        startInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();

            //        Dispatcher.BeginInvoke(new Action(() => {
            //            textBox1.Invoke(new appendNewOutputCallback(this.appendNewOutput), new object[] { "[" + item.SubItems[0].Text + "]" });
            //        }));

            //        // start installer
            //        using (Process exeProcess = Process.Start(startInfo))
            //        {
            //            if (startInfo.RedirectStandardOutput == true)
            //            {
            //                string standard_output;
            //                while (!exeProcess.StandardOutput.EndOfStream)
            //                {
            //                    standard_output = exeProcess.StandardOutput.ReadLine();
            //                    if (standard_output.StartsWith("FMM_OUTPUT "))
            //                    {
            //                        standard_output = standard_output.Trim().Replace("FMM_OUTPUT ", "");
            //                        textBox1.Invoke(new appendNewOutputCallback(this.appendNewOutput), new object[] { standard_output });
            //                    }
            //                    else if (standard_output.StartsWith("FMM_ALERT "))
            //                    {
            //                        standard_output = standard_output.Trim().Replace("FMM_ALERT ", "");
            //                        Invoke(new showMessageBoxCallback(this.showMessageBox), new object[] { standard_output });
            //                    }
            //                }
            //            }

            //            exeProcess.WaitForExit();
            //        }

            //        i++;
            //        float progress = ((float)i / (float)listView1.CheckedItems.Cast<ListViewItem>().Count()) * 100f;
            //        worker.ReportProgress(Convert.ToInt32(progress));
            //    }
            //    catch (Exception ex)
            //    {

            //        Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            WindowInteropHelper wih = new WindowInteropHelper(this);
            //            FlashWindow(wih.Handle, true);
            //        }));
            //        MessageBox.Show("Error installing " + item.Name + ".\nPlease consult the #eldorito IRC for help.\n\n\"" + ex.Message + "\"", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    finally
            //    {
            //        File.Delete(batFile);
            //    }
            //}
        }
    }
}
