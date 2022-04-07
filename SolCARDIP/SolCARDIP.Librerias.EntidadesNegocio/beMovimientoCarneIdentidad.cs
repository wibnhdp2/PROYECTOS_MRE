using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beMovimientoCarneIdentidad
	{
		public int MovimientoCarneIdentidadid { get; set; }                               //MOCI_IMOVIMIENTO_CARNE_IDENTIDADID
		public short CarneIdentidadid { get; set; }                                       //MOCI_ICARNE_IDENTIDADID
		public DateTime FechaMovimiento { get; set; }                                     //MOCI_DFECHA_MOVIMIENTO
		public short Estadoid { get; set; }                                               //MOCI_SESTADOID
		public short Oficinaconsularid { get; set; }                                      //MOCI_SOFICINACONSULARID
        public short ObservacionTipo { get; set; }
        public string ObservacionDetalle { get; set; }
        public string Estado { get; set; }                                                //MOCI_CESTADO
		public short Usuariocreacion { get; set; }                                        //MOCI_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //MOCI_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //MOCI_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //MOCI_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //MOCI_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //MOCI_DFECHAMODIFICACION
        //CONSULTA
        public string MovUsuario {get;set;}
        public string MovFecha {get;set;}
        public string MovHora {get;set;}
        public string MovEstado {get;set;}
        public string MovOficina {get;set;}
        public string MovTipoObs {get;set;}
        public string MovObsDesc {get;set;}
	}
}
