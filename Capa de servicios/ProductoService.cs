using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Capa_de_datos;

namespace Capa_de_servicios.Services
{
    public class ProductoService
    {
        // Devuelve todos los productos (solo lectura, AsNoTracking)
        public List<Producto> GetAll()
        {
            using var db = DbContextFactory.Create();
            return db.Set<Producto>()
                     .AsNoTracking()
                     .OrderBy(p => p.Nombre)
                     .ToList();
        }

        // Buscar por texto en el nombre (case-insensitive)
        public List<Producto> BuscarPorNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new List<Producto>();

            using var db = DbContextFactory.Create();
            // Usamos EF.Functions.Like para mayor compatibilidad entre proveedores y para
            // evitar problemas con la cultura.
            string pattern = $"%{texto.Replace("%", "[%]").Replace("_", "[_]")}%";
            return db.Set<Producto>()
                     .AsNoTracking()
                     .Where(p => EF.Functions.Like(p.Nombre ?? string.Empty, pattern))
                     .OrderBy(p => p.Nombre)
                     .ToList();
        }

        // Obtener productos por categoriaId
        public List<Producto> GetByCategoria(int categoriaId)
        {

            using var db = DbContextFactory.Create();
            return db.Set<Producto>()
                     .AsNoTracking()
                     .Where(p => p.CategoriaID == categoriaId)
                     .OrderBy(p => p.Nombre)
                     .ToList();
        }

        // Obtener por id
        public Producto? GetById(int id)
        {
            using var db = DbContextFactory.Create();
            return db.Set<Producto>().Find(id);
        }

        // Guardar/actualizar (transaccional simple dentro de EF)
        public void Save(Producto p)
        {
            using var db = DbContextFactory.Create();
            if (p.ProductoID == 0) db.Set<Producto>().Add(p);
            else db.Set<Producto>().Update(p);
            db.SaveChanges();
        }

        // Eliminar
        public void Delete(int id)
        {
            using var db = DbContextFactory.Create();
            var p = db.Set<Producto>().Find(id);
            if (p == null) throw new Exception("Producto no encontrado");
            db.Set<Producto>().Remove(p);
            db.SaveChanges();
        }
    }
}
