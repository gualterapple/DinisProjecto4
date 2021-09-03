using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace DinisProjecto4.Service
{
    public class UserService
    {
        FirebaseClient client;

        public UserService()
        {
            client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
        }

        public async Task<bool> IsUserExists(string name)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>()).Where(u => u.Object.UserName == name).FirstOrDefault();
            return (user != null);
        }

        public async Task<List<User>> GetUsers()
        {
            var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u=> new User
                {
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil
                }).ToList();
            return users;
        }

        public async Task<List<User>> GetPacientes()
        {
            var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil
                }).Where(a => a.UserName == "Paciente").ToList();

            return users;
        }


        public async Task<bool> RegisterUser(string name, string pass, string perfil)
        {
            if (await IsUserExists(name) == false)
            {
                await client.Child("Users").PostAsync(new User()
                {
                    UserName = name,
                    Password = pass,
                    Perfil = perfil
                });
                return true;
            }
            else
                return false;

        }

        public async Task<bool> UpdateUser(string lastName, string name, string pass, string perfil)
        {
            if (await IsUserExists(lastName) == true)
            {
                var toUpdatePerson = (await client
                .Child("Users")
                .OnceAsync<User>()).Where(a => a.Object.UserName == lastName).FirstOrDefault();

                await client
                  .Child("Users")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new User() { UserName = name, Password = pass, Perfil = perfil });
                return true;
            }
            else
                return false;


        }

        public async Task<bool> DeleteUser(string name)
        {
            if (await IsUserExists(name) == true)
            {
                var toDeletePerson = (await client
               .Child("Users")
               .OnceAsync<User>()).Where(a => a.Object.UserName == name).FirstOrDefault();
                await client.Child("Users").Child(toDeletePerson.Key).DeleteAsync();
                return true;
            }
            else
                return false;

        }

        


        public async Task<bool> LoginUser(string name, string pass)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>()).Where(u => u.Object.UserName == name)
                .Where(u => u.Object.Password == pass).FirstOrDefault();

            return (user != null);

        }
    }
}
