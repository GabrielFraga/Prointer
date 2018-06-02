using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Go_Parking.Models
{
    public class IndexViewModel
    {
        public string Nome { get; set; }
        [Display(Name="E-mail")]
        public string Email { get; set; }
        public string Perfil { get; set; }
        
    }   

    public class AtualizarSenhaViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve possuir pelo menos {2} characteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("NewPassword", ErrorMessage = "As senha não conferem.")]
        public string ConfirmPassword { get; set; }
    }

  
}