using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    public class FormaPago
    {
        [Key]
        public int PagoID { get; set; }
        public string TipoPago { get; set; }
    }
}
