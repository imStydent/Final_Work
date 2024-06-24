namespace FragrantWorld.Classes
{
    public class Product
    {
        public string ArticleNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public double Cost { get; set; }
        public int DiscountAmount { get; set; }
        public int QuantityInStock { get; set; }
        public string Status { get; set; }
        public double CostWithDiscount
        {
            get
            {
                if (DiscountAmount > 0)
                    return Cost * (1 - DiscountAmount * 0.01);
                return Cost;
            }
            set { }
        }
        public string Color
        {
            get
            {
                return DiscountAmount >= 15 ? "#7fff00" : "White";
            }
        }
        public string CostConverter
        {
            get
            {
                if (Cost == CostWithDiscount)
                    return "";
                return Cost.ToString();
            }
            set { }
        }
    }
}
