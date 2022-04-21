using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
	public class bePersonaidentificacion
	{
		public long Personaidentificacionid { get; set; }                                 //PEID_IPERSONAIDENTIFICACIONID
		public long Personaid { get; set; }                                               //PEID_IPERSONAID
		public short Documentotipoid { get; set; }                                        //PEID_SDOCUMENTOTIPOID
		public string Documentonumero { get; set; }                                       //PEID_VDOCUMENTONUMERO
		public DateTime Fecvcto { get; set; }                                             //PEID_DFECVCTO
		public DateTime Fecexpedicion { get; set; }                                       //PEID_DFECEXPEDICION
		public string Lugarexpedicion { get; set; }                                       //PEID_VLUGAREXPEDICION
		public DateTime Fecrenovacion { get; set; }                                       //PEID_DFECRENOVACION
		public string Lugarrenovacion { get; set; }                                       //PEID_VLUGARRENOVACION
		public bool Activoenrune { get; set; }                                            //PEID_BACTIVOENRUNE
		public string Estado { get; set; }                                                //PEID_CESTADO
		public short Usuariocreacion { get; set; }                                        //PEID_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //PEID_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //PEID_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //PEID_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //PEID_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //PEID_DFECHAMODIFICACION
		public string Tipodocumento { get; set; }                                         //PEID_VTIPODOCUMENTO
	}
}
