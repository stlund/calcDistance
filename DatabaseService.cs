namespace calcDistance
{
    public class DatabaseService
    {
        private readonly string _connectionString =
            "Host=10.0.2.2;Port=5432;Username=postgres;Database=berakna_distans;Password=password123";

        //Function to load all cars from the database
        public async Task<List<string>> GetCarsAsync()
        {
            return await QueryStringListAsync("SELECT brand FROM car");
        }

        // Function to load locations from the database
        public async Task<List<string>> GetLocationsAsync()
        {
            return await QueryStringListAsync("SELECT name FROM location");
        }

        private async Task<List<string>> QueryStringListAsync(string query)
        {
            var results = new List<string>();

            using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new Npgsql.NpgsqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(reader.GetString(0));
            }

            return results;
        }
    }
}