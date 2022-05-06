using System.Runtime.Serialization;
using System;

namespace SGAC.Integrador.Contratos.Datos
{
    [DataContract]
    public class Ticket
    {
        //[DataMember]
        //public int ITicketId { get; set; }
        //[tick_ITicketId]

        [DataMember]
        public int tick_sTipoServicioId { get; set; }

        [DataMember]
        public int tick_iPersonalId { get; set; }

        [DataMember]
        public int tick_iNumero { get; set; }

        [DataMember]
        public string tick_dFechaHoraGeneracion { get; set; }

        [DataMember]
        public string tick_dAtencionInicio { get; set; }

        [DataMember]
        public string tick_dAtencionFinal { get; set; }

        [DataMember]
        public int tick_sPrioridadId { get; set; }

        [DataMember]
        public int tick_sTipoCliente { get; set; }

        [DataMember]
        public int tick_sTamanoTicket { get; set; }

        [DataMember]
        public int tick_sTipoEstado { get; set; }

        [DataMember]
        public int tick_sTicketeraId { get; set; }

        [DataMember]
        public string tick_vLLamada { get; set; }

        [DataMember]
        public int tick_sUsuarioAtendio { get; set; }

        [DataMember]
        public string tick_cEstado { get; set; }

        [DataMember]
        public int tick_sUsuarioCreacion { get; set; }

        [DataMember]
        public string tick_vIPCreacion { get; set; }

        [DataMember]
        public string tick_dFechaCreacion { get; set; }

        [DataMember]
        public int tick_sUsuarioModificacion { get; set; }

        [DataMember]
        public string tick_vIPModificacion { get; set; }

        [DataMember]
        public string tick_dFechaModificacion { get; set; }

        [DataMember]
        public int tick_sVentanillaId { get; set; }
    }
}