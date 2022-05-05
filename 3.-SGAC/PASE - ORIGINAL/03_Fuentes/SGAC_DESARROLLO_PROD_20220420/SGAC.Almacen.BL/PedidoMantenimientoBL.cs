using System;
using SGAC.Almacen.DA;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using System.Transactions;
using System.Data;

namespace SGAC.Almacen.BL
{
    public class PedidoMantenimientoBL
    {
        private PedidoMantenimientoDA objDA;

        public int PedidoAdicionar(BE.AL_PEDIDO objBE)
        {
            objDA = new PedidoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.PedidoAdicionar(objBE);

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
                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.pedi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }

          
        }

        public int PedidoActualizar(BE.AL_PEDIDO objBE)
        {
          
            objDA = new PedidoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.PedidoActualizar(objBE);

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


                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.pedi_sUsuarioModificacion.Value);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }

        public int PedidoEliminar(BE.AL_PEDIDO objBE)
        {
            objDA = new PedidoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int32 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.PedidoEliminar(objBE);

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


                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.pedi_sUsuarioModificacion.Value);

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