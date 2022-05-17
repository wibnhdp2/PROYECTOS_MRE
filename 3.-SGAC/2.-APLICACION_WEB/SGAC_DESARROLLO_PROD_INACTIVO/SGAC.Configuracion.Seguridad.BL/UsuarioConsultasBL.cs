using System;
using System.Data;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class UsuarioConsultasBL
    {
        private UsuarioConsultasDA objDA;

        public DataTable Obtener()
        {
            try
            {
                objDA = new UsuarioConsultasDA();
                return objDA.Obtener();
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

        public DataTable ObtenerTodo()
        {
            try
            {
                objDA = new UsuarioConsultasDA();
                return objDA.ObtenerTodo();
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

        public DataTable ObtenerPorId(int iUsuarioId)
        {
            try
            {
                objDA = new UsuarioConsultasDA();
                return objDA.ObtenerPorId(iUsuarioId);
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

        public DataTable ObtenerLista(int intOficinaConsularId)
        {
            try
            {
                objDA = new UsuarioConsultasDA();
                return objDA.ObtenerLista(intOficinaConsularId);
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

        public DataTable ObtenerPorFiltros(int intPaginaActual,
                                            int intPaginaCantidad,
                                            ref int intTotalRegistros,
                                            ref int intTotalPaginas,
                                            int intOficinaConsularId,
                                            string strDocumentoNumero,
                                            string strNombres, 
                                            string strApellidoPaterno, 
                                            string strApellidoMaterno,
                                            string strEstado)
        {
            DataTable dt = new DataTable();
            try
            {
                objDA = new UsuarioConsultasDA();
                dt = objDA.ObtenerPorFiltros(intPaginaActual,
                                               intPaginaCantidad,
                                               ref intTotalRegistros,
                                               ref intTotalPaginas,
                                               intOficinaConsularId,
                                               strDocumentoNumero,
                                               strNombres,
                                               strApellidoPaterno,
                                               strApellidoMaterno,
                                               strEstado);
            }
            catch (Exception ex)
            {
                // Auditoría
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
            return dt;
        }

        public DataTable Autenticar(int intAplicacionId, string strAlias, string strHostName, string strDireccionIP)
        {
            try
            {
                objDA = new UsuarioConsultasDA();
                
                return objDA.Autenticar(intAplicacionId, strAlias, strHostName, strDireccionIP);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataSet Autorizar(int intAplicacionId, long intUsuarioId)
        {
            try
            {
                objDA = new UsuarioConsultasDA();
                return objDA.Autorizar(intAplicacionId, intUsuarioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }

        public string ValidarUsuarioDocumento(string StrNroDocumento, string StrAlias, int IntOperacion)
        {
            objDA = new UsuarioConsultasDA();

            try
            {
                return objDA.ValidarUsuarioDocumento(StrNroDocumento, StrAlias, IntOperacion);
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
