using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beContinente
	{
		public short Continenteid { get; set; }                                           //CONT_SCONTINENTEID
		public string Nombre { get; set; }                                                //CONT_VNOMBRE
		public string Estado { get; set; }                                                //CONT_CESTADO
		public short Usuariocreacion { get; set; }                                        //CONT_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //CONT_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //CONT_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //CONT_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //CONT_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //CONT_DFECHAMODIFICACION
	}
}
