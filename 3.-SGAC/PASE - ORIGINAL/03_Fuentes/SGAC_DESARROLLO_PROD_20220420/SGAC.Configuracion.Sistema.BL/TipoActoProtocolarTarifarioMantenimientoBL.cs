using System;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using System.Collections.Generic;

namespace SGAC.Configuracion.Sistema.BL
{
    public class TipoActoProtocolarTarifarioMantenimientoBL
    {
        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO InsertarTipoActoProtocolarTarifario(List<SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO> listaTipoActoProtocolarTarifario)
        {
            TipoActoProtocolarTarifarioMantenimientoDA objDA = new TipoActoProtocolarTarifarioMantenimientoDA();

            SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifarioBE = new SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();

            short intOficinaConsularId = listaTipoActoProtocolarTarifario[0].OficinaConsultar;
            short intUsuarioId = (Int16)listaTipoActoProtocolarTarifario[0].acta_sUsuarioCreacion;
                                    
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    for (int i = 0; i < listaTipoActoProtocolarTarifario.Count; i++)
                    {
                        objTipoActoProtocolarTarifarioBE = objDA.InsertarTipoActoProtocolarTarifario(listaTipoActoProtocolarTarifario[i]);
                        if (objTipoActoProtocolarTarifarioBE.Error)
                        {
                            break;
                        }
                    }

                    if (objTipoActoProtocolarTarifarioBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objTipoActoProtocolarTarifarioBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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


            return objTipoActoProtocolarTarifarioBE;
        }

        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO AnularTipoActoProtocolarTarifario(SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifario)
        {
            TipoActoProtocolarTarifarioMantenimientoDA objDA = new TipoActoProtocolarTarifarioMantenimientoDA();

            SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifarioBE = new SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();

            short intOficinaConsularId = objTipoActoProtocolarTarifario.OficinaConsultar;
            short intUsuarioId = (Int16)objTipoActoProtocolarTarifario.acta_sUsuarioModificacion;
                        
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objTipoActoProtocolarTarifarioBE = objDA.AnularTipoActoProtocolarTarifario(objTipoActoProtocolarTarifario);

                    if (objTipoActoProtocolarTarifarioBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = (Int16)intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objTipoActoProtocolarTarifarioBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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

            return objTipoActoProtocolarTarifarioBE;
        }

        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO AnularTipoActoProtocolarTarifarioTodos(SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifario)
        {
            TipoActoProtocolarTarifarioMantenimientoDA objDA = new TipoActoProtocolarTarifarioMantenimientoDA();

            SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifarioBE = new SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO();

            short intOficinaConsularId = objTipoActoProtocolarTarifario.OficinaConsultar;
            short intUsuarioId = (Int16)objTipoActoProtocolarTarifario.acta_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objTipoActoProtocolarTarifarioBE = objDA.AnularTipoActoProtocolarTarifarioTodos(objTipoActoProtocolarTarifario);

                    if (objTipoActoProtocolarTarifarioBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = (Int16)intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objTipoActoProtocolarTarifarioBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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

            return objTipoActoProtocolarTarifarioBE;
        }

    }
}
