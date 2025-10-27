using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace Capa_de_datos
{
    public class Factura
    {
        public int FacturaID { get; set; }
        public int ClienteID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }

        public ClienteDAL? Cliente { get; set; }
        public UsuarioDAL? Usuario { get; set; }
        public List<DetalleFactura> DetalleFactura { get; set; }
    }
}
