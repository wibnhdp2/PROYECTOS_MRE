using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using SGAC.Configuracion.Sistema.DA;
using SGAC.BE.MRE;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class OficinaConsularFuncMantenimientoBL
    {
        public void Insert(RE_OFICINACONSULARFUNCIONARIO pobjBE, ref int IntFuncionarioOficConsulId, ref bool Error)
        {
            DA.OficinaConsularFuncMantenimientoDA objDA = new DA.OficinaConsularFuncMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Insert(pobjBE, ref IntFuncionarioOficConsulId, ref Error);
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

        public void Update(RE_OFICINACONSULARFUNCIONARIO pobjBE, ref int IntFuncionarioOficConsulId, ref bool Error)
        {
            DA.OficinaConsularFuncMantenimientoDA objDA = new DA.OficinaConsularFuncMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref IntFuncionarioOficConsulId, ref Error);
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

        public void Eliminar(RE_OFICINACONSULARFUNCIONARIO pobjBE, ref bool Error)
        {
            DA.OficinaConsularFuncMantenimientoDA objDA = new DA.OficinaConsularFuncMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

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

        public void ActualizarCargo(Int16 ocfu_sOficinaConsularId, Int16 ocfu_sUsuarioModificacion, string ocfu_vIPModificacion, Int16 ocfu_sCargoFuncionarioId, Int16 ocfu_sGenero, ref bool Error)
        {
            DA.OficinaConsularFuncMantenimientoDA objDA = new DA.OficinaConsularFuncMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.ActualizarCargo(ocfu_sOficinaConsularId, ocfu_sUsuarioModificacion, ocfu_vIPModificacion, ocfu_sCargoFuncionarioId,ocfu_sGenero, ref Error);
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
