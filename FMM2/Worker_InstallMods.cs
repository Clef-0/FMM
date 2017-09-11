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
using System.Windows.Media.Effects;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private bool isFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        string filesinuse = "The game files are in use and cannot be modified. Please close the game or any game files you have open.";
        string yousureapply = "Are you sure you want to apply these mods?";
        string unsafelocations = "Mods downloaded from unsafe locations may harm your computer.";
        string pleasecheckmods = "Please check the mods you want to install before applying.";

        private void infobarApply_Click(object sender, RoutedEventArgs e)
        {
            List<Mod> checkedMods = new List<Mod>();

            foreach (Mod listedMod in myModsList.Items)
            {
                if (listedMod.IsChecked == true)
                {
                    checkedMods.Add(listedMod);
                }
            }
            modsTabs.IsEnabled = false;
            menu.IsEnabled = false;
            everythingGrid.Effect = new BlurEffect { Radius = 10 };
            installLogGrid.Visibility = Visibility.Visible;
            closeLogButton.Visibility = Visibility.Collapsed;
            closeLogButton.Focus();
            installLogBox.Text = "";

            if (checkedMods.Count == 0)
            {
                string fmmdat = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "fmm.dat");
                FileStream fmmdatWiper = File.Open(fmmdat, FileMode.OpenOrCreate);
                fmmdatWiper.SetLength(0);
                fmmdatWiper.Close();

                string mapsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "maps");
                DirectoryInfo dir1 = Directory.CreateDirectory(Path.Combine(mapsPath, "fmmbak"));
                DirectoryInfo dir2 = Directory.CreateDirectory(Path.Combine(mapsPath, "fmmbak", "fonts"));
                DirectoryInfo dir3 = Directory.CreateDirectory(Path.Combine(mapsPath, "fonts"));

                if (File.Exists(Path.Combine(mapsPath, "fmmbak", "tags.dat")))
                {
                    if (workerBackupRestore.IsBusy != true && !isFileLocked(new FileInfo(Path.Combine(mapsPath, "tags.dat"))))
                    {
                        workerBackupRestore.RunWorkerAsync(new string[] { mapsPath });
                        closeLogButton.Visibility = Visibility.Visible;
                    }
                    else if (isFileLocked(new FileInfo(Path.Combine(mapsPath, "tags.dat"))))
                    {
                        string sMessageBoxText = filesinuse;
                        string sCaption = this.Title;
                        MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                        MessageBoxImage icnMessageBox = MessageBoxImage.Error;

                        MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                        closeLogButton.Visibility = Visibility.Visible;
                        return;
                    }
                    else if (workerBackupRestore.IsBusy == true)
                    {
                        string sMessageBoxText = "Worker busy.";
                        string sCaption = this.Title;
                        MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                        MessageBoxImage icnMessageBox = MessageBoxImage.Error;

                        MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                        closeLogButton.Visibility = Visibility.Visible;
                        return;
                    }
                }
                else
                {
                    string sMessageBoxText = pleasecheckmods;
                    string sCaption = this.Title;
                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    closeLogButton.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                {
                    string sMessageBoxText = yousureapply + Environment.NewLine + unsafelocations;
                    string sCaption = this.Title;
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            break;
                        default:
                            return;
                    }
                }

                string mapsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "maps");

                // Backup tags and stuff
                DirectoryInfo dir1 = Directory.CreateDirectory(Path.Combine(mapsPath, "fmmbak"));
                DirectoryInfo dir2 = Directory.CreateDirectory(Path.Combine(mapsPath, "fmmbak", "fonts"));
                DirectoryInfo dir3 = Directory.CreateDirectory(Path.Combine(mapsPath, "fonts"));

                if (File.Exists(Path.Combine(mapsPath, "fmmbak", "tags.dat")))
                {
                    if (workerBackupRestore.IsBusy != true && !isFileLocked(new FileInfo(Path.Combine(mapsPath, "tags.dat"))))
                    {
                        workerBackupRestore.RunWorkerAsync(new string[] { mapsPath });
                    }
                    else if (isFileLocked(new FileInfo(Path.Combine(mapsPath, "tags.dat"))))
                    {
                        string sMessageBoxText = filesinuse;
                        string sCaption = this.Title;
                        MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                        MessageBoxImage icnMessageBox = MessageBoxImage.Error;

                        MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    }
                    else if (workerBackupRestore.IsBusy == true)
                    {
                        string sMessageBoxText = "Worker busy.";
                        string sCaption = this.Title;
                        MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                        MessageBoxImage icnMessageBox = MessageBoxImage.Error;

                        MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    }
                }
                else
                {
                    workerBackupCreate.RunWorkerAsync(new string[] { mapsPath });
                }
            }
        }

        private void closeLogButton_Click(object sender, RoutedEventArgs e)
        {
            everythingGrid.Effect = new BlurEffect { Radius = 0 };
            installLogGrid.Visibility = Visibility.Collapsed;
            modsTabs.IsEnabled = true;
            menu.IsEnabled = true;
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

        string installingmods = "Installing mods";
        string backuprestored = "Backup restored.";

        private void installModWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            Dispatcher.Invoke(new Action(() => {
                modsTabs.IsEnabled = false;
                menu.IsEnabled = false;
                installLogGrid.Visibility = Visibility.Visible;
                closeLogButton.Visibility = Visibility.Collapsed;
                closeLogButton.Focus();
                installLogBox.Text += "-- " + installingmods.ToUpper() + " --" + Environment.NewLine + Environment.NewLine;
            }));

            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 0;

            // Save File Storing Checked Items And Order

            string fmmdat = Path.Combine(Directory.GetCurrentDirectory(), "fmm.dat");
            FileStream fmmdatWiper = File.Open(fmmdat, FileMode.OpenOrCreate);
            fmmdatWiper.SetLength(0);
            fmmdatWiper.Close();

            StreamWriter fmmdatWriter = new StreamWriter(fmmdat);

            List<Mod> checkedMods = new List<Mod>();
            List<string> checkedModsNames = new List<string>();

            foreach (Mod listedMod in myModsList.Items)
            {
                if (listedMod.IsChecked == true)
                {
                    checkedMods.Add(listedMod);
                    checkedModsNames.Add(listedMod.Name);
                }
            }

            if (!installListOrder)
            {
                checkedModsNames.Reverse();
            }

            foreach (string checkedMod in checkedModsNames)
            {
                fmmdatWriter.WriteLine(checkedMod);
            }
            
            fmmdatWriter.Close();

            if (checkedMods.Count == 0)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(backuprestored, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    closeLogButton.Visibility = Visibility.Visible;
                }));
                return;
            }

            if (installListOrder == false)
            {
                checkedMods.Reverse();
            }


            //apply mods
            foreach (Mod item in checkedMods)
            {
                // init variables
                string fmFile = item.Location.Replace(".ini", ".fm");
                string batFile = Path.Combine(Path.GetDirectoryName(fmFile), "fm_temp.bat");

                try
                {
                    // duplicate .fm as temp .bat installer.
                    File.Copy(fmFile, batFile, true);

                    // startInfo for installer
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    if (showTagTool == false)
                    {
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        startInfo.RedirectStandardOutput = true;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    }
                    startInfo.FileName = batFile;
                    startInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();

                    Dispatcher.Invoke(new Action(() => {
                        installLogGrid.Visibility = Visibility.Visible;
                        closeLogButton.Visibility = Visibility.Collapsed;
                        closeLogButton.Focus();
                        installLogBox.Text += "[" + item.Name + "]" + Environment.NewLine;
                    }));

                    // start installer
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        if (startInfo.RedirectStandardOutput == true)
                        {
                            string standard_output;
                            while (!exeProcess.StandardOutput.EndOfStream)
                            {
                                standard_output = exeProcess.StandardOutput.ReadLine();
                                if (standard_output.StartsWith("FMM_OUTPUT "))
                                {
                                    string output = standard_output.Trim().Replace("FMM_OUTPUT ", "");

                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        installLogBox.Text += "| " + output + Environment.NewLine;
                                    }));
                                }
                                else if (standard_output.StartsWith("FMM_ALERT "))
                                {
                                    standard_output = standard_output.Trim().Replace("FMM_ALERT ", "");

                                    Dispatcher.Invoke(new Action(() => { MessageBox.Show(standard_output); }));
                                }
                            }
                        }

                        exeProcess.WaitForExit();
                    }

                    i++;
                }
                catch (Exception ex)
                {

                    Dispatcher.Invoke(new Action(() =>
                    {
                        WindowInteropHelper wih = new WindowInteropHelper(this);
                    }));
                    MessageBox.Show(errorinstalling + " '" + item.Name + "'." + Environment.NewLine + pleaseconsultdiscord + Environment.NewLine + Environment.NewLine + "\"" + ex.Message + "\"", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    File.Delete(batFile);
                }
            }

            Dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show(checkedmodsinstalled, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                closeLogButton.Visibility = Visibility.Visible;
            }));
        }

        string errorinstalling = "Error installing";
        string pleaseconsultdiscord = "Please consult the ElDewrito Discord server for help.";
        string checkedmodsinstalled = "Checked mods installed.";

        private void installModWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // todo?
        }
    }
}
