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
        public string Horario { get; set; }
        public Command SelectConsultaCommand
        {
            get;
        }

        private async Task Select()
        {
            var mainViewModel = MainViewModel.GetInstance();
            var consulta = mainViewModel.newConsulta = new NewConsultaViewModel(true);

            consulta.Paciente = this.Paciente;
            consulta.Medico = this.Medico;
            consulta.Horario = this.Horario;
            consulta.Especialidade = this.Especialidade;
            consulta.Descricao = this.Descricao;

            consulta.LastPaciente = this.Paciente;
            consulta.LastMedico = this.Medico;
            consulta.LastHorario = this.Horario;

            MainViewModel.GetInstance().consultas.LoadPacientesAndMedicos();

        }
    }
}
