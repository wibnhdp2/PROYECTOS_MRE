using System;
using System.Data;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class FormularioConsultasBL
    {
        private FormularioConsultasDA objDA;

        public DataTable ObtenerPorAplicacion(int intPaginaActual,
                                              int intPaginaCantidad,
                                              ref int intTotalRegistros,
                                              ref int intTotalPaginas,
                                              int intAplicacionId,
                                              string strDescripcion,
                                              string cEstado)
        {
            try
            {
                objDA = new FormularioConsultasDA();
                return objDA.ObtenerPorAplicacion(intPaginaActual,
                                                  intPaginaCantidad,
                                                  ref intTotalRegistros,
                                                  ref intTotalPaginas,
                                                  intAplicacionId,
                                                  strDescripcion,
                                                  cEstado);
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

        public DataTable ListarPorAplicacion(int intAplicacionid, int intVisibleId)
        {
            try
            {
                objDA = new FormularioConsultasDA();
                return objDA.ListarPorAplicacion(intAplicacionid, intVisibleId);
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