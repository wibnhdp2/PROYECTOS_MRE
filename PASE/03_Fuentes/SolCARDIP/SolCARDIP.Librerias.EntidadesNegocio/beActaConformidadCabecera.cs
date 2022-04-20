using System;
using System.Collections.Generic;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beActaConformidadCabecera
    {
        public short ActaConformidadCabeceraId { get; set; }                               //ACCA_SACTA_CONFORMIDAD_CABECERA_ID
        public long SolicitanteId { get; set; }                                         //ACCA_SPERSONA_RECOJO_ID
        public string Observacion { get; set; }                                           //ACCA_VOBSERVACION
        public string Estado { get; set; }                                                //ACCA_CESTADO
        public short UsuarioCreacion { get; set; }                                        //ACCA_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //ACCA_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //ACCA_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //ACCA_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //ACCA_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //ACCA_DFECHA_MODIFICACION
        // CONSULTA
        public string ConSolicitante { get; set; }
        public string ConObservacion { get; set; }
        public string ConTipoDocIdent { get; set; }
        public string ConNumeroDocIdent { get; set; }
    }
}
