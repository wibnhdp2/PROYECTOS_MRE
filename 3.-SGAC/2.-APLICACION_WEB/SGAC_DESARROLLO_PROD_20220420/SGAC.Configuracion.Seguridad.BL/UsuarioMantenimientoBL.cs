using System;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Configuracion.Seguridad.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class UsuarioMantenimientoBL
    {
        public void Insert(SE_USUARIO pobjBE, SE_USUARIOROL pobjDetalleBE, ref bool Error)
        {
            UsuarioMantenimientoDA objDA = new UsuarioMantenimientoDA();
            UsuarioRolMantenimientoDA objDADet = new UsuarioRolMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    int IntUsuarioId = 0;
                    objDA.Insert(pobjBE, ref IntUsuarioId, ref Error);

                    if ((Error == false) || (IntUsuarioId != 0))
                    {
                        if (lCancel == false)
                        {
                            if (pobjDetalleBE != null)
                            {
                                pobjDetalleBE.usro_sUsuarioId = (short)IntUsuarioId;
                                int IntUsuarioRolId = 0;
                                objDADet.Insert(pobjDetalleBE, ref IntUsuarioRolId, ref Error);

                                if ((Error == true) || (IntUsuarioRolId == 0))
                                {
                                    lCancel = true;
                                }                                
                            }
                        }
                    }
                    else
                    {
                        lCancel = true;
                    }

                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                        throw new Exception("Rollback: UsuarioMantenimientoBL - Insert");
                    }
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.OficinaConsularId,
                        audi_vComentario = ex.Message,
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.usua_sUsuarioCreacion,
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
        }

        public void Update(SE_USUARIO pobjBE, SE_USUARIOROL pobjDetalleBE, ref bool Error)
        {
            UsuarioMantenimientoDA objDA = new UsuarioMantenimientoDA();
            UsuarioRolMantenimientoDA objDADet = new UsuarioRolMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    int IntUsuarioId = 0;
                    objDA.Update(pobjBE, ref IntUsuarioId, ref Error);

                    if ((Error == false) || (IntUsuarioId != 0))
                    {
                        if (lCancel == false)
                        {
                            if (pobjDetalleBE != null)
                            {                                
                                objDADet.Update(pobjDetalleBE, ref Error);

                                if (Error == true)
                                {
                                    lCancel = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        lCancel = true;
                    }

                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                        throw new Exception("Rollback: UsuarioMantenimientoBL - Update");
                    }
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.OficinaConsularId,
                        audi_vComentario = ex.Message,
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.usua_sUsuarioModificacion,
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
        }

        public void Actualizar_Sesion_Activa(int IntUsuarioId, bool bActivo, string IPAddress)
        {
            UsuarioMantenimientoDA objDA = new UsuarioMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Actualizar_Sesion_Activa(IntUsuarioId, bActivo, IPAddress);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();


                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = 1,
                        audi_vComentario = "Rollback: " + ex.Message,
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)IntUsuarioId,
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
        }

        public void Actualizar_Sesion_Bloqueada(int IntUsuarioId, bool bActivo, string vIP)
        {
            UsuarioMantenimientoDA objDA = new UsuarioMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Actualizar_Sesion_Bloqueada(IntUsuarioId, bActivo, vIP);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = 1,
                        audi_vComentario = "Rollback: " + ex.Message,
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)IntUsuarioId,
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
        }
        public void Delete(SE_USUARIO pobjBE, ref bool Error)
        {
            UsuarioMantenimientoDA objDA = new UsuarioMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Delete(pobjBE, ref Error);

                    if (Error == false)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                        throw new Exception("Rollback: UsuarioMantenimientoBL - Delete");
                    }
                }
                catch (Exception ex)
                {

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.OficinaConsularId,
                        audi_vComentario = ex.Message,
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.usua_sUsuarioModificacion,
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
        }        
    }    
}
