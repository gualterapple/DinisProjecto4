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
    public class NewUserViewModel:BaseViewModel
    {
        private string userName;
        private string password;
        private string perfil;
        private string especialidade;
        private string hospital;
        private Perfil selectedPerfil;
        private Especialidade selectedEspecialidade; 
        private Genero selectedGenero; 
        private Hospital selectedHospital;
        private bool isMedico;
        private bool isNewRegister;

        private string fullName;
        private string email;
        private string telefone;
        private string address;
        private string genero;
        private DateTime dataNascimento;


        public string LastName { get; set; }
        public bool IsEditing { get; set; }

        public bool IsMedico
        {
            get { return this.isMedico; }
            set { SetValue(ref this.isMedico, value); }
        }
        public bool IsNewRegister
        {
            get { return this.isNewRegister; }
            set { SetValue(ref this.isNewRegister, value); }
        }

        public string FullName
        {
            get { return this.fullName; }
            set { SetValue(ref this.fullName, value); }
        }
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Telefone
        {
            get { return this.telefone; }
            set { SetValue(ref this.telefone, value); }
        }
        public string Address
        {
            get { return this.address; }
            set { SetValue(ref this.address, value); }
        }
        public string Genero
        {
            get { return this.genero; }
            set { SetValue(ref this.genero, value); }
        }
        public DateTime DataNascimento
        {
            get { return this.dataNascimento; }
            set { SetValue(ref this.dataNascimento, value); }
        }

        HospitaisService hService = new HospitaisService();

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

        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
                OnPropertyChanged();
            }
        }

        public string Perfil
        {
            get { return this.perfil; }
            set
            {
                this.perfil = value;
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

        public Perfil SelectedPerfil
        {
            get { return selectedPerfil; }
            set
            {

                if (selectedPerfil != value)
                {
                    selectedPerfil = value;
                    Perfil = selectedPerfil.Title;
                    if(Perfil == "Médico")
                    IsMedico = true;
                    else
                    IsMedico = false;
                    OnPropertyChanged();
                }
            }
        }
        public Genero SelectedGenero
        {
            get { return selectedGenero; }
            set
            {

                if (selectedGenero != value)
                {
                    selectedGenero = value;
                    Genero = selectedGenero.Title;
                    
                    OnPropertyChanged();
                }
            }
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
                    OnPropertyChanged();
                }
            }
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

                    foreach (var item in Hospitais)
                    {
                        if (item.Title == Hospital)
                        {
                            selectedEspecialidade = null;
                            Especialidades = item.Especialidades;
                        }
                    }

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

        public NewUserViewModel(bool editing, User user)
        {
            IsMedico = false;

            CriarContaCommand = new Command(async () => await Register());
            AtualizarContaCommand = new Command(async () => await Update());
            DeleteContaCommand = new Command(async () => await Delete());
            VoltarContaCommand = new Command(async () => await GoBack());

            
            Hospitais = hService.LoadHospitais();
            Especialidades = new ObservableCollection<Especialidade>();

            IsEditing = editing;
            LoadPerfis();
            LoadGeneros();

            if (IsEditing)
            {
                Perfil = user.Perfil;
                foreach (var item in Perfis)
                {
                    if (item.Title == Perfil)
                        selectedPerfil = item;
                }
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Perfil> perfis;
        private ObservableCollection<Genero> generos;


        public ObservableCollection<Perfil> Perfis
        {
            get { return this.perfis; }
            set { SetValue(ref this.perfis, value); }
        }

        public ObservableCollection<Genero> Generos
        {
            get { return this.generos; }
            set { SetValue(ref this.generos, value); }
        }
        

        public IEnumerable<Perfil> LoadPerfis()
        {

            Perfis = new ObservableCollection<Perfil>
            {
                new Perfil
                {
                    Title = "Administrador",
                },
                new Perfil
                {
                    Title = "Médico",
                },
                new Perfil
                {
                    Title = "Paciente",
                },
                new Perfil
                {
                    Title = "Antendente",
                }
            };
            return Perfis;
        }

        public IEnumerable<Genero> LoadGeneros()
        {

            Generos = new ObservableCollection<Genero>
            {
                new Genero
                {
                    Title = "Masculino",
                },
                new Genero
                {
                    Title = "Femenino",
                }
            };
            return Generos;
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

        public Command VoltarContaCommand
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

                var userService = new UserService();

                if(MainViewModel.GetInstance().newUser.IsNewRegister)
                if (await userService.RegisterPaciente(FullName, Password, UserName, Genero, Telefone, 
                    Email, Address, DataNascimento))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Usuário registado com sucesso!",
                        "Accept");


                    var users = new UsuariosViewModel();
                    await users.LoadUsers();
                    MainViewModel.GetInstance().usuarios = users;
                    StopLoading();
                    Application.Current.MainPage = new LoginPage();
                    return;
                }

                if (await userService.RegisterUser(UserName, Password, Perfil, Hospital, Especialidade))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Usuário registado com sucesso!",
                        "Accept");


                    var users = new UsuariosViewModel();
                    await users.LoadUsers();
                    MainViewModel.GetInstance().usuarios = users;
                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Este usuário já foi registado!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao registar o usuário",
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

                var userService = new UserService();
                if (await userService.UpdateUser(LastName, UserName, Password, Perfil))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Usuário atualizado com sucesso!",
                        "Accept");


                    var users = new UsuariosViewModel();
                    await users.LoadUsers();
                    MainViewModel.GetInstance().usuarios = users;
                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Não foi possível atualizar os dados deste usuário, tente novamente!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao atualizar o usuário",
                        "Accept");

                StopLoading();
                return;
            }
        }

        
            public async Task GoBack()
            {
                 Application.Current.MainPage = new LoginPage();
            }

        public async Task Delete()
        {
            try
            {
                StartLoading();
                if (!await ValidarCampos())
                {
                    StopLoading();
                    return;
                }

                var userService = new UserService();
                if (await userService.DeleteUser(LastName))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Usuário apagado com sucesso!",
                        "Accept");


                    var users = new UsuariosViewModel();
                    await users.LoadUsers();
                    MainViewModel.GetInstance().usuarios = users;
                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Não foi possível apagar os dados deste usuário, tente novamente!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao apagar o usuário",
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

            if (MainViewModel.GetInstance().newUser.IsNewRegister) 
            {

                if (string.IsNullOrEmpty(this.FullName))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo nome completo está vázio",
                        "Digite o seu nome completo",
                        "OK");
                    return false;
                }

                if (string.IsNullOrEmpty(this.Genero))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo gênero completo está vázio",
                        "Digite o seu gênero",
                        "OK");
                    return false;
                }

                if (string.IsNullOrEmpty(this.DataNascimento.ToString()))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo data de nascimento está vázio",
                        "Digite a sua nascimento",
                        "OK");
                    return false;
                }

                if (string.IsNullOrEmpty(this.Email.ToString()))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo email está vázio",
                        "Digite o seu email",
                        "OK");
                    return false;
                }

                if (string.IsNullOrEmpty(this.Telefone.ToString()))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo telefone está vázio",
                        "Digite o seu telefone",
                        "OK");
                    return false;
                }

                if (string.IsNullOrEmpty(this.Address.ToString()))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo endereço está vázio",
                        "Digite o seu endereço",
                        "OK");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(this.UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo Usuário está vázio",
                    "Digite o seu nome do usuário",
                    "OK");

                return false;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo senha está vázio",
                    "Digite a sua senha",
                    "OK");
                this.Password = string.Empty;
                return false;
            }

            if (!MainViewModel.GetInstance().newUser.IsNewRegister)
                if (string.IsNullOrEmpty(this.Perfil))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Perfil não seleccionado",
                    "Selecione o perfil",
                    "OK");
                this.Perfil = string.Empty;
                return false;
            }

            if (!MainViewModel.GetInstance().newUser.IsNewRegister)
                if (IsMedico) 
            {
                if (string.IsNullOrEmpty(this.Hospital))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Hospital não seleccionado",
                        "Selecione o hospital",
                        "OK");
                    this.Perfil = string.Empty;
                    return false;
                }
                if (string.IsNullOrEmpty(this.Especialidade))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Especialidade não seleccionada",
                        "Selecione a especialidade",
                        "OK");
                    this.Perfil = string.Empty;
                    return false;
                }
            }
                

            return true;
        }

    }


}

public class Perfil
{
    public string Title { get; set; }
};

public class Genero
{
    public string Title { get; set; }
};
