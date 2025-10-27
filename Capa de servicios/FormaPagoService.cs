using System.Collections.Generic;
using System.Linq;
using Capa_de_datos;

namespace Capa_de_servicios.Services
{
    public class FormaPagoService
    {
        // Devuelve todas las formas de pago desde la BD
        public List<FormaPago> GetAll()
        {
            using var db = DbContextFactory.Create();
            // Asegúrate que el DbSet se llame FormaPago en tu AppDbContext
            return db.FormaPago
                     .OrderBy(f => f.PagoID)
                     .ToList();
        }
    }
}
