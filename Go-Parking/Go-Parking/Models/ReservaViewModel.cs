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
    public class ListarObjetos
    {
        public List<Reserva> Reservas{ get; set; }
        public List<Vaga> Vagas { get; set; }        
        [Display(Name ="Veículo")]
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

        public List<string> HorasReserva()
        {
            return new List<string>
            {
                {"9:00"},{"9:30"},{"10:00"},{"10:30"},{"11:00"},
                {"11:30"},{"12:00"},{"12:30"},{"13:00"},{"13:30"},
                {"14:00"},{"14:30"},{"15:00"},{"15:30"},{"16:00"},              
            };
            
        }    
        
    }

    public class Pesquisar
    {
        public DateTimeOffset Entrada { get; set; }
        public DateTimeOffset Saida { get; set; }
        public string VeiculoId { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSaida { get; set; }
    }

    public class VagaReserva
    {
        public List<Relatorio> Reservas { get; set; }
        public  List<VagasViewModel> Vagas { get; set; }
    }


    public class Relatorio
    {        
        [Display(Name = "Vaga")]
        public string  VagaNome { get; set; }
        [Display(Name = "Cliente")]
        public string UsuarioNome { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public double Valor { get; set; }
        public string Cor { get; set; }
        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; }
        [Display(Name = "Tempo Reservado")]
        public DateTimeOffset HorasReservadas { get; set; }
        public DateTimeOffset Entrada { get; set; }
        public DateTimeOffset Saida { get; set; }
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

       
    }
}