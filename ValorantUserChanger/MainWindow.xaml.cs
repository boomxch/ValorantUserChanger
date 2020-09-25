using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using RadioButton = System.Windows.Controls.RadioButton;

// オプションタブとヘルプタブを追加して、下の項目を消す
namespace ValorantUserChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeSettingFileDirectoryMenuClick(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                Description =
                    @"""AppData/Local""フォルダを選択してください",
                RootFolder = Environment.SpecialFolder.Desktop,
                SelectedPath = this.SettingFileDirectoryTextBox.Text,
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog().ToString() != "OK")
            {
                return;
            }

            this.SettingFileDirectoryTextBox.Text = fbd.SelectedPath;
            this.UpdateUserNavigationText(false);
            this.UpdateUserDataField();
            this.GetandSavePathData();
        }

        private void ChangeAppFileDirectoryMenuClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                FileName = "RainbowSix.exe",
                InitialDirectory = this.AppFileDirectoryTextBox.Text,
                Filter = @"EXEファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*",
                FilterIndex = 1,
                Title = @"Valorantのアプリケーションファイルを選択してください",
                RestoreDirectory = true,
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (ofd.ShowDialog().ToString() != "OK")
            {
                return;
            }

            this.AppFileDirectoryTextBox.Text = ofd.FileName;
            this.GetandSavePathData();
        }

        /// <summary>
        /// パスを取得し、そのままそのデータを保存
        /// </summary>
        private void GetandSavePathData()
        {
            FileController.SaveApplicationDirectoryInfo(this.GetPathData(), this.ItemPropertyName, ServerChangerTextContainer.SettingItemsName);
        }

        /// <summary>
        /// ユーザー名が記載されたスタックパネルの要素を削除、与えられたパス(SettingFileDirectoryTextBox.Text)を元にユーザー名を新規追加する
        /// </summary>
        private void UpdateUserDataField()
        {
            if (this.UserDataStackPanel.Children.Count > 0)
            {
                this.UserDataStackPanel.Children.Clear();
            }

            var dir = this.SettingFileDirectoryTextBox.Text;
            var userNames = FileController.GetUserNames(dir);

            foreach (var item in userNames)
            {
                var rb = new RadioButton
                {
                    Style = this.ToggleStyledRadioButton.Style,
                    Template = this.ToggleStyledRadioButton.Template,
                    Tag = item,

                    // 画像サイズ:200*160
                    Width = this.UserDataStackPanel.ActualWidth / 4,
                    Height = this.UserDataStackPanel.ActualWidth / 5,
                    FontSize = this.UserDataStackPanel.ActualWidth / 40
                };
                if (this.UserData[item][this.UserDataPropertyName[0]] != string.Empty)
                {
                    rb.Content = this.UserData[item][this.UserDataPropertyName[0]];
                }
                else
                {
                    rb.Content = item.Substring(0, 12) + "...";
                }

                rb.Checked += this.RbChecked;
                rb.MouseRightButtonDown += this.RbRightButtonDown;
                this.UserDataStackPanel.Children.Add(rb);
            }
        }

        // ユーザデータ右クリック
        private void RbRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rb = (RadioButton)sender;
            var userName = rb.Tag.ToString();
            var udiw = new UserDataInputWindow
            {
                Title = "データ管理 -" + rb.Tag.ToString() + "-",
                Tag = userName,
                Owner = this
            };

            this.InitializeUserDataInputWindow(userName, udiw);

            udiw.UserDataSaveButton.Click += this.UserDataSaveButtonClick;
            udiw.Show();
        }

        private void UpdateUserNavigationText(bool isServerChanged)
        {
            this.UserNavigateTextBlock.Text = R6SServerChangerTextContainer.GetNavigationText(
                this.CheckIsUserDataSelected(),
                this.CheckIsServerListSelected(),
                isServerChanged);
        }
    }
}
