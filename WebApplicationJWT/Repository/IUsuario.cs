using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationJWT.Models;

namespace WebApplicationJWT.Repository
{
    public interface IUsuario
    {
        IList<ApplicationUserModel> ObterUsuariosFake();
       
    }
}
