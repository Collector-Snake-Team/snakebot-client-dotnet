namespace Cygni.Snake.Client
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public class OpenBrowser : IGameObserver
    {
        public void OnSnakeDied(string reason, string snakeId)
        {
        }

        public void OnGameStart()
        {
        }

        public void OnGameEnd(Map map)
        {
        }

        public void OnUpdate(Map map)
        {
        }

        public void OnGameLink(string url)
        {
            OpenWebBrowser(url);
        }

        private void OpenWebBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }

}