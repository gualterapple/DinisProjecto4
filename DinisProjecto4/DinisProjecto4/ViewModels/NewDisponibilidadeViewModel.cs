using DinisProjecto4.Models;
using DinisProjecto4.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class NewDisponibilidadeViewModel : BaseViewModel
    {

        private string descricao;
        private string medico;
        private DateTime data;
        private TimeSpan hora;

        private User selectedMedico;

        public ObservableCollection<User> Medicos { get; set; }

        public string LastMedico { get; set; }
        public TimeSpan LastHora { get; set; }


        public bool IsEditing { get; set; }

        public string Medico
        {
            get { return this.medico; }
            set
            {
                this.medico = value;
                OnPropertyChanged();
            }
        }
        public string Descricao
        {
            get { return this.descricao; }
            set
            {
                this.descricao = value;
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

        public TimeSpan Hora
        {
            get { return this.hora; }
            set
            {
                this.hora = value;
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

        public NewDisponibilidadeViewModel(bool editing)
        {
            CriarDisponibilidadeCommand = new Command(async () => await Register());
            AtualizarDisponibilidadeCommand = new Command(async () => await Update());
            DeleteDisponibilidadeCommand = new Command(async () => await Delete());

            IsEditing = editing;

            if (editing)
            {

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

        public Command CriarDisponibilidadeCommand
        {
            get;
        }

        public Command AtualizarDisponibilidadeCommand
        {
            get;
        }

        public Command DeleteDisponibilidadeCommand
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

                var consultaService = new DisponibilidadeService();
                Descricao = Data.ToString("dd/MMM/yyyy") + " - "+ Hora.ToString();
                if (await consultaService.NovaDisponibilidade(Medico, Descricao, Data, Hora)) {


                    if (MainViewModel.GetInstance().Perfil == "Médico")
                    {
                        var mDisponibilidade = MainViewModel.GetInstance().disponibilidades;
                        mDisponibilidade.Disponibilidades = mDisponibilidade.toObservablee(await consultaService.GetDisponibilidadesByMedico(
                            MainViewModel.GetInstance().Login.Email));
                    }

                    else
                    {
                        var mDisponibilidade = MainViewModel.GetInstance().disponibilidades;
                        mDisponibilidade.Disponibilidades = mDisponibilidade.toObservablee(await consultaService.GetDisponibilidades());
                    }      

                    await Application.Current.MainPage.DisplayAlert(
                    "Informação",
                    "Disponibilidade registada com sucesso!",
                    "Accept");

                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Já foi adicionada uma disponibilidade com este médico e hora!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao registar a disponibilidade, volte a tentar",
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

                var disponibilidadeService = new DisponibilidadeService();
                if (await disponibilidadeService.updateDisponibilidade(Medico, Data, Hora, LastMedico, LastHora))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Disponibilidade atualizada com sucesso!",
                        "Accept");


                    var disponibilidades = new DisponibilidadesViewModel();
                    disponibilidades.LoadDisponibilidades();
                    MainViewModel.GetInstance().disponibilidades = disponibilidades;
                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Não foi possível atualizar os dados desta disponibilidade, tente novamente!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao atualizar a disponibilidade, volte a tentar",
                        "Accept");

                StopLoading();
                return;
            }
        }

        public async Task Delete()
        {
            try
            {


                var disponibilidadeService = new DisponibilidadeService();
                if (await disponibilidadeService.DeleteDisponibilidade(Medico, Hora))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Disponibilidade apagada com sucesso!",
                        "Accept");

                    var disponibilidades = new DisponibilidadesViewModel();
                    await disponibilidades.LoadDisponibilidades();
                    MainViewModel.GetInstance().disponibilidades = disponibilidades;
                    StopLoading();
                    Application.Current.MainPage = new MainPage();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Não foi possível apagar os dados desta disponibilidade, tente novamente!",
                        "Accept");

                    StopLoading();
                    return;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Erro",
                        "Ocorreu um erro ao apagar a disponibilidade",
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

            if (MainViewModel.GetInstance().Perfil != "Médico") 
            {
                if (string.IsNullOrEmpty(this.Medico))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo Medico está vázio",
                        "Digite o nome do médico",
                        "OK");
                    this.Medico = string.Empty;
                    return false;
                }
            }
                
            else
                Medico = MainViewModel.GetInstance().Login.Email;



            if (string.IsNullOrEmpty(this.Data.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Data",
                    "Defina a data da disponibilidade",
                    "OK");
                return false;
            }

            if (string.IsNullOrEmpty(this.Hora.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Data",
                    "Defina a hora da disponibilidade",
                    "OK");
                return false;
            }

            return true;
        }

    }


}

