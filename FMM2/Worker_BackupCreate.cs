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
        private void backupCreate_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = (string[])e.Argument;

            string mapsPath = args[0];


            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 0;
            if (!File.Exists(Path.Combine(mapsPath, "fmmbak", "tags.dat")))
            {
                List<string> files = lookBackupDirectory(Path.Combine(Environment.CurrentDirectory, "maps"));

                Dispatcher.Invoke(new Action(() => {
                    modsTabs.IsEnabled = false;
                    menu.IsEnabled = false;
                    installLogGrid.Visibility = Visibility.Visible;
                    closeLogButton.Visibility = Visibility.Collapsed;
                    closeLogButton.Focus();
                    installLogBox.Text = "";
                    installLogBox.Text += "-- " + creatingbackup.ToUpper() + " --" + Environment.NewLine + Environment.NewLine;
                }));

                if (createBackup)
                {
                    foreach (string file in files)
                    {
                        string fileloc = file.Replace(Path.Combine(Environment.CurrentDirectory, "maps"), "");
                        if (fileloc.StartsWith("\\"))
                        {
                            fileloc = fileloc.Substring(1);
                        }
                        if ((worker.CancellationPending == true || !createBackup))
                        {
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            i++;
                            float progress = ((float)i / files.Count()) * 100;
                            worker.ReportProgress(Convert.ToInt32(progress), fileloc);
                            try
                            {
                                File.Copy(Path.Combine(mapsPath, fileloc), Path.Combine(mapsPath, "fmmbak", fileloc), true);
                                Dispatcher.Invoke(new Action(() => {
                                    installLogBox.Text += "| " + bakcreatesuccess + Environment.NewLine;
                                }));
                            }
                            catch
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    installLogBox.Text += "| " + bakcreatefailed + Environment.NewLine;
                                }));
                            }
                        }
                    }
                }
                else
                {
                    Dispatcher.Invoke(new Action(() => {
                        installLogBox.Text += skippeddev + Environment.NewLine + Environment.NewLine;
                    }));
                }

                Dispatcher.Invoke(new Action(() => {
                    if (createBackup)
                    {
                        installLogBox.Text += cleanfilesbacked + Environment.NewLine + Environment.NewLine;
                    }
                    workerInstallMods.RunWorkerAsync();
                }));
            }
            else
            {
                workerBackupRestore.RunWorkerAsync(new string[] { mapsPath });
            }
        }
        string creatingbackup = "Creating backup";
        string bakcreatesuccess = "File backed up successfully.";
        string bakcreatefailed = "File failed to back up.";
        string skippeddev = "Skipped due to developer setting.";
        string cleanfilesbacked = "Clean files backed up.";

        private bool areBakAndMainEqual(string file1, string file2)
        {
            if (file1 == file2)
            {
                return true;
            }

            using (FileStream fs1 = File.OpenRead(file1))
            {
                using (FileStream fs2 = File.OpenRead(file2))
                {
                    if (fs1.Length != fs2.Length)
                    {
                        return false;
                    }

                    int count;
                    const int size = 0x1000000;

                    var buffer = new byte[size];
                    var buffer2 = new byte[size];

                    while ((count = fs1.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs2.Read(buffer2, 0, buffer2.Length);

                        for (int i = 0; i < count; i++)
                        {
                            if (buffer[i] != buffer2[i])
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        string preparingbackup = "Preparing a backup";

        private void backupCreate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            installLogBox.Text += preparingbackup + " (" + e.ProgressPercentage + "%) : " + Path.GetFileName((string)e.UserState) + Environment.NewLine;
        }

        private void backupCreate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
            {
                MessageBox.Show("Error: " + e.Error.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
