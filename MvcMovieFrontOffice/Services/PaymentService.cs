using Microsoft.Data.SqlClient;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Services;

public class PaymentService(string? connectionString)
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
   
    public async Task<List<Payment>> GetAllReservationsAsync()
    {
        var payment = new List<Payment>();
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT * FROM Payments", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            payment.Add(MapToPayment(reader));
        }

        return payment;
    }

    public async Task<List<Payment>> GetPaymentByUserIdAsync(string? id)
    {
        var payments = new List<Payment>();
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT * FROM Payments WHERE UserId = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            payments.Add(MapToPayment(reader));
        }

        return payments;
    }
    
    public async Task CreatePaymentAsync(Payment payment)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
            "INSERT INTO Payments (ReservationId, PaymentDate, Amount, PaymentMethod, UserId) " +
            "VALUES (@ReservationId, @PaymentDate, @Amount, @PaymentMethod, @UserId)", connection);

        AddPaymentParameters(command, payment);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
    
    private static Payment MapToPayment(SqlDataReader reader)
    {
        return new Payment(
            id : reader.GetInt32(reader.GetOrdinal("Id")),
            reservationId: reader.GetInt32(reader.GetOrdinal("ReservationId")),
            amount: reader.GetInt32(reader.GetOrdinal("Amount")),
            paymentDate: reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
            paymentMethod:reader.GetString(reader.GetOrdinal("PaymentMethod")),
            userId: reader.GetString(reader.GetOrdinal("UserId"))
        );
    }
    
    private static void AddPaymentParameters(SqlCommand command, Payment payment)
    {
        command.Parameters.AddWithValue("@ReservationId", payment.ReservationId);
        command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
        command.Parameters.AddWithValue("@Amount", payment.Amount);
        command.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
        command.Parameters.AddWithValue("@UserId", payment.UserId);
    }
}