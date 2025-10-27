using System;
using System.Linq;
using Capa_de_datos;
using Capa_de_servicios;
using Microsoft.EntityFrameworkCore;

namespace Capa_de_servicios.Services
{
    public class FacturaService
    {
        public int CreateFactura(FacturaDto dto)
        {
            using var db = DbContextFactory.Create();
            using var tx = db.Database.BeginTransaction();
            try
            {
                var factura = new Factura
                {
                    ClienteID = dto.ClienteID,
                    UsuarioID = dto.UsuarioID,
                    Fecha = DateTime.UtcNow
                };

                db.Factura.Add(factura);
                db.SaveChanges(); // obtiene factura.FacturaID

                decimal total = 0m;

                foreach (var d in dto.Detalles)
                {
                    // d.Cantidad es decimal en el DTO; validamos que sea entero si Stock es int
                    decimal cantidadDecimal = d.Cantidad;
                    if (cantidadDecimal < 0)
                        throw new Exception("Cantidad inválida (negativa).");

                    // Validar que la cantidad sea un número entero (sin fracción)
                    if (cantidadDecimal != Math.Truncate(cantidadDecimal))
                        throw new Exception($"La cantidad del producto {d.ProductoID} debe ser un número entero.");

                    // Convertir a int de forma segura (ya sabemos que cabe en un entero razonable)
                    if (cantidadDecimal > int.MaxValue)
                        throw new Exception($"Cantidad demasiado grande para el producto {d.ProductoID}.");

                    int cantidadInt = (int)cantidadDecimal;

                    // Buscar producto
                    var prod = db.Producto.Find(d.ProductoID);
                    if (prod == null) throw new Exception($"Producto {d.ProductoID} no encontrado.");

                    // Comparar usando int (stock) contra cantidadInt
                    if (prod.Stock < cantidadInt) throw new Exception($"Stock insuficiente para {prod.Nombre} (disponible: {prod.Stock}, solicitado: {cantidadInt}).");

                    // Actualizar stock (prod.Stock es int)
                    prod.Stock -= cantidadInt;

                    // Crear detalle (si tu DetalleFactura.Cantidad es decimal, lo dejamos como decimal)
                    var detalle = new DetalleFactura
                    {
                        FacturaID = factura.FacturaID,
                        ProductoID = d.ProductoID,
                        Cantidad = d.Cantidad,            // mantener la cantidad en el detalle (decimal)
                        PrecioUnitario = d.PrecioUnitario,
                        SubTotal = d.Cantidad * d.PrecioUnitario
                    };

                    db.DetalleFactura.Add(detalle);
                    total += detalle.SubTotal;
                }

                factura.Total = total;
                db.SaveChanges();

                // Registrar pagos (tabla Cantidad)
                foreach (var p in dto.Pagos)
                {
                    var registroPago = new Cantidad
                    {
                        FacturaID = factura.FacturaID,
                        PagoID = p.PagoID,
                        MontoPago = p.MontoPago,
                        FechaCantidad = DateTime.UtcNow
                    };
                    db.Cantidad.Add(registroPago);
                }

                db.SaveChanges();
                tx.Commit();
                return factura.FacturaID;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
        public Factura? GetFacturaConDetalle(int facturaId)
        {
            using var db = DbContextFactory.Create();
            return db.Factura
                     .Include(f => f.DetalleFactura)
                     .ThenInclude(d => d.Producto)
                     .Include(f => f.Cliente)
                     .Include(f => f.Usuario)
                     .FirstOrDefault(f => f.FacturaID == facturaId);
        }
    }
}
