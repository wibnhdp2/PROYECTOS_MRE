using System;
using System.Collections.Generic;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beRegistroPrevioListas
    {
        public beRegistroPrevio RegistroPrevio { get; set; }
        public List<beRegistroPrevio> listaRegistros { get; set; }
        public beMovimientoCarneIdentidad MovimientoCarne { get; set; }
        public List<beMovimientoCarneIdentidad> ListaMovimientoCarne { get; set; }
        public bePaginacion Paginacion { get; set; }
        public beSolicitante Solicitante { get; set; }
        public beActaRecepcionCabecera RecepcionCabecera { get; set; }
        public beActaRecepcionDetalle RecepcionDetalle { get; set; }
    }
}
