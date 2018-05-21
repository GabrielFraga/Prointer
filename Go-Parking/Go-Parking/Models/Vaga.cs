using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Go_Parking.Models
{
    public class Vaga
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name = "Vaga Id")]
        public int Id { get; set; }
        [Display(Name = "Vaga")]
        public string Nome { get; set; }
    }
}