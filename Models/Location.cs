namespace calcDistance.Models
{
    //  Represents a location with an ID, name, latitude, and longitude.
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Lat { get; set; } 
        public double Lng { get; set; }

        //  Overrides the ToString method to return the location's name, which is what will be displayed in the Picker.
        public override string ToString() => Name;
    }
}