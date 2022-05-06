using System;
using System.Collections;
using System.Collections.Generic;
using SGAC.BE.MRE;
using System.Data;
using SGAC.Cliente.Colas.DA;
using System.Transactions;
using SGAC.Accesorios;



namespace SGAC.Cliente.Colas.BL
{
    public class ServicioMantenimientoBL
    {
        public Int64 InsertarDet(CL_SERVICIO ObjTicteraBE, DataTable DtSubServicios)
        {
            ServicioMantenimientoDA objDA = new ServicioMantenimientoDA();
            Int64 intResultado = 0;
            int Result = (int)Enumerador.enmResultadoOperacion.ERROR;
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.InsertarDet(ObjTicteraBE);
                    if (intResultado > 0)
                    {
                        foreach (DataRow row in DtSubServicios.Rows)
                        {
                            #region Creacion Objeto
                            BE.MRE.CL_SERVICIO ServicioDetalle = new BE.MRE.CL_SERVICIO();
                            ServicioDetalle.serv_sOficinaConsularId = Convert.ToInt16(row["serv_sOficinaConsularId"].ToString());
                            ServicioDetalle.serv_vDescripcion = row["serv_vDescripcion"].ToString();
                            ServicioDetalle.serv_IOrden = Convert.ToInt32(row["serv_IOrden"].ToString());
                            ServicioDetalle.serv_sServicioIdCab = Convert.ToInt16(intResultado);
                            ServicioDetalle.serv_sTipo = Convert.ToInt16(row["serv_sTipo"].ToString());
                            ServicioDetalle.serv_cEstado = row["serv_cEstado"].ToString();
                            ServicioDetalle.serv_sUsuarioCreacion = ObjTicteraBE.serv_sUsuarioCreacion;
                            ServicioDetalle.serv_vIPCreacion = ObjTicteraBE.serv_vIPCreacion;
                            ServicioDetalle.serv_dFechaCreacion = ObjTicteraBE.serv_dFechaCreacion;
                            //--------------------------------------------
                            //Fecha: 23/02/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Se adiciona la columna: serv_vServicioDireccion
                            //--------------------------------------------
                            ServicioDetalle.serv_vServicioDireccion = row["serv_vServicioDireccion"].ToString();

                            objDA.InsertDetail(ServicioDetalle);
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
                        audi_sOficinaConsularId = (Int16)ObjTicteraBE.serv_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)ObjTicteraBE.serv_sUsuarioCreacion,
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

        public int Update(BE.MRE.CL_SERVICIO ObjTicteraBE, DataTable DtSubServicios)
        {
            ServicioMantenimientoDA objDA = new ServicioMantenimientoDA();
            int intResultado = 0;
            int Result = (int)Enumerador.enmResultadoOperacion.ERROR;
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Update(ObjTicteraBE);

                    if (intResultado > 0)
                    {

                        foreach (DataRow row in DtSubServicios.Rows)
                        {
                            #region Creacion Objeto
                            BE.MRE.CL_SERVICIO ServicioDetalle = new BE.MRE.CL_SERVICIO();
                            ServicioDetalle.serv_sServicioId = Convert.ToInt16(row["serv_sServicioId"]);
                            ServicioDetalle.serv_sOficinaConsularId = ObjTicteraBE.serv_sOficinaConsularId;
                            ServicioDetalle.serv_vDescripcion = row["serv_vDescripcion"].ToString();
                            ServicioDetalle.serv_IOrden = Convert.ToInt32(row["serv_IOrden"].ToString());
                            ServicioDetalle.serv_sServicioIdCab = ObjTicteraBE.serv_sServicioId;

                            ServicioDetalle.serv_cEstado = row["serv_cEstado"].ToString();
                            ServicioDetalle.serv_sTipo = Convert.ToInt16(row["serv_sTipo"].ToString());
                            //--------------------------------------------
                            //Fecha: 23/02/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Se adiciona la columna: serv_vServicioDireccion
                            //--------------------------------------------
                            ServicioDetalle.serv_vServicioDireccion = row["serv_vServicioDireccion"].ToString();

                            ServicioDetalle.serv_dFechaCreacion = ObjTicteraBE.serv_dFechaCreacion;
                            ServicioDetalle.serv_sUsuarioCreacion = ObjTicteraBE.serv_sUsuarioCreacion;
                            ServicioDetalle.serv_vIPCreacion = ObjTicteraBE.serv_vIPCreacion;
                            ServicioDetalle.serv_dFechaModificacion = ObjTicteraBE.serv_dFechaModificacion;
                            ServicioDetalle.serv_sUsuarioModificacion = ObjTicteraBE.serv_sUsuarioModificacion;
                            ServicioDetalle.serv_vIPModificacion = ObjTicteraBE.serv_vIPModificacion;
                            objDA.UpdateDetail(ServicioDetalle);

                            #endregion
                        }
                    }
                    tran.Complete();
                    Result = (int)Enumerador.enmResultadoOperacion.OK;
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
                        audi_sOficinaConsularId = (Int16)ObjTicteraBE.serv_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)ObjTicteraBE.serv_sUsuarioModificacion,
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

        public int Delete(BE.MRE.CL_SERVICIO ObjTicteraBE)
        {
            ServicioMantenimientoDA objDA = new ServicioMantenimientoDA();
            int Result = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    Result = objDA.Delete(ObjTicteraBE);
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
                        audi_sOficinaConsularId = (Int16)ObjTicteraBE.serv_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)ObjTicteraBE.serv_sUsuarioModificacion,
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
    }
}



