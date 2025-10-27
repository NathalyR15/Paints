using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_servicios
{
    public class PagoDto
    {
        public int PagoID { get; set; }    // FK a FormaPago
        public decimal MontoPago { get; set; }
    }
}
