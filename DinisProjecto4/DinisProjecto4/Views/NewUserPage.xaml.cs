using System;
using System.Collections.Generic;
using DinisProjecto4.ViewModels;
using Xamarin.Forms;

namespace DinisProjecto4.Views
{
    public partial class NewUserPage : ContentPage
    {
        public NewUserPage()
        {
            InitializeComponent();
            BindingContext = MainViewModel.GetInstance().newUser;
            if (MainViewModel.GetInstance().newUser.IsNewRegister) 
            {
                btnVoltar.IsVisible = true;
                btnApagar.IsVisible = false;
                btnAtualizar.IsVisible = false;
                perfil.IsVisible = false;
            }
            else {

                btnVoltar.IsVisible = false;
                btnApagar.IsVisible = true;
                btnAtualizar.IsVisible = true;
                perfil.IsVisible = true;

            }

            if (MainViewModel.GetInstance().newUser.IsMedico)
            {
                hospital.IsVisible = true;
                especialidade.IsVisible = true;
            }
            else
            {
                hospital.IsVisible = false;
                especialidade.IsVisible = false;
            }
            if (!MainViewModel.GetInstance().newUser.IsEditing)
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

        }
    }
}
