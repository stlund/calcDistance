using calcDistance.Models;
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

            Resultat.Text = $"Distance from {fromLocation.Name} to {toLocation.Name} using {selectedCar.Brand} ({selectedCar.Consumption} l/100km)";
        }
    }
}
