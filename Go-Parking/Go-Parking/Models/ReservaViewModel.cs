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
        public string Pagamentos { get; set; }

        public List<string> FormasPagamentos()
        {
            return new List<string>{
                {"Dinheiro"},
                {"Crédito"},
                {"Débito"}
            };
        }
    }

    public class Reserva
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name = "Reserva ID")]
        public int Id { get; set; }

        public DateTimeOffset Entrada { get; set; }
        public DateTimeOffset Saida { get; set; }
        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; }
        public double Valor { get; set; }

        public virtual string UserId { get; set; }
        public virtual int VagaId { get; set; }
        public virtual int VeiculoId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Vaga Vagas { get; set; }
        public virtual Veiculo Veiculos { get; set; }
    }
}