using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beActaRecepcionPrincipal
    {
        public beSolicitante Solicitante { get; set; }
        public beActaRecepcionCabecera ActaCabecera { get; set; }
        public List<beActaRecepcionDetalle> ActaDetalle { get; set; }
        public List<beOficinaconsularExtranjera> Instituciones { get; set; }
        public List<beMovimientoCarneIdentidad> ListaMovimientos { get; set; }
    }
}
