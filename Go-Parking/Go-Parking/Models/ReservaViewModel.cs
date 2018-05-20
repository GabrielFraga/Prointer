using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go_Parking.Models
{
    public class FazerReserva
    {
        public List<Reserva> Reservas{ get; set; }
        public List<Vaga> Vagas { get; set; }
        public IEnumerable <SelectListItem> Veiculos { get; set; }
        public DateTimeOffset Entrada { get; set; }
        public DateTimeOffset Saida { get; set; }
    }

    public class Reserva
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name = "Reserva ID")]
        public int Id { get; set; }
        public  virtual Vaga Vagas { get; set; }
        public  virtual Veiculo Veiculos { get; set; }
        public DateTimeOffset Entrada { get; set; }
        public DateTimeOffset Saida { get; set; }
    }
}