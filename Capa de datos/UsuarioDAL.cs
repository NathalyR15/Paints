using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    public class UsuarioDAL
    {
        public int UsuarioID { get; set; }
        public string? Nombre { get; set; }
        public string? Contrasena { get; set; }
        public string? Rol { get; set; }
        public DateTime FechaCreacion{ get; set; }

        public ICollection<LoginLogDAL> LoginLogs { get; set; } = new List<LoginLogDAL>();

    }
}
