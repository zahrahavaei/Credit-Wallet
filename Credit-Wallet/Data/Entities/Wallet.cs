using System.ComponentModel.DataAnnotations;

namespace Credit_Wallet.Data.Entities;

public class Wallet
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    public decimal Balance { get; set; }
    public DateTime LastUpdateDateTime { get; set; } = DateTime.Now;

    [Timestamp]
    public byte[] RowVersion { get; set; }
}