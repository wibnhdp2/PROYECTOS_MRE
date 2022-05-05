using System;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Contabilidad.RemesaDetalle.DA;

namespace SGAC.Contabilidad.RemesaDetalle.BL
{
    public class RemesaDetalleMantenimientoBL
    {
        public void Insert(CO_REMESADETALLE pobjBE, ref long LonRemesaDetalleId, ref bool Error)
        {
            RemesaDetalleMantenimientoDA objDA = new RemesaDetalleMantenimientoDA();
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    string ErrorMensaje = string.Empty;
                    objDA.Insert(pobjBE, ref LonRemesaDetalleId, ref Error, ref ErrorMensaje);
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

        public void Update(CO_REMESADETALLE pobjBE, ref long LonRemesaDetalleId, ref bool Error)
        {
            RemesaDetalleMantenimientoDA objDA = new RemesaDetalleMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref LonRemesaDetalleId, ref Error);
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

        public void Eliminar(CO_REMESADETALLE pobjBE, ref bool Error)
        {
            RemesaDetalleMantenimientoDA objDA = new RemesaDetalleMantenimientoDA();

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
