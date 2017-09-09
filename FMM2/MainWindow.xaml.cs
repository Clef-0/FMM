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
using System.Reflection;
using System.Diagnostics;
using System.Collections;

namespace FMM2
{
    public class CollectionItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private bool _checked;
        public bool IsChecked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }
    }

    public class Mod : CollectionItem
    {
        public string Name { get; set; } = "";
        public string Author { get; set; } = "";
        public string Version { get; set; } = "";
        public string Desc { get; set; } = "";
        public string Description // aliases for column binding
        {
            get
            {
                return Desc;
            }
        }
        public string LongDesc { get; set; } = "";
        public string Warnings { get; set; } = "";
        public string LongWarnings { get; set; } = "";
        public BitmapImage Icon { get; set; } = new BitmapImage();
        public string Url { get; set; } = "";
        public BitmapImage Image { get; set; } = new BitmapImage();
        public string ImageFull { get; set; } = "";
        public string Location { get; set; } = "";
        public string RevisionDate { get; set; } = "";
        public string EDVersion { get; set; } = "";
        public string Credits { get; set; } = "";
        public string Required { get; set; } = "";

        public Mod()
        {

        }
    }

    public class FMMFile : CollectionItem
    {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string Author { get; set; } = "";
        public string Version { get; set; } = "";
        public string Desc { get; set; } = "";
        public string LongDesc { get; set; } = "";
        public string Description
        {
            get
            {
                return Desc;
            }
        }
        public BitmapImage Icon { get; set; } = new BitmapImage();
        public string Url { get; set; } = "";
        private BitmapImage _Image;
        public BitmapImage Image
        {
            get
            {
                if (_Image != null)
                {
                    return _Image;
                }
                switch (BaseMap)
                {
                    case ("Standoff"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/bunkerworld.png", UriKind.Absolute));
                    case ("Narrows"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/chill.png", UriKind.Absolute));
                    case ("The Pit"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/cyberdyne.png", UriKind.Absolute));
                    case ("High Ground"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/deadlock.png", UriKind.Absolute));
                    case ("Flatgrass"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/flatgrass.png", UriKind.Absolute));
                    case ("Guardian"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/guardian.png", UriKind.Absolute));
                    case ("Hang Em High"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/hangem-high.png", UriKind.Absolute));
                    case ("Lockout"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/lockout.png", UriKind.Absolute));
                    case ("Main Menu"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/mainmenu.png", UriKind.Absolute));
                    case ("Valhalla"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/riverworld.png", UriKind.Absolute));
                    case ("Diamondback"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_avalanche.png", UriKind.Absolute));
                    case ("Edge"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_edge.png", UriKind.Absolute));
                    case ("Reactor"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_reactor.png", UriKind.Absolute));
                    case ("Icebox"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_turf.png", UriKind.Absolute));
                    case ("Sandtrap"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/shrine.png", UriKind.Absolute));
                    case ("Station"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/station.png", UriKind.Absolute));
                    case ("Last Resort"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/zanzibar.png", UriKind.Absolute));
                }
                return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/unknown.png", UriKind.Absolute));
            }
            set
            {
                _Image = value;
                NotifyPropertyChanged("Image");
            }
        }
        public string ImageFull { get; set; } = "";
        public string Location { get; set; } = "";
        public string RevisionDate { get; set; } = "";
        public string EDVersion { get; set; } = "";
        public string Credits { get; set; } = "";
        public string Submitter { get; set; } = "";
        public string BaseMap { get; set; } = "";
        public int DLs { get; set; } = 0;

        public FMMFile()
        {

        }
    }

    public class Server : CollectionItem
    {
        // Lower case for sake of JSON deserialisation
        public string name { get; set; } = "";
        public string Name
        {
            get
            {
                return MainWindow.ReturnCleanASCII(name).Trim();
            }
        }
        public int port { get; set; } = 0;
        public string hostPlayer { get; set; } = "";
        public string Host
        {
            get
            {
                return hostPlayer;
            }
        }
        public bool isDedicated { get; set; } = false;
        public int sprintEnabled { get; set; } = 0;
        public bool sprintEnabledBool
        {
            get
            {
                if (sprintEnabled == 1)
                    return true;
                else
                    return false;
            }
        }
        public int sprintUnlimitedEnabled { get; set; } = 0;
        public int assassinationEnabled { get; set; } = 0;
        public bool assassinationEnabledBool
        {
            get
            {
                if (assassinationEnabled == 1)
                    return true;
                else
                    return false;
            }
        }
        public bool VoIP { get; set; } = false;
        public bool teams { get; set; } = false;
        private string _map;
        public string map
        {
            get
            {
                if (_map == "" || _map == "None" || _map == "none")
                {
                    return char.ToUpper(mapFile[0]) + mapFile.Substring(1);
                }
                else
                {
                    return _map;
                }
            }
            set
            {
                _map = value;
            }
        }
        public string Map
        {
            get
            {
                return map;
            }
        }
        public string mapFile { get; set; } = "";
        private string _variant;
        public string variant
        {
            get
            {
                if (_variant == "" || _variant == "None" || _variant == "none" || _variant == " " || _variant == null)
                {
                    return variantType;
                }
                else
                {
                    return _variant;
                }
            }
            set
            {
                _variant = value;
            }
        }
        public string Gametype
        {
            get
            {
                return variant;
            }
        }
        private string _variantType;
        public string variantType
        {
            get
            {
                if (_variantType == "" || _variantType == "None" || _variantType == "none" || _variantType == " " || _variantType == null)
                {
                    return "Slayer";
                }
                else
                {
                    return _variantType;
                }
            }
            set
            {
                _variantType = value;
            }
        }
        public string status { get; set; } = "";
        public int numPlayers { get; set; } = 0;
        public int maxPlayers { get; set; } = 0;
        public string playerSlash
        { 
            get
            {
                return numPlayers + "/" + maxPlayers;
            }
        }
        public int Players // for column sorting
        {
            get
            {
                return numPlayers;
            }
        }
        public ObservableCollection<Player> players { get; set; } = new ObservableCollection<Player> { };
        public List<string> mods { get; set; } = new List<string> { };
        public string gameVersion { get; set; } = "";
        public string eldewritoVersion { get; set; } = "";
        public string ipAddress { get; set; } = "";
        public string ping { get; set; } = "99999";
        public int Ping // for column sorting
        {
            get
            {
                int rping = 0;
                if (int.TryParse(ping, out rping))
                {
                    return rping;
                }
                else
                {
                    return 9999;
                }
            }
        }
        public string passworded { get; set; } = "";
        private BitmapImage _MapImage;
        public BitmapImage MapImage
        {
            get
            {
                if (_MapImage != null)
                {
                    return _MapImage;
                }
                switch (mapFile.ToLowerInvariant())
                {
                    case ("bunkerworld"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/bunkerworld.png", UriKind.Absolute));
                    case ("chill"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/chill.png", UriKind.Absolute));
                    case ("cyberdyne"):
                    case ("xe_pit"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/cyberdyne.png", UriKind.Absolute));
                    case ("deadlock"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/deadlock.png", UriKind.Absolute));
                    case ("flatgrass"):
                    case ("xe_flatgrass-r8"):
                    case ("xe_flatgrass-r9"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/flatgrass.png", UriKind.Absolute));
                    case ("guardian"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/guardian.png", UriKind.Absolute));
                    case ("hangem-high"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/hangem-high.png", UriKind.Absolute));
                    case ("lockout"):
                    case ("lockout-050"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/lockout.png", UriKind.Absolute));
                    case ("mainmenu"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/mainmenu.png", UriKind.Absolute));
                    case ("riverworld"):
                    case ("xe_valhalla"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/riverworld.png", UriKind.Absolute));
                    case ("s3d_avalanche"):
                    case ("xe_avalanche"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_avalanche.png", UriKind.Absolute));
                    case ("s3d_edge"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_edge.png", UriKind.Absolute));
                    case ("s3d_reactor"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_reactor.png", UriKind.Absolute));
                    case ("s3d_turf"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/s3d_turf.png", UriKind.Absolute));
                    case ("shrine"):
                    case ("xe_sandtrap"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/shrine.png", UriKind.Absolute));
                    case ("station"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/station.png", UriKind.Absolute));
                    case ("zanzibar"):
                    case ("xe_resort"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/zanzibar.png", UriKind.Absolute));
                }
                return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "maps/unknown.png", UriKind.Absolute));
            }
            set
            {
                _MapImage = value;
                NotifyPropertyChanged("MapImage");
            }
        }
        private BitmapImage _GametypeImage;
        public BitmapImage GametypeImage
        {
            get
            {
                if (_GametypeImage != null)
                {
                    return _GametypeImage;
                }
                switch (variantType.ToLowerInvariant())
                {
                    case ("assault"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/assault.png", UriKind.Absolute));
                    case ("ctf"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/ctf.png", UriKind.Absolute));
                    case ("forge"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/forge.png", UriKind.Absolute));
                    case ("infection"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/infection.png", UriKind.Absolute));
                    case ("juggernaut"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/juggernaut.png", UriKind.Absolute));
                    case ("koth"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/koth.png", UriKind.Absolute));
                    case ("none"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/none.png", UriKind.Absolute));
                    case ("oddball"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/oddball.png", UriKind.Absolute));
                    case ("slayer"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/slayer.png", UriKind.Absolute));
                    case ("territories"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/territories.png", UriKind.Absolute));
                    case ("vip"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/vip.png", UriKind.Absolute));
                    case ("zombiez"):
                        return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/zombiez.png", UriKind.Absolute));
                }
                return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "gametypes/undefined.png", UriKind.Absolute));
            }
            set
            {
                _GametypeImage = value;
                NotifyPropertyChanged("GametypeImage");
            }
        }

        public Server()
        {

        }
    }
    public class Result
    {
        public int code { get; set; }
        public string msg { get; set; }
        public List<string> servers { get; set; }
    }
    public class MasterServer
    {
        public int listVersion { get; set; }
        public Result result { get; set; }
    }

    public class Player : CollectionItem
    {
        public string name { get; set; } = "";
        public int score { get; set; } = 0;
        public int kills { get; set; } = 0;
        public int assists { get; set; } = 0;
        public int deaths { get; set; } = 0;
        public int team { get; set; } = 0;

        public bool isAlive { get; set; } = false;
        public string uid { get; set; } = "";

        public string Name // for column sorting
        {
            get
            {
                return name;
            }
        }
        public int Score
        {
            get
            {
                return score;
            }
        }
        public int K
        {
            get
            {
                return kills;
            }
        }
        public int A
        {
            get
            {
                return assists;
            }
        }
        public int D
        {
            get
            {
                return deaths;
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // global settings
        bool installListOrder = true;
        bool offlineMode = false;
        bool developerMode = false;
        bool createBackup = true;
        bool restoreBackup = true;
        bool showTagTool = false;
        // end


        public static Random Rnd = new Random();
        ObservableCollection<Mod> mMods { get; set; }
        ObservableCollection<Mod> dMods { get; set; }
        ObservableCollection<FMMFile> mMaps { get; set; }
        ObservableCollection<FMMFile> dMaps { get; set; }
        ObservableCollection<FMMFile> mGametypes { get; set; }
        ObservableCollection<FMMFile> dGametypes { get; set; }
        ObservableCollection<FMMFile> mMedals { get; set; }
        ObservableCollection<FMMFile> dMedals { get; set; }
        ObservableCollection<Server> servers { get; set; }
        BackgroundWorker workerPopulateMyMods = new BackgroundWorker();
        BackgroundWorker workerPopulateDLMods = new BackgroundWorker();
        BackgroundWorker workerPopulateMyMaps = new BackgroundWorker();
        BackgroundWorker workerPopulateMyGametypes = new BackgroundWorker();
        BackgroundWorker workerPopulateDLFiles = new BackgroundWorker();
        List<Task> taskPopulateMyMods = new List<Task>();
        List<Task> taskPopulateMyMaps = new List<Task>();
        List<Task> taskPopulateMyGametypes = new List<Task>();
        List<Task> taskPopulateDLMods = new List<Task>();
        List<Task> taskPopulateDLFiles = new List<Task>();
        BackgroundWorker workerPopulateServers = new BackgroundWorker();
        List<Task> taskPopulateServers = new List<Task>();
        BackgroundWorker workerDownloadMods = new BackgroundWorker();
        BackgroundWorker workerDownloadMaps = new BackgroundWorker();
        BackgroundWorker workerInstallMods = new BackgroundWorker();
        BackgroundWorker workerBackupCreate = new BackgroundWorker();
        BackgroundWorker workerBackupRestore = new BackgroundWorker();

        const string repository = "https://github.com/Clef-0/FMM-Mods/trunk/";
        const string filerepository = "https://github.com/Clef-0/FMM-Files/trunk/";

        public static string ReturnCleanASCII(string s)
        {
            StringBuilder sb = new StringBuilder(s.Length);
            foreach (char c in s.ToCharArray())
            {
                if ((int)c > 122)
                    continue;
                if ((int)c < 48)
                    continue;
                if ((int)c > 57 && (int)c < 65)
                    continue;
                if ((int)c > 90 && (int)c < 97)
                    continue;
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static void initialiseAssembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string assemblyFile = (args.Name.Contains(','))
                    ? args.Name.Substring(0, args.Name.IndexOf(','))
                    : args.Name;

                assemblyFile += ".dll";

                string absoluteFolder = new FileInfo((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath).Directory.FullName;
                string targetPath = Path.Combine(absoluteFolder, "FMM", "lib", assemblyFile);

                try
                {
                    return Assembly.LoadFile(targetPath);
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        public MainWindow()
        {
            InitializeComponent();

            // add handlers for workers
            workerPopulateMyMods.DoWork += new DoWorkEventHandler(populateMyModsList);
            workerPopulateDLMods.DoWork += new DoWorkEventHandler(populateDLModsList);
            //workerPopulateMyMaps.DoWork += new DoWorkEventHandler(populateMyMapsList);
            //workerPopulateMyGametypes.DoWork += new DoWorkEventHandler(populateMyGametypesList);
            //workerPopulateDLFiles.DoWork += new DoWorkEventHandler(populateDLMapsList);
            //workerPopulateServers.DoWork += new DoWorkEventHandler(populateServersList);
            //workerPopulateServers.RunWorkerCompleted += new RunWorkerCompletedEventHandler(populateServersList_Completed);
            workerDownloadMods.DoWork += new DoWorkEventHandler(dlModWorker);
            //workerDownloadMaps.DoWork += new DoWorkEventHandler(dlMapWorker);
            workerInstallMods.DoWork += new DoWorkEventHandler(installModWorker_DoWork);
            workerInstallMods.RunWorkerCompleted += new RunWorkerCompletedEventHandler(installModWorker_RunWorkerCompleted);
            workerBackupCreate.DoWork += new DoWorkEventHandler(backupCreate_DoWork);
            workerBackupCreate.WorkerReportsProgress = true;
            workerBackupCreate.WorkerSupportsCancellation = true;
            workerBackupCreate.ProgressChanged += new ProgressChangedEventHandler(backupCreate_ProgressChanged);
            workerBackupCreate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backupCreate_RunWorkerCompleted);
            workerBackupRestore.DoWork += new DoWorkEventHandler(backupRestore_DoWork);
            workerBackupRestore.WorkerReportsProgress = true;
            workerBackupRestore.WorkerSupportsCancellation = true;
            workerBackupRestore.ProgressChanged += new ProgressChangedEventHandler(backupRestore_ProgressChanged);
            workerBackupRestore.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backupRestore_Completed);

            mMods = new ObservableCollection<Mod>();
            dMods = new ObservableCollection<Mod>();
            //mMaps = new ObservableCollection<FMMFile>();
            //dMaps = new ObservableCollection<FMMFile>();
            //mGametypes = new ObservableCollection<FMMFile>();
            //dGametypes = new ObservableCollection<FMMFile>();
            //servers = new ObservableCollection<Server>();
            myModsList.ItemsSource = mMods;
            downloadableModsList.ItemsSource = dMods;
            //myMapsList.ItemsSource = mMaps;
            //downloadableMapsList.ItemsSource = dMaps;
            //myGametypesList.ItemsSource = mGametypes;
            //downloadableGametypesList.ItemsSource = dGametypes;
            //serverBrowserList.ItemsSource = servers;
            this.DataContext = this;

            mMods.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
            dMods.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
            //mMaps.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
            //dMaps.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
            //mGametypes.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
            //dGametypes.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
            //servers.CollectionChanged += new NotifyCollectionChangedEventHandler(tabsUpdateStatus);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initialiseAssembly();
            bool doNotInit = false;
#if !DEBUG
            if (!File.Exists("mtndew.dll"))
            {
                doNotInit = true;
                MessageBox.Show(Application.Current.MainWindow, "Error: FMM.exe should be placed in the ElDewrito folder next to mtndew.dll.", "Foundation Mod Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
#endif

            if (!File.Exists(Path.Combine("FMM", "lib", "INIFileParser.dll")))
            {
                if (!doNotInit)
                {
                    doNotInit = true;
                    ExtractDLLs();
                    if (File.Exists(Path.Combine("FMM", "lib", "INIFileParser.dll")))
                    {
                        ProcessStartInfo Info = new ProcessStartInfo();
                        Info.Arguments = "/C choice /C Y /N /D Y /T 1 & START \"\" \"" + Assembly.GetExecutingAssembly().Location + "\"";
                        Info.WindowStyle = ProcessWindowStyle.Hidden;
                        Info.CreateNoWindow = true;
                        Info.FileName = "cmd.exe";
                        Process.Start(Info);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        MessageBox.Show(Application.Current.MainWindow, "Error: FMM failed to deploy its libraries.", "Foundation Mod Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            if (!doNotInit)
            {
                new ListViewDragDropManager<Mod>(myModsList);

                modsTabs.Margin = new Thickness { Bottom = 0, Left = 0, Right = 0, Top = menu.ActualHeight };
                installLogGrid.Visibility = Visibility.Collapsed;

                downloadableModsAlert.Visibility = Visibility.Collapsed;
                //downloadableMapsAlert.Visibility = Visibility.Collapsed;
                //downloadableGametypesAlert.Visibility = Visibility.Collapsed;
                //serverBrowserAlert.Visibility = Visibility.Collapsed;

                if (!File.Exists("FMM.ini"))
                {
                    try
                    {
                        using (new FileStream("FMM.ini", FileMode.CreateNew)) { }
                    }
                    catch { }
                }
                loadFMMSettings();
                loadFMMLang();
                if (readFMMIni("FirstTime") != "False")
                {
                    writeFMMIni("FirstTime", "False");
                    MessageBox.Show(Application.Current.MainWindow, "Mods are not officially supported by the ElDewrito developers.\nIf you experience bugs while mods are installed, please report them to the creators of those mods.\nThe ElDewrito dev team will not accept bug reports suspected to be caused by mods.", "Foundation Mod Manager", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    myModsAlert.Visibility = Visibility.Collapsed;
                }

                if (offlineMode)
                {
                    downloadableModsTab.Visibility = Visibility.Collapsed;
                }

                //if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "profiles")))
                //{
                //    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "profiles"));
                //}

                if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp")))
                {
                    Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "fmm", "temp"), true);
                }
                
                workerPopulateMyMods.RunWorkerAsync(); // populates local mod list
                if (!offlineMode)
                {
                    workerPopulateDLMods.RunWorkerAsync(); //populate dl mod list
                }
                //workerPopulateMyMaps.RunWorkerAsync();
                //workerPopulateDLFiles.RunWorkerAsync();
                //workerPopulateServers.RunWorkerAsync();

                // hide infobar
                infobarScroll.Visibility = Visibility.Collapsed;
                infobarDLScroll.Visibility = Visibility.Collapsed;
                //infobarMMScroll.Visibility = Visibility.Collapsed;
                //infobarMDLScroll.Visibility = Visibility.Collapsed;
                //infobarSBScroll.Visibility = Visibility.Collapsed;
            }
        }

        private void infobarUp_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = myModsList.SelectedIndex;

            if (selectedIndex > 0)
            {
                Mod itemToMoveUp = mMods[selectedIndex];
                mMods.RemoveAt(selectedIndex);
                mMods.Insert(selectedIndex - 1, itemToMoveUp);
                myModsList.SelectedIndex = selectedIndex - 1;
            }
        }

        private void infobarDn_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = myModsList.SelectedIndex;
            if (selectedIndex + 1 < mMods.Count)
            {
                Mod itemToMoveDown = mMods[selectedIndex];
                mMods.RemoveAt(selectedIndex);
                mMods.Insert(selectedIndex + 1, itemToMoveDown);
                myModsList.SelectedIndex = selectedIndex + 1;
            }
        }

        private void infobarDel_Click(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Are you sure you want to delete " + ((Mod)myModsList.SelectedItem).Name + "?" + Environment.NewLine + "This cannot be undone.";
            string sCaption = "Foundation Mod Manager";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    string localLocation = Path.GetDirectoryName(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "mods", "tagmods", ((Mod)myModsList.SelectedItem).Location.Replace("/", "\\")));

                    try
                    {
                        deleteDirectory(localLocation);
                    }
                    catch
                    {
                        // mod folder doesn't exist or user not elevated. oh well.
                    }

                    mMods.Clear();
                    infobarScroll.Visibility = Visibility.Collapsed;
                    workerPopulateMyMods.RunWorkerAsync();
                    break;
            }
        }

        //private void infobarMMDel_Click(object sender, RoutedEventArgs e)
        //{
        //    string sMessageBoxText = "Are you sure you want to delete " + ((FMMFile)myMapsList.SelectedItem).Name + "?" + Environment.NewLine + "This cannot be undone.";
        //    string sCaption = "Foundation Mod Manager";
        //    MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
        //    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

        //    MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

        //    switch (rsltMessageBox)
        //    {
        //        case MessageBoxResult.Yes:
        //            int selectedIndex = myMapsList.SelectedIndex;
        //            string locDel = mMaps[selectedIndex].Location;
        //            File.Delete(locDel);
        //            try
        //            {
        //                Directory.Delete(Path.GetDirectoryName(locDel), false);
        //            }
        //            catch (IOException)
        //            {
        //                // directory is not empty, so that's fine.
        //            }

        //            if (!workerPopulateMyMaps.IsBusy)
        //            {
        //                mMaps.Clear();
        //                workerPopulateMyMaps.RunWorkerAsync();
        //            }
        //            infobarMMScroll.Visibility = Visibility.Collapsed;
        //            break;
        //    }
        //}

        //private void infobarGTDel_Click(object sender, RoutedEventArgs e)
        //{
        //    string sMessageBoxText = "Are you sure you want to delete " + ((FMMFile)myGametypesList.SelectedItem).Name + "?" + Environment.NewLine + "This cannot be undone.";
        //    string sCaption = "Foundation Mod Manager";
        //    MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
        //    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

        //    MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

        //    switch (rsltMessageBox)
        //    {
        //        case MessageBoxResult.Yes:
        //            int selectedIndex = myGametypesList.SelectedIndex;
        //            string locDel = mGametypes[selectedIndex].Location;
        //            File.Delete(locDel);
        //            try
        //            {
        //                Directory.Delete(Path.GetDirectoryName(locDel), false);
        //            }
        //            catch (IOException)
        //            {
        //                // directory is not empty, so that's fine.
        //            }

        //            if (!workerPopulateMyGametypes.IsBusy)
        //            {
        //                mGametypes.Clear();
        //                workerPopulateMyGametypes.RunWorkerAsync();
        //            }
        //            infobarGTScroll.Visibility = Visibility.Collapsed;
        //            break;
        //    }
        //}

        private void myModsAlertHide_Click(object sender, RoutedEventArgs e)
        {
            myModsAlert.Visibility = Visibility.Collapsed;
        }

        private void downloadableModsAlertHide_Click(object sender, RoutedEventArgs e)
        {
            downloadableModsAlert.Visibility = Visibility.Collapsed;
        }

        //private void downloadableMapsAlertHide_Click(object sender, RoutedEventArgs e)
        //{
        //    downloadableMapsAlert.Visibility = Visibility.Collapsed;
        //}

        //private void downloadableGametypesAlertHide_Click(object sender, RoutedEventArgs e)
        //{
        //    downloadableGametypesAlert.Visibility = Visibility.Collapsed;
        //}

        //private void serverBrowserAlertHide_Click(object sender, RoutedEventArgs e)
        //{
        //    serverBrowserAlert.Visibility = Visibility.Collapsed;
        //}

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            //if (mainTabs.SelectedIndex == 0) //mods
            {
                if (modsTabs.SelectedIndex == 0 && taskPopulateMyMods.Count == 0) //my
                {
                    mMods.Clear();
                    workerPopulateMyMods.RunWorkerAsync();

                    infobarScroll.Visibility = Visibility.Collapsed;
                }
                else if (modsTabs.SelectedIndex == 1) //dl
                {
                    if (!workerPopulateDLMods.IsBusy && taskPopulateDLMods.Count == 0)
                    {
                        dMods.Clear();
                        workerPopulateDLMods.RunWorkerAsync(); //populate dl mod list

                        infobarDLScroll.Visibility = Visibility.Collapsed;
                    }
                }
            }
            //else if (mainTabs.SelectedIndex == 1) //maps
            //{
            //    if (mapsTabs.SelectedIndex == 0) //my
            //    {
            //        if (!workerPopulateMyMaps.IsBusy && taskPopulateMyMaps.Count == 0)
            //        {
            //            mMaps.Clear();
            //            workerPopulateMyMaps.RunWorkerAsync();

            //            infobarMMScroll.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //    else if (mapsTabs.SelectedIndex == 1) //dl
            //    {
            //        if (!workerPopulateDLFiles.IsBusy && taskPopulateDLFiles.Count == 0)
            //        {
            //            dMaps.Clear();
            //            workerPopulateDLFiles.RunWorkerAsync();

            //            infobarMDLScroll.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //}
            //else if (mainTabs.SelectedIndex == 2) //gametypes
            //{
            //    if (gametypesTabs.SelectedIndex == 0) //my
            //    {
            //        if (!workerPopulateMyGametypes.IsBusy && taskPopulateMyGametypes.Count == 0)
            //        {
            //            mGametypes.Clear();
            //            workerPopulateMyGametypes.RunWorkerAsync();

            //            infobarGTScroll.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //    else if (gametypesTabs.SelectedIndex == 1) //dl
            //    {
            //        if (!workerPopulateDLFiles.IsBusy && taskPopulateDLFiles.Count == 0)
            //        {
            //            dGametypes.Clear();
            //            workerPopulateDLFiles.RunWorkerAsync();

            //            infobarGTDLScroll.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //}
            //else if (mainTabs.SelectedIndex == 3) //medals
            //{
            //    //TODO
            //}
            //else if (mainTabs.SelectedIndex == 4) //server browser
            //{
            //    if (!workerPopulateServers.IsBusy && taskPopulateServers.Count == 0)
            //    {
            //        servers.Clear();
            //        workerPopulateServers.RunWorkerAsync();

            //        infobarSBScroll.Visibility = Visibility.Collapsed;
            //    }
            //}
        }
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void myModsList_HeaderClicked(object sender, RoutedEventArgs e)
        {
            //infobarScroll.Visibility = Visibility.Collapsed;
            listViewHeaderClicked<Mod>(myModsList, mMods, e);
        }

        private void downloadableModsList_HeaderClicked(object sender, RoutedEventArgs e)
        {
            //infobarDLScroll.Visibility = Visibility.Collapsed;
            listViewHeaderClicked<Mod>(downloadableModsList, dMods, e);
        }

        //private void myMapsList_HeaderClicked(object sender, RoutedEventArgs e)
        //{
        //    //infobarMMScroll.Visibility = Visibility.Collapsed;
        //    listViewHeaderClicked<FMMFile>(myMapsList, mMaps, e);
        //}

        //private void downloadableMapsList_HeaderClicked(object sender, RoutedEventArgs e)
        //{
        //    //infobarMDLScroll.Visibility = Visibility.Collapsed;
        //    listViewHeaderClicked<FMMFile>(downloadableMapsList, dMaps, e);
        //}

        //private void myGametypesList_HeaderClicked(object sender, RoutedEventArgs e)
        //{
        //    listViewHeaderClicked<FMMFile>(myGametypesList, mGametypes, e);
        //}

        //private void downloadableGametypesList_HeaderClicked(object sender, RoutedEventArgs e)
        //{
        //    listViewHeaderClicked<FMMFile>(downloadableGametypesList, dGametypes, e);
        //}

        //private void serverBrowserList_HeaderClicked(object sender, RoutedEventArgs e)
        //{
        //    //infobarSBScroll.Visibility = Visibility.Collapsed;
        //    listViewHeaderClicked<Server>(serverBrowserList, servers, e);
        //}

        //private void infobarSBPlayersList_HeaderClicked(object sender, RoutedEventArgs e)
        //{
        //    //infobarSBScroll.Visibility = Visibility.Collapsed;
        //    listViewHeaderClicked<Player>(infobarSBPlayers, ((Server)(serverBrowserList.SelectedItem)).players, e);
        //}

        string modAvailable = "mod available";
        string modsAvailable = "mods available";

        private void tabsUpdateStatus(object sender, SelectionChangedEventArgs e)
        {
            if (modsTabs.SelectedIndex == 0)
            {
                int itemCount = mMods.Count;
                if (itemCount == 1)
                {
                    myModsStatusNumber.Content = "1 " + modAvailable;
                }
                else if (itemCount != 1)
                {
                    myModsStatusNumber.Content = itemCount + " " + modsAvailable;
                }
            }
            if (modsTabs.SelectedIndex == 1)
            {
                int itemCount = dMods.Count;
                if (itemCount == 1)
                {
                    dlModsStatusNumber.Content = "1 " + modAvailable;
                }
                else if (itemCount != 1)
                {
                    dlModsStatusNumber.Content = itemCount + " " + modsAvailable;
                }
            }
            //if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 0)
            //{
            //    int itemCount = mMaps.Count;
            //    if (itemCount == 1)
            //    {
            //        myMapsStatusNumber.Content = "1 " + "map available";
            //    }
            //    else if (itemCount != 1)
            //    {
            //        myMapsStatusNumber.Content = itemCount + " maps available";
            //    }
            //}
            //if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 1)
            //{
            //    int itemCount = dMaps.Count;
            //    if (itemCount == 1)
            //    {
            //        dlMapsStatusNumber.Content = "1 " + "mod available";
            //    }
            //    else if (itemCount != 1)
            //    {
            //        dlMapsStatusNumber.Content = itemCount + " maps available";
            //    }
            //}
            //if (mainTabs.SelectedIndex == 4)
            //{
            //    int itemCount = servers.Count;
            //    if (itemCount == 1)
            //    {
            //        serversStatusNumber.Content = "1 " + "server available";
            //    }
            //    else if (itemCount != 1)
            //    {
            //        serversStatusNumber.Content = itemCount + " servers available";
            //    }
            //}
        }
        private void tabsUpdateStatus(object sender, NotifyCollectionChangedEventArgs e)
        {
            tabsUpdateStatus(sender, (SelectionChangedEventArgs)null);
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            infobarScroll.Visibility = Visibility.Collapsed;
            infobarDLScroll.Visibility = Visibility.Collapsed;
            //infobarMMScroll.Visibility = Visibility.Collapsed;
            //infobarMDLScroll.Visibility = Visibility.Collapsed;
            //infobarSBScroll.Visibility = Visibility.Collapsed;
            if (modsTabs.SelectedIndex == 0)
            {
                CollectionViewSource.GetDefaultView(myModsList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            }
            if (modsTabs.SelectedIndex == 1)
            {
                CollectionViewSource.GetDefaultView(downloadableModsList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            }
            //if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 0)
            //{
            //    CollectionViewSource.GetDefaultView(myMapsList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            //}
            //if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 1)
            //{
            //    CollectionViewSource.GetDefaultView(downloadableMapsList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            //}
            //if (mainTabs.SelectedIndex == 2 && gametypesTabs.SelectedIndex == 0)
            //{
            //    CollectionViewSource.GetDefaultView(myGametypesList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            //}
            //if (mainTabs.SelectedIndex == 2 && gametypesTabs.SelectedIndex == 1)
            //{
            //    CollectionViewSource.GetDefaultView(downloadableGametypesList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            //}
            //if (mainTabs.SelectedIndex == 4)
            //{
            //    CollectionViewSource.GetDefaultView(serverBrowserList.ItemsSource).Filter = new Predicate<object>(collectionItemContainsSearch);
            //}
        }

        private bool collectionItemContainsSearch(object listItem)
        {
            switch (listItem.GetType().ToString())
            {
                case "FMM2.Mod":
                    Mod mod = (Mod)listItem;
                    if (modsTabs.SelectedIndex == 0)
                    {
                        if (mod.Name.ToLowerInvariant().Contains(myModsSearchBox.Text.ToLowerInvariant()) || mod.Author.ToLowerInvariant().Contains(myModsSearchBox.Text.ToLowerInvariant()) || mod.Desc.ToLowerInvariant().Contains(myModsSearchBox.Text.ToLowerInvariant()))
                        {
                            return true;
                        }
                    }
                    if (modsTabs.SelectedIndex == 1)
                    {
                        if (mod.Name.ToLowerInvariant().Contains(dlModsSearchBox.Text.ToLowerInvariant()) || mod.Author.ToLowerInvariant().Contains(dlModsSearchBox.Text.ToLowerInvariant()) || mod.Desc.ToLowerInvariant().Contains(dlModsSearchBox.Text.ToLowerInvariant()))
                        {
                            return true;
                        }
                    }
                    break;
                //case "FMM2.FMMFile":
                //    FMMFile map = (FMMFile)listItem;
                //    if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 0)
                //    {
                //        if (map.Name.ToLowerInvariant().Contains(myMapsSearchBox.Text.ToLowerInvariant()) || map.Author.ToLowerInvariant().Contains(myMapsSearchBox.Text.ToLowerInvariant()) || map.Desc.ToLowerInvariant().Contains(myMapsSearchBox.Text.ToLowerInvariant()))
                //        {
                //            return true;
                //        }
                //    }
                //    if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 1)
                //    {
                //        if (map.Name.ToLowerInvariant().Contains(dlMapsSearchBox.Text.ToLowerInvariant()) || map.Author.ToLowerInvariant().Contains(dlMapsSearchBox.Text.ToLowerInvariant()) || map.Desc.ToLowerInvariant().Contains(dlMapsSearchBox.Text.ToLowerInvariant()))
                //        {
                //            return true;
                //        }
                //    }
                //    if (mainTabs.SelectedIndex == 2 && gametypesTabs.SelectedIndex == 0)
                //    {
                //        if (map.Name.ToLowerInvariant().Contains(myGametypesSearchBox.Text.ToLowerInvariant()) || map.Author.ToLowerInvariant().Contains(myGametypesSearchBox.Text.ToLowerInvariant()) || map.Desc.ToLowerInvariant().Contains(myGametypesSearchBox.Text.ToLowerInvariant()))
                //        {
                //            return true;
                //        }
                //    }
                //    if (mainTabs.SelectedIndex == 2 && gametypesTabs.SelectedIndex == 1)
                //    {
                //        if (map.Name.ToLowerInvariant().Contains(dlGametypesSearchBox.Text.ToLowerInvariant()) || map.Author.ToLowerInvariant().Contains(dlGametypesSearchBox.Text.ToLowerInvariant()) || map.Desc.ToLowerInvariant().Contains(dlGametypesSearchBox.Text.ToLowerInvariant()))
                //        {
                //            return true;
                //        }
                //    }
                //    break;
                //case "FMM2.Server":
                //    Server server = (Server)listItem;
                //    if (server.name.ToLowerInvariant().Contains(serversSearchBox.Text.ToLowerInvariant()) || server.Host.ToLowerInvariant().Contains(serversSearchBox.Text.ToLowerInvariant()))
                //    {
                //        return true;
                //    }
                //    break;
            }

            return false;
        }

        private void modsList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myModsList.SelectedItems.Count > 0)
                infobarScroll.Visibility = Visibility.Visible;
            if (downloadableModsList.SelectedItems.Count > 0)
                infobarDLScroll.Visibility = Visibility.Visible;
            //if (myMapsList.SelectedItems.Count > 0)
            //    infobarMMScroll.Visibility = Visibility.Visible;
            //if (downloadableMapsList.SelectedItems.Count > 0)
            //    infobarMDLScroll.Visibility = Visibility.Visible;
            //if (serverBrowserList.SelectedItems.Count > 0)
            //    infobarSBScroll.Visibility = Visibility.Visible;
        }

        private void infobarCon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (modsTabs.SelectedIndex == 0)
            {
                if (myModsList.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(((Mod)myModsList.SelectedItem).ImageFull))
                    {
                        Process.Start(((Mod)myModsList.SelectedItem).ImageFull);
                    }
                    else if (!string.IsNullOrEmpty(((Mod)myModsList.SelectedItem).Url))
                    {
                        Process.Start(((Mod)myModsList.SelectedItem).Url);
                    }
                }
            }
            if (modsTabs.SelectedIndex == 1)
            {
                if (downloadableModsList.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(((Mod)downloadableModsList.SelectedItem).ImageFull))
                    {
                        Process.Start(((Mod)downloadableModsList.SelectedItem).ImageFull);
                    }
                    else if (!string.IsNullOrEmpty(((Mod)downloadableModsList.SelectedItem).Url))
                    {
                        Process.Start(((Mod)downloadableModsList.SelectedItem).Url);
                    }
                }
            }
            //if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 0)
            //{
            //    if (myMapsList.SelectedItem != null)
            //    {
            //        if (!string.IsNullOrEmpty(((FMMFile)myMapsList.SelectedItem).ImageFull))
            //        {
            //            Process.Start(((FMMFile)myMapsList.SelectedItem).ImageFull);
            //        }
            //        else if (!string.IsNullOrEmpty(((FMMFile)myMapsList.SelectedItem).Url))
            //        {
            //            Process.Start(((FMMFile)myMapsList.SelectedItem).Url);
            //        }
            //    }
            //}
            //if (mainTabs.SelectedIndex == 1 && mapsTabs.SelectedIndex == 1)
            //{
            //    if (downloadableMapsList.SelectedItem != null)
            //    {
            //        if (!string.IsNullOrEmpty(((FMMFile)downloadableMapsList.SelectedItem).ImageFull))
            //        {
            //            Process.Start(((FMMFile)downloadableMapsList.SelectedItem).ImageFull);
            //        }
            //        else if (!string.IsNullOrEmpty(((FMMFile)downloadableMapsList.SelectedItem).Url))
            //        {
            //            Process.Start(((FMMFile)downloadableMapsList.SelectedItem).Url);
            //        }
            //    }
            //}
            //if (mainTabs.SelectedIndex == 2 && gametypesTabs.SelectedIndex == 0)
            //{
            //    if (myGametypesList.SelectedItem != null)
            //    {
            //        if (!string.IsNullOrEmpty(((FMMFile)myGametypesList.SelectedItem).ImageFull))
            //        {
            //            Process.Start(((FMMFile)myGametypesList.SelectedItem).ImageFull);
            //        }
            //        else if (!string.IsNullOrEmpty(((FMMFile)myGametypesList.SelectedItem).Url))
            //        {
            //            Process.Start(((FMMFile)myGametypesList.SelectedItem).Url);
            //        }
            //    }
            //}
            //if (mainTabs.SelectedIndex == 2 && gametypesTabs.SelectedIndex == 1)
            //{
            //    if (downloadableGametypesList.SelectedItem != null)
            //    {
            //        if (!string.IsNullOrEmpty(((FMMFile)downloadableGametypesList.SelectedItem).ImageFull))
            //        {
            //            Process.Start(((FMMFile)downloadableGametypesList.SelectedItem).ImageFull);
            //        }
            //        else if (!string.IsNullOrEmpty(((FMMFile)downloadableGametypesList.SelectedItem).Url))
            //        {
            //            Process.Start(((FMMFile)downloadableGametypesList.SelectedItem).Url);
            //        }
            //    }
            //}
        }

        private void profile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((MenuItem)sender).Header.ToString());
        }

        private void fileAbout_Click(object sender, RoutedEventArgs e)
        {
            Window aboutWind = new About();
            aboutWind.Owner = this;
            aboutWind.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (installLogGrid.Visibility == Visibility.Visible && closeLogButton.Visibility == Visibility.Collapsed)
            {
                string sMessageBoxText = "FMM is currently working and quitting while performing file operations or mod installations can be dangerous.\nAre you sure you want to quit?";
                string sCaption = "Foundation Mod Manager";
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(Application.Current.MainWindow, sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        e.Cancel = false;
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        //private void infobarSBConnect_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Process ed = new Process();
        //        ed.StartInfo.FileName = "eldorado.exe";
        //        ed.StartInfo.Arguments = "--connect " + ((Server)serverBrowserList.SelectedItem).ipAddress;
        //        ed.Start();
        //    }
        //    catch
        //    {
        //        MessageBox.Show(Application.Current.MainWindow, "Could not open eldorado.exe.", "Foundation Mod Manager", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}
