namespace calcDistance.Models
{
    //  Represents a car with an ID, brand, fuel consumption, and fuel type.
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public double Consumption { get; set; }
        public string FuelType { get; set; } = string.Empty;

        //  Overrides the ToString method to return the car's brand, which is what will be displayed in the Picker.
        public override string ToString() => $"{Brand}  ({FuelType})";
    }
}