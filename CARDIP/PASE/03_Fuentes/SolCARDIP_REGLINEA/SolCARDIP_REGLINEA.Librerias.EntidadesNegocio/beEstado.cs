using System;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    [Serializable]
	public class beEstado
	{
		public short Estadoid { get; set; }                                               //ESTA_SESTADOID
		public string DescripcionCorta { get; set; }                                      //ESTA_VDESCRIPCIONCORTA
		public string DescripcionLarga { get; set; }                                      //ESTA_VDESCRIPCIONLARGA
		public string Grupo { get; set; }                                                 //ESTA_VGRUPO
		public string Estado { get; set; }                                                //ESTA_CESTADO
		public short Usuariocreacion { get; set; }                                        //ESTA_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //ESTA_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //ESTA_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //ESTA_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //ESTA_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //ESTA_DFECHAMODIFICACION
	}
}
