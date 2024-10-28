using Newtonsoft.Json.Linq;
using PlateDropletApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlateDropletApp.Services
{
    public class PlateDataService
    {
        public Plate LoadPlateData(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var jObject = JObject.Parse(json);

            var wellsArray = jObject["PlateDropletInfo"]["DropletInfo"]["Wells"] as JArray;

            var wells = wellsArray.Select(w => new Well
            {
                WellIndex = (int)w["WellIndex"],
                WellName = (string)w["WellName"],
                DropletCount = (int)w["DropletCount"]
            }).ToList();

            int wellCount = wells.Count;
            Plate plate = new Plate();

            if (wellCount == 96)
            {
                plate.Rows = 8;
                plate.Columns = 12;
            }
            else if (wellCount == 48)
            {
                plate.Rows = 8;
                plate.Columns = 6;
            }
            else
            {
                // Handle unexpected well counts
                plate.Rows = 0;
                plate.Columns = 0;
            }

            // Initialize wells in order
            for (int i = 0; i < plate.Rows * plate.Columns; i++)
            {
                var well = wells.FirstOrDefault(w => w.WellIndex == i);
                if (well == null)
                {
                    well = new Well
                    {
                        WellIndex = i,
                        WellName = GetWellName(i, plate.Columns),
                        DropletCount = 0
                    };
                }
                plate.Wells.Add(well);
            }

            return plate;
        }

        private string GetWellName(int index, int columns)
        {
            char row = (char)('A' + index / columns);
            int col = (index % columns) + 1;
            return $"{row}{col:D2}";
        }
    }
}
