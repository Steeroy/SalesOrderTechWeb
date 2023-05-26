using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesOrderTechTestWeb.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        
        [Column(TypeName ="nvarchar(20)")]
        [DisplayName("Order Number")]
        [Required]
        public string OrderNumber { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [DisplayName("Order Type")]
        [Required]
        public string OrderType { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [DisplayName("Order Status")]
        [Required]
        public string OrderStatus { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Customer Name")]
        [Required]
        public string CustomerName { get; set; }

        [DisplayName("Order Date")]
        [Required]
        public DateTime OrderDate { get; set; }

        public virtual List<OrderLine> OrderLines { get; set; }

    }
}
