using Prism.Commands;
using Prism.Mvvm;
using PlateDropletApp.Models;
using PlateDropletApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlateDropletApp.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifyDataErrorInfo
    {
        private readonly PlateDataService _plateDataService;
        private readonly IFileDialogService _fileDialogService;

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

        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateThreshold()
        {
            const int minThreshold = 0;
            const int maxThreshold = 500;
            const string propertyName = nameof(DropletThreshold);

            RemoveError(propertyName); // Clear previous errors

            if (DropletThreshold == null)
            {
                AddError(propertyName, "Threshold cannot be empty.");
            }
            else if (DropletThreshold < minThreshold || DropletThreshold > maxThreshold)
            {
                AddError(propertyName, $"Threshold must be between {minThreshold} and {maxThreshold}.");
            }
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void RemoveError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        #endregion
    }
}