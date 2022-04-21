using System;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class LibroMantenimientoBL
    {
        public SI_LIBRO insertar(SI_LIBRO objLibro)
        {
            LibroMantenimientoDA objDA = new LibroMantenimientoDA();
            SI_LIBRO IntResultado = new SI_LIBRO();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objLibro.libr_sOficinaConsularId;
            UsuarioId = objLibro.libr_sUsuarioCreacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Insertar(objLibro);

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

        public SI_LIBRO actualizar(SI_LIBRO objLibro)
        {

            LibroMantenimientoDA objDA = new LibroMantenimientoDA();
            SI_LIBRO IntResultado = new SI_LIBRO();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objLibro.libr_sOficinaConsularId;
            UsuarioId = (Int16)objLibro.libr_sUsuarioModificacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Actualizar(objLibro);

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

            //LibroMantenimientoDA objDA = new LibroMantenimientoDA();
            //return objDA.Actualizar(objLibro);
        }

        public SI_LIBRO eliminar(SI_LIBRO objLibro)
        {
            LibroMantenimientoDA objDA = new LibroMantenimientoDA();
            SI_LIBRO IntResultado = new SI_LIBRO();

            Int16 UsuarioId = 0;
            Int16 OficnaConsularId = 0;


            OficnaConsularId = objLibro.libr_sOficinaConsularId;
            UsuarioId = (Int16)objLibro.libr_sUsuarioModificacion;

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    IntResultado = objDA.Eliminar(objLibro);

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

            //LibroMantenimientoDA objDA = new LibroMantenimientoDA();
            //return objDA.Eliminar(objLibro);
        }
    }
}
