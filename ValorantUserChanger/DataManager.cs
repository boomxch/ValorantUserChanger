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
        public string application_path { get; set; }
        public string setting_folder_path { get; set; }
        public List<DetailUserData> user_data { get; set; }

        // expire情報について
        public class DetailUserData
        {
            public string user_name { get; set; }
            public string yaml_data { get; set; }
        }
    }

    public static class DataManager
    {
        private const string filePath = "data/data.xml";

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
                var sw = new StreamWriter(filePath, false, new UTF8Encoding(false));
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
            if (!File.Exists(filePath)) return new UserData();

            var serializer = new XmlSerializer(typeof(UserData));
            var sr = new StreamReader(filePath, new UTF8Encoding(false));
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
