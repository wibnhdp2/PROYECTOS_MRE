using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class ParametroMantenimientoBL
    {
        public int Insertar(SI_PARAMETRO pobjBE, int IntOficinaConsular)
        {
            DA.ParametroMantenimientoDA objDA = new DA.ParametroMantenimientoDA();

            int IntResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Insertar(pobjBE, IntOficinaConsular);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(IntOficinaConsular),
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = pobjBE.para_sUsuarioCreacion,
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
            return IntResultado;
        }

        public int Actualizar(SI_PARAMETRO pobjBE, int IntOficinaConsular)
        {
            DA.ParametroMantenimientoDA objDA = new DA.ParametroMantenimientoDA();

            int IntResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Actualizar(pobjBE, IntOficinaConsular);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(IntOficinaConsular),
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.para_sUsuarioModificacion,
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

            return IntResultado;
        }

        public int Eliminar(SI_PARAMETRO pobjBE, int IntOficinaConsular)
        {
            DA.ParametroMantenimientoDA objDA = new DA.ParametroMantenimientoDA();

            int IntResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Eliminar(pobjBE, IntOficinaConsular);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(IntOficinaConsular),
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.para_sUsuarioModificacion,
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
            return IntResultado;
        }
    }
}
    
