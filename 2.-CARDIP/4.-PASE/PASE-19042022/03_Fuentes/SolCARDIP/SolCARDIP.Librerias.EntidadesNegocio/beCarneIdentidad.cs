using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beCarneIdentidad
	{
		public int CarneIdentidadid { get; set; }                                       //CAID_ICARNE_IDENTIDADID
        public int Periodo { get; set; }
        public string IdentMesaPartes { get; set; } 
        public int IdentNumero { get; set; }
		public string CarneNumero { get; set; }                                           //CAID_VCARNE_NUMERO
		public long Personaid { get; set; }                                               //CAID_IPERSONAID
		public short CalidadMigratoriaid { get; set; }                                    //CAID_SCALIDAD_MIGRATORIAID
        public short CalidadMigratoriaSecId { get; set; }
		public DateTime FechaEmision { get; set; }                                        //CAID_DFECHA_EMISION
		public DateTime FechaVencimiento { get; set; }                                    //CAID_DFECHA_VENCIMIENTO
		public short OficinaConsularExid { get; set; }                                    //CAID_SOFICINA_CONSULAR_EXID
		public short CategoriaFuncionarioid { get; set; }                                 //CAID_SCATEGORIA_FUNCIONARIOID
		public string RutaArchivoFoto { get; set; }                                       //CAID_VRUTA_ARCHIVO_FOTO
		public short Estadoid { get; set; }                                               //CAID_SESTADOID
		public string Estado { get; set; }                                                //CAID_CESTADO
		public short Usuariocreacion { get; set; }                                        //CAID_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //CAID_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //CAID_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //CAID_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //CAID_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //CAID_DFECHAMODIFICACION
        public bool Renovar { get; set; }
        public string RutaArchivoFirma { get; set; }
        public short RegistroPrevio { get; set; }
        public short UsuarioDeriva { get; set; }
        public short TipoEntrada { get; set; }
        public bool FlagRegistroCompleto { get; set; }
        public bool FlagControlCalidad { get; set; }
        public string ControlCalidad { get; set; }
        public string ControlCalidadDetalle { get; set; }
        public bool FlagEntregado { get; set; }
        public bool FlagSolicitudActiva { get; set; }
        //PARAMETROS
        public bool NuevoCarne { get; set; }
        public string ApeMatPersona { get; set; }
        public string ApePatPersona { get; set; }
        public string NombresPersona { get; set; }
        public short PaisPersona { get; set; }
        public short CatMision { get; set; }
        public long NumPag { get; set; }
        public long CantReg { get; set; }
        // CONSULTA
        public string ConIdent { get; set; }
        public string ConEstado { get; set; }
        public string ConFuncionario { get; set; }
        public string ConPaisNacionalidad { get; set; }
        public string ConCalidadMigratoria { get; set; }
        public string ConCargo { get; set; }
        public string ConOficinaConsularEx { get; set; }
        public string ConFechaEmision { get; set; }
        public string ConFechaVen { get; set; }
        public string ConFechaNac { get; set; }
        public string ConGenero { get; set; }
        public string ConCatMision { get; set; }
        public string ConEstCivil { get; set; }
        public string ConFechaInscripcion { get; set; }
        public short ConRegistroPrevioId { get; set; }
        public string ConUbigeo { get; set; }
        public string ConDireccion { get; set; }
        public string ConTitDep { get; set; }
        public int ConSolicitudId { get; set; }
        public string ConTipoEntrada { get; set; }
        public bool Duplicado { get; set; }

        public string TipodocDesc { get; set; }
        public string DocumentoNumero { get; set; }
        public string Oservacion { get; set; }
        public string Correo { get; set; }
        public string Numero_reg_linea { get; set; }

        public int reg_lineaID { get; set; }

    }
}
