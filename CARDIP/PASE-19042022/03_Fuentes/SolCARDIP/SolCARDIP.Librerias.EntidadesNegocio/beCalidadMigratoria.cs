using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beCalidadMigratoria
	{
        public short CalidadMigratoriaid { get; set; }                                    //CAMI_SCALIDAD_MIGRATORIAID
        public string NumeroOrden { get; set; }                                           //CAMI_VNUMERO_ORDEN
        public short FlagTitularDependiente { get; set; }                                 //CAMI_SFLAG_TITULAR_DEPENDIENTE
        public bool FlagNivelCalidad { get; set; }                                        //CAMI_SFLAG_NIVEL_CALIDAD
        public short ReferenciaId { get; set; }                                           //CAMI_SREFERENCIA_ID
        public string Nombre { get; set; }                                                //CAMI_VNOMBRE
        public string Definicion { get; set; }                                            //CAMI_VDEFINICION
        public string Estado { get; set; }                                                //CAMI_CESTADO
        public short Usuariocreacion { get; set; }                                        //CAMI_SUSUARIOCREACION
        public string Ipcreacion { get; set; }                                            //CAMI_VIPCREACION
        public DateTime Fechacreacion { get; set; }                                       //CAMI_DFECHACREACION
        public short Usuariomodificacion { get; set; }                                    //CAMI_SUSUARIOMODIFICACION
        public string Ipmodificacion { get; set; }                                        //CAMI_VIPMODIFICACION
        public DateTime Fechamodificacion { get; set; }                                   //CAMI_DFECHAMODIFICACION
        public short GeneroId { get; set; }
        //CONSULTA
        public string TitularDependiente { get; set; }
	}
}
