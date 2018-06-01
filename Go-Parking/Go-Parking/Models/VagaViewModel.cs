﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Go_Parking.Models
{
    public class VagasViewModel
    {
        [Display(Name = "Vaga Id")]
        public int Id { get; set; }
        [Display(Name = "Vaga")]
        public string Nome { get; set; }
        public string Porte { get; set; }
        public bool Ocupada { get; set; }
        [Display(Name="Reservas hoje")]
        public int Reservas { get; set; }
    }

    public class VagaReservasViewModel
    {
        public int ReservaId { get; set; }
        [Display(Name = "Cliente")]
        public string UsuarioNome { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        [Display(Name = "Tempo Reservado")]
        public TimeSpan HorasReservadas { get; set; }
        public DateTimeOffset Entrada { get; set; }
        public DateTimeOffset Saida { get; set; }
    }
}