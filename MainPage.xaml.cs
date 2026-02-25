using calcDistance.Models;
using calcDistance.Calculations;
using Location = calcDistance.Models.Location;
using Android.Service.Voice;

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
                await DisplayAlert("Database Error", ex.Message, "OK");
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
                await DisplayAlert("Selection Error", "Please select a start, destination, and car.", "OK");
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

        //  Event handler for the Add Car button click. Shows a series of prompts to collect car details and saves the new car to the database.
        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            // Prompt for brand
            var brand = await DisplayPromptAsync("Ny bil", "Ange bilmärke:", "OK", "Avbryt");
            if (string.IsNullOrWhiteSpace(brand))
                return;

            // Prompt for fuel consumption
            var consumptionStr = await DisplayPromptAsync("Ny bil", "Ange förbrukning (l/100km):", "OK", "Avbryt", keyboard: Keyboard.Numeric);
            if (string.IsNullOrWhiteSpace(consumptionStr) || !double.TryParse(consumptionStr, out var consumption))
            {
                await DisplayAlert("Fel", "Ogiltig förbrukning.", "OK");
                return;
            }

            // Prompt for fuel type
            var fuelType = await DisplayPromptAsync("Ny bil", "Ange bränsletyp (t.ex. Petrol, Diesel):", "OK", "Avbryt");
            if (string.IsNullOrWhiteSpace(fuelType))
                return;

            try
            {
                var newCar = new Car
                {
                    Brand = brand,
                    Consumption = consumption,
                    FuelType = fuelType
                };

                await _db.AddCarAsync(newCar);

                // Reload the car picker to include the new car
                LoadDataAsync();

                await DisplayAlert("Klar", $"{brand} har lagts till!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Database Error", ex.Message, "OK");
            }
        }

        // on delete car button click, shows a prompt to select a car to delete and removes it from the database.
        private async void OnDeleteCarClicked(object sender, EventArgs e)
        {
            var cars = await _db.GetCarsAsync();
            var carNames = cars.Select(c => c.Brand).ToArray();
            var selectedCarName = await DisplayActionSheet("Välj en bil att ta bort:", "Avbryt", null, carNames);
            if (selectedCarName == "Avbryt" || string.IsNullOrWhiteSpace(selectedCarName))
                return;
            var carToDelete = cars.FirstOrDefault(c => c.Brand == selectedCarName);
            if (carToDelete == null)
            {
                await DisplayAlert("Fel", "Bilen kunde inte hittas.", "OK");
                return;
            }
            try
            {
                await _db.DeleteCarAsync(carToDelete.Id);
                LoadDataAsync();
                await DisplayAlert("Klar", $"{selectedCarName} har tagits bort!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Database Error", ex.Message, "OK");
            }
        }
    }
}
