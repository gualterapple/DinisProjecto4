using System;
using System.Threading.Tasks;
using DinisProjecto4.Service;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
            CriarContaCommand = new Command(async () => await Register());

            StopLoading();
        }
        #region Properties
        public bool Logar_admin { get; set; }
        public bool Logar_medico { get; set; }
        public bool Logar_paciente { get; set; }

        private string email;

        public string Email
        {
            get { return this.email; }
            set
            {
                this.email = value;
                OnPropertyChanged();
            }
        }

        private string password;

        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
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


        #endregion
        public Command LoginCommand
        {
            get;
        }

        public Command CriarContaCommand
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
                if (await userService.RegisterUser(Email, Password))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Usuário registado com sucesso!",
                        "Accept");

                    StopLoading();
                    return;
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

        public async Task Login()
        {
            try
            {
                if (!await ValidarCampos())
                    return;

                StartLoading();
                var userService = new UserService();
                if (await userService.LoginUser(Email, Password))

                {
                    await Application.Current.MainPage.DisplayAlert(
                            "Info",
                            "Login Feito com sucesso!",
                            "Accept");
                    StopLoading();

                    //Application.Current.MainPage = new MasterPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                            "Erro",
                            "Usuário ou senha errado, volte a tentar!",
                            "Accept");

                    StopLoading();
                    return;
                }
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "E-mail ou senha Inválido(s), volte a tentar!",
                        "Accept");
                StopLoading();
                return;
            }

        }

        private async Task<bool> ValidarCampos()
        {

            if (string.IsNullOrEmpty(this.Email))
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

            return true;
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
    }
}
