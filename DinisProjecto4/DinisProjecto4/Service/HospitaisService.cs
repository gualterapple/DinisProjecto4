using DinisProjecto4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DinisProjecto4.Service
{
    public class HospitaisService
    {
        ObservableCollection<Especialidade> especialidades1 = new ObservableCollection<Especialidade>();
        ObservableCollection<Especialidade> especialidades2 = new ObservableCollection<Especialidade>();

        public ObservableCollection<Hospital> Hospitais = new ObservableCollection<Hospital>();
        public ObservableCollection<Hospital> LoadHospitais()
        {
            especialidades1 = new ObservableCollection<Especialidade>();
            especialidades1.Add(new Especialidade { Title = "Gastro Interiologia" });
            especialidades1.Add(new Especialidade { Title = "Neurologia" });

            especialidades2 = new ObservableCollection<Especialidade>();
            especialidades2.Add(new Especialidade { Title = "Psicologia" });
            especialidades2.Add(new Especialidade { Title = "Fisioterapia" });

            Hospitais = new ObservableCollection<Hospital>
            {
                new Hospital
                {
                    Title = "Cligest",
                    Especialidades = especialidades1
                },
                new Hospital
                {
                    Title = "Multi Perfil",
                    Especialidades = especialidades2
                }
            };
            return Hospitais;
        }
    }
}
