using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using DinisProjecto4.Service;
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
        private Hospital selectedHospital;
        private bool isMedico;

        public string LastName { get; set; }
        public bool IsEditing { get; set; }

        public bool IsMedico
        {
            get { return this.isMedico; }
            set { SetValue(ref this.isMedico, value); }
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

            Hospitais = hService.LoadHospitais();
            Especialidades = new ObservableCollection<Especialidade>();

            IsEditing = editing;
            LoadPerfis();
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

        public ObservableCollection<Perfil> Perfis
        {
            get { return this.perfis; }
            set { SetValue(ref this.perfis, value); }
        }

        public IEnumerable<Perfil> LoadPerfis()
        {

            Perfis = new ObservableCollection<Perfil>
            {
                new Perfil
                {
                    Title = "Admnistrador",
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

                var userService = new UserService();
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

            if (string.IsNullOrEmpty(this.Perfil))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Perfil não seleccionado",
                    "Selecione o perfil",
                    "OK");
                this.Perfil = string.Empty;
                return false;
            }

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
