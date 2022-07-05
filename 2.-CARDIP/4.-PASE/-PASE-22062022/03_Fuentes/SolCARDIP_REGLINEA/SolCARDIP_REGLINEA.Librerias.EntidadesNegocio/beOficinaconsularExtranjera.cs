using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    [Serializable]
	public class beOficinaconsularExtranjera
	{
		public short OficinaconsularExtranjeraid { get; set; }                            //OFCE_SOFICINACONSULAR_EXTRANJERAID
		public short Categoriaid { get; set; }                                            //OFCE_SCATEGORIAID
		public string Siglas { get; set; }                                                //OFCE_VSIGLAS
		public string Nombre { get; set; }                                                //OFCE_VNOMBRE
		public string Direccion { get; set; }                                             //OFCE_VDIRECCION
		public string Telefono { get; set; }                                              //OFCE_VTELEFONO
		public string Ubigeocodigo { get; set; }                                          //OFCE_CUBIGEOCODIGO
		public bool Jefaturaflag { get; set; }                                            //OFCE_BJEFATURAFLAG
		public string Sitioweb { get; set; }                                              //OFCE_VSITIOWEB
		public string Estado { get; set; }                                                //OFCE_CESTADO
		public short Usuariocreacion { get; set; }                                        //OFCE_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //OFCE_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //OFCE_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //OFCE_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //OFCE_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //OFCE_DFECHAMODIFICACION
		public short Paisid { get; set; }                                                 //OFCE_SPAISID
        // CONSULTA
        public string ConCategoria { get; set; }
	}
}
