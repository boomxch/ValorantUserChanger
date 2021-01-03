using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ValorantUserChanger
{
    /// <summary>
    /// UserDataInputWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UserDataInputWindow : Window
    {
        private WindowLogic wl;

        public UserDataInputWindow(WindowLogic instance)
        {
            InitializeComponent();

            wl = instance;
        }

        private void UserDataNameInitializeButtonClick(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Text = Tag.ToString();
        }

        private void UserDataSaveButton_Click(object sender, RoutedEventArgs e)
        {
            wl.ChangeUserData(Tag.ToString(), UserNameTextBox.Text, PasswordTextBox.Password);
            Close();
        }
    }
}
