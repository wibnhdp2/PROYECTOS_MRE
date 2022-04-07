using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beRegistroPrevio
    {
        public short RegistroPrevioId { get; set; }                                       //REPE_SREGISTRO_PREVIO_ID
        public string PrimerApellido { get; set; }                                        //REPE_VPRIMER_APELLIDO
        public string SegundoApellido { get; set; }                                       //REPE_VSEGUNDO_APELLIDO
        public string Nombres { get; set; }                                               //REPE_VNOMBRES
        public short GeneroId { get; set; }                                               //REPE_SGENERO_ID
        public short OficinaConsularExId { get; set; }                                    //REPE_SOFICINA_CONSULAR_EX_ID
        public short CalidadMigratoria { get; set; }                                      //REPE_SCALIDAD_MIGRATORIA
        public DateTime FechaConsulares { get; set; }                                    //REPE_DFECHA_CONSULARES
        public DateTime FechaPrivilegios { get; set; }                                    //REPE_DFECHA_PRIVILEGIOS
        public bool FlagRegistroCompleto { get; set; }                                    //REPE_BFLAG_REGISTRO_COMPLETO
        public string Estado { get; set; }                                                //REPE_CESTADO
        public short UsuarioCreacion { get; set; }                                        //REPE_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //REPE_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //REPE_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //REPE_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //REPE_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //REPE_DFECHA_MODIFICACION
        public short SolicitanteId { get; set; }
        // PARAMETROS
        public int Periodo { get; set; }
        public string CarneNumero { get; set; }
        public short EstadoId { get; set; }
        public int Ident { get; set; }
        public DateTime FechaRegistroDesde { get; set; }
        public DateTime FechaRegistroHasta { get; set; }
        public short TipoEntrada { get; set; }
        public short CategoriaOfcoExId { get; set; }
        // CONSULTA
        public string ConIdent { get; set; }
        public string ConCarneNumero { get; set; }
        public string ConEstadoDesc { get; set; }
        public string ConNombreCompleto { get; set; }
        public string ConGenero { get; set; }
        public string ConOficinaConsular { get; set; }
        public string ConCalidadMigratoriaDesc { get; set; }
        public string ConFechaRegistro { get; set; }
        public string ConFechaEmision { get; set; }
        public string ConFechaVencimiento { get; set; }
        public short ConCarneIdentidadId { get; set; }
        public string ConTipoEntrada { get; set; }
        public string ConFechaConsulares { get; set; }
        public string ConFechaPrivilegios { get; set; }
        public short ConRegistradorId { get; set; }
        public bool ConFlagEntregado { get; set; }
        public short ConActaConformidad { get; set; }
        public short ConActaRecepcion { get; set; }
        //PAGINACION
        public long CantReg { get; set; }
        public long NumPag { get; set; }
        public long Total { get; set; }
        public long Residuo { get; set; }
        public long TotalRegistros { get; set; }
    }
}
