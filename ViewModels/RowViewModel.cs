using PlateDropletApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateDropletApp.ViewModels
{
    public class RowViewModel
    {
        public string RowHeader { get; set; }
        public ObservableCollection<Well> Cells { get; set; }
    }
}
