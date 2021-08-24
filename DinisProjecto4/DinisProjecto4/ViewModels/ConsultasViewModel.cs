using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using DinisProjecto4.Service;
using DinisProjecto4.Views;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class ConsultasViewModel : BaseViewModel
    {
        public ConsultasService consultasService { get; set; }
        public ObservableCollection<Consulta> Consultas { get; set; }
        public INavigation Navigation { get; set; }


        public ConsultasViewModel()
        {
            consultasService = new ConsultasService();
            AddConsultaCommand = new Command(async () => await AddConsulta());
            LoadConsultas();

        }

        public async Task<ObservableCollection<Consulta>> LoadConsultas()
        {
            Consultas = toObservablee(await this.consultasService.GetConsultas());
            return Consultas;
        }

        public Command AddConsultaCommand
        {
            get;
        }

        private async Task AddConsulta()
        {
            MainViewModel.GetInstance().newConsulta = new NewConsultaViewModel(false);
            Navigation = MainViewModel.GetInstance().Navigation;
            await Application.Current.MainPage.Navigation.PushAsync(new NewConsultaPage());
        }

        private ObservableCollection<Consulta> toObservablee(List<Consulta> consultas)
        {
            var co = new ObservableCollection<Consulta>();
            foreach (var item in consultas)
            {
                co.Add(
                    new Consulta
                    {
                        Medico = item.Medico,
                        Paciente = item.Paciente,
                        Especialidade = item.Especialidade,
                        Hora = item.Hora,

                    });
            }
            return co;
        }
    }
}