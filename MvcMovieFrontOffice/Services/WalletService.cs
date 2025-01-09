using System.Data;
using Microsoft.Data.SqlClient;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Services;

public class WalletService(string? connectionString)
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
   
    public async Task<Wallet> GetWalletByIdAsync(string? id)
    {
        var query = "SELECT * FROM Wallet WHERE UserId = @Id";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Wallet(
                        id: reader.GetInt32("Id"),
                        amount: reader.GetInt32("Amount"),
                        userId: reader.GetString("UserId")
                    );
                }
            }
        }

        throw new KeyNotFoundException("Wallet not found.");
    }
    
    public async Task CreateWalletAsync(Wallet wallet)
    {
        var query = @"INSERT INTO Wallet (Amount, UserId) 
                      VALUES (@Amount, @UserId)";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Amount", wallet.Amount);
            command.Parameters.AddWithValue("@UserId", wallet.UserId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateWalletAsync(Wallet wallet)
    {
        var query = @"UPDATE Wallet 
                      SET Amount = @Amount, UserId = @UserId 
                      WHERE Id = @Id";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", wallet.Id);
            command.Parameters.AddWithValue("@Amount", wallet.Amount);
            command.Parameters.AddWithValue("@UserId", wallet.UserId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}