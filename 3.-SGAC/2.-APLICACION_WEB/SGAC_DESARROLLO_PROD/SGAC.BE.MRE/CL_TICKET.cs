using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_TICKET
    {
        public long tick_iTicketId { get; set; }
        public Nullable<short> tick_sTipoServicioId { get; set; }
        public Nullable<long> tick_iPersonalId { get; set; }
        public Nullable<int> tick_iNumero { get; set; }
        public Nullable<System.DateTime> tick_dFechaHoraGeneracion { get; set; }
        public Nullable<System.DateTime> tick_dAtencionInicio { get; set; }
        public Nullable<System.DateTime> tick_dAtencionFinal { get; set; }
        public Nullable<short> tick_sPrioridadId { get; set; }
        public Nullable<short> tick_sTipoCliente { get; set; }
        public Nullable<short> tick_sTamanoTicket { get; set; }
        public Nullable<short> tick_sTipoEstado { get; set; }
        public Nullable<short> tick_sTicketeraId { get; set; }
        public string tick_vLLamada { get; set; }
        public Nullable<short> tick_sUsuarioAtendio { get; set; }
        public string tick_cEstado { get; set; }
        public short tick_sUsuarioCreacion { get; set; }
        public string tick_vIPCreacion { get; set; }
        public System.DateTime tick_dFechaCreacion { get; set; }
        public Nullable<short> tick_sUsuarioModificacion { get; set; }
        public string tick_vIPModificacion { get; set; }
        public Nullable<System.DateTime> tick_dFechaModificacion { get; set; }
        public Nullable<short> tick_sVentanillaId { get; set; }
    }
}
