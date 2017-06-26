using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        private void serverBrowserWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Populates the server browser list.
            UpdateServerList();
        }

        private void serverBrowserWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
            {
                statusBarText.Content = ("Error: " + e.Error.Message);
            }
        }
        /// <summary>
        /// Hardcoded string array of master-server url's.
        /// dynamically instead.
        /// </summary>
        private string[] masterServers = new string[]
        {
            "http://eldewrito.red-m.net/list",
            "http://158.69.166.144:8080/list"
        };

        /// <summary>
        /// Downloads the .json from each master server in the masterServers list as a string, then serializes it into a MasterServer object.
        /// Downloads the .json from each server listed in each MasterServer object as a string while pinging the servers.
        /// Then serializes the server-info .jsons into HostServer objects, and adds them as ListViewItem's to "listView3"
        /// </summary>
        private async void UpdateServerList()
        {
            // This is used for downloading the server-info .jsons...
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };

            // List of the Task<string[]> from the output of ProcessURLAsync for each server, so that they can be awaited
            // in the "Add Servers To Listview" #Region.
            var serverTaskStrings = new List<Task<string[]>> { };

            // Using a WebClient object for downloading the MasterServer .jsons.
            using (var wc = new WebClient())
            {
                // This is to prevent Red-M's master server from 403'ing.
                wc.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");

                // Used for storing IP's of server's that have been added to "listView3" already, so that it can be checked
                // if a server has already been added before adding it potentially again.
                var serversAdded = new List<string> { };
                // Foreach "masterServer" url-string in the "masterServers" url array...
                foreach (string masterServer in masterServers)
                {
                    try
                    {
                        // Download the masterServer .json
                        string masterJson = wc.DownloadString(masterServer);
                        // Serialize the masterServer .json into a MasterServer object.
                        MasterServer master = JsonConvert.DeserializeObject<MasterServer>(masterJson);

                        // Foreach "server" url-string in the MasterServer object...
                        foreach (string server in master.result.servers)
                        {
                            // If the server has already been added to "serversAdded", don't doawnload the server-info json again.
                            if (!serversAdded.Contains(server))
                            {
                                // Add the server to "serversAdded" so that any following duplicate servers get filtered out.
                                serversAdded.Add(server);
                                // Download the server-info .json for the "server"
                                Task<string[]> download = ProcessURLAsync(server, client);
                                // Add the task to serverTaskStrings so that it can be awaited in the "Add Servers To Listview" #Region.
                                serverTaskStrings.Add(download);
                            }
                        }
                    }
                    catch { }
                }

                /// Awaits the tasks in "serverTaskStrings", serializes the .jsons from that output into "HostServer" objects,
                /// creates a ListViewItem from the properties of that HostServer, and then adds it to "listView3".
                #region Add Servers To Listview
                

                // Every Task<string[]> in "serverTaskStrings" needs to be awaited, serialized, and then added to "listView3".
                foreach (Task<string[]> serverTaskString in serverTaskStrings)
                {
                    try
                    {
                        // Await the "serverTaskString", which returns a string[] containing: server-info json text, server IP, and Ping.
                        string[] server = await serverTaskString;
                        // Serialize the server-info .json at index [0] in the "server-string[]" into a HostServer object.
                        Server host = JsonConvert.DeserializeObject<Server>(server[0]);
                        // Set the "ipAddress" in the HostServer object to index [1] in the "server-string[]".
                        host.ipAddress = server[1].Substring(0, server[1].IndexOf(":"));

                        if (host.variant == "" || host.variant == "none" || host.variant == "None")
                        {
                            char[] a = host.variantType.ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            host.variant = new string(a);
                        }
                        if (host.map == "" || host.map == "none" || host.map == "None")
                        {
                            char[] a = host.mapFile.ToCharArray();
                            a[0] = char.ToUpper(a[0]);
                            host.map = new string(a);
                        }

                        // Set the "ping" in the HostServer object to index [2] in the "server-string[]"... after doing some warfare on it.
                        // This is some weird math to calculate the "ping multiplier" based on the user's own connection
                        // instead of just using a flat 0.45 multiplier for everyone... If no server's responded to an
                        // actual ping request, the multiplier cannot be derived through this, so 0.45 is used instead.
                        if (pings != 0 && pingEsts != 0)
                            host.ping = ((int)(int.Parse(server[2]) * ((float)pings / pingEsts))).ToString();
                        else
                            host.ping = ((int)(int.Parse(server[2]) * 0.45)).ToString();

                        // NOTE: only here for evaluation purposes during debuging/testing.
                        //host.hostPlayer = ((float)pings / pingEsts).ToString();
                        
                        // Invoke needs to be called here to access the control outside of the thread it was created on...
                        await Dispatcher.BeginInvoke(new Action(() => {
                            //// Create a new ListViewItem using the HostServer's properties.
                            //Server item = new Server() { 
                            //        Passworded = host.Passworded, Name = host.Name, Host = host.Host, Ping = host.Ping, Map = host.Map, Gametype = host.Gametype, host.NumPlayers.ToString() + '/' + host.maxPlayers.ToString()
                            //    };
                            //// Set the ListViewItem.Tag to the HostServer object, so that additional information that isn't
                            //// being displayed in the listview can be easily looked up when a server is selected.
                            //item.Tag = host;

                            servers.Add(host);
                        }));

                        // Update the statusStrip at bottom left corner to show the amount of server's available.
                        if (mainTabs.SelectedIndex == 4)
                        {
                            int itemCount = servers.Count;
                            if (itemCount == 1) // If ONLY one server is available, use singular "server".
                            {
                                await Dispatcher.BeginInvoke(new Action(() => { statusNumber.Content = "1 server available"; }));
                            }
                            else // If MORE than one server is available, use plural "servers".
                            {
                                await Dispatcher.BeginInvoke(new Action(() => { statusNumber.Content = " servers available"; }));
                            }
                        }
                    }
                    catch { continue; }
                }
                #endregion 
            }
            client.Dispose();
        }

        // These are used to add successive pings into, to later be used in the multiplier derivation.
        // If either of these are empty, 0.45 will be used as the multiplier instead.
        public static long pings = 0;
        public static long pingEsts = 0;
        private async Task<string[]> ProcessURLAsync(string url, HttpClient client)
        {
            try
            {
                //Downloads the infoserver json (uses the download time for a rough ping estimate...)
                var serverString = await client.GetStringAsync($"http://{url}");

                // Intialize ping variable to "9999" so that if ping-estimation fails for a server, it gets sorted
                // to the bottom when sorting by ping..
                int ping = 9999;

                // Timing this to estimate a ping... Other server browsers use a 0.45 multiplier, and through
                // testing that seems like a pretty good one. This is innacurate for very slow connections though...
                // Not timing "GetStringAsync" above because the number is much higher... Because it is "async" maybe???
                // - really unsure on that... Server-Hosts really need to set up their machines to respond to
                // ping requests properly.
                try
                {
                    Stopwatch timer = new Stopwatch();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{url}");
                    timer.Start();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Close();
                    timer.Stop();
                    ping = (int)timer.ElapsedMilliseconds;
                }
                catch { }

                // If an actual ping request is successful, add both ping types to their totaller, so that they can
                // be used to derive an average ratio to use as a multiplier for calculating ping from the HTTP RTT
                Ping pingSender = new Ping();
                long reply = pingSender.Send(url.Replace(":11775", "")).RoundtripTime;
                if (reply != 0 && ping != 9999)
                {
                    pings += reply;
                    pingEsts += ping;
                }


                string[] serverArray = { serverString, url, ping.ToString() };
                return serverArray;
            }
            catch { return null; }
        }
    }
}
