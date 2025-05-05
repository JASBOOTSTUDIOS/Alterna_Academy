using Microsoft.Data.SqlClient;
using System;

namespace SQLServerConnection
{
    class Program
    {
        static void Main(string[] args)
        {
           string connString = "Server=localhost;Database=employees;User Id=SA;Password=Sa123456;TrustServerCertificate=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    Console.WriteLine("Conexión a la base de datos exitosa."); 

                    string query = "SELECT id, nombre, edad, fechaNacimiento, rol FROM Personas;";
                    // string query = "SELECT edad, nombre FROM Personas WHERE edad = @edad";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@edad", 34);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int edad = reader.GetInt32(reader.GetOrdinal("edad"));
                                string nombre = reader.GetString(reader.GetOrdinal("nombre"));
                                Console.WriteLine($"Edad: {edad}, Nombre: {nombre}");
                            }
                        }
                    }

                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL:");
                foreach (SqlError error in ex.Errors)
                {
                    Console.WriteLine($"  Error Code: {error.Number}");
                    Console.WriteLine($"  Message: {error.Message}");
                    Console.WriteLine($"  Line Number: {error.LineNumber}");
                    Console.WriteLine($"  Source: {error.Source}");
                    Console.WriteLine($"  Procedure: {error.Procedure}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
            }

            Console.WriteLine("Fin del programa."); // Mensaje de fin de programa
            // Console.ReadKey(); // Esto es útil si ejecutas el programa desde el Explorador de Windows
            //                   // para evitar que la ventana se cierre inmediatamente.  No es necesario
            //                   // si lo ejecutas desde el IDE.
        }
    }
}
