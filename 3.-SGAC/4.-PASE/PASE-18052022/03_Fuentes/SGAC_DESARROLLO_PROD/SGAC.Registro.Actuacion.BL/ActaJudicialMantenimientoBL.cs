using System;
using System.Configuration;
using SGAC.Accesorios;
using System.Collections.Generic;
using SGAC.Registro.Actuacion.DA;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using SGAC.BE.MRE;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActaJudicialMantenimientoBL
    {
        // ***********
        // **  OK   **
        // ***********
        public int Insertar(List<SGAC.BE.MRE.RE_ACTAJUDICIAL> ActasLista)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTAJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTAJUDICIAL();
             
            int intFila = 0;
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
                {

                    for (intFila = 0; intFila <= ActasLista.Count - 1; intFila++)
                    {
                        SGAC.BE.MRE.RE_ACTAJUDICIAL Notificacion = new RE_ACTAJUDICIAL();

                        if (ActasLista[intFila].acjd_iActaJudicialId <= 0)
                        {
                            Notificacion = objDA.Insertar(ActasLista[intFila]);

                        }
                        else
                        {
                            Notificacion = objDA.Actualizar(ActasLista[intFila]);
                        }

                        ValidacionError(objDA.strError, Notificacion.OficinaConsultar, Notificacion.acjd_sUsuarioCreacion);

                        if (Notificacion.Error == true)
                        {
                            intResultado = 0;
                            Transaction.Current.Rollback();
                            scope.Dispose();
                            break;
                        }
                    }

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


        public int Insertar_Actu_Det(
                    SGAC.BE.MRE.RE_ACTUACION RE_ACTUACION,
                    List<SGAC.BE.MRE.RE_ACTUACIONDETALLE> LIS_RE_ACUTACIONDETALLE,
                    List<SGAC.BE.MRE.RE_PAGO> LIS_RE_PAGO,
                    DataTable dtPersonas,
                    ref String iActoJudicialParticipanteId,
                    ref String _dFechaRegistro
                    )
        {
            #region DECLARACION DE VARIABLES
            Int16 intResultado = 0;
            int intFila = 0;
            int intFilaDet = 0;
            Int64 intActuacionID = 0;
            string strMensajeError = "";
            _dFechaRegistro = "";
            //var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

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
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    
                    #region INSERTAMOS LA ACTUACION
                    // INSERTAMOS EN LA TABLA RE_ACTUACION                           
                    //se quito para el nuevo modelo de exhortos  

                    int NRoParticipante = 0;

                    if (dtPersonas != null)
                    {
                        //foreach (DataRow item in dtPersonas.Rows)
                        //{
                        //    sCodParticipante += item["ajpa_iActoJudicialParticipanteId"].ToString() + ",";
                        //}

                        foreach (DataRow item in dtPersonas.Rows)
                        {
                            if (item["ajpa_iActoJudicialParticipanteId"].ToString() == iActoJudicialParticipanteId)
                            {
                                break;
                            } 
                            NRoParticipante += 1;
                        }// sCodParticipante.Substring(0, sCodParticipante.Length - 1);
                    }
                    RE_ACTUACION.Message = iActoJudicialParticipanteId;
                    RE_ACTUACION.actu_FCantidad = 1; // es 1 por que una actuacion pertenece a un solo participante al igual que el pago
                    RE_ACTUACION = funActuacion.insertar_exhorto(RE_ACTUACION);               // INSERTAMOS LA ACTUACION EN LA TABLA RE_ACTUACION

                    // ValidacionError(funActuacion.strError, RE_ACTUACION.OficinaConsultar, RE_ACTUACION.actu_sUsuarioCreacion);
                    if (RE_ACTUACION.Error == false )
                    {
                        if (RE_ACTUACION.actu_iActuacionId != 0)
                        {
                            //int intNumregistros = 0;// cantidad_participantes - 1;
                            intActuacionID = RE_ACTUACION.actu_iActuacionId;               // OBTENEMOS EL ID DE LA ACTUACION

                            //intNumregistros = LIS_RE_ACUTACIONDETALLE.Count - 1;

                            //for (intFilaDet = 0; intFilaDet <= intNumregistros; intFilaDet++)
                            //{
                                // INSERTAMOS EN LA TABLA RE_ACTUACIONDETALLE    
                                SGAC.BE.MRE.RE_ACTUACIONDETALLE objActuacionDetalle = new SGAC.BE.MRE.RE_ACTUACIONDETALLE();
                                objActuacionDetalle = LIS_RE_ACUTACIONDETALLE[NRoParticipante];
                                
                                objActuacionDetalle.acde_iActuacionId = intActuacionID;
                                objActuacionDetalle.Message = iActoJudicialParticipanteId;
                                objActuacionDetalle = funActuacionDetalle.insertar(objActuacionDetalle);    // INSERTAMOS EL DETALLE DE LA ACTUACION EN LA TABLA RE_ACTUACIONDETALLE
                                ValidacionError(funActuacionDetalle.strError, objActuacionDetalle.OficinaConsultar, objActuacionDetalle.acde_sUsuarioCreacion);

                                

                                LIS_RE_ACUTACIONDETALLE[NRoParticipante].acde_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;  // ACTUALIZAMOS EL ID DE LA ACTUACION DETALLE
                                iActoJudicialParticipanteId = objActuacionDetalle.acde_iActuacionDetalleId.ToString();
                                _dFechaRegistro = objActuacionDetalle.acde_dFechaRegistro.ToString();
                                if (objActuacionDetalle.Error == false)
                                {
                                    if (objActuacionDetalle.acde_iActuacionDetalleId != 0)
                                    {
                                        // INSERTAMOS LOS PAGOS DE LA ACTUACION
                                        SGAC.BE.MRE.RE_PAGO objPago = new SGAC.BE.MRE.RE_PAGO();
                                        //objPago = funPago.insertar(Pago[intFilaDet]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                        LIS_RE_PAGO[NRoParticipante].pago_iActuacionDetalleId = objActuacionDetalle.acde_iActuacionDetalleId;    // ACTUALIZAMOS EL ID DE LA TABLA ACTUACIONDETALLE

                                        objPago = funPago.insertar(LIS_RE_PAGO[NRoParticipante]);                           // INSERTAMOS EL PAGO EN LA TABLA RE_PAGO 
                                        ValidacionError(funPago.strError, LIS_RE_PAGO[NRoParticipante].OficinaConsultar, LIS_RE_PAGO[NRoParticipante].pago_sUsuarioCreacion);
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

                                
                            //}
                        }
                        else
                        {
                            intResultado = 0;
                            scope.Dispose();
                            return intResultado;
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
        
        // ***********
        // **  OK   **
        // ***********
        public int Actualizar_Estado(Int64 iActaJudicialI, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName,
            string sObservaciones)
        {
            int intResult = 0;
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTAJUDICIAL funActa = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTAJUDICIAL();
            try
            {
                
                intResult = funActa.Actualizar_Estado(iActaJudicialI, sEstadoId, sUsuarioModificacion, vIPModificacion, sOficinaConsularId, vHostName,
                    sObservaciones);
                return intResult;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(funActa.strError, sOficinaConsularId, sUsuarioModificacion);
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