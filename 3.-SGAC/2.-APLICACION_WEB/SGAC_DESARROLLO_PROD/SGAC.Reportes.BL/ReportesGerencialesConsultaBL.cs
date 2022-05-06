using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.Reportes.DA;
using System.Data;
namespace SGAC.Reportes.BL
{
    public class ReportesGerencialesConsultaBL
    {
        ReportesGerencialesConsultasDA oReportesGerencialesConsultasDA = new ReportesGerencialesConsultasDA();

        public DataSet ObtenerMayorVentaDetalle(DateTime dFecInicio, DateTime dFecFin)
        {
            return oReportesGerencialesConsultasDA.ObtenerMayorVentaDetalle(dFecInicio, dFecFin);
        }

        public DataSet ObtenerVentaMes(Int16 ofco_sOficinaConsularId, DateTime dFecInicio, DateTime dFecFin)
        {
            return oReportesGerencialesConsultasDA.ObtenerVentaMes(ofco_sOficinaConsularId, dFecInicio, dFecFin);
        }
        //-------------------------------------------------------------------------
        //Autor: Miguel Márquez Beltrán
        //Fecha: 26/06/2017
        //Objetivo: Obtener el consolidado de las actuaciones por tipo de pago.
        //-------------------------------------------------------------------------

        public DataSet ObtenerConsolidadoActuacionesPorTipoPago(Int16 sOficinaConsular, string strPeriodo)
        {
            return oReportesGerencialesConsultasDA.ObtenerConsolidadoActuacionesPorTipoPago(sOficinaConsular, strPeriodo);
        }

        public DataSet ObtenerTarifaConsularPaís(string tari_sTarifarioId, DateTime dFecInicio, DateTime dFecFin){
            return oReportesGerencialesConsultasDA.ObtenerTarifaConsularPaís(tari_sTarifarioId, dFecInicio, dFecFin);
        }

        public DataSet ObtenerRecordActuacion(DateTime dFecInicio, DateTime dFecFin)
        {
            return oReportesGerencialesConsultasDA.ObtenerRecordActuacion(dFecInicio, dFecFin);
        }

        public DataSet ObtenerMayorVentaPais(DateTime dFecInicio, DateTime dFecFin)
        {
            return oReportesGerencialesConsultasDA.ObtenerMayorVentaPais(dFecInicio, dFecFin);
        }

        public DataSet ObtenerMayorVentaContinente(DateTime dFecInicio,DateTime dFecFin) {
            return oReportesGerencialesConsultasDA.ObtenerMayorVentaContinente(dFecInicio, dFecFin);
        }

        public DataSet ObtenerCategoriaVentas(DateTime dFecInicio, DateTime dFecFin)
        {
            return oReportesGerencialesConsultasDA.ObtenerCategoriaVentas(dFecInicio, dFecFin);
        }

        public DataSet ObtenerRecordVenta(Int16? sUsuarioCreacion, Int16? sOficinaConsularId, DateTime fechainicio, DateTime fechafin)
        {
            return oReportesGerencialesConsultasDA.ObtenerRecordVenta(sUsuarioCreacion, sOficinaConsularId, fechainicio, fechafin);
        }

        public DataSet ObtenerRGExCategoria(DateTime fechainicio, DateTime fechafin)
        {
            return oReportesGerencialesConsultasDA.ObtenerRGExCategoria(fechainicio, fechafin);
        }

        public DataSet ObtenerConsolidado(string strContinente, string strPais, Int16? idcategoriaoficinaconsular,
                                          Int16? idoficinaconsular, string idtarifaconsular, Int16? idtipopago,
                                          DateTime datFechainicio, DateTime datFechafin,
            Int16? intUsuarioId)
        {
            return oReportesGerencialesConsultasDA.ObtenerConsolidado(strContinente, strPais, idcategoriaoficinaconsular, idoficinaconsular, idtarifaconsular, idtipopago, datFechainicio, datFechafin,
                intUsuarioId);
        }

        public DataSet USP_REPORTE_AUTOADHESIVOS_USUARIO_OFICINACONSULAR(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, Int16 sEstadoInsumo)
        {
            try
            {

                return oReportesGerencialesConsultasDA.USP_REPORTE_AUTOADHESIVOS_USUARIO_OFICINACONSULAR(iOficinaConsularId, dFechaInicio, dFechaFin, intUsuarioId, sEstadoInsumo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //----------------------------------------------------
        //Fecha: 07/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Asignar el parametro opcional bTramiteSinVincular
        //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
        //----------------------------------------------------
        public DataSet USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, int intTipoPagoId, int intTipoRolId, int intTarifaId = 0, bool bTramiteSinVincular = false)
        {
            try
            {

                return oReportesGerencialesConsultasDA.REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR(iOficinaConsularId, dFechaInicio, dFechaFin, intUsuarioId, intTipoPagoId, intTipoRolId, intTarifaId, bTramiteSinVincular);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }


        //------------------------------------------------
        //Fecha: 29/12/2016
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE ACTUACIONES PARA TARIFA 20B
        //------------------------------------------------
        //----------------------------------------------------
        //Fecha: 07/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Asignar el parametro opcional bTramiteSinVincular
        //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
        //----------------------------------------------------
        public DataSet USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR_PARATARIFA20B(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, int intTipoPagoId, int intTipoRolId, int intTarifaId = 0, int intClasificacionId = 0, bool bTramiteSinVincular = false)
        {
            try
            {

                return oReportesGerencialesConsultasDA.REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR_PARATARIFA20B(iOficinaConsularId, dFechaInicio, dFechaFin, intUsuarioId, intTipoPagoId, intTipoRolId, intTarifaId, intClasificacionId, bTramiteSinVincular);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //------------------------------------------------
        //Fecha: 29/12/2016
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE PERSONAS
        //------------------------------------------------
        public DataSet USP_REPORTE_PERSONAS_USUARIO_OFICINACONSULAR(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, 
            Int16 EstadoCivil = 0,
            Int16 GeneroId = 0,
            string CodigoPostal = "",
            Int16 OcupacionId = 0,
            Int16 ProfesionId = 0,
            Int16 GradoInstruccionID = 0,
            bool BuscarResidencia = false,
            Int16 TipoResidencia = 0,
            string ResidenciaUbigeo = "",
            Int16 Nacionalidad = 0)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_PERSONAS_USUARIO_OFICINACONSULAR(iOficinaConsularId, dFechaInicio, dFechaFin, 
                    EstadoCivil, GeneroId, CodigoPostal, OcupacionId, ProfesionId, GradoInstruccionID, BuscarResidencia, TipoResidencia, ResidenciaUbigeo, Nacionalidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //------------------------------------------------
        //Fecha: 02/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CANTIDAD DE ACTUACIONES POR CONSULADO
        //------------------------------------------------
        public DataSet ObtenerCantidadActuacionesPorConsulado(string strContinente, string strPais, Int16? idcategoriaoficinaconsular,
                                          Int16? idoficinaconsular, string idtarifaconsular, Int16? idtipopago,
                                          DateTime datFechainicio, DateTime datFechafin,
            Int16? intUsuarioId)
        {
            return oReportesGerencialesConsultasDA.ObtenerCantidadActuacionesPorConsulado(strContinente, strPais, idcategoriaoficinaconsular, idoficinaconsular, idtarifaconsular, idtipopago, datFechainicio, datFechafin,
                intUsuarioId);
        }
        //------------------------------------------------
        //Fecha: 02/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CANTIDAD DE TARIFAS
        //------------------------------------------------
        public DataSet ObtenerCantidadMaximaTarifas(string strContinente, string strPais, Int16? idcategoriaoficinaconsular,
                                          Int16? idoficinaconsular, string idtarifaconsular, Int16? idtipopago,
                                          DateTime datFechainicio, DateTime datFechafin,
            Int16? intUsuarioId)
        {
            return oReportesGerencialesConsultasDA.ObtenerCantidadMaximaTarifas(strContinente, strPais, idcategoriaoficinaconsular, idoficinaconsular, idtarifaconsular, idtipopago, datFechainicio, datFechafin,
                intUsuarioId);
        }

        //------------------------------------------------
        //Fecha: 11/04/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE RANKING DE RECAUDACIÓN Y ACTUACIONES CONSULARES 
        //------------------------------------------------
        public DataSet ObtenerRankingRecaudacion(Int16? idoficinaconsular,
                                          DateTime datFechainicio, DateTime datFechafin)
        {
            return oReportesGerencialesConsultasDA.ObtenerRankingRecaudacion(idoficinaconsular, datFechainicio, datFechafin);
        }

        //------------------------------------------------
        //Fecha: 12/07/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE RANKING DE CAPTACIÓN CONSULAR
        //------------------------------------------------
        public DataSet ObtenerRankingCaptacion(Int16? idoficinaconsular,
                                          Int16 Anio, string Ordenado, string pagoLima)
        {
            return oReportesGerencialesConsultasDA.ObtenerRankingCaptacion(idoficinaconsular, Anio, Ordenado, pagoLima);
        }

        //------------------------------------------------
        //Fecha: 12/07/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CUADRO DE SALDOS DE AUTOAHDESIVOS
        //------------------------------------------------
        public DataSet ObtenerCuadroAutoahdesivos(Int16? idoficinaconsular,
                                          Int16 Anio, string Ordenado)
        {
            return oReportesGerencialesConsultasDA.ObtenerCuadroAutoahdesivos(idoficinaconsular, Anio, Ordenado);
        }

        //------------------------------------------------
        //Fecha: 18/08/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CUADRO DE SALDOS DE AUTOAHDESIVOS UTILIZADOS
        //------------------------------------------------
        public DataSet ObtenerCuadroAutoahdesivosUtilizados(Int16? idoficinaconsular,
                                          Int16 Anio, string Ordenado)
        {
            return oReportesGerencialesConsultasDA.ObtenerCuadroAutoahdesivosUtilizados(idoficinaconsular, Anio, Ordenado);
        }

        //------------------------------------------------
        //Fecha: 11/10/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE ACTUACIONES_PERIODO_USUARIO_OFICINACONSULARS
        //------------------------------------------------
        public DataSet USP_REPORTE_ACTUACIONES_PERIODO_USUARIO_OFICINACONSULAR(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, int intTipoPagoId, int intTarifaId = 0)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_ACTUACIONES_PERIODO_USUARIO_OFICINACONSULAR(iOficinaConsularId, dFechaInicio, dFechaFin, intUsuarioId, intTipoPagoId, intTarifaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //------------------------------------------------
        //Fecha: 11/10/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CONSOLIDADO DE ACTUACIONES POR USUARIO
        //------------------------------------------------
        public DataSet USP_REPORTE_CONSOLIDADO_ACTUACIONES_USUARIO_OFICINACONSULAR(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_CONSOLIDADO_ACTUACIONES_USUARIO_OFICINACONSULAR(iOficinaConsularId, dFechaInicio, dFechaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }

        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE CORRELATIVOS
        //------------------------------------------------
        public DataSet USP_REPORTE_CORRELATIVOS_TARIFA(int iOficinaConsularId, Int16 Anio)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_CORRELATIVOS_TARIFA(iOficinaConsularId, Anio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }

        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE RECAUDACION MENSUAL
        //------------------------------------------------
        public DataSet USP_REPORTE_RECAUDACION_MENSUAL(int iOficinaConsularId, Int16 Anio)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_RECAUDACION_MENSUAL(iOficinaConsularId, Anio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        
        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE RECAUDACION POR TARIFAS
        //------------------------------------------------
        public DataSet USP_REPORTE_RECAUDACION_TARIFA(int iOficinaConsularId, Int16 Anio)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_RECAUDACION_TARIFA(iOficinaConsularId, Anio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CARGA INICIAL CORRELATIVO
        //------------------------------------------------
        public DataSet USP_REPORTE_CARGA_INICIAL_CORRELATIVO(int iOficinaConsularId)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_CARGA_INICIAL_CORRELATIVO(iOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //################################################
        //Fecha: 24/11/2020
        //Autor: Vidal Pipa
        //Objetivo: REPORTE Actuaciones mensuales
        //################################################
        public DataSet USP_REPORTE_Actuaciones_Mensuales(int anioDesde, int mesDesde, int anioHasta, int mesHasta, string tipoReporte)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_Actuaciones_Mensuales(anioDesde, mesDesde, anioHasta, mesHasta, tipoReporte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE RECAUDACION DIARIO
        //------------------------------------------------
        public DataSet USP_REPORTE_RECAUDACION_DIARIO(int iOficinaConsularId, DateTime Fecha)
        {
            try
            {
                return oReportesGerencialesConsultasDA.USP_REPORTE_RECAUDACION_DIARIO(iOficinaConsularId, Fecha);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oReportesGerencialesConsultasDA = null;
            }
        }
        //------------------------------------------------
        //Fecha: 24/10/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE ITINERANTES
        //------------------------------------------------
        public DataSet ReporteItinerantes(Int16? idoficinaconsular,
                                          DateTime datFechainicio, DateTime datFechafin)
        {
            return oReportesGerencialesConsultasDA.ReporteItinerantes(idoficinaconsular, datFechainicio, datFechafin);
        }
    }
}
