using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beActaRecepcionDetalle
    {
        public short ActaRecepcionDetalleId { get; set; }                                 //ACRD_SACTA_RECEPCION_DETALLE_ID
        public short ActaRecepcionCabId { get; set; }                                     //ACRD_SACTA_RECEPCION_CAB_ID
        public int CarneIdentidadId { get; set; }                                            //ACRD_SREG_PREVIO_ID
        public string Estado { get; set; }                                                //ACRD_CESTADO
        public short UsuarioCreacion { get; set; }                                        //ACRD_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //ACRD_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //ACRD_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //ACRD_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //ACRD_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //ACRD_DFECHA_MODIFICACION
        //CONSULTA
        public string ConTitular { get; set; }
        public string ConNumeroIdent { get; set; }
    }
}
