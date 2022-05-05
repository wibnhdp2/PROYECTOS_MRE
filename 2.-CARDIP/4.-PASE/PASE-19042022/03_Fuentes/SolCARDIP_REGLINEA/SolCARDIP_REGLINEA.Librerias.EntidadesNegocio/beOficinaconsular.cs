using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
	public class beOficinaconsular
	{
		public short Oficinaconsularid { get; set; }                                      //OFCO_SOFICINACONSULARID
		public string Codigo { get; set; }                                                //OFCO_VCODIGO
		public short Categoriaid { get; set; }                                            //OFCO_SCATEGORIAID
		public string Siglas { get; set; }                                                //OFCO_VSIGLAS
		public string Nombre { get; set; }                                                //OFCO_VNOMBRE
		public string Direccion { get; set; }                                             //OFCO_VDIRECCION
		public string Telefono { get; set; }                                              //OFCO_VTELEFONO
		public string Ubigeocodigo { get; set; }                                          //OFCO_CUBIGEOCODIGO
		public short Monedaid { get; set; }                                               //OFCO_SMONEDAID
		public short Porcentajetipocambio { get; set; }                                   //OFCO_SPORCENTAJETIPOCAMBIO
		public string Horarioatencion { get; set; }                                       //OFCO_CHORARIOATENCION
		public double Diferenciahoraria { get; set; } //OFCO_FDIFERENCIAHORARIA
		public short Zonahoraria { get; set; }                                            //OFCO_SZONAHORARIA
		public short Horarioverano { get; set; }                                          //OFCO_SHORARIOVERANO
		public bool Asnflag { get; set; }                                                 //OFCO_BASNFLAG
		public bool Jefaturaflag { get; set; }                                            //OFCO_BJEFATURAFLAG
		public bool Remesalimaflag { get; set; }                                          //OFCO_BREMESALIMAFLAG
		public string Documentosemite { get; set; }                                       //OFCO_VDOCUMENTOSEMITE
		public short Referenciaid { get; set; }                                           //OFCO_SREFERENCIAID
		public string Privilegiosespeciales { get; set; }                                 //OFCO_VPRIVILEGIOSESPECIALES
		public string Sitioweb { get; set; }                                              //OFCO_VSITIOWEB
		public string Rangoinicialip { get; set; }                                        //OFCO_VRANGOINICIALIP
		public string Rangofinip { get; set; }                                            //OFCO_VRANGOFINIP
		public string Estado { get; set; }                                                //OFCO_CESTADO
		public short Usuariocreacion { get; set; }                                        //OFCO_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //OFCO_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //OFCO_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //OFCO_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //OFCO_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //OFCO_DFECHAMODIFICACION
		public short Paisid { get; set; }                                                 //OFCO_SPAISID
		public string Kbcs { get; set; }                                                  //OFCO_CKBCS
		public string Glosa { get; set; }                                                 //OFCO_VGLOSA
		public string Codigolocal { get; set; }                                           //OFCO_VCODIGOLOCAL
	}
}
