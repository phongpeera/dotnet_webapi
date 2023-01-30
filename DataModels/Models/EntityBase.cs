using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosApi.DataModels.DataModels;

public class EntityBase
{
    [Column("create_date")] public DateTime CreateDate { get; set; }
    [Column("update_date")] public DateTime? UpdateDate { get; set; }
    [Column("create_by")][MaxLength(36)] public string CreateBy { get; set; }
    [Column("update_by")][MaxLength(36)] public string UpdateBy { get; set; }
    [Column("is_deleted")] public bool IsDeleted { get; set; }
}