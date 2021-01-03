using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValorantUserChanger
{
    class GameManager
    {
        private const string ProcessName = "VALORANT-Win64-Shipping";

        // アプリケーションのスタート cmdを経由しないと実行できない?
        public void StartGame()
        {
            var psi = new ProcessStartInfo { RedirectStandardInput = true, FileName = @"cmd.exe" };
            var cmd = Process.Start(psi);

            if (cmd == null) return;

            cmd.StandardInput.WriteLine(@"cd ""C:\Riot Games\Riot Client""");
            cmd.StandardInput.WriteLine(@"""RiotClientServices.exe"" --launch-product=valorant --launch-patchline=live");
            cmd.StandardInput.WriteLine(@"exit");
        }

        public async void StartGame(string userName, string password)
        {
            var psi = new ProcessStartInfo { RedirectStandardInput = true, FileName = @"cmd.exe" };
            var cmd = Process.Start(psi);

            if (cmd == null) return;

            cmd.StandardInput.WriteLine(@"cd ""C:\Riot Games\Riot Client""");
            cmd.StandardInput.WriteLine(@"""RiotClientServices.exe"" --launch-product=valorant --launch-patchline=live");
            cmd.StandardInput.WriteLine(@"exit");

            // アプリケーション情報を取得して入力可能かを見る
            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                var pc = Process.GetProcessesByName("RiotClientServices");
                var pc2 = Process.GetProcessesByName("RiotClientUx");
                if (!pc.Any() || !pc2.Any()) continue;

                await Task.Delay(1000);
                WindowManager.ActiveWindow(pc.First().MainWindowHandle);
                SendKeys.SendWait(userName + "{TAB}" + password + "{ENTER}");
                break;
            }
        }

        public void StopGame()
        {
            var process = Process.GetProcessesByName(ProcessName);
            if (process.Any()) process.First().Kill();
        }
    }
}
