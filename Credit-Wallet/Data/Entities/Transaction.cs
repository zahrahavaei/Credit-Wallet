using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credit_Wallet.Data.Entities;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; } = null!;
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
   
    public DateTime CreatedDateTime { get; set; }= DateTime.UtcNow;
    public string TransactionHash { get; set; }= string.Empty;
}

public enum TransactionType
{
    Deposit = 1,
    Withdraw = 2
}