using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
	public class beRolconfiguracion
	{
		public short Rolconfiguracionid { get; set; }                                     //ROCO_SROLCONFIGURACIONID
		public short Aplicacionid { get; set; }                                           //ROCO_SAPLICACIONID
		public short Roltipoid { get; set; }                                              //ROCO_SROLTIPOID
		public string Rolopcion { get; set; }                                             //ROCO_VROLOPCION
		public string Nombre { get; set; }                                                //ROCO_VNOMBRE
		public string Horario { get; set; }                                               //ROCO_CHORARIO
		public string Estado { get; set; }                                                //ROCO_CESTADO
		public short Usuariocreacion { get; set; }                                        //ROCO_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //ROCO_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //ROCO_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //ROCO_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //ROCO_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //ROCO_DFECHAMODIFICACION
	}
}
