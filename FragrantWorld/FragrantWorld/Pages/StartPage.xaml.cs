using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FragrantWorld.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Classes.User currentUser = DataAccessLayer.GetUser(loginTextBox.Text);
                if (currentUser != null)
                {
                    if (currentUser.Password == passwordTextBox.Text)
                    {
                        switch (currentUser.Role)
                        {
                            case 1:
                                NavigationService.Navigate(new AdminPage(currentUser.Fullname));
                                break;
                            case 2:
                                NavigationService.Navigate(new ManagerPage(currentUser.Fullname));
                                break;
                            default:
                                NavigationService.Navigate(new ClientPage(currentUser.Fullname));
                                break;
                        }
                    }
                    else
                        MessageBox.Show("Неверный пароль", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    MessageBox.Show("Неверный логин", "Некорректные данные", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch
            {
                MessageBox.Show("Не удалось получить данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginGuestButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage("Гость"));
        }
    }
}
