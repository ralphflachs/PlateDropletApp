using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateDropletApp.Models
{
    public class PlateData
    {
        public PlateDropletInfo PlateDropletInfo { get; set; }
    }

    public class PlateDropletInfo
    {
        public DropletInfo DropletInfo { get; set; }
    }

    public class DropletInfo
    {
        public List<Well> Wells { get; set; }
    }
}
