using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beUbicaciongeografica
	{
		public string Codigo { get; set; }                                                //UBGE_CCODIGO
		public string Ubi01 { get; set; }                                                 //UBGE_CUBI01
		public string Ubi02 { get; set; }                                                 //UBGE_CUBI02
		public string Ubi03 { get; set; }                                                 //UBGE_CUBI03
		public string Siglapais { get; set; }                                             //UBGE_VSIGLAPAIS
		public string Departamento { get; set; }                                          //UBGE_VDEPARTAMENTO
		public string Provincia { get; set; }                                             //UBGE_VPROVINCIA
		public string Distrito { get; set; }                                              //UBGE_VDISTRITO
		public string Estado { get; set; }                                                //UBGE_CESTADO
		public short Usuariocreacion { get; set; }                                        //UBGE_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //UBGE_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //UBGE_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //UBGE_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //UBGE_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //UBGE_DFECHAMODIFICACION
	}
}
