using System;
using System.Collections.Generic;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beActaConformidadDetalle
    {
        public short ActaConformidadDetalleId { get; set; }                               //ACDE_SACTA_CONFORMIDAD_DETALLE_ID
        public short ActaConformidadCabId { get; set; }                                   //ACDE_SACTA_CONFORMIDAD_CAB_ID
        public int CarneIdentidadId { get; set; }                                       //ACDE_SCARNE_IDENTIDAD_ID
        public string Estado { get; set; }                                                //ACDE_CESTADO
        public short UsuarioCreacion { get; set; }                                        //ACDE_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //ACDE_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //ACDE_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //ACDE_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //ACDE_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //ACDE_DFECHA_MODIFICACION
        //CONSULTA
        public string ConTitular { get; set; }
        public string ConNumeroCarne { get; set; }
        public short ConOficinaId { get; set; }
    }
}
