using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class bePersonaresidencia
	{
        public long PersonaResidenciaId { get; set; }
        public long Personaid { get; set; }                                               //PERE_IPERSONAID
		public long Residenciaid { get; set; }                                            //PERE_IRESIDENCIAID
		public string Estado { get; set; }                                                //PERE_CESTADO
		public short Usuariocreacion { get; set; }                                        //PERE_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //PERE_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //PERE_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //PERE_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //PERE_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //PERE_DFECHAMODIFICACION
	}
}
