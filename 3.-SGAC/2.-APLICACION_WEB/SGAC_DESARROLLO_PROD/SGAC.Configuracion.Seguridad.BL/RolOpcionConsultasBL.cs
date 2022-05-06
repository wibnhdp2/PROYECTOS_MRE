using System;
using System.Data;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class RolOpcionConsultasBL
    {
        private RolOpcionConsultasDA objDA;

        public DataTable ObtenerPorUsuario(int iUsuarioId, int iAplicacionId)
        {
            try
            {
                objDA = new RolOpcionConsultasDA();
                return objDA.ObtenerPorUsuario(iUsuarioId, iAplicacionId);
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

        public DataTable ObtenerPorRolConfiguracion(int intPaginaActual, 
                                                    int intPaginaCantidad, 
                                                    ref int intTotalRegistros, 
                                                    ref int intTotalPaginas, 
                                                    int iRolConfiguracionId,
                                                    string strEstado)
        {
            try
            {
                objDA = new RolOpcionConsultasDA();
                return objDA.ObtenerPorRolConfiguracion(intPaginaActual,
                                                        intPaginaCantidad,
                                                        ref intTotalRegistros,
                                                        ref intTotalPaginas,
                                                        iRolConfiguracionId,
                                                        strEstado);
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

        public DataTable ObtenerOpcionesFormulario(int intAplicacionId)
        {
            try
            {
                objDA = new RolOpcionConsultasDA();
                return objDA.ObtenerOpcionesFormulario(intAplicacionId);
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
