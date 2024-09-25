using System.Net;

namespace Proxy
{
    internal static class Program
    {
        private static ProxyService s_proxyService;
        private static readonly EventHandler s_processExitHandler = new(OnProcessExit);
        
        private static void Main()
        {
            Console.Title = Config.Title;
            CheckProxy();

            var config = Config.LoadConfig(Config.ConfigFilePath);
            s_proxyService = new ProxyService(config.IpAddress, config.Port);
            AppDomain.CurrentDomain.ProcessExit += s_processExitHandler;

            Thread.Sleep(-1);
        }

        private static void OnProcessExit(object sender, EventArgs args)
        {
            s_proxyService?.Shutdown();
        }

        public static void CheckProxy()
        {
            try
            {
                string ProxyInfo = GetProxyInfo();
                if (ProxyInfo != null)
                {
                    Console.WriteLine($"Find existed system proxy: {ProxyInfo}, press any key to continue...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        public static string GetProxyInfo()
        {
            try
            {
                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                Uri proxyUri = proxy.GetProxy(new Uri("https://www.example.com"));
                return $"{proxyUri.Host}:{proxyUri.Port}";
            }
            catch
            {
                return null;
            }
        }
    }
}