using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// オプションタブとヘルプタブを追加して、下の項目を消す
namespace ValorantUserChanger
{
    /// <summary>
    /// !MVVM
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowLogic wl;

        public MainWindow()
        {
            InitializeComponent();

            wl = new WindowLogic();
            Loaded += (s, a) => wl.UpdateUserDataField(UserDataStackPanel, ToggleStyledRadioButton, ToggleStyledRadioButton_MouseRightButtonUp);
            SizeChanged += (s, a) => wl.UpdateUserDataFieldSize(UserDataStackPanel);
        }


        private void UpdateUserButton_Click(object sender, RoutedEventArgs e) => wl.UpdateUserDataField(UserDataStackPanel, ToggleStyledRadioButton, ToggleStyledRadioButton_MouseRightButtonUp);

        private void ChangeUserButton_Click(object sender, RoutedEventArgs e) => wl.ChangeUser(((RadioButton)sender).Tag.ToString());

        private void StartGameButton_Click(object sender, RoutedEventArgs e) => wl.StartGame();

        private void LogoutButton_Click(object sender, RoutedEventArgs e) => wl.Logout();

        private void ChangeSettingFolderMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeApplicationFileMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowVersionInformationMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleStyledRadioButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e) => wl.RbRightButtonDown(UserDataStackPanel, ((RadioButton)sender).Tag.ToString());
    }
}
