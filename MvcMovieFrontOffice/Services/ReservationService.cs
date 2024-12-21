using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Services;

public class ReservationService(string? connectionString)
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        var reservations = new List<Reservation>();
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT * FROM Reservations", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            reservations.Add(MapToReservation(reader));
        }

        return reservations;
    }

    public async Task<Reservation?> GetReservationByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT * FROM Reservations WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapToReservation(reader);
        }

        return null;
    }

    public async Task CreateReservationAsync(Reservation reservation)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
            "INSERT INTO Reservations (VehicleId, UserId, StartDate, EndDate, Status, TotalPrice, CreatedAt, UpdatedAt) " +
            "VALUES (@VehicleId, @UserId, @StartDate, @EndDate, @Status, @TotalPrice, @CreatedAt, @UpdatedAt)", connection);

        AddReservationParameters(command, reservation);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateReservationAsync(Reservation reservation)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
            "UPDATE Reservations SET VehicleId = @VehicleId, UserId = @UserId, StartDate = @StartDate, EndDate = @EndDate, " +
            "Status = @Status, TotalPrice = @TotalPrice, CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt WHERE Id = @Id", connection);

        AddReservationParameters(command, reservation);
        command.Parameters.AddWithValue("@Id", reservation.Id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteReservationAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("DELETE FROM Reservations WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public bool ReservationExists(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT COUNT(*) FROM Reservations WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        var count = (int)command.ExecuteScalar();
        return count > 0;
    }

    private static Reservation MapToReservation(SqlDataReader reader)
    {
        return new Reservation(
            id: reader.GetInt32(reader.GetOrdinal("Id")),
            vehicleId: reader.GetInt32(reader.GetOrdinal("VehicleId")),
            userId: reader.GetString(reader.GetOrdinal("UserId")),
            startDate: reader.GetDateTime(reader.GetOrdinal("StartDate")),
            endDate: reader.GetDateTime(reader.GetOrdinal("EndDate")),
            status: reader.GetString(reader.GetOrdinal("Status")),
            totalPrice: reader.GetInt32(reader.GetOrdinal("TotalPrice")),
            createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
        );
    }

    private static void AddReservationParameters(SqlCommand command, Reservation reservation)
    {
        command.Parameters.AddWithValue("@VehicleId", reservation.VehicleId);
        command.Parameters.AddWithValue("@UserId", reservation.UserId);
        command.Parameters.AddWithValue("@StartDate", reservation.StartDate);
        command.Parameters.AddWithValue("@EndDate", reservation.EndDate);
        command.Parameters.AddWithValue("@Status", reservation.Status);
        command.Parameters.AddWithValue("@TotalPrice", reservation.TotalPrice);
        command.Parameters.AddWithValue("@CreatedAt", reservation.CreatedAt ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@UpdatedAt", reservation.UpdatedAt ?? (object)DBNull.Value);
    }
}
