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
    public partial class UserDetailsPage : ContentPage
    {
        public UserDetailsPage()
        {
            InitializeComponent();
            BindingContext = MainViewModel.GetInstance().currentUser;


            if (MainViewModel.GetInstance().Perfil == "Médico")
            {
                hospital_label.IsVisible = true;
                especialidade_label.IsVisible = true;
                hospital.IsVisible = true;
                especialidade.IsVisible = true;
            }
            else
            {
                hospital_label.IsVisible = false;
                especialidade_label.IsVisible = false;
                hospital.IsVisible = false;
                especialidade.IsVisible = false;
            }
        }
    }
}