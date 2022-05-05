using System;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class TipoCambioBancarioMantenimientoBL
    {
        public int Insert(SI_TIPOCAMBIO_BANCARIO pobjBE)
        {
            DA.TipoCambioBancarioMantenimientoDA objDA =new DA.TipoCambioBancarioMantenimientoDA();

            int IntResultado=0;

            using (TransactionScope tran=new TransactionScope())
            {
                try
                {
                    IntResultado=objDA.Insert(ref pobjBE);
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
                        audi_sOficinaConsularId = (Int16)pobjBE.tiba_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tiba_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });


                    throw new Exception(ex.Message,ex.InnerException);

                }finally
                {
                    if(objDA !=null)
                    {
                        objDA=null;
                    }
                }
            }
            return IntResultado;
        }


        public int Update(SI_TIPOCAMBIO_BANCARIO pobjBE)
        {
            DA.TipoCambioBancarioMantenimientoDA objDA=new DA.TipoCambioBancarioMantenimientoDA();

            int IntResultado=0;
            using (TransactionScope tran=new TransactionScope())
            {
                try
                {
                    IntResultado=objDA.Update(pobjBE);
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
                        audi_sOficinaConsularId = (Int16)pobjBE.tiba_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tiba_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    throw new Exception(ex.Message,ex.InnerException);
                }
                finally
                {
                    if(objDA != null)
                    {
                        objDA=null;
                    }

                }
            }
            return IntResultado;
        }

        public int Delete(SI_TIPOCAMBIO_BANCARIO pobjBE)
        {
            DA.TipoCambioBancarioMantenimientoDA objDA=new TipoCambioBancarioMantenimientoDA();

            int IntResultado=0;

            using(TransactionScope tran=new TransactionScope())
            {
                try
                {
                    IntResultado=objDA.Delete(pobjBE);
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
                        audi_sOficinaConsularId = (Int16)pobjBE.tiba_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tiba_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    throw new Exception(ex.Message,ex.InnerException);
                }
                finally
                {
                    if(objDA !=null)
                    {
                        objDA=null;
                    }
                }
            }
            return IntResultado;
        }
    }
}