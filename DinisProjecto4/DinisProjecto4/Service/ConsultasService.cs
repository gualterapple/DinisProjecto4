using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinisProjecto4.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace DinisProjecto4.Service
{
    public class ConsultasService
    {
        FirebaseClient client;

        public ConsultasService()
        {
            client = new FirebaseClient("https://consultas-793b1-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Consulta>> GetConsultas()
        {
            var consultas = (await client.Child("Consultas")
                .OnceAsync<Consulta>()).Select(c => new Consulta
                {
                    Paciente = c.Object.Paciente,
                    Medico = c.Object.Medico,
                    Especialidade = c.Object.Especialidade,
                    Data = c.Object.Data,
                    Hora = c.Object.Hora
                }).ToList();
            return consultas;
        }

        public async Task<bool> NovaConsulta(string paciente, string medico, string especialidade, DateTime data, TimeSpan hora)
        {
                await client.Child("Consultas").PostAsync(new Consulta()
                {
                    Paciente = paciente,
                    Medico = medico,
                    Especialidade = especialidade,
                    Data = data,
                    Hora = hora
                });

                return true;

        }

        public async Task<bool> IsConsultaExists(string paciente, string medico, TimeSpan hora)
        {
            var consulta = (await client.Child("Consultas")
                .OnceAsync<Consulta>()).Where(u => u.Object.Paciente == paciente && u.Object.Medico == medico && u.Object.Hora == hora).FirstOrDefault();
            return (consulta != null);
        }

        public async Task<bool> updateConsulta(string lastDescription,string descricao, string paciente, string medico, string especialidade
            ,DateTime data, TimeSpan hora, string lastpaciente, string lastmedico, TimeSpan lasthora)
        {
            if (await IsConsultaExists(lastpaciente, lastmedico, lasthora) == true)
            {
                var toUpdatePerson = (await client
                .Child("Consultas")
                .OnceAsync<Consulta>()).Where(a => a.Object.Paciente == lastpaciente && a.Object.Medico == lastmedico && a.Object.Hora == lasthora).FirstOrDefault();

                await client
                  .Child("Consultas")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new Consulta() { Descricao = descricao, Medico = medico, Paciente = paciente, Data = data, Hora = hora });
                return true;
            }
            else
                return false;


        }

        public async Task<bool> DeleteConsulta(string paciente, string medico, TimeSpan hora)
        {
            if (await IsConsultaExists(paciente, medico, hora) == true)
            {
                var toDeletePerson = (await client
               .Child("Consultas")
               .OnceAsync<Consulta>()).Where(a => a.Object.Paciente == paciente &&
               a.Object.Medico ==  medico && a.Object.Hora == hora).FirstOrDefault();
                await client.Child("Consultas").Child(toDeletePerson.Key).DeleteAsync();
                return true;
            }
            else
                return false;

        }

    }

}
