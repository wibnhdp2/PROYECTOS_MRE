using System;
using System.Collections.Generic;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Contabilidad.Remesa.DA;
using SGAC.Contabilidad.RemesaDetalle.DA;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.Remesa.BL
{
    public class RemesaMantenimientoBL
    {
        public void Insert(CO_REMESA pobjBE, List<CO_REMESADETALLE> lstDetalle, ref bool Error)
        {
            RemesaMantenimientoDA objDA = new RemesaMantenimientoDA();
            RemesaDetalleMantenimientoDA objDADet = new RemesaDetalleMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            string ErrorMensaje = string.Empty;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    long LonRemesaId = 0;
                    objDA.Insert(pobjBE, ref LonRemesaId, ref Error);

                    if ((Error == false) || (LonRemesaId != 0))
                    {
                        if ((lCancel == false) && (lstDetalle.Count != 0))
                        {
                            if (lstDetalle != null)
                            {
                                foreach (SGAC.BE.MRE.CO_REMESADETALLE lstObj in lstDetalle)
                                {
                                    ErrorMensaje = "";

                                    SGAC.BE.MRE.CO_REMESADETALLE objDetalleBE = new SGAC.BE.MRE.CO_REMESADETALLE();
                                    objDetalleBE.rede_iRemesaId = LonRemesaId;
                                    objDetalleBE.rede_sTipoId = lstObj.rede_sTipoId;
                                    objDetalleBE.rede_cPeriodo = lstObj.rede_cPeriodo;
                                    objDetalleBE.rede_dFechaEnvio = lstObj.rede_dFechaEnvio;
                                    objDetalleBE.rede_FTipoCambioBancario = lstObj.rede_FTipoCambioBancario;
                                    objDetalleBE.rede_FTipoCambioConsular = lstObj.rede_FTipoCambioConsular;
                                    objDetalleBE.rede_sCuentaCorrienteId = lstObj.rede_sCuentaCorrienteId;
                                    objDetalleBE.rede_sMonedaId = lstObj.rede_sMonedaId;
                                    objDetalleBE.rede_FMonto = lstObj.rede_FMonto;
                                    objDetalleBE.rede_vNroVoucher = lstObj.rede_vNroVoucher;
                                    objDetalleBE.rede_vResponsableEnvio = lstObj.rede_vResponsableEnvio;
                                    objDetalleBE.rede_vRecurrente = lstObj.rede_vRecurrente;
                                    objDetalleBE.rede_sTarifarioId = lstObj.rede_sTarifarioId;
                                    objDetalleBE.rede_vObservacion = lstObj.rede_vObservacion;
                                    objDetalleBE.rede_bMesFlag = lstObj.rede_bMesFlag;
                                    objDetalleBE.rede_sEstadoId = lstObj.rede_sEstadoId;
                                    objDetalleBE.rede_sUsuarioCreacion = lstObj.rede_sUsuarioCreacion;
                                    objDetalleBE.rede_vIPCreacion = lstObj.rede_vIPCreacion;
                                    objDetalleBE.OficinaConsularId = lstObj.OficinaConsularId;

                                    objDetalleBE.ClasificacionID = lstObj.ClasificacionID;
                                    long LonRemesaDetalleId = 0;
                                    objDADet.Insert(objDetalleBE, ref LonRemesaDetalleId, ref Error, ref ErrorMensaje);

                                    if ((Error == true) || (LonRemesaDetalleId == 0))
                                    {
                                        lCancel = true;
                                    }
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
                      //  Transaction.Current.Rollback();
                        scope.Dispose();

                        #region Registro Incidencia
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_vValoresTabla = "",
                            audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                            audi_vComentario = ErrorMensaje,
                            audi_vMensaje = ErrorMensaje,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = pobjBE.reme_sUsuarioCreacion,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                        #endregion
                    }
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

        public void Update(CO_REMESA pobjBE, List<CO_REMESADETALLE> lstDetalle, ref bool Error)
        {
            RemesaMantenimientoDA objDA = new RemesaMantenimientoDA();
            RemesaDetalleMantenimientoDA objDADet = new RemesaDetalleMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            string strErrorMensaje = string.Empty;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref Error);

                    if (Error == false)
                    {
                        if (lCancel == false)
                        {
                            SGAC.BE.MRE.CO_REMESADETALLE objDetalleDeleteBE = new SGAC.BE.MRE.CO_REMESADETALLE();
                            objDetalleDeleteBE.rede_iRemesaId = pobjBE.reme_iRemesaId;
                            objDetalleDeleteBE.rede_sUsuarioModificacion = pobjBE.reme_sUsuarioModificacion;
                            objDetalleDeleteBE.rede_vIPModificacion = pobjBE.reme_vIPModificacion;
                            objDADet.Delete(objDetalleDeleteBE, ref Error);

                            if (!Error)
                            {
                                if (lstDetalle != null)
                                {
                                    foreach (SGAC.BE.MRE.CO_REMESADETALLE lstObj in lstDetalle)
                                    {
                                        SGAC.BE.MRE.CO_REMESADETALLE objDetalleBE = new SGAC.BE.MRE.CO_REMESADETALLE();
                                        objDetalleBE.rede_iRemesaDetalleId = lstObj.rede_iRemesaDetalleId;
                                        objDetalleBE.rede_iRemesaId = pobjBE.reme_iRemesaId;
                                        objDetalleBE.rede_sTipoId = lstObj.rede_sTipoId;
                                        objDetalleBE.rede_cPeriodo = lstObj.rede_cPeriodo;
                                        objDetalleBE.rede_dFechaEnvio = lstObj.rede_dFechaEnvio;
                                        objDetalleBE.rede_FTipoCambioBancario = lstObj.rede_FTipoCambioBancario;
                                        objDetalleBE.rede_FTipoCambioConsular = lstObj.rede_FTipoCambioConsular;
                                        objDetalleBE.rede_sCuentaCorrienteId = lstObj.rede_sCuentaCorrienteId;
                                        objDetalleBE.rede_sMonedaId = lstObj.rede_sMonedaId;
                                        objDetalleBE.rede_FMonto = lstObj.rede_FMonto;
                                        objDetalleBE.rede_vNroVoucher = lstObj.rede_vNroVoucher;
                                        objDetalleBE.rede_vResponsableEnvio = lstObj.rede_vResponsableEnvio;
                                        objDetalleBE.rede_vRecurrente = lstObj.rede_vRecurrente;
                                        objDetalleBE.rede_sTarifarioId = lstObj.rede_sTarifarioId;
                                        objDetalleBE.rede_vObservacion = lstObj.rede_vObservacion;
                                        objDetalleBE.rede_bMesFlag = lstObj.rede_bMesFlag;
                                        objDetalleBE.rede_sEstadoId = lstObj.rede_sEstadoId;
                                        objDetalleBE.rede_sUsuarioModificacion = lstObj.rede_sUsuarioModificacion;
                                        objDetalleBE.rede_vIPModificacion = lstObj.rede_vIPModificacion;
                                        objDetalleBE.OficinaConsularId = lstObj.OficinaConsularId;
                                        objDetalleBE.ClasificacionID = lstObj.ClasificacionID;

                                        long LonRemesaDetalleId = 0;
                                        objDADet.Update(objDetalleBE, ref LonRemesaDetalleId, ref Error);

                                        if ((Error == true) || (LonRemesaDetalleId == 0))
                                        {
                                            lCancel = true;
                                        }
                                    }
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
                      //  Transaction.Current.Rollback();
                        scope.Dispose();

                        //#region Registro Incidencia
                        //new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                        //{
                        //    audi_vNombreRuta = Util.ObtenerNameForm(),
                        //    audi_vValoresTabla = "",
                        //    audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        //    audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                        //    audi_sTablaId = null,
                        //    audi_sClavePrimaria = null,
                        //    audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                        //    audi_vComentario = ErrorMensaje,
                        //    audi_vMensaje = ErrorMensaje,
                        //    audi_vHostName = Util.ObtenerHostName(),
                        //    audi_sUsuarioCreacion = pobjBE.reme_sUsuarioCreacion,
                        //    audi_vIPCreacion = Util.ObtenerDireccionIP()
                        //});
                        //#endregion
                    }
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
        }

        public void Eliminar(CO_REMESA pobjBE, ref bool Error)
        {
            RemesaMantenimientoDA objDA = new RemesaMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Eliminar(pobjBE, ref Error);

                    if (Error == false)
                    {
                        scope.Complete();
                    }
                    else
                    {
                      //  Transaction.Current.Rollback();
                        scope.Dispose();
                    }
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

        public void ActualizarEstado(int intRemesaId,
                                     int intTipoRemesa,
                                     int intEstadoId,
                                     int intUsuarioModificacion,
                                     string strIPModificacion,
                                     int intOficinaConsular,
                                     ref bool Error)
        {
            RemesaMantenimientoDA objDA = new RemesaMantenimientoDA();

            string strErrorMensaje = string.Empty;
            string strErrorDetalle = string.Empty;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {

                    objDA.ActualizarEstado(intRemesaId,
                                            intTipoRemesa,
                                            intEstadoId,
                                            intUsuarioModificacion,
                                            strIPModificacion,
                                            intOficinaConsular,
                                            ref Error,
                                            ref strErrorMensaje, ref strErrorDetalle);

                    if (Error == false)
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
                    #region Registro Incidencia
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_vValoresTabla = "",
                        audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(intOficinaConsular),
                        audi_vComentario = strErrorMensaje,
                        audi_vMensaje = strErrorDetalle,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = Convert.ToInt16(intUsuarioModificacion),
                        audi_vIPCreacion = strIPModificacion
                    });
                    #endregion
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
