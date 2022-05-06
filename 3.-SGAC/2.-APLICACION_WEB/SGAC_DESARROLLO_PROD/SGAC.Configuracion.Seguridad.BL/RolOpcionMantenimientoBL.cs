using System;
using System.Transactions;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class RolOpcionMantenimientoBL
    {
        public void Insert(BE.SE_ROLOPCION pobjBE, ref int IntRolOpcionId, ref bool Error)
        {
            RolOpcionMantenimientoDA objDA = new RolOpcionMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Insert(pobjBE, ref IntRolOpcionId, ref Error);
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

        public void Update(BE.SE_ROLOPCION pobjBE, ref bool Error)
        {
            DA.RolOpcionMantenimientoDA objDA = new DA.RolOpcionMantenimientoDA();

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

        public void Delete(BE.SE_ROLOPCION pobjBE, ref bool Error)
        {
            DA.RolOpcionMantenimientoDA objDA = new DA.RolOpcionMantenimientoDA();

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
