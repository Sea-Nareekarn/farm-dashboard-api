using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FarmApi.Models;

[Table("mas_type_db")]
public class MasTypeDb
{
    [Key]
    [Column("group_code")]
    [StringLength(20)]
    public string GroupCode { get; set; } = null!;

    [Key]
    [Column("code")]
    [StringLength(10)]
    public string Code { get; set; } = null!;

    [Column("name_loc")]
    [StringLength(50)]
    public string? NameLoc { get; set; }

    [Column("name_eng")]
    [StringLength(50)]
    public string? NameEng { get; set; }

    [Column("condition_1")]
    [StringLength(50)]
    public string? Condition1 { get; set; }

    [Column("condition_2")]
    [StringLength(50)]
    public string? Condition2 { get; set; }

    [Column("condition_3")]
    [StringLength(50)]
    public string? Condition3 { get; set; }

    [Column("is_active")]
    [StringLength(1)]
    public string IsActive { get; set; } = "Y";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")]
    [StringLength(30)]
    public string? CreatedBy { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_by")]
    [StringLength(30)]
    public string? UpdatedBy { get; set; }
}