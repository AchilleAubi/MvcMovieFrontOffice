using System.Data;
using Microsoft.Data.SqlClient;

namespace MvcMovieFrontOffice.Models;

public class DatabaseHelper(string? connectionString)
{
    public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[]? parameters = null)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(query, connection);

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        var dataTable = new DataTable();
        await using var reader = await command.ExecuteReaderAsync();
        dataTable.Load(reader);

        return dataTable;
    }

    public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(query, connection);

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        return await command.ExecuteNonQueryAsync();
    }
}
