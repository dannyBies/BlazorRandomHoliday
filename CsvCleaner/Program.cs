using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CsvCleaner
{
    class Program
    {
        static void Main()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                MissingFieldFound = null,
                Delimiter = "\t"
            };
            // Data from http://download.geonames.org/export/dump/
            using var reader = new StreamReader("cities15000.txt");
            using var csvReader = new CsvReader(reader, config);
            var records = csvReader.GetRecords<UnsanitizedCsvData>();

            string path = GetApplicationRoot();
            using var writer = new StreamWriter(Path.Combine(path, "cities.csv"));
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            foreach (var record in records)
            {
                if (record.Population < 1000)
                {
                    continue;
                }
                var sanitizedRecord = new SantizedCsvData
                {
                    Name = record.Name,
                    CountryCode = record.CountryCode,
                    Latitude = record.Latitude,
                    Longitude = record.Longitude,
                };

                csvWriter.WriteRecord(sanitizedRecord);
                csvWriter.NextRecord();
            }
        }

        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }

    public class SantizedCsvData
    {
        [Index(0)]
        public string Name { get; set; }
        [Index(1)]
        public string CountryCode { get; set; }
        [Index(2)]
        public double Latitude { get; set; }
        [Index(3)]
        public double Longitude { get; set; }
    }

    public class UnsanitizedCsvData
    {
        [Index(0)]
        public long GeoNameId { get; set;  }
        [Index(1)]
        public string Name { get; set; }
        [Index(2)]
        public string AsciiName { get; set; }
        [Index(3)]
        public string AlternateNames { get; set; }
        [Index(4)]
        public double Latitude { get; set; }
        [Index(5)]
        public double Longitude { get; set; }
        [Index(6)]
        public string FeatureClass { get; set; }
        [Index(7)]
        public string FeatureCode { get; set; }
        [Index(8)]
        public string CountryCode { get; set; }
        [Index(9)]
        public string CC2 { get; set; }
        [Index(10)]
        public string Admin1Code { get; set; }
        [Index(11)]
        public string Admin2Code { get; set; }
        [Index(12)]
        public string Admin3Code { get; set; }
        [Index(13)]
        public string Admin4Code { get; set; }
        [Index(14)]
        public long Population { get; set; }
        [Index(15)]
        public string Elevation { get; set; }
        [Index(16)]
        public string Dem { get; set; }
        [Index(17)]
        public string Timezone { get; set; }
        [Index(18)]
        public string ModificationDate { get; set; }
    }
}
