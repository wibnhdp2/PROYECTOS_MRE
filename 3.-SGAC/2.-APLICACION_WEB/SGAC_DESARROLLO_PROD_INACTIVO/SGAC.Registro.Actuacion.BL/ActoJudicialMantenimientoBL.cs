using System;
using System.Collections.Generic;
using SGAC.Registro.Actuacion.DA;
using System.Data;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoJudicialMantenimientoBL
    {
        string strMensajeError = "";

        bool AgregarPersonaJuridica(ref DataTable dtParticipantes, Int16 sOficinaConsularId, string vHostName, Int16 intUsuarioCreacionId, string vIPCreacion, int intFila)
        {
            bool booResult = false;
            SGAC.BE.MRE.RE_EMPRESA RE_EMPRESA = new SGAC.BE.MRE.RE_EMPRESA();

            RE_EMPRESA.empr_iEmpresaId = 0;
            RE_EMPRESA.empr_sTipoEmpresaId = Convert.ToInt16(Enumerador.enmTipoEmpresa.PRIVADO);             // LE INDICAMOS QUE EL TIPO DE EMPRESA ES PRIVADO
            RE_EMPRESA.empr_sTipoDocumentoId = Convert.ToInt16(dtParticipantes.Rows[intFila]["ajpa_sDocumentoTipoId"].ToString());
            RE_EMPRESA.empr_vRazonSocial = dtParticipantes.Rows[intFila]["ajpa_vNombre"].ToString();
            RE_EMPRESA.empr_vNumeroDocumento = dtParticipantes.Rows[intFila]["ajpa_vDocumentoNumero"].ToString();
            RE_EMPRESA.empr_vActividadComercial = "";
            RE_EMPRESA.empr_vTelefono = "";
            RE_EMPRESA.empr_vCorreo = "";
            RE_EMPRESA.empr_cEstado = "";
            RE_EMPRESA.empr_sUsuarioCreacion = intUsuarioCreacionId;
            RE_EMPRESA.empr_vIPCreacion = vIPCreacion;
            RE_EMPRESA.empr_dFechaCreacion = DateTime.Now;
            RE_EMPRESA.HostName = vHostName;
            RE_EMPRESA.OficinaConsultar = sOficinaConsularId;

            SGAC.DA.MRE.RE_EMPRESA_DA funEmpresa = new SGAC.DA.MRE.RE_EMPRESA_DA();
            RE_EMPRESA = funEmpresa.insertar(RE_EMPRESA);
            ValidacionError(RE_EMPRESA.Message, RE_EMPRESA.OficinaConsultar, RE_EMPRESA.empr_sUsuarioCreacion);

            if (RE_EMPRESA.Error == true)
            {
                booResult = false;
            }
            else
            {
                //dtParticipantes.Rows[intFila]["ajpa_iPersonaId"] = RE_EMPRESA.empr_iEmpresaId;
                dtParticipantes.Rows[intFila]["ajpa_iEmpresaId"] = RE_EMPRESA.empr_iEmpresaId;
                booResult = true;
            }

            return booResult;
        }


        public int Insertar_Actuacion(ref SGAC.BE.MRE.RE_ACTOJUDICIAL RE_ACTOJUDICIAL,
                            List<SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE> LIS_RE_ACTOJUDICIALPARTICIPANTE,
                            SGAC.BE.MRE.RE_ACTUACION RE_ACTUACION,
                            List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACUTACIONDETALLE,
                            List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO,
                            DataTable dtPersonas
                            )
        {
            #region DECLARACION DE VARIABLES
            Int16 intResultado = 0;
            int intFila = 0;
            int intFilaDet = 0;
            Int64 intActuacionID = 0;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            SGAC.DA.MRE.RE_PERSONA_DA funPersona = new SGAC.DA.MRE.RE_PERSONA_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTUACION_DA funActuacion = new SGAC.DA.MRE.ACTUACION.RE_ACTUACION_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTUACIONDETALLE_DA funActuacionDetalle = new SGAC.DA.MRE.ACTUACION.RE_ACTUACIONDETALLE_DA();
            SGAC.DA.MRE.ACTUACION.RE_PAGO_DA funPago = new SGAC.DA.MRE.ACTUACION.RE_PAGO_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTU_PAGO_DA funActuPago = new SGAC.DA.MRE.ACTUACION.RE_ACTU_PAGO_DA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL funActoJudicial = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA funActoParticipante = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            #endregion

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
                {
                    #region INSERTAMOS LOS PARTICIPANTES NUEVOS
                    int intNumregistros = dtPersonas.Rows.Count - 1;     
                    #endregion

                    #region INSERTAMOS LA ACTUACION
                    // INSERTAMOS EN LA TABLA RE_ACTUACION                           
                    //se quito para el nuevo modelo de exhortos  
                    RE_ACTUACION = funActuacion.insertar(RE_ACTUACION);               // INSERTAMOS LA ACTUACION EN LA TABLA RE_ACTUACION

                    // ValidacionError(funActuacion.strError, RE_ACTUACION.OficinaConsultar, RE_ACTUACION.actu_sUsuarioCreacion);
                    if (RE_ACTUACION.Error == false)
                    {
                        intActuacionID = RE_ACTUACION.actu_iActuacionId;               // OBTENEMOS EL ID DE LA ACTUACION

                        intNumregistros = LIS_RE_ACUTACIONDETALLE.Count - 1;
                        for (intFilaDet = 0; intFilaDet <= intNumregistros; intFilaDet++)
                        {
                            // INSERTAMOS EN LA TABLA RE_ACTUACIONDETALLE    
                            SGAC.BE.MRE.RE_ACTUACIONDETALLE objActuacionDetalle = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();
                            objActuacionDetalle = LIS_RE_ACUTACIONDETALLE[intFilaDet];

                            objActuacionDetalle.acde_iActuacionId = intActuacionID;
                            objActuacionDetalle = funActuacionDetalle.insertar(objActuacionDetalle);    // INSERTAMOS EL DETALLE DE LA ACTUACION EN LA TABLA RE_ACTUACIONDETALLE
                            ValidacionError(funActuacionDetalle.strError, objActuacionDetalle.OficinaConsultar, objActuacionDetalle.acde_sUsuarioCreacion);

                            LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;  // ACTUALIZAMOS EL ID DE LA ACTUACION DETALLE

                            if (objActuacionDetalle.Error == false)
                            {
                                if (objActuacionDetalle.acde_iActuacionDetalleId != 0)
                                {
                                    // INSERTAMOS LOS PAGOS DE LA ACTUACION
                                    SGAC.BE.MRE.RE_PAGO objPago = new SGAC.BE.MRE.RE_PAGO();
                                    //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                    LIS_RE_PAGO[intFilaDet].pago_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE
                                    objPago = funPago.insertar(LIS_RE_PAGO[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                    ValidacionError(funPago.strError, LIS_RE_PAGO[intFilaDet].OficinaConsultar, LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion);
                                    if (objPago.Error == true)
                                    {
                                        intResultado = 0;
                                        strMensajeError = objPago.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                        Transaction.Current.Rollback();
                                        scope.Dispose();
                                        return intResultado;
                                    }
                                }
                            }
                            else
                            {
                                intResultado = 0;
                                strMensajeError = objActuacionDetalle.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }
                        }
                    }
                    else
                    {
                        intResultado = 0;
                        strMensajeError = RE_ACTUACION.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                        Transaction.Current.Rollback();
                        scope.Dispose();
                        return intResultado;
                    }
                    #endregion

                    intResultado = 1;
                    scope.Complete();
                    scope.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return intResultado;
        }


        //---------------------------------------------------------------------

        public Boolean eliminar_actojudicialparticipante(int intParticipanteId) { 
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA funActoParticipante = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            return funActoParticipante.eliminar_actojudicialparticipante(intParticipanteId);
        } 

        // ****************************************************************
        public int Insertar(ref SGAC.BE.MRE.RE_ACTOJUDICIAL RE_ACTOJUDICIAL,
                            List<SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE> LIS_RE_ACTOJUDICIALPARTICIPANTE,
                            SGAC.BE.MRE.RE_ACTUACION RE_ACTUACION,
                            List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACUTACIONDETALLE,
                            List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO,
                            DataTable dtPersonas
                            )
        {
            #region DECLARACION DE VARIABLES
            Int16 intResultado = 0;
            int intFila = 0;
            int intFilaDet = 0;
            Int64 intActuacionID = 0;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            SGAC.DA.MRE.RE_PERSONA_DA funPersona = new SGAC.DA.MRE.RE_PERSONA_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTUACION_DA funActuacion = new SGAC.DA.MRE.ACTUACION.RE_ACTUACION_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTUACIONDETALLE_DA funActuacionDetalle = new SGAC.DA.MRE.ACTUACION.RE_ACTUACIONDETALLE_DA();
            SGAC.DA.MRE.ACTUACION.RE_PAGO_DA funPago = new SGAC.DA.MRE.ACTUACION.RE_PAGO_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTU_PAGO_DA funActuPago = new SGAC.DA.MRE.ACTUACION.RE_ACTU_PAGO_DA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL funActoJudicial = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA funActoParticipante = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            #endregion

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
                {
                    #region INSERTAMOS LOS PARTICIPANTES NUEVOS
                    int intNumregistros = dtPersonas.Rows.Count - 1;
                    for (intFila = 0; intFila <= intNumregistros; intFila++)
                    {
                        if (Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iPersonaId"].ToString()) == 0)         // SI EL ID DE LA PERSONA ES IGUAL 0, INSERTAMO UN NUEVO RUNE
                        {
                            bool booResult = false;

                            if (Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_sTipoPersonaId"].ToString()) == Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL))
                            {
                                booResult = funPersona.InsertarRuneRapido(ref dtPersonas, RE_ACTOJUDICIAL.OficinaConsultar, RE_ACTOJUDICIAL.HostName, RE_ACTOJUDICIAL.acju_sUsuarioCreacion, RE_ACTOJUDICIAL.acju_vIPCreacion, intFila);
                                ValidacionError(funPersona.strError, RE_ACTOJUDICIAL.OficinaConsultar, RE_ACTOJUDICIAL.acju_sUsuarioCreacion);
                            }
                            else
                            {
                                if (Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iEmpresaId"].ToString()) == 0)
                                {
                                    booResult = AgregarPersonaJuridica(ref dtPersonas, RE_ACTOJUDICIAL.OficinaConsultar, RE_ACTOJUDICIAL.HostName, RE_ACTOJUDICIAL.acju_sUsuarioCreacion, RE_ACTOJUDICIAL.acju_vIPCreacion, intFila);
                                    
                                }
                                else
                                {
                                    booResult = true;
                                }
                            }

                            if (booResult == false)
                            {
                                intResultado = 0;

                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }
                            else
                            {
                                // ACTUALIZAMOS EL ID DE LA PERSONA INGRESADA
                                //if (LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_sTipoParticipanteId == 8542)
                                // {
                                if (LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_sTipoPersonaId == 2101)
                                {
                                    LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_iPersonaId = Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iPersonaId"].ToString());
                                }
                                else
                                {
                                    LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_iEmpresaId = Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iEmpresaId"].ToString());
                                }
                                // }
                            }
                        }
                    }

                    #endregion

                    #region INSERTAMOS LA ACTUACION
                    //// INSERTAMOS EN LA TABLA RE_ACTUACION                           
                    ////se quito para el nuevo modelo de exhortos  
                    //RE_ACTUACION = funActuacion.insertar(RE_ACTUACION);               // INSERTAMOS LA ACTUACION EN LA TABLA RE_ACTUACION

                    //// ValidacionError(funActuacion.strError, RE_ACTUACION.OficinaConsultar, RE_ACTUACION.actu_sUsuarioCreacion);
                    //if (RE_ACTUACION.Error == false)
                    //{
                    //    intActuacionID = RE_ACTUACION.actu_iActuacionId;               // OBTENEMOS EL ID DE LA ACTUACION

                    //    intNumregistros = LIS_RE_ACUTACIONDETALLE.Count - 1;
                    //    for (intFilaDet = 0; intFilaDet <= intNumregistros; intFilaDet++)
                    //    {
                    //        // INSERTAMOS EN LA TABLA RE_ACTUACIONDETALLE    
                    //        SGAC.BE.MRE.RE_ACTUACIONDETALLE objActuacionDetalle = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();
                    //        objActuacionDetalle = LIS_RE_ACUTACIONDETALLE[intFilaDet];

                    //        objActuacionDetalle.acde_iActuacionId = intActuacionID;
                    //        objActuacionDetalle = funActuacionDetalle.insertar(objActuacionDetalle);    // INSERTAMOS EL DETALLE DE LA ACTUACION EN LA TABLA RE_ACTUACIONDETALLE
                    //        ValidacionError(funActuacionDetalle.strError, objActuacionDetalle.OficinaConsultar, objActuacionDetalle.acde_sUsuarioCreacion);

                    //        LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;  // ACTUALIZAMOS EL ID DE LA ACTUACION DETALLE

                    //        if (objActuacionDetalle.Error == false)
                    //        {
                    //            if (objActuacionDetalle.acde_iActuacionDetalleId != 0)
                    //            {
                    //                // INSERTAMOS LOS PAGOS DE LA ACTUACION
                    //                SGAC.BE.MRE.RE_PAGO objPago = new SGAC.BE.MRE.RE_PAGO();
                    //                //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                    //                LIS_RE_PAGO[intFilaDet].pago_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE
                    //                objPago = funPago.insertar(LIS_RE_PAGO[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                    //                ValidacionError(funPago.strError, LIS_RE_PAGO[intFilaDet].OficinaConsultar, LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion);
                    //                if (objPago.Error == true)
                    //                {
                    //                    intResultado = 0;
                    //                    strMensajeError = objPago.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                    //                    Transaction.Current.Rollback();
                    //                    scope.Dispose();
                    //                    return intResultado;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            intResultado = 0;
                    //            strMensajeError = objActuacionDetalle.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                    //            Transaction.Current.Rollback();
                    //            scope.Dispose();
                    //            return intResultado;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    intResultado = 0;
                    //    strMensajeError = RE_ACTUACION.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                    //    Transaction.Current.Rollback();
                    //    scope.Dispose();
                    //    return intResultado;
                    //}
                    #endregion

                    

                    #region INSERTAMOS EL ACTO JUDICIAL
                    SGAC.BE.MRE.RE_ACTOJUDICIAL objActoJudicial = new SGAC.BE.MRE.RE_ACTOJUDICIAL();
                    RE_ACTOJUDICIAL.acju_iActuacionId = RE_ACTUACION.actu_iActuacionId;                      // ACTUALIZAMOS EL ID DE LA ACTUACION
                    objActoJudicial = funActoJudicial.Insertar(RE_ACTOJUDICIAL);                             // INSERTAMOS EN LA TABLA ACTO JUDICIAL

                    int intNumParticipante = LIS_RE_ACTOJUDICIALPARTICIPANTE.Count - 1;
                    for (intFilaDet = 0; intFilaDet <= intNumParticipante; intFilaDet++)
                    {
                        SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE objParticipante = new SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE();
                        objParticipante = LIS_RE_ACTOJUDICIALPARTICIPANTE[intFilaDet];
                        objParticipante.ajpa_iActoJudicialId = objActoJudicial.acju_iActoJudicialId;         // ACTUALIZAMOS EL ID DEL ACTOJUDICIAL


                        if (objParticipante.ajpa_sTipoParticipanteId != Convert.ToInt16(Enumerador.enmJudicialTipoParticipante.DEMANDANTE) &&
                            objParticipante.ajpa_sTipoParticipanteId != Convert.ToInt16(Enumerador.enmJudicialTipoParticipante.RECURRENTE) &&
                        objParticipante.ajpa_sEstadoId != Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.ANULADO))
                        {
                            objParticipante.ajpa_iActuacionDetalleId = LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_iActuacionDetalleId;
                        }

                        if (LIS_RE_ACTOJUDICIALPARTICIPANTE[intFilaDet].ajpa_iActoJudicialParticipanteId <= 0)
                        {
                            SGAC.BE.MRE.RE_ACTUA_PAGO LIS_RE_ACTU_PAGO = new SGAC.BE.MRE.RE_ACTUA_PAGO();
                            SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE objActuParti = new SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE();

                            //bool bResul;
                            objActuParti =  funActoParticipante.Insertar(ref objParticipante);
                            ValidacionError(funActoParticipante.strError, objParticipante.OficinaConsultar, objParticipante.ajpa_sUsuarioCreacion);


                            if ((Int16)objActuParti.ajpa_sTipoParticipanteId == 8542)
                            {
                                // INSERTAMOS LOS ACTUACION PAGOS DE LA ACTUACION
                                SGAC.BE.MRE.RE_ACTUA_PAGO objActuPago = new SGAC.BE.MRE.RE_ACTUA_PAGO();
                                //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                //LIS_RE_PAGO[intFilaDet].pago_iActuacionDetalleId = 0;//objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE

                                LIS_RE_ACTU_PAGO.AP_iAJparticipanteId = (Int64)objActuParti.ajpa_iActoJudicialParticipanteId;
                                LIS_RE_ACTU_PAGO.AP_iTarifarioId = LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_sTarifarioId;
                                LIS_RE_ACTU_PAGO.AP_sPagoTipoId = LIS_RE_PAGO[intFilaDet].pago_sPagoTipoId;
                                LIS_RE_ACTU_PAGO.AP_iPersonaRecurrenteId = (Int64)RE_ACTUACION.actu_iPersonaRecurrenteId;
                                LIS_RE_ACTU_PAGO.AP_iEmpresaRecurrenteId = (Int64)RE_ACTUACION.actu_iEmpresaRecurrenteId;
                                LIS_RE_ACTU_PAGO.AP_dFechaOperacion = LIS_RE_PAGO[intFilaDet].pago_dFechaOperacion;
                                LIS_RE_ACTU_PAGO.AP_sMonedaLocalId = LIS_RE_PAGO[intFilaDet].pago_sMonedaLocalId;
                                LIS_RE_ACTU_PAGO.AP_FMontoMonedaLocal = LIS_RE_PAGO[intFilaDet].pago_FMontoMonedaLocal;
                                LIS_RE_ACTU_PAGO.AP_FMontoSolesConsulares = LIS_RE_PAGO[intFilaDet].pago_FMontoSolesConsulares;
                                LIS_RE_ACTU_PAGO.AP_FTipCambioBancario = LIS_RE_PAGO[intFilaDet].pago_FTipCambioBancario;
                                LIS_RE_ACTU_PAGO.AP_FTipCambioConsular = LIS_RE_PAGO[intFilaDet].pago_FTipCambioConsular;
                                LIS_RE_ACTU_PAGO.AP_sBancoId = LIS_RE_PAGO[intFilaDet].pago_sBancoId;
                                LIS_RE_ACTU_PAGO.AP_vBancoNumeroOperacion = LIS_RE_PAGO[intFilaDet].pago_vBancoNumeroOperacion;
                                LIS_RE_ACTU_PAGO.AP_bPagadoFlag = LIS_RE_PAGO[intFilaDet].pago_bPagadoFlag;
                                LIS_RE_ACTU_PAGO.AP_vComentario = LIS_RE_PAGO[intFilaDet].pago_vComentario;
                                LIS_RE_ACTU_PAGO.AP_vNumeroVoucher = LIS_RE_PAGO[intFilaDet].pago_vNumeroVoucher;
                                LIS_RE_ACTU_PAGO.AP_cEstado = LIS_RE_PAGO[intFilaDet].pago_cEstado;
                                LIS_RE_ACTU_PAGO.AP_sUsuarioCreacion = LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion;
                                LIS_RE_ACTU_PAGO.AP_vIPCreacion = LIS_RE_PAGO[intFilaDet].pago_vIPCreacion;
                                LIS_RE_ACTU_PAGO.AP_dFechaCreacion = LIS_RE_PAGO[intFilaDet].pago_dFechaCreacion;
                                LIS_RE_ACTU_PAGO.AP_sUsuarioModificacion = LIS_RE_PAGO[intFilaDet].pago_sUsuarioModificacion;
                                LIS_RE_ACTU_PAGO.AP_vIPModificacion = LIS_RE_PAGO[intFilaDet].pago_vIPModificacion;
                                LIS_RE_ACTU_PAGO.AP_dFechaModificacion = LIS_RE_PAGO[intFilaDet].pago_dFechaModificacion;
                                LIS_RE_ACTU_PAGO.OficinaConsultar = objActoJudicial.OficinaConsultar;

                                objActuPago = funActuPago.insertar(LIS_RE_ACTU_PAGO);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                ValidacionError(funPago.strError, LIS_RE_PAGO[intFilaDet].OficinaConsultar, LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion);
                                if (objActuPago.Error == true)
                                {
                                    intResultado = 0;
                                    strMensajeError = objActuPago.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                    Transaction.Current.Rollback();
                                    scope.Dispose();
                                    return intResultado;
                                }
                            }
                            

                            if (objActuParti.Error)          // INSERTAMOS EN LA TABLA ACTO JUDICIALPARTICIPANTE
                            {
                                intResultado = 0;
                                strMensajeError = objParticipante.Message.ToString();                //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }

                            
                        }
                        else
                        {
                            bool bResul = funActoParticipante.Actualizar(ref objParticipante);
                            ValidacionError(funActoParticipante.strError, objParticipante.OficinaConsultar, objParticipante.ajpa_sUsuarioCreacion);

                            if (!bResul)        // ACTUALIZAMOS LA TABLA ACTO JUDICIALPARTICIPANTE
                            {
                                intResultado = 0;
                                strMensajeError = objParticipante.Message.ToString();                //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }
                        }
                    }

                    #endregion

                    intResultado = 1;
                    scope.Complete();
                    scope.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return intResultado;
        }

        // ******************************************************************
        public int Actualizar(ref SGAC.BE.MRE.RE_ACTOJUDICIAL RE_ACTOJUDICIAL,
                    List<SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE> LIS_RE_ACTOJUDICIALPARTICIPANTE,
                    SGAC.BE.MRE.RE_ACTUACION RE_ACTUACION,
                    List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACUTACIONDETALLE,
                    List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO,
                    DataTable dtPersonas
                    )
        {
            #region DECLARACION DE VARIABLES
            Int16 intResultado = 0;
            int intFila = 0;
            int intFilaDet = 0;
            Int64 intActuacionID = 0;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            SGAC.DA.MRE.RE_PERSONA_DA funPersona = new SGAC.DA.MRE.RE_PERSONA_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTUACION_DA funActuacion = new SGAC.DA.MRE.ACTUACION.RE_ACTUACION_DA();
            SGAC.DA.MRE.ACTUACION.RE_ACTUACIONDETALLE_DA funActuacionDetalle = new SGAC.DA.MRE.ACTUACION.RE_ACTUACIONDETALLE_DA();
            SGAC.DA.MRE.ACTUACION.RE_PAGO_DA funPago = new SGAC.DA.MRE.ACTUACION.RE_PAGO_DA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL funActoJudicial = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA funActoParticipante = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            #endregion

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
                {
                    #region INSERTAMOS LOS PARTICIPANTES NUEVOS
                    int intNumregistros = dtPersonas.Rows.Count - 1;
                    for (intFila = 0; intFila <= intNumregistros; intFila++)
                    {
                        Int64 intIdParticipante = 0;

                        // SI EL TIPO DE PERSONA ES IGUAL A PERSONA NATURAL
                        if (Convert.ToInt16(dtPersonas.Rows[intFila]["ajpa_sTipoPersonaId"].ToString()) == 2101)
                        {
                            intIdParticipante = Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iPersonaId"].ToString());
                        }
                        else
                        {
                            intIdParticipante = Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iEmpresaId"].ToString());
                        }

                        if (intIdParticipante == 0)         // SI EL ID DE LA PERSONA ES IGUAL 0, INSERTAMO UN NUEVO RUNE
                        {
                            //bool booResult = funPersona.InsertarRuneRapido(ref dtPersonas, RE_ACTOJUDICIAL.OficinaConsultar, RE_ACTOJUDICIAL.HostName, Convert.ToInt16(RE_ACTOJUDICIAL.acju_sUsuarioModificacion), RE_ACTOJUDICIAL.acju_vIPModificacion, intFila);
                            bool booResult = false;

                            if (Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_sTipoPersonaId"].ToString()) == Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL))
                            {
                                booResult = funPersona.InsertarRuneRapido(ref dtPersonas, RE_ACTOJUDICIAL.OficinaConsultar, RE_ACTOJUDICIAL.HostName, Convert.ToInt16(RE_ACTOJUDICIAL.acju_sUsuarioModificacion), RE_ACTOJUDICIAL.acju_vIPModificacion, intFila);
                            }
                            else
                            {
                                if (Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iEmpresaId"].ToString()) == 0)
                                {
                                    booResult = AgregarPersonaJuridica(ref dtPersonas, RE_ACTOJUDICIAL.OficinaConsultar, RE_ACTOJUDICIAL.HostName, Convert.ToInt16(RE_ACTOJUDICIAL.acju_sUsuarioModificacion), RE_ACTOJUDICIAL.acju_vIPModificacion, intFila);
                                }
                                else
                                {
                                    booResult = true;
                                }


                            }

                            if (booResult == false)
                            {
                                intResultado = 0;

                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }
                            else
                            {
                                // ACTUALIZAMOS EL ID DE LA PERSONA INGRESADA
                                if (LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_sTipoPersonaId == 2101)
                                {
                                    LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_iPersonaId = Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iPersonaId"].ToString());
                                }
                                else
                                {
                                    LIS_RE_ACTOJUDICIALPARTICIPANTE[intFila].ajpa_iEmpresaId = Convert.ToInt64(dtPersonas.Rows[intFila]["ajpa_iEmpresaId"].ToString());
                                }
                            }
                        }
                    }

                    #endregion
                    SGAC.DA.MRE.ACTUACION.RE_ACTU_PAGO_DA funActuPago = new SGAC.DA.MRE.ACTUACION.RE_ACTU_PAGO_DA();
                    #region INSERTAMOS LA ACTUACION
                    //// INSERTAMOS EN LA TABLA RE_ACTUACION                           
                    ////RE_ACTUACION = funActuacion.insertar(RE_ACTUACION);               // INSERTAMOS LA ACTUACION EN LA TABLA RE_ACTUACION
                    //RE_ACTUACION = funActuacion.Actualizar(RE_ACTUACION);               // INSERTAMOS LA ACTUACION EN LA TABLA RE_ACTUACION

                    //if (RE_ACTUACION.Error == false)
                    //{
                    //    intActuacionID = RE_ACTUACION.actu_iActuacionId;               // OBTENEMOS EL ID DE LA ACTUACION

                    //    intNumregistros = LIS_RE_ACUTACIONDETALLE.Count - 1;
                    //    for (intFilaDet = 0; intFilaDet <= intNumregistros; intFilaDet++)
                    //    {
                    //        // INSERTAMOS EN LA TABLA RE_ACTUACIONDETALLE    
                    //        SGAC.BE.MRE.RE_ACTUACIONDETALLE objActuacionDetalle = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();
                    //        objActuacionDetalle = LIS_RE_ACUTACIONDETALLE[intFilaDet];

                    //        objActuacionDetalle.acde_iActuacionId = intActuacionID;

                    //        if (objActuacionDetalle.acde_iActuacionDetalleId <= 0)                          // HABERIGUAMOS SI EL DETALLE DE LA ACTUACION ES NUEVO
                    //        {
                    //            objActuacionDetalle = funActuacionDetalle.insertar(objActuacionDetalle);    // INSERTAMOS EL DETALLE DE LA ACTUACION EN LA TABLA RE_ACTUACIONDETALLE
                    //        }
                    //        //else
                    //        //{
                    //        //    objActuacionDetalle = funActuacionDetalle.a(objActuacionDetalle);    // INSERTAMOS EL DETALLE DE LA ACTUACION EN LA TABLA RE_ACTUACIONDETALLE
                    //        //}

                    //        LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;  // ACTUALIZAMOS EL ID DE LA ACTUACION DETALLE

                    //        if (objActuacionDetalle.Error == false)
                    //        {
                    //            if (objActuacionDetalle.acde_iActuacionDetalleId != 0)
                    //            {
                    //                // INSERTAMOS LOS PAGOS DE LA ACTUACION
                    //                SGAC.BE.MRE.RE_PAGO objPago = new SGAC.BE.MRE.RE_PAGO();
                    //                //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                    //                LIS_RE_PAGO[intFilaDet].pago_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE

                    //                if (LIS_RE_PAGO[intFilaDet].pago_iPagoId <= 0)
                    //                {
                    //                    objPago = funPago.insertar(LIS_RE_PAGO[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                    //                }
                    //                else
                    //                {
                    //                    objPago = funPago.Actualizar(LIS_RE_PAGO[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                    //                }

                    //                if (objPago.Error == true)
                    //                {
                    //                    intResultado = 0;
                    //                    strMensajeError = objPago.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                    //                    Transaction.Current.Rollback();
                    //                    scope.Dispose();
                    //                    return intResultado;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            intResultado = 0;
                    //            strMensajeError = objActuacionDetalle.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                    //            Transaction.Current.Rollback();
                    //            scope.Dispose();
                    //            return intResultado;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    intResultado = 0;
                    //    strMensajeError = RE_ACTUACION.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                    //    Transaction.Current.Rollback();
                    //    scope.Dispose();
                    //    return intResultado;
                    //}
                    #endregion

                    #region INSERTAMOS EL ACTO JUDICIAL
                    SGAC.BE.MRE.RE_ACTOJUDICIAL objActoJudicial = new SGAC.BE.MRE.RE_ACTOJUDICIAL();
                    RE_ACTOJUDICIAL.acju_iActuacionId = RE_ACTUACION.actu_iActuacionId;                      // ACTUALIZAMOS EL ID DE LA ACTUACION
                    objActoJudicial = funActoJudicial.Actualizar(RE_ACTOJUDICIAL);                             // INSERTAMOS EN LA TABLA ACTO JUDICIAL

                    int intNumParticipante = LIS_RE_ACTOJUDICIALPARTICIPANTE.Count - 1;
                    for (intFilaDet = 0; intFilaDet <= intNumParticipante; intFilaDet++)
                    {

                        SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE objParticipante = new SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE();
                        objParticipante = LIS_RE_ACTOJUDICIALPARTICIPANTE[intFilaDet];
                        objParticipante.ajpa_iActoJudicialId = objActoJudicial.acju_iActoJudicialId;         // ACTUALIZAMOS EL ID DEL ACTOJUDICIAL

                        if (objParticipante.ajpa_sTipoParticipanteId != Convert.ToInt16(Enumerador.enmJudicialTipoParticipante.DEMANDANTE) &&
                            objParticipante.ajpa_sEstadoId != Convert.ToInt16(Enumerador.enmJudicialParticipanteEstado.ANULADO))
                        {
                            objParticipante.ajpa_iActuacionDetalleId = LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_iActuacionDetalleId;
                        }



                        SGAC.BE.MRE.RE_ACTUA_PAGO LIS_RE_ACTU_PAGO = new SGAC.BE.MRE.RE_ACTUA_PAGO();
                        if (LIS_RE_ACTOJUDICIALPARTICIPANTE[intFilaDet].ajpa_iActoJudicialParticipanteId <= 0)
                        {                           
                            if (funActoParticipante.Insertar(ref objParticipante).Error != false)          // INSERTAMOS EN LA TABLA ACTO JUDICIALPARTICIPANTE
                            {
                                intResultado = 0;
                                strMensajeError = objParticipante.Message.ToString();                //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }
                            else {
                                if ((Int16)objParticipante.ajpa_sTipoParticipanteId == 8542)
                                {
                                    // INSERTAMOS LOS ACTUACION PAGOS DE LA ACTUACION
                                    SGAC.BE.MRE.RE_ACTUA_PAGO objActuPago = new SGAC.BE.MRE.RE_ACTUA_PAGO();
                                    //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                    //LIS_RE_PAGO[intFilaDet].pago_iActuacionDetalleId = 0;//objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE

                                    LIS_RE_ACTU_PAGO.AP_iAJparticipanteId = (Int16)objParticipante.ajpa_iActoJudicialParticipanteId;
                                    LIS_RE_ACTU_PAGO.AP_iTarifarioId = LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_sTarifarioId;
                                    LIS_RE_ACTU_PAGO.AP_sPagoTipoId = LIS_RE_PAGO[intFilaDet].pago_sPagoTipoId;
                                    LIS_RE_ACTU_PAGO.AP_dFechaOperacion = LIS_RE_PAGO[intFilaDet].pago_dFechaOperacion;
                                    LIS_RE_ACTU_PAGO.AP_sMonedaLocalId = LIS_RE_PAGO[intFilaDet].pago_sMonedaLocalId;
                                    LIS_RE_ACTU_PAGO.AP_FMontoMonedaLocal = LIS_RE_PAGO[intFilaDet].pago_FMontoMonedaLocal;
                                    LIS_RE_ACTU_PAGO.AP_FMontoSolesConsulares = LIS_RE_PAGO[intFilaDet].pago_FMontoSolesConsulares;
                                    LIS_RE_ACTU_PAGO.AP_FTipCambioBancario = LIS_RE_PAGO[intFilaDet].pago_FTipCambioBancario;
                                    LIS_RE_ACTU_PAGO.AP_FTipCambioConsular = LIS_RE_PAGO[intFilaDet].pago_FTipCambioConsular;
                                    LIS_RE_ACTU_PAGO.AP_sBancoId = LIS_RE_PAGO[intFilaDet].pago_sBancoId;
                                    LIS_RE_ACTU_PAGO.AP_vBancoNumeroOperacion = LIS_RE_PAGO[intFilaDet].pago_vBancoNumeroOperacion;
                                    LIS_RE_ACTU_PAGO.AP_bPagadoFlag = LIS_RE_PAGO[intFilaDet].pago_bPagadoFlag;
                                    LIS_RE_ACTU_PAGO.AP_vComentario = LIS_RE_PAGO[intFilaDet].pago_vComentario;
                                    LIS_RE_ACTU_PAGO.AP_vNumeroVoucher = LIS_RE_PAGO[intFilaDet].pago_vNumeroVoucher;
                                    LIS_RE_ACTU_PAGO.AP_cEstado = LIS_RE_PAGO[intFilaDet].pago_cEstado;
                                    LIS_RE_ACTU_PAGO.AP_sUsuarioCreacion = LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion;
                                    LIS_RE_ACTU_PAGO.AP_vIPCreacion = LIS_RE_PAGO[intFilaDet].pago_vIPCreacion;
                                    LIS_RE_ACTU_PAGO.AP_dFechaCreacion = LIS_RE_PAGO[intFilaDet].pago_dFechaCreacion;
                                    LIS_RE_ACTU_PAGO.AP_sUsuarioModificacion = LIS_RE_PAGO[intFilaDet].pago_sUsuarioModificacion;
                                    LIS_RE_ACTU_PAGO.AP_vIPModificacion = LIS_RE_PAGO[intFilaDet].pago_vIPModificacion;
                                    LIS_RE_ACTU_PAGO.AP_dFechaModificacion = LIS_RE_PAGO[intFilaDet].pago_dFechaModificacion;

                                    objActuPago = funActuPago.insertar(LIS_RE_ACTU_PAGO);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                    ValidacionError(funPago.strError, LIS_RE_PAGO[intFilaDet].OficinaConsultar, LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion);
                                    if (objActuPago.Error == true)
                                    {
                                        intResultado = 0;
                                        strMensajeError = objActuPago.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                        Transaction.Current.Rollback();
                                        scope.Dispose();
                                        return intResultado;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (funActoParticipante.Actualizar(ref objParticipante) == false)        // ACTUALIZAMOS LA TABLA ACTO JUDICIALPARTICIPANTE
                            {
                                intResultado = 0;
                                strMensajeError = objParticipante.Message.ToString();                //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                Transaction.Current.Rollback();
                                scope.Dispose();
                                return intResultado;
                            }
                            else
                            {
                                if ((Int16)objParticipante.ajpa_sTipoParticipanteId == 8542 && (Int16)objParticipante.ajpa_sEstadoId == 72)
                                {
                                    // INSERTAMOS LOS ACTUACION PAGOS DE LA ACTUACION
                                    SGAC.BE.MRE.RE_ACTUA_PAGO objActuPago = new SGAC.BE.MRE.RE_ACTUA_PAGO();
                                    //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                    //LIS_RE_PAGO[intFilaDet].pago_iActuacionDetalleId = 0;//objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE

                                    LIS_RE_ACTU_PAGO.AP_iAJparticipanteId = (Int16)objParticipante.ajpa_iActoJudicialParticipanteId;
                                    LIS_RE_ACTU_PAGO.AP_iTarifarioId = LIS_RE_ACUTACIONDETALLE[intFilaDet].acde_sTarifarioId;
                                    LIS_RE_ACTU_PAGO.AP_sPagoTipoId = LIS_RE_PAGO[intFilaDet].pago_sPagoTipoId;
                                    LIS_RE_ACTU_PAGO.AP_dFechaOperacion = LIS_RE_PAGO[intFilaDet].pago_dFechaOperacion;
                                    LIS_RE_ACTU_PAGO.AP_sMonedaLocalId = LIS_RE_PAGO[intFilaDet].pago_sMonedaLocalId;
                                    LIS_RE_ACTU_PAGO.AP_FMontoMonedaLocal = LIS_RE_PAGO[intFilaDet].pago_FMontoMonedaLocal;
                                    LIS_RE_ACTU_PAGO.AP_FMontoSolesConsulares = LIS_RE_PAGO[intFilaDet].pago_FMontoSolesConsulares;
                                    LIS_RE_ACTU_PAGO.AP_FTipCambioBancario = LIS_RE_PAGO[intFilaDet].pago_FTipCambioBancario;
                                    LIS_RE_ACTU_PAGO.AP_FTipCambioConsular = LIS_RE_PAGO[intFilaDet].pago_FTipCambioConsular;
                                    LIS_RE_ACTU_PAGO.AP_sBancoId = LIS_RE_PAGO[intFilaDet].pago_sBancoId;
                                    LIS_RE_ACTU_PAGO.AP_vBancoNumeroOperacion = LIS_RE_PAGO[intFilaDet].pago_vBancoNumeroOperacion;
                                    LIS_RE_ACTU_PAGO.AP_bPagadoFlag = LIS_RE_PAGO[intFilaDet].pago_bPagadoFlag;
                                    LIS_RE_ACTU_PAGO.AP_vComentario = LIS_RE_PAGO[intFilaDet].pago_vComentario;
                                    LIS_RE_ACTU_PAGO.AP_vNumeroVoucher = LIS_RE_PAGO[intFilaDet].pago_vNumeroVoucher;
                                    LIS_RE_ACTU_PAGO.AP_cEstado = LIS_RE_PAGO[intFilaDet].pago_cEstado;
                                    LIS_RE_ACTU_PAGO.AP_sUsuarioCreacion = LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion;
                                    LIS_RE_ACTU_PAGO.AP_vIPCreacion = LIS_RE_PAGO[intFilaDet].pago_vIPCreacion;
                                    LIS_RE_ACTU_PAGO.AP_dFechaCreacion = LIS_RE_PAGO[intFilaDet].pago_dFechaCreacion;
                                    LIS_RE_ACTU_PAGO.AP_sUsuarioModificacion = LIS_RE_PAGO[intFilaDet].pago_sUsuarioModificacion;
                                    LIS_RE_ACTU_PAGO.AP_vIPModificacion = LIS_RE_PAGO[intFilaDet].pago_vIPModificacion;
                                    LIS_RE_ACTU_PAGO.AP_dFechaModificacion = LIS_RE_PAGO[intFilaDet].pago_dFechaModificacion;

                                    objActuPago = funActuPago.insertar(LIS_RE_ACTU_PAGO);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                    ValidacionError(funPago.strError, LIS_RE_PAGO[intFilaDet].OficinaConsultar, LIS_RE_PAGO[intFilaDet].pago_sUsuarioCreacion);
                                    if (objActuPago.Error == true)
                                    {
                                        intResultado = 0;
                                        strMensajeError = objActuPago.Message.ToString();                       //  SI OCURRE UN ERROR ENVIAMOS EL MENSAJE DE ERROR
                                        Transaction.Current.Rollback();
                                        scope.Dispose();
                                        return intResultado;
                                    }
                                }
                            }                            

                        }
                    }

                    #endregion

                    intResultado = 1;
                    scope.Complete();
                    scope.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return intResultado;
        }


        public int ActualizarEstado(Int64 iActoJudicialId, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        {
            //ActoJudicialMantenimientoDA objDA = new ActoJudicialMantenimientoDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();

            try
            {
                int intResultado = objDA.ActualizarEstado(iActoJudicialId, sEstadoId, sUsuarioModificacion, vIPModificacion, sOficinaConsularId, vHostName);
                return intResultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(objDA.strError, sOficinaConsularId, sUsuarioModificacion);

                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public int ActualizarCorrelativoActoJudicial(Int64 intActuacionDetalleId, Int16 intOficinaConsularId, Int16 intTarifarioId, Int16 intUsuarioId)
        {
            //ActoJudicialMantenimientoDA objDA = new ActoJudicialMantenimientoDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            try
            {
                int intResultado = objDA.ActualizarCorrelativoActoJudicial(intActuacionDetalleId, intOficinaConsularId, intTarifarioId, intUsuarioId);
                return intResultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {

                ValidacionError(objDA.strError, intOficinaConsularId, intUsuarioId);

                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public void ValidacionError(string mensaje, Int16 sOficinaConsular, Int16 sUsuario)
        {

            if (!string.IsNullOrEmpty(mensaje))
            {


                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = sOficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = sUsuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

            }
        }
    }
}