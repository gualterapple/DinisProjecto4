using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public INavigation Navigation { get; set; }

        private ObservableCollection<User> users;
        private ObservableCollection<User> usersRecebe;

        public UsuariosViewModel()
        {
            userService = new UserService();
            AddUserCommand = new Command(async () => await AddUser());
            SearchCommand = new Command(Search);
            UsersRecebe = new ObservableCollection<User>();
            Users = new ObservableCollection<User>();

        }

        public async Task<ObservableCollection<User>> LoadUsers()
        {
            Users = toObservablee(await this.userService.GetUsers());
            Console.WriteLine(Users);

            UsersRecebe = Users;
            return Users;
        }

        public ObservableCollection<User> UsersRecebe
        {
            get { return this.usersRecebe; }
            set { SetValue(ref this.usersRecebe, value); }
        }
        public ObservableCollection<User> Users
        {
            get { return this.users; }
            set { SetValue(ref this.users, value); }
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

        public Command SearchCommand
        {
            get;
        }

        private string filter;

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }

        private void Search()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Filter))
                {
                    this.LoadUsersToSearch();
                }
                else
                {
                    Users = new ObservableCollection<User>(
                        this.ToUser().Where(
                            l => l.UserName.ToString().ToLower().Contains(this.Filter.ToLower()) || l.Perfil.ToString().ToLower().Contains(this.Filter.ToLower())));
                }
            }
            catch (Exception)
            {
                return;
            }
            
        }

        public void LoadUsersToSearch()
        {
            Users = UsersRecebe;
        }

        private IEnumerable<User> ToUser()
        {
            return this.usersRecebe.Select(o => new User
            {
                UserName = o.UserName,
                Perfil = o.Perfil,

            });
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
