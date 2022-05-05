using System;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class StockMinimoMantenimientoBL
    {
        public SI_STOCK_ALMACEN insertar(SI_STOCK_ALMACEN objStock)
        {
            StockMinimoMantenimientoDA objDA = new StockMinimoMantenimientoDA();
            SI_STOCK_ALMACEN IntResultado = new SI_STOCK_ALMACEN();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;

            OficnaConsularId = objStock.stck_sOficinaConsularId;
            UsuarioId = objStock.stck_sUsuarioCreacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Insertar(objStock);

                    if (IntResultado.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = OficnaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = IntResultado.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = UsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        tran.Complete();
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
            return IntResultado;
        }

        public SI_STOCK_ALMACEN actualizar(SI_STOCK_ALMACEN objStock)
        {

            StockMinimoMantenimientoDA objDA = new StockMinimoMantenimientoDA();
            SI_STOCK_ALMACEN IntResultado = new SI_STOCK_ALMACEN();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objStock.stck_sOficinaConsularId;
            UsuarioId = (Int16)objStock.stck_sUsuarioModificacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Actualizar(objStock);

                    if (IntResultado.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = OficnaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = IntResultado.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = UsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        tran.Complete();
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
            return IntResultado;

        }

        public SI_STOCK_ALMACEN eliminar(SI_STOCK_ALMACEN objStock)
        {
            StockMinimoMantenimientoDA objDA = new StockMinimoMantenimientoDA();
            SI_STOCK_ALMACEN IntResultado = new SI_STOCK_ALMACEN();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objStock.stck_sOficinaConsularId;
            UsuarioId = (Int16)objStock.stck_sUsuarioModificacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Eliminar(objStock);

                    if (IntResultado.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = OficnaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = IntResultado.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = UsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        tran.Complete();
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
            return IntResultado;

        }
    }
}
