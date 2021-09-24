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
    public class NewConsultaViewModel : BaseViewModel
    {
        FirebaseClient client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
        public INavigation Navigation { get; set; }

        UserService userService = new UserService();
        DisponibilidadeService dispService = new DisponibilidadeService();
        HospitaisService hService = new HospitaisService();
        ConsultasService consultasService = new ConsultasService();

        private string paciente;
        private string medico;
        private string especialidade;
        private string hospital;
        private string descricao;
        private string desponibilidade;
        private bool mostrarPaciente; 

        private DateTime data;
        private string horario;

        private Especialidade selectedEspecialidade;
        private Hospital selectedHospital;
        private Disponibilidade selectedDisponibilidade;
        
        private User selectedPaciente;
        private User selectedMedico;

        public ObservableCollection<User> Pacientes { get; set; }
        private ObservableCollection<User> medicos;

        public ObservableCollection<User> Medicos
        {
            get { return this.medicos; }
            set { SetValue(ref this.medicos, value); }
        }

        public bool MostrarPaciente
        {
            get { return this.mostrarPaciente; }
            set { SetValue(ref this.mostrarPaciente, value); }
        }
        

        ObservableCollection<Especialidade> especialidades1 = new ObservableCollection<Especialidade>();
        ObservableCollection<Especialidade> especialidades2 = new ObservableCollection<Especialidade>();

        public string LastDescricao { get; set; }
        public string LastPaciente { get; set; }
        public string LastMedico { get; set; }
        public string LastHorario { get; set; }
        public bool IsEditing { get; set; }
        public string Descricao
        {
            get { return this.descricao; }
            set
            {
                this.descricao = value;
                OnPropertyChanged();
            }
        }

        public string Paciente
        {
            get { return this.paciente; }
            set
            {
                this.paciente = value;
                OnPropertyChanged();
            }
        }

        public string Medico
        {
            get { return this.medico; }
            set
            {
                this.medico = value;
                OnPropertyChanged();
            }
        }

        public DateTime Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
                OnPropertyChanged();
            }
        }

        public string Horario
        {
            get { return this.horario; }
            set
            {
                this.horario = value;
                OnPropertyChanged();
            }
        }

        public string Especialidade
        {
            get { return this.especialidade; }
            set
            {
                this.especialidade = value;
                OnPropertyChanged();
            }
        }

        public string Hospital
        {
            get { return this.hospital; }
            set
            {
                this.hospital = value;
                OnPropertyChanged();
            }
        }
        public Consulta ConsultaRecebida { get; set; }

        public string Disponibilidade
        {
            get { return this.desponibilidade; }
            set
            {
                this.desponibilidade = value;
                OnPropertyChanged();
            }
        }

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

        public NewConsultaViewModel(bool editing, Consulta consulta)
        {
            IsEditing = editing;

            MostrarPaciente = false;

            HospitaisService hService = new HospitaisService();
            Hospitais = hService.LoadHospitais();

            CriarContaCommand = new Command(async () => await Register());
            AtualizarContaCommand = new Command(async () => await Update());
            DeleteContaCommand = new Command(async () => await Delete());

            Disponibilidades = new ObservableCollection<Disponibilidade>();            
            Especialidades = new ObservableCollection<Especialidade>();

            LoadPacientesAndMedicos();

            if (editing)
            {
                ConsultaRecebida = new Consulta();
                ConsultaRecebida = consulta;

                LastPaciente = ConsultaRecebida.Paciente;
                LastMedico = ConsultaRecebida.Medico;
                LastHorario = ConsultaRecebida.Horario;

                Especialidade = consulta.Especialidade;
                foreach (var item in Especialidades)
                {
                    if(item.Title == this.Especialidade)
                    selectedEspecialidade = item;
                }

                OnPropertyChanged();
            }

            if (MainViewModel.GetInstance().Perfil != "Paciente")
                MostrarPaciente = true;

        }

        private void CarregarDadosDoMedico()
        {
            foreach (var item in Medicos)
            {
                if(item.UserName == MainViewModel.GetInstance().Login.Email)
                {
                    Medico = item.UserName;
                    Especialidade = item.Especialidade;
                    Hospital = item.Hospital;
                }
                    
            }
            LoadDisponibilidade(Medico);

        }

        public Especialidade SelectedEspecialidade
        {
            get { return selectedEspecialidade; }
            set
            {

                if (selectedEspecialidade != value)
                {
                    selectedEspecialidade = value;
                    Especialidade = selectedEspecialidade.Title;
                    LoadMedicos(Especialidade, selectedHospital.Title);
                    OnPropertyChanged();

                }
            }
        }

        private async void LoadMedicos(string especialidade, string hospital)
        {
            selectedMedico = null;
            Medicos = new ObservableCollection<User>();
            Medicos = toObservablee(await userService.GetMedicoByEspecialidade(especialidade, hospital));
        }

        private async void LoadDisponibilidade(string medico)
        {
            selectedDisponibilidade = null;
            Disponibilidades = new ObservableCollection<Disponibilidade>();
            Disponibilidades = toObservableDisponibilidade(await dispService.GetDisponibilidadesByMedico(medico));
        }

        public ObservableCollection<User> toObservablee(List<User> users)
        {
            var us = new ObservableCollection<User>();
            foreach (var item in users)
            {
                us.Add(
                    new User
                    {
                        UserName = item.UserName,
                        Password = item.Password,
                        Perfil = item.Perfil,
                        Hospital = item.Hospital
                    });
            }
            return us;
        }

        public ObservableCollection<Disponibilidade> toObservableDisponibilidade(List<Disponibilidade> disponibilidades)
        {
            var us = new ObservableCollection<Disponibilidade>();
            foreach (var item in disponibilidades)
            {
                us.Add(
                    new Disponibilidade
                    {
                        Descricao = item.Descricao,
                        Medico = item.Medico,
                        Data = item.Data,
                        Hora = item.Hora
                    });
            }
            return us;
        }

        public Hospital SelectedHospital
        {
            get { return selectedHospital; }
            set
            {

                if (selectedHospital != value)
                {
                    selectedHospital = value;
                    Hospital = selectedHospital.Title;

                    selectedEspecialidade = null;
                    selectedDisponibilidade = null;
                    selectedMedico = null;

                    foreach (var item in Hospitais)
                    {
                        if(item.Title == Hospital)
                        {
                            Especialidades = item.Especialidades;
                        }
                    }

                    OnPropertyChanged();
                }
            }
        }

        public Disponibilidade SelectedDisponibilidade
        {
            
            get { return selectedDisponibilidade; }
            set
            {

                if (selectedDisponibilidade != value)
                {
                    selectedDisponibilidade = value;
                    Disponibilidade = selectedDisponibilidade.Descricao;
                    OnPropertyChanged();
                }
            }
        }

        public User SelectedPaciente
        {
            get { return selectedPaciente; }
            set
            {

                if (selectedPaciente != value)
                {
                    selectedPaciente = value;
                    Paciente = selectedPaciente.UserName;
                    OnPropertyChanged();
                }
            }
        }

        public User SelectedMedico
        {
            get { return selectedMedico; }
            set
            {

                if (selectedMedico != value)
                {
                    selectedMedico = value;
                    Medico = selectedMedico.UserName;
                    LoadDisponibilidade(Medico);
                    OnPropertyChanged();
                }
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

        private ObservableCollection<Especialidade> especialidades;

        public ObservableCollection<Especialidade> Especialidades
        {
            get { return this.especialidades; }
            set { SetValue(ref this.especialidades, value); }
        }

        private ObservableCollection<Hospital> hospitais;

        public ObservableCollection<Hospital> Hospitais
        {
            get { return this.hospitais; }
            set { SetValue(ref this.hospitais, value); }
        }

        private ObservableCollection<Disponibilidade> disponibilidades;

        public ObservableCollection<Disponibilidade> Disponibilidades
        {
            get { return this.disponibilidades; }
            set { SetValue(ref this.disponibilidades, value); }
        }

        /*public IEnumerable<Especialidade> LoadEspecialidade()
        {

            Especialidades = new ObservableCollection<Especialidade>
            {
                new Especialidade
                {
                    Title = "Gastro Interiologia",
                },
                new Especialidade
                {
                    Title = "Neurologia",
                }
            };
            return Especialidades;
        }*/

        /*public IEnumerable<Hospital> LoadHospitais()
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
        }*/
        public async void LoadPacientesAndMedicos()
        {

            var users = (await this.client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil,
                    Especialidade = u.Object.Especialidade,
                    Hospital = u.Object.Hospital
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
            var d_ = disps.ToList();

            Pacientes = MainViewModel.GetInstance().consultas.toObservableuser(p_);
            Medicos = MainViewModel.GetInstance().consultas.toObservableuser(m_);
            Disponibilidades = MainViewModel.GetInstance().disponibilidades.toObservablee(d_);
            Hospitais = hService.LoadHospitais();

            if (MainViewModel.GetInstance().Perfil == "Médico")
            {
                CarregarDadosDoMedico();
            }

            if (IsEditing)
            {

                Medico = ConsultaRecebida.Medico;
                foreach (var item in Medicos)
                {
                    if (item.UserName == this.Medico)
                        selectedMedico = item;
                }

                Paciente = ConsultaRecebida.Paciente;
                foreach (var item in Pacientes)
                {
                    if (item.UserName == this.Paciente)
                        selectedPaciente = item;
                }

                Disponibilidade = ConsultaRecebida.Horario;
                foreach (var item in Disponibilidades)
                {
                    if (item.Descricao == this.Disponibilidade)
                        selectedDisponibilidade = item;
                }

                Hospital = ConsultaRecebida.Hospital;
                foreach (var item in Hospitais)
                {
                    if (item.Title == this.Hospital) {
                        selectedHospital = item;
                        Especialidades = item.Especialidades;
                    }
                }

                Especialidade = ConsultaRecebida.Especialidade;
                foreach (var item in Especialidades)
                {
                    if (item.Title == this.Especialidade)                    
                        selectedEspecialidade = item;                    
                }

                Descricao = ConsultaRecebida.Descricao;

                OnPropertyChanged();
            }

            Navigation = MainViewModel.GetInstance().Navigation;
            await Application.Current.MainPage.Navigation.PushAsync(new NewConsultaPage());

        }

        public Command CriarContaCommand
        {
            get;
        }

        public Command AtualizarContaCommand
        {
            get;
        }

        public Command DeleteContaCommand
        {
            get;
        }

        public async Task Register()
        {
            try
            {
                StartLoading();
                if (!await ValidarCampos())
                {
                    StopLoading();
                    return;
                }

                var consultaService = new ConsultasService();
                if (await consultaService.NovaConsulta(Paciente, Medico, Especialidade, Disponibilidade, Descricao, Hospital))
                {

                    var mConsulta = MainViewModel.GetInstance().consultas;
                    mConsulta.LoadConsultas();

                    await Application.Current.MainPage.DisplayAlert(
                    "Informação",
                    "Consulta registada com sucesso!",
                    "Accept");

                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Já foi adicionada uma consulta com esta descrição!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao registar a consulta, volte a tentar",
                        "Accept");

                StopLoading();
                return;
            }
        }
        public async Task Update()
        {
            try
            {
                StartLoading();
                if (!await ValidarCampos())
                {
                    StopLoading();
                    return;
                }

                var consultaService = new ConsultasService();
                if (await consultaService.updateConsulta(LastDescricao, Descricao, Paciente, Medico, Especialidade, Disponibilidade, Hospital, LastPaciente, LastMedico, LastHorario))
                {
                    
                    if (MainViewModel.GetInstance().Perfil == "Paciente")
                    {
                        MainViewModel.GetInstance().consultas.Consultas = MainViewModel.GetInstance().consultas.toObservablee(await this.consultasService.GetConsultasByPaciente(
                        MainViewModel.GetInstance().Login.Email));
                    }

                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Consulta atualizada com sucesso!",
                        "Accept");

                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Não foi possível atualizar os dados desta consulta, tente novamente!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao atualizar a consulta, volte a tentar",
                        "Accept");

                StopLoading();
                return;
            }
        }
        public async Task Delete()
        {
            try
            {
              
                var consultaService = new ConsultasService();
                if (await consultaService.DeleteConsulta(Paciente, Medico, Disponibilidade))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Consulta apagada com sucesso!",
                        "Accept");

                    var consultas = new ConsultasViewModel();
                    await consultas.LoadConsultas();
                    MainViewModel.GetInstance().consultas = consultas;
                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Não foi possível apagar os dados desta consulta, tente novamente!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao apagar a consulta",
                        "Accept");

                StopLoading();
                return;
            }
        }
        public void StartLoading()
        {
            this.IsRunning = true;
            this.IsEnabled = false;
        }

        public void StopLoading()
        {
            this.IsRunning = false;
            this.IsEnabled = true;
        }

        private async Task<bool> ValidarCampos()
        {
            if (MostrarPaciente)
            {
                if (string.IsNullOrEmpty(this.Paciente))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo Paciente está vázio",
                        "Digite o seu nome do Paciente",
                        "OK");

                    return false;
                }
            }
            else
            Paciente = MainViewModel.GetInstance().Login.Email;

            if (string.IsNullOrEmpty(this.Medico))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo Medico está vázio",
                    "Digite o nome do médico",
                    "OK");
                this.Medico = string.Empty;
                return false;
            }

            if (string.IsNullOrEmpty(this.Especialidade))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Especialidade não seleccionada",
                    "Selecione a especialidade",
                    "OK");
                this.Especialidade = string.Empty;
                return false;
            }

            if (string.IsNullOrEmpty(this.Disponibilidade.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Data",
                    "Defina o horario da consulta",
                    "OK");
                return false;
            }
            if (string.IsNullOrEmpty(this.Descricao))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Data",
                    "Descreva o motivo da consulta",
                    "OK");
                return false;
            }

            if (string.IsNullOrEmpty(this.Hospital))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Data",
                    "Escolha o hospital",
                    "OK");
                return false;
            }

            return true;
        }

    }
}
