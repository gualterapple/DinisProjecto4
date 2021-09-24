using DinisProjecto4.Models;
using DinisProjecto4.Service;
using DinisProjecto4.Views;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class DisponibilidadesViewModel : BaseViewModel
    {

        private bool isRunning;

        public bool IsRunning
        {
            get { return this.isRunning; }
            set
            {
                this.isRunning = value;
                OnPropertyChanged();
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                this.isEnabled = value;
                OnPropertyChanged();
            }
        }

        public DisponibilidadeService disponibilidadesService { get; set; }
        public ObservableCollection<Disponibilidade> Disponibilidades { get; set; }
        public INavigation Navigation { get; set; }

        public DisponibilidadesViewModel()
        {
            disponibilidadesService = new DisponibilidadeService();
            AddDisponibilidadeCommand = new Command(async () => await AddDisponibilidade());
            this.userService = new UserService();
            client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
            LoadDisponibilidades();

            IsEnabled = true;
            IsRunning = false;
                 
        }

        public async Task<ObservableCollection<Disponibilidade>> LoadDisponibilidades()
        {
            if (MainViewModel.GetInstance().Perfil == "Médico")
            {
                Disponibilidades = toObservablee(await this.disponibilidadesService.GetDisponibilidadesByMedico(
                    MainViewModel.GetInstance().Login.Email));
                return Disponibilidades;
            }

            else
            {
                Disponibilidades = toObservablee(await this.disponibilidadesService.GetDisponibilidades());
                return Disponibilidades;
            }
            
        }

        FirebaseClient client;

        private async Task AddDisponibilidade()
        {
            var main = MainViewModel.GetInstance();
            main.newDisponibilidade = new NewDisponibilidadeViewModel(false);
            LoadPacientesAndMedicos();
        }

        public async void LoadPacientesAndMedicos()
        {
            var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil
                });

            var m_ = users.Where(a => a.Perfil == "Médico").ToList();

            MainViewModel.GetInstance().newDisponibilidade.Medicos = toObservableuser(m_);

            Navigation = MainViewModel.GetInstance().Navigation;
            await Application.Current.MainPage.Navigation.PushAsync(new NewDisponibilidadePage());
        }

        public Command AddDisponibilidadeCommand
        {
            get;
        }

        public ObservableCollection<Disponibilidade> toObservablee(List<Disponibilidade> disponibilidades)
        {
            var di = new ObservableCollection<Disponibilidade>();
            foreach (var item in disponibilidades)
            {
                di.Add(
                    new Disponibilidade
                    {
                        Medico = item.Medico,
                        Hora = item.Hora,
                        Data = item.Data,
                        Descricao = item.Descricao,

                    });
            }
            return di;
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