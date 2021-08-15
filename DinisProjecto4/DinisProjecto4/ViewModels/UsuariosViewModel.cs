using System;
using DinisProjecto4.Service;

namespace DinisProjecto4.ViewModels
{
    public class UsuariosViewModel
    {
        public UserService userService { get; set; }

        public UsuariosViewModel()
        {
            userService = new UserService();
            var users = this.userService.GetUsers();
        }
    }
}
