using Prism.Commands;
using Prism.Mvvm;
using PlateDropletApp.Models;
using PlateDropletApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlateDropletApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly PlateDataService _plateDataService;
        private readonly IFileDialogService _fileDialogService;

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
            set
            {
                if (SetProperty(ref _dropletThreshold, value))
                {
                    ValidateThreshold();
                }
            }
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

        public ICommand BrowseCommand { get; }
        public ICommand UpdateThresholdCommand { get; }

        public MainWindowViewModel(PlateDataService plateDataService, IFileDialogService fileDialogService)
        {
            _plateDataService = plateDataService;
            _fileDialogService = fileDialogService;

            DropletThreshold = int.Parse(ConfigurationManager.AppSettings["DefaultDropletThreshold"] ?? "100");

            BrowseCommand = new DelegateCommand(async () => await OnBrowseAsync());
            UpdateThresholdCommand = new DelegateCommand(OnUpdateThreshold);

            ValidateThreshold();
        }

        private async Task OnBrowseAsync()
        {
            var filePath = _fileDialogService.OpenFileDialog();
            if (!string.IsNullOrEmpty(filePath))
            {
                Plate = await _plateDataService.LoadPlateData(filePath);
                TotalWellCount = Plate.Wells.Count;
                UpdateWellStatuses();
            }
        }

        private void OnUpdateThreshold()
        {
            if (ValidateThreshold())
            {
                UpdateWellStatuses();
            }
        }

        private bool ValidateThreshold()
        {
            if (DropletThreshold < 0 || DropletThreshold > 500)
            {
                // Provide feedback to the user, e.g., through a validation message property
                return false;
            }
            return true;
        }

        private void UpdateWellStatuses()
        {
            if (Plate == null) return;

            int lowDropletCount = 0;

            foreach (var well in Plate.Wells)
            {
                well.IsLowDroplet = well.DropletCount < DropletThreshold;
                if (well.IsLowDroplet)
                {
                    lowDropletCount++;
                }
            }

            LowDropletWellCount = lowDropletCount;
        }
    }
}