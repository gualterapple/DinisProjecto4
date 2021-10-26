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
        public ConsultasService consultasService { get; set; }

        private ObservableCollection<Consulta> consultas;

        public ObservableCollection<Consulta> Consultas
        {
            get { return this.consultas; }
            set { SetValue(ref this.consultas, value); }
        }
        public DisponibilidadeService disponibilidadesService { get; set; }

        private ObservableCollection<Consulta> consultasRecebe;

        public ObservableCollection<Consulta> ConsultasRecebe
        {
            get { return this.consultasRecebe; }
            set { SetValue(ref this.consultasRecebe, value); }
        }

        public ObservableCollection<Disponibilidade> Disponibilidades { get; set; }
        public INavigation Navigation { get; set; }


        public ConsultasViewModel()
        {
            consultasService = new ConsultasService();
            disponibilidadesService = new DisponibilidadeService();
            AddConsultaCommand = new Command(async () => await AddConsulta());
            SearchCommand = new Command(Search);

            this.userService = new UserService();
            ConsultasRecebe = new ObservableCollection<Consulta>();
            client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
            IsRunning = false;
            IsEnabled = true;
            LoadConsultas();

        }

        public Command SearchCommand
        {
            get;
        }

        public async Task<ObservableCollection<Consulta>> LoadConsultas()
        {
            if(MainViewModel.GetInstance().Perfil == "Paciente")
            {
                Consultas = toObservablee(await this.consultasService.GetConsultasByPaciente(
                    MainViewModel.GetInstance().Login.Email));
            }
            if (MainViewModel.GetInstance().Perfil == "Médico")
            {
                Consultas = toObservablee(await this.consultasService.GetConsultasByMedico(
                    MainViewModel.GetInstance().Login.Email));
            }
            if (MainViewModel.GetInstance().Perfil == "Antendente")
            {
                Consultas = toObservablee(await this.consultasService.GetConsultas());
            }
            if (MainViewModel.GetInstance().Perfil == "Administrador")
            {
                Consultas = toObservablee(await this.consultasService.GetConsultas());
            }

            //Consultas.OrderBy(c => c.Horario);
            ConsultasRecebe = Consultas;

            return Consultas;
        }

     

        public async Task<ObservableCollection<Disponibilidade>> LoadDisponibilidades()
        {
            Disponibilidades = MainViewModel.GetInstance().disponibilidades.toObservablee(await this.disponibilidadesService.GetDisponibilidades());
            Disponibilidades.OrderBy(d => d.Hora);
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
                    FullName = u.Object.FullName,
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


                /*Navigation = MainViewModel.GetInstance().Navigation;
                await Application.Current.MainPage.Navigation.PushAsync(new NewConsultaPage());*/
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
            try
            {
                var main = MainViewModel.GetInstance();
                main.newConsulta = new NewConsultaViewModel(false, null);
                LoadPacientesAndMedicos();
            }
            catch (Exception)
            {

                return;
            }
        }

        public ObservableCollection<Consulta> toObservablee(List<Consulta> consultas)
        {
            var co = new ObservableCollection<Consulta>();
            foreach (var item in consultas)
            {
                var cor = "";
                var dConsulta = Convert.ToDateTime(item.Horario);
                var dHoje = Convert.ToDateTime(DateTime.Now);

                TimeSpan t =  dConsulta - dHoje;
                double dias = t.TotalDays;
                if(dias >= 0)
                cor = "green";
                else
                cor = "red";

                co.Add(
                    new Consulta
                    {
                        Medico = item.Medico,
                        Paciente = item.Paciente,
                        Especialidade = item.Especialidade,
                        Horario = item.Horario,
                        Descricao = item.Descricao,
                        Hospital = item.Hospital,
                        Color = cor

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
                        FullName = item.FullName,
                        UserName = item.UserName,
                        Password = item.Password,
                        Perfil = item.Perfil,
                        Especialidade = item.Especialidade,
                        Hospital = item.Hospital

                    });
            }
            return co;
        }

        private string filter;

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }

        private void Search()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Filter))
                {
                    this.LoadConsultasToSearch();
                }
                else
                {
                    Consultas = new ObservableCollection<Consulta>(
                        this.ToConsulta().Where(
                            l => l.Hospital.ToString().ToLower().Contains(this.Filter.ToLower())
                            || l.Paciente.ToString().ToLower().Contains(this.Filter.ToLower())
                            || l.Medico.ToString().ToLower().Contains(this.Filter.ToLower())
                            || l.Especialidade.ToString().ToLower().Contains(this.Filter.ToLower())));
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }

        private IEnumerable<Consulta> ToConsulta()
        {
            return this.consultasRecebe.Select(o => new Consulta
            {
                Medico = o.Medico,
                Paciente = o.Paciente,
                Descricao = o.Descricao,
                Especialidade = o.Especialidade,
                Hospital = o.Hospital

            });
        }

        public void LoadConsultasToSearch()
        {
            Consultas = ConsultasRecebe;
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