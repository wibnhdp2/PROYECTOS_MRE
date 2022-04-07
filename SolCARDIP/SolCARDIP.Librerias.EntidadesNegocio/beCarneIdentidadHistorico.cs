using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beCarneIdentidadHistorico
	{
		public int HistoricoCarneIdentidadid { get; set; }                                //HICI_IHISTORICO_CARNE_IDENTIDADID
		public short CarneIdentidadid { get; set; }                                       //HICI_ICARNE_IDENTIDADID
		public short CalidadMigratoriaid { get; set; }                                    //HICI_SCALIDAD_MIGRATORIAID
        public short CalidadMigratoriaSecId { get; set; }                                 
        public DateTime FechaEmision { get; set; }                                        //HICI_DFECHA_EMISION
		public DateTime FechaVencimiento { get; set; }                                    //HICI_DFECHA_VENCIMIENTO
		public string RutaArchivoFoto { get; set; }                                       //HICI_VRUTA_ARCHIVO_FOTO
		public short CategoriaFuncionarioid { get; set; }                                 //HICI_SCATEGORIA_FUNCIONARIOID
		public short OficinaConsularExid { get; set; }                                    //HICI_SOFICINA_CONSULAR_EXID
        public short EstadoId { get; set; }
        public string Estado { get; set; }                                                //HICI_CESTADO
		public short Usuariocreacion { get; set; }                                        //HICI_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //HICI_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //HICI_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //HICI_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //HICI_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //HICI_DFECHAMODIFICACION
        public string RutaArchivoFirma { get; set; }
	}
}
