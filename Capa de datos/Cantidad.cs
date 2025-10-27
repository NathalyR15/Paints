using System;
using Azure;

namespace Capa_de_datos
{
    public class Cantidad
    {
        
        public int CantidadID { get; set; }
        public int FacturaID { get; set; }
        public int PagoID { get; set; }
        public decimal MontoPago { get; set; }
        public DateTime FechaCantidad { get; set; } = DateTime.UtcNow;

        // navegación (opcional)
        public Factura ? Factura { get; set; }
        public PagoDAL Pago { get; set; }
    }
}
