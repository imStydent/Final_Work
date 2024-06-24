using FragrantWorld.Classes;
using System.Windows;
using System.Windows.Controls;

namespace FragrantWorld
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            productNameTextBox.Text = string.Empty;
            productArticleTextBox.Text = string.Empty;
            productDescriptionTextBox.Text = string.Empty;
            productCategoryTextBox.Text = string.Empty;
            productManufacturerTextBox.Text = string.Empty;
            productCostTextBox.Text = string.Empty;
            productDiscountTextBox.Text = string.Empty;
            productQuantityInStockTextBox.Text = string.Empty;
            productStatusTextBox.Text = string.Empty;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new()
                {
                    ArticleNumber = productArticleTextBox.Text,
                    Name = productNameTextBox.Text.ToString(),
                    Description = productDescriptionTextBox.Text,
                    Category = productCategoryTextBox.Text,
                    Manufacturer = productManufacturerTextBox.Text,
                    Cost = Convert.ToDouble(productCostTextBox.Text),
                    DiscountAmount = Convert.ToInt32(productDiscountTextBox.Text),
                    QuantityInStock = Convert.ToInt32(productQuantityInStockTextBox.Text),
                    Status = productStatusTextBox.Text
                };
                DataAccessLayer.AddProduct(product);

                MessageBox.Show("Товар успешно создан", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch when (DataAccessLayer.GetArticleNumber().Contains(productArticleTextBox.Text))
            {
                MessageBox.Show("Данный артикул уже занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBox.Show("Не удалось создать товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы хотите закрыть окно создателя товаров", "Закрытие", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
