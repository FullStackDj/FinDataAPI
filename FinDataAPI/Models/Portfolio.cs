using System.ComponentModel.DataAnnotations.Schema;

namespace FinDataAPI.Models;

[Table("Portfolio")]
public class Portfolio
{
    public required string AppUserId { get; set; }
    public required int StockId { get; set; }
    public required AppUser AppUser { get; set; }
    public required Stock Stock { get; set; }

}