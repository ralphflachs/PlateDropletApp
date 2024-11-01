using System.Collections.Generic;

namespace PlateDropletApp.Models
{
    public class Plate
    {
        public List<Well> Wells { get; set; } = new List<Well>();
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}