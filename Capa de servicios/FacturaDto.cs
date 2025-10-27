using Capa_de_datos;

namespace Capa_de_servicios
{
    public class FacturaDto
    {
        public int ClienteID { get; set; }
        public int UsuarioID { get; set; }
        public List<FacturaDetalleDto> Detalles { get; set; } = new();
        public List<PagoDto> Pagos { get; set; } = new();
    }
}
