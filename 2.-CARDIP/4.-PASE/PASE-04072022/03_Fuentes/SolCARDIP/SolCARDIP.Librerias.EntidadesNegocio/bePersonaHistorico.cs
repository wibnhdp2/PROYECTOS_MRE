using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class bePersonaHistorico
	{
		public int HistoricoPersonaid { get; set; }                                       //PEHI_IHISTORICO_PERSONAID
		public long Personaid { get; set; }                                               //PEHI_IPERSONAID
		public string ApellidoPaterno { get; set; }                                       //PEHI_VAPELLIDO_PATERNO
		public string ApellidoMaterno { get; set; }                                       //PEHI_VAPELLIDO_MATERNO
		public string Nombres { get; set; }                                               //PEHI_VNOMBRES
		public short EstadoCivilid { get; set; }                                          //PEHI_SESTADO_CIVILID
		public short PaisNacionalidadid { get; set; }                                     //PEHI_SPAIS_NACIONALIDADID
		public DateTime FechaNacimiento { get; set; }                                       //PEHI_DFECHA_NACIMIENTO
		public short Generoid { get; set; }                                               //PEHI_SGENEROID
        public long PersonaIdentificacionId { get; set; }                                   //PEHI_IRESINDENCIAID
		public long PersonaResindenciaId { get; set; }                                      //PEHI_IRESINDENCIAID
		public string Estado { get; set; }                                                //PEHI_CESTADO
		public short Usuariocreacion { get; set; }                                        //PEHI_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //PEHI_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //PEHI_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //PEHI_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //PEHI_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //PEHI_DFECHAMODIFICACION
        public string Telefono { get; set; }
        public bool MenorEdad { get; set; }
	}
}
