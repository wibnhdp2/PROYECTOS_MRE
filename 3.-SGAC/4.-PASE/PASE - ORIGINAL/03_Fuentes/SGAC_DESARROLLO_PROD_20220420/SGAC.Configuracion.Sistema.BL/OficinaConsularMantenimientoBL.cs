using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class OficinaConsularMantenimientoBL
    {
        public string str_Mensaje { get; set; }
        public void Insert(SI_OFICINACONSULAR pobjBE, List<RE_OFICINACONSULARFUNCIONARIO> lstDetalle, ref bool Error)
        {
            DA.OficinaConsularMantenimientoDA objDA = new DA.OficinaConsularMantenimientoDA();
            DA.OficinaConsularFuncMantenimientoDA objDADet = new DA.OficinaConsularFuncMantenimientoDA();



            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    int IntOficinaConsularId = 0;
                    objDA.Insert(pobjBE, ref IntOficinaConsularId, ref Error);

                    if ((Error == false) || (IntOficinaConsularId != 0))
                    {
                        if ((lCancel == false) && (lstDetalle != null))
                        {
                            int IntFuncionarioOficConsulId;
                            foreach (BE.MRE.RE_OFICINACONSULARFUNCIONARIO objFuncionario in lstDetalle)
                            {
                                if (objFuncionario.ocfu_sOficinaConsularId == 0)
                                    objFuncionario.ocfu_sOficinaConsularId = Convert.ToInt16(IntOficinaConsularId);

                                IntFuncionarioOficConsulId = 0;
                                if (objFuncionario.ocfu_cEstado.Equals(((char)Enumerador.enmEstado.DESACTIVO).ToString()))
                                {
                                    if (objFuncionario.ocfu_sOfiConFuncionarioId != 0)
                                    {
                                        objDADet.Delete(objFuncionario, ref Error);

                                        if ((Error == true) || (IntFuncionarioOficConsulId == 0))
                                        {
                                            lCancel = true;
                                            str_Mensaje = objDADet.strError;
                                        }
                                    }
                                }
                                else
                                {
                                    if (objFuncionario.ocfu_sOfiConFuncionarioId == 0)
                                    {
                                        objDADet.Insert(objFuncionario, ref IntFuncionarioOficConsulId, ref Error);

                                        if ((Error == true) || (IntFuncionarioOficConsulId == 0))
                                        {
                                            lCancel = true;
                                            str_Mensaje = objDADet.strError;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lCancel = true;
                        str_Mensaje = objDA.strError;
                    }

                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
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

                if (!string.IsNullOrEmpty(str_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA),
                        audi_vComentario = "",
                        audi_vMensaje = str_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = pobjBE.ofco_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }
            }
        }

        public void Update(SI_OFICINACONSULAR pobjBE, List<RE_OFICINACONSULARFUNCIONARIO> lstDetalle, List<SI_OFICINACONSULAR> lstDependiente, ref bool Error)
        {
            DA.OficinaConsularMantenimientoDA objDA = new DA.OficinaConsularMantenimientoDA();
            DA.OficinaConsularFuncMantenimientoDA objDADet = new DA.OficinaConsularFuncMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.Update(pobjBE, ref Error);

                    if (Error == false)
                    {
                        if (lCancel == false)
                        {
                            if (lstDetalle != null)
                            {
                                int IntFuncionarioOficConsulId = 0;
                                foreach (BE.MRE.RE_OFICINACONSULARFUNCIONARIO objFuncionario in lstDetalle)
                                {
                                    if (objFuncionario.ocfu_cEstado.Equals(((char)Enumerador.enmEstado.DESACTIVO).ToString()))
                                    {
                                        if (objFuncionario.ocfu_sOfiConFuncionarioId != 0)
                                        {
                                            IntFuncionarioOficConsulId = Convert.ToInt32(objFuncionario.ocfu_IFuncionarioId);
                                            objDADet.Delete(objFuncionario, ref Error);

                                            if ((Error == true) || (IntFuncionarioOficConsulId == 0))
                                            {
                                                lCancel = true;
                                                str_Mensaje = objDADet.strError;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objFuncionario.ocfu_sOfiConFuncionarioId == 0)
                                        {
                                            objDADet.Insert(objFuncionario, ref IntFuncionarioOficConsulId, ref Error);

                                            if ((Error == true) || (IntFuncionarioOficConsulId == 0))
                                            {
                                                lCancel = true;
                                                str_Mensaje = objDADet.strError;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (lstDependiente != null)
                        {
                            BE.MRE.SI_OFICINACONSULAR objDependienteDelete = new BE.MRE.SI_OFICINACONSULAR();
                            objDependienteDelete.ofco_sOficinaConsularId = pobjBE.ofco_sOficinaConsularId;
                            objDependienteDelete.ofco_sUsuarioModificacion = pobjBE.ofco_sUsuarioModificacion;
                            objDependienteDelete.ofco_vIPModificacion = pobjBE.ofco_vIPModificacion;
                            objDA.DeleteDependent(objDependienteDelete);

                                foreach (BE.MRE.SI_OFICINACONSULAR lstDependObj in lstDependiente)
                                {
                                    BE.MRE.SI_OFICINACONSULAR ObjDetalleDependiente = new BE.MRE.SI_OFICINACONSULAR();
                                    ObjDetalleDependiente.ofco_sOficinaConsularId = lstDependObj.ofco_sOficinaConsularId;
                                    ObjDetalleDependiente.ofco_sReferenciaId = lstDependObj.ofco_sReferenciaId;
                                    ObjDetalleDependiente.ofco_sUsuarioModificacion = lstDependObj.ofco_sUsuarioModificacion;
                                    ObjDetalleDependiente.ofco_vIPModificacion = lstDependObj.ofco_vIPModificacion;

                                    objDA.UpdateDependent(ObjDetalleDependiente);
                                }
                        }
                    }
                    else
                    {
                        lCancel = true;
                        str_Mensaje = objDA.strError;
                    }

                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
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

                if (!string.IsNullOrEmpty(str_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA),
                        audi_vComentario = "",
                        audi_vMensaje = str_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.ofco_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }
            }
        }

        public void Delete(SI_OFICINACONSULAR pobjBE, ref bool Error)
        {
            DA.OficinaConsularMantenimientoDA objDA = new DA.OficinaConsularMantenimientoDA();

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

                        str_Mensaje = objDA.strError;
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

                if (!string.IsNullOrEmpty(str_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA),
                        audi_vComentario = "",
                        audi_vMensaje = str_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.ofco_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }
            }
        }


        public void RegistrarMoneda(Int16 ofmo_sOficinaMonedaId, Int16 ofco_sOficinaConsularId, Int16 intMoneda, DateTime dFechaInicio, DateTime dFechaFin, Int16 UsuarioCreacion, string vIP,
            ref string resultado, ref bool Error)
        {
            DA.OficinaConsularMantenimientoDA objDA = new DA.OficinaConsularMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.RegistrarMoneda(ofmo_sOficinaMonedaId, ofco_sOficinaConsularId, intMoneda, dFechaInicio, dFechaFin, UsuarioCreacion,vIP, ref resultado,ref Error);

                    if (Error == false)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                        str_Mensaje = objDA.strError;
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

                if (!string.IsNullOrEmpty(str_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA),
                        audi_vComentario = "",
                        audi_vMensaje = str_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)UsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }
            }
        }


        public void EliminarMoneda(Int16 ofmo_sOficinaMonedaId, Int16 UsuarioCreacion, string vIP,
            ref bool Error)
        {
            DA.OficinaConsularMantenimientoDA objDA = new DA.OficinaConsularMantenimientoDA();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objDA.EliminarMoneda(ofmo_sOficinaMonedaId, UsuarioCreacion, vIP, ref Error);

                    if (Error == false)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();

                        str_Mensaje = objDA.strError;
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

                if (!string.IsNullOrEmpty(str_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA),
                        audi_vComentario = "",
                        audi_vMensaje = str_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)UsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }
            }
        }
    }
}

