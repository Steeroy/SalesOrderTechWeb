namespace SalesOrderTechTestWeb.Models
{
    public class OrdersViewModel
    {
        public string SearchInput { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}
