using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Panel = System.Windows.Controls.Panel;
using RadioButton = System.Windows.Controls.RadioButton;

namespace ValorantUserChanger
{
    public class WindowLogic
    {
        private UserManager um;
        private GameManager gm;

        public WindowLogic()
        {
            um = new UserManager();
            gm = new GameManager();
        }

        public void UpdateUserDataField(Panel parentPanel, RadioButton baseRadioButton, MouseButtonEventHandler radioButtonMouseRightButtonUpHandler)
        {
            if (parentPanel.Children.Count > 0)
                parentPanel.Children.Clear();

            um.AddUserLoginFile();

            var userData = um.GetUserData();

            foreach (var (guid, userName, isFileExist) in userData)
            {
                var rb = new RadioButton
                {
                    Style = baseRadioButton.Style,
                    Template = baseRadioButton.Template,
                    Tag = guid,

                    // 画像サイズ:200*160
                    Width = parentPanel.ActualWidth / 4,
                    Height = parentPanel.ActualWidth / 5,
                    Background = isFileExist ? new ImageBrush(new BitmapImage(new Uri(@"Resources/folder.png", UriKind.Relative))) : new ImageBrush(new BitmapImage(new Uri(@"Resources/folder4.png", UriKind.Relative))),
                    FontSize = parentPanel.ActualWidth / 40,
                    Foreground = isFileExist ? Brushes.Black : Brushes.Red,
                    Content = userName.Length > 12 ? userName.Substring(0, 12) + "..." : userName
                };

                rb.MouseRightButtonDown += radioButtonMouseRightButtonUpHandler;
                parentPanel.Children.Add(rb);
            }
        }

        public void UpdateUserDataFieldName(Panel parentPanel)
        {
            var userData = um.GetUserData();
            foreach (RadioButton child in parentPanel.Children)
            {
                var (_, userName, isFileExist) = userData.First(v => v.guid == child.Tag.ToString());
                child.Content = userName;
                child.Foreground = isFileExist ? Brushes.Black : Brushes.Red;
            }
        }

        public void UpdateUserDataFieldSize(Panel parentPanel)
        {
            foreach (RadioButton child in parentPanel.Children)
            {
                child.Width = parentPanel.ActualWidth / 4;
                child.Height = parentPanel.ActualWidth / 5;
                child.FontSize = parentPanel.ActualWidth / 40;
            }
        }

        public void ChangeUser(string tag)
        {
            um.CopyUserLoginFile(tag);
        }

        public void ChangeUserName(string guid, string newUserName)
        {
            um.ChangeUserName(guid, newUserName);
        }

        public void StartGame()
        {
            gm.StartGame();
        }

        public void Logout()
        {
            gm.StopGame();
            um.DisableAutoLogin();
        }

        /// <summary>
        /// ユーザ名を記載したスタックパネルの要素であるラジオボタンのどれかが右クリックされたらUserDataInputWindowを開く
        /// </summary>
        public void RbRightButtonDown(Panel parentPanel, string tag)
        {
            var udiw = new UserDataInputWindow(this)
            {
                Title = "データ管理 -" + tag,
                Tag = tag,
            };

            var userName = um.GetUserNameFromGuid(tag);
            udiw.UserNameTextBox.Text = userName;
            udiw.Closed += (s, a) => UpdateUserDataFieldName(parentPanel);

            udiw.ShowDialog();
        }
    }
}
