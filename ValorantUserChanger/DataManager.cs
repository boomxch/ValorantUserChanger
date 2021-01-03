using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace ValorantUserChanger
{
    public class UserData
    {
        public List<DetailUserData> user_data { get; set; }

        // BASE64エンコードでyamlファイルを内部に保持してもよいが、直接yamlファイルとして保持してたほうが楽そう
        // expire情報についてどうするか
        public class DetailUserData
        {
            public string user_name { get; set; }
            public string password { get; set; }
            public string guid { get; set; }
        }

        public UserData()
        {
            user_data = new List<DetailUserData>();
        }
    }

    public static class DataManager
    {
        public const string FilePath = "data/data.xml";

        internal static void SaveData(UserData data)
        {
            var xmlSerializer = new XmlSerializer(typeof(UserData));

            try
            {
                if (!Directory.Exists("data"))
                    Directory.CreateDirectory("data");
            }
            catch (Exception)
            {
                MessageBox.Show("dataフォルダを作成することが出来ませんでした。");
            }

            try
            {
                var sw = new StreamWriter(FilePath, false, new UTF8Encoding(false));
                xmlSerializer.Serialize(sw, data);
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("ユーザー情報を保存することができませんでした。");
            }
        }

        internal static UserData LoadData()
        {
            try
            {
                if (!Directory.Exists("data"))
                    Directory.CreateDirectory("data");
            }
            catch (Exception)
            {
                MessageBox.Show("dataフォルダを作成することが出来ませんでした。");
            }

            if (!File.Exists(FilePath))
                return new UserData();

            var serializer = new XmlSerializer(typeof(UserData));
            var sr = new StreamReader(FilePath, new UTF8Encoding(false));
            var data = new UserData();
            try
            {
                data = (UserData)serializer.Deserialize(sr);
            }
            catch (System.Windows.Markup.XamlParseException)
            {
                MessageBox.Show("data.xmlの形式が正しくありません。", "無効なdata.xml", MessageBoxButton.OK);
            }
            finally
            {
                sr.Close();
            }

            return data;
        }
    }
}
