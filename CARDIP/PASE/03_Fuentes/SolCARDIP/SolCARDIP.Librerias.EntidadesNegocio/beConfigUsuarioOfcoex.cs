using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beConfigUsuarioOfcoex
	{
		public short ConfigUsuarioOfcoexid { get; set; }                                  //CUOE_SCONFIG_USUARIO_OFCOEXID
		public string OficinasCadena { get; set; }                                        //CUOE_VOFICINAS_CADENA
		public short Usuarioid { get; set; }                                              //CUOE_SUSUARIOID
		public string Estado { get; set; }                                                //CUOE_CESTADO
		public short Usuariocreacion { get; set; }                                        //CUOE_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //CUOE_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //CUOE_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //CUOE_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //CUOE_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //CUOE_DFECHAMODIFICACION
	}
}
