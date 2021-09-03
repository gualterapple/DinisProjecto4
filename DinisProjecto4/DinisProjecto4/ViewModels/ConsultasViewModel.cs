using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using DinisProjecto4.Service;
using DinisProjecto4.Views;
using Firebase.Database;
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
            this.userService = new UserService();
            client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
            LoadConsultas();

        }

        public async Task<ObservableCollection<Consulta>> LoadConsultas()
        {
            Consultas = toObservablee(await this.consultasService.GetConsultas());

            return Consultas;
        }

        FirebaseClient client;

        public async void LoadPacientesAndMedicos()
        {
            var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil
                });

            var p_ = users.Where(a => a.Perfil == "Paciente").ToList();
            var m_ = users.Where(a => a.Perfil == "Médico").ToList();

            MainViewModel.GetInstance().newConsulta.Pacientes = toObservableuser(p_);
            MainViewModel.GetInstance().newConsulta.Medicos = toObservableuser(m_);

            Navigation = MainViewModel.GetInstance().Navigation;
            await Application.Current.MainPage.Navigation.PushAsync(new NewConsultaPage());
        }

        public Command AddConsultaCommand
        {
            get;
        }

        private async Task AddConsulta()
        {
            var main = MainViewModel.GetInstance();
            main.newConsulta = new NewConsultaViewModel(false);
            LoadPacientesAndMedicos();
        }

        public ObservableCollection<Consulta> toObservablee(List<Consulta> consultas)
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
                        Data = item.Data

                    });
            }
            return co;
        }

        private ObservableCollection<User> toObservableuser(List<User> users)
        {
            var co = new ObservableCollection<User>();
            foreach (var item in users)
            {
                co.Add(
                    new User
                    {
                        UserName = item.UserName,
                        Password = item.Password,
                        Perfil = item.Perfil,

                    });
            }
            return co;
        }

        public UserService userService { get; set; }




    }
}