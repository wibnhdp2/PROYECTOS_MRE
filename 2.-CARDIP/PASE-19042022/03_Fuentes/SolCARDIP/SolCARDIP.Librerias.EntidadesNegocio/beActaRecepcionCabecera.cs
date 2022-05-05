using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beActaRecepcionCabecera
    {
        public short ActaRecepcionId { get; set; }                                        //ACRE_SACTA_RECEPCION_ID
        public short SolicitanteId { get; set; }                                          //ACRE_SSOLICITANTE_ID
        public string Observacion { get; set; }                                           //ACRE_VOBSERVACION
        public string Estado { get; set; }                                                //ACRE_CESTADO
        public short UsuarioCreacion { get; set; }                                        //ACRE_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //ACRE_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //ACRE_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //ACRE_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //ACRE_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //ACRE_DFECHA_MODIFICACION
        // CONSULTA
        public string ConSolicitante { get; set; }
        public string ConObservacion { get; set; }
        public string ConTipoDocIdent { get; set; }
        public string ConNumeroDocIdent { get; set; }
    }
}
