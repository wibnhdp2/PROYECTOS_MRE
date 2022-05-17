using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;
using SGAC.BE;
using SGAC.Accesorios;
using System.Transactions;
using SGAC.DA.MRE.ACTUACION;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionMantenimientoBL
    {

        public string strMensajeError = string.Empty;

        public int InsertarDesdeJudicial(RE_ACTUACION ObjActuacBE, ref Int64 intActuacionId, ref List<RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE, int intsOficinaConsularId, string strHostName, DataTable DTRePagos)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.InsertarDesdeJudicial(ObjActuacBE, ref intActuacionId, ref LIS_RE_ACTUACIONDETALLE, intsOficinaConsularId, strHostName, DTRePagos);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
                scope.Complete();
                scope.Dispose();
            }
        }

        public int ModificarDesdeJudicial(RE_ACTUACION ObjActuacBE, ref Int64 intActuacionId, ref List<RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE, int intsOficinaConsularId, string strHostName, DataTable DTRePagos)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.ModificarDesdeJudicial(ObjActuacBE, ref intActuacionId, ref LIS_RE_ACTUACIONDETALLE, intsOficinaConsularId, strHostName, DTRePagos);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
                scope.Complete();
                scope.Dispose();
            }
        }
        public BE.MRE.RE_ACTUACION Insertar(BE.MRE.RE_ACTUACION Actuacion)
        {
            RE_ACTUACION_DA lACTUACION_DA = new RE_ACTUACION_DA();
            return lACTUACION_DA.insertar(Actuacion);
        }

        public int Insertar(RE_ACTUACION ObjActuacBE,
                             DataTable DtDetActuaciones,
                             RE_PAGO ObjPagoBE,
                             ref long LonActuacionId)
        {
            bool bolCancelar = false;
            int intResultado = 0;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                intResultado = objDA.Insertar(ObjActuacBE,
                                   DtDetActuaciones,
                                   ObjPagoBE,
                                   ref LonActuacionId);
            }
            catch (Exception ex)
            {
                intResultado = -1;
                Transaction.Current.Rollback();
                bolCancelar = true;
                throw ex;
            }
            finally
            {
                objDA = null;
                if (!bolCancelar)
                {
                    scope.Complete();
                    scope.Dispose();
                }
            }
            return intResultado;
        }

        public int Actualizar(RE_ACTUACION ObjActuacBE,
                              RE_ACTUACIONDETALLE ObjActuacDetBE,Int16 clasificacionTarifa = 0)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.Actualizar(ObjActuacBE, ObjActuacDetBE,clasificacionTarifa);
            }
            catch (SGACExcepcion ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        public int EliminarParticipante(Int64 Participante)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.EliminarParticipante(Participante);
            }
            catch (SGACExcepcion ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        public int Reasignar(RE_ACTUACION ObjActuacBE,Int16 sTarifarioID, ref string strMensaje,Int64 acde_iActuacionDetalleId = 0)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.Reasignar(ObjActuacBE, sTarifarioID, ref strMensaje, acde_iActuacionDetalleId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public int ActualizarEstado(RE_ACTUACION ObjActuacBE,
                                    RE_ACTUACIONDETALLE ObjActuacDetBE)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.ActualizarEstado(ObjActuacBE,
                                           ObjActuacDetBE);
            }
            catch (SGACExcepcion ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        //---------------------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 09/09/2016
        // Objetivo: Se eliminan los parametros: string vNombreAutorizador, string vComentarios
        //           esta implicito en la clase ObjActuacDetBE.
        //---------------------------------------------------------------------------------------

        public int Anular(RE_ACTUACION ObjActuacBE,
                          RE_ACTUACIONDETALLE ObjActuacDetBE)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.Anular(ObjActuacBE, ObjActuacDetBE);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
                scope.Complete();
                scope.Dispose();
            }
        }

        #region Vinculación
        public int VincularAutoadhesivo(Int64 intActuacionId, Int64 intActuacionDetalleId,
            int intInsumoTipoId,
            string strCodAutoadhesivo, DateTime datFechaVinculacion, bool bolImpreso,
            DateTime datFechaImpresion, int intFuncionarioImprimeId,
            int intOficinaConsularId, int intUsuarioModificacionId, ref string strMensaje, Enumerador.enmNotarialTipoFormato enmNotarialFormato = Accesorios.Enumerador.enmNotarialTipoFormato.OTROS, Int16 codigoCiudadItinerante = 0)
        {
            bool bolCancelar = false;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);

            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            int intResultado = 0;
            try
            {
                if (enmNotarialFormato == Accesorios.Enumerador.enmNotarialTipoFormato.ESCRITURA)
                {
                    #region Escritura - Vincular Varias Tarifas
                    intResultado = objDA.VincularAutoadhesivo(intActuacionId, intActuacionDetalleId, intInsumoTipoId,
                                    strCodAutoadhesivo, datFechaVinculacion, bolImpreso, datFechaImpresion, intFuncionarioImprimeId,
                                    intOficinaConsularId, intUsuarioModificacionId, ref strMensaje);

                    if (intResultado > 0)
                    {
                        RE_ACTUACIONINSUMODETALLE_DA objInsumoDetalleDA = new RE_ACTUACIONINSUMODETALLE_DA();
                        
                        DataTable dt = objDA.ObtenerActuacionDetalleDeActuacion(intActuacionId);

                        // 1. Buscar iInsumoId
                        string strTarifa = "";
                        Int64 intInsumoId = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            strTarifa = Convert.ToString(dr["tari_vNumero"]).ToUpper();                            
                            if (strTarifa != Constantes.CONST_EXCEPCION_TARIFA_17A &&
                                strTarifa != Constantes.CONST_EXCEPCION_TARIFA_17B &&
                                strTarifa != Constantes.CONST_EXCEPCION_TARIFA_17C &&
                                Convert.ToInt64(dr["insu_iInsumoId"]) != 0)
                            {
                                intInsumoId = Convert.ToInt64(dr["insu_iInsumoId"]);
                                break;
                            }
                        }
                        // 2. Vincular tarifas de Escritura pública
                        if (intInsumoId != 0)
                        {
                            BE.MRE.RE_ACTUACIONINSUMODETALLE ActuacionInsumoDetalle = new BE.MRE.RE_ACTUACIONINSUMODETALLE();
                            foreach (DataRow dr in dt.Rows)
                            {
                                strTarifa = dr["tari_vNumero"].ToString().ToUpper();
                                if (strTarifa != Constantes.CONST_EXCEPCION_TARIFA_17A &&
                                    strTarifa != Constantes.CONST_EXCEPCION_TARIFA_17B &&
                                    strTarifa != Constantes.CONST_EXCEPCION_TARIFA_17C &&
                                    Convert.ToInt64(dr["insu_iInsumoId"]) == 0)
                                {
                                    ActuacionInsumoDetalle.aide_iActuacionDetalleId = Convert.ToInt64(dr["acde_iActuacionDetalleId"]);
                                    ActuacionInsumoDetalle.aide_iInsumoId = intInsumoId;
                                    ActuacionInsumoDetalle.aide_dFechaVinculacion = datFechaVinculacion;
                                    ActuacionInsumoDetalle.aide_sUsuarioVinculacionId = Convert.ToInt16(intUsuarioModificacionId);
                                    ActuacionInsumoDetalle.aide_bFlagImpresion = bolImpreso;
                                    ActuacionInsumoDetalle.aide_dFechaImpresion = datFechaImpresion;
                                    ActuacionInsumoDetalle.aide_cEstado = "A";
                                    ActuacionInsumoDetalle.aide_sUsuarioCreacion = Convert.ToInt16(intUsuarioModificacionId);
                                    ActuacionInsumoDetalle.aide_vIPCreacion = Util.ObtenerDireccionIP();
                                    ActuacionInsumoDetalle.OficinaConsultar = Convert.ToInt16(intOficinaConsularId);
                                    
                                    ActuacionInsumoDetalle = objInsumoDetalleDA.insertar(ActuacionInsumoDetalle);

                                    if (ActuacionInsumoDetalle.Error)
                                    {
                                        bolCancelar = true;
                                        throw new Exception("Ocurrió un Error en la ejecución.");
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    intResultado = objDA.VincularAutoadhesivo(intActuacionId, intActuacionDetalleId, intInsumoTipoId,
                        strCodAutoadhesivo, datFechaVinculacion, bolImpreso, datFechaImpresion, intFuncionarioImprimeId,
                        intOficinaConsularId, intUsuarioModificacionId, ref strMensaje, codigoCiudadItinerante);

                    if (enmNotarialFormato == Accesorios.Enumerador.enmNotarialTipoFormato.PARTE)
                    {
                        #region Parte: Actualizar datos 
                        // RE_ACTONOTARIALDETALLE -> Nro. Oficio
                        #endregion
                    }
                }                               
            }
            catch (Exception ex)
            {
                strMensaje = ex.Message;                
                Transaction.Current.Rollback();
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;

                if (!bolCancelar)
                {
                    scope.Complete();
                    scope.Dispose();
                }
            }
            return intResultado;
        }

        public int USP_RE_ACTUACIONINSUMODETALLE_ACTUALIZAR_IMPRESION(Int64 iActuacionInsumoDetalleId, Boolean bFlagImpresion, Int16 aide_sUsuarioCreacion, Int16 sOficinaConsular, ref string strMensaje)
        { 
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            return objDA.USP_RE_ACTUACIONINSUMODETALLE_ACTUALIZAR_IMPRESION(iActuacionInsumoDetalleId, bFlagImpresion,aide_sUsuarioCreacion,sOficinaConsular, ref strMensaje);
        }

        public int VincularAutoadhesivoNotarialProtocolar(List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> lRE_ACTUACIONDETALLE, ref string strMensaje)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            bool lCancel = false;
            int intResultado = 0;
            String Mensaje = String.Empty;
            //var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    int contador = 1;
                    foreach (SGAC.BE.MRE.RE_ACTUACIONDETALLE oRE_ACTUACIONDETALLE in lRE_ACTUACIONDETALLE)
                    {
                        intResultado = objDA.VincularAutoadhesivo(oRE_ACTUACIONDETALLE.acde_iActuacionId, oRE_ACTUACIONDETALLE.acde_iActuacionDetalleId, oRE_ACTUACIONDETALLE.intInsumoTipoId,
                         oRE_ACTUACIONDETALLE.Insumo,
                         oRE_ACTUACIONDETALLE.acde_dFechaCreacion, oRE_ACTUACIONDETALLE.acde_bRequisitosFlag, oRE_ACTUACIONDETALLE.acde_dFechaCreacion, oRE_ACTUACIONDETALLE.acde_IImpresionFuncionarioId,
                         oRE_ACTUACIONDETALLE.sOficinaConsultaID, oRE_ACTUACIONDETALLE.acde_sUsuarioCreacion,
                         ref strMensaje);

                        if (contador == 1)
                        {
                            if (strMensaje != String.Empty)
                                Mensaje += strMensaje;
                        }
                        else {
                            if (strMensaje != String.Empty)
                                Mensaje += "<br>" + strMensaje;
                        }
                        contador++;
                    }

                    if (Mensaje == String.Empty)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                        Transaction.Current.Rollback();
                        scope.Dispose();
                    }
                }
                strMensaje = Mensaje;
                return intResultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
        
        public int Vincular_Insumo(Int64 intActuacionId, Int64 intActuacionDetalleId,
            int intInsumoAutoadhesivoId,
            int intInsumoLaminaId,
            int intInsumoPassaporte,
            string strCodAutoadhesivo, string strCodLamina, string strCodPassaporte,DateTime datFechaVinculacion, bool bolImpreso,
            DateTime datFechaImpresion, int intFuncionarioImprimeId,
            int intOficinaConsularId, int intUsuarioModificacionId, string str_Lamina, string str_Librillo, ref string strMensaje)
        {
            int i_Resultado = 0;
            string s_Mensaje_1 = string.Empty;
            string s_Mensaje_2 = string.Empty;
            string s_Mensaje_3 = string.Empty;

            strMensaje = "";
            int n2 = 0;
            int n3 = 0;
            bool b_Insumo_1 = false;
            bool b_Insumo_2 = false;
            bool b_Insumo_3 = false;

            #region - Validando si ys se encontro registrado -
            var obj_existe = new ActuacionMantenimientoDA().Obtener_ActuacionInsumoDetalle(intActuacionDetalleId,
                1, 10, ref n2, ref n3);

            n2 = 0;
            n3 = 0;
            
            foreach (DataRow row in obj_existe.Rows)
            {
                if (Convert.ToInt32(row["TipoInsumo"]) == intInsumoPassaporte)
                    b_Insumo_3 = true;

                if (Convert.ToInt32(row["TipoInsumo"]) == intInsumoLaminaId)
                    b_Insumo_2 = true;
                if (Convert.ToInt32(row["TipoInsumo"]) == intInsumoAutoadhesivoId)
                    b_Insumo_1 = true;
                
            }
            obj_existe.Dispose();
            #endregion

            using (TransactionScope scope = new System.Transactions.TransactionScope(TransactionScopeOption.RequiresNew))
            {
                ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
                try
                {
                    /*Valiando que existe en migratorio*/

                    if (!b_Insumo_1)
                    {
                        if (strCodAutoadhesivo.Trim() != string.Empty)
                        {
                            i_Resultado = objDA.Vincular_Insumo_Migratorio(
                                intActuacionId, intActuacionDetalleId, intInsumoAutoadhesivoId,
                                strCodAutoadhesivo,
                                datFechaVinculacion, bolImpreso, datFechaImpresion, intFuncionarioImprimeId,
                                intOficinaConsularId, intUsuarioModificacionId, "",
                                ref s_Mensaje_1);
                        }
                    }

                    if (!b_Insumo_2)
                    {
                        if (strCodLamina.Trim() != string.Empty)
                        {
                            i_Resultado = objDA.Vincular_Insumo_Migratorio(
                                intActuacionId, intActuacionDetalleId, intInsumoLaminaId,
                                strCodLamina,
                                datFechaVinculacion, bolImpreso, datFechaImpresion, intFuncionarioImprimeId,
                                intOficinaConsularId, intUsuarioModificacionId, str_Lamina,
                                ref s_Mensaje_2);
                        }
                    }

                    if (intInsumoPassaporte > 0)
                    {
                        if (!b_Insumo_3)
                        {
                            if (strCodPassaporte.Trim() != string.Empty)
                            {
                                i_Resultado = objDA.Vincular_Insumo_Migratorio(
                                    intActuacionId, intActuacionDetalleId, intInsumoPassaporte,
                                    strCodPassaporte,
                                    datFechaVinculacion, bolImpreso, datFechaImpresion, intFuncionarioImprimeId,
                                    intOficinaConsularId, intUsuarioModificacionId, str_Librillo,
                                    ref s_Mensaje_3);
                            }
                        }
                    }


                    if (string.IsNullOrEmpty(s_Mensaje_1) && string.IsNullOrEmpty(s_Mensaje_2) && string.IsNullOrEmpty(s_Mensaje_3))
                        scope.Complete();
                    else
                    {
                        if (s_Mensaje_1 != "") strMensaje += "[Autoadhesivo] " + s_Mensaje_1 + " ";
                        if (s_Mensaje_2 != "") strMensaje += "[Lámina] " + s_Mensaje_2;
                        if (s_Mensaje_3 != "") strMensaje += "[Librillo] " + s_Mensaje_3;
                        scope.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    strMensaje += "Error: " + ex.Message;
                    scope.Dispose();
                }
                finally
                {
                    objDA = null;
                }
            }
            return i_Resultado;
        }
        #endregion

        public string ObtenerFormularioPorTarifa(Int16 intTarifaId, int intAccionId)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            return objDA.ObtenerFormularioPorTarifa(intTarifaId, intAccionId);
        }

        public DataTable Obtener_ActuacionInsumoDetalle(Int64 iActuacionDetalleId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            return objDA.Obtener_ActuacionInsumoDetalle(iActuacionDetalleId, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
        }

        public DataTable Obtener_ActuacionInsumoDetalleAll(Int64 iActuacionDetalleId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            return objDA.Obtener_ActuacionInsumoDetalleAll(iActuacionDetalleId, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
        }

        public DataTable Obtener_ActuacionInsumo(Int64 iActuacionId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            return objDA.Obtener_ActuacionInsumo(iActuacionId, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstActuacionDetalle"></param>
        /// <param name="lstPago"></param>
        /// <returns></returns>
        public int InsertarActuacionDetalle(List<BE.RE_ACTUACIONDETALLE> lstActuacionDetalle, List<BE.RE_PAGO> lstPago)
        {
            int intResultado = 0;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                int indice = 0;
                Int64 intActuacionDetalleId = 0;
                foreach (BE.RE_ACTUACIONDETALLE objActuacionDetalle in lstActuacionDetalle)
                {
                    intActuacionDetalleId = objDA.InsertarActuacionDetalle(objActuacionDetalle);
                    
                    lstPago[indice].pago_iActuacionDetalleId = intActuacionDetalleId;

                    objDA.InsertarPago(lstPago[indice]);
                    indice++;
                }
            }
            catch
            {
                Transaction.Current.Rollback();
                intResultado = -1;
            }
            finally
            {
                objDA = null;
                scope.Complete();
                scope.Dispose();
            }

            return intResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstActuacionDetalle"></param>
        /// <param name="lstPago"></param>
        /// <param name="lonActuacionDetalle"></param>
        /// <returns></returns>
        public int InsertarActuacionDetalle(List<BE.RE_ACTUACIONDETALLE> lstActuacionDetalle, List<BE.RE_PAGO> lstPago, ref  long lonActuacionDetalle,
            BE.MRE.RE_ACTONOTARIAL ActoNotarial = null, Int16 intFojasParte = 0, Int16 intFojasEscritura = 0)
        {
            int intResultado = 0;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();

            try
            {
                int indice = 0;
                Int64 intActuacionDetalleId = 0;

                // Objetos para la inserción de formato de acto notarial
                ActoNotarialMantenimiento objBL = new ActoNotarialMantenimiento();
                BE.MRE.RE_ACTONOTARIALDETALLE ActoNotarialDetalle;
                int intCantEscritura = 0;
                // --
                foreach (BE.RE_ACTUACIONDETALLE objActuacionDetalle in lstActuacionDetalle)
                {
                    //1. Inserta Actuación Detalle
                    intActuacionDetalleId = objDA.InsertarActuacionDetalle(objActuacionDetalle);

                    lstPago[indice].pago_iActuacionDetalleId = intActuacionDetalleId;
                    lonActuacionDetalle = intActuacionDetalleId;
                    //2. Inserta Pago
                    objDA.InsertarPago(lstPago[indice]);

                    //3. Inserta Formato
                    #region Insertar Formatos Notariales : parte y testimonio
                    if (ActoNotarial != null)
                    {
                        if (objActuacionDetalle.acde_sTarifarioId == Constantes.CONST_PROTOCOLAR_ID_TARIFA_17A ||
                            objActuacionDetalle.acde_sTarifarioId == Constantes.CONST_PROTOCOLAR_ID_TARIFA_17B ||
                            objActuacionDetalle.acde_sTarifarioId == Constantes.CONST_PROTOCOLAR_ID_TARIFA_17C)
                        {
                            ActoNotarialDetalle = new BE.MRE.RE_ACTONOTARIALDETALLE();
                            ActoNotarialDetalle.ande_iActuacionDetalleId = intActuacionDetalleId;
                            ActoNotarialDetalle.ande_sOficinaConsularId = objActuacionDetalle.OficinaConsularId;
                            if (objActuacionDetalle.acde_sTarifarioId == Constantes.CONST_PROTOCOLAR_ID_TARIFA_17A)
                            {
                                ActoNotarialDetalle.ande_sTipoFormatoId = (int)Enumerador.enmNotarialTipoFormato.TESTIMONIO;
                                ActoNotarialDetalle.ande_sNumeroFoja = Convert.ToInt16(intFojasEscritura + 1);
                            }
                            else
                            {
                                ActoNotarialDetalle.ande_sTipoFormatoId = (int)Enumerador.enmNotarialTipoFormato.PARTE;
                                ActoNotarialDetalle.ande_sNumeroFoja = intFojasParte;
                            }
                            ActoNotarialDetalle.ande_IFuncionarioAutorizadorId = ActoNotarial.acno_IFuncionarioAutorizadorId;

                            if (objActuacionDetalle.acde_sTarifarioId != Constantes.CONST_PROTOCOLAR_ID_TARIFA_17A)
                                ActoNotarialDetalle.ande_vNumeroOficio = ActoNotarial.acno_vNumeroOficio;

                            ActoNotarialDetalle.ande_vPresentanteNombre = ActoNotarial.acno_vPresentanteNombre;
                            ActoNotarialDetalle.ande_sPresentanteTipoDocumento = ActoNotarial.acno_sPresentanteTipoDocumento;
                            ActoNotarialDetalle.ande_vPresentanteNumeroDocumento = ActoNotarial.acno_vPresentanteNumeroDocumento;
                            ActoNotarialDetalle.ande_sPresentanteGenero = ActoNotarial.acno_sPresentanteGenero;
                            ActoNotarialDetalle.ande_dFechaExtension = ActoNotarial.acno_dFechaExtension;                            
                            ActoNotarialDetalle.ande_sUsuarioCreacion = objActuacionDetalle.acde_sUsuarioCreacion;
                            ActoNotarialDetalle.ande_vIPCreacion = objActuacionDetalle.acde_vIPCreacion;

                            ActoNotarialDetalle = objBL.InsertarActoNotarialDetalle(ActoNotarialDetalle, objActuacionDetalle.acde_iActuacionId);
                            if (ActoNotarialDetalle.Error)
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            intCantEscritura++;
                            if (intCantEscritura == 1)
                            {
                                // Tomo el primer identificador de la actuación detalle
                                ActoNotarialDetalle = new BE.MRE.RE_ACTONOTARIALDETALLE();
                                ActoNotarialDetalle.ande_iActuacionDetalleId = intActuacionDetalleId;
                                ActoNotarialDetalle.ande_sOficinaConsularId = objActuacionDetalle.OficinaConsularId;
                                ActoNotarialDetalle.ande_sTipoFormatoId = (int)Enumerador.enmNotarialTipoFormato.ESCRITURA;
                                ActoNotarialDetalle.ande_IFuncionarioAutorizadorId = ActoNotarial.acno_IFuncionarioAutorizadorId;
                                ActoNotarialDetalle.ande_sNumeroFoja = intFojasEscritura;
                                ActoNotarialDetalle.ande_vNumeroOficio = "";
                                ActoNotarialDetalle.ande_vPresentanteNombre = ActoNotarial.acno_vPresentanteNombre;
                                ActoNotarialDetalle.ande_sPresentanteTipoDocumento = ActoNotarial.acno_sPresentanteTipoDocumento;
                                ActoNotarialDetalle.ande_vPresentanteNumeroDocumento = ActoNotarial.acno_vPresentanteNumeroDocumento;
                                ActoNotarialDetalle.ande_sPresentanteGenero = ActoNotarial.acno_sPresentanteGenero;
                                ActoNotarialDetalle.ande_dFechaExtension = ActoNotarial.acno_dFechaExtension;
                                ActoNotarialDetalle.ande_sUsuarioCreacion = objActuacionDetalle.acde_sUsuarioCreacion;
                                ActoNotarialDetalle.ande_vIPCreacion = objActuacionDetalle.acde_vIPCreacion;

                                ActoNotarialDetalle = objBL.InsertarActoNotarialDetalle(ActoNotarialDetalle, objActuacionDetalle.acde_iActuacionId);

                                if (ActoNotarialDetalle.Error)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                    #endregion

                    indice++;
                }
            }
            catch
            {
                Transaction.Current.Rollback();
                intResultado = -1;
            }
            finally
            {
                objDA = null;
                scope.Complete();
                scope.Dispose();
            }

            return intResultado;
        }

        // IDM - Vincular varias actuaciones
        public void VincularProtocolar(Int64 intPersonaId, Int64 intActuacionId, Int64 intActuacionDetalleId, 
            int intTipoInsumoId, string strCodigoInsumo,
            int intOficinaConsular, int intUsuarioId, ref string strMensaje)
        {
            bool bolCancelar = false;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            //var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            ActuacionConsultaDA objConsultaDA = new ActuacionConsultaDA();
            ActuacionMantenimientoDA objMantenimientoDA = new ActuacionMantenimientoDA();

            BE.MRE.RE_ACTUACIONINSUMODETALLE objActuacionInsumoDetalle;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                // Vinculación actuación Detalle
                objMantenimientoDA.VincularAutoadhesivo(intActuacionId, intActuacionDetalleId, intTipoInsumoId,
                    strCodigoInsumo, DateTime.Now, true, DateTime.Now, 0, intOficinaConsular, intUsuarioId,
                    ref strMensaje);

                if (!strMensaje.Equals(""))
                {
                    bolCancelar = true;
                }
                else
                {
                    // MÁS DE UNA TARIFA
                    Int64 intInsumoId = 0;
                    DataTable dtActuacionDetalle = objConsultaDA.ObtenerActuacionDetalle_Actuacion(intPersonaId, intActuacionId);
                    if (dtActuacionDetalle.Rows.Count > 1)
                    {
                        // Búsqueda Identificador Insumo Vinculado
                        foreach (DataRow dr in dtActuacionDetalle.Rows)
                        {
                            if (!dr["vCodigoInsumo"].ToString().Equals(""))
                            {
                                intInsumoId = Convert.ToInt64(dr["aide_iInsumoId"]);
                                break;
                            }
                        }

                        if (intInsumoId != 0)
                        {
                            foreach (DataRow dr in dtActuacionDetalle.Rows)
                            {
                                if (dr["vCodigoInsumo"].ToString().Equals(""))
                                {
                                    objActuacionInsumoDetalle = new BE.MRE.RE_ACTUACIONINSUMODETALLE();
                                    objActuacionInsumoDetalle.aide_iActuacionDetalleId = Convert.ToInt64(dr["iActuacionDetalleId"]);
                                    objActuacionInsumoDetalle.aide_iInsumoId = intInsumoId;
                                    objActuacionInsumoDetalle.aide_dFechaVinculacion = DateTime.Now;
                                    objActuacionInsumoDetalle.aide_sUsuarioVinculacionId = Convert.ToInt16(intUsuarioId);
                                    objActuacionInsumoDetalle.aide_bFlagImpresion = true;
                                    objActuacionInsumoDetalle.aide_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                                    objActuacionInsumoDetalle.aide_sUsuarioCreacion = Convert.ToInt16(intUsuarioId);
                                    objActuacionInsumoDetalle.aide_dFechaCreacion = DateTime.Now;
                                    objActuacionInsumoDetalle.aide_vIPCreacion = Util.ObtenerDireccionIP();
                                    objActuacionInsumoDetalle.OficinaConsultar = Convert.ToInt16(intOficinaConsular);
                                    objActuacionInsumoDetalle.HostName = Util.ObtenerHostName();

                                    objActuacionInsumoDetalle = objMantenimientoDA.InsertarVinculacionInsumo(objActuacionInsumoDetalle);
                                    if (objActuacionInsumoDetalle.Error || objActuacionInsumoDetalle.aide_iActuacionInsumoDetalleId == 0)
                                    {
                                        bolCancelar = true;
                                        break;
                                    }
                                }
                            }
                        }                        
                    }
                }
                
                if (!bolCancelar)
                {
                    scope.Complete();
                }
                else
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }            
        }

        public string ActualizarTarifarioCorrelativo(List<SGAC.BE.MRE.RE_CORRELATIVO> listCorrelativo)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            int intResultado = 0;
            String Mensaje = String.Empty;
           // var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {

                    foreach (SGAC.BE.MRE.RE_CORRELATIVO correlativo in listCorrelativo)
                    {
                        Mensaje = objDA.ActualizarTarifarioCorrelativo(correlativo, ref intResultado);

                        if (intResultado == 0)
                        {
                            Mensaje = objDA.strError;
                            break;
                        }
                        
                    }

                    if (Mensaje == String.Empty)
                    {
                        scope.Complete();
                    }
                    else
                    {

                        intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                        Transaction.Current.Rollback();
                        strMensajeError = Mensaje;
                        scope.Dispose();
                    }

                }

                return Mensaje;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        //---------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Actualiza Flag de envio de correo
        //---------------------------------------------------------------------------------------
        public int ActualizarFlagEnvioCorreo(RE_ACTUACION ObjActuacBE)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.ActualizarFlagEnvioCorreo(ObjActuacBE);
            }
            catch (SGACExcepcion ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable Verificar_FichaRegistral(Int64 iActuacionDetalleId)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            return objDA.Verificar_FichaRegistral(iActuacionDetalleId);
        }

        public int ReactivarActuacion(Int64 acde_iActuacionDetalleId, Int64 acde_iActuacionId, Int16 SUSUARIOMODIFICACION, Int16 SOFICINACONSULARID,string observacion)
        {
            ActuacionMantenimientoDA objDA = new ActuacionMantenimientoDA();
            try
            {
                return objDA.ReactivarActuacion(acde_iActuacionDetalleId, acde_iActuacionId, SUSUARIOMODIFICACION, SOFICINACONSULARID,observacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
    }
}
