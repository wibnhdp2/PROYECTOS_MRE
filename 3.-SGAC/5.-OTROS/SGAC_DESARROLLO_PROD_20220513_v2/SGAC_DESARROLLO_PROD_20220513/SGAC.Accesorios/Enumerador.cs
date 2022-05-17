using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.Accesorios
{
    public class Enumerador
    {
        #region Configuración
        public enum enmConfiguracionCorreo
        {
            PUERTO = 402,
            SERVIDOR = 404,
            CORREO_DE = 405,
            CONTRASENIA = 406,
            CORREO_PARA = 401,
            FECHA_INICIO_ENVIO = 403
        }

        public enum enmTarifarioEstado
        {
            ACTIVO = 33,
            INACTIVO = 34,
            ELIMINADO = 35
        }

        public enum enmModoVista : int
        {
            HTML = 1,
            ITEXT_SHARP = 2
        }

        public enum enmConfiguracionAlmacen : int
        {
            ALMACEN = 10968
        } 
        #endregion

        #region Accesorios
        public enum enmEstadoVisa
        {
            REGISTRADO = 49,
            SOLICITADO = 50,
            APROBADO = 51,
            RECHAZADO = 82,
            OBSERVADO = 83,
            CORREGIDO = 84,
            FINALIZADO = 85,
            ENTREGADO = 115
        }
        public enum enmEstadoSalvodonducto
        {
            REGISTRADO = 46,
            SOLICITADO = 47,
            APROBADO = 48,
            RECHAZADO = 78,
            OBSERVADO = 79,
            CORREGIDO = 80,
            FINALIZADO = 81,
            ANULADO = 107,
            IMPRESO = 112,
            ENTREGADO = 113
        }
        public enum enmEstadoTraminte
        {
            CANCELADO = 53,
            RECHAZADO = 54
        }
        public enum enmEstado
        {
            ACTIVO = 'A',
            DESACTIVO = 'E'
        }

        public enum enmIdioma : int
        {
            ESPANIOL = 1,
            INGLES = 2
        }

        public enum enmActoNotarialIdioma : int
        {
            ESPANIOL = 8201,
            INGLES = 8202
        }

        public enum enmResultadoQuery : int
        {
            OK = 1101,
            ERR = 1102,
            OTRO = 0,
            OKRESPUESTA = -99
        }

        public enum enmResultadoOperacion : int
        {
            OK = 1,
            ERROR = 0,
        }    
            
        public enum enmBoton : int
        {
            VER = 1,
            EDITAR = 2,
            ELIMINAR = 3,
            NINGUNO = 0
        }

        public enum enmResultadoAuditoria : int
        {
            OK = 1101,
            ERR = 1102
        }

        public enum enmTipoIncidencia : int
        {
            ERROR_APLICATION = 1111,
            ERROR_DATA_BASE = 1112          
        }

        // sado en Auditoria (para los formularios visibles)
        public enum enmVisibilidad : int
        {
            INVISIBLE = 0,
            VISIBLE = 1,
            NINGUNO = 2
        }

        public enum enmMoneda : int
        {
            NUEVO_SOL = 1,
            DOLAR_AMERICANO = 2,
            BOLIVAR = 3,
            EURO = 4,
            PESO_ARGENTINO = 5,
            PESO_CHILENO = 6,
            REAL = 7,
            YEN = 8,
            DOLAR_AMERICANO2 = 42
        }
        #endregion

        #region General
        public enum enmBusquedaDirecciona : int
        {
            NINGUNO = 0,
            RUNE = 1,
            TRAMITE = 2
        }

        // AÚN NO SE USA - DROPDOWNLIST - DESCRIPCION
        public enum enmDescripcionItem : int
        {
            NINGUNO = 0,
            SELECCIONAR = 1,
            TODOS = 2
        }

        public enum enmMes : int
        {
            ENERO = 21,
            FEBRERO = 22,
            MARZO = 23,
            ABRIL = 24,
            MAYO = 25,
            JUNIO = 26,
            JULIO = 27,
            AGOSTO = 28,
            SETIEMBRE = 29,
            OCTUBRE = 30,
            NOVIEMBRE = 31,
            DICIEMBRE = 32
        }


        public enum enmTipoUbigeo : int
        {
            DEPARTAMENTO_CONT = 1,
            PROVINCIA_PAIS = 2,
            DISTRITO_CIUD = 3
        }

        public enum enmContinente : int
        {
            AFRICA=1,
            AMERICA=2,
            ASIA=3,
            EUROPA=4,
            OCEANIA=5
        }

        public enum enmContinenteUbigeo : int
        {
            AFRICA = 91,
            AMERICA = 92,
            ASIA = 93,
            EUROPA = 94,
            OCEANIA = 95
        }
        #endregion

        #region Mensajes
        public enum enmTipoMensaje : int
        {
            NONE = 0,
            INFORMATION = 1,
            QUESTION = 2,
            WARNING = 3,
            ERROR = 4

        }
        public enum enmTipoRespuesta : int
        {
            NONE = 0,
            OK = 1,
            YES_NO = 2,
            APPLY_CANCEL = 3
        }
        public enum enmTipoNotificacion : int
        {
            INFO = 1,
            WARNING = 2,
            ERROR = 3,
            SUCCESS = 4
        }

        #endregion

        #region Grupos

        public enum enmEsquema
        {
            CONFIGURACION,
            SEGURIDAD,
            SISTEMA,
            PERSONA,
            TARIFARIO,
            ACREDITACION,
            ACTUACION,
            REGISTRO_CIVIL,
            REGISTRO_NOTARIAL,
            REGISTRO_MILITAR,
            CONTABILIDAD,
            ALMACEN,
            COLAS
        }
               

        public enum enmGrupo : int
        {
            NINGUNO = -1,

            CONFIG_PROCESOS_SISTEMA = 0,

            CONFIG_MES = 20,

            CONFIG_EMISION_DOC_MRE = 50,
            CONFIG_TIPO_LOCACION = 100,
            CONFIG_ROL_LOCACION = 150,
            CONFIG_CATEGORIA_OFICINA_CONSULAR = 200,
            CONFIG_ATRIBUTOS_OFICINA_CONSULAR = 250,
            CONFIG_TABLAS = 300,
            CONFIG_SERVIDOR_CORREO = 400,
            CONFIG_TIPO_ROL = 450,
            CONFIG_IDIOMA = 500,
            CONFIG_RUTA_COMPARTIDA = 600,
            CONFIG_DIFERENCIA_HORARIA = 700,

            SEGURIDAD_TIPO_OPERADOR = 1000,
            SEGURIDAD_TIPO_OPERACION = 1050,
            SEGURIDAD_RESULTADO_OPERACION = 1100,
            SEGURIDAD_RESULTADO_INCIDENCIA = 1110,
            SEGURIDAD_SERVICIOS = 1150,
            SEGURIDAD_GRUPOS = 1200,
            SEGURIDAD_ENTIDAD = 1250,
            SEGURIDAD_TIPO_AUTENTICACION = 1300,

            SISTEMA_ACCESOS = 1350,

            PERSONA_GENERO = 2000,
            PERSONA_NACIONALIDAD = 2050,
            PERSONA_TIPO = 2100,
            PERSONA_GRADO_INSTRUCCION = 2150,
            PERSONA_TIPO_VINCULO = 2200,
            PERSONA_TIPO_RESIDENCIA = 2250,
            PERSONA_GRUPO_SANGUINEO = 2300,
            PERSONA_COLOR_OJOS = 2350,
            PERSONA_COLOR_TEZ = 2400,
            PERSONA_COLOR_CABELLO = 2430,
            PERSONA_ORIGEN_DATOS = 2450,
            PERSONA_RELACION_CONTACTO = 2500,
            PERSONA_TIPO_IMAGEN = 2550,

            PERSONA_TIPO_INCAPACIDAD = 2600,

            EMPRESA_TIPO = 2800,
            EMPRESA_TIPO_DOCUMENTO = 2850,

            TARIFARIO_TIPO_CALCULO = 3000,
            TARIFARIO_TIPO_FONDO = 3050,
            TARIFARIO_TOPE_UNIDAD = 3100,
            TARIFARIO_ESTADO = 3200,

            ACREDITACION_TIPO_COBRO = 3500,

            //ACTUACION_UNIDAD_MONETARIA = 4000,
            ACTUACION_TIPO_REGISTRO = 4050,
            ACTUACION_TIPO_ANOTACION = 4100,
            ACTUACION_TIPO_VISA = 4150,
            ACTUACION_TIPO_ADJUNTO = 4200,
            ACTUACION_REQUISITO_CONDICION = 4250,
            //ACTUACION_MOTIVO_ANOTACION = 4250,

            REG_CIVIL_TIPO_RECONOCIMIENTO = 4500,
            REG_CIVIL_NACIMIENTO_LUGAR = 4550,
            REG_CIVIL_NACIMIENTO_DECLARANTE = 4600,
            REG_CIVIL_NACIMIENTO_MODIFICACION = 4650,
            REG_CIVIL_MATRIMONIO_REGISTRO = 4700,
            REG_CIVIL_TRAMITE_DNI_REGISTRO = 4750,

            //ACTO_CIVIL_PARTICIPANTE_TIPO_DATO = 8250,
            ACTO_CIVIL_PARTICIPANTE_TIPO_DATO = 4801,

            REG_CIVIL_PARTICIPANTE_NACIMIENTO = 4810,
            REG_CIVIL_PARITICPANTE_MATRIMONIO = 4820,
            REG_CIVIL_PARTICIPANTE_DEFUNCION = 4830,

            //----------------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 07/12/2016
            // Objetivo: Grupo de Participantes de la Ficha Registral
            //----------------------------------------------------------
            FICHA_REGISTRAL_PARTICIPANTE_MENOR = 4850,
            FICHA_REGISTRAL_PARTICIPANTE_MAYOR = 4860,
            //----------------------------------------------------------
            REG_MILITAR_FUERZA_ARMADA = 4900,
            REG_MILITAR_ENTREGA_DOC = 4950,
            REGISTRO_MILITAR_TIPO_CALIFICACION = 7800,
            REGISTRO_MILITAR_SERVICIO_RESERVA_MILITAR = 7920,
            REGISTRO_MILITAR_TIPO_PARTICIPANTE = 7960,
            REGISTRO_MILITAR_TIPO_INSCRIPCION = 7970,

            CONTA_TIPO_LIBRO = 5000,
            CONTA_TIPO_ABONO = 5050, // IDM-PENDIENTE
            CONTA_TIPO_TRANSACCION = 5100,
            CONTA_TIPO_CUENTA = 5150,
            CONTA_SITUACION_CUENTA = 5200,
            CONTA_TIPO_REMESA_DETALLE = 5250,
            CONTA_TIPO_INGRESO = 5300,
            CONTA_TIPO_EGRESO = 5350,
            CONTA_PLAZO_ENVIO_REMESA = 5400,

            ALMACEN_TIPO_BOVEDA = 6000,
            ALMACEN_TIPO_MOVIMIENTO = 6050,
            ALMACEN_MOTIVO_MOVIMIENTO = 6100,
            ALMACEN_TIPO_PEDIDO = 6150,
            ALMACEN_MOTIVO_PEDIDO = 6200,
            ALMACEN_TIPO_INSUMO = 6250,
            ALMACEN_TIPO_REPORTE = 6300,

            COLAS_TIPO_REPORTE = 7000,
            COLAS_TIPO_SERVICIO = 7050,
            COLAS_PROCESOS = 7100,
            COLAS_ACCION_VENTANILLA = 7150,
            COLAS_SECCION_CONFIG = 7200,
            COLAS_PANTALLAS_SISTEMA = 7250,
            COLAS_REPORTE_TICKETS = 7300,
            //-------------------------------------------------------
            // Fecha: 27/02/2017
            // Autor: Miguel Márquez Beltrán
            // Objetivo: Mostrar la descripción: COLAS- SEGEMENTACIÓN CLIENTES
            //-------------------------------------------------------
            COLAS_SEGMENTACION_CLIENTE = 7350,
            //COLAS_TIPO_ATENCION = 7350,            
            COLAS_TIPO_CLIENTE = 7400,
            COLAS_IMPRESION_TICKET = 7450,
            COLAS_TAMANIO_TICKET = 7500,
            COLAS_OPCION_SERVICIO = 7600,
            COLAS_SECTOR_CONSULAR = 7550,
            COLAS_REPORTES = 7750,            

            GRUPO = -2,

            ACTO_JUDICIAL_EXPEDIENTE_TIPO_NOTIFICACIÓN = 8500,
            ACTO_JUDICIAL_EXPEDIENTE_ENTIDAD_SOLICITANTE = 8510,
            ACTO_JUDICIAL_NOTIFICACION_TIPO_RECEPCIÓN = 8520,
            ACTO_JUDICIAL_NOTIFICACION_VIA_ENVÍO = 8550,
            ACTO_JUDICIAL_TIPO_PARTICIPANTE = 8540,
            ACTO_JUDICIAL_ACTA_TIPO = 8530,

            REG_NOTARIAL_TIPO_ACTO = 8000,
            REG_NOTARIAL_TIPO_ACTOR = 8020,
            REG_NOTARIAL_TIPO_ACTO_EXTRAPROTOCOLAR = 8030,
            REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR = 8040,
            REG_NOTARIAL_TIPO_ACCION_ACTO_PROTOCOLAR = 8421,
            REG_NOTARIAL_TIPO_INFORMACION_INSERTO = 8100,
            REG_NOTARIAL_SUB_TIPO_INFORMACION_INSERTO = 8150,
            REG_NOTARIAL_IDIOMA = 8200,
            REG_NOTARIAL_TIPO_LIBRO = 8250,

            REGISTRO_NOTARIAL_PROTOCOLAR_TIPO_PARTICIPANTE = 8410,

            AVISOS = 8430,

            REG_MIGRA_VISA_TIPO = 9000,
            REG_MIGRA_VISA_TIPO_RESIDENTE = 9010,
            REG_MIGRA_VISA_TIPO_TEMPORAL = 9050,
            REG_MIGRA_TITULAR_FAMILIA = 9100,
            REG_MIGRA_VISA_TIPO_DOCUMENTO_RREE = 9110,
            REG_MIGRA_VISA_TIPO_DOCUMENTO_DIGEMIN = 9150,
            ACTO_MIGRATORIO_TIPO_NRO_PASAPORTE = 9200,
            REG_MIGRA_VISA_AUTORIZACION = 9221,
            REG_MIGRA_VISA_CARGO_PRENSA = 9237,
            REG_MIGRA_VISA_CARGO_DIPLOMATIVO = 9239,
            REG_MIGRA_VISA_CARGO_FUNCIONARIO = 9241,
            REG_MIGRA_PASAPORTE_TIPO = 8300,
            ACTO_MIGRATORIO_MOTIVOS = 9260, // BAJA
            ACTO_MIGRATORIO_MOTIVOS_ANULAR = 9290, //Anulación
            ACTO_MIGRATORIO_MOTIVO_CANCELADO = 9270,
            ACTO_MIGRATORIO_MOTIVO_RECHAZO = 9280,
            ACTO_MIGRATORIO_DOCUMENTOS = 9300,

            REG_REPORTE_GERENCIAL = 10000,
            REG_REPORTE_MIGRATORIO = 10100,

            TRADUCCION_IDIOMA = 10200
        }

        public enum enmTraduccionTipoIdioma : int
        {
            CASTELLANO = 10549
        }

        public enum enmFichaTipoParticipanteMenor : int
        {
            TITULAR = 4851,
            PADRE = 4852,
            MADRE = 4853,
            DECLARANTE = 4854
        }
        public enum enmFichaTipoParticipanteMayor : int
        {
            TITULAR = 4861,
            PADRE = 4862,
            MADRE = 4863,
            CONYUGE = 4864
        }

        public enum enmMaestro : int
        {
            PROCESO,
            CONTINENTE,
            DOCUMENTO_IDENTIDAD,
            OCUPACION,
            PROFESION,
            BANCO,
            ESTADO,
            CARGO_FUNCIONARIO,
            SERVICIO,
            SECCION,
            BASE_PERCEPCION,
            REQUISITO_ACTUACION,
            PLANTILLA,
            MONEDA,
            CATEGORIA_FUNCIONARIO,
            ESTADO_CIVIL,
            TIPO_ACTA,
            REG_NOTARIAL_ESTADO_ESCRITURA,
            CARGO_FUNCIONARIO_LISTA
        }

       

        public enum enmEstadoGrupo : int
        {
            ACTUACION = 4000,
            REMESA = 5000,
            MOVIMIENTO = 6000,
            PEDIDO = 6010,
            INSUMO = 6020,
            TICKET = 7000,
            ASISTENCIA = 8000,
            PASAPORTE = 42,
            SALVOCONDUCTO = 46,
            VISA = 49,
            VISA_TRAMITE = 52,
            TARIFARIO_ESTADO = 66,
            JUDICIAL_ESTADO_EXPEDIENTE = 62,
            JUDICIAL_ESTADO_ACTA = 69,
            PROTOCOLARES_ESTADO_ESCRITURA = 98
        }
        
        public enum enmServicio : int
        {
            PAHL = 1,
            PAH = 2,
            OTROS = 3
        }

        public enum enmTipoImagen : int
        {
            FOTO_RENIEC = 2551,
            FIRMA_RENIEC = 2552,
            HUELLA_RENIEC = 2553,
            FOTO = 2554
        }
        #endregion

        #region Persona

        public enum enmTipoResidencia
        {
            PROCEDENCIA = 2251,
            RESIDENCIA = 2252
        }
        public enum enmNacionalidad : int
        {
            PERUANA = 2051,
            EXTRANJERA = 2052,
            NINGUNA = 0
        }

        public enum enmTipoPersona : int
        {
            NATURAL = 2101,
            JURIDICA = 2102,
            NINGUNA = 0
        }

        public enum enmOrigenDatos : int
        {
            RENIEC = 1,
            DNI = 2,
            RUNE = 3
        }

        public enum enmTipoDocumento : int
        {
            DNI = 1,
            LIBRETA_MILITAR = 2,
            CARNET_EXTRANJERIA = 3,
            OTROS = 4,
            PASAPORTE = 5,
            CUI = 6,
            LICENCIA = 7,
            DNI_EUA = 8,
            PASAPORTE_E = 9
        }

        public enum enmEstadoCivil : int
        {
            SOLTERO = 1,
            CASADO = 2,
            DIVORCIADO = 3,
            VIUDO = 4
        }

        public enum enmGenero : int
        {
            MASCULINO = 2001,
            FEMENINO = 2002,

        }

        public enum enmVinculo : int
        {
            PADRE = 2202,
            MADRE = 2203,
            CONYUGE = 2204,
            HERMANO_PRIMO = 2205,
            HIJO = 2206,
            APODERADO = 2207,
            ABUELO = 2208,
            SOBRINO = 2209,
            TIO = 2210,
            OTROS = 2211
        }

        public enum enmTipoIncapacidad : int
        {
            ILETRADA= 2601,
            INCAPACIDAD_FISICA = 2602
        }
        #endregion

        #region Empresa

        public enum enmEmpresaTipoDocumento
        {
            RUC = 2851,
            OTROS = 2852
        }

        public enum enmTipoEmpresa
        {
            PUBLICO = 2801,
            PRIVADO = 2802,
            MIXTA   = 2803
        }

        #endregion

        #region Actuación

        public enum enmTramiteTipo : int
        {
            ACTUACION_DETALLE = 0,
            EXPEDIENTE_JUDICIAL = 1,
            ACTO_NOTARIAL = 2
        }

        public enum enmPersonaConsulta
        {
            RUNE = 1,
            ACTUACION = 2
        }

        public enum enmTipoActa : int
        {
            NACIMIENTO = 4501,
            MATRIMONIO = 4502,
            DEFUNCION = 4503
        }
        public enum enmTipoActuacion : int
        {
            SALVOCONDUCTO = 6,
            VISA = 7
        }
        public enum enmSeccion : int
        {
            ACTO_CIVIL = 1,
            ACTO_NOTARIAL = 2,
            ACTO_JUDICIAL = 3,
            ACTO_RELATIVO = 4,
            REGISTRO_NACIONALES = 5,
            PASAPORTE_SALVOCONDUCTO = 6,
            VISA = 7,
            ACT_REG_IDENTIDAD = 8,
            REGISTRO_MILITAR = 9,
            OTRAS_ACTUACIONES = 10
        }

        public enum enmTipoAdjunto : int
        {
            OTRO = 1111,
            DOCUMENTO_DIGITALIZA = 4204,
            PDF = 4201,
            FOTO = 4202,
            HUELLA = 4203,
            FIRMA = 4205,

            FOTO_PERFIL = 4206,
            HUELLA_INDICE_IZQUIERDO = 4207,
            HUELLA_INDICE_DERECHO = 4208
        }

        public enum enmTipoCalculoTarifario : int
        {
            MONTO_FIJO = 3001,
            PORCENTAJE = 3002,
            FORMULA = 3003,
            SIN_CALCULO = 3004
        }

        public enum enmTipoCobroActuacion : int
        {
            PAGADO_EN_LIMA = 3501,
            NO_COBRADO = 3502,
            POR_CORREO = 3503,
            EFECTIVO = 3504,
            TARJETA_DE_CREDITO = 3505,
            MONEY_ORDER = 3506,
            TRANSFERENCIA_BANCARIA = 3507,
            DEPOSITO_CUENTA = 3508,
            GRATIS = 3509
        }

        public enum enmTipoAyudaConnacional : int
        {
            PAHL = 1,
            PAH = 2
        }

        public enum enmEstadoAyudaConnacional : int
        {
            ENTRANTE = 1,
            CONCLUIDO = 2
        }

        public enum enmActuacionEstado : int
        {
            CANCELADO = 1,
            EN_PROCESO_REGISTRO = 2,
            REGISTRADO = 3,
            EN_PRODUCCION = 4,
            SIN_AUTOADHESIVO = 5,
            CON_AUTOADHESIVO = 6,
            POR_ENTREGAR = 7,
            ENTREGADO_AL_SOLI = 8
        }

        public enum enmInsumoEstado : int
        {
            VINCULADO_ACTUACION=19,
            BAJA = 20,
            ANULADO = 106

        }
        public enum enmRegistroReporte : int
        {
            RUNE = 1,
            ANOTACION = 2,
			FILIACION = 3
        }

        public enum enmParticipanteLeyenda : int
        {
            NACIMIENTO = 1,
            MATRIMONIO = 2,
            DEFUNCION = 3,
            MILITAR = 4

        }

        public enum enmActuacionParticipante : int
        {
            RECURRENTE = 4301
        }
        #endregion

        #region Contabilidad

        public enum enmTipoRuta : int
        {
            CONTABILIDAD = 2,
        }

        //-----------------------------------------
        // Fecha: 21/11/2016
        // Autor: Miguel Márquez Beltrán
        // Objetivo: Adicionar las variables 
        //          REPORTE_ACTUACIONES_ANULADAS y
        //          RENDICION_CUENTAS_TARIFAS_RENIEC.
        //-----------------------------------------
        public enum enmLibroContable : int
        {
            SALDOS_CONSULARES = 5001,
            AUTOADHESIVO_CONSULAR = 5002,
            LIBRO_CAJA = 5003,
            DOCUMENTO_UNICO = 5004,
            REGISTRO_GENERAL_ENTRADAS = 5005,
            REPORTE_ACTUACIONES_ANULADAS = 5008,
            RENIEC = 5009
        }
        public enum enmRemesaEstado : int
        {
            PENDIENTE = 22,
            ENVIADA = 23,
            OBSERVADA = 24,
            APROBADA = 25,
            ANULADA = 26,
            CERRADA = 110
        }
        public enum enmTipoTransaccion : int
        {
            BANCARIA = 5101,
            ORDEN_PAGO = 5102,
            CHEQUE = 5103
        }
        public enum enmTipoMovimientoTransaccion : int
        {
            INGRESOS = 5051,
            SALIDAS = 5052
        }
        public enum enmTipoTranIngreso : int
        {
            DEPOSITO_EFECTIVO = 5301,
            CHEQUE = 5302,
            TRANSFERENCIA = 5303,
            TRANSFERENCIA_DEPENDIENTE = 5304,
            REPOSICION_COMISION = 5305,
            OTRO = 5306,
            SALDO_INICIAL=5307
        }
        public enum enmTipoTranEgreso : int
        {
            TRANSFERENCIA_LIMA = 5351,
            TRANSFERENCIA_JEFATURA = 5352,
            COMISION = 5353,
            DEVOLUCION = 5354,
            OTROS = 5355
        }
        public enum enmTipoCuenta : int
        {
            TRANSACCION = 5151,
            COMPARTIDA = 5152
        }
        public enum enmSituacion : int
        {
            OPERATIVA = 5201,
            CANCELADA = 5202,
            ANULADA = 5203
        }
        //------------------------------------------------------
        //Fecha: 12/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Exportar e Imprimir el tipo de reporte:
        //        Rendición de Cuentas por Tarifas RENIEC.
        //Documento: OBSERVACIONES_SGAC_12052021.doc
        //------------------------------------------------------

        public enum enmReporteContable : int
        {
            REMESA,
            ESTADO_BANCARIO,
            DOCUMENTO_UNICO,
            REGISTRO_GENERAL_ENTRADAS,
            AUTOADHESIVO,
            CAJA,
            SALDOS_CONSULARES,
            CONCILIACION,
            AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR,
            ACTUACIONES_USUARIO_OFICINA_CONSULAR,
            REPORTE_ACTUACIONES_ANULADAS,
            RENIEC_FORMATO_ENVIO_TARIFA,
            RENIEC_FORMATO_ENVIO_ESTADO,
            RENIEC_GUIA_DESPAHO,
            RENIEC_RENDICION_CUENTAS,
            RENIEC_RENDICION_CUENTAS_NOHEAD,
            CONST_REPORTE_RENIEC_INCOMPLETOS,
            REGCIVIL_ACTA_NACIMIENTO,
            CERTIFICADOS_CONSULARES_ANTECEDENTES_PENALES,
            REPORTE_TITULARES,
            REPORTE_RENIEC_CONCILIACION,
            CERTIFICADO_ANTECEDENTE_PENAL,
            CERTIFICADOS_CONSULARES_ANTECEDENTES_PENALES_USUARIO

        }

        public enum enmPlazoEnvioRemesa : int
        {
            DEPENDIENTE = 3,
            OFICINA_CONSULAR = 5,
            JEFATURA = 10
        }

        #endregion

        #region Seguridad

        public enum enmEntidad : int
        {
            MRE = 1251,
            MIGRACIONES = 1252
        }

        public enum enmTipoOperacion : int
        {
            ACCESO = 1051,
            REGISTRO_ACTUACION = 1052,
            IMPRESION = 1053,
            CONSULTA = 1054,
            REGISTRO = 1055,
            ACTUALIZACION = 1056,
            ANULACION = 1057
        }

        public enum enmAccion : int
        {
            INSERTAR = 0,
            MODIFICAR = 1,
            ELIMINAR = 2,
            LEERREGISTRO = 3,
            BUSCAR = 4,
            LEER = 5,
            CONSULTAR = 6,
            FILTRAR = 7,
            REASIGNAR = 8,
            OBTENER = 9,
            OBTENER_DEPARTAMENTOS = 10,
            OBTENER_PROVINCIAS = 11,
            OBTENER_DISTRITOS = 11,
            OBTENER_UBIGEO = 12,
            OBTENERREGISTRO = 13,
            BUSCAR_1 = 14,
            OBTENERRPTAVALOR = 15,
            ATENDER = 16,
            ACTIVAR = 17
        }

        public enum enmAplicacion : int
        {
            WEB = 1,
            OTRO = 0,
        }

        public enum enmTipoRol : int
        {
            SUPERADMIN = 451,
            ADMINISTRATIVO = 452,
            OPERATIVO = 453
        }

        //N|E|G|C|B|I|S
        public enum enmPermisoAccion
        {
            NUEVO = 'N', // MANTENIMIENTO
            MODIFICAR = 'M', // MANTENIMIENTO
            ELIMINAR = 'E', // CONSULTA       
            CONFIGURAR = 'R', // MANTENIMIENTO
            GRABAR = 'G', // MANTENIMIENTO
            CANCELAR_M = 'C', // MANTENIMIENTO            
            //EXCEL = 'X', // CONSULTA
            //PDF = 'P', // CONSULTA
            BUSCAR = 'B', // CONSULTA
            IMPRIMIR = 'I', // CONSULTA
            CANCELAR_C = 'A',
            CERRAR = 'S' // CONSULTA -SALIR
            //SALIR = 'S' // EXTRA
        }

        public enum enmFormulario : int
        {
            RUNE = 8,
            ACTUACIONES = 9,
            MOVIMIENTOS = 13,
            PEDIDOS = 12,
            CONTROL_STOCK = 14,
            LISTADO_INSUMOS = 15,
            RENDICION_CUENTA = 19,
            REMESA_CONSULAR = 18,
            ESTADO_BANCARIO = 17,
            LIBRO_CONTABLE = 16,

            TIPO_CAMBIO_BANCARIO = 26,
            TIPO_CAMBIO = 27,

            CUENTA_BANCARIA = 21,
            AUDITORIA = 25,
            USUARIOS = 28,
            ROLES = 29,
            FORMULARIOS = 30,
            PARAMETROS = 41,
            TARIFARIO = 43,
            OFICINA_CONSULAR = 42,

            UBIGEO = 44,

            TIPOS_DE_SERVICIO = 33,
            VENTANILLA = 34,
            PERFILES_DE_ATENCION = 35,
            TELEVISORES = 36,
            TICKETERAS = 37,
            VIDEO = 38,

            TABLAS_MAESTRAS = 39
        }

        public enum enmTabla : int
        {
            AL_INSUMOHISTORICO = 302,
            AL_MOVIMIENTO = 303,
            AL_MOVIMIENTODETALLE = 304,
            AL_PEDIDO = 305,
            AL_PEDIDOHISTORICO = 306,

            CL_PERFILATENCION = 307,
            CL_SERVICIO = 308,
            CL_TELEVISOR = 309,
            CL_TICKET = 310,
            CL_TICKETERA = 311,
            CL_VENTANILLA = 312,
            CL_VENTANILLASERVICIO = 313,
            CL_VIDEO = 314,
            CL_VIDEOTELEVISOR = 315,

            CO_CUENTACORRIENTE = 316,
            CO_REMESA = 317,
            CO_REMESADETALLE = 318,
            CO_TRANSACCION = 319,

            RE_ACTUACION = 320,
            RE_ACTUACIONADJUNTO = 321,
            RE_ACTUACIONDETALLE = 322,
            RE_ANOTACION = 323,
            RE_FUNCIONARIO = 324,
            RE_PAGO = 325,
            RE_PERSONA = 326,
            RE_PERSONA_FILIACION = 327,
            RE_PERSONAFOTO = 328,
            RE_PERSONAIDENTIFICACION = 329,
            RE_PERSONARESIDENCIA = 330,
            RE_REGISTROUNICO = 331,
            RE_RESIDENCIA = 332,

            SE_FORMULARIO = 335,
            SE_GRUPO = 336,
            SE_ROLCONFIGURACION = 337,
            SE_ROLOPCION = 338,
            SE_USUARIO = 339,
            SE_USUARIOROL = 340,

            SI_AUDITORIA = 341,
            SI_BOVEDA = 342,
            SI_CIUDAD = 343,
            SI_FERIADO = 344,
            SI_OFICINACONSULAR = 345,
            SI_PAIS = 346,
            SI_PARAMETRO = 347,
            SI_REGION = 348,
            SI_TARIFARIO = 349,
            SI_TARIFARIOREQUISITO = 350,
            SI_TIPOCAMBIO_BANCARIO = 351,
            SI_TIPOCAMBIO_CONSULAR = 352,
            SI_UBICACIONGEOGRAFICA = 353,
            SE_PERFIL_OPCION = 354,
            SE_SISTEMA = 355,

            MA_BANCO = 356,
            MA_BASE_PERCEPCION = 357,
            MA_CARGO_FUNCIONARIO = 358,
            MA_CATEGORIA_FUNCIONARIO = 359,
            MA_CONTINENTE = 360,
            MA_DOCUMENTO_IDENTIDAD = 361,
            MA_ESTADO = 362,
            MA_ESTADO_CIVIL = 363,
            MA_MONEDA = 364,
            MA_OCUPACION = 365,
            MA_PLANTILLA = 366,
            MA_PROCESO = 367,
            MA_PROFESION = 368,
            MA_REQUISITO_ACTUACION = 369,
            MA_SECCION = 370,
            MA_SERVICIO = 371,

        }

        public enum enmAccesoUsuario : int
        {
            TOTAL = 1351,
            PARCIAL = 1352,
            INDIVIDUAL = 1353
        }

        #endregion

        #region Almacen

        public enum enmBovedaTipo : int
        {
            MISION = 6001,
            JEFATURA = 6002,
            OFICINA_DEPENDIENTE = 6003,
            VENTANILLA = 6004,
            USUARIO = 6005
        }

        public enum enmMovimientoTipo : int
        {
            ENTRADA = 6101,
            SALIDA = 6102,
            DEVOLUCION = 6103,
            RETORNO = 6104,
            BAJA = 6105
        }

        public enum enmMovimientoMotivo : int
        {
            CARGA_INICIAL = 6151,
            POR_PEDIDO = 6152,
            POR_DETERIORADO = 6153,
            POR_ANULACION = 6154,
            POR_ERROR_FABRICA = 6155,
            POR_ACTUACION_CONSULAR = 6156,
            POR_MALA_IMPRESION = 6157,
            POR_PERDIDA_ROBO = 6158,
            POR_EXCESO_EN_STOCK = 6159,
            POR_MALA_ASIGNACIÓN = 6160,
            POR_TRASLADO_INTERNO = 6161,
            DETERIORADO = 6162,
            INUTILIZADO = 6163,
            DEVOLUCION = 6164
        }

        public enum enmMovimientoEstado : int
        {
            REGISTRADO = 9,
            ACEPTADO = 10,
            RECHAZADO = 11,
            ANULADO = 12
        }

        public enum enmAlmacenReporte : int
        {
            KARDEX_DE_INSUMOS = 6301,
            KARDEX_DE_INSUMOS_DETALLADO = 6302,
            INSUMOS_REMITIDOS = 6303
        }

        public enum enmRangoEstado : int
        {
            NUEVO = 1,
            MODIFICADO = 2,
            ANULADO = 3
        }

        public enum enmPedidoEstado : int
        {
            PENDIENTE = 13,
            ATENDIDO = 14,
            RECHAZADO = 15,
            ANULADO = 16
        }

        public enum enmPedidoMotivo : int
        {
            REPOSICION_STOCK = 6251,
            STOCK_SUFICIENTE = 6252,
            SIN_AUTORIZACION = 6253,
            OTRO = 6254
        }

        public enum enmPedidoTipo : int
        {
            PEDIDO_LIMA = 6201,
            PEDIDO_JEFATURA = 6202
        }

        public enum enmInsumoTipo : int
        {
            AUTOADHESIVO = 6051,
            DNI = 6052,
            PASAPORTE = 6053,
            VISA = 6054,
            OTROS = 6055,
            LAMINA = 6057,
            ETIQUETA = 6062,
            SALVOCONDUCTO = 6060,
            LAMINA_REPOSICION = 6061,
            LAMINA_RENOVACION = 6063,
            LAMINA_SALVOCONDUCTO = 6064,
            LAMINA_SALVOCONDUCTO_REPOSICION = 6065
        }

        #endregion

        #region Migratorio
        public enum enmDocumentoMigratorio : int
        {
            PASAPORTE = 9301,
            SALVOCONDUCTO = 9302,
            VISAS = 9303
        }
        public enum enmOficinaComplementaria : int
        {
            CANCILLERIA = 221,
            MIGRACIONES = 222
        }

        public enum enmPasaporteTipo : int
        {
            EXPEDIDO = 9211,
            REVALIDADO = 9212
        }

        public enum enmMigratorioPasaporteEstados : int
        {
            EXPEDIDO = 86,
            ANULADO = 87,
            BAJA = 88,
            RECHAZADO = 89,
            OBSERVADO = 90,
            CORREGIDO = 91,
            FINALIZADO = 92,
            ANULADO_2 = 108,
            IMPRESO = 111,
            ENTREGADO = 114
        }

        public enum enmMigratorioMotivos : int
        {
            OCURRENCIA_MISION = 9261,
            AGOTAMIENTO_PAGINAS = 9271,
            DETERIORO_PERDIDA = 9272,
            REGULARIZACION = 9273,
            RENUNCIA_NACIONALIDAD = 9274,
            ROBO_SALVOCONDUCTO = 9275,
            VERIFICACION_DOCS = 9276,
            DOCUMENTACION_FRAGUADA = 9281,
            ENFERMEDAD_CONTAGIOSA = 9282,
            NO_SOLVENTE = 9283,
            PRESENTA_ANTECEDENTES = 9284,
            OTROS = 9285,

            NINGUNO = 9291
        }

        public enum enmMigratorioFormato : int
        {
            DGC_001_PASAPORTE_EXPEDIDO = 1,
            DGC_002_PASAPORTE_REVALIDADO = 2,

            DGC_003_PASAPORTE_ANULADO = 3,
            DGC_004_PASAPORTE_BAJA = 4,

            DGC_005_VISA = 5,
            DGC_006_SALVOCONDUCTO = 6,

            DGC_001_PASAPORTE_EXPEDIDO_LAMINA = 7,
            DGC_002_PASAPORTE_REVALIDADO_LAMINA = 8,

            DGC_003_PASAPORTE_ANULADO_LAMINA = 9,
            DGC_004_PASAPORTE_BAJA_LAMINA = 10,

            DGC_005_VISA_LAMINA = 11,
            DGC_006_SALVOCONDUCTO_LAMINA = 12
        }


        public enum enmMigratorioVisaTipo : int
        {
            RESIDENTE = 9001,
            TEMPORAL = 9002
        }

        public enum enmMigratorioVisaTipoTemporal : int
        {
            TEMPORAL = 9061,
            TURISTAS = 9068,
            NEGOCIOS = 9062,
            PERIODISTA = 9064,
            DIPLOMATICA = 9055,
            FAMILIAR_OFICIAL = 9057,
            INTERCAMBIO = 9061,
            OFICIAL = 9063
        }

        public enum enmMigratorioVisaTipoResidente : int
        {
            PERIODISTA = 9018
        }

        public enum enmMigratorioPasaporteVigencia : int
        {
            UN_1 = 1,
            DOS_2 = 2,
            CINCO_5 = 5
        }

        public enum enmMigratorioSalvoconductoVigencia : int
        {
            TREINTA = 30
        }

        public enum enmVisaAutorizacion : int
        {
            MISION_CONSULAR = 9221,
            MRE_TRC = 9222,
            MRE_CON = 9223,
            MRE_DGC = 9224,
            MRE_PRI = 9225,
            MRE_OGC = 9226
        }

        #endregion

        #region Participantes
        public enum enmTipoActuacionParticipante : int
        {
            CIVIL = 1,
            MILITAR = 2,
            JUDICIAL = 3,
            NOTARIAL = 4
        }

        public enum enmTipoParticipanteJudicial : int
        {
            DEMANDANTE = 0,
            DEMANDADO = 1
        }



        #endregion

        #region Militar
        public enum enmTipoParticipanteMilitar : int
        {
            TITULAR = 7961,
            PADRE = 7962,
            MADRE = 7963,
            CONYUGE = 7964,
            RECURRENTE = 7965
        }

        public enum enmServicioReserva : int
        {
            ORGANICA = 7921,
            APOYO = 7922,
            DISPONIBLE = 7923
        }
        public enum enmInstitucion : int
        {
            EJERCITO_PERU = 7941,
            MARINA_GUERRA = 7942,
            FUERZA_AEREA = 7943
        }
        public enum enmCalificacion : int
        {
            SELECCIONADO = 7901,
            NO_SELECCIONADO = 7902,
            EXCEPTUADO = 7903
        }
        #endregion

        #region Civil
        public enum enmParticipanteNacimiento : int
        {
            TITULAR = 4811,
            PADRE = 4812,
            MADRE = 4813,
            REGISTRADOR_CIVIL = 4814,
            DECLARANTE_1 = 4815,
            DECLARANTE_2 = 4816,
            RECURRENTE = 4817
        }
        public enum enmParticipanteDefuncion : int
        {
            TITULAR = 4831,
            PADRE = 4832,
            MADRE = 4833,
            REGISTRADOR_CIVIL = 4834,
            DECLARANTE = 4835,
            RECURRENTE = 4836
        }
        public enum enmParticipanteMatrimonio : int
        {
            CELEBRANTE = 4821,
            DON = 4822,
            DONIA = 4823,
            REGISTRADOR_CIVIL = 4824,
            RECURRENTE = 4825
        }

        public enum enmParticipanteTipoDatos : int
        { 
            PADRE = 4801,
            MADRE = 4802
        }

        public enum enmParticipanteTipoResidencia : int
        {
            PROCEDENCIA = 2251,
            RESIDENCIA = 2252
        }
        #endregion

        #region Notarial

        public enum enmProtocolarMostrarAutoadhesivo : int { 
            AUTOADHESIVO    = 8441,
            PARTE           = 8442,
            TESTIMONIO      = 8443,
            PARTE_ADICIONAL = 8444
        }

        public enum enmNotarialAvisos:int
        {
            CONFORMIDAD_DE_TEXTO=8431
        }

        public enum enmProtocolarTipoInformacion : int
        {
            TEXTO_NORMATIVO = 8101,
            TRANSCRIPCION = 8102,
            INSERTO = 8103

        }
        public enum enmProtocolarSubTipoInformacion : int
        {
            ARTICULO_74 = 8151,
            ARTICULO_75 = 8152,
            ARTICULO_156 = 8153,
            ARCHIVO_PDF = 8154,
            ARCHIVO_JPG = 8155

        }

        public enum enmExtraprotocolarTipo : int
        {
            PODER_FUERA_REGISTRO = 8031,
            CERTIFICADO_SUPERVIVENCIA = 8032,
            AUTORIZACION_VIAJE_MENOR = 8033,
            OTRAS_CERTIFICACIONES_NOTARIALES=8034
        }

        public enum enmProtocolarTipo : int
        {
            COMPRA_VENTA = 8041,
            CONTRATOS_DE_LOCACIÓN_DE_SERVICIOS_OTROS = 8042,
            CONVENIOS_ARBITRALES_ACREEDORES_DEUDORES = 8043,
            CONVENIO_ARBITRAL_DESIGNACIÓN_ARBITRO = 8044,
            RENDICIÓN_CUENTAS_TUTELA_CURATELA = 8045,
            SUSTITUCIÓN_REGIMEN_CONYUGAL = 8046,
            TESTAMENTOS_CERRADOS = 8047,
            TESTAMENTO_ESCRITURA_PÚBLICA = 8048,
            ANTICIPO_HERENCIA = 8049,
            PODER_GENERAL_AMPLIO_ABSOLUTO = 8050,
            PODER_ESPECIAL = 8051,
            RENUNCIA_NACIONALIDAD = 8052,
            RECONOCIMIENTO_PATERNIDAD = 8053,
            CUALQUIER_ACTO_JURIDICO_NO_ESPECIFICADO = 8054,
            OTRO = 0,
            DONACION = 8070
        }

        public enum enmProtocolarAccionModificatoria : int
        {
            CREACION = 8421,
            REVOCATORIA = 8422,
            MODIFICACIÓN = 8423,
            AMPLIACIÓN = 8424,
            SUSTITUCIÓN = 8425,
            RECTIFICACIÓN = 8426
        }

        public enum enmFormatoProtocolar : int
        {
            MINUTA = 8401,
            PARTE = 8402,
            TESTIMONIO = 8403,
            ESCRITURA = 8404,
            LISTADO_ESCRITURAS = 8405
        }

        public enum enmNotarialTipoActo : int
        {
            PROTOCOLAR = 8001,
            EXTRAPROTOCOLAR = 8002
        }

        public enum enmNotarialTipoParticipante : int
        {
            OTORGANTE = 8021,
            APODERADO = 8022,
            PADRE = 8023,
            MADRE = 8024,
            MENOR = 8025,
            INTERPRETE = 8026,
            TITULAR = 8027,
            ACOMPANANTE = 8028,
            TESTIGO_A_RUEGO = 8029,
            RECURRENTE = 8130
        }

        public enum enmNotarialProtocolarTipoParticipante : int
        {
            INTERPRETE = 8413,
            TESTIGO_A_RUEGO = 8414,
            OTORGANTE = 8415,
            APODERADO = 8417,
            RECURRENTE = 8418,
            //----------------------------------------------------------
            //Fecha: 11/04/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el código que le corresponde a los 
            //          nuevos participantes del acto protocolar.
            //----------------------------------------------------------
            VENDEDOR = 8600,
            COMPRADOR = 8601,
            ANTICIPANTE = 8602,
            ANTICIPADO = 8603,
            //----------------------------------------------------------
            //Fecha: 24/09/2021
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el código que le corresponde a los 
            //          nuevos participantes del acto protocolar.
            //----------------------------------------------------------
            DONANTE = 8604,
            DONATARIO = 8605,
            //----------------------------------------------------------

            NINGUNO = 0
        }

        public enum enmNotarialProtocolarEstado : int
        {
            REGISTRADO = 98,
            ASOCIADA = 99,
            TRANSCRITA = 100,
            APROBADA = 101,
            PAGADA = 102,
            VINCULADA = 103,
            DIGITALIZADA = 104,
            ANULADA = 105
        }

        public enum enmFlujoProtocolar : int
        {
            CONSULTA = 1,
            REGISTRO = 2,
            EDICION = 3,
            RECTIFICACION = 4,
            PARTE = 5,
            TESTIMONIO = 6            
        }

        public enum enmNotarialLibroTipo : int
        {
            ESCRITURA_PUBLICA = 8251
        }

        public enum enmNotarialTipoFormato : int
        {
            ESCRITURA = 8141,
            PARTE = 8142,
            TESTIMONIO = 8143,
            OTROS = 8144,
        }

        public enum enmNotarialAccion : int
        {
            RECTIFICACION = 4,
            CONSULTA = 1054,
            EDICION = 1056,            
            SOLICITUD = 1058
        }
        


        #endregion

        #region Judicial
        public enum enmJudicialExpedienteEstado : int
        {
            REGISTRADO = 62,
            ENVIADO = 63,
            CERRADO = 64,
            OBSERVADO = 65,
            ANULADO = 74
        }

        public enum enmJudicialNotificacionEstado : int
        {
            REGISTRADO = 66,
            ENVIADO = 67,
            NOTIFICACION_RECIBIDA = 68,
            ANULADO = 75,
            NOTIFICACION_NO_RECIBIDA = 97

        }

        public enum enmJudicialActaEstado : int
        {
            REGISTRADO = 69,
            ENVIADO = 70,
            OBSERVADO = 71,
            ANULADO = 76
        }

        public enum enmJudicialParticipanteEstado : int
        {
            REGISTRADO = 72,
            NOTIFICADO = 73,
            ANULADO = 77,
            CERRADO = 96
        }

        public enum enmJudicialTipoNotificacion : int
        {
         
            NOTICACION_JUDICIAL = 8501,
            NOTICACION_ADMINISTRATIVA = 8502,
        }

        public enum enmJudicialViaEnvio : int
        {
            NOTIFICACION_PERSONAL = 8551,
            NOTIFICACION_CORREO = 8552,
        }

        public enum enmJudicialTipoRecepcion : int
        {
            RECIBIDO_POR_EL_DESTINATARIO = 8521,
            RECIBIDO_POR_EL_DESTINATARIO_NEGANDOSE_A_FIRMAR = 8522,
            RECIBIDO_POR_TERCERO_MAYOR_DE_EDAD_EN_EL_DOMICILIO = 8523,
            DEJADO_BAJO_LA_PUERTA = 8524,
            DEJADO_EN_EL_BUZON = 8525,
            OTROS = 8526

        }

        public enum enmJudicialTipoActa : int
        {
            ACTA_DILIGENCIAMIENTO = 8531,
            ACTA_COMPLEMENTARIA = 8532
        }

        public enum enmJudicialTipoParticipante : int
        {
            DEMANDANTE = 8541,
            DEMANDADO = 8542,
            RECURRENTE = 8543

        }

        #endregion

        #region Reportes
            
            #region ReportesGerenciales
            public enum enmReportesGerenciales : int
            {
                RGE_CONSOLIDADO = 10001,
                MAYOR_VENTA_Y_DETALLE  =10002,
                VENTAS_POR_MES = 10003,
                TARIFA_CONSULAR_POR_PAIS = 10004,
                RECORD_DE_VENTA = 10005,
                RECORD_DE_ACTUACIONES = 10006,
                TOP_14_MAYOR_VENTA_POR_PAIS = 10007,
                RGE_POR_CONTIENENTE = 10008,
                RGE_POR_CATEGORIA_POR_RECORD_DE_VENTA = 10009,
                RGE_POR_CATEGORIA = 10010,
                AUTOADHESIVOS_USUARIO_OFICINA_CONSULAR = 5006,
                ACTUACIONES_USUARIO_OFICINA_CONSULAR = 5007,
                CONSOLIDADO_ACTUACIONES_TIPO_PAGO = 10011,
                //------------------------------------------------
                //Fecha: 29/12/2016
                //Autor: Jonatan Silva Cachay
                //Objetivo: Enumerador para nuevos reporte
                //------------------------------------------------
                PERSONAS_USUARIO_OFICINA_CONSULAR = 11064,
                CANTIDAD_ACTUACIONES_CONSULADO = 11065,
                CANTIDAD_TARIFAS_REGISTRADAS = 11066
            }
            #endregion
        #region Acto Migratorio
            public enum enmReportesActoMigratorio : int
            {
                PASAPORTES_EN_GENERAL = 10101,
                CONSOLIDADO_DE_TRAMITES_POR_ANIO = 10102,
                INVENTARIO_DE_DOCUMENTOS_DE_VIAJE = 10103,
                SALVOCONDUCTOS_EN_GENERAL = 10104
            }
        #endregion
        #endregion
       
    }
}
