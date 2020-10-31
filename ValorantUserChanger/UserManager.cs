using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ValorantUserChanger
{
    public class UserManager
    {
        // 決め打ち
        private const string ApplicationPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Riot Games";
        private readonly string ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                             @"\VALORANT\Saved\Config";
        private readonly string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                             @"\Riot Games\Riot Client\Data";
        private readonly List<(int id, string userName, string guid)> _userDataTuples;

        public UserManager()
        {
            _userDataTuples = new List<(int id, string userName, string guid)>();
            var userData = DataManager.LoadData();

            // data.xml からロード
            foreach (var data in userData.user_data)
                _userDataTuples.Add((_userDataTuples.Count, data.user_name, data.guid));

            // Configに新たなユーザが存在する場合、追加
            foreach (var directory in Directory.GetDirectories(ConfigPath))
            {
                if (!Guid.TryParse(directory, out var guid) || userData.user_data.Select(v => v.guid).Any(v => v == guid.ToString())) continue;

                // guidを名前として追加
                _userDataTuples.Add((_userDataTuples.Count, guid.ToString(), string.Empty));
            }
        }

        public void CopyUserLoginFile(int id)
        {
            var source = Path.GetDirectoryName(DataManager.FilePath) + _userDataTuples[id].guid + ".yaml";
            var dest = DataPath + @"\RiotClientPrivateSettings.yaml";
            File.Copy(source, dest, true);
        }

        public void ChangeUserName(int id, string newUserName)
        {
            _userDataTuples[id] = (id, newUserName, _userDataTuples[id].guid);
            SaveUserData();
        }

        // 管轄外？ アプリケーションのスタート
        public void StartGame()
        {
            var proc = new Process { StartInfo = { FileName = ApplicationPath } };
            proc.Start();
        }

        public IReadOnlyCollection<(int id, string userName, string guid)> GetUserData()
        {
            return _userDataTuples;
        }

        // 変更があった際にセーブする
        private void SaveUserData()
        {
            var userData = new UserData { user_data = new List<UserData.DetailUserData>() };
            foreach (var detailUserData in _userDataTuples.Select(userDataTuple => new UserData.DetailUserData
            {
                user_name = userDataTuple.userName,
                guid = userDataTuple.guid
            }))
                userData.user_data.Add(detailUserData);

            DataManager.SaveData(userData);
        }
    }
}
