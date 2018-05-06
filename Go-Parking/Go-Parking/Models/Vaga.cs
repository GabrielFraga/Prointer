using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Go_Parking.Models
{
    public class Vaga
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public string  Modelo { get; set; }
        public string placa { get; set; }
        public int VeiculoId { get; set; }
        public virtual Veiculo veiculo {get; set;}
        [DataType(DataType.DateTime)]
        public DateTimeOffset? HoraEntrada { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset? HoraSaida { get; set; }

    }
}