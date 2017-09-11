//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Globalization;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.NetworkInformation;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;

//namespace FMM2
//{
//    public partial class MainWindow : Window
//    {
//        private void populateServersList(object sender, DoWorkEventArgs e)
//        {
//            BackgroundWorker worker = sender as BackgroundWorker;

//            // Populates the server browser list.
//            UpdateServerList();
//        }

//        private void populateServersList_Completed(object sender, RunWorkerCompletedEventArgs e)
//        {
//            if (!(e.Error == null))
//            {
//                // :shrug:
//            }
//        }


//        /// <summary>
//        /// Hardcoded string array of master-server url's.
//        /// dynamically instead.
//        /// </summary>
//        private string[] masterServers = new string[]
//        {
//            "http://158.69.166.144:8080/list",
//            "http://eldewrito.red-m.net/list"
//        };

//        /// <summary>
//        /// Downloads the .json from each master server in the masterServers list as a string, then serializes it into a MasterServer object.
//        /// Downloads the .json from each server listed in each MasterServer object as a string while pinging the servers.
//        /// Then serializes the server-info .jsons into HostServer objects, and adds them as ListViewItem's to "listView3"
//        /// </summary>
//        private void UpdateServerList()
//        {
//            Dispatcher.Invoke(new Action(() =>
//            {
//                serversRefreshButton.Content = "Loading...";
//                serversRefreshButton.IsEnabled = false;
//            }));

//            List<string> masterJsons = new List<string>();
//            var serversAdded = new List<string>();

//            using (var wc = new WebClient())
//            {
//                // This is to prevent Red-M's master server from 403'ing.
//                wc.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");

//                foreach (string masterServer in masterServers)
//                {
//                    // Download the masterServer .json
//                    try
//                    {
//                        masterJsons.Add(wc.DownloadString(masterServer));
//                    }
//                    catch
//                    {
//                        //server down or 403
//                    }
//                }
//            }
//            foreach (string masterJson in masterJsons)
//                {
//                    // Serialize the masterServer .json into a MasterServer object.
//                    MasterServer master = JsonConvert.DeserializeObject<MasterServer>(masterJson);


//                    foreach (string server in master.result.servers)
//                    {
//                        // If the server has already been added to "serversAdded", don't download the server-info json again.
//                        if (!serversAdded.Contains(server))
//                        {
//                            // Add the server to "serversAdded" so that any following duplicate servers get filtered out.
//                            serversAdded.Add(server);
//                            taskPopulateServers.Add(new Task(() =>
//                            {
//                                AddServerToListView(server);
//                            }));
//                        }
//                    }
//                }
//                Task[] tasks = taskPopulateServers.ToArray();
//                if (tasks.Length > 0)
//                {
//                    Task.Factory.ContinueWhenAll(tasks, AddServersToListView_Done);
//                    Array.ForEach(tasks, (t) => t.Start());
//                }
//        }



//        private void AddServerToListView(string serverString)
//        {
//                HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
//                string[] server = ProcessURL(serverString, client);
//            if (server == null || server.Count() == 0 || server[0] == null || server[0] == "")
//            {
//                client.Dispose();
//                return;
//            }
//            if (server[1] == null)
//            {
//                server[1] = serverString;
//            }
//            if (server[2] == null)
//            {
//                server[2] = "9999";
//            }
//            Dispatcher.Invoke(new Action(() =>
//            {
//                // Serialize the server-info .json at index [0] in the "server-string[]" into a HostServer object.
//                Server host = JsonConvert.DeserializeObject<Server>(server[0]);
//                // Set the "ipAddress" in the HostServer object to index [1] in the "server-string[]".
//                host.ipAddress = server[1].Substring(0, server[1].IndexOf(":"));

//                host.name = host.name.Trim();
//                host.hostPlayer = host.hostPlayer.Trim();

//                if (host.variant == "" || host.variant == "none" || host.variant == "None")
//                {
//                    char[] a = host.variantType.ToCharArray();
//                    a[0] = char.ToUpper(a[0]);
//                    host.variant = new string(a);
//                }
//                if (host.map == "" || host.map == "none" || host.map == "None")
//                {
//                    char[] a = host.mapFile.ToCharArray();
//                    a[0] = char.ToUpper(a[0]);
//                    host.map = new string(a);
//                }

//                ObservableCollection<Player> newPlayers = new ObservableCollection<Player>();

//                foreach(Player player in host.players)
//                {
//                    if (player.name != null && player.name != "")
//                    {
//                        newPlayers.Add(player);
//                    }
//                }
//                host.players = newPlayers;
                
//                host.ping = int.Parse(server[2]).ToString();

//                // NOTE: only here for evaluation purposes during debuging/testing.
//                //host.hostPlayer = ((float)pings / pingEsts).ToString();

//                servers.Add(host);

//                // Update the statusStrip at bottom left corner to show the amount of server's available.
//                if (mainTabs.SelectedIndex == 4)
//                {
//                    int itemCount = servers.Count;
//                    if (itemCount == 1) // If ONLY one server is available, use singular "server".
//                    {
//                        serversStatusNumber.Content = "1 server available";
//                    }
//                    else // If MORE than one server is available, use plural "servers".
//                    {
//                        serversStatusNumber.Content = itemCount + " servers available";
//                    }
//                }
//                client.Dispose();
//            }));
//}
        
//        private string[] ProcessURL(string url, HttpClient client)
//        {
//            string serverString = "";
//            try
//            {
//                serverString = client.GetStringAsync($"http://{url}").Result;
//            }
//            catch { }
//            if (serverString != "")
//            {
//                var times = new List<double>();
//                for (int i = 0; i < 4; i++)
//                {
//                    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                    sock.Blocking = true;

//                    var stopwatch = new Stopwatch();
//                    double t = 0;

//                    // Measure the Connect call only
//                    try
//                    {
//                        stopwatch.Start();
//                        sock.Connect(CreateIPEndPoint(url));
//                        stopwatch.Stop();
//                        t = stopwatch.Elapsed.TotalMilliseconds;
//                    }
//                    catch (SocketException)
//                    {
//                        t = 9999;
//                    }
//                    times.Add(t);

//                    sock.Close();

//                    Thread.Sleep(100);
//                }


//                string[] serverArray = { serverString, url, Math.Round(times.Average(), MidpointRounding.AwayFromZero).ToString() };
//                return serverArray;
//            }
//            else
//            {
//                return new string[] { "", "", "" };
//            }
//        }
//        private void AddServersToListView_Done(Task[] tasks)
//        {
//            taskPopulateServers.Clear();
//            Dispatcher.Invoke(new Action(() =>
//            {
//                serversRefreshButton.Content = "Refresh";
//                serversRefreshButton.IsEnabled = true;
//            }));
//        }

//        public static IPEndPoint CreateIPEndPoint(string endPoint)
//        {
//            string[] ep = endPoint.Split(':');
//            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
//            IPAddress ip;
//            if (ep.Length > 2)
//            {
//                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
//                {
//                    throw new FormatException("Invalid ip-adress");
//                }
//            }
//            else
//            {
//                if (!IPAddress.TryParse(ep[0], out ip))
//                {
//                    throw new FormatException("Invalid ip-adress");
//                }
//            }
//            int port;
//            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
//            {
//                throw new FormatException("Invalid port");
//            }
//            return new IPEndPoint(ip, port);
//        }
//    }
//}
