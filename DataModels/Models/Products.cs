using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosApi.DataModels.DataModels;

[Table("products")]
public class Products : EntityBase
{
    [Key]
    [Column("id")][MaxLength(36)] public string Id { get; set; }
    [Column("product_types_id")][MaxLength(36)] public string ProductTypesId { get; set; }
    [Column("product_code")][MaxLength(10)] public string ProductCode { get; set; }
    [Column("product_name")][MaxLength(255)] public string ProductName { get; set; }
    [Column("product_barcode")][MaxLength(255)] public string ProductBarcode { get; set; }
    [Column("product_sale_price")] public double ProductSalePrice { get; set; }
    [Column("unit_name")] [MaxLength(50)] public string UnitName { get; set; }
    [Column("product_image_path")][MaxLength(255)] public string ProductImagePath { get; set; }
}