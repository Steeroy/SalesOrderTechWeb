using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesOrderTechTestWeb.Models
{
    public class OrderLine
    {
        [Key]
        public int LineId { get; set; }

        public int LineNum { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [DisplayName("Product Type")]
        public string ProductType { get; set; }

        [DisplayName("Product Cost Price")]
        public decimal ProductCostPrice { get; set; }

        [DisplayName("Product Sales Price")]
        public decimal ProductSalesPrice { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }


        public int OrderId { get; set; }

    }
}
