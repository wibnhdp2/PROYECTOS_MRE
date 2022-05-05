using System;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Configuracion.Seguridad.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Seguridad.BL
{
    public class FormularioMantenimientoBL
    {
        public int Insert(ref SE_FORMULARIO pobjBE)
        {
            DA.FormularioMantenimientoDA objDA = new FormularioMantenimientoDA();
            int IntResultado = 0;
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Insert(ref pobjBE);

                    if (IntResultado == (int)Enumerador.enmResultadoQuery.ERR)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = pobjBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = pobjBE.form_sUsuarioCreacion,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        tran.Complete();
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
            return IntResultado;
        }

        public int Update(SE_FORMULARIO pobjBE)
        {
            DA.FormularioMantenimientoDA objDA = new FormularioMantenimientoDA();

            int intResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Update(pobjBE);
                    if (intResultado == (int)Enumerador.enmResultadoQuery.ERR)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = pobjBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = pobjBE.form_sUsuarioCreacion,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        tran.Complete();
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
            return intResultado;
        }

        public int Delete(SE_FORMULARIO pobjBE)
        {
            DA.FormularioMantenimientoDA objDA = new FormularioMantenimientoDA();
            int intResultado = 0;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    intResultado = objDA.Delete(pobjBE);
                    if (intResultado == (int)Enumerador.enmResultadoQuery.ERR)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = pobjBE.OficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = pobjBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = pobjBE.form_sUsuarioCreacion,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        tran.Complete();
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
            return intResultado;
        }
    }
}