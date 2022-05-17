using System;
using System.Collections.Generic;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{
    public class ServicioConsultaBL
    {
        public List<SGAC.BE.CL_SERVICIO> ListarServiciosConsulado(int iOficinaconsularId)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.ListarServiciosConsulado(iOficinaconsularId);
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

        public DataTable GetAll(int iOficinaconsularId, string servServicioId)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.GetAll(iOficinaconsularId, servServicioId);
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

        public DataTable Consultar(int iOficinaconsularId,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.Consultar(iOficinaconsularId, intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);
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

        public DataTable ConsultarDetalle(int iOficinaconsularId,
                                          int intPaginaActual,
                                          int intPaginaCantidad,
                                          int intservIServicioId,
                                          int intTotalRegistros,
                                          int intTotalPaginas)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.ConsultarDetalle(iOficinaconsularId, 
                                             intPaginaActual, 
                                             intPaginaCantidad, 
                                             intservIServicioId, 
                                             ref intTotalRegistros, 
                                             ref intTotalPaginas);
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

        public DataTable ConsultarElegir(int iOficinaconsularId,
                                         int intPaginaActual,
                                         int intPaginaCantidad,
                                         ref int intTotalRegistros,
                                         ref int intTotalPaginas)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();
            try
            {
                return xFun.ConsultarElegir(iOficinaconsularId, intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);
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

        public DataTable LlenarCabeceraTreeView(int iOficinaconsularId, int vedeIVentanillaId)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();
            try
            {
                return xFun.LlenarCabeceraTreeView(iOficinaconsularId, vedeIVentanillaId);
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

        public DataTable LlenarDetalleTreeView(int iOficinaconsularId, int servIServicioId, int vedeIVentanillaId)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();
            try
            {
                return xFun.LlenarDetalleTreeView(iOficinaconsularId, servIServicioId, vedeIVentanillaId);
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

        public DataTable ConsultarElegirServicios(int iOficinaconsularId,
                                                  int intPaginaActual,
                                                  int intPaginaCantidad,
                                                  ref int intTotalRegistros,
                                                  ref int intTotalPaginas)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.ConsultarElegirServicios(iOficinaconsularId, 
                                                     intPaginaActual, 
                                                     intPaginaCantidad, 
                                                     ref intTotalRegistros, 
                                                     ref intTotalPaginas);
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
        public int ConsultarcodigoServicio(int ventIVentanillaId)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.ObtenerCodigoServicio(ventIVentanillaId);
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
        public int ConsultarCoddigoServicio(int ventIVentanillaId)
        {
            ServicioConsultaDA xFun = new ServicioConsultaDA();

            try
            {
                return xFun.ConsultarCodigoServicio(ventIVentanillaId);

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
