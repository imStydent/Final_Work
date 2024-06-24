using FragrantWorld.Classes;
using System.Windows;
using System.Windows.Controls;

namespace FragrantWorld
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public EditProductWindow(Product selectedProduct)
        {
            InitializeComponent();

            productNameTextBox.Text = selectedProduct.Name;
            productDescriptionTextBox.Text = selectedProduct.Description;
            productCategoryTextBox.Text = selectedProduct.Category;
            productManufacturerTextBox.Text = selectedProduct.Manufacturer;
            productCostTextBox.Text = selectedProduct.Cost.ToString();
            productDiscountTextBox.Text = selectedProduct.DiscountAmount.ToString();
            productQuantityInStockTextBox.Text = selectedProduct.QuantityInStock.ToString();
            productStatusTextBox.Text = selectedProduct.Status;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new()
                {
                    Name = productNameTextBox.Text.ToString(),
                    Description = productDescriptionTextBox.Text,
                    Category = productCategoryTextBox.Text,
                    Manufacturer = productManufacturerTextBox.Text,
                    Cost = Convert.ToInt32(productCostTextBox.Text),
                    DiscountAmount = Convert.ToInt32(productDiscountTextBox.Text),
                    QuantityInStock = Convert.ToInt32(productQuantityInStockTextBox.Text),
                    Status = productStatusTextBox.Text
                };
                DataAccessLayer.UpdateProduct(product);

                MessageBox.Show("Изменения успешно записаны", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось изменить товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            productNameTextBox.Text = string.Empty;
            productDescriptionTextBox.Text = string.Empty;
            productCategoryTextBox.Text = string.Empty;
            productManufacturerTextBox.Text = string.Empty;
            productCostTextBox.Text = string.Empty;
            productDiscountTextBox.Text = string.Empty;
            productQuantityInStockTextBox.Text = string.Empty;
            productStatusTextBox.Text = string.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы хотите закрыть окно редактора товаров", "Закрытие", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
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
