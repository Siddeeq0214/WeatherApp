# WeatherApp
WeatherApp is a simple web-based weather dashboard that allows users to search for any city and view real-time weather information. The app displays temperature, humidity, wind speed, weather description, and more, with a clean and modern UI.

## Features
- Search for weather by city name
- Displays temperature, feels like, humidity, wind speed, country, and weather icon
- Responsive and visually appealing interface
- Error handling for invalid city names

## Stack Used
- **Backend:** ASP.NET Core 9 Web API
- **Frontend:** HTML, CSS (Poppins font), Vanilla JavaScript
- **HTTP Client:** `HttpClient` via dependency injection
- **API Documentation:** Swagger (Swashbuckle.AspNetCore)
- **Serialization:** Newtonsoft.Json

## Weather API Used
- **Geocoding:** [Open-Meteo Geocoding API](https://open-meteo.com/)
  - Used to convert city names to latitude/longitude coordinates.
- **Weather Data:** [Open-Meteo Weather API](https://open-meteo.com/)
  - Provides current weather data (temperature, humidity, wind speed, weather code, etc.) for given coordinates.

## How It Works
1. User enters a city name in the search box.
2. The frontend sends a request to the backend API endpoint:  
   `GET /api/weather/{city}`
3. The backend:
    - Uses Open-Meteo Geocoding API to get coordinates for the city.
    - Uses Open-Meteo Weather API to fetch current weather data for those coordinates.
    - Maps weather codes to human-readable descriptions and icons.
    - Returns a structured JSON response.
4. The frontend displays the weather data in a user-friendly format.

## Running Locally
1. **Clone the repository**
2. **Restore NuGet packages**
3. **Run the app**
   ```sh
   dotnet run
   ```
4. Open [http://localhost:5006](http://localhost:5006) in your browser.

## License

MIT

---

**Powered by [Open-Meteo](https://open-meteo.com/)**
