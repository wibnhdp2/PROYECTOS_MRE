using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    public class beRegistroLinea
    {
        public int RegistroLineaId { get; set; }                                          //RELI_IREGISTRO_LINEA_ID
        public string NumeroRegLinea { get; set; }                                        //RELI_VNUMERO_REG_LINEA
        public int CarneIdentidadId { get; set; }                                       //RELI_ICARNE_IDENTIDAD_ID
        public short EstadoId { get; set; }                                               //RELI_SESTADO_ID
        public short SolicitudId { get; set; }                                            //RELI_SSOLICITUD_ID
        public short TipoEmision { get; set; }                                            //RELI_STIPO_EMISION
        public short DpReldepTitdep { get; set; }                                         //RELI_SDP_RELDEP_TITDEP
        public short DpReldepTitular { get; set; }                                        //RELI_SDP_RELDEP_TITULAR
        public string DpPrimerApellido { get; set; }                                      //RELI_VDP_PRIMER_APELLIDO
        public string DpSegundoApellido { get; set; }                                     //RELI_VDP_SEGUNDO_APELLIDO
        public string DpNombres { get; set; }                                             //RELI_VDP_NOMBRES
        public DateTime DpFechaNacimiento { get; set; }                                   //RELI_DDP_FECHA_NACIMIENTO
        public short DpGeneroId { get; set; }                                             //RELI_SDP_GENERO_ID
        public short DpEstadoCivilId { get; set; }                                        //RELI_SDP_ESTADO_CIVIL_ID
        public short DpTipoDocIdentidad { get; set; }                                     //RELI_SDP_TIPO_DOC_IDENTIDAD
        public string DpNumeroDocIdentidad { get; set; }                                  //RELI_VDP_NUMERO_DOC_IDENTIDAD
        public short DpPaisNacionalidad { get; set; }                                     //RELI_SDP_PAIS_NACIONALIDAD
        public string DpDomicilioPeru { get; set; }                                       //RELI_VDP_DOMICILIO_PERU
        public string DpUbigeo { get; set; }                                              //RELI_CDP_UBIGEO
        public string DpCorreoElectronico { get; set; }                                   //RELI_VDP_CORREO_ELECTRONICO
        public string DpNumeroTelefono { get; set; }                                      //RELI_VDP_NUMERO_TELEFONO
        public string DpRutaAdjunto { get; set; }                                         //RELI_VDP_RUTA_ADJUNTO
        public short InTipoInstitucion { get; set; }                                      //RELI_SIN_TIPO_INSTITUCION
        public string InNombreInstitucion { get; set; }                                   //RELI_VIN_NOMBRE_INSTITUCION
        public string InPersonaContacto { get; set; }                                     //RELI_VIN_PERSONA_CONTACTO
        public string InCargoContacto { get; set; }                                       //RELI_VIN_CARGO_CONTACTO
        public string InCorreoElectronico { get; set; }                                   //RELI_VIN_CORREO_ELECTRONICO
        public string InNumeroTelefono { get; set; }                                      //RELI_VIN_NUMERO_TELEFONO
        public short InTipoCargo { get; set; }                                            //RELI_SIN_TIPO_CARGO
        public string InCargoNombre { get; set; }                                         //RELI_VIN_CARGO_NOMBRE
        public string Estado { get; set; }                                                //RELI_CESTADO
        public short UsuarioCreacion { get; set; }                                        //RELI_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //RELI_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //RELI_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //RELI_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //RELI_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //RELI_DFECHA_MODIFICACION
        public string DpRutaFirma { get; set; }                                         //RELI_VDP_RUTA_FIRMA
        public string DpRutaPasaporte { get; set; }                                         //RELI_VDP_RUTA_PASAPORTE
        public string DpRutaDenunciaPolicial{ get; set; }                                         //RELI_VDP_RUTA_DEN_POLICIAL    
        public string DpMotivoDuplicado { get; set; }                                                //RELI_VDP_MOTIVO_DUPLICADO 
        public string DpRutaResumen { get; set; }                                                //RELI_VDP_RUTA_RESUMEN_ANEXOS 

        public short COD_INSTITUCION { get; set; }                                                //RELI_SIN_COD_INSTITUCION 
        public short COD_CATEGORIA_MISION { get; set; }                                                //RELI_SIN_COD_CATEGORIA_MISION 
        public short TIPO_CALIDAD_MIGRATORIA { get; set; }                                                //RELI_SIN_TIPO_CALIDAD_MIGRATORIA 
        public short COD_CARGO { get; set; }                                                //RELI_SIN_COD_CARGO 

        public string numeroCanet { get; set; }                                                // 
        //CONSULTA
        public string ConFechaNacimiento { get; set; }
        public string ConFechaCreacion { get; set; }
        public string ConHoraCreacion { get; set; }
        public string ConNombreCompleto { get; set; }
        public string ConEstadoDesc { get; set; }
        public string ConGenero { get; set; }
        public string ConEstadoCivil { get; set; }
        public string ConTipoDocIdent { get; set; }
        public string ConDepartamento { get; set; }
        public string ConProvincia { get; set; }
        public string ConDistrito { get; set; }
        public string ConTitDep { get; set; }
        public string ConPais { get; set; }
        public string ConTipoEmision { get; set; }
        // TIPO EMISION (OBJECT)
        public string TipoEmisionObject { get; set; }
        public string TipoEmisionSTR { get; set; }
        private string f = "changes";
        public string finalizadoRegLinea
        {
            get
            {
                return f;
            }
            set
            {
                f = value;
            }
        }

        /*=========adicion de campos calidad migratoria===========*/
        public string TipoVisa { get; set; } //RELI_VTIPO_VISA
        public string Visa { get; set; }//RELI_VVISA
        public string TitularidadFamiliar { get; set; }//RELI_VTITULARIDAD_FAMILIAR
        public string TiempoPermanencia { get; set; }//RELI_Stiempo_PERMANENCIA
        public string Observacion { get; set; }//RELI_Stiempo_PERMANENCIA
        /*========================*/
    }
}
