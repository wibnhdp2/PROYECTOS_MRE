using System;
using System.Data;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class RolConfigConsultasBL
    {
        private RolConfigConsultasDA objDA;

        public DataTable Consultar(int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas,
                                   int intAplicacionId,
                                   string strNombre,
                                   string strEstado,
                                   string strAuditoria)
        {
            try
            {
                objDA = new RolConfigConsultasDA();
                return objDA.ObtenerPorFiltro(intPaginaActual,
                                              intPaginaCantidad,
                                              ref intTotalRegistros,
                                              ref intTotalPaginas,
                                              intAplicacionId,
                                              strNombre,
                                              strEstado,
                                              strAuditoria);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable Listar(int intAplicacionId)
        {
            try
            {
                objDA = new RolConfigConsultasDA();
                return objDA.Listar(intAplicacionId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        // REVISION
        public DataTable ObtenerPorUsuario(int iUsuarioId)
        {
            try
            {
                objDA = new RolConfigConsultasDA();
                return objDA.ObtenerPorUsuario(iUsuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }
    }
}