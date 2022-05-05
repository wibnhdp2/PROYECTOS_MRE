using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{
    public class VentanillaMantenimientoBL
    {

        public int Insertar(CL_VENTANILLA pobjBE, List<CL_VENTANILLASERVICIO> objListVenServicio)
        {
            VentanillaMantenimientoDA objDA = new VentanillaMantenimientoDA();
            Int64 intResultado = 0;
            int Result = (int)Enumerador.enmResultadoOperacion.ERROR;
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Insertar(pobjBE);
                    if (intResultado > 0)
                    {
                        foreach (CL_VENTANILLASERVICIO objEn in objListVenServicio)
                        {
                            #region Creacion Objeto
                            BE.MRE.CL_VENTANILLASERVICIO VentServDetalle = new BE.MRE.CL_VENTANILLASERVICIO();
                            VentServDetalle.vede_sVentanillaId = Convert.ToInt16(intResultado);
                            VentServDetalle.vede_sServicioId = objEn.vede_sServicioId;
                            VentServDetalle.vede_IObligatorio = objEn.vede_IObligatorio;
                            VentServDetalle.vede_cEstado = objEn.vede_cEstado;
                            VentServDetalle.vede_sUsuarioCreacion = objEn.vede_sUsuarioCreacion;
                            VentServDetalle.vede_vIPCreacion = objEn.vede_vIPCreacion;
                            VentServDetalle.vede_dFechaCreacion = objEn.vede_dFechaCreacion;
                            VentServDetalle.vent_sOficinaConsularId = pobjBE.vent_sOficinaConsularId;

                            objDA.InsertarDetalle(VentServDetalle);
                            #endregion
                        }
                    }
                    Result = (int)Enumerador.enmResultadoOperacion.OK;
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    Result = (int)Enumerador.enmResultadoOperacion.ERROR;

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.vent_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.vent_sUsuarioCreacion,
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
            return Result;
        }

        public int Actualizar(CL_VENTANILLA pobjBE, List<CL_VENTANILLASERVICIO> objListVenServicio)
        {
            VentanillaMantenimientoDA objDA = new VentanillaMantenimientoDA();
            Int64 intResultado = 0;
            int Result = (int)Enumerador.enmResultadoOperacion.ERROR;
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Actualizar(pobjBE);
                    if (intResultado > 0)
                    {
                        foreach (CL_VENTANILLASERVICIO objEn in objListVenServicio)
                        {
                            #region Creacion Objeto
                            BE.MRE.CL_VENTANILLASERVICIO VentServDetalle = new BE.MRE.CL_VENTANILLASERVICIO();
                            VentServDetalle.vede_sVentanillaId = pobjBE.vent_sVentanillaId;
                            VentServDetalle.vede_sServicioId = objEn.vede_sServicioId;
                            VentServDetalle.vede_IObligatorio = objEn.vede_IObligatorio;
                            VentServDetalle.vede_cEstado = objEn.vede_cEstado;
                            VentServDetalle.vede_sUsuarioModificacion = objEn.vede_sUsuarioModificacion;
                            VentServDetalle.vede_vIPModificacion = objEn.vede_vIPModificacion;
                            VentServDetalle.vede_dFechaModificacion = objEn.vede_dFechaModificacion;
                            VentServDetalle.vent_sOficinaConsularId = pobjBE.vent_sOficinaConsularId;

                            objDA.ActualizarDetalle(VentServDetalle);
                            #endregion
                        }
                    }
                    Result = (int)Enumerador.enmResultadoOperacion.OK;
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    Result = (int)Enumerador.enmResultadoOperacion.ERROR;

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.vent_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.vent_sUsuarioModificacion,
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
            return Result;
        }

        public int Eliminar(CL_VENTANILLA pobjBE)
        {
            VentanillaMantenimientoDA objDA = new VentanillaMantenimientoDA();
            int intResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Eliminar(pobjBE);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.vent_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.vent_sUsuarioModificacion,
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
    }
}
