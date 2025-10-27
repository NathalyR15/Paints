using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    public class Condicion
    {
        public int ID_Condicion { get; set; }
        public int CategoriaID { get; set; }
        public string? CondicionDescripcion { get; set; }

        public Categoria? Categoria { get; set; }
    }
}
