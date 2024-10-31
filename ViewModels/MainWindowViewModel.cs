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
using System.Collections.ObjectModel;

namespace PlateDropletApp.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifyDataErrorInfo
    {
        private const int MinThreshold = 0;
        private const int MaxThreshold = 500;

        private readonly PlateDataService _plateDataService;
        private readonly IFileDialogService _fileDialogService;
        private readonly ErrorsContainer<string> _errorsContainer;

        public MainWindowViewModel(PlateDataService plateDataService, IFileDialogService fileDialogService)
        {
            _plateDataService = plateDataService;
            _fileDialogService = fileDialogService;
            _errorsContainer = new ErrorsContainer<string>(OnErrorsChanged);

            DropletThresholdText = GetDefaultDropletThreshold().ToString();

            BrowseCommand = new AsyncDelegateCommand(async () => await OnBrowseAsync());
            UpdateThresholdCommand = new DelegateCommand(OnUpdateThreshold);

            ValidateDropletThreshold();
        }

        #region Properties

        private Plate _plate;
        public Plate Plate
        {
            get => _plate;
            set => SetProperty(ref _plate, value);
        }

        private string _dropletThresholdText;
        public string DropletThresholdText
        {
            get => _dropletThresholdText;
            set
            {
                if (SetProperty(ref _dropletThresholdText, value))
                {
                    ValidateDropletThreshold();
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

        #endregion

        public ObservableCollection<string> ColumnHeaders { get; set; }
        public ObservableCollection<RowViewModel> PlateRows { get; set; }

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

                InitializeHeadersAndRows();
                UpdateWellStatuses();
            }
        }

        private void OnUpdateThreshold()
        {
            if (!HasErrors)
            {
                UpdateWellStatuses();
            }
        }

        private void InitializeHeadersAndRows()
        {
            // Initialize ColumnHeaders
            ColumnHeaders = new ObservableCollection<string>();
            for (int i = 0; i < Plate.Columns; i++)
            {
                char colHeader = (char)('A' + i);
                ColumnHeaders.Add(colHeader.ToString());
            }
            RaisePropertyChanged(nameof(ColumnHeaders));

            // Initialize PlateRows
            PlateRows = new ObservableCollection<RowViewModel>();
            for (int row = 0; row < Plate.Rows; row++)
            {
                var rowWells = new ObservableCollection<Well>();
                for (int col = 0; col < Plate.Columns; col++)
                {
                    int index = row * Plate.Columns + col;
                    rowWells.Add(Plate.Wells[index]);
                }
                PlateRows.Add(new RowViewModel
                {
                    RowHeader = (row + 1).ToString(),
                    Cells = rowWells
                });
            }
            RaisePropertyChanged(nameof(PlateRows));
        }

        private void UpdateWellStatuses()
        {
            if (Plate == null || !int.TryParse(DropletThresholdText, out int thresholdValue)) return;

            int lowDropletCount = 0;

            foreach (var row in PlateRows)
            {
                foreach (var well in row.Cells)
                {
                    well.IsLowDroplet = well.DropletCount < thresholdValue;
                    if (well.IsLowDroplet)
                    {
                        lowDropletCount++;
                    }
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
            RaisePropertyChanged(nameof(HasErrors));
        }

        private void ValidateDropletThreshold()
        {
            _errorsContainer.ClearErrors(nameof(DropletThresholdText));

            if (string.IsNullOrWhiteSpace(DropletThresholdText))
            {
                _errorsContainer.SetErrors(nameof(DropletThresholdText), new[] { "Threshold cannot be empty." });
            }
            else if (!int.TryParse(DropletThresholdText, out int thresholdValue) ||
                     thresholdValue < MinThreshold || thresholdValue > MaxThreshold)
            {
                _errorsContainer.SetErrors(nameof(DropletThresholdText), new[] { $"Threshold must be a number between {MinThreshold} and {MaxThreshold}." });
            }
        }

        #endregion
    }
}