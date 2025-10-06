namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public long Timestamp { get; set; }
    }

    public class OpenWeatherResponse
    {
        public Coord? Coord { get; set; }
        public List<Weather>? Weather { get; set; }
        public string? Base { get; set; }
        public Main? Main { get; set; }
        public int Visibility { get; set; }
        public Wind? Wind { get; set; }
        public Clouds? Clouds { get; set; }
        public long Dt { get; set; }
        public Sys? Sys { get; set; }
        public int Timezone { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Cod { get; set; }
    }

    public class Coord
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Feels_Like { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }

    public class Sys
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public string? Country { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }
}