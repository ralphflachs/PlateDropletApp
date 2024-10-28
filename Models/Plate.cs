using System.Collections.ObjectModel;

namespace PlateDropletApp.Models
{
    public class Plate
    {
        public ObservableCollection<Well> Wells { get; set; } = new ObservableCollection<Well>();
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
