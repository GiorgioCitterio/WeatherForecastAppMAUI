using Microsoft.Win32;
using System.Net;
using System.Runtime.InteropServices;

namespace SettingProxy
{
    public class MyProxy
    {
        public static void HttpClientProxySetup(out HttpClient client)
        {
            Uri proxy;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string userProxy = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings").GetValue("ProxyServer") as string;
                int? proxyEnable = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings").GetValue("ProxyEnable") as int?;
                proxy = proxyEnable > 0 ? new Uri(userProxy) : null;
            }
            else
            {
                Uri destinationUri = new Uri("https://www.google.it");
                proxy = HttpClient.DefaultProxy.GetProxy(destinationUri);
            }

            HttpClientHandler httpHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(proxy, true),
                UseProxy = true,
                PreAuthenticate = false,
                UseDefaultCredentials = false,
            };
            client = new HttpClient(httpHandler);
        }
    }
}
