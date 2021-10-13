using System;
using System.Threading.Tasks;
using DinisProjecto4.ViewModels;
using DinisProjecto4.Views;
using Xamarin.Forms;

namespace DinisProjecto4.Models
{
    public class Consulta
    {

        public Consulta()
        {
            SelectConsultaCommand = new Command(async () => await Select());
        }

        public string Descricao { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public string Especialidade { get; set; }
        public string Hospital { get; set; }

        public string Horario { get; set; }
        public Command SelectConsultaCommand
        {
            get;
        }
        public DateTime Data { get; set; }

        public string Color { get; set; }

        private async Task Select()
        {
            MainViewModel.GetInstance().newConsulta = new NewConsultaViewModel(true, this);           

        }
    }
}
