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
                    Horario = c.Object.Horario,
                    Descricao = c.Object.Descricao,
                    Hospital = c.Object.Hospital
                }).ToList();
            return consultas;
        }

        public async Task<List<Consulta>> GetConsultasByPaciente(string paciente)
        {
            var consultas = (await client.Child("Consultas")
                .OnceAsync<Consulta>()).Select(c => new Consulta
                {
                    Paciente = c.Object.Paciente,
                    Medico = c.Object.Medico,
                    Especialidade = c.Object.Especialidade,
                    Horario = c.Object.Horario,
                    Descricao = c.Object.Descricao,
                    Hospital = c.Object.Hospital
                }).Where(c => c.Paciente == paciente).ToList();
            return consultas;
        }

        public async Task<bool> NovaConsulta(string paciente, string medico, string especialidade, string horario, string descricao, string hospital)
        {
                await client.Child("Consultas").PostAsync(new Consulta()
                {
                    Paciente = paciente,
                    Medico = medico,
                    Especialidade = especialidade,
                    Horario = horario,
                    Descricao = descricao,
                    Hospital = hospital
                });

                return true;

        }

        public async Task<bool> IsConsultaExists(string paciente, string medico, string horario)
        {
            var consulta = (await client.Child("Consultas")
                .OnceAsync<Consulta>()).Where(u => u.Object.Paciente == paciente && u.Object.Medico == medico && u.Object.Horario == horario).FirstOrDefault();
            return (consulta != null);
        }

        public async Task<bool> updateConsulta(string lastDescription,string descricao, string paciente, string medico, string especialidade
            , string horario, string hospital, string lastpaciente, string lastmedico, string lasthorario)
        {
            if (await IsConsultaExists(lastpaciente, lastmedico, lasthorario) == true)
            {
                var toUpdatePerson = (await client
                .Child("Consultas")
                .OnceAsync<Consulta>()).Where(a => a.Object.Paciente == lastpaciente && a.Object.Medico == lastmedico && a.Object.Horario == lasthorario).FirstOrDefault();

                await client
                  .Child("Consultas")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new Consulta() { Descricao = descricao, Medico = medico, Paciente = paciente, Horario = horario, Especialidade = especialidade, Hospital = hospital });
                return true;
            }
            else
                return false;


        }

        public async Task<bool> DeleteConsulta(string paciente, string medico, string horario)
        {
            if (await IsConsultaExists(paciente, medico, horario) == true)
            {
                var toDeletePerson = (await client
               .Child("Consultas")
               .OnceAsync<Consulta>()).Where(a => a.Object.Paciente == paciente &&
               a.Object.Medico ==  medico && a.Object.Horario == horario).FirstOrDefault();
                await client.Child("Consultas").Child(toDeletePerson.Key).DeleteAsync();
                return true;
            }
            else
                return false;

        }

    }

}
