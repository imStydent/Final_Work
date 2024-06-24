﻿using FragrantWorld.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FragrantWorld.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        List<Product> selectedProducts = new();
        List<Product> products = DataAccessLayer.GetProduct();

        public AdminPage(string userFullname)
        {
            InitializeComponent();
            discountRangeComboBox.SelectedIndex = 0;
            sortProductsComboBox.SelectedIndex = 0;
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

        private void SortProductsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortProductsComboBox.SelectedIndex == 0)
                products = products.OrderBy(product => product.CostWithDiscount).ToList();
            else
                products = products.OrderByDescending(product => product.CostWithDiscount).ToList();
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
            var orderWindow = new Windows.OrderWindow(selectedProducts);
            orderWindow.userFullnameTextBlock.Text = userInfoTextBlock.Text;
            orderWindow.ShowDialog();
            if (!orderWindow.productsListBox.HasItems)
                showOrderButton.Visibility = Visibility.Collapsed;
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var editOrderWindow = new Windows.EditOrderWindow();
            editOrderWindow.ShowDialog();
            products = DataAccessLayer.GetProduct();
            productsListBox.ItemsSource = products;
        }

        private void EditProductMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var editProductWindow = new Windows.EditProductWindow(products[productsListBox.SelectedIndex]);
            editProductWindow.ShowDialog();
            products = DataAccessLayer.GetProduct();
        }

        private void AddProductMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new Windows.AddProductWindow();
            addProductWindow.ShowDialog();
            products = DataAccessLayer.GetProduct();
            productsListBox.ItemsSource = products;
        }

        private void DeleteProductMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы хотите удалить товар?", "Удалить", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    DataAccessLayer.DeleteProduct(products[productsListBox.SelectedIndex]);
                    products = DataAccessLayer.GetProduct();
                    productsListBox.ItemsSource = products;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить товар из списка (товар находится в списке заказов)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
