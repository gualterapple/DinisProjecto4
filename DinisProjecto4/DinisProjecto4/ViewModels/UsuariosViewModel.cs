using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using DinisProjecto4.Service;
using DinisProjecto4.Views;
using Xamarin.Forms;

namespace DinisProjecto4.ViewModels
{
    public class UsuariosViewModel:BaseViewModel
    {
        public UserService userService { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public INavigation Navigation { get; set; }


        public UsuariosViewModel()
        {
            userService = new UserService();
            AddUserCommand = new Command(async () => await AddUser());

        }

        public async Task<ObservableCollection<User>> LoadUsers()
        {
            Users = toObservablee(await this.userService.GetUsers());
            Console.WriteLine(Users);

            return Users;
        }

        public Command AddUserCommand
        {
            get;
        }

        private async Task AddUser()
        {
            MainViewModel.GetInstance().newUser = new NewUserViewModel(false, null);
            Navigation = MainViewModel.GetInstance().Navigation;
            //await Navigation.PushAsync(new NewUserPage(), true);
            await Application.Current.MainPage.Navigation.PushAsync(new NewUserPage());

        }

        public ObservableCollection<User> toObservablee(List<User> users)
        {
            var us = new ObservableCollection<User>();
            foreach (var item in users)
            {
                us.Add(
                    new User {
                        UserName = item.UserName,
                        Password = item.Password,
                        Perfil = item.Perfil
                    });
            }
            return us;
        }
    }
}
