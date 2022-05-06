using System;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{
    public class VentanillaConsultaBL
    {
        public DataTable Consultar(int iVentanillaId, int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.Consultar(iVentanillaId, intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }

        }

        public DataTable ImprimeAgenciaRemota(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeAgenciaRemota(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeAfluenciaClientes(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeAfluenciaClientes(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeAfluenciaVentanilla(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeAfluenciaVentanilla(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeAtencionxVentanilla(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeAtencionxVentanilla(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeAtencionesxUsusario(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeAtencionesxUsusario(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeAtencionesxTipoAtencion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeAtencionesxTipoAtencion(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }


        public DataTable ImprimerRendimientoProcesos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimerRendimientoProcesos(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }


        public DataTable ImprimeTiempoEsperaxCliente(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeTiempoEsperaxCliente(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }


        public DataTable ImprimeProductividadxOperador(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeProductividadxOperador(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeTiempoAtencionxTransaccion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeTiempoAtencionxTransaccion(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeEvaluacionAtencionxTipoAtencion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeEvaluacionAtencionxTipoAtencion(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeTicketsAtendidosNoAtendidos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeTicketsAtendidosNoAtendidos(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable ImprimeTicketsEmitidos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeTicketsEmitidos(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }


        public DataTable ImprimeSeguimientoControl(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            VentanillaConsultaDA xFun = new VentanillaConsultaDA();

            try
            {
                return xFun.ImprimeSeguimientoControl(idOfinaConsular, fechaInicio, fechaFin, iUsuario, vIp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }




    }
}
