using DinisProjecto4.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DinisProjecto4.Views
{
    public partial class DisponibilidadesPage : ContentPage
    {
        public DisponibilidadesPage()
        {
            InitializeComponent();
            BindingContext = MainViewModel.GetInstance().disponibilidades;

        }
    }
}
