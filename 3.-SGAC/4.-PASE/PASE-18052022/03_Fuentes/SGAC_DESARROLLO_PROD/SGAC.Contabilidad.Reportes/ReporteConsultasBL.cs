using SGAC.Contabilidad.Reportes.DA;
using System.Data;
using System;
using SGAC.Accesorios;
using System.Collections.Generic;

namespace SGAC.Contabilidad.Reportes.BL
{
    public class ReporteConsultasBL
    {
        private ReporteConsultasDA objDA;

   
        public DataSet ObtenerReporteRegistroGeneralEntradas(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteRegistroGeneralEntradas(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteTitulares(Int16 iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            Int16 intEstadoCivilId = 0, Int16 intGeneroId = 0, string strCodigoPostal = "", Int16 intOcupacionId = 0,
            Int16 intProfesionId = 0, Int16 intGradoInstruccionId = 0, Int16 intResidenciaTipoId = 0, string strResidenciaUbigeo = "",
            Int16 intNacionalidadId = 0)                        
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteTitulares(iOficinaConsularId, dFechaInicio, dFechaFin, intEstadoCivilId, intGeneroId,
                     strCodigoPostal, intOcupacionId, intProfesionId, intGradoInstruccionId, intResidenciaTipoId, strResidenciaUbigeo, intNacionalidadId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }


        public DataSet ObtenerReporteRegistroGeneralEntradasTimeOut(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteRegistroGeneralEntradasTimeOut(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ObtenerDTReporteRegistroGeneralEntradasTimeOut(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerDTReporteRegistroGeneralEntradasTimeOut(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public List<csReporteRGE> ObtenerReporteRegistroGeneralEntradasTimeOut_Reader(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteRegistroGeneralEntradasTimeOut_Reader(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
        public List<csRGE> ObtenerReporteRGE_Reader(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteRGE_Reader(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        
        public DataSet ObtenerReporteActuacionesAnuladas(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            int intUsuarioId, string strDireccionIP, string strClaseFecha, Int16 intUsuarioElimina = 0)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteActuacionesAnuladas(iOficinaConsularId, dFechaInicio, dFechaFin,
                                        intUsuarioId, strDireccionIP, strClaseFecha, intUsuarioElimina);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 21/11/2016
        // Objetivo: Reporte de Rendición de Cuentas por tarifa
        //------------------------------------------------------------------------

        public DataSet ObtenerReporteRendicionCuenta(long intFichaRegistralId, string strNumeroFicha, Int16 intEstadoId,
                                    string strAnioMes, string strNumeroGuia, Int16 intOficinaConsularId, Int16 OrdenadoPor)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteRendicionCuenta(intFichaRegistralId, strNumeroFicha, intEstadoId,
                                        strAnioMes, strNumeroGuia, intOficinaConsularId, OrdenadoPor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 29/12/2016
        // Objetivo: Reporte de Guía de Despacho
        //------------------------------------------------------------------------

        public DataSet ObtenerReporteGuiaDespacho(Int16 intOficinaConsularId, string strNumeroGuia, string strAnioMes)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteGuiaDespacho(intOficinaConsularId, strNumeroGuia, strAnioMes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteFormatoEnvio(Int16 intOficinaConsularId, string strNumeroGuia, string strAnioMes,
                                                  Int16 intTarifarioId, Int16 intEstadoId)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteFormatoEnvio(intOficinaConsularId, strNumeroGuia, strAnioMes, intTarifarioId, intEstadoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
        public DataSet ObtenerReporteFormatoEnvioRecuparados(Int16 intOficinaConsularId, string strNumeroGuia, string strAnioMes,
                                                  Int16 intTarifarioId, Int16 intEstadoId)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteFormatoEnvioRecuparados(intOficinaConsularId, strNumeroGuia, strAnioMes, intTarifarioId, intEstadoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteSaldosConsulares(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo, int CuentaCorrienteId)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteSaldosConsulares(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo,CuentaCorrienteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteLibroAutoadhesivo(Int16 intOficinaConsularId, Int16 intPeriodo, Int16 intMes, ref Int16 intIDResultado, ref string strMensaje)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteLibroAutoadhesivo(intOficinaConsularId, intPeriodo, intMes, ref intIDResultado, ref strMensaje);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
                

        public DataSet ObtenerReporteLibroCaja(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteLibroCaja(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteDocumentoUnico(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteDocumentoUnico(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        // Otros
        public DataTable ObtenerRemesasConsulares(int intOficinaConsularOrigenId, int intOficinaConsularDestinoId,
            int intTipoid, int intEstadoId,
            DateTime datFechaInicio, DateTime datFechaFin, int iUsuario, string vIp)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerRemesasConsulares(intOficinaConsularOrigenId, intOficinaConsularDestinoId,
                    intTipoid, intEstadoId, datFechaInicio, datFechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ObtenerEstadosBancarios(int intOficinaConsularid,
            int intBancoId, string strNumCuentaCorriente,
            DateTime datFechaInicio, DateTime datFechaFin,Int16 PeriodoAnio, Int16 PeriodoMes, string strTipoBusqueda, Int16 intCodCuentaCorriente)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerEstadosBancarios(intOficinaConsularid, intBancoId,
                    strNumCuentaCorriente, datFechaInicio, datFechaFin, PeriodoAnio, PeriodoMes, strTipoBusqueda, intCodCuentaCorriente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }


        public DataSet ObtenerTransaccionResumen(short sCuentaCorrienteId, DateTime datFechaInicio, DateTime datFechaFin,
                                                    Int16 intPeriodoAnio, Int16 intPeriodoMes, string cBusqueda)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerTransaccionResumen(sCuentaCorrienteId, datFechaInicio, datFechaFin,intPeriodoAnio,intPeriodoMes,cBusqueda);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteConciliacion(Int16 intOficinaConsularId, Int16 intBancoId, Int16 intCuentaCorrienteId, DateTime datFechaInicio, DateTime datFechaFin)
        {
            ReporteConsultasDA objDA = new ReporteConsultasDA();
            return objDA.ObtenerReporteConciliacion(intOficinaConsularId, intBancoId, intCuentaCorrienteId, datFechaInicio, datFechaFin);
        }

        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 13/06/2017
        // Objetivo: Reporte Tramites Incompletos
        //------------------------------------------------------------------------

        public DataSet ObtenerReporteTramitesIncompletos(string strAnioMes, Int16 intOficinaConsularId)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteTramitesIncompletos(strAnioMes,  intOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 13/06/2017
        // Objetivo: Reporte Conciliación reniec
        //------------------------------------------------------------------------

        public DataSet ObtenerReporteConciliacionReniec(string strAnioMes)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteConciliacionReniec(strAnioMes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
        public DataSet ObtenerReporteSaldosConsularesTimeuot(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo, int CuentaCorrienteId)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteSaldosConsularesTimeuot(iOficinaConsularId, dFechaInicio, dFechaFin,
                    strHostName, intUsuarioId, strDireccionIP, intOficinaConsularIdLogeo, CuentaCorrienteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 13/06/2017
        // Objetivo: Reporte Tramites Incompletos
        //------------------------------------------------------------------------

        public DataSet ObtenerReporteListadoRegCivil(DateTime dFechaInicio, DateTime dFechaFin, int intOficinaConsularId)
        {
            try
            {
                objDA = new ReporteConsultasDA();
                return objDA.ObtenerReporteListadoRegCivil(dFechaInicio, dFechaFin, intOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
    }
}
