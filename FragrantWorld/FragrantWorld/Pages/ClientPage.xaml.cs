using FragrantWorld.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FragrantWorld.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        List<Product> selectedProducts = new();
        List<Product> products = DataAccessLayer.GetProduct();

        public ClientPage(string userFullname)
        {
            InitializeComponent();
            sortProductsComboBox.SelectedIndex = 0;
            discountRangeComboBox.SelectedIndex = 0;
            userInfoTextBlock.Text = userFullname;
            products = products.OrderBy(product => product.CostWithDiscount).ToList();
            productsListBox.Items.Refresh();
            productsListBox.ItemsSource = products;
        }

        private void ExitSystemButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы хотите выйти из системы", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                NavigationService.GoBack();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
                hintSearchTextBlock.Visibility = Visibility.Visible;
            else
                hintSearchTextBlock.Visibility = Visibility.Collapsed;

            List<Product> searchedProducts = new();
            foreach (var product in products)
            {
                if (product.Name.Contains(searchTextBox.Text))
                    searchedProducts.Add(product);
            }
            countProductsTextBlock.Text = $"{searchedProducts.Count} / {DataAccessLayer.GetProduct().Count}";
            productsListBox.Items.Refresh();
            productsListBox.ItemsSource = searchedProducts;
        }

        private void SortProductsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortProductsComboBox.SelectedIndex == 0)
                products = products.OrderBy(product => product.CostWithDiscount).ToList();
            else
                products = products.OrderByDescending(product => product.CostWithDiscount).ToList();
            productsListBox.Items.Refresh();
            productsListBox.ItemsSource = products;
        }

        private void DiscountRangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            products = DataAccessLayer.GetProduct();
            switch (discountRangeComboBox.SelectedIndex)
            {
                case 0:
                    products = DataAccessLayer.GetProduct();
                    break;
                case 1:
                    products = products.Where(order => order.DiscountAmount >= 0 && order.DiscountAmount < 10).OrderBy(product => product.CostWithDiscount).ToList();
                    break;
                case 2:
                    products = products.Where(order => order.DiscountAmount >= 10 && order.DiscountAmount < 15).OrderBy(product => product.CostWithDiscount).ToList();
                    break;
                case 3:
                    products = products.Where(order => order.DiscountAmount >= 15).OrderBy(product => product.CostWithDiscount).ToList();
                    break;
            }
            sortProductsComboBox.SelectedIndex = 0;
            countProductsTextBlock.Text = $"{products.Count} / {DataAccessLayer.GetProduct().Count}";
            productsListBox.Items.Refresh();
            productsListBox.ItemsSource = products;
        }

        private void AddOrderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedProducts.Add(products[productsListBox.SelectedIndex]);
                showOrderButton.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Не удалось добавить товар в заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow(selectedProducts);
            orderWindow.userFullnameTextBlock.Text = userInfoTextBlock.Text;
            orderWindow.ShowDialog();
            

            if (!orderWindow.productsListBox.HasItems)
                showOrderButton.Visibility = Visibility.Collapsed;
        }
    }
}
