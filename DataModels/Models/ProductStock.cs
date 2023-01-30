using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosApi.DataModels.DataModels;

[Table("product_stocks")]
public class ProductStocks : EntityBase
{
    [Key]
    [Column("id")][MaxLength(36)] public string Id { get; set; }
    [Column("products_id")][MaxLength(36)] public string ProductsId { get; set; }
    [Column("receive_date")] public DateTime ReceiveDate { get; set; }
    [Column("receive_quantity")] public double ReceiveQuantity { get; set; }
    [Column("remain_quantity")] public double RemainQuantity { get; set; }
    [Column("cost_price")] public double CostPrice { get; set; }
}