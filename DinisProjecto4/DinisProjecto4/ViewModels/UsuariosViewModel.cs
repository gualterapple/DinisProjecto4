using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using DinisProjecto4.Service;

namespace DinisProjecto4.ViewModels
{
    public class UsuariosViewModel
    {
        public UserService userService { get; set; }

        public ObservableCollection<User> Users { get; set; }

        public UsuariosViewModel()
        {
            userService = new UserService();
        }

        public async Task<ObservableCollection<User>> LoadUsers()
        {
            Users = toObservablee(await this.userService.GetUsers());
            Console.WriteLine(Users);

            return Users;
        }

        private ObservableCollection<User> toObservablee(List<User> users)
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
