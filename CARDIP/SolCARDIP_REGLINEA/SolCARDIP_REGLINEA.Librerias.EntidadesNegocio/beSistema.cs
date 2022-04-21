using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    public class beSistema
    {
        public short Sistemaid { get; set; }                                              //SIST_SSISTEMAID
        public string Nombre { get; set; }                                                //SIST_VNOMBRE
        public string Descripcion { get; set; }                                           //SIST_VDESCRIPCION
        public string Abreviatura { get; set; }                                           //SIST_VABREVIATURA
        public string Urldesarrollo { get; set; }                                         //SIST_VURLDESARROLLO
        public string Urlpruebas { get; set; }                                            //SIST_VURLPRUEBAS
        public string Urlproduccion { get; set; }                                         //SIST_VURLPRODUCCION
        public string Guid { get; set; }                                                  //SIST_VGUID
        //public [type] Imagen { get; set; } //SIST_GIMAGEN
        public string Extension { get; set; }                                             //SIST_VEXTENSION
        public short Orden { get; set; }                                                  //SIST_SORDEN
        public string Estado { get; set; }                                                //SIST_CESTADO
        public short Usuariocreacion { get; set; }                                        //SIST_SUSUARIOCREACION
        public string Ipcreacion { get; set; }                                            //SIST_VIPCREACION
        public DateTime Fechacreacion { get; set; }                                       //SIST_DFECHACREACION
        public short Usuariomodificacion { get; set; }                                    //SIST_SUSUARIOMODIFICACION
        public string Ipmodificacion { get; set; }                                        //SIST_VIPMODIFICACION
        public DateTime Fechamodificacion { get; set; }                                   //SIST_DFECHAMODIFICACION
    }
}
