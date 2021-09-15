using System;
using System.Threading.Tasks;
using DinisProjecto4.ViewModels;
using DinisProjecto4.Views;
using Xamarin.Forms;

namespace DinisProjecto4.Models
{
    public class User:BaseViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Address { get; set; }
        public string Genero { get; set; }
        public DateTime DataNascimento { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string Perfil { get; set; }
        public string Especialidade { get; set; }
        public string Hospital { get; set; }
        public string Disponibilidade { get; set; }


        public User()
        {
            SelectUserCommand = new Command(async () => await Select());
        }

        public Command SelectUserCommand
        {
            get;
        }

        private async Task Select()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.newUser = new NewUserViewModel(true, this);

            mainViewModel.newUser.LastName = this.UserName;
            mainViewModel.newUser.UserName = this.UserName;
            mainViewModel.newUser.Password = this.Password;
            mainViewModel.newUser.Perfil = this.Perfil;
            await Application.Current.MainPage.Navigation.PushAsync(new NewUserPage());

        }
    }
}
