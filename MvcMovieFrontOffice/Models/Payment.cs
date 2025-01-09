using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.Models;

public class Payment
{
    public Payment(int id, int reservationId, int amount, DateTime paymentDate, string paymentMethod, string? userId)
    {
        Id = id;
        ReservationId = reservationId;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod;
        UserId = userId;
    }

    public Payment()
    {
    }

    [Key]
    public int Id { get; set; }

    public int ReservationId { get; set; }

    public int Amount { get; set; }
    
    public DateTime PaymentDate { get; set; }
    
    public string PaymentMethod { get; set; }

    public string? UserId { get; set; }
}