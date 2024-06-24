using FragrantWorld.Classes;
using System.Windows;
using System.Windows.Controls;

namespace FragrantWorld
{
    /// <summary>
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        List<PickupPoint> pickupPoints = DataAccessLayer.GetPickupPoint();
        List<Order> orders;

        public EditOrderWindow()
        {
            InitializeComponent();

            orders = DataAccessLayer.GetOrder();
            addressComboBox.ItemsSource = pickupPoints;
            ordersListBox.ItemsSource = orders;

            sortOrderComboBox.SelectedIndex = 0;
            orderDiscountRangeComboBox.SelectedIndex = 0;
        }

        private void OrderDiscountRangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orders = DataAccessLayer.GetOrder();
            switch (orderDiscountRangeComboBox.SelectedIndex)
            {
                case 0:
                    orders = DataAccessLayer.GetOrder();
                    break;
                case 1:
                    orders = orders.Where(order => order.TotalDiscount >= 0 && order.TotalDiscount < 10).ToList();
                    break;
                case 2:
                    orders = orders.Where(order => order.TotalDiscount >= 10 && order.TotalDiscount < 15).ToList();
                    break;
                case 3:
                    orders = orders.Where(order => order.TotalDiscount >= 15).ToList();
                    break;
            }
            ordersListBox.Items.Refresh();
            ordersListBox.ItemsSource = orders;
        }

        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortOrderComboBox.SelectedIndex == 0)
                orders = orders.OrderBy(order => order.TotalCost).ToList();
            else
                orders = orders.OrderByDescending(order => order.TotalCost).ToList();
            ordersListBox.Items.Refresh();
            ordersListBox.ItemsSource = orders;
        }

        private void OrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = (Order)ordersListBox.SelectedItem;
            if (selectedOrder != null)
                addressComboBox.SelectedItem =
                    pickupPoints.Where(point => point.Id == selectedOrder.PickupPoint).FirstOrDefault();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            orderDiscountRangeComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = (Order)ordersListBox.SelectedItem;
                if (order != null)
                {
                    order.Status = statusTextBox.Text;
                    order.PickupPoint = ((PickupPoint)addressComboBox.SelectedItem).Id;
                    DataAccessLayer.UpdateOrder(order);
                    ordersListBox.Items.Refresh();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось изменить параметры заказа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
