using DinisProjecto4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DinisProjecto4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDisponibilidadePage : ContentPage
    {
        public NewDisponibilidadePage()
        {
            InitializeComponent();
            BindingContext = MainViewModel.GetInstance().newDisponibilidade;
            if (MainViewModel.GetInstance().Perfil == "Médico")
                medicoPicker.IsVisible = false;
            else
                medicoPicker.IsVisible = true;


            if (!MainViewModel.GetInstance().newDisponibilidade.IsEditing)
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