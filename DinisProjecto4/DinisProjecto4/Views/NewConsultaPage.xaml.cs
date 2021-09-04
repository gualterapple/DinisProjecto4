﻿using System;
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
        }
    }
}