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
                MessageBox.Show("test2");
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
            try
            {
                File.Delete("SharpSvn.dll");
            }
            catch { }
            try
            {
                File.Delete("SharpSvn.UI.dll");
            }
            catch { }
            try
            {
                File.Delete("Newtonsoft.Json.dll");
            }
            catch { }
            try
            {
                File.Delete("INIFileParser.dll");
            }
            catch { }
            File.WriteAllBytes(@"SharpSvn.dll", ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.SharpSvn.dll"));
            File.WriteAllBytes(@"SharpSvn.UI.dll", ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.SharpSvn.UI.dll"));
            File.WriteAllBytes(@"Newtonsoft.Json.dll", ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.Newtonsoft.Json.dll"));
            File.WriteAllBytes(@"INIFileParser.dll", ExtractResource(Assembly.GetExecutingAssembly(), "FMM2.INIFileParser.dll"));
            
        }
    }
}
