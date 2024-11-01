using PlateDropletApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateDropletApp.ViewModels
{
    public class RowViewModel : BindableBase
    {
        private string _rowHeader;
        public string RowHeader
        {
            get => _rowHeader;
            set => SetProperty(ref _rowHeader, value);
        }

        private ObservableCollection<WellViewModel> _cells;
        public ObservableCollection<WellViewModel> Cells
        {
            get => _cells;
            set => SetProperty(ref _cells, value);
        }
    }
}
