using Minecraft;
using System;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

public class Database : Cordinates
{

    public async Task<bool> SaveCordinates(Cordinates cordinates)
    {
        var success = false; // To track if the insert was successful.

        // Corrected connection string for MySQL database.
        var connectionString = "Server=localhost;Port=3306;Database=Player;Uid=root;Pwd=;";

        try
        {
            // Open the connection asynchronously.
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            // SQL query to insert coordinates into the 'position' table.
            var query = "INSERT INTO player (x, y) VALUES (@x, @y)";

            // Prepare the command with the query.
            using var command = new MySqlCommand(query, connection);

            // Add parameters to prevent SQL injection.
            command.Parameters.AddWithValue("@x", cordinates.x);
            command.Parameters.AddWithValue("@y", cordinates.y);

            // Execute the query asynchronously and check the number of affected rows.
            var rowsAffected = await command.ExecuteNonQueryAsync();

            // If one or more rows are affected, it means the data was successfully inserted.
            if (rowsAffected > 0)
            {
                success = true; // Set success to true if the insertion is successful.
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the database operations.
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        // Return whether the save operation was successful or not.
        return success;
    }

    public static async Task<bool> playerPos(Cordinates cordinates,string name)
    {
        var success = false; // To track if the insert was successful.

        // Corrected connection string for MySQL database.
        var connectionString = "Server=localhost;Port=3306;Database=Player;Uid=root;Pwd=;";

        try
        {
            // Open the connection asynchronously.
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            // SQL query to insert coordinates into the 'position' table.
            var query = "UPDATE  player  SET x = @x, y = @y WHERE Name = @Name";

            // Prepare the command with the query.
            using var command = new MySqlCommand(query, connection);

            // Add parameters to prevent SQL injection.
            command.Parameters.AddWithValue("@x", cordinates.x);
            command.Parameters.AddWithValue("@y", cordinates.y);
            command.Parameters.AddWithValue("@Name", name);

            // Execute the query asynchronously and check the number of affected rows.
            var rowsAffected = await command.ExecuteNonQueryAsync();

            // If one or more rows are affected, it means the data was successfully inserted.
            if (rowsAffected > 0)
            {
                success = true; // Set success to true if the insertion is successful.
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the database operations.
            
        }

        // Return whether the save operation was successful or not.
        return success;
    }
    public static async Task<List<string>> GetName()
        {
            //var position = new List<Cordinates>(); // Инициализиране на списък за съхраняване на продуктите.
            var name = new List<string>();  
            // Коригирана връзка с MySQL база данни
            using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=Player;Uid=root;Pwd=;");
            await connection.OpenAsync();

            // SQL заявка за извличане на всички продукти
            using var command = new MySqlCommand("SELECT * FROM player", connection);
            using var reader = await command.ExecuteReaderAsync();

            // Четене на данни от резултата на заявката
            while (await reader.ReadAsync())
            {
                name.Add(reader.GetString(0));
                //var cordinates = new Cordinates
                //{
                   
                //    x = reader.GetInt32(1),
                //    y = reader.GetInt32(2),
                    
                //};

                /*position.Add(cordinates)*/; // Добавяне на продукта в списъка
            }

            return name;
        }
    public static async Task<List<Cordinates>> GetPos()
    {
        var position = new List<Cordinates>(); // Инициализиране на списък за съхраняване на продуктите.
        //var name = new List<string>();
        // Коригирана връзка с MySQL база данни
        using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=Player;Uid=root;Pwd=;");
        await connection.OpenAsync();

        // SQL заявка за извличане на всички продукти
        using var command = new MySqlCommand("SELECT * FROM player", connection);
        using var reader = await command.ExecuteReaderAsync();

        // Четене на данни от резултата на заявката
        while (await reader.ReadAsync())
        {

            var cordinates = new Cordinates
            {

                x = reader.GetInt32(1),
                y = reader.GetInt32(2),

            };

            position.Add(cordinates)
            ; // Добавяне на продукта в списъка
        }

        return position;
    }

}
