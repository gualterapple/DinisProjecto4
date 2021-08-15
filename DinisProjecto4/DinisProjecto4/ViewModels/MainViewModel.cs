using System;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel instance;
        public INavigation Navigation { get; set; }

        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();

        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        public LoginViewModel Login
        {
            get;
            set;
        }
        public UsuariosViewModel usuarios
        {
            get;
            set;
        }
    }
}
