using System;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.BE.MRE;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class TarifarioRequisitoMantenimientoBL
    {
        public void Insert(SI_TARIFARIOREQUISITO pobjBE, ref int IntTarifarioRequisitoId, ref bool Error)
        {
            DA.TarifarioRequisitoMantenimientoDA objDA = new DA.TarifarioRequisitoMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Insert(pobjBE, ref IntTarifarioRequisitoId, ref Error);
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

        public void Update(SI_TARIFARIOREQUISITO pobjBE, ref int IntTarifarioRequisitoId, ref bool Error)
        {
            DA.TarifarioRequisitoMantenimientoDA objDA = new DA.TarifarioRequisitoMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref IntTarifarioRequisitoId, ref Error);
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

        public void Eliminar(SI_TARIFARIOREQUISITO pobjBE, ref bool Error)
        {
            DA.TarifarioRequisitoMantenimientoDA objDA = new DA.TarifarioRequisitoMantenimientoDA();

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
