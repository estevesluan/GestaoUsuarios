using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUsuarios.WebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }
    }
}
