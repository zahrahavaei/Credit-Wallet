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

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime LastUpdateDateTime { get; set; } 
}