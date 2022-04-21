using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
	public class bePersona
	{
		public long Personaid { get; set; }                                               //PERS_IPERSONAID
		public short Estadocivilid { get; set; }                                          //PERS_SESTADOCIVILID
		public short Generoid { get; set; }                                               //PERS_SGENEROID
		public string Apellidopaterno { get; set; }                                       //PERS_VAPELLIDOPATERNO
		public string Apellidomaterno { get; set; }                                       //PERS_VAPELLIDOMATERNO
		public string Nombres { get; set; }                                               //PERS_VNOMBRES
		public string Correoelectronico { get; set; }                                     //PERS_VCORREOELECTRONICO
		public DateTime Nacimientofecha { get; set; }                                     //PERS_DNACIMIENTOFECHA
		public short Personatipoid { get; set; }                                          //PERS_SPERSONATIPOID
		public string Observaciones { get; set; }                                         //PERS_VOBSERVACIONES
		public short Nacionalidadid { get; set; }                                         //PERS_SNACIONALIDADID
		public string Estado { get; set; }                                                //PERS_CESTADO
		public short Usuariocreacion { get; set; }                                        //PERS_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //PERS_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //PERS_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //PERS_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //PERS_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //PERS_DFECHAMODIFICACION
		public short Paisid { get; set; }                                                 //PERS_SPAISID
        public string Telefono { get; set; }
        public bool MenorEdad { get; set; }

        
        public string EstadoCivil { get; set; }
        public string Nacionalidad { get; set; }
        public string Pasaporte { get; set; }
        public string sexo { get; set; }
        public string TiempoPermanencia { get; set; }
        public string TipoVisa { get; set; }
        public string TitularFamiliar { get; set; }
        public string Visa { get; set; }
	}
}
