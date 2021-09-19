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
                    FullName = u.Object.FullName,
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil,
                    Address = u.Object.Address,
                    Email = u.Object.Email,
                    Telefone = u.Object.Telefone,
                    Genero = u.Object.Genero,
                    Especialidade = u.Object.Especialidade,
                    Hospital = u.Object.Hospital,
                    DataNascimento = u.Object.DataNascimento
                }).ToList();
            return users;
        }

        public async Task<List<User>> GetMedicoByEspecialidade(string especialidade, string hospital)
        {
            var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    FullName = u.Object.FullName,
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil,
                    Address = u.Object.Address,
                    Email = u.Object.Email,
                    Telefone = u.Object.Telefone,
                    Genero = u.Object.Genero,
                    Especialidade = u.Object.Especialidade,
                    Hospital = u.Object.Hospital,
                    DataNascimento = u.Object.DataNascimento

                }).Where(a => a.Especialidade == especialidade && a.Hospital == hospital).ToList();

            return users;
        }

        public async Task<List<User>> GetPacientes()
        {
            var users = (await client.Child("Users")
                .OnceAsync<User>()).Select(u => new User
                {
                    FullName = u.Object.FullName,
                    UserName = u.Object.UserName,
                    Password = u.Object.Password,
                    Perfil = u.Object.Perfil,
                    Address = u.Object.Address,
                    Email = u.Object.Email,
                    Telefone = u.Object.Telefone,
                    Genero = u.Object.Genero,
                    DataNascimento = u.Object.DataNascimento

                }).Where(a => a.UserName == "Paciente").ToList();

            return users;
        }


        public async Task<bool> RegisterUser(string name, string pass, string perfil, string hospital, string especialidade)
        {
            if (await IsUserExists(name) == false)
            {
                await client.Child("Users").PostAsync(new User()
                {
                    UserName = name,
                    Password = pass,
                    Perfil = perfil,
                    Especialidade = especialidade,
                    Hospital = hospital
                });
                return true;
            }
            else
                return false;

        }

        public async Task<bool> RegisterPaciente(string fullname, string pass, string user, string genero, string telefone, string email, string endereco, DateTime dataNascimento)
        {
            if (await IsUserExists(user) == false)
            {
                await client.Child("Users").PostAsync(new User()
                {
                    UserName = user,
                    Password = pass,
                    Perfil = "Paciente",
                    FullName = fullname,
                    Genero = genero,
                    Telefone = telefone,
                    Email = email,
                    Address = endereco,
                    DataNascimento = dataNascimento
                });
                return true;
            }
            else
                return false;

        }

        public async Task<bool> UpdateUser(string userName, string pass, string perfil,
            string fullName, DateTime dataNascimento, string genero, string email, string telefone, string hospital, string address)
        {
            if (await IsUserExists(userName) == true)
            {
                var toUpdatePerson = (await client
                .Child("Users")
                .OnceAsync<User>()).Where(a => a.Object.UserName == userName).FirstOrDefault();

                await client
                  .Child("Users")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new User() {
                      UserName = userName,
                      Password = pass,
                      Perfil = perfil,
                      FullName = fullName,
                      DataNascimento = dataNascimento,
                      Genero = genero,
                      Email = email,
                      Telefone = telefone,
                      Hospital = hospital,
                      Address = address});
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
