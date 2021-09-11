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
        public DisponibilidadeService disponibilidadesService { get; set; }

        public ObservableCollection<Disponibilidade> Disponibilidades { get; set; }
        public INavigation Navigation { get; set; }


        public ConsultasViewModel()
        {
            consultasService = new ConsultasService();
            disponibilidadesService = new DisponibilidadeService();
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

        public async Task<ObservableCollection<Disponibilidade>> LoadDisponibilidades()
        {
            Disponibilidades = MainViewModel.GetInstance().disponibilidades.toObservablee(await this.disponibilidadesService.GetDisponibilidades());
            return Disponibilidades;
        }

        FirebaseClient client;

        public async void LoadPacientesAndMedicos()
        {
            try
            {
                client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");

                var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil
                });

                var disps = (await client.Child("Disponibilidades")
                   .OnceAsync<Disponibilidade>()).Select(u => new Disponibilidade
                   {
                       Descricao = u.Object.Descricao,
                       Medico = u.Object.Medico,
                       Data = u.Object.Data,
                       Hora = u.Object.Hora,
                   });

                var p_ = users.Where(a => a.Perfil == "Paciente").ToList();
                var m_ = users.Where(a => a.Perfil == "Médico").ToList();
                var d_ = disps.Where(a => a.Descricao != "").ToList();


                MainViewModel.GetInstance().newConsulta.Pacientes = toObservableuser(p_);
                //MainViewModel.GetInstance().newConsulta.Medicos = toObservableuser(m_);
                MainViewModel.GetInstance().newConsulta.Disponibilidades = toObservableDispo(d_);


                Navigation = MainViewModel.GetInstance().Navigation;
                await Application.Current.MainPage.Navigation.PushAsync(new NewConsultaPage());
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert(
                                   "Erro",
                                   "Ocorreu um erro ao carregar dos dados, por favor volte a tentar!",
                                   "Ok");
            }
            
        }

        public Command AddConsultaCommand
        {
            get;
        }

        private async Task AddConsulta()
        {
            var main = MainViewModel.GetInstance();
            main.newConsulta = new NewConsultaViewModel(false, null);
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
                        Horario = item.Horario,
                        Descricao = item.Descricao

                    });
            }
            return co;
        }

        public ObservableCollection<User> toObservableuser(List<User> users)
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

        private ObservableCollection<Disponibilidade> toObservableDispo(List<Disponibilidade> disps)
        {
            var di = new ObservableCollection<Disponibilidade>();
            foreach (var item in disps)
            {
                di.Add(
                    new Disponibilidade
                    {
                        Descricao = item.Descricao,
                        Medico = item.Medico,
                        Data = item.Data,
                        Hora = item.Hora

                    });
            }
            return di;
        }

        public UserService userService { get; set; }




    }
}