using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beUsuarioRol
	{
		public short Usuariorolid { get; set; }                                           //USRO_SUSUARIOROLID
		public short Usuarioid { get; set; }                                              //USRO_SUSUARIOID
		public short Grupoid { get; set; }                                                //USRO_SGRUPOID
		public short Rolconfiguracionid { get; set; }                                     //USRO_SROLCONFIGURACIONID
		public short Oficinaconsularid { get; set; }                                      //USRO_SOFICINACONSULARID
		public short Acceso { get; set; }                                                 //USRO_SACCESO
		public string Estado { get; set; }                                                //USRO_CESTADO
		public short Usuariocreacion { get; set; }                                        //USRO_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //USRO_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //USRO_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //USRO_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //USRO_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //USRO_DFECHAMODIFICACION

        // CONSULTA AUTENTICAR
        public short idSIstema { get; set; }
        public string usuarioAlias { get; set; }
	}
}
