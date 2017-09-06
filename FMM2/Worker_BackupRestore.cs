using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private List<string> lookBackupDirectory(string targetDirectory)
        {
            List<string> fileEntries = new List<string>(Directory.GetFiles(targetDirectory));
            //foreach (string fileName in fileEntries)
                
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                List<string> subfiles = lookBackupDirectory(subdirectory);
                foreach (string subfile in subfiles)
                {
                    fileEntries.Add(subfile);
                }
            }

            return fileEntries;
        }

        private void backupRestore_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = (string[])e.Argument;

            string mapsPath = args[0];

            List<string> files = lookBackupDirectory(Path.Combine(Environment.CurrentDirectory, "maps", "fmmbak"));

            Dispatcher.BeginInvoke(new Action(() => {
                installLogGrid.Visibility = Visibility.Visible;
                closeLogButton.Visibility = Visibility.Collapsed;
                closeLogButton.Focus();
                installLogBox.Text += "-- RESTORING BACKUP --" + Environment.NewLine + Environment.NewLine;
            }));

            if (restoreBackup)
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                int i = 0;
                foreach (string file in files)
                {
                    string fileloc = file.Replace(Path.Combine(Environment.CurrentDirectory, "maps", "fmmbak"), "");
                    if (fileloc.StartsWith("\\"))
                    {
                        fileloc = fileloc.Substring(1);
                    }
                    if ((worker.CancellationPending == true))
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        if (File.Exists(Path.Combine(mapsPath, "fmmbak", fileloc)))
                        {
                            if (!areBakAndMainEqual(Path.Combine(mapsPath, "fmmbak", fileloc), Path.Combine(mapsPath, fileloc)))
                            {
                                File.Copy(Path.Combine(mapsPath, "fmmbak", fileloc), Path.Combine(mapsPath, fileloc), true);
                            }
                            i++;
                            float progress = ((float)i / (float)files.Count()) * 100;
                            worker.ReportProgress(Convert.ToInt32(progress), fileloc);
                        }
                    }
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => {
                    installLogBox.Text += "Skipped due to developer setting." + Environment.NewLine + Environment.NewLine;
                }));
            }
        }

        private void backupRestore_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            installLogBox.Text += "Restoring clean files (" + e.ProgressPercentage + "%) : " + Path.GetFileName((string)e.UserState) + " restored." + Environment.NewLine;
        }
        
        private void backupRestore_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                if (restoreBackup)
                {
                    installLogBox.Text += "Clean files restored." + Environment.NewLine + Environment.NewLine;
                }
                workerInstallMods.RunWorkerAsync();
            }
        }
    }
}
