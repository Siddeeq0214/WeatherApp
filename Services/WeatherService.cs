using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherAsync(string city);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string city)
        {
            try
            {
                // Step 1: Get coordinates from city name using geocoding API
                var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1&language=en&format=json";
                var geoResponse = await _httpClient.GetAsync(geoUrl);

                if (!geoResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to get location data");
                    return null;
                }

                var geoContent = await geoResponse.Content.ReadAsStringAsync();
                var geoData = JsonConvert.DeserializeObject<GeocodingResponse>(geoContent);

                if (geoData?.Results == null || geoData.Results.Count == 0)
                {
                    Console.WriteLine("City not found");
                    return null;
                }

                var location = geoData.Results[0];

                // Step 2: Get weather data using coordinates
                var weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,relative_humidity_2m,apparent_temperature,weather_code,wind_speed_10m&timezone=auto";
                var weatherResponse = await _httpClient.GetAsync(weatherUrl);

                if (!weatherResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to get weather data");
                    return null;
                }

                var weatherContent = await weatherResponse.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<OpenMeteoResponse>(weatherContent);

                if (weatherData?.Current == null)
                {
                    return null;
                }

                // Map weather code to description
                var description = GetWeatherDescription(weatherData.Current.WeatherCode);
                var icon = GetWeatherIcon(weatherData.Current.WeatherCode);

                return new WeatherResponse
                {
                    City = location.Name,
                    Country = location.Country ?? location.CountryCode,
                    Temperature = Math.Round(weatherData.Current.Temperature, 1),
                    FeelsLike = Math.Round(weatherData.Current.ApparentTemperature, 1),
                    Description = description,
                    Icon = icon,
                    Humidity = weatherData.Current.Humidity,
                    WindSpeed = Math.Round(weatherData.Current.WindSpeed, 1),
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather: {ex.Message}");
                return null;
            }
        }

        private string GetWeatherDescription(int code)
        {
            return code switch
            {
                0 => "Clear sky",
                1 or 2 or 3 => "Partly cloudy",
                45 or 48 => "Foggy",
                51 or 53 or 55 => "Drizzle",
                61 or 63 or 65 => "Rain",
                71 or 73 or 75 => "Snow",
                77 => "Snow grains",
                80 or 81 or 82 => "Rain showers",
                85 or 86 => "Snow showers",
                95 => "Thunderstorm",
                96 or 99 => "Thunderstorm with hail",
                _ => "Unknown"
            };
        }

        private string GetWeatherIcon(int code)
        {
            return code switch
            {
                0 => "01d",
                1 or 2 => "02d",
                3 => "03d",
                45 or 48 => "50d",
                51 or 53 or 55 or 61 or 63 or 65 or 80 or 81 or 82 => "10d",
                71 or 73 or 75 or 77 or 85 or 86 => "13d",
                95 or 96 or 99 => "11d",
                _ => "01d"
            };
        }
    }

    // Response models for Open-Meteo API
    public class GeocodingResponse
    {
        [JsonProperty("results")]
        public List<LocationResult>? Results { get; set; }
    }

    public class LocationResult
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }
    }

    public class OpenMeteoResponse
    {
        [JsonProperty("current")]
        public CurrentWeather? Current { get; set; }
    }

    public class CurrentWeather
    {
        [JsonProperty("temperature_2m")]
        public double Temperature { get; set; }

        [JsonProperty("apparent_temperature")]
        public double ApparentTemperature { get; set; }

        [JsonProperty("relative_humidity_2m")]
        public int Humidity { get; set; }

        [JsonProperty("weather_code")]
        public int WeatherCode { get; set; }

        [JsonProperty("wind_speed_10m")]
        public double WindSpeed { get; set; }
    }
}