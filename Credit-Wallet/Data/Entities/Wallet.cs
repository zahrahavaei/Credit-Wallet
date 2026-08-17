using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credit_Wallet.Data.Entities;

public class Wallet
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    public decimal Balance { get; private set; } = 0;
    public DateTime LastUpdateDateTime { get; set; } = DateTime.UtcNow;
    /*[Timestamp]
    public byte[] RowVersion { get; set; }
    */
    public Guid RowVersion { get; set; } = Guid.NewGuid();
    public string WalletHash { get; set; } = string.Empty;

    public void Deposit(decimal amount)
    {
        if (amount<0)
        {
            throw new ArgumentException("Deposit amount cannot be negative.");
        }
        Balance += amount;
    }
    public void Withdraw(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Withdrawal amount cannot be negative.");
        }
        if (amount > Balance)
        {
            throw new InvalidOperationException("Insufficient balance for withdrawal.");
        }
        Balance -= amount;
    }
}