using System;
namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    [Serializable]
	public class beParametro
	{
		public short Parametroid { get; set; }                                            //PARA_SPARAMETROID
		public string Grupo { get; set; }                                                 //PARA_VGRUPO
		public string Descripcion { get; set; }                                           //PARA_VDESCRIPCION
		public string Valor { get; set; }                                                 //PARA_VVALOR
		public string Referencia { get; set; }                                            //PARA_VREFERENCIA
		public byte Orden { get; set; }                                                   //PARA_TORDEN
		public bool Visible { get; set; }                                                 //PARA_BVISIBLE
		public DateTime Vigenciainicio { get; set; }                                      //PARA_DVIGENCIAINICIO
		public DateTime Vigenciafin { get; set; }                                         //PARA_DVIGENCIAFIN
		public bool Precarga { get; set; }                                                //PARA_BPRECARGA
		public string Estado { get; set; }                                                //PARA_CESTADO
		public short Usuariocreacion { get; set; }                                        //PARA_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //PARA_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //PARA_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //PARA_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //PARA_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //PARA_DFECHAMODIFICACION
	}
}
