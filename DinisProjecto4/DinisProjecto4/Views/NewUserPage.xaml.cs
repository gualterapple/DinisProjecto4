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
                fullName.IsVisible = true;
                labelNascimento.IsVisible = true;
                dataNascimento.IsVisible = true;
                generoPicker.IsVisible = true;
                email.IsVisible = true;
                endereco.IsVisible = true;
                btnVoltar.IsVisible = true;
                telefone.IsVisible = true;

                perfil.IsVisible = false;
                medico.IsVisible = false;
                especialidade.IsVisible = false;
                btnApagar.IsVisible = false;
                btnAtualizar.IsVisible = false;
            }
            else {
                fullName.IsVisible = false;
                labelNascimento.IsVisible = false;
                dataNascimento.IsVisible = false;
                generoPicker.IsVisible = false;
                email.IsVisible = false;
                endereco.IsVisible = false;
                btnVoltar.IsVisible = false;
                telefone.IsVisible = false;

                perfil.IsVisible = true;
                medico.IsVisible = true;
                especialidade.IsVisible = true;
                btnApagar.IsVisible = true;
                btnAtualizar.IsVisible = true;
            }
        }
    }
}
