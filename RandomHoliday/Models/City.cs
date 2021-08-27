using CsvHelper.Configuration.Attributes;
using System;

namespace RandomHoliday.Models
{
    public class City
    {
        [Index(0)]
        public long Id { get; set; }
        [Index(1)]
        public string Name { get; set; }
        [Index(2)]
        public string CountryCode { get; set; }
        [Index(3)]
        public double Latitude { get; set; }
        [Index(4)]
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Name} - {CountryCode}";
        }
    }
}
