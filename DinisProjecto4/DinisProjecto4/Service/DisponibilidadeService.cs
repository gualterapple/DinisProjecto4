using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace DinisProjecto4.Service
{
    public class DisponibilidadeService
    {
        FirebaseClient client;

        public DisponibilidadeService()
        {
            client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Disponibilidade>> GetDisponibilidades()
        {
            var disponibilidades = (await client.Child("Disponibilidades")
                .OnceAsync<Disponibilidade>()).Select(c => new Disponibilidade
                {                    
                    Medico = c.Object.Medico,
                    Data = c.Object.Data,
                    Hora = c.Object.Hora,
                    Descricao = c.Object.Descricao
                }).ToList();
            return disponibilidades;
        }

        public async Task<List<Disponibilidade>> GetDisponibilidades(string medico)
        {
            var disponibilidades = (await client.Child("Disponibilidades")
                .OnceAsync<Disponibilidade>()).Select(c => new Disponibilidade
                {
                    Medico = c.Object.Medico,
                    Data = c.Object.Data,
                    Hora = c.Object.Hora,
                    Descricao = c.Object.Descricao
                }).Where(d => d.Medico == medico).ToList();
            return disponibilidades;
        }

        public async Task<List<Disponibilidade>> GetDisponibilidadesByMedico(string medico)
        {
            var disponibilidades = (await client.Child("Disponibilidades")
                .OnceAsync<Disponibilidade>()).Select(c => new Disponibilidade
                {
                    Medico = c.Object.Medico,
                    Data = c.Object.Data,
                    Hora = c.Object.Hora,
                    Descricao = c.Object.Descricao
                }).Where(a=> a.Medico  == medico).ToList();
            return disponibilidades;
        }

        public async Task<bool> NovaDisponibilidade(string medico,string descricao, DateTime data, TimeSpan hora)
        {
            await client.Child("Disponibilidades").PostAsync(new Disponibilidade()
            {
                Medico = medico,
                Descricao = descricao,
                Data = data,
                Hora = hora
            });

            return true;

        }

        public async Task<bool> IsDisponibilidadeExists(string medico, TimeSpan hora)
        {
            var disponibilidade = (await client.Child("Disponibilidades")
                .OnceAsync<Disponibilidade>()).Where(u => u.Object.Medico == medico && u.Object.Hora == hora).FirstOrDefault();
            return (disponibilidade != null);
        }

        public async Task<bool> updateDisponibilidade(string medico
            , DateTime data, TimeSpan hora, string lastmedico, TimeSpan lasthora)
        {
            if (await IsDisponibilidadeExists(lastmedico, lasthora) == true)
            {
                var toUpdatePerson = (await client
                .Child("Disponibilidades")
                .OnceAsync<Disponibilidade>()).Where(a => a.Object.Medico == lastmedico && a.Object.Hora == lasthora).FirstOrDefault();

                await client
                  .Child("Disponibilidades")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new Disponibilidade() { Medico = medico, Data = data, Hora = hora });
                return true;
            }
            else
                return false;


        }

        public async Task<bool> DeleteDisponibilidade(string medico, TimeSpan hora)
        {
            if (await IsDisponibilidadeExists(medico, hora) == true)
            {
                var toDeletePerson = (await client
               .Child("Disponibilidades")
               .OnceAsync<Disponibilidade>()).Where(a => 
               a.Object.Medico == medico && a.Object.Hora == hora).FirstOrDefault();
                await client.Child("Disponibilidades").Child(toDeletePerson.Key).DeleteAsync();
                return true;
            }
            else
                return false;

        }

    }

}
