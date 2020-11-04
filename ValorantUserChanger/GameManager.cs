using System.Diagnostics;
using System.Linq;

namespace ValorantUserChanger
{
    class GameManager
    {
        private const string ApplicationPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Riot Games";

        // アプリケーションのスタート
        public void StartGame()
        {
            var proc = new Process { StartInfo = { FileName = ApplicationPath } };
            proc.Start();
        }

        public void StopGame()
        {
            var process = Process.GetProcessesByName("VALORANT");
            if (process.Any()) process.First().Kill();
        }
    }
}
