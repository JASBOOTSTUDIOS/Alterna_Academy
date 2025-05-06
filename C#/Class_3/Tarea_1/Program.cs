using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int CompanyId { get; set; } 
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=.;Database=MusikOn;Trusted_Connection=True;TrustServerCertificate=True";
        }
        var app = builder.Build();

        app.MapGet("/companias", async () =>
        {
            List<Company> companies = new List<Company>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Id, Name, Address FROM Companies";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(reader);
                        Company company = new Company
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Address = reader.GetString(2)
                        };
                        companies.Add(company);
                    }
                }
            }
            return Results.Ok(companies);
        });

        app.MapPost("/companias", async ([FromBody] Company newCompany) =>
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Companies (Name, Address) VALUES (@Name, @Address); SELECT SCOPE_IDENTITY()"; // Obtener el ID insertado
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", newCompany.Name);
                    cmd.Parameters.AddWithValue("@Address", newCompany.Address);
                    int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync()); 
                    newCompany.Id = newId; 
                    return Results.Created($"/companias/{newId}", newCompany);
                }
            }
        });

        app.MapPut("/companias/{id}", async (int id, [FromBody] Company updatedCompany) =>
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Companies SET Name = @Name, Address = @Address WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", updatedCompany.Name);
                    cmd.Parameters.AddWithValue("@Address", updatedCompany.Address);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        return Results.NotFound(); 
                    }
                    return Results.Ok(updatedCompany); 
                }
            }
        });

        app.MapDelete("/companias/{id}", async (int id) =>
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
               
                string checkQuery = "SELECT COUNT(*) FROM Employees WHERE CompanyId = @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Id", id);
                    int employeeCount = (int) await checkCmd.ExecuteScalarAsync();
                    if (employeeCount > 0)
                    {
                        return Results.BadRequest("No se puede eliminar la compañía porque tiene empleados asociados.");
                    }
                }


                string deleteQuery = "DELETE FROM Companies WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        return Results.NotFound(); 
                    }
                    return Results.NoContent(); 
                }
            }
        });

      
        app.MapGet("/empleados", async () =>
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Id, Name, Position, CompanyId FROM Employees";  
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Position = reader.GetString(2),
                            CompanyId = reader.GetInt32(3)
                        };
                        employees.Add(employee);
                    }
                }
            }
            return Results.Ok(employees);
        });

        app.MapPost("/empleados", async ([FromBody] Employee newEmployee) =>
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Employees (Name, Position, CompanyId) VALUES (@Name, @Position, @CompanyId); SELECT SCOPE_IDENTITY()"; 
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", newEmployee.Name);
                    cmd.Parameters.AddWithValue("@Position", newEmployee.Position);
                    cmd.Parameters.AddWithValue("@CompanyId", newEmployee.CompanyId);
                    int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync()); 
                    newEmployee.Id = newId;
                    return Results.Created($"/empleados/{newId}", newEmployee);
                }
            }
        });

        app.MapPut("/empleados/{id}", async (int id, [FromBody] Employee updatedEmployee) =>
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Employees SET Name = @Name, Position = @Position, CompanyId = @CompanyId WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", updatedEmployee.Name);
                    cmd.Parameters.AddWithValue("@Position", updatedEmployee.Position);
                    cmd.Parameters.AddWithValue("@CompanyId", updatedEmployee.CompanyId);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(updatedEmployee);
                }
            }
        });

        app.MapDelete("/empleados/{id}", async (int id) =>
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Employees WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        return Results.NotFound();
                    }
                    return Results.NoContent();
                }
            }
        });

        app.Run();
    }
}
