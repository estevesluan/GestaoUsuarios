using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GestaoUsuarios.WebApp.Models
{
    public class LoginResultViewModel
    {

        public string Token { get; set; }
        public bool Succeeded { get; set; }

        public LoginResultViewModel(string token, HttpStatusCode statusCode)
        {
            Token = token;
            Succeeded = (statusCode == HttpStatusCode.OK);
        }
    }
}
