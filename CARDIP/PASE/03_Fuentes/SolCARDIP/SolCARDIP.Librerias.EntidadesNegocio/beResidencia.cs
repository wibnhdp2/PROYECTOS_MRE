using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beResidencia
	{
		public long Residenciaid { get; set; }                                            //RESI_IRESIDENCIAID
		public short Residenciatipoid { get; set; }                                       //RESI_SRESIDENCIATIPOID
		public string Residenciadireccion { get; set; }                                   //RESI_VRESIDENCIADIRECCION
		public string Codigopostal { get; set; }                                          //RESI_VCODIGOPOSTAL
		public string Residenciatelefono { get; set; }                                    //RESI_VRESIDENCIATELEFONO
		public string Residenciaubigeo { get; set; }                                      //RESI_CRESIDENCIAUBIGEO
		public int Centropobladoid { get; set; }                                          //RESI_ICENTROPOBLADOID
		public string Estado { get; set; }                                                //RESI_CESTADO
		public short Usuariocreacion { get; set; }                                        //RESI_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //RESI_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //RESI_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //RESI_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //RESI_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //RESI_DFECHAMODIFICACION
	}
}
