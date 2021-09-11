using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DinisProjecto4.Models
{
    public class Hospital
    {
        public string Title { get; set; }
        public ObservableCollection<Especialidade> Especialidades { get; set; }
    }
}




