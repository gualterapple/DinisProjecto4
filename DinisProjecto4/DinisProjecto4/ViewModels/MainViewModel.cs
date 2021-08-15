using System;
namespace DinisProjecto4.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel instance;

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
    }
}
