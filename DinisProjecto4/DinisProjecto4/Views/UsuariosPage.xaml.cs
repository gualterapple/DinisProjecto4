using System;
using System.Collections.Generic;
using DinisProjecto4.ViewModels;
using Xamarin.Forms;

namespace DinisProjecto4.Views
{
    public partial class UsuariosPage : ContentPage
    {
        public UsuariosPage()
        {
            InitializeComponent();
            Navigation.PushModalAsync(new LoginPage(), true);
            BindingContext = MainViewModel.GetInstance().usuarios;

        }
    }
}
