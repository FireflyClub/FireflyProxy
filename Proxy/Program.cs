using System.Net;

namespace Proxy {
    internal static class Program {
        private static ProxyService s_proxyService;
        private static readonly ManualResetEvent s_exitEvent = new(false);
        private static bool isShutdown = false;

        private static void Main() {
            Console.Title = Config.Title;
            CheckProxy();

            var ConfigData = Config.LoadConfig(Config.ConfigFilePath);
            s_proxyService = new ProxyService(
                ConfigData.ProxyIp,
                ConfigData.ProxyPort,
                ConfigData.RunPort
            );

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;

            s_exitEvent.WaitOne();
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            if (!isShutdown) {
                Console.WriteLine("Cancel key pressed: Cleaning up...");
                s_proxyService?.Shutdown();
                isShutdown = true;
            }

            e.Cancel = true;
            s_exitEvent.Set();
        }

        private static void OnProcessExit(object sender, EventArgs args) {
            if (!isShutdown) {
                s_proxyService?.Shutdown();
                isShutdown = true;
            }
        }

        public static void CheckProxy() {
            try {
                string proxyInfo = GetProxyInfo();
                if (proxyInfo != null)
                {
                    Console.WriteLine($"Find existed system proxy: {proxyInfo} , press any key to continue...");
                    Console.ReadKey();
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        public static string GetProxyInfo() {
            try {
                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                Uri proxyUri = proxy.GetProxy(new Uri("https://www.example.com"));
                return $"{proxyUri.Host}:{proxyUri.Port}";
            } catch {
                return null;
            }
        }
    }
}
