using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;

namespace SGAC.Accesorios
{
    public class Constantes
    {
        public const int CONST_ID_MRE = 1;
        public const int CONST_CANT_REGISTRO = 10;
        public const int CONST_CANT_REGISTRO_REPORTE = 100000;
        public const string CONST_CLAVE_DEFECTO_ENCRIPTADA = "KawlZg4weOh+MJfTgi5Q1w=="; //Password: 12345678
        public const int CONST_PORCENTAJE_MAX_TC = 15; // Reglamento Consular 2005
        public const string CONST_UBIGEO_PERU = "920000"; // Creado
        public const int CONST_OFICINACONSULAR_LIMA = 1;   // PS_SISTEMA.SI_OFICINACONSULAR
        public const string CONST_OFICINACONSULAR_LIMA_UBIGEO = "140101";
        public const int CONST_DOLAR_ID = 42; // 40; antiguo - SC_MAESTRO.MA_MONEDA
        public const int CONST_SESION_ROLOPCION_INICIO = 1; // PS_SEGURIDAD.SE_ROLOPCION

        public const string CONST_ACTUACION_TIPO_ANOTACION_POR_CANCELACION = "4105";

        public const int CONST_TAMANIO_MAX_ADJUNTO_KB = 1024;
        public const int CONST_TAMANIO_MIN_ADJUNTO_FOTO_KB = 10;
        public const int CONST_TAMANIO_MAX_ADJUNTO_FOTO_KB = 18;
        public const int CONST_TAMANIO_MIN_ADJUNTO_HUELLA_KB = 10;
        public const int CONST_TAMANIO_MAX_ADJUNTO_HUELLA_KB = 20;
        public const int CONST_TAMANIO_MIN_ADJUNTO_FIRMA_KB = 5;
        public const int CONST_TAMANIO_MAX_ADJUNTO_FIRMA_KB = 10;


        public const string CONST_DOCUMENTO_PADRE_MADRE = "0"; //29/03/2016 

        public const int CONST_CONTROLADOR_ERROR_NO = 0; 
        public const int CONST_UNIDAD_PESO = 1000;

        public const int CONST_DIAS_FECHA_PASAPORTE = 1;
        public const int CONST_VALIDACION_DECLARANTE = 2;
        public const int CONST_VALIDACION_PADRES = 2;

        public const string CONST_VALIDACION_DECLARANTE1 = "DECLARANTE 1";
        public const string CONST_VALIDACION_DECLARANTE2 = "DECLARANTE 2";

        public const string CONST_VALIDACION_IDIOMA = "CASTELLANO";

        public const string CONST_VALOR_PERUANO = "1";

        public const string CONST_VALOR_OTROS = "OTROS";
        //-------------------------------------------------------------------------------------------
        // Autor: Vidal Pipa
        // Fecha: 24/11/2020
        // Objetivo: Nuvo Reporte
        //-------------------------------------------------------------------------------------------
        public const string CONST_REPORTES_ACTUACIONES_MENSUALES_X_CONSULADO = "ACTUACIONES MENSUALES POR CONSULADO";
        //-------------------------------------------------------------------------------------------
        // Autor: JONATAN SILVA CACHAY
        // Fecha: 10/03/2017
        // Objetivo: VARIABLE TIPO DE REGISTRO
        //-------------------------------------------------------------------------------------------
        public const string CONST_REGISTRO_MANUAL = "REGISTRO MANUAL";
        public const string CONST_REGISTRO_SEMIAUTOMATICO = "REGISTRO SEMIAUTOMATICO";

        //-------------------------------------------------------------------------------------------
        // Autor: JONATAN SILVA CACHAY
        // Fecha: 11/04/2017
        // Objetivo: VARIABLE REPORTE GERENCIAL
        //-------------------------------------------------------------------------------------------
//        public const string CONST_REPORTES_RANKING_RECAUDACION = "REPORTE RANKING DE RECAUDACIÓN Y ACTUACIONES CONSULARES";
        public const string CONST_REPORTES_RANKING_RECAUDACION = "REPORTE DE RECAUDACIÓN Y ACTUACIONES CONSULARES";
        public const string CONST_REPORTES_RANKING_CAPTACION = "REPORTE DE CAPTACIÓN CONSULAR";
        public const string CONST_REPORTES_CUADRO_SALDOS_AUTOADHESIVOS = "CUADRO DE SALDOS DE AUTOADHESIVOS";
        public const string CONST_REPORTES_CUADRO_AUTOADHESIVOS_UTILIZADOS = "CUADRO DE AUTOADHESIVOS UTILIZADOS";
        public const string CONST_REPORTES_ITINERANTES = "REPORTE DE ITINERANTES";
        public const string CONST_REPORTES_TITULARES = "REPORTE DE TITULARES";
        public const string CONST_REPORTES_RECAUDACION_MENSUAL = "REPORTE DE RECAUDACIÓN MENSUALIZADO";
        public const string CONST_REPORTES_RECAUDACION_DIARIA = "REPORTE DE RECAUDACIÓN DIARIO";
        public const string CONST_REPORTES_RECAUDACION_TARIFA = "REPORTE DE RECAUDACIÓN POR TARIFAS";
        public const string CONST_REPORTES_CARGA_INICIAL_CORRELATIVO = "REPORTE DE CARGA INICIAL DE CORRELATIVOS";
       
        //-------------------------------------------------------------------------------------------
        // Autor: JONATAN SILVA CACHAY
        // Fecha: 11/04/2017
        // Objetivo: VARIABLE REPORTE ARQUEO
        //-------------------------------------------------------------------------------------------
        public const string CONST_REPORTES_CONSOLIDADO_ACTUACIONES_USUARIO = "CONSOLIDADO DE RECAUDACIÓN POR USUARIO";
        public const string CONST_REPORTES_RESUMEN_DIA_ACTUACIONES_USUARIO = "DETALLE DE RECAUDACIÓN POR USUARIO";
        public const string CONST_REPORTES_CORRELATIVOS = "REPORTE DE CORRELATIVOS POR TARIFA";
        //-------------------------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15-08-2016 / 08-11-2016
        // Objetivo: Estandarizar el nombre de la variable del grupo: 
        //           ACTUACIÓN-CLASIFICACION TARIFA / NORMA-ESTADO / FICHA-ESTADO 
        //           REPORTES-RENIEC / TIPO-ENVIO-RENIEC
        //-------------------------------------------------------------------------------------------
        public const string CONST_ACTUACION_CLASIFICACION_TARIFA = "ACTUACIÓN-CLASIFICACION TARIFA";
        public const string CONST_NORMA_ESTADO = "NORMA-ESTADO";
        public const string CONST_FICHA_ESTADO = "FICHA-ESTADO";
        public const string CONST_REPORTES_RENIEC = "REPORTES-RENIEC";
        public const string CONST_REPORTES_REGISTRO_CIVIL = "REPORTES-REGISTROCIVIL";
        public const string CONST_FORMATO_REGISTRO_CIVIL = "FORMATO-REGISTROCIVIL";
        public const string CONST_RENIEC_FORMATO_ENVIO_TARIFA = "FORMATO DE ENVIÓ (TARIFAS - SECCIÓN 8)";
        public const string CONST_RENIEC_FORMATO_ENVIO_ESTADO = "FORMATO DE ENVIÓ (ESTADOS - FICHA REGISTRAL)";
        public const string CONST_RENIEC_GUIA_DESPACHO = "GUÍA DE DESPACHO";
        public const string CONST_RENIEC_RENDICION_CUENTAS = "RENDICIÓN DE CUENTAS POR TARIFAS RENIEC";
        public const string CONST_RENIEC_CONCILIACION = "REPORTE DE CONCILIACIÓN DE RENIEC";
        public const string CONST_RENIEC_TIPO_ENVIO = "TIPO-ENVIO-RENIEC";
        public const string CONST_GUIA_DESPACHO_ESTADO = "GUIA-DESPACHO-ESTADO";
        public const string CONST_REPORTE_ARQUEO = "REPORTES - ARQUEO";
        public const string CONST_REPORTE_RENIEC_INCOMPLETOS = "TRAMITES INCOMPLETOS";

        public const string CONST_TIPO_DECLARANTE_RENIEC = "TIPO-DECLARANTE-RENIEC";
        public const string CONST_TIPO_TUTOR_GUARDADOR_RENIEC = "TIPO-TUTOR-GUARDADOR-RENIEC";

        public const string CONST_DOC_ADJUNTOS_RENIEC_MAYOR = "DOC-ADJUNTOS-RENIEC-MAYOR";
        public const string CONST_DOC_ADJUNTOS_RENIEC_MENOR = "DOC-ADJUNTOS-RENIEC-MENOR";

        public const string CONST_IMPRESION_COMPLETA = "IMPRESION_COMPLETA_RENIEC";

        public const string CONST_RESTRICCIONES_FICHA_RENIEC = "RESTRICCIONES_FICHA_RENIEC";
        public const string CONST_TARIFAS_SIN_FICHAS_REGISTRALES = "TARIFAS_SIN_FICHAS_REGISTRALES";

        public const string CONST_DOCUMENTOS_IMPRESION = "DOCUMENTOS-IMPRESION";
        public const string CONST_DOC_FICHA_MAYOR_EDAD = "FICHA REGISTRAR - MAYOR DE EDAD";
        public const string CONST_DOC_FICHA_MENOR_EDAD = "FICHA REGISTRAR - MENOR DE EDAD";
        //-------------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 23-04-2018 
        // Objetivo: constantes para reporte de registro civil
        //-------------------------------------------------------------------------------------------
        public const string CONST_REPORTE_ACTA_NACIMIENTO = "LISTADO DE REGISTRO CIVIL - ACTA DE NACIMIENTO";
        
        //-------------------------------------------------------------------------------------------
        // Autor: JONATAN SILVA CACHAY
        // Fecha: 24/07/2017
        // Objetivo: CLASIFICACIÓN DE ENVIO DE REMESA
        //-------------------------------------------------------------------------------------------
        public const string CONST_CLASIFICACION_REMESA = "CLASIFICACIÓN DE ENVIO DE REMESA";

        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        // Autor: Sandra Acosta Celis
        // Fecha: 06/02/2017
        // Objetivo: CONSTANTE TIPO DE CONFIGURACIÓN ALMACEN
        //-------------------------------------------------------------------------------------------
        public const string CONST_ALMACEN = "ALMACEN";

        #region Participante 
        public const string CONST_PARTICIPANTE_NACIMIENTO = "TITULAR, PADRE, MADRE, DECLARANTE , DECLARANTE , REGISTRADOR CIVIL";
        public const string CONST_PARTICIPANTE_MATRIMONIO = "CELEBRANTE, DON, DOÑA, REGISTRADOR CIVIL";
        public const string CONST_PARTICIPANTE_DEFUNCION = "TITULAR, PADRE, MADRE, REGISTRADOR CIVIL";
        public const string CONST_PARTICIPANTE_MILITAR = "PADRE, MADRE, CÓNYUGE";

        public const int CONST_ACTO_JUDICIAL_PARTICIPANTE_TIPO = 8541;   
        #endregion

        #region Texto Reporte

        public const string CONST_REPORTE_SUB_TITULO = "SERVICIO CONSULAR DEL PERÚ";

        #endregion

        #region Tarifario
        public const int CONST_EXCEPCION_TARIFA_ID_1 = 1; // 1 - INSCRIPCION NACIMIENTO, MATRIMONIO O DEFUNCION
        public const int CONST_EXCEPCION_TARIFA_ID_2 = 2; // 2 - INSCRIPCION SENTENCIAS O ANOTACIONES EST. CIVIL
        public const int CONST_EXCEPCION_TARIFA_ID_3 = 3; // 2* - ANOTACIÓN EN RECTIFICACIÓN
        public const int CONST_EXCEPCION_TARIFA_ID_4 = 4; // 3 - COPIA CERTIFICADA AS. ESTADO CIVIL
        public const int CONST_EXCEPCION_TARIFA_ID_5 = 5; // 3A - 1º COPIA CERTIFICADA AS. ESTADO CIVIL - GRATIS
        public const int CONST_EXCEPCION_TARIFA_ID_6 = 6; // 3B - COPIA CERTIFICADA REG.MILITAR
        
        public const int CONST_EXCEPCION_TARIFA_ID_171 = 171; // 79A - POR LA INSCRIPCION REGISTRO MILITAR

        public const string CONST_EXCEPCION_TARIFA_3A = "3A";
        public const string CONST_EXCEPCION_TARIFA_3B = "3B";
        public const string CONST_EXCEPCION_TARIFA_79A = "79A";
        public const string CONST_EXCEPCION_TARIFA_58A = "58A";
        public const string CONST_EXCEPCION_TARIFA_63 = "63";
        public const string CONST_EXCEPCION_TARIFA_66 = "66";
        public const string CONST_EXCEPCION_TARIFA_12A = "12A";
        public const string CONST_EXCEPCION_TARIFA_13A = "13A";
        public const string CONST_EXCEPCION_TARIFA_17A = "17A";
        public const string CONST_EXCEPCION_TARIFA_17B = "17B";
        public const string CONST_EXCEPCION_TARIFA_17C = "17C";
        public const string CONST_EXCEPCION_TARIFA_ID_122 = "122";
                
        public const int CONST_TARIFA_SOLO_EXTRANJERO_76 = 76;
        public const int CONST_TARIFA_32_A = 78;
        public const int CONST_TARIFA_32_B = 79;
        public const int CONST_TARIFA_32_C = 80;
        public const int CONST_TARIFA_33_A = 81;
        public const int CONST_TARIFA_33_B = 82;
        public const int CONST_TARIFA_33_C = 83;

        // TARIFAS EXTRA-PROTOCOLAR
        public const string CONST_TARIFA_EXTRA_PODER_FUERA = "15A";
        public const string CONST_TARIFA_EXTRA_SUPERVIVENCIA = "28A";
        public const string CONST_TARIFA_EXTRA_AUTORIZACION = "14";

        /*---------------------------------------------------------------------------------------------------------------*/
        /*CONSTANTES DE TARIFAS PARA ACTOS PROTOCOLARES*/
        /*---------------------------------------------------------------------------------------------------------------*/
        public const int CONST_PROTOCOLAR_ID_TARIFA_17A = 50;
        public const int CONST_PROTOCOLAR_ID_TARIFA_17B = 51;
        public const int CONST_PROTOCOLAR_ID_TARIFA_17C = 52;
        
        public const int CONST_PROTOCOLAR_ID_TARIFA_5A = 8;
        public const int CONST_PROTOCOLAR_ID_TARIFA_5B = 9;
        public const int CONST_PROTOCOLAR_ID_TARIFA_5C = 10;
        public const int CONST_PROTOCOLAR_ID_TARIFA_6A = 11;
        public const int CONST_PROTOCOLAR_ID_TARIFA_6B = 12;
        public const int CONST_PROTOCOLAR_ID_TARIFA_6C = 13;
        public const int CONST_PROTOCOLAR_ID_TARIFA_7A = 14;
        public const int CONST_PROTOCOLAR_ID_TARIFA_7B = 15;
        public const int CONST_PROTOCOLAR_ID_TARIFA_7C = 16;
        public const int CONST_PROTOCOLAR_ID_TARIFA_7D = 17;

        public const int CONST_PROTOCOLAR_ID_TARIFA_8A = 18;
        public const int CONST_PROTOCOLAR_ID_TARIFA_8B = 19;
        public const int CONST_PROTOCOLAR_ID_TARIFA_8C = 20;

        public const int CONST_PROTOCOLAR_ID_TARIFA_9A = 21;
        public const int CONST_PROTOCOLAR_ID_TARIFA_9B = 22;
        public const int CONST_PROTOCOLAR_ID_TARIFA_10A = 23;
        public const int CONST_PROTOCOLAR_ID_TARIFA_10B = 24;
        public const int CONST_PROTOCOLAR_ID_TARIFA_11A = 25;
        public const int CONST_PROTOCOLAR_ID_TARIFA_11B = 26;

        public const int CONST_PROTOCOLAR_ID_TARIFA_12A = 27;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12B = 28;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12C = 29;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12D = 30;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12E = 31;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12F = 32;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12G = 33;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12H = 34;
        public const int CONST_PROTOCOLAR_ID_TARIFA_12I = 35;

        public const int CONST_PROTOCOLAR_ID_TARIFA_13A = 36;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13B = 37;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13C = 38;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13D = 39;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13E = 40;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13F = 41;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13G = 42;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13H = 43;
        public const int CONST_PROTOCOLAR_ID_TARIFA_13I = 44;

        public const int CONST_PROTOCOLAR_ID_TARIFA_20A = 58;
        public const int CONST_PROTOCOLAR_ID_TARIFA_20B = 59;

        /*---------------------------------------------------------------------------------------------------------------*/

        #endregion

        #region Tarifario Dependencia
                public const string CONST_EXCEPCION_TARIFA_DEPENDENCIA_17C = "17B";

        #endregion
        #region Paginacion
        public const int CONST_PAGE_SIZE_PERSONA_RUNE = 20;
        public const int CONST_PAGE_SIZE_ANOTACIONES = 5;
        public const int CONST_PAGE_SIZE_PERSONA_RECURRENTE = 20;
        public const int CONST_PAGE_SIZE_ACTUACIONES = 5;
        public const int CONST_PAGE_SIZE_NORIFICACIONES = 5;
        public const int CONST_PAGE_SIZE_ADJUNTOS = 5;
        public const int CONST_PAGE_SIZE_TARIFARIO = 10;
        public const int CONST_PAGE_SIZE_PERSONA_REASIGNACION = 10;
        #endregion

        #region Mensajes Actuaciones
        public const string CONST_MENSAJE_TARIFA_SOLO_PERUANO = "Solo aplica para nacionaliad EXTRANJERA y al menos uno de sus padres sea PERUANO";
        public const string CONST_MENSAJE_TARIFA_SOLO_EXTRANJERO = "Tarifa no aplica a peruanos en el exterior.";
        public const string CONST_MENSAJE_VALIDA_TIPO_PERSONA = "El tipo de persona ingresado no puede realizar esta actuación.";
        public const string CONST_MENSAJE_VALIDA_ACTO_JUDICIAL = "La actuación judicial se debe registra desde Expediente.";
        public const string CONST_MENSAJE_VALIDA_ACTO_NOTARIAL  = "La actuación seleccionada se debe registrar desde Acto Notarial.";
        public const string CONST_MENSAJE_VALIDA_TARIFA_CANTIDAD = "No se puede volver a registrar esta tarifa.";
        public static string CONST_MENSAJE_TARIFA_FECHA_VENCIMIENTO = "Tarifa no aplica porque existe una tarifa pendiente de aprobación o expiración.";

        public static string CONST_MENSAJE_PERDIDA_SESSION_ACTUACION = "SE HA PERDIDO DATOS EN LA ACTUACION, POR FAVOR REFRESQUE LA PAGINA.";
        public static string CONST_MENSAJE_SIN_PARTICIPANTES = "DEBE REGISTRAR AL MENOS UN PARTICIPANTE PARA PODER GRABAR.";

        public const string CONST_MENSAJE_VALIDA_TARIFA_EDAD = "Para registrar la tarifa 1. El recurrente tiene que ser mayor de edad.";
        public const string CONST_MENSAJE_VALIDA_TARIFA_EDAD_SINFECHA = "Para registrar la tarifa 1. Se debe ingresar la fecha de nacimiento del recurrente.";
        #endregion

        #region Mensajes Generales
        public const string CONST_MENSAJE_EXITO = "El registro ha sido grabado correctamente.";
        public const string CONST_MENSAJE_MANT_EXITO = "Los cambios se han realizado correctamente.";
        public const string CONST_RUC_NO_VALIDO = "El número de documento no es válido";
        public const string CONST_SERVICIO_SUNAT_NO_RESPONDE = "El servicio de Sunat no responde";
        public const string CONST_MENSAJE_DNI_DUPLICADO = "El DNI ingresado ya se encuentra registrado";
        public const string CONST_MENSAJE_NOTIFICACIONES = "Solo se permiten dos notificaciones";
        public const string CONST_MENSAJE_ELIMINADO = "El registro ha sido eliminado";
        public const string CONST_MENSAJE_EXITO_ANULAR = "El registro fue anulado correctamente";
        public const string CONST_MENSAJE_OPERACION_FALLIDA = "No se pudo realizar la operación";
        public const string CONST_MENSAJE_OPERACION_FALLIDA_ESTADO = "No se pudo realizar el cambio de estado";
        public const string CONST_MENSAJE_ERROR_MANTENIMIENTO = "No se han grabado los cambios";
        public const string CONST_MENSAJE_BUSQUEDA_EXITO = "La búsqueda se ha realizado con éxito. Total Registros: ";
        public const string CONST_MENSAJE_NO_SELECCION_FILTROS = "No ha seleccionado el filtro de búsqueda.";
        public const string CONST_MENSAJE_NO_IMPLEMENTACION = "No hay implementación";

        public const string CONST_MENSAJE_NO_TIENE_ANOTACIONES = "No tiene Asignado ninguna Anotación";
        public const string CONST_MENSAJE_CAMBIO_OFICINA_CONSULAR = "Se ha cambiado la oficina consular del usuario. Para que pueda ver los cambios deberá volver a iniciar Sesión.";

        public const string CONST_VALIDACION_MIN_3_CARACTERES = "Debe ingresar mínimo 3 caracteres para realizar la búsqueda.";
        public const string CONST_VALIDACION_BUSQUEDA = "No se encuentran registros";
        public const string CONST_VALIDACION_DOS_FECHAS = "La Fecha Final es menor a la Inicial.";
        public const string CONST_VALIDACION_TIPO_ACTO = "Selecione el tipo de acto a consultar.";
        public const string CONST_VALIDACION_FECHA = "Formato de Fecha Incorrecto : dic-01-2014" ;
        public const string CONST_VALIDACION_FECHA_VACIA = "La Fecha no ha sido ingresada.";
        public const string CONST_VALIDACION_FALTAFILTRO = "No hay suficientes datos para la búsqueda";
        public const string CONST_VALIDACION_FECHA_INICIAL = "La fecha inicial no es válida.";
        public const string CONST_VALIDACION_FECHA_FINAL = "La fecha final no es válida.";
        public const string CONST_VALIDACION_FECHA_NO_VALIDA = "La fecha no es válida.";        

        public const string CONST_MENSAJE_SALDO_INSUFICIENTE = "No se podrá registrar ninguna actuación porque el saldo de los autoadhesivos es insuficiente.";
        public const string CONST_MENSAJE_SIN_TIPOCAMBIO = "No se podrá registrar ninguna actuación porque no se ha establecido el tipo de cambio del día.";

        public const string CONST_MENSAJE_ACT_DIGITALIZADO = "No se podrá editar la actuación porque se encuentra Digitalizada.";
        public const string CONST_MENSAJE_CONSULADO_DIFERENTE = "No se podrá editar la actuación porque la Oficina Consular seleccionada no es la Oficina Consular origen.";
        public const string CONST_MENSAJE_CONSULADO_DIFERENTE_LIMA = "Solo LIMA puede modificar los trámites realizados en los consulados.";
        public const string CONST_MENSAJE_TARIFA_3A = "No se podrá editar la actuación porque es de tarifa 3A.";
        public const string CONST_MENSAJE_TARIFA_3A_CONSULTA = "No se podrá consultar la actuación porque es de tarifa 3A.";
        public const string CONST_MENSAJE_TIPODOC_NOSELECCIONADO = "Seleccione el tipo de documento.";

        public const string CONST_MENSAJE_GENERA_PDF = "Ha ocurrido un error en la generación del Documento PDF. Favor de verificar el texto ingresado.";

        public const string CONST_MENSAJE_ACTUACION_PERSONA_FALLECIDA = "No puede realizar ningun trámite, la persona se encuentra fallecida.";

        public const string CONST_MENSAJE_STOCK_MINIMO = "El Stock del insumo es: (valorstock) y se encuentra por debajo del stock mínimo requerido (valorStockMinimo), solicitar el pedido a Lima.";

        public const string CONST_MENSAJE_INGRESAR_AUTOADHESIVO = "No ha ingresado el Autoadhesivo, por favor ingresar el Número de Autoadhesivo";

        public const Int16 CONST_ID_CONSULADO_CARACAS = 62;
        #endregion

        #region Tab TÍTULO
        public const string CONST_TAB_INICIAL = "Consulta";
        public const string CONST_TAB_CONSULTAR = "Detalle";
        public const string CONST_TAB_REGISTRAR = "Registro";
        public const string CONST_TAB_EDITAR = "Modifica";
        public const string CONST_TAB_ELIMINAR = "Elimina";
        #endregion

        #region Nombres Variables de Sesion
        public const string CONST_SESION_IDIOMA = "IdiomaSistema";

        public const string CONST_SESION_USUARIO = "UsuarioSistema";
        public const string CONST_SESION_USUARIO_ROL = "UsuarioSistemaRol";
        public const string CONST_SESION_USUARIO_ID = "UsuarioSistemaId";
        public const string CONST_SESION_GRUPO_ID = "UsuarioSistemaGrupoId";
        public const string CONST_SESION_ROL_ID = "UsuarioSistemaTipoRolId";
        public const string CONST_SESION_ACCESO_ID = "UsuarioAccesoId";
        public const string CONST_SESION_NOTI_REMESA = "UsuarioNotificaRemesa";
        public const string CONST_SESION_USUARIO_ROL_TIPO_ACCESO = "UsuarioRolTipoAcceso";

        public const string CONST_SESION_CONTINENTE = "UsuarioContinente";

        public const string CONST_SESION_OFICINACONSULTA_DT = "OficinaConsularDt";
        public const string CONST_SESION_OFICINACONSULAR_ID = "OficinaConsularId";
        public const string CONST_SESION_OFICINACONSULAR_REF_ID = "OficinaConsularRefId";
        public const string CONST_SESION_OFICINACONSULAR_NOMBRE = "OficinaConsularNombre";

        public const string CONST_SESION_CIUDAD_ITINERANTE = "Itinerante";
        public const string CONST_SESION_CIUDAD_CODIGO_ITINERANTE = "CodItinerante";
        //-------------------------------------------------------
        //Fecha:15/02/2017
        //Autor:Miguel Márquez Beltrán
        //Objetivo: Almacenar el código de local del consulado
        //-------------------------------------------------------
        public const string CONST_SESION_OFICINACONSULAR_CODIGOLOCAL = "vCodigoLocal";
        //-------------------------------------------------------
        //Fecha: 15/03/2017
        //Autor:Miguel Márquez Beltrán
        //Objetivo: Almacenar la tabla Paises
        //-------------------------------------------------------
        public const string CONST_SESION_TABLA_PAISES = "DT_Paises";
        //-------------------------------------------------------
        //-----------------------------------------------------------------
        //Fecha: 02/12/2019
        //Autor:Miguel Márquez Beltrán
        //Objetivo: Almacenar la diferencia horaria y horario de verano.
        //-----------------------------------------------------------------
        public const string CONST_SESION_DIFERENCIA_HORARIA = "sesion_diferencia_horaria";
        public const string CONST_SESION_HORARIO_VERANO = "sesion_horario_verano";
        
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 07/12/2021
        // Objetivo: Adicionar la constante Siglas de la Oficina Consular
        //------------------------------------------------------------------------
        public const string CONST_SESSION_VSIGLAS = "ofco_vSiglas";
        //------------------------------------------------------------------------
        public const string CONST_SESION_TIPO_CAMBIO_BANCARIO = "TipoCambioBancarioValor";
        public const string CONST_SESION_TIPO_CAMBIO = "TipoCambioValor";
        public const string CONST_SESION_TIPO_MONEDA = "TipoMonedaNombre";
        public const string CONST_SESION_TIPO_MONEDA_ID = "TipoMonedaId";

        public const string CONST_SESION_TIPO_MONEDA_PAIS_ID = "TipoMonedaPaisId";

        public const string CONST_SESION_PAIS_ID = "PaisId";
        public const string CONST_SESION_IDIOMA_ID = "IdiomaId";
        public const string CONST_SESION_IDIOMA_TEXTO = "IdiomaTexto";
        public const string CONST_SESION_UBIGEO = "UbigeoCodigo";
        public const string CONST_SESION_ES_JEFATURA = "bEsJefatura";

        public const string PARA_GRUPO_OTROS_PARAMETROS = "OTROS PARAMETROS";
        public const string OTROS_PARAMETROS_FECHAFIN_TEMPORAL = "FECHA_FIN_MODIFICA_TEMPORAL";

        //-------------------------------------------------------
        //Fecha: 06/11/2017
        //Autor:Jonatan Silva Cachay
        //Objetivo: Parametros de Conciliación
        //-------------------------------------------------------
        public const string CONST_PARAMETRO_INGRESO_CONCILIACION = "CONTABILIDAD-TIPO INGRESO CONCILIACION";
        public const string CONST_PARAMETRO_EGRESO_CONCILIACION = "CONTABILIDAD-TIPO EGRESO CONCILIACION";
        public const string CONST_PARAMETRO_ESTADO_DEPOSITO_CONCILIACION = "CONTABILIDAD-ESTADO-DEPOSITO CONCILIACION";

        public const string CONST_PARAMETRO_ESTADO_CONCILIACION_PENDIENTE = "PENDIENTE";
        public const string CONST_PARAMETRO_ESTADO_CONCILIACION_CONCILIADO = "CONCILIADO";
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 18/08/2016
        // Objetivo: Declarar la variable si es jefatura con respecto al procedimiento: 
        //           [PS_SISTEMA].[USP_SI_PARAMETRO_CARGA_INICIAL]
        //------------------------------------------------------------------------
        public const string CONST_SESION_JEFATURA_FLAG = "ofco_iJefaturaFlag";
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 03/04/2017
        // Objetivo: Declarar la variable lista de departamento.
        //------------------------------------------------------------------------
        public const string CONST_SESION_LISTA_DPTO = "LIST_DPTO";
        //------------------------------------------------------------------------

        public const string CONST_SESION_DT_TARIFARIO = "DT_Tarifas";
        public const string CONST_SESION_DT_TARIFARIOREQUISITO = "DT_Requisitos";
        public const string CONST_SESION_DT_OFICINACONSULAR = "DT_OficinasConsulares";
        public const string CONST_SESION_DT_PARAMETRO = "DT_dtParametros";
        public const string CONST_SESION_DT_ESTADO = "DT_dtEstados";
        public const string CONST_SESION_DT_MAESTRA = "DT_dtMaestras";
        public const string CONST_SESION_DT_SISTEMA = "DT_dtSistemas";
        public const string CONST_SESION_DT_DOCUMENTOIDENTIDAD = "DT_DocumentoIdentidad";
        public const string CONST_SESION_DT_TARIFARIOCONSULTAS = "DT_TarifasConsultas";
        public const string CONST_SESION_DT_OFICINACONSULARACTIVAS = "DT_OficinasConsularesActivas";
        
        public const string CONST_SESION_DT_FUNCIONARIO = "DT_dtFuncionario";

        public const string CONST_SESION_DT_CONFIGURACION = "DT_Configuracion";
        public const string CONST_SESION_DT_CONFIGURACION_EXTRA = "DT_Configuracion_Extra";

        public const string CONST_SESION_DT_BOVEDA = "DT_Boveda";

        public const string CONST_SESION_REPORTE_DT = "DT_RptGeneral";
        public const string CONST_SESION_REPORTE_TIPO = "Tipo_RptGeneral";

        public const string CONST_SESION_HOSTNAME = "HostName";
        public const string CONST_SESION_DIRECCION_IP = "DireccionIP";

        public const string CONST_SESION_ACTUACIONDET_ID = "ACTDET_ID";        
        public const string CONST_SESION_ACTUACIONDET_ID_AUX = "ACTDET_ID_AUX";
        public const string CONST_SESION_ACTUACION_ID = "ACT_ID";
        public const string CONST_ACTUACION_INSUMO_DETALLE_ID = "ACT_INS_DET_ID";
        public const string CONST_ACTUACION_INSUMO_DETALLE_ID_LAMINA = "ACT_INS_DET_ID_LAMINA";
        public const string CONST_ACTUACION_INSUMO_DETALLE_ID_PAS = "ACT_INS_DET_ID_PAS";
        public const string CONST_SESION_NACIONALIDAD_ID = "PER_NACIONALIDAD";
        public const string CONST_SESION_PERSONA_ID = "iPersonaId";
        public const string CONST_SESION_ACTUACIONDET_TABS = "ACTDET_TABS";
        public const string CONST_SESION_OBJ_TARIFA_PAGO = "OBJ_TARIFA_PAGO";
        public const string CONST_SESION_OBJ_TARIFA_PAGO_MIGRA = "OBJ_TARIFA_PAGO_MIGRA";

        public const string CONST_SESION_ACTONOTARIAL_ID = "acno_iActoNotarialId";
        public const string CONST_SESION_ACTONOTARIAL_TIPO_ID = "acno_sTipoActoNotarialId";
        public const string CONST_CERTIFICADO_ANTECEDENTES_PENALES = "CERTIFICADO DE ANTECEDENTES PENALES";
        public const string CONST_CERTIFICADO_ANTECEDENTES_PENALES_USUARIO = "CERTIFICADO DE ANTECEDENTES PENALES POR USUARIO";

        #endregion

        #region ASN
        public const int CONST_ESTADO_ASISTENCIA_CONCLUIDO = 28;

        #endregion ASN

        #region Nacionalidad

        public const string CONST_NACIONALIDAD_PERUANA = "2051";
        public const string CONST_NACIONALIDAD_EXTRANJERA = "2052";
        
        #endregion

        #region Contabilidad

        #endregion Contabilidad

        #region CUI

        public const int CONST_EXCEPCION_CUI_ID = 6;
        public const String CONST_EXCEPCION_CUI = "CUI";
        #endregion
 
    }
}
