using System;
using SGAC.Configuracion.Maestro.DA;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Maestro.BL
{
    public class TablaMaestraMantenimientoBL
    {
        public int Insertar(MA_TABLA_MAESTRA pobjBE)
        {
            TablaMaestraMantenimientoDA objDA = new TablaMaestraMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Insertar(pobjBE);
                    scope.Complete();
                }
                catch (Exception ex)
                {

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.tama_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tama_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });


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

        public int Actualizar(MA_TABLA_MAESTRA pobjBE)
        {
            TablaMaestraMantenimientoDA objDA = new TablaMaestraMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Actualizar(pobjBE);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.tama_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tama_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

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


        public int Eliminar(MA_TABLA_MAESTRA pobjBE)
        {
            TablaMaestraMantenimientoDA objDA = new TablaMaestraMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Eliminar(pobjBE);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.tama_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tama_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

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



        public int EliminarMargenImpresion(long mado_iCorrelativo,
                                           Int16 mado_sUsuarioCreacion)
        {
            TablaMaestraMantenimientoDA objDA = new TablaMaestraMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.EliminarMargenImpresion(mado_iCorrelativo, mado_sUsuarioCreacion);
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

        public int RegistrarMargenImpresion(long mado_iCorrelativo,
                                            Int16 mado_sTipDocImpresion,
                                            Int16 mado_sOficinaConsular,
                                            byte mado_sSeccion,
                                            string mado_vDescripcion,
                                            Int16 mado_sMargenSuperior,
                                            Int16 mado_sMargenIzquierdo,
                                            Int16 mado_sUsuarioCreacion)
            
        {
            TablaMaestraMantenimientoDA objDA = new TablaMaestraMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.RegistrarMargenImpresion(mado_iCorrelativo, mado_sTipDocImpresion,mado_sOficinaConsular,
                                        mado_sSeccion, mado_vDescripcion, mado_sMargenSuperior, mado_sMargenIzquierdo, mado_sUsuarioCreacion);
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