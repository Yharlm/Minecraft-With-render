using Minecraft;
using System;
using MySql.Data.MySqlClient;

public class Database : Cordinates
{
	
        public async Task<List<Cordinates>> GetCordinates()
        {
            var position = new List<Cordinates>(); // Инициализиране на списък за съхраняване на продуктите.

            // Коригирана връзка с MySQL база данни
            using var connection = new MySqlConnection("Server=localhost;Port=3306;Database=Player;Uid=root;Pwd=;");
            await connection.OpenAsync();

            // SQL заявка за извличане на всички продукти
            using var command = new MySqlCommand("SELECT * FROM position", connection);
            using var reader = await command.ExecuteReaderAsync();

            // Четене на данни от резултата на заявката
            while (await reader.ReadAsync())
            {
                var cordinates = new Cordinates
                {

                    x = reader.GetInt32(0),
                    y = reader.GetInt32(1),

                };

                position.Add(cordinates); // Добавяне на продукта в списъка
            }

            return position;
        }
    
}
