using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationJWT.Models;

namespace WebApplicationJWT.Repository
{
    public class Usuario : IUsuario
    {
        IList<ApplicationUserModel> IUsuario.ObterUsuariosFake()
        {

            IList<ApplicationUserModel> lista = new List<ApplicationUserModel>();

            for (int i = 0; i < 20; i++)
            {
                lista.Add(new ApplicationUserModel
                {
                    Email = "email-usuario-" + i + "@tste.com",
                    FullName = "Nome do Usuario " + i + "",
                    Password = "Senha do usuario " + i + "",
                    UserName = " User name Usuario " + i + ""
                });

            }
            return lista;

        }
    }
}
