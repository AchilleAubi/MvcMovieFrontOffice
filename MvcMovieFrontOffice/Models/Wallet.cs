using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.Models;

public class Wallet
{
    public Wallet(int id, int amount, string? userId)
    {
        Id = id;
        Amount = amount;
        UserId = userId;
    }

    public Wallet()
    {
    }

    [Key]
    public int Id { get; set; }

    public int Amount { get; set; }

    public string? UserId { get; set; }
}