using System;
using Capa_de_datos;
using Capa_de_servicios;

namespace Capa_de_servicios.Services
{
    public class PagoService
    {
        public void RegistrarPago(int facturaId, PagoDto pago)
        {
            using var db = DbContextFactory.Create();
            var registro = new Cantidad
            {
                FacturaID = facturaId,
                PagoID = pago.PagoID,
                MontoPago = pago.MontoPago,
                FechaCantidad = DateTime.UtcNow
            };
            db.Cantidad.Add(registro);
            db.SaveChanges();
        }
    }
}
