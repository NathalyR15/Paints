using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_datos
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public int CategoriaID { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }
    }
}
