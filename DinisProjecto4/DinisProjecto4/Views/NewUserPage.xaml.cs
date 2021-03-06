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
            btnApagar.IsVisible = false;
            if (MainViewModel.GetInstance().newUser.IsNewRegister) 
            {
                btnVoltar.IsVisible = true;
                //btnApagar.IsVisible = false;
                btnAtualizar.IsVisible = false;
                perfil.IsVisible = false;
            }
            else {

                btnVoltar.IsVisible = false;
                //btnApagar.IsVisible = true;
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
                btnAtualizar.IsVisible = false;
            }
            else
            {
                btnGuardar.IsVisible = false;
                //btnApagar.IsVisible = true;
                btnAtualizar.IsVisible = true;
            }

            if (MainViewModel.GetInstance().Perfil == "Administrador") 
            {
                perfil.IsVisible = true;
                btnApagar.IsVisible = true;
            }
                
            else
            {
                perfil.IsVisible = false;
                btnApagar.IsVisible = false;
            }

            if (MainViewModel.GetInstance().newUser.IsNewRegister)
                perfil.IsVisible = false;

        }
    }
}
