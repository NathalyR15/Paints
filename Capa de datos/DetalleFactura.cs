using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    public class DetalleFactura
    {
        [Key]
        public int DetalleID { get; set; }
        public int FacturaID { get; set; }
        public int ProductoID { get; set; }
        public decimal Cantidad { get; set; }     // <-- decimal para cantidades
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }

        public Factura? Factura { get; set; }
        public Producto? Producto { get; set; }
    }
}
