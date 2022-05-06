using System;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class PaisMantenimientoBL
    {

        public Int64 Insertar(SI_PAIS pobjBe, int intOficinaConsular)
        {
            DA.PaisMantenimientoDA objDA = new PaisMantenimientoDA();

            Int64 intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            string s_Mensaje = string.Empty;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Insertar(pobjBe, intOficinaConsular, ref s_Mensaje);

                    if (s_Mensaje != string.Empty)
                    {
                    //    Transaction.Current.Rollback();
                        scope.Dispose();

                    }
                    else
                    {
                        scope.Complete();
                    }

                }
                catch (Exception ex)
                {
                    intResultado = new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)intOficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBe.pais_sUsuarioCreacion,
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

            if (s_Mensaje != string.Empty)
            {

                intResultado = new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)intOficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = s_Mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)pobjBe.pais_sUsuarioCreacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

            }


            return intResultado;
        }

        public int Actualizar(SI_PAIS pobjBe, int intOficinaConsular)
        {
            DA.PaisMantenimientoDA objDA = new PaisMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Actualizar(pobjBe, intOficinaConsular);
                    scope.Complete();


                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)intOficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBe.pais_sUsuarioModificacion,
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

        public int Anular(SI_PAIS pobjBe, int intOficinaConsular)
        {
            DA.PaisMantenimientoDA objDA = new PaisMantenimientoDA();

            int intResultado = 0;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResultado = objDA.Anular(pobjBe, intOficinaConsular);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)intOficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = ex.StackTrace.ToString(),
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)pobjBe.pais_sUsuarioModificacion,
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
