using PlateDropletApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateDropletApp.ViewModels
{

    public class WellViewModel : BindableBase
    {
        public Well Well { get; }

        private bool _isLowDroplet;
        public bool IsLowDroplet
        {
            get => _isLowDroplet;
            set => SetProperty(ref _isLowDroplet, value);
        }

        public WellViewModel(Well well)
        {
            Well = well;
        }

        public int DropletCount
        {
            get => Well.DropletCount;
            set
            {
                if (Well.DropletCount != value)
                {
                    Well.DropletCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string WellName => Well.WellName;
    }
}
