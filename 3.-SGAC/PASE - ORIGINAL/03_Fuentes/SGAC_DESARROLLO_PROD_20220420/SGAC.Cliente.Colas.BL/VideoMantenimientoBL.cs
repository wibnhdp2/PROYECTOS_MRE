using SGAC.BE.MRE;
using System.Transactions;
using SGAC.Cliente.Colas.DA;
using System;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.BL
{
    public class VideoMantenimientoBL
    {
        public int Insertar(CL_VIDEO pobjBE)
        {
            VideoMantenimientoDA objDA = new VideoMantenimientoDA();

            int intResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Insertar(pobjBE);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.vide_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.vide_sUsuarioCreacion,
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

        public int Actualizar(CL_VIDEO pobjBE)
        {
            VideoMantenimientoDA objDA = new VideoMantenimientoDA();

            int intResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Actualizar(pobjBE);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.vide_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.vide_sUsuarioModificacion,
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

        public int Eliminar(CL_VIDEO pobjBE)
        {
            VideoMantenimientoDA objDA = new VideoMantenimientoDA();

            int intResultado = 0;

            using(TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Eliminar(pobjBE);
                    tran.Complete();
                }
                catch(Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)pobjBE.vide_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.vide_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if(objDA != null)
                    {
                        objDA = null;
                    }
                }
            }

            return intResultado;
        }
    }
}