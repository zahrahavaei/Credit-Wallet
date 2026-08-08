using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credit_Wallet.Data.Entities;

public class Wallet
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    public decimal Balance { get; set; }
    public DateTime LastUpdateDateTime { get; set; } = DateTime.UtcNow;
    /*[Timestamp]
    public byte[] RowVersion { get; set; }
    */
    public Guid RowVersion { get; set; } = Guid.NewGuid();
    public string WalletHash { get; set; } = string.Empty;
}