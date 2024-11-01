namespace PlateDropletApp.Models
{
    public class Well
    {
        public int WellIndex { get; set; }
        public string WellName { get; set; }

        public int DropletCount { get; set; }

        public bool IsLowDroplet { get; set; }
    }
}
