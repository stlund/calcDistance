using calcDistance.Models;
using calcDistance.Calculations;
using Location = calcDistance.Models.Location;

namespace calcDistance
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _db = new();

        public MainPage()
        {
            InitializeComponent();
            LoadDataAsync();
        }

        //  Asynchronously loads cars and locations from the database and populates the Pickers. If an error occurs, it displays an alert with the error message.
        private async void LoadDataAsync()
        {
            try
            {
                var cars = await _db.GetCarsAsync();
                var locations = await _db.GetLocationsAsync();

                PickerCar.ItemsSource = cars;
                FromPicker.ItemsSource = locations;
                ToPicker.ItemsSource = locations;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Database Error", ex.Message, "OK");
            }
        }

        //  Event handler for the Calculate button click. It retrieves the selected start location,
        //  destination, and car. If any of these are not selected, it shows an alert. Otherwise, it displays a message with the selected options.
        private async void CalculateBtn_Clicked(object sender, EventArgs e)
        {
            var fromLocation = FromPicker.SelectedItem as Location;
            var toLocation = ToPicker.SelectedItem as Location;
            var selectedCar = PickerCar.SelectedItem as Car;

            if (fromLocation == null || toLocation == null || selectedCar == null)
            {
                await DisplayAlertAsync("Selection Error", "Please select a start, destination, and car.", "OK");
                return;
            }


            var distance = Calculation.CalculateDistance(fromLocation.Lat, fromLocation.Lng, toLocation.Lat, toLocation.Lng);
            var fuelConsumption = Calculation.CalculateFuelConsumption(distance, selectedCar.Consumption);
            // Parses the fuel price from the FuelPriceEntry Text property. If parsing fails, it defaults to 0.
            var fuelPrice = double.TryParse(FuelPriceEntry.Text, out var price) ? price : 0;


            var totalCost = (distance / 10) * fuelConsumption * fuelPrice;
            var consumptionPerMile = distance / 10;
            var fuelUsed = consumptionPerMile * fuelConsumption;

            Resultat.Text = $"Avstånd: {fromLocation.Name} till {toLocation.Name}: {distance:F2} Km\n" +
                $"{selectedCar.Brand} beräknas dra: {consumptionPerMile:F2} L {selectedCar.FuelType}\n" +
                $"Totalkostnad: {totalCost:F0} Kr";
        }
    }
}
