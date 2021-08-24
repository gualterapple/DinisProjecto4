using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DinisProjecto4.Service;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class NewConsultaViewModel : BaseViewModel
    {
        private string paciente;
        private string medico;
        private string especialidade;
        private string descricao;

        private DateTime hora;

        private Especialidade selectedEspecialidade;


        public string LastDescricao { get; set; }

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

        public DateTime Hora
        {
            get { return this.hora; }
            set
            {
                this.hora = value;
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

        public NewConsultaViewModel(bool editing)
        {
            CriarContaCommand = new Command(async () => await Register());
            AtualizarContaCommand = new Command(async () => await Update());
            DeleteContaCommand = new Command(async () => await Delete());


            IsEditing = editing;

            LoadEspecialidade();
        }


        private ObservableCollection<Especialidade> especialidades;

        public ObservableCollection<Especialidade> Especialidades
        {
            get { return this.especialidades; }
            set { SetValue(ref this.especialidades, value); }
        }

        public IEnumerable<Especialidade> LoadEspecialidade()
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
                if (await consultaService.NovaConsulta(Medico, Paciente, Especialidade))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Consulta registada com sucesso!",
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
                if (await consultaService.updateConsulta(LastDescricao, Descricao, Medico, Paciente, Especialidade, Hora))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Consulta atualizada com sucesso!",
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
                StartLoading();
                if (!await ValidarCampos())
                {
                    StopLoading();
                    return;
                }

                var userService = new UserService();
                if (await userService.DeleteUser(LastDescricao))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Consulta apagada com sucesso!",
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

            if (string.IsNullOrEmpty(this.Paciente))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo Paciente está vázio",
                    "Digite o seu nome do Paciente",
                    "OK");

                return false;
            }

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

            return true;
        }

    }


}

public class Especialidade
{
    public string Title { get; set; }
};
