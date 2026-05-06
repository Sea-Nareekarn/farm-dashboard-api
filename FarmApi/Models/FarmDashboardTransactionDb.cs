using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmApi.Models;

[Table("farm_dashboard_transaction_db")]
public class FarmDashboardTransactionDb
{
    [Key]
    [Column("transaction_id")]
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    [Column("transaction_date")]
    public DateTime TransactionDate { get; set; }

    [Column("type_code")]
    public string? TypeCode { get; set; } 

    [Column("category_code")]
    public string? CategoryCode { get; set; } 

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } 

    [Column("updated_by")]
    public string? UpdatedBy { get; set; }
}