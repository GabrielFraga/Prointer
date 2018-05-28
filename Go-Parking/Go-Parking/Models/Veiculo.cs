using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Go_Parking.Models
{
    public class Veiculo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Porte { get; set; }

        public string UserId { get; set; }


    }
}