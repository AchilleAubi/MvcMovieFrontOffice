using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.Models;

public class ReservationView
{
    public ReservationView(int reservationId, DateTime startDate, DateTime endDate, string status, int totalPrice, int vehicleId, string model, string make, bool availability, string userId, string fullName, string email)
    {
        ReservationId = reservationId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        TotalPrice = totalPrice;
        VehicleId = vehicleId;
        Model = model;
        this.make = make;
        this.availability = availability;
        UserId = userId;
        FullName = fullName;
        Email = email;
    }

    public ReservationView()
    {
    }

    [Key]
    public int ReservationId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public int TotalPrice { get; set; }
    public int VehicleId { get; set; }
    public string Model { get; set; }
    public string make { get; set; }
    public bool availability { get; set; }
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}