using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_de_datos
{
  
    public class LoginLogDAL
    {
       
        public int LoginID { get; set; }

        public int UsuarioID { get; set; }
        public DateTime FechaLogin { get; set; }
        public DateTime? FechaLogout { get; set; }
        public string Estado { get; set; }
        public string Info { get; set; }

        // navegación
        public UsuarioDAL Usuario { get; set; }
    }
   
}
