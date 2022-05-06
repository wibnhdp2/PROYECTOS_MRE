using System;
using System.Data;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class TipoCambioMantenimientoBL
    {
        public int Insert(SI_TIPOCAMBIO_CONSULAR pobjBE)
        {
            DA.TipoCambioMantenimientoDA objDA=new TipoCambioMantenimientoDA();

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
                        audi_sOficinaConsularId = (Int16)pobjBE.tico_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tico_sUsuarioCreacion,
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

         public int Update(SI_TIPOCAMBIO_CONSULAR pobjBE)
        {
            DA.TipoCambioMantenimientoDA objDA=new DA.TipoCambioMantenimientoDA();

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
                        audi_sOficinaConsularId = (Int16)pobjBE.tico_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBE.tico_sUsuarioModificacion,
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
         
         public int Delete(SI_TIPOCAMBIO_CONSULAR pobjBE)
         {
             DA.TipoCambioMantenimientoDA objDA = new TipoCambioMantenimientoDA();

             int IntResultado = 0;

             using (TransactionScope tran = new TransactionScope())
             {
                 try
                 {
                     IntResultado = objDA.Delete(pobjBE);
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
                         audi_sOficinaConsularId = (Int16)pobjBE.tico_sOficinaConsularId,
                         audi_vComentario = "",
                         audi_vMensaje = ex.StackTrace.ToString(),
                         audi_vHostName = Util.ObtenerHostName(),
                         audi_sUsuarioCreacion = (Int16)pobjBE.tico_sUsuarioModificacion,
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
