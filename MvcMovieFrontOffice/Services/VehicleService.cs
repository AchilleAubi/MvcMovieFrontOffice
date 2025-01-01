using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Services;

public class VehicleService(string? connectionString)
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public async Task<List<Vehicle>> GetVehiclesAsync()
    {
        var vehicles = new List<Vehicle>();
        var query = "SELECT * FROM Vehicles";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    vehicles.Add(new Vehicle(
                        id: reader.GetInt32("Id"),
                        model: reader.GetString("Model"),
                        make: reader.GetString("Make"),
                        year: reader.GetInt32("Year"),
                        availability: reader.IsDBNull("Availability") ? (bool?)null : reader.GetBoolean("Availability"),
                        typeId: reader.GetInt32("TypeId"),
                        price: reader.GetInt32("Price"),
                        image: reader.GetString("Image"),
                        createdAt: reader.IsDBNull("CreatedAt") ? (DateTime?)null : reader.GetDateTime("CreatedAt"),
                        updatedAt: reader.IsDBNull("UpdatedAt") ? (DateTime?)null : reader.GetDateTime("UpdatedAt")
                    ));
                }
            }
        }

        return vehicles;
    }

    public async Task<List<VehicleView>> GetVehiclesViewAsync(int offset, int limit, string? model = null, string? make = null, string? vehicleType = null, bool? availability = null)
    {
        var vehicles = new List<VehicleView>();
        var query = @"SELECT * FROM VehicleView 
                      WHERE (@Model IS NULL OR Model LIKE '%' + @Model + '%') 
                        AND (@Make IS NULL OR Make LIKE '%' + @Make + '%') 
                        AND (@VehicleType IS NULL OR VehicleType = @VehicleType) 
                        AND (@Availability IS NULL OR Availability = @Availability)
                      ORDER BY VehicleId
                      OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Model", (object?)model ?? DBNull.Value);
            command.Parameters.AddWithValue("@Make", (object?)make ?? DBNull.Value);
            command.Parameters.AddWithValue("@VehicleType", (object?)vehicleType ?? DBNull.Value);
            command.Parameters.AddWithValue("@Availability", (object?)availability ?? DBNull.Value);
            command.Parameters.AddWithValue("@Offset", offset);
            command.Parameters.AddWithValue("@Limit", limit);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    vehicles.Add(new VehicleView(
                        vehicleId: reader.GetInt32("VehicleId"),
                        model: reader.GetString("Model"),
                        make: reader.GetString("Make"),
                        year: reader.GetInt32("Year"),
                        availability: reader.GetBoolean("Availability"),
                        price: reader.GetInt32("Price"),
                        createdAt: reader.GetDateTime("CreatedAt"),
                        updatedAt: reader.GetDateTime("UpdatedAt"),
                        vehicleTypeId: reader.GetInt32("VehicleTypeId"),
                        vehicleType: reader.GetString("VehicleType"),
                        vehicleImage: reader.GetString("VehicleImage")
                    ));
                }
            }
        }

        return vehicles;
    }

    public async Task<int> GetTotalVehicleViewCountAsync(string? model = null, string? make = null, string? vehicleType = null, bool? availability = null)
    {
        var query = @"SELECT COUNT(*) FROM VehicleView 
                      WHERE (@Model IS NULL OR Model LIKE '%' + @Model + '%') 
                        AND (@Make IS NULL OR Make LIKE '%' + @Make + '%') 
                        AND (@VehicleType IS NULL OR VehicleType = @VehicleType) 
                        AND (@Availability IS NULL OR Availability = @Availability)";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Model", (object?)model ?? DBNull.Value);
            command.Parameters.AddWithValue("@Make", (object?)make ?? DBNull.Value);
            command.Parameters.AddWithValue("@VehicleType", (object?)vehicleType ?? DBNull.Value);
            command.Parameters.AddWithValue("@Availability", (object?)availability ?? DBNull.Value);

            await connection.OpenAsync();
            return (int)await command.ExecuteScalarAsync();
        }
    }

    public async Task<VehicleView> GetVehicleViewByIdAsync(int id)
    {
        Console.WriteLine($"Query Parameter: Id = {id}");

        var query = "SELECT * FROM VehicleView WHERE VehicleId = @Id";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new VehicleView(
                        vehicleId: reader.GetInt32("VehicleId"),
                        model: reader.GetString("Model"),
                        make: reader.GetString("Make"),
                        year: reader.GetInt32("Year"),
                        availability: reader.GetBoolean("Availability"),
                        price: reader.GetInt32("Price"),
                        createdAt: reader.GetDateTime("CreatedAt"),
                        updatedAt: reader.GetDateTime("UpdatedAt"),
                        vehicleTypeId: reader.GetInt32("VehicleTypeId"),
                        vehicleType: reader.GetString("VehicleType"),
                        vehicleImage: reader.GetString("VehicleImage")
                    );
                }
            }
        }

        throw new KeyNotFoundException("VehicleView not found.");
    }

    public async Task<Vehicle> GetVehicleByIdAsync(int id)
    {
        var query = "SELECT * FROM Vehicles WHERE Id = @Id";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Vehicle(
                        id: reader.GetInt32("Id"),
                        model: reader.GetString("Model"),
                        make: reader.GetString("Make"),
                        year: reader.GetInt32("Year"),
                        availability: reader.IsDBNull("Availability") ? (bool?)null : reader.GetBoolean("Availability"),
                        typeId: reader.GetInt32("TypeId"),
                        price: reader.GetInt32("Price"),
                        image: reader.GetString("Image"),
                        createdAt: reader.IsDBNull("CreatedAt") ? (DateTime?)null : reader.GetDateTime("CreatedAt"),
                        updatedAt: reader.IsDBNull("UpdatedAt") ? (DateTime?)null : reader.GetDateTime("UpdatedAt")
                    );
                }
            }
        }

        throw new KeyNotFoundException("Vehicle not found.");
    }

    public async Task<List<string>> GetVehicleTypesAsync()
    {
        var vehicleTypes = new List<string>();
        var query = "SELECT Name FROM VehicleTypes";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    vehicleTypes.Add(reader.GetString("Name"));
                }
            }
        }

        return vehicleTypes;
    }

    public async Task<List<object>> GetVehicleTypesWithIdsAsync()
    {
        var vehicleTypes = new List<object>();
        var query = "SELECT Id, Name FROM VehicleTypes";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    vehicleTypes.Add(new { Id = reader.GetInt32("Id"), Name = reader.GetString("Name") });
                }
            }
        }

        return vehicleTypes;
    }

    public async Task CreateVehicleAsync(Vehicle vehicle)
    {
        var query = @"INSERT INTO Vehicles (Model, Make, Year, Availability, TypeId, Price, Image, CreatedAt, UpdatedAt) 
                      VALUES (@Model, @Make, @Year, @Availability, @TypeId, @Price, @Image, @CreatedAt, @UpdatedAt)";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Model", vehicle.Model);
            command.Parameters.AddWithValue("@Make", vehicle.Make);
            command.Parameters.AddWithValue("@Year", vehicle.Year);
            command.Parameters.AddWithValue("@Availability", (object?)vehicle.Availability ?? DBNull.Value);
            command.Parameters.AddWithValue("@TypeId", vehicle.TypeId);
            command.Parameters.AddWithValue("@Price", vehicle.Price);
            command.Parameters.AddWithValue("@Image", vehicle.Image);
            command.Parameters.AddWithValue("@CreatedAt", (object?)vehicle.CreatedAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@UpdatedAt", (object?)vehicle.UpdatedAt ?? DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateVehicleAsync(Vehicle vehicle)
    {
        var query = @"UPDATE Vehicles 
                      SET Model = @Model, Make = @Make, Year = @Year, Availability = @Availability, 
                          TypeId = @TypeId, Price = @Price, Image = @Image, CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt 
                      WHERE Id = @Id";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", vehicle.Id);
            command.Parameters.AddWithValue("@Model", vehicle.Model);
            command.Parameters.AddWithValue("@Make", vehicle.Make);
            command.Parameters.AddWithValue("@Year", vehicle.Year);
            command.Parameters.AddWithValue("@Availability", (object?)vehicle.Availability ?? DBNull.Value);
            command.Parameters.AddWithValue("@TypeId", vehicle.TypeId);
            command.Parameters.AddWithValue("@Price", vehicle.Price);
            command.Parameters.AddWithValue("@Image", vehicle.Image);
            command.Parameters.AddWithValue("@CreatedAt", (object?)vehicle.CreatedAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@UpdatedAt", (object?)vehicle.UpdatedAt ?? DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public bool VehicleExist(int id)
    {
        var query = "SELECT COUNT(*) FROM Vehicles WHERE Id = @Id";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            return (int)command.ExecuteScalar() > 0;
        }
    }
    
    public async Task DeleteVehicleAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("DELETE FROM Vehicles WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
