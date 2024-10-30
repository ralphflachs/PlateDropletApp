using Prism.Commands;
using Prism.Mvvm;
using PlateDropletApp.Models;
using PlateDropletApp.Services;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlateDropletApp.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifyDataErrorInfo
    {
        private readonly PlateDataService _plateDataService;
        private readonly IFileDialogService _fileDialogService;
        private readonly ErrorsContainer<string> _errorsContainer;

        public MainWindowViewModel(PlateDataService plateDataService, IFileDialogService fileDialogService)
        {
            _plateDataService = plateDataService;
            _fileDialogService = fileDialogService;
            _errorsContainer = new ErrorsContainer<string>(OnErrorsChanged);

            DropletThreshold = GetDefaultDropletThreshold();

            BrowseCommand = new AsyncDelegateCommand(async () => await OnBrowseAsync());
            UpdateThresholdCommand = new DelegateCommand(OnUpdateThreshold);

            ValidateThreshold();
        }

        private Plate _plate;
        public Plate Plate
        {
            get => _plate;
            set => SetProperty(ref _plate, value);
        }

        private int? _dropletThreshold;
        public int? DropletThreshold
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

        private int GetDefaultDropletThreshold()
        {
            int defaultThreshold = 100; // Fallback value
            string thresholdValue = ConfigurationManager.AppSettings["DefaultDropletThreshold"];

            if (int.TryParse(thresholdValue, out int parsedValue))
            {
                return parsedValue;
            }

            return defaultThreshold;
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
            if (!HasErrors && DropletThreshold != null)
            {
                UpdateWellStatuses();
            }
        }

        private void UpdateWellStatuses()
        {
            if (Plate == null || DropletThreshold == null) return;

            int lowDropletCount = 0;

            foreach (var well in Plate.Wells)
            {
                well.IsLowDroplet = well.DropletCount < DropletThreshold.Value;
                if (well.IsLowDroplet)
                {
                    lowDropletCount++;
                }
            }

            LowDropletWellCount = lowDropletCount;
        }

        #region Validation Implementation

        public bool HasErrors => _errorsContainer.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName) => _errorsContainer.GetErrors(propertyName);

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateThreshold()
        {
            const int minThreshold = 0;
            const int maxThreshold = 500;

            _errorsContainer.ClearErrors(nameof(DropletThreshold));

            if (DropletThreshold == null)
            {
                _errorsContainer.SetErrors(nameof(DropletThreshold), new[] { "Threshold cannot be empty." });
            }
            else if (DropletThreshold < minThreshold || DropletThreshold > maxThreshold)
            {
                _errorsContainer.SetErrors(nameof(DropletThreshold), new[] { $"Threshold must be between {minThreshold} and {maxThreshold}." });
            }
        }

        #endregion
    }
}