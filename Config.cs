using Newtonsoft.Json;

namespace Proxy
{
    public class Config
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public const string Title = "FireflyProxy";
        public const string ConfigFilePath = "Config.json";

        public static Config LoadConfig(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                var defaultConfig = new Config { IpAddress = "182.92.218.218", Port = 21000 };
                File.WriteAllText(configFilePath, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
            }
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
        }
    }
}