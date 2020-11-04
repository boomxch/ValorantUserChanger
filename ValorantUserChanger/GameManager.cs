using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

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

        public void StopGame()
        {
            var process = Process.GetProcessesByName(ProcessName);
            if (process.Any()) process.First().Kill();
        }
    }
}
