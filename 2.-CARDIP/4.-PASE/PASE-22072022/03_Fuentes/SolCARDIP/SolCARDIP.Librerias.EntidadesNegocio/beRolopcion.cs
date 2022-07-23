using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beRolopcion
	{
		public short Rolopcionid { get; set; }                                            //ROOP_SROLOPCIONID
		public short Formularioid { get; set; }                                           //ROOP_SFORMULARIOID
		public string Acciones { get; set; }                                              //ROOP_VACCIONES
		public string Estado { get; set; }                                                //ROOP_CESTADO
		public short Usuariocreacion { get; set; }                                        //ROOP_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //ROOP_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //ROOP_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //ROOP_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //ROOP_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //ROOP_DFECHAMODIFICACION
	}
}
