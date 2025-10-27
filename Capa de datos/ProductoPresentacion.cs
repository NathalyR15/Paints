using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    public class ProductoPresentacion
    {
        public int PresentacionID { get; set; }
        public int ProductoID { get; set; }
        public int ID_Condicion { get; set; }
        public string? Estado { get; set; }
        public string? Descripcion { get; set; }

        public Producto? Producto { get; set; }
        public Condicion? Condicion { get; set; }
    }
}
