using System;
using System.IO;
using Newtonsoft.Json;
namespace TVNK
{
    public class ConfigService
    {
        public ConfigModel settings;

        public ConfigService()
        {
            ReadFromDisk();
        }

        public void ReadFromDisk()
        {
            try
            {
                using (StreamReader file = File.OpenText(@"config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    settings = (ConfigModel)serializer.Deserialize(file, typeof(ConfigModel));
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message + " Config not found, generating...");
                settings = new ConfigModel();
                SaveToDisk();
            }
        }

        public void SaveToDisk()
        {
            if (settings == null)
                settings = new ConfigModel();
            using (StreamWriter file = File.CreateText(@"config.json"))
            {
                
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, settings);
            }
        }
    }
}
