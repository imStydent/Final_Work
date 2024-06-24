using FragrantWorld.Classes;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace FragrantWorld
{
    class DataAccessLayer
    {
        public static string ServerName { get; set; } = @"(localdb)\MSSQLLocalDB"; //@"prserver\SQLEXPRESS";
        public static string DatabaseName { get; set; } = "ExamDatabase"; //"ispp2113";
        public static string Login { get; set; } = "ispp2113";
        public static string Password { get; set; } = "2113";
        public static string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new()
                {
                    DataSource = ServerName,
                    //UserID = Login,
                    //Password = Password,
                    InitialCatalog = DatabaseName,
                    TrustServerCertificate = true
                };
                return builder.ConnectionString;
            }
        }

        public static List<Product> GetProduct()
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "USE ExamDatabase SELECT * FROM ExamProduct";
            SqlCommand command = new(query, connection);
            List<Product> products = new();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var product = new Product
                {
                    ArticleNumber = reader["ProductArticleNumber"].ToString(),
                    Name = reader["ProductName"].ToString(),
                    Description = reader["ProductDescription"].ToString(),
                    Category = reader["ProductCategory"].ToString(),
                    Manufacturer = reader["ProductManufacturer"].ToString(),
                    Cost = Convert.ToDouble(reader["ProductCost"]),
                    DiscountAmount = Convert.ToInt32(reader["ProductDiscountAmount"]),
                    QuantityInStock = Convert.ToInt32(reader["ProductQuantityInStock"]),
                    Status = reader["ProductStatus"].ToString()
                };
                products.Add(product);
            }
            return products;
        }

        public static List<string> GetArticleNumber()
        {
            List<Product> products = GetProduct();
            List<string> articleNumbers = new();

            foreach (var product in products)
            {
                articleNumbers.Add(product.ArticleNumber);
            }
            return articleNumbers;
        }

        public static List<Order> GetOrder()
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "USE ExamDatabase SELECT * FROM ExamOrder";
            SqlCommand command = new(query, connection);
            List<Order> orders = new();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var order = new Order
                {
                    Id = Convert.ToInt32(reader["OrderID"]),
                    Status = reader["OrderStatus"].ToString(),
                    Date = (DateTime)reader["OrderDate"],
                    DeliveryDate = (DateTime)reader["OrderDeliveryDate"],
                    PickupPoint = Convert.ToInt32(reader["OrderPickupPoint"]),
                    ReceiptCode = Convert.ToInt32(reader["OrderReceiptCode"]),
                };
                orders.Add(order);
            }
            return orders;
        }

        public static List<PickupPoint> GetPickupPoint()
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "USE ExamDatabase SELECT * FROM ExamPickupPoint";
            SqlCommand command = new(query, connection);
            List<PickupPoint> pickupPoints = new();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var pickupPoint = new PickupPoint
                {
                    Id = Convert.ToInt32(reader["PickupPointID"]),
                    Address = reader["Address"].ToString(),
                    PostCode = reader["PostCode"].ToString(),
                };
                pickupPoints.Add(pickupPoint);
            }
            return pickupPoints;
        }

        public static User GetUser(string userLogin)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "USE ExamDatabase SELECT * FROM ExamUser WHERE UserLogin LIKE @searchText";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@searchText", $"{userLogin}");

            User user = null;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                user = new User
                {
                    Id = Convert.ToInt32(reader["UserID"]),
                    Surname = reader["UserSurname"].ToString(),
                    Name = reader["UserName"].ToString(),
                    Patronymic = reader["UserPatronymic"].ToString(),
                    Login = reader["UserLogin"].ToString(),
                    Password = reader["UserPassword"].ToString(),
                    Role = Convert.ToInt32(reader["UserRole"])
                };
            }
            return user;
        }

        public static string GetUserFullName(int orderId)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "SELECT CONCAT_WS(' ',UserSurname, UserName, UserPatronymic) FROM ExamUser WHERE UserID = @orderId";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            return Convert.ToString(command.ExecuteScalar());
        }

        public static void AddOrder(int pickupPoint, int receiptCode)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "USE ExamDatabase INSERT INTO ExamOrder (OrderStatus, OrderDate, OrderDeliveryDate, OrderPickupPoint, OrderReceiptCode) " +
                $"VALUES (N'Новый', '{DateTime.Now:yyyy-MM-dd}', '{DateTime.Now.AddDays(3):yyyy-MM-dd}', '{pickupPoint}', '{receiptCode}')";
            SqlCommand command = new(query, connection);
            command.ExecuteNonQuery();
        }

        public static void UpdateOrder(Order order)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = $"USE ExamDatabase UPDATE ExamOrder SET OrderStatus = N'{order.Status}', OrderPickupPoint = {order.PickupPoint} WHERE OrderID = {order.Id}";
            SqlCommand command = new(query, connection);
            command.ExecuteNonQuery();
        }

        public static double GetOrderTotalCost(int orderId)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "GetOrderTotalCost";
            SqlCommand command = new(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@orderId", orderId);
            var result = Convert.ToDouble(command.ExecuteScalar());
            return result;
        }

        public static double GetOrderTotalDiscount(int orderId)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = "GetOrderTotalDiscount";
            SqlCommand command = new(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@orderId", orderId);
            var result = Convert.ToInt32(command.ExecuteScalar());
            return result;
        }

        public static void UpdateProduct(Product product)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = $"USE ExamDataBase UPDATE ExamProduct SET ProductName = N'{product.Name}', ProductDescription = N'{product.Description}', " +
                $"ProductCategory = N'{product.Category}', ProductManufacturer = N'{product.Manufacturer}', ProductCost = {product.Cost}, " +
                $"ProductDiscountAmount = {product.DiscountAmount}, ProductQuantityInStock = {product.QuantityInStock}, ProductStatus = N'{product.Status}'" +
                $"WHERE ProductArticleNumber = N'{product.ArticleNumber}'";
            SqlCommand command = new(query, connection);
            command.ExecuteNonQuery();
        }

        public static void DeleteProduct(Product product)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = $"USE ExamDataBase DELETE ExamProduct WHERE ProductArticleNumber = N'{product.ArticleNumber}'";
            SqlCommand command = new(query, connection);
            command.ExecuteNonQuery();
        }

        public static void AddProduct(Product product)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            var query = $"USE ExamDataBase INSERT INTO ExamProduct (ProductArticleNumber, ProductName, ProductDescription, ProductCategory, ProductManufacturer, " +
                $"ProductCost, ProductDiscountAmount, ProductQuantityInStock, ProductStatus) " +
                $"VALUES (N'{product.ArticleNumber}', N'{product.Name}', N'{product.Description}', N'{product.Category}', N'{product.Manufacturer}', " +
                $"'{product.Cost}', '{product.DiscountAmount}', '{product.QuantityInStock}', N'{product.Status}')";
            SqlCommand command = new(query, connection);
            command.ExecuteNonQuery();
        }
    }
}


