using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ValorantUserChanger
{
    // データベース操作
    // CRUDをしっかり行っていると仮定して、_userDataTuple内のデータが存在しない場合を無くし、例外処理を省く
    public class UserManager
    {
        // 決め打ち
        private const string AutoLoginFileName = @"\RiotClientPrivateSettings.yaml";
        private readonly string ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                             @"\VALORANT\Saved\Config";
        private readonly string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                             @"\Riot Games\Riot Client\Data";
        private readonly List<(string guid, string userName, string password, bool isFileExist)> _userDataTuples;

        public UserManager()
        {
            _userDataTuples = new List<(string guid, string userName, string password, bool isFileExist)>();
            var userData = DataManager.LoadData();

            // data.xml からロード
            foreach (var data in userData.user_data)
            {
                var path = Path.GetDirectoryName(DataManager.FilePath) + @"\" + data.guid + ".yaml";
                _userDataTuples.Add((data.guid, data.user_name, data.password, File.Exists(path)));
            }

            // Configに新たなユーザが存在する場合、追加
            foreach (var directory in Directory.GetDirectories(ConfigPath))
            {
                var folderName = Path.GetFileNameWithoutExtension(directory);
                if (!Guid.TryParse(folderName, out var guid) || userData.user_data.Select(v => v.guid).Any(v => v == guid.ToString())) continue;

                // guidを名前として追加
                _userDataTuples.Add((guid.ToString(), guid.ToString(), "", false));
            }
        }

        public void AddUserLoginFile()
        {
            var source = DataPath + AutoLoginFileName;
            if (!File.Exists(source)) return;

            // yamlファイルからguidを抜き出す(どのユーザのデータかを判定)
            var yaml = File.ReadAllLines(source).Aggregate((v, d) => v + d);
            var match = Regex.Match(yaml, @"""(?<guid>\w{8}-\w{4}-\w{4}-\w{4}-\w{12})""");
            if (match.Groups["guid"].Captures.Count == 0) return;

            var guid = match.Groups["guid"].Captures.First().Value;

            var dest = Path.GetDirectoryName(DataManager.FilePath) + @"\" + guid + ".yaml";
            File.Copy(source, dest, true);

            var id = _userDataTuples.FindIndex(v => v.guid == guid);
            _userDataTuples[id] = (_userDataTuples[id].guid, _userDataTuples[id].userName, _userDataTuples[id].password, true);

            SaveUserData();
        }

        public void CopyUserLoginFile(string guid)
        {
            if (!_userDataTuples.Any(v => v.guid == guid && v.isFileExist)) return;

            var source = Path.GetDirectoryName(DataManager.FilePath) + @"\" + guid + ".yaml";
            var dest = DataPath + AutoLoginFileName;
            File.Copy(source, dest, true);
        }

        public void ChangeUserData(string guid, string userName, string password)
        {
            var id = _userDataTuples.FindIndex(v => v.guid == guid);
            _userDataTuples[id] = (_userDataTuples[id].guid, userName, password, _userDataTuples[id].isFileExist);
            SaveUserData();
        }

        public void DisableAutoLogin()
        {
            File.Delete(DataPath + AutoLoginFileName);
        }

        public IReadOnlyCollection<(string guid, string userName, string password, bool isFileExist)> GetUserData()
        {
            return _userDataTuples;
        }

        public string GetUserNameFromGuid(string guid)
        {
            var data = _userDataTuples.Where(v => v.guid == guid);
            return data.Any() ? data.First().userName : string.Empty;
        }

        public string GetUserPasswordFromGuid(string guid)
        {
            var data = _userDataTuples.Where(v => v.guid == guid);
            return data.Any() ? data.First().password : string.Empty;
        }

        // 変更があった際にセーブする
        private void SaveUserData()
        {
            var userData = new UserData { user_data = new List<UserData.DetailUserData>() };
            foreach (var detailUserData in _userDataTuples.Select(userDataTuple => new UserData.DetailUserData
            {
                user_name = userDataTuple.userName,
                password = userDataTuple.password,
                guid = userDataTuple.guid
            }))
                userData.user_data.Add(detailUserData);

            DataManager.SaveData(userData);
        }
    }
}
