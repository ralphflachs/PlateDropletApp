using Prism.Mvvm;

namespace PlateDropletApp.Models
{
    public class Well : BindableBase
    {
        private int _dropletCount;
        private bool _isLowDroplet;

        public int WellIndex { get; set; }
        public string WellName { get; set; }

        public int DropletCount
        {
            get => _dropletCount;
            set => SetProperty(ref _dropletCount, value);
        }

        public bool IsLowDroplet
        {
            get => _isLowDroplet;
            set => SetProperty(ref _isLowDroplet, value);
        }
    }
}
