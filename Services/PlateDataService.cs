﻿using Newtonsoft.Json;
using PlateDropletApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PlateDropletApp.Services
{
    public class PlateDataService
    {
        public async Task<Plate> LoadPlateData(string filePath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var plateData = JsonConvert.DeserializeObject<PlateData>(json);

                if (plateData?.PlateDropletInfo?.DropletInfo?.Wells == null)
                    throw new InvalidDataException("Invalid plate data format.");

                var wells = plateData.PlateDropletInfo.DropletInfo.Wells;
                int wellCount = wells.Count;

                Plate plate = InitializePlate(wellCount);
                PopulatePlateWells(plate, wells);

                return plate;
            }
            catch (IOException ex)
            {
                throw new ApplicationException("An error occurred while reading the file.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("An error occurred while parsing the plate data.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load plate data.", ex);
            }
        }

        private Plate InitializePlate(int wellCount)
        {
            Plate plate = new Plate();

            switch (wellCount)
            {
                case 96:
                    plate.Rows = 8;
                    plate.Columns = 12;
                    break;
                case 48:
                    plate.Rows = 8;
                    plate.Columns = 6;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported well count: {wellCount}");
            }

            return plate;
        }

        private void PopulatePlateWells(Plate plate, List<Well> wells)
        {
            for (int i = 0; i < plate.Rows * plate.Columns; i++)
            {
                var well = wells.Find(w => w.WellIndex == i);
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
        }

        private string GetWellName(int index, int columns)
        {
            char row = (char)('A' + index / columns);
            int col = (index % columns) + 1;
            return $"{row}{col:D2}";
        }
    }
}
