using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosApi.DataModels.DataModels;

[Table("product_types")]
public class ProductTypes : EntityBase
{
    [Key]
    [Column("id")][MaxLength(36)] public string Id { get; set; }
    [Column("product_type_name")][MaxLength(255)] public string ProductTypeName { get; set; }
    [Column("product_type_image_path")][MaxLength(255)] public string ProductTypeImagePath { get; set; }
}