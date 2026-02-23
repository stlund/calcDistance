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

        private async void LoadDataAsync()
        {
            try
            {
                var cars = await _db.GetCarsAsync();
                var locations = await _db.GetLocationsAsync();

                // Fill the PickerCar with the list of cars
                PickerCar.ItemsSource = cars;
                // Fill the FromPicker with the list of start locations
                FromPicker.ItemsSource = locations;
                // Fill the ToPicker with the list of destinations
                ToPicker.ItemsSource = locations;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Database Error", ex.Message, "OK");
            }
        }

        private void CalculateBtn_Clicked(object sender, EventArgs e)
        {
            // calculate the distance and between FromPicker and ToPicker and display the result in the ResultLabel
                var fromLocation = FromPicker.SelectedItem as string;
                var toLocation = ToPicker.SelectedItem as string;
            var selectedCar = PickerCar.SelectedItem as string;
    
                if (fromLocation == null || toLocation == null)
                {
                    DisplayAlert("Selection Error", "Please select both a starting location and a destination.", "OK");
                    return;
                }
    
                // Here you would implement the logic to calculate the distance between the two locations
                // For demonstration purposes, we'll just display a placeholder result
                Resultat.Text = $"Distance from {fromLocation} to {toLocation} is: [calculated distance] Using a {selectedCar}";
        }
    }
}
