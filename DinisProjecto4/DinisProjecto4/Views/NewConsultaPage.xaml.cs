using System;
using System.Collections.Generic;
using DinisProjecto4.ViewModels;
using Xamarin.Forms;

namespace DinisProjecto4.Views
{
    public partial class NewConsultaPage : ContentPage
    {
        public NewConsultaPage()
        {
            InitializeComponent();
            BindingContext = MainViewModel.GetInstance().newConsulta;
            if (!MainViewModel.GetInstance().newConsulta.IsEditing)
            {
                btnGuardar.IsVisible = true;
                btnApagar.IsVisible = false;
                btnAtualizar.IsVisible = false;
            }
            else
            {
                btnGuardar.IsVisible = false;
                btnApagar.IsVisible = true;
                btnAtualizar.IsVisible = true;
            }
            if(MainViewModel.GetInstance().Perfil == "Médico")
            {
                hospital.IsVisible = false;
                especialidade.IsVisible = false;
                medico.IsVisible = false;
            }
            else
            {
                hospital.IsVisible = true;
                especialidade.IsVisible = true;
                medico.IsVisible = true;
            }
        }
    }
}
