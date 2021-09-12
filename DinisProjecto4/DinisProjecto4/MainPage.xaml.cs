using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinisProjecto4.ViewModels;
using DinisProjecto4.Views;
using Xamarin.Forms;

namespace DinisProjecto4
{
    public partial class MainPage : Shell
    {
        public string Perfil { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Perfil = MainViewModel.GetInstance().Perfil;
            switch (Perfil)
            {
                case "Administrador":
                    Menu_usuario.IsVisible = true;
                    Menu_consultas.IsVisible = true;
                    Menu_dispon.IsVisible = true;
                    break;
                case "Médico":
                    Menu_usuario.IsVisible = false;
                    Menu_consultas.IsVisible = true;
                    Menu_dispon.IsVisible = true;
                    break;
                case "Paciente":
                    Menu_usuario.IsVisible = false;
                    Menu_consultas.IsVisible = true;
                    Menu_dispon.IsVisible = false;
                    break;
                case "Secretária":
                    Menu_usuario.IsVisible = false;
                    Menu_consultas.IsVisible = true;
                    Menu_dispon.IsVisible = false;
                    break;
                default:
                    break;
            }
                
        }

        void sair_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}
