using System;
using System.Collections.Generic;
using SGAC.Almacen.DA;
using System.Transactions;
using SGAC.Accesorios;
using System.Data;
using SGAC.BE.MRE;

namespace SGAC.Almacen.BL
{
    public class MovimientoMantenimientoBL
    {
        private MovimientoMantenimientoDA objDA;

        public int MovimientoAdicionar(BE.AL_MOVIMIENTO objBE, List<BE.AL_MOVIMIENTODETALLE> Lista)
        {
            objDA = new MovimientoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(120) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            Nullable<Int16> iInsumoEstadoId = null;


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.MovimientoAdicionar(ref objBE, ref iInsumoEstadoId);                    

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK) || (objBE.movi_iMovimientoId > 0))
                    {
                        bool Error = false;

                        foreach (BE.AL_MOVIMIENTODETALLE objBEDetalle in Lista)
                        {
                            objDA.MovimientoDetalleAdicionar(objBEDetalle, objBE, iInsumoEstadoId, ref Error);                           
                        }


                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                    else
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
                    intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.movi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }


        public int MovimientoAdicionarYActualizarEstado(BE.AL_MOVIMIENTO objBE, List<BE.AL_MOVIMIENTODETALLE> Lista,BE.AL_MOVIMIENTO objBE_Estado, List<BE.AL_MOVIMIENTODETALLE> ListaActualizaEstado)
        {
            objDA = new MovimientoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(120) };
            int intResult = Convert.ToInt32(Enumerador.enmResultadoQuery.ERR);
            Nullable<Int16> iInsumoEstadoId = null;


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.MovimientoActualizar(ref objBE_Estado, ref iInsumoEstadoId);

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                    {
                        bool Error = false;
                        intResult = Convert.ToInt32(Enumerador.enmResultadoQuery.ERR);

                        foreach (BE.AL_MOVIMIENTODETALLE objBEDetalle in ListaActualizaEstado)
                        {
                            objDA.MovimientoDetalleActualizar(objBEDetalle, objBE_Estado, iInsumoEstadoId, ref Error);
                            ValidacionError(objDA.strError, objBEDetalle.OficinaConsularId, objBEDetalle.mode_sUsuarioCreacion);
                        }

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                    else
                    {
                        lCancel = true;
                        intResult = Convert.ToInt32(Enumerador.enmResultadoQuery.ERR);
                    }

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                    {
                        intResult = objDA.MovimientoAdicionar(ref objBE, ref iInsumoEstadoId);
                    }
                    
                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK) || (objBE.movi_iMovimientoId > 0))
                    {
                        bool Error = false;

                        foreach (BE.AL_MOVIMIENTODETALLE objBEDetalle in Lista)
                        {
                            objDA.MovimientoDetalleAdicionar(objBEDetalle, objBE, iInsumoEstadoId, ref Error);
                        }


                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                    else
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
                    intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.movi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }



        public int MovimientoActualizar(BE.AL_MOVIMIENTO objBE, List<BE.AL_MOVIMIENTODETALLE> Lista)
        {

            objDA = new MovimientoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(120) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            Nullable<Int16> iInsumoEstadoId = null;


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.MovimientoActualizar(ref objBE, ref iInsumoEstadoId);

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                    {
                        bool Error = false;

                        foreach (BE.AL_MOVIMIENTODETALLE objBEDetalle in Lista)
                        {
                            objDA.MovimientoDetalleActualizar(objBEDetalle, objBE, iInsumoEstadoId, ref Error);
                            ValidacionError(objDA.strError, objBEDetalle.OficinaConsularId, objBEDetalle.mode_sUsuarioCreacion);
                        }


                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                    else
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

                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.movi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }

        public int MovimientoEliminar(BE.AL_MOVIMIENTO objBE)
        {

            objDA = new MovimientoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.MovimientoEliminar(objBE);

                    if (intResult != Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
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
                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.movi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }           
        }

        public void ValidacionError(string mensaje, Int16 sOficinaConsular, Int16 sUsuario)
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
    }
}