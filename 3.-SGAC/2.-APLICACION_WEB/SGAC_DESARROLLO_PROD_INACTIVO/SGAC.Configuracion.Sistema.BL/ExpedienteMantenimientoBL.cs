using System;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class ExpedienteMantenimientoBL
    {
        public SI_EXPEDIENTE insertar(SI_EXPEDIENTE objExpediente)
        {
            ExpedienteMantenimientoDA objDA = new ExpedienteMantenimientoDA();
            SI_EXPEDIENTE IntResultado = new SI_EXPEDIENTE();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;

            OficnaConsularId = objExpediente.exp_sOficinaConsularId;
            UsuarioId = objExpediente.exp_sUsuarioCreacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Insertar(objExpediente);

                    if (IntResultado.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = OficnaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = IntResultado.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = UsuarioId,
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

        public SI_EXPEDIENTE actualizar(SI_EXPEDIENTE objExpediente)
        {

            ExpedienteMantenimientoDA objDA = new ExpedienteMantenimientoDA();
            SI_EXPEDIENTE IntResultado = new SI_EXPEDIENTE();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objExpediente.exp_sOficinaConsularId;
            UsuarioId = (Int16)objExpediente.exp_sUsuarioModificacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Actualizar(objExpediente);

                    if (IntResultado.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = OficnaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = IntResultado.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = UsuarioId,
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

        public SI_EXPEDIENTE eliminar(SI_EXPEDIENTE objExpediente)
        {
            ExpedienteMantenimientoDA objDA = new ExpedienteMantenimientoDA();
            SI_EXPEDIENTE IntResultado = new SI_EXPEDIENTE();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objExpediente.exp_sOficinaConsularId;
            UsuarioId = (Int16)objExpediente.exp_sUsuarioModificacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Eliminar(objExpediente);

                    if (IntResultado.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = OficnaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = IntResultado.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = UsuarioId,
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
    }
}
