using System;
using System.Collections.Generic;
using DinisProjecto4.ViewModels;
using Xamarin.Forms;

namespace DinisProjecto4.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            MainViewModel.GetInstance().Navigation = Navigation;
            BindingContext = MainViewModel.GetInstance().Login;
        }
    }
}
