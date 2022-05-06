using System;
using System.Data;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class UsuarioRolConsultasBL 
    {
        private UsuarioRolConsultasDA objDA;

        public DataTable Obtener()
        {
            try
            {
                objDA = new UsuarioRolConsultasDA();
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

        public DataTable ObtenerPorEmpresa(int iEmpresaId)
        {
            try
            {
                objDA = new UsuarioRolConsultasDA();
                return objDA.ObtenerPorEmpresa(iEmpresaId);
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

        public DataSet ObtenerConfiguracion(string strAlias)
        {
            try
            {
                objDA = new UsuarioRolConsultasDA();
                return objDA.ObtenerConfiguracion(strAlias);
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

        public int VerificaIntegridadReferencial(int IntRolId)
        {
            try
            {
                UsuarioRolConsultasDA DA = new UsuarioRolConsultasDA();
                return DA.VerificaIntegridadReferencial(IntRolId);
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

        public DataTable Validar(string StrAlias, int IntOficinaConsular)
        {
            try
            {
                objDA = new UsuarioRolConsultasDA();
                return objDA.Validar(StrAlias, IntOficinaConsular);
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
