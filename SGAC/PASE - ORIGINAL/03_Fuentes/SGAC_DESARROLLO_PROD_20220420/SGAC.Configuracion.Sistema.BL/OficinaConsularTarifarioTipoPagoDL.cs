using System;
using System.Data;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.Accesorios;
using System.Collections.Generic;

namespace SGAC.Configuracion.Sistema.BL
{

    public class OficinaConsularTarifarioTipoPagoDL
    {
        public DataTable Consultar(Int16 intOficinaConsularId, Int16 intTipoPagoId, string strTarifaLetra, bool bExcepcion,
            int intPageSize, int intPageNumber, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            OficinaConsularTarifarioTipoPagoDA objDA = new OficinaConsularTarifarioTipoPagoDA();

            try
            {
                return objDA.Consultar(intOficinaConsularId, intTipoPagoId, strTarifaLetra, bExcepcion, intPageSize, intPageNumber, strContar, ref IntTotalCount, ref IntTotalPages);
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

        public SI_OFICINA_TARIFA_TIPO_PAGO Insertar(List<SI_OFICINA_TARIFA_TIPO_PAGO> listaOficinaTarifaTipoPago)
        {
            OficinaConsularTarifarioTipoPagoDA objDA = new OficinaConsularTarifarioTipoPagoDA();

            SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPagoBE = new SI_OFICINA_TARIFA_TIPO_PAGO();

            Int16 intOficinaConsularId = listaOficinaTarifaTipoPago[0].OficinaConsultar;
            Int16 intUsuarioId = (Int16)listaOficinaTarifaTipoPago[0].ofpa_sUsuarioCreacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    for (int i = 0; i < listaOficinaTarifaTipoPago.Count; i++)
                    {
                        objOficinaTarifaTipoPagoBE = objDA.Insertar(listaOficinaTarifaTipoPago[i]);
                        if (objOficinaTarifaTipoPagoBE.Error)
                        {
                            break;
                        }
                    }

                    if (objOficinaTarifaTipoPagoBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objOficinaTarifaTipoPagoBE.Message,
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
            return objOficinaTarifaTipoPagoBE;
        }

        public SI_OFICINA_TARIFA_TIPO_PAGO Actualizar(List<SI_OFICINA_TARIFA_TIPO_PAGO> listaOficinaTarifaTipoPago)
        {
            OficinaConsularTarifarioTipoPagoDA objDA = new OficinaConsularTarifarioTipoPagoDA();

            SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPagoBE = new SI_OFICINA_TARIFA_TIPO_PAGO();

            Int16 intOficinaConsularId = listaOficinaTarifaTipoPago[0].OficinaConsultar;
            Int16 intUsuarioId = (Int16)listaOficinaTarifaTipoPago[0].ofpa_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    for (int i = 0; i < listaOficinaTarifaTipoPago.Count; i++)
                    {
                        objOficinaTarifaTipoPagoBE = objDA.Actualizar(listaOficinaTarifaTipoPago[i]);
                        if (objOficinaTarifaTipoPagoBE.Error)
                        {
                            break;
                        }
                    }

                    if (objOficinaTarifaTipoPagoBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objOficinaTarifaTipoPagoBE.Message,
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
            return objOficinaTarifaTipoPagoBE;
        }

        public SI_OFICINA_TARIFA_TIPO_PAGO Anular(SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPago)
        {
            OficinaConsularTarifarioTipoPagoDA objDA = new OficinaConsularTarifarioTipoPagoDA();

            SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPagoBE = new SI_OFICINA_TARIFA_TIPO_PAGO();

            Int16 intOficinaConsularId = objOficinaTarifaTipoPago.OficinaConsultar;
            Int16 intUsuarioId = (Int16)objOficinaTarifaTipoPago.ofpa_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objOficinaTarifaTipoPagoBE = objDA.Anular(objOficinaTarifaTipoPago);

                    if (objOficinaTarifaTipoPagoBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objOficinaTarifaTipoPagoBE.Message,
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
            return objOficinaTarifaTipoPagoBE;
        }
    }
}
