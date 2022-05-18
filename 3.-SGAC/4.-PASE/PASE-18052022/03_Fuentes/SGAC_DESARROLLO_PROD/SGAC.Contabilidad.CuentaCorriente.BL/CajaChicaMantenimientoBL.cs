using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE;
using SGAC.Contabilidad.CuentaCorriente.DA;
using System.Transactions;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.CuentaCorriente.BL
{
    public class CajaChicaMantenimientoBL
    {
        public CO_MOVIMIENTOCAJACHICA Insert(CO_MOVIMIENTOCAJACHICA pobjBE)
        {
            CajaChicaMantenimientoDA objDA = new CajaChicaMantenimientoDA();
            try
            {
                bool lCancel = false;
                var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
                {
                    pobjBE = objDA.Insert(pobjBE);

                    if (pobjBE.Error == true || pobjBE.moca_iMovimientoCajaChicaId == 0)
                    {
                        lCancel = true;
                    }

                    if (lCancel == true)
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }

                if (!string.IsNullOrEmpty(objDA.ErrMessage))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = pobjBE.moca_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = objDA.ErrMessage,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = pobjBE.moca_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }

                return pobjBE;
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

        public CO_MOVIMIENTOCAJACHICA Update(CO_MOVIMIENTOCAJACHICA pobjBE)
        {
            //CuentaMantenimientoDA objDA = new CuentaMantenimientoDA();
            //try
            //{
            //    bool lCancel = false;

            //    var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            //    {
            //        pobjBE = objDA.Update(pobjBE);

            //        if (pobjBE.Error == true)
            //        {
            //            lCancel = true;
            //        }

            //        if (lCancel == true)
            //        {
            //            Transaction.Current.Rollback();
            //            scope.Dispose();
            //        }
            //        else
            //        {
            //            scope.Complete();
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(pobjBE.Message))
            //    {
            //        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
            //        {
            //            audi_vNombreRuta = Util.ObtenerNameForm(),
            //            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
            //            audi_sTablaId = null,
            //            audi_sClavePrimaria = null,
            //            audi_sOficinaConsularId = pobjBE.cuco_sOficinaConsularId,
            //            audi_vComentario = "",
            //            audi_vMensaje = pobjBE.Message,
            //            audi_vHostName = Util.ObtenerHostName(),
            //            audi_sUsuarioCreacion = pobjBE.cuco_sUsuarioCreacion,
            //            audi_vIPCreacion = Util.ObtenerDireccionIP()
            //        });
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message, ex.InnerException);
            //}
            //finally
            //{
            //    if (objDA != null)
            //    {
            //        objDA = null;
            //    }
            //}

            return pobjBE;
        }

        public int Delete(CO_MOVIMIENTOCAJACHICA pobjBE)
        {
            //CuentaMantenimientoDA objDA = new CuentaMantenimientoDA();
            //int intResultado = 0;

            //var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            //{
            //    try
            //    {
            //        intResultado = objDA.Delete(pobjBE);
            //        if (intResultado > 0)
            //            scope.Complete();
            //        else
            //        {
            //            Transaction.Current.Rollback();
            //            scope.Dispose();

            //            new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
            //            {
            //                audi_vNombreRuta = Util.ObtenerNameForm(),
            //                audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
            //                audi_sTablaId = null,
            //                audi_sClavePrimaria = null,
            //                audi_sOficinaConsularId = pobjBE.cuco_sOficinaConsularId,
            //                audi_vComentario = "",
            //                audi_vMensaje = "Error al eliminar el registro",
            //                audi_vHostName = Util.ObtenerHostName(),
            //                audi_sUsuarioCreacion = pobjBE.cuco_sUsuarioCreacion,
            //                audi_vIPCreacion = Util.ObtenerDireccionIP()
            //            });

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message, ex.InnerException);
            //    }
            //    finally
            //    {
            //        if (objDA != null)
            //        {
            //            objDA = null;
            //        }
            //    }
            //}

            return 0;
        }
    }
}
