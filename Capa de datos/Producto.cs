using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    public class Producto
    {
        [Key]
        public int ProductoID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }        // <-- decimal para medidas (galones, etc.)
        public int CategoriaID { get; set; }

        public Categoria? Categoria { get; set; }
    }
}
