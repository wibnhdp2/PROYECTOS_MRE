using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    public class RE_ExpedienteJudicial
    {

        public int sOficinaConsularDestinoId { get; set; }
        public int sTipoParticipanteId { get; set; }
        public string vNumeroExpediente { get; set; }
        public int sEstadoExpedienteId { get; set; }
        public int sEstadoActaId { get; set; }
        public string vNumeroHojaRemision { get; set; }
        public int sTipoPersonaId { get; set; }
        public string vdemandado { get; set; }
        public Nullable<DateTime> dFechaInicio { get; set; }
        public Nullable<DateTime> dFechaFin { get; set; }        
               
        //para el paginado
        public int iPaginaActual { get; set; }
        public int iPaginaCantidad { get; set; }
        public int iTotalRegistros { get; set; }
        public int iTotalPaginas { get; set; }

    }
}
