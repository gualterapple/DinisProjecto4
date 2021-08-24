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
                    Especialidade = c.Object.Especialidade
                }).ToList();
            return consultas;
        }

        public async Task<bool> NovaConsulta(string paciente, string medico, string especialidade)
        {
                await client.Child("Consultas").PostAsync(new Consulta()
                {
                    Paciente = paciente,
                    Medico = medico,
                    Especialidade = especialidade,
                });

                return true;

        }

        public async Task<bool> IsConsultaExists(string descricao)
        {
            var consulta = (await client.Child("Consultas")
                .OnceAsync<Consulta>()).Where(u => u.Object.Descricao == descricao).FirstOrDefault();
            return (consulta != null);
        }

        public async Task<bool> updateConsulta(string lastDescription,string descricao, string paciente, string medico, string especialidade
            ,DateTime hora)
        {
            if (await IsConsultaExists(lastDescription) == true)
            {
                var toUpdatePerson = (await client
                .Child("ConsultaS")
                .OnceAsync<Consulta>()).Where(a => a.Object.Descricao == lastDescription).FirstOrDefault();

                await client
                  .Child("ConsultaS")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new Consulta() { Descricao = descricao, Medico = medico, Paciente = paciente, Hora = hora });
                return true;
            }
            else
                return false;


        }
        
    }
}
