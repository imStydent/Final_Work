using FragrantWorld.Classes;
using System.IO;
using System.Windows;

namespace FragrantWorld
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        double costWithDiscount = 0;
        double totalCost = 0;
        double discount;
        int receiptCode = new Random().Next(100, 1000);
        List<Product> selectedProducts;
        List<PickupPoint> pickupPoints = DataAccessLayer.GetPickupPoint();

        public OrderWindow(List<Product> selectedProducts)
        {
            InitializeComponent();
            pickupPointSelectionComboBox.ItemsSource = pickupPoints;
            receiptCodeTextBlock.Text = receiptCode.ToString();
            this.selectedProducts = selectedProducts;
            productsListBox.ItemsSource = selectedProducts;

            foreach (var product in selectedProducts)
            {
                costWithDiscount += product.CostWithDiscount;
                totalCost += product.Cost;
            }
            discount = (totalCost - costWithDiscount) * 100 / totalCost;

            priceTextBlock.Text += string.Format("{0:C2}", costWithDiscount);

            discountTextBlock.Text += $"{Math.Round(discount, 2)}%";
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer.AddOrder(pickupPointSelectionComboBox.SelectedIndex + 1, receiptCode);
        }

        private void RemoveOrderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            selectedProducts.Remove((Product)productsListBox.SelectedItem);
            productsListBox.Items.Refresh();
        }

        private void SaveTicketButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.SelectedPath;
                string filePath = Path.Combine(selectedFolder, "ticket.txt");

                string orderList = "";
                foreach (Product product in selectedProducts)
                {
                    orderList += $"\n-{product.Name}";
                }

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine($"Дата заказа: {DateTime.Now:yyyy-MM-dd}\n\nСписок товаров: {orderList}\nCумма заказа: {string.Format("{0:C2}", totalCost)}, итоговая скидка: " +
                            $"{Math.Round(discount, 2)}%\nПункт выдачи: {((PickupPoint)pickupPointSelectionComboBox.SelectedItem).Address}\n\nКод получения заказа: {receiptCode}");
                    }
                    MessageBox.Show("Талон успешно сохарнен в файл ticket.txt", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Не удалось создать талон", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

