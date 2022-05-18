using System;
using System.Collections.Generic;
using System.Transactions;
using SGAC.Configuracion.Sistema.DA;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class TarifarioMantenimientoBL
    {
        public void Insert(SI_TARIFARIO pobjBE, List<SI_TARIFARIOREQUISITO> lstDetalle, ref bool Error)
        {
            DA.TarifarioMantenimientoDA objDA = new DA.TarifarioMantenimientoDA();
            DA.TarifarioRequisitoMantenimientoDA objDADet = new DA.TarifarioRequisitoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    int IntTarifarioId = 0;
                    objDA.Insert(pobjBE, ref IntTarifarioId, ref Error);

                    if ((Error == false) || (IntTarifarioId != 0))
                    {
                        if ((lCancel == false) && (lstDetalle != null))
                        {
                            foreach (BE.MRE.SI_TARIFARIOREQUISITO lstObj in lstDetalle)                                
                            {
                                BE.MRE.SI_TARIFARIOREQUISITO objDetalleBE = new BE.MRE.SI_TARIFARIOREQUISITO();
                                objDetalleBE.tare_sTarifarioId = (short)IntTarifarioId;
                                objDetalleBE.tare_sRequisitoId = lstObj.tare_sRequisitoId;
                                objDetalleBE.tare_sTipoActaId = lstObj.tare_sTipoActaId;
                                objDetalleBE.tare_sCondicionId = lstObj.tare_sCondicionId;
                                objDetalleBE.tare_sUsuarioCreacion = lstObj.tare_sUsuarioCreacion;
                                objDetalleBE.tare_vIPCreacion = lstObj.tare_vIPCreacion;
                                objDetalleBE.OficinaConsularId = lstObj.OficinaConsularId;

                                int IntTarifarioRequisitoId = 0;
                                objDADet.Insert(objDetalleBE, ref IntTarifarioRequisitoId, ref Error);

                                if ((Error == true) || (IntTarifarioRequisitoId == 0))
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
                       // Transaction.Current.Rollback();
                        scope.Dispose();
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
                        audi_sOficinaConsularId = (Int16)pobjBE.OficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tari_sUsuarioCreacion,
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

        public void Update(SI_TARIFARIO pobjBE, BE.MRE.SI_TARIFARIO pobjBEc, List<SI_TARIFARIOREQUISITO> lstDetalle, ref bool Error)
        {
            DA.TarifarioMantenimientoDA objDA = new DA.TarifarioMantenimientoDA();
            DA.TarifarioRequisitoMantenimientoDA objDADet = new DA.TarifarioRequisitoMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref Error);

                    if (Error == false)
                    {
                        if (lCancel == false)
                        {
                            BE.MRE.SI_TARIFARIOREQUISITO objDetalleDeleteBE = new BE.MRE.SI_TARIFARIOREQUISITO();
                            objDetalleDeleteBE.tare_sTarifarioId = pobjBE.tari_sTarifarioId;
                            objDetalleDeleteBE.tare_sUsuarioModificacion = pobjBE.tari_sUsuarioModificacion;
                            objDetalleDeleteBE.tare_vIPModificacion = pobjBE.tari_vIPModificacion;
                            objDADet.Delete(objDetalleDeleteBE, ref Error);

                            if (!Error)
                            {
                                if (lstDetalle != null)
                                {
                                    foreach (BE.MRE.SI_TARIFARIOREQUISITO lstObj in lstDetalle)  
                                    {
                                        BE.MRE.SI_TARIFARIOREQUISITO objDetalleBE = new BE.MRE.SI_TARIFARIOREQUISITO();
                                        objDetalleBE.tare_sTarifarioId = pobjBE.tari_sTarifarioId;
                                        objDetalleBE.tare_sRequisitoId = lstObj.tare_sRequisitoId;
                                        objDetalleBE.tare_sTipoActaId = lstObj.tare_sTipoActaId;
                                        objDetalleBE.tare_sCondicionId = lstObj.tare_sCondicionId;
                                        objDetalleBE.tare_sUsuarioModificacion = lstObj.tare_sUsuarioModificacion;
                                        objDetalleBE.tare_vIPModificacion = lstObj.tare_vIPModificacion;
                                        objDetalleBE.OficinaConsularId = lstObj.OficinaConsularId;

                                        int IntTarifarioRequisitoId = 0;
                                        objDADet.Update(objDetalleBE, ref IntTarifarioRequisitoId, ref Error);

                                        if ((Error == true) || (IntTarifarioRequisitoId == 0))
                                        {
                                            lCancel = true;                                            
                                        }
                                    }
                                }
                            }

                            if (pobjBE.tari_sEstadoId != 35) // SI ES DIFERENTE DE ANULADO GUARDA HISTORIAL
                            {
                                int IntTarifarioId = 0;
                                objDA.Insert(pobjBEc, ref IntTarifarioId, ref Error);

                                if ((Error == true) || (IntTarifarioId == 0))
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

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                     //   Transaction.Current.Rollback();
                        scope.Dispose();
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
                        audi_sOficinaConsularId = (Int16)pobjBE.OficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tari_sUsuarioModificacion,
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

        public void Delete(SI_TARIFARIO pobjBE, ref bool Error)
        {
            DA.TarifarioMantenimientoDA objDA = new DA.TarifarioMantenimientoDA();

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
                     //   Transaction.Current.Rollback();
                        scope.Dispose();
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
                        audi_sOficinaConsularId = (Int16)pobjBE.OficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tari_sUsuarioModificacion,
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
