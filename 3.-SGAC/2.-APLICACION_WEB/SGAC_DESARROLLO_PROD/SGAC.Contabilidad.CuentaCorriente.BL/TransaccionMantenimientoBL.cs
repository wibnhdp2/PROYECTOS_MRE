using SGAC.BE.MRE;
using SGAC.Contabilidad.CuentaCorriente.DA;
using System.Transactions;
using System;

namespace SGAC.Contabilidad.CuentaCorriente.BL
{
    //public class TransaccionMantenimientoBL : EntityCrud<BE.CO_TRANSACCION>
    public class TransaccionMantenimientoBL
    {
        public int Insert(CO_TRANSACCION pobjBE)
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Insert(pobjBE);
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

            return intResultado;
        }
        public long InsertNew(CO_TRANSACCION pobjBE)
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            long intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.InsertNew(pobjBE);
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

            return intResultado;
        }
        public int Update(CO_TRANSACCION pobjBE)
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Update(pobjBE);
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

            return intResultado;
        }
        public int UpdateNew(CO_TRANSACCION pobjBE)
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.UpdateNew(pobjBE);
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

            return intResultado;
        }
        public int Delete(CO_TRANSACCION pobjBE)
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Delete(pobjBE);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
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

            return intResultado;
        }

        //--------------------------------------------------------------
        //Fecha: 10/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Quitar los parametros de Tipo de Operación y 
        //          Tipo de Transacción. 
        //          DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //--------------------------------------------------------------

        public int RegistroMasivoTransacciones(string strXMLExcel,
                                   Int16 sOficinaConsular,
                                   Int16 CuentaCorrienteId,
                                   string cPeriodo,
                                   Int16 sUsuarioCreacion,
                                   string vIPCreacion)
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(300) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.RegistroMasivoTransacciones(strXMLExcel,
                                              sOficinaConsular,
                                              CuentaCorrienteId,                                              
                                              cPeriodo,
                                              sUsuarioCreacion,
                                              vIPCreacion);
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

            return intResultado;
        }

        public int ActualizarDatosConciliacion(Int16 sOficinaConsular,
                                   Int64 intTransaccion,
                                   DateTime dFechaConciliacion,
                                   Double NuevoMonto
                                   )
        {
            TransaccionMantenimientoDA objDA = new TransaccionMantenimientoDA();
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(300) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.ActualizarDatosConciliacion(sOficinaConsular,
                                              intTransaccion,
                                              dFechaConciliacion,
                                              NuevoMonto);
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

            return intResultado;
        }
    }
}
