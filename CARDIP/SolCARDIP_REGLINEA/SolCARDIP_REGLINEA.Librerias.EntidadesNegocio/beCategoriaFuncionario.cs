using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
	public class beCategoriaFuncionario
	{
		public short Categoriafuncionarioid { get; set; }                                 //CATF_SCATEGORIAFUNCIONARIOID
		public string DescripcionCorta { get; set; }                                      //CATF_VDESCRIPCIONCORTA
		public string DescripcionLarga { get; set; }                                      //CATF_VDESCRIPCIONLARGA
		public string Estado { get; set; }                                                //CATF_CESTADO
		public short Usuariocreacion { get; set; }                                        //CATF_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //CATF_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //CATF_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //CATF_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //CATF_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //CATF_DFECHAMODIFICACION
	}
}
