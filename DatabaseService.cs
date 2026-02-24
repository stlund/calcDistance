using calcDistance.Models;

namespace calcDistance
{

    //  A simple service to fetch data from the PostgreSQL database.
    public class DatabaseService
    {
        //  Connection string with database credentials.
        private readonly string _connectionString =
            "Host=10.0.2.2;Port=5432;Username=postgres;Database=berakna_distans;Password=password123";

        //  Fetches all cars from the database and returns them as a list.
        public async Task<List<Car>> GetCarsAsync()
        {
            var cars = new List<Car>(); //  List to hold the fetched cars.

            using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            //  SQL command to select all cars from the "car" table.
            using var command = new Npgsql.NpgsqlCommand("SELECT car_id, brand, consumption, fuel_type FROM car", connection);
            using var reader = await command.ExecuteReaderAsync();

            //  Read each row from the result set and create Car objects to add to the list.
            while (await reader.ReadAsync()) //
            {
                cars.Add(new Car
                {
                    Id = reader.GetInt32(0),
                    Brand = reader.GetString(1),
                    Consumption = reader.GetDouble(2),
                    FuelType = reader.GetString(3)
                });
            }

            return cars;
        }

        //  Fetches all locations from the database and returns them as a list.
        public async Task<List<Models.Location>> GetLocationsAsync()
        {
            var locations = new List<Models.Location>(); //  List to hold the fetched locations.

            using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            //  SQL command to select all locations from the "location" table.
            using var command = new Npgsql.NpgsqlCommand("SELECT location_id, name, lat, lng FROM location", connection);
            using var reader = await command.ExecuteReaderAsync();

            //  Read each row from the result set and create Location objects to add to the list.
            while (await reader.ReadAsync())
            {
                locations.Add(new Models.Location
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Lat = reader.GetDouble(2),
                    Lng = reader.GetDouble(3)
                });
            }

            return locations;
        }
    }
}