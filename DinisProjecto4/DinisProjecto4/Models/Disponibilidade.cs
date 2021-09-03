using DinisProjecto4.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DinisProjecto4.Models
{
    public class Disponibilidade
    {
        public string Medico { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }

        public Disponibilidade()
        {
            SelectDisponibilidadeCommand = new Command(async () => await Select());
        }

        public Command SelectDisponibilidadeCommand
        {
            get;
        }

        private async Task Select()
        {
            var mainViewModel = MainViewModel.GetInstance();
            var disponibilidade = mainViewModel.newDisponibilidade = new NewDisponibilidadeViewModel(true);

            disponibilidade.Medico = this.Medico;
            disponibilidade.Hora = this.Hora;
            disponibilidade.Data = this.Data;

            disponibilidade.LastMedico = this.Medico;
            disponibilidade.LastHora = this.Hora;

            MainViewModel.GetInstance().disponibilidades.LoadPacientesAndMedicos();

        }
    }
}
