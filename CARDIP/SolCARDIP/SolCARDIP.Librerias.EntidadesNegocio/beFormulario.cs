using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beFormulario
	{
		public short Formularioid { get; set; }                                           //FORM_SFORMULARIOID
		public short Aplicacionid { get; set; }                                           //FORM_SAPLICACIONID
		public short Componenteid { get; set; }                                           //FORM_SCOMPONENTEID
		public string Nombre { get; set; }                                                //FORM_VNOMBRE
		public short Referenciaid { get; set; }                                           //FORM_SREFERENCIAID
		public string Ruta { get; set; }                                                  //FORM_VRUTA
		public string Imagen { get; set; }                                                //FORM_VIMAGEN
		public short Orden { get; set; }                                                  //FORM_SORDEN
		public bool Visible { get; set; }                                                 //FORM_BVISIBLE
		public string Estado { get; set; }                                                //FORM_CESTADO
		public short Usuariocreacion { get; set; }                                        //FORM_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //FORM_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //FORM_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //FORM_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //FORM_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //FORM_DFECHAMODIFICACION
	}
}
