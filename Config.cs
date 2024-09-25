using Newtonsoft.Json;

namespace Proxy
{
    public class Config
    {
        public const string Title = "FireflyProxy";
        public const string ConfigFilePath = "Config.json";

        public int RunPort { get; set; }
        public string ProxyIp { get; set; }
        public int ProxyPort { get; set; }

        public static Config LoadConfig(string configFilePath)
        {
            if (!File.Exists(configFilePath)) {
                var Default = new Config {
                    RunPort = 1337,
                    ProxyIp = "127.0.0.1",
                    ProxyPort = 619
                };

                File.WriteAllText(configFilePath, JsonConvert.SerializeObject(Default, Formatting.Indented));

            }

            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
        }
    }
}