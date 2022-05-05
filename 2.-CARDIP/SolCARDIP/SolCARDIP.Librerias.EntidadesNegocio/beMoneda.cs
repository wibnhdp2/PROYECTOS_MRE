using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beMoneda
	{
		public short Monedaid { get; set; }                                               //MONE_SMONEDAID
		public string Nombre { get; set; }                                                //MONE_VNOMBRE
		public string Descripcion { get; set; }                                           //MONE_VDESCRIPCION
		public string Simbolo { get; set; } //MONE_VSIMBOLO
		public string Codigo { get; set; }                                                //MONE_VCODIGO
		public string Estado { get; set; }                                                //MONE_CESTADO
		public short Usuariocreacion { get; set; }                                        //MONE_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //MONE_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //MONE_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //MONE_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //MONE_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //MONE_DFECHAMODIFICACION
	}
}
