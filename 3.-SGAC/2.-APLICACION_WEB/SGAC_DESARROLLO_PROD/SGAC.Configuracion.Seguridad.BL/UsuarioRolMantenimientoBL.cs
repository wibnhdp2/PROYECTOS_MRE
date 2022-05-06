using System;
using SGAC.BE.MRE;
using System.Transactions;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class UsuarioRolMantenimientoBL
    {
        public void Insert(BE.MRE.SE_USUARIOROL pobjBE, ref int IntUsuarioRolId, ref bool Error)
        {
            UsuarioRolMantenimientoDA objDA = new UsuarioRolMantenimientoDA();           

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Insert(pobjBE, ref IntUsuarioRolId, ref Error);
                    scope.Complete();
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

        public void Update(BE.MRE.SE_USUARIOROL pobjBE, ref int IntUsuarioRolId, ref bool Error)
        {
            UsuarioRolMantenimientoDA objDA = new UsuarioRolMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref Error);
                    scope.Complete();
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

        public void Delete(BE.MRE.SE_USUARIOROL pobjBE, ref bool Error)
        {
            UsuarioRolMantenimientoDA objDA = new UsuarioRolMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Delete(pobjBE, ref Error);
                    scope.Complete();
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
}
