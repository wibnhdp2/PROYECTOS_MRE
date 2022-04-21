using System;
using System.Transactions;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.Configuracion.Seguridad.DA;
using System.Collections.Generic;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class RolConfigMantenimientoBL
    {
        public void Insertar(BE.SE_ROLCONFIGURACION pobjBE, List<BE.SE_ROLOPCION> lstDetalle, ref bool Error)
        {
            RolConfigMantenimientoDA objDA = new RolConfigMantenimientoDA();
            RolOpcionMantenimientoDA objDADet = new RolOpcionMantenimientoDA();

            // Por defecto, se le asigna a la Configuración el ROLOPCION del formulario INICIO (~/Default.aspx)
            string strOpciones = Constantes.CONST_SESION_ROLOPCION_INICIO.ToString();
            string strMensajeError = string.Empty;

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    int IntRolConfiguracionId = 0;
                    objDA.Insert(pobjBE, ref IntRolConfiguracionId, ref Error);

                    if ((Error == false) || (IntRolConfiguracionId != 0))
                    {
                        if ((lCancel == false) && (lstDetalle != null))
                        {
                            foreach (BE.SE_ROLOPCION lstObj in lstDetalle)
                            {
                                BE.SE_ROLOPCION objDetalleBE = new BE.SE_ROLOPCION();                                    
                                objDetalleBE.roop_sFormularioId = lstObj.roop_sFormularioId;
                                objDetalleBE.roop_vAcciones = lstObj.roop_vAcciones;
                                objDetalleBE.roop_cEstado = lstObj.roop_cEstado;
                                objDetalleBE.roop_sUsuarioCreacion = lstObj.roop_sUsuarioCreacion;
                                objDetalleBE.roop_vIPCreacion = lstObj.roop_vIPCreacion;
                                objDetalleBE.OficinaConsularId = lstObj.OficinaConsularId;

                                int IntRolOpcionId = 0;
                                objDADet.Insert(objDetalleBE, ref IntRolOpcionId, ref Error);

                                strOpciones += "|" + IntRolOpcionId.ToString();

                                if ((Error == true) || (IntRolOpcionId == 0))
                                {
                                    lCancel = true;
                                }
                            }

                            if (strOpciones.Length > 0)
                            {
                                pobjBE.roco_sRolConfiguracionId = (short)IntRolConfiguracionId;
                                pobjBE.roco_vRolOpcion = strOpciones;
                            }

                            objDA.Update(pobjBE, ref Error);

                            if (Error == true)
                            {
                                lCancel = true;
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

                        throw new Exception("Rollback: RolConfigMantenimientoBL - Insertar");
                    }
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new  SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = pobjBE.roco_sUsuarioCreacion,
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

        public void Actualizar(BE.SE_ROLCONFIGURACION pobjBE, List<BE.SE_ROLOPCION> lstDetalle, ref bool Error)
        {
            RolConfigMantenimientoDA objDA = new RolConfigMantenimientoDA();
            RolOpcionMantenimientoDA objDADet = new RolOpcionMantenimientoDA();

            // Por defecto, se le asigna a la Configuración el ROLOPCION del formulario INICIO (~/Default.aspx)
            string strOpciones = Constantes.CONST_SESION_ROLOPCION_INICIO.ToString();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref Error);

                    if (Error == false)
                    {
                        if ((lCancel == false) && (lstDetalle != null))
                        {
                            if (!Error)
                            {
                                foreach (BE.SE_ROLOPCION lstObj in lstDetalle)
                                {
                                    BE.SE_ROLOPCION objDetalleBE = new BE.SE_ROLOPCION();
                                    objDetalleBE.roop_sFormularioId = lstObj.roop_sFormularioId;
                                    objDetalleBE.roop_vAcciones = lstObj.roop_vAcciones;
                                    objDetalleBE.roop_cEstado = lstObj.roop_cEstado;
                                    objDetalleBE.roop_sUsuarioCreacion = lstObj.roop_sUsuarioCreacion;
                                    objDetalleBE.roop_vIPCreacion = lstObj.roop_vIPCreacion;
                                    objDetalleBE.OficinaConsularId = lstObj.OficinaConsularId;

                                    int IntRolOpcionId = 0;
                                    objDADet.Insert(objDetalleBE, ref IntRolOpcionId, ref Error);

                                    strOpciones += "|" + IntRolOpcionId.ToString();

                                    if ((Error == true) || (IntRolOpcionId == 0))
                                    {
                                        lCancel = true;
                                    }                                       
                                }

                                if (strOpciones.Length > 0)
                                {                                        
                                    pobjBE.roco_vRolOpcion = strOpciones;
                                }

                                objDA.Update(pobjBE, ref Error);

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

                        throw new Exception("Rollback: RolConfigMantenimientoBL - Actualizar");
                    }
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                        audi_vComentario = ex.Message,
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.roco_sUsuarioModificacion,
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

        public void Eliminar(BE.SE_ROLCONFIGURACION pobjBE, ref bool Error)
        {
            RolConfigMantenimientoDA objDA = new RolConfigMantenimientoDA();            

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

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

                        throw new Exception("Rollback: RolConfigMantenimientoBL - Eliminar");
                    }
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.roco_sUsuarioModificacion,
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
