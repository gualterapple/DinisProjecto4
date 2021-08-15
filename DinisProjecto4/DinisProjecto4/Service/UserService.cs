using System;
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

        public async Task<bool> RegisterUser(string name, string pass)
        {
            if (await IsUserExists(name) == false)
            {
                await client.Child("Users").PostAsync(new User()
                {
                    UserName = name,
                    Password = pass,
                });
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
