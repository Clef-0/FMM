using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FMM2
{
    public partial class MainWindow : Window
    {
        public static byte[] ExtractResource(Assembly assembly, string resourceName)
        {
            if (assembly == null)
            {
                return null;
            }

            using (Stream resFilestream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resFilestream == null)
                {
                    MessageBox.Show(string.Join(",",assembly.GetManifestResourceNames()));
                    return null;
                }

                byte[] bytes = new byte[resFilestream.Length];
                resFilestream.Read(bytes, 0, bytes.Length);

                return bytes;
            }
        }

        private void ExtractDLLs()
        {
            if (File.Exists(Path.Combine("FMM", "lib", "SharpSvn.dll")))
            {
                File.Delete(Path.Combine("FMM", "lib", "SharpSvn.dll"));
            }

            if (File.Exists(Path.Combine("FMM", "lib", "SharpSvn.UI.dll")))
            {
                File.Delete(Path.Combine("FMM", "lib", "SharpSvn.UI.dll"));
            }
            if (File.Exists(Path.Combine("FMM", "lib", "Newtonsoft.Json.dll")))
            {
                File.Delete(Path.Combine("FMM", "lib", "Newtonsoft.Json.dll"));
            }
            if (File.Exists(Path.Combine("FMM", "lib", "INIFileParser.dll")))
            {
                File.Delete(Path.Combine("FMM", "lib", "INIFileParser.dll"));
            }
            Directory.CreateDirectory(Path.Combine("FMM", "lib"));
            File.WriteAllBytes(Path.Combine("FMM", "lib", "SharpSvn.dll"), ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.SharpSvn.dll"));
            File.WriteAllBytes(Path.Combine("FMM", "lib", "SharpSvn.UI.dll"), ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.SharpSvn.UI.dll"));
            File.WriteAllBytes(Path.Combine("FMM", "lib", "Newtonsoft.Json.dll"), ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.Newtonsoft.Json.dll"));
            File.WriteAllBytes(Path.Combine("FMM", "lib", "INIFileParser.dll"), ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.INIFileParser.dll"));
        }
    }
}
