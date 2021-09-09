﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using DinisProjecto4.Service;
using Firebase.Database;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class NewConsultaViewModel : BaseViewModel
    {
        FirebaseClient client;

        private string paciente;
        private string medico;
        private string especialidade;
        private string descricao;
        private string desponibilidade;

        private DateTime data;
        private string horario;

        private Especialidade selectedEspecialidade;
        private Disponibilidade selectedDisponibilidade;
        
        private User selectedPaciente;
        private User selectedMedico;

        public ObservableCollection<User> Pacientes { get; set; }
        public ObservableCollection<User> Medicos { get; set; }

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
            CriarContaCommand = new Command(async () => await Register());
            AtualizarContaCommand = new Command(async () => await Update());
            DeleteContaCommand = new Command(async () => await Delete());
            Disponibilidades = new ObservableCollection<Disponibilidade>();
            IsEditing = editing;
            LoadEspecialidade();
            LoadPacientesAndMedicos();

            if (editing)
            {
                Especialidade = consulta.Especialidade;
                foreach (var item in Especialidades)
                {
                    if(item.Title == this.Especialidade)
                    selectedEspecialidade = item;
                }

                Medico = consulta.Medico;
                foreach (var item in Medicos)
                {
                    if (item.UserName == this.Medico)
                        selectedMedico = item;
                }

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

        private ObservableCollection<Disponibilidade> disponibilidades;

        public ObservableCollection<Disponibilidade> Disponibilidades
        {
            get { return this.disponibilidades; }
            set { SetValue(ref this.disponibilidades, value); }
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

        public async void LoadPacientesAndMedicos()
        {

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


            Pacientes = MainViewModel.GetInstance().consultas.toObservableuser(p_);
            Medicos = MainViewModel.GetInstance().consultas.toObservableuser(m_);

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
                if (await consultaService.NovaConsulta(Paciente, Medico, Especialidade, Disponibilidade, Descricao))
                {

                    var mConsulta = MainViewModel.GetInstance().consultas;
                    mConsulta.Consultas = mConsulta.toObservablee(await consultaService.GetConsultas());

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
                if (await consultaService.updateConsulta(LastDescricao, Descricao, Paciente, Medico, Especialidade, Disponibilidade, LastPaciente, LastMedico, LastHorario))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Informação",
                        "Consulta atualizada com sucesso!",
                        "Accept");


                    var consultas = new ConsultasViewModel();
                    consultas.LoadConsultas();
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

            return true;
        }

    }
}

public class Especialidade
{
    public string Title { get; set; }
};
