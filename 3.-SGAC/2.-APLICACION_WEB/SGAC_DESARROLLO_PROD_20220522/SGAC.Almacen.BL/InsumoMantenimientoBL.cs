using System;
using SGAC.Almacen.DA;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using System.Transactions;
using System.Data;

namespace SGAC.Almacen.BL
{
    public class InsumoMantenimientoBL
    {
        private InsumoMantenimientoDA objDA;

        public int InsumoAdicionar(BE.AL_INSUMO objBE, int intMovimientoId, int intRangoInicial, int intRangoFinal)
        {
            objDA = new InsumoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.InsumoAdicionar(objBE, intMovimientoId, intRangoInicial, intRangoFinal);

                    if (!(intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK)))
                    {

                        lCancel = true;
                    }
                    
                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {

                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.insu_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }

          
        }

        public int InsumoActualizar(BE.AL_INSUMO objBE)
        {
            objDA = new InsumoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.InsumoActualizar(objBE);

                    if (!(intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK)))
                    {

                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {

                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.insu_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }

        public int InsumoDarDeBaja(BE.AL_INSUMO objBE)
        {
            objDA = new InsumoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.InsumoDarDeBaja(objBE);

                    if (!(intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK)))
                    {

                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {

                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.insu_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }

        public void ValidacionError(string mensaje, Int16 sOficinaConsular, Int16 sUsuario )
        {

            if (!string.IsNullOrEmpty(mensaje))
            {


                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = sOficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = sUsuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

            }
        }


        public int RecalcularSaldosInsumo(Int16 OficinaConsultar, Int16 AnioInicial, Int16 MesInicial, Int16 AnioFinal, Int16 MesFinal, Int16 Usuario,bool SoloReiniciar = false)
        {
            objDA = new InsumoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.RecalcularSaldosInsumo(OficinaConsultar, AnioInicial, MesInicial, AnioFinal, MesFinal, Usuario, SoloReiniciar);

                    if (!(intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK)))
                    {

                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {

                    ValidacionError(objDA.strError, OficinaConsultar, Usuario);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }


        public int ReactivarInsumos(Int16 OficinaConsultar, string iRangoInicial, string iRangoFinal, Int16 Usuario, string Observacion)
        {
            objDA = new InsumoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.ReactivarInsumos(OficinaConsultar, iRangoInicial, iRangoFinal, Usuario, Observacion);

                    if (!(intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK)))
                    {

                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {

                    ValidacionError(objDA.strError, OficinaConsultar, Usuario);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }
    }
}