using FuzzySharp;
using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using RandomHoliday.Models;
using RandomHoliday.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static MudBlazor.Defaults.Classes;

namespace RandomHoliday.Components
{
    public partial class HolidaySelector
    {
        [Inject]
        public GeoApiService GeoApiService { get; set; }

        [Inject]
        public IJSRuntime JsRunTime { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        private City SelectedCity = null;
        private double SelectedRange = 1;

        private bool IsFormValid;
        private readonly Random _random = new();
        private IEnumerable<City> Cities = new List<City>();
        private string browserLanguage = "";
        private string selectedDistanceUnit = "km";

        private void OnBlur(FocusEventArgs args)
        {
            if (SelectedRange == 0)
            {
                SelectedRange = 1;
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            browserLanguage = (await JsRunTime.InvokeAsync<string>("getBrowserLanguage")).ToUpper(CultureInfo.InvariantCulture);
            Cities = await GeoApiService.RetrieveCities();
        }

        private Task<IEnumerable<City>> SearchAsync(string value)
        {
            return Task.FromResult(FindCities(value));
        }

        private void ToggleDistanceUnits()
        {
            if (string.Equals(selectedDistanceUnit, "km", StringComparison.OrdinalIgnoreCase))
            {
                selectedDistanceUnit = "miles";
            }
            else
            {
                selectedDistanceUnit = "km";
            }
        }

        private void CalculateCity()
        {
            var distanceUnit = DistanceUnit.Kilometers;
            if (string.Equals(selectedDistanceUnit, "miles", StringComparison.OrdinalIgnoreCase))
            {
                distanceUnit = DistanceUnit.Miles;
            }
            var cities = GetNearbyCities(SelectedCity, SelectedRange, distanceUnit);
            if (cities.Any())
            {
                var randomCity = cities[_random.Next(cities.Count)];

                string message = $"Success! You will be going to {randomCity}!";
                Snackbar.Clear();
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add(message, Severity.Success, config =>
                {
                    config.RequireInteraction = true;
                    config.ShowCloseIcon = true;
                });
            }
        }

        private IEnumerable<City> FindCities(string searchKey)
        {
            var filteredRecords = Cities
                .OrderByDescending(record => Fuzz.Ratio(searchKey ?? browserLanguage, record.ToString()))
                .ThenBy(record => record.Name)
                .Take(5);
            return filteredRecords;
        }

        private IList<City> GetNearbyCities(City baseCity, double selectedRange, DistanceUnit distanceUnit)
        {
            var filteredList = new List<City>();
            foreach (var city in Cities)
            {
                if (string.Equals(city.Name, baseCity.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var distance = GeoCalculator.GetDistance(baseCity.Latitude, baseCity.Longitude, city.Latitude, city.Longitude, distanceUnit: distanceUnit);
                if (distance < selectedRange)
                {
                    filteredList.Add(city);
                }
            }

            return filteredList;
        }
    }
}