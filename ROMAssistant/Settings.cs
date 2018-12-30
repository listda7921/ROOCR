using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant
{
    public static class Configurator
    {
        public static Settings LoadConfig(string fileName)
        {
            String json = System.IO.File.ReadAllText(fileName);
            Settings config = new Settings();
            config = JsonConvert.DeserializeObject<Settings>(json);
            return config;
        }
        public static void SaveConfig(string fileName, Settings config)
        {
            String json = JsonConvert.SerializeObject(config);
            try
            {
                System.IO.File.WriteAllText(fileName, json);
            }
            finally { }
        }

    }

    public class Settings
    {
        // Default Configuration
        public double[] btnBag = { 0.734, 0.028 };
        public double[] btnMore = { 0.805, 0.028 };
        public double[] btnMVP = { 0.688, 0.456 };
        public double[] btnClose = { 0.973, 0.203 };
        public double[] btnSettings = { 0.852, 0.601 };
        public string windowTitle = "(HW)";
        public int asd = 1;
    }
}
