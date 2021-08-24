using System;
namespace DinisProjecto4.Models
{
    public class Consulta
    {
        public string Descricao { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public string Especialidade { get; set; }
        public DateTime Hora { get; set; }

        public Consulta()
        {
        }
    }
}
