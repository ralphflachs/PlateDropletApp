using Prism.Commands;
using Prism.Mvvm;
using PlateDropletApp.Models;
using PlateDropletApp.Services;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace PlateDropletApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private PlateDataService _plateDataService = new PlateDataService();

        private Plate _plate;
        public Plate Plate
        {
            get => _plate;
            set => SetProperty(ref _plate, value);
        }

        private int _dropletThreshold;
        public int DropletThreshold
        {
            get => _dropletThreshold;
            set => SetProperty(ref _dropletThreshold, value);
        }

        private int _totalWellCount;
        public int TotalWellCount
        {
            get => _totalWellCount;
            set => SetProperty(ref _totalWellCount, value);
        }

        private int _lowDropletWellCount;
        public int LowDropletWellCount
        {
            get => _lowDropletWellCount;
            set => SetProperty(ref _lowDropletWellCount, value);
        }

        public ICommand BrowseCommand { get; set; }
        public ICommand UpdateThresholdCommand { get; set; }

        public MainWindowViewModel()
        {
            // Set default droplet threshold directly
            DropletThreshold = 100;

            // Initialize commands
            BrowseCommand = new DelegateCommand(OnBrowse);
            UpdateThresholdCommand = new DelegateCommand(OnUpdateThreshold);
        }

        private void OnBrowse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                Plate = _plateDataService.LoadPlateData(openFileDialog.FileName);
                TotalWellCount = Plate.Wells.Count;
                UpdateWellStatuses();
            }
        }

        private void OnUpdateThreshold()
        {
            if (DropletThreshold < 0 || DropletThreshold > 500)
                return;

            UpdateWellStatuses();
        }

        private void UpdateWellStatuses()
        {
            if (Plate == null) return;

            int lowDropletCount = 0;

            foreach (var well in Plate.Wells)
            {
                if (well.DropletCount >= DropletThreshold)
                {
                    well.DisplayText = "n";
                    well.IsLowDroplet = false;
                    well.BackgroundColor = "White";
        }
                else
                {
                    well.DisplayText = "L";
                    well.IsLowDroplet = true;
                    well.BackgroundColor = "Gray";
                    lowDropletCount++;
    }
}

            LowDropletWellCount = lowDropletCount;
        }
    }
}