using System.Linq;
using Capa_de_datos;
using System.Collections.Generic;

namespace Capa_de_servicios.Services
{
    public class ClienteService
    {
        public List<ClienteDAL> GetAll()
        {
            using var db = DbContextFactory.Create();
            return db.Cliente.OrderBy(c => c.Nombre).ToList();
        }
    }
}
