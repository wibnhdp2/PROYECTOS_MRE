using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beDocumentoIdentidad
	{
		public short Tipodocumentoidentidadid { get; set; }                               //DOID_STIPODOCUMENTOIDENTIDADID
		public string DescripcionCorta { get; set; }                                      //DOID_VDESCRIPCIONCORTA
		public string DescripcionLarga { get; set; }                                      //DOID_VDESCRIPCIONLARGA
		public string Estado { get; set; }                                                //DOID_CESTADO
		public short Usuariocreacion { get; set; }                                        //DOID_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //DOID_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //DOID_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //DOID_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //DOID_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //DOID_DFECHAMODIFICACION
		public short Digitos { get; set; }                                                //DOID_SDIGITOS
		public short Tiponacionalidad { get; set; }                                       //DOID_STIPONACIONALIDAD
		public bool Visible { get; set; }                                                 //DOID_BVISIBLE
		public bool Numero { get; set; }                                                  //DOID_BNUMERO
		public short Digitosminimo { get; set; }                                          //DOID_SDIGITOSMINIMO
        public short PaisId { get; set; }
	}
}
