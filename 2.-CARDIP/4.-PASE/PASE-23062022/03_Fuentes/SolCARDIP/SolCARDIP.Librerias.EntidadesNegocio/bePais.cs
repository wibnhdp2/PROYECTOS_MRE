using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class bePais
	{
		public short Paisid { get; set; }                                                 //PAIS_SPAISID
		public string Ubigeo { get; set; }                                                //PAIS_CUBIGEO
		public string Nombre { get; set; }                                                //PAIS_VNOMBRE
		public string Zonahoraria { get; set; }                                           //PAIS_VZONAHORARIA
		public string Capital { get; set; }                                               //PAIS_VCAPITAL
		public string Nacionalidad { get; set; }                                          //PAIS_VNACIONALIDAD
		public string Referenciamapa { get; set; }                                        //PAIS_VREFERENCIAMAPA
		public short Monedaid { get; set; }                                               //PAIS_SMONEDAID
		public string Monedacodigo { get; set; }                                          //PAIS_VMONEDACODIGO
		public string Monedasimbolo { get; set; }                                         //PAIS_VMONEDASIMBOLO
		public string Estado { get; set; }                                                //PAIS_CESTADO
		public short Usuariocreacion { get; set; }                                        //PAIS_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //PAIS_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //PAIS_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //PAIS_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //PAIS_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //PAIS_DFECHAMODIFICACION
		public short Continenteid { get; set; }                                           //PAIS_SCONTINENTEID
		public string LetraIso3166 { get; set; }                                          //PAIS_CLETRA_ISO_3166
		public short NumeroIso3166 { get; set; }                                          //PAIS_SNUMERO_ISO_3166
		public string GentilicioMas { get; set; }                                         //PAIS_VGENTILICIO_MAS
		public string GentilicioFem { get; set; }                                         //PAIS_VGENTILICIO_FEM
	}
}
