using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Go_Parking.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string placa { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}