using Blazored.LocalStorage;
using CsvHelper;
using Microsoft.Extensions.Logging;
using RandomHoliday.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RandomHoliday.Services
{
    public class GeoApiService
    {
        private readonly ILocalStorageService localStorage;
        private readonly ILogger<GeoApiService> logger;
        private const string CITIES_STORAGE_KEY = "CityData";

        public GeoApiService(ILocalStorageService LocalStorage, ILogger<GeoApiService> logger)
        {
            localStorage = LocalStorage;
            this.logger = logger;
        }

        public async Task<IEnumerable<City>> RetrieveCities()
        {
            // Just to try out localstorage. Very inefficient as the data will have to be sent to/from the client anyways,
            if (await localStorage.ContainKeyAsync(CITIES_STORAGE_KEY))
            {
                logger.LogDebug("Retrieving cities from localstorage");
                return await localStorage.GetItemAsync<IEnumerable<City>>(CITIES_STORAGE_KEY);
            }
            else
            {
                var path = Path.Join(Directory.GetCurrentDirectory(), "wwwroot", "cities.csv");
                using var reader = new StreamReader(path);
                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                var cities = csvReader.GetRecords<City>()
                    .ToList();

                logger.LogDebug("Retrieving cities from localstorage");
                await localStorage.SetItemAsync(CITIES_STORAGE_KEY, cities);

                return cities;
            }
        }
    }
}
