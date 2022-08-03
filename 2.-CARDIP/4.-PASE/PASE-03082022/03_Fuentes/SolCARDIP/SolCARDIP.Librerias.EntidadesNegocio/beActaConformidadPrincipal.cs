using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beActaConformidadPrincipal
    {
        public beSolicitante Solicitante { get; set; }
        public beActaConformidadCabecera ActaCabecera { get; set; }
        public List<beActaConformidadDetalle> ActaDetalle { get; set; }
        public List<beOficinaconsularExtranjera> Instituciones { get; set; }
        public List<beMovimientoCarneIdentidad> ListaMovimientos { get; set; }
    }
}
