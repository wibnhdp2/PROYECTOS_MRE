using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionMantenimientoDA
    {
        private string StrConnectionName = string.Empty;
        public string strError = "";

        public ActuacionMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionMantenimientoDA()
        {
            GC.Collect();
        }

        //public int InsertarDesdeJudicial(RE_ACTUACION ObjActuacBE, ref Int64 intActuacionId, ref List<RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE, int intsOficinaConsularId, string strHostName, DataTable DTRePagos)
        //{
        //    long LonResultQueryActuacion = 0;
        //    int intValor = 0;
        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[10];

        //        prmParameterActuacion[0] = new SqlParameter("@actu_iActuacionId", SqlDbType.BigInt);
        //        prmParameterActuacion[0].Direction = ParameterDirection.Output;

        //        prmParameterActuacion[1] = new SqlParameter("@actu_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterActuacion[1].Value = ObjActuacBE.actu_sOficinaConsularId;

        //        prmParameterActuacion[2] = new SqlParameter("@actu_FCantidad", SqlDbType.Float);
        //        prmParameterActuacion[2].Value = ObjActuacBE.actu_FCantidad;

        //        prmParameterActuacion[3] = new SqlParameter("@actu_iPersonaRecurrenteId", SqlDbType.BigInt);
        //        prmParameterActuacion[3].Value = ObjActuacBE.actu_iPersonaRecurrenteId;

        //        prmParameterActuacion[4] = new SqlParameter("@actu_iEmpresaRecurrenteId", SqlDbType.BigInt);
        //        prmParameterActuacion[4].Value = ObjActuacBE.actu_iEmpresaRecurrenteId;

        //        prmParameterActuacion[5] = new SqlParameter("@actu_dFechaRegistro", SqlDbType.DateTime);
        //        prmParameterActuacion[5].Value = ObjActuacBE.actu_dFechaRegistro;

        //        prmParameterActuacion[6] = new SqlParameter("@actu_sEstado", SqlDbType.SmallInt);
        //        prmParameterActuacion[6].Value = ObjActuacBE.actu_sEstado;

        //        prmParameterActuacion[7] = new SqlParameter("@actu_sUsuarioCreacion", SqlDbType.Int);
        //        prmParameterActuacion[7].Value = ObjActuacBE.actu_sUsuarioCreacion;

        //        prmParameterActuacion[8] = new SqlParameter("@actu_vIPCreacion", SqlDbType.VarChar, 50);
        //        prmParameterActuacion[8].Value = ObjActuacBE.actu_vIPCreacion;

        //        prmParameterActuacion[9] = new SqlParameter("@actu_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterActuacion[9].Value = Util.ObtenerHostName();

        //        LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                            CommandType.StoredProcedure,
        //                                                            "PN_REGISTRO.USP_RE_ACTUACION_ADICIONAR",
        //                                                            prmParameterActuacion);

        //        intActuacionId = (long)prmParameterActuacion[0].Value;
        //        if (intActuacionId != 0)
        //        {
        //            int intresult = InsertardesdeJudicial_Detalle(ref LIS_RE_ACTUACIONDETALLE, intsOficinaConsularId, strHostName, intActuacionId, DTRePagos);

        //            if (intresult == (int)Enumerador.enmResultadoQuery.OK)
        //            {
        //                intValor = (int)Enumerador.enmResultadoQuery.OK;
        //            }
        //            else
        //            {
        //                intValor = (int)Enumerador.enmResultadoQuery.ERR;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        intValor = (int)Enumerador.enmResultadoQuery.ERR;
        //        throw ex;
        //    }
        //    return intValor;
        //}

        //------------------------------------------------------------------
        //Fecha: 20/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public int InsertarDesdeJudicial(RE_ACTUACION ObjActuacBE, ref Int64 intActuacionId, ref List<RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE, int intsOficinaConsularId, string strHostName, DataTable DTRePagos)
        {

            int intValor = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@actu_iActuacionId", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@actu_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjActuacBE.actu_sOficinaConsularId;
                        cmd.Parameters.Add("@actu_FCantidad", SqlDbType.Float).Value = ObjActuacBE.actu_FCantidad;
                        cmd.Parameters.Add("@actu_iPersonaRecurrenteId", SqlDbType.BigInt).Value = ObjActuacBE.actu_iPersonaRecurrenteId;
                        cmd.Parameters.Add("@actu_iEmpresaRecurrenteId", SqlDbType.BigInt).Value = ObjActuacBE.actu_iEmpresaRecurrenteId;
                        cmd.Parameters.Add("@actu_dFechaRegistro", SqlDbType.DateTime).Value = ObjActuacBE.actu_dFechaRegistro;
                        cmd.Parameters.Add("@actu_sEstado", SqlDbType.SmallInt).Value = ObjActuacBE.actu_sEstado;
                        cmd.Parameters.Add("@actu_sUsuarioCreacion", SqlDbType.Int).Value = ObjActuacBE.actu_sUsuarioCreacion;
                        cmd.Parameters.Add("@actu_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjActuacBE.actu_vIPCreacion;
                        cmd.Parameters.Add("@actu_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intActuacionId = Convert.ToInt64(lReturn1.Value);

                        if (intActuacionId != 0)
                        {
                            int intresult = InsertardesdeJudicial_Detalle(ref LIS_RE_ACTUACIONDETALLE, intsOficinaConsularId, strHostName, intActuacionId, DTRePagos);

                            if (intresult == (int)Enumerador.enmResultadoQuery.OK)
                            {
                                intValor = (int)Enumerador.enmResultadoQuery.OK;
                            }
                            else
                            {
                                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                            }
                        }
                    }
                }
               
                
            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intValor;
        }


        public int ModificarDesdeJudicial(RE_ACTUACION ObjActuacBE, ref Int64 intActuacionId, ref List<RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE, int intsOficinaConsularId, string strHostName, DataTable DTRePagos)
        {
            int intValor = 0;
            try
            {
                int intresult = InsertardesdeJudicial_Detalle(ref LIS_RE_ACTUACIONDETALLE, intsOficinaConsularId, strHostName, intActuacionId, DTRePagos);

                if (intresult == (int)Enumerador.enmResultadoQuery.OK)
                {
                    intValor = (int)Enumerador.enmResultadoQuery.OK;
                }
                else
                {
                    intValor = (int)Enumerador.enmResultadoQuery.ERR;
                }

            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intValor;
        }

        private int InsertardesdeJudicial_Detalle(ref List<RE_ACTUACIONDETALLE> LIS_RE_ACTUACIONDETALLE, int intsOficinaConsularId, string strHostName, Int64 intActuacionId, DataTable DTRePagos)
        {
            long LonResultQueryActuacion = 0;
            int intFila = 0;
            int intValor = 0;

            try
            {
                SqlParameter[] prmParameterActuacion = new SqlParameter[21];
                Int64 iActuacionDetalleId = 0;

                for (intFila = 0; intFila <= LIS_RE_ACTUACIONDETALLE.Count - 1; intFila++)
                {
                    prmParameterActuacion[0] = new SqlParameter("@acde_iActuacionId", SqlDbType.BigInt);
                    prmParameterActuacion[0].Value = intActuacionId;//  LIS_RE_ACTUACIONDETALLE[intFila].acde_iActuacionId;

                    prmParameterActuacion[1] = new SqlParameter("@acde_sTarifarioId", SqlDbType.SmallInt);
                    prmParameterActuacion[1].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_sTarifarioId;

                    prmParameterActuacion[2] = new SqlParameter("@acde_sItem ", SqlDbType.SmallInt);
                    prmParameterActuacion[2].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_sItem;

                    prmParameterActuacion[3] = new SqlParameter("@acde_dFechaRegistro ", SqlDbType.DateTime);
                    prmParameterActuacion[3].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_dFechaRegistro;

                    prmParameterActuacion[4] = new SqlParameter("@acde_bRequisitosFlag ", SqlDbType.Bit);
                    prmParameterActuacion[4].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_bRequisitosFlag;

                    prmParameterActuacion[5] = new SqlParameter("@acde_IVinculacionInsumoId", SqlDbType.Int);
                    prmParameterActuacion[5].Value = 0;

                    prmParameterActuacion[6] = new SqlParameter("@acde_dVinculacionFecha", SqlDbType.DateTime);
                    prmParameterActuacion[6].Value = null;

                    prmParameterActuacion[7] = new SqlParameter("@acde_ICorrelativoActuacion", SqlDbType.Int);
                    prmParameterActuacion[7].Value = 0;

                    prmParameterActuacion[8] = new SqlParameter("@acde_ICorrelativoTarifario ", SqlDbType.Int);
                    prmParameterActuacion[8].Value = 0;

                    prmParameterActuacion[9] = new SqlParameter("@acde_sFuncionarioFirmanteId", SqlDbType.SmallInt);
                    prmParameterActuacion[9].Value = null;//LIS_RE_ACTUACIONDETALLE[intFila].acde_IFuncionarioFirmanteId;

                    prmParameterActuacion[10] = new SqlParameter("@acde_sFuncionarioContactoId", SqlDbType.SmallInt);
                    prmParameterActuacion[10].Value = null; //LIS_RE_ACTUACIONDETALLE[intFila].acde_IFuncionarioContactoId;

                    prmParameterActuacion[11] = new SqlParameter("@acde_bImpresionFlag", SqlDbType.Bit);
                    prmParameterActuacion[11].Value = 0;

                    prmParameterActuacion[12] = new SqlParameter("@acde_dImpresionFecha", SqlDbType.DateTime);
                    prmParameterActuacion[12].Value = null;

                    prmParameterActuacion[13] = new SqlParameter("@acde_sImpresionFuncionarioId", SqlDbType.SmallInt);
                    prmParameterActuacion[13].Value = 0;

                    prmParameterActuacion[14] = new SqlParameter("@acde_vNotas", SqlDbType.VarChar, 1000);
                    prmParameterActuacion[14].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_vNotas;

                    prmParameterActuacion[15] = new SqlParameter("@acde_sEstadoId", SqlDbType.SmallInt);
                    prmParameterActuacion[15].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_sEstadoId;

                    prmParameterActuacion[16] = new SqlParameter("@acde_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterActuacion[16].Value = intsOficinaConsularId;

                    prmParameterActuacion[17] = new SqlParameter("@acde_sUsuarioCreacion", SqlDbType.SmallInt);
                    prmParameterActuacion[17].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_sUsuarioCreacion;

                    prmParameterActuacion[18] = new SqlParameter("@acde_vIPCreacion", SqlDbType.VarChar, 50);
                    prmParameterActuacion[18].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_vIPCreacion;

                    prmParameterActuacion[19] = new SqlParameter("@acde_vHostName", SqlDbType.VarChar, 20);
                    prmParameterActuacion[19].Value = strHostName;

                    if (LIS_RE_ACTUACIONDETALLE[intFila].acde_iActuacionDetalleId == 0)
                    {
                        prmParameterActuacion[20] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.VarChar, 20);
                        prmParameterActuacion[20].Direction = ParameterDirection.Output;

                        LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName, CommandType.StoredProcedure, "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_ADICIONAR", prmParameterActuacion);
                        iActuacionDetalleId = Convert.ToInt64(prmParameterActuacion[20].Value);
                    }
                    else
                    {
                        prmParameterActuacion[20] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.VarChar, 20);
                        prmParameterActuacion[20].Value = LIS_RE_ACTUACIONDETALLE[intFila].acde_iActuacionDetalleId;

                        LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName, CommandType.StoredProcedure, "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_MODIFICAR", prmParameterActuacion);
                        iActuacionDetalleId = LIS_RE_ACTUACIONDETALLE[intFila].acde_iActuacionDetalleId;
                    }

                    if (iActuacionDetalleId != 0)
                    {
                        LIS_RE_ACTUACIONDETALLE[intFila].acde_iActuacionDetalleId = Convert.ToInt32(iActuacionDetalleId);

                        if (Convert.ToInt32(DTRePagos.Rows[intFila]["pago_iPagoId"].ToString()) <= 0)
                        {
                            InsertarPagoDesdeJudicial(DTRePagos, iActuacionDetalleId, intFila);
                        }
                        intValor = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intValor;
        }

        //private int InsertarPagoDesdeJudicial(DataTable DTRePagos, Int64 iActuacionDetalleId, int intFilaActual)
        //{
        //    long LonResultQueryActuacion = 0;
        //    int intFila = intFilaActual;
        //    int intValor = 0;

        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[17];

        //        prmParameterActuacion[0] = new SqlParameter("pago_sPagoTipoId", SqlDbType.SmallInt);
        //        prmParameterActuacion[0].Value = DTRePagos.Rows[intFila]["pago_sPagoTipoId"].ToString();

        //        prmParameterActuacion[1] = new SqlParameter("@pago_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterActuacion[1].Value = iActuacionDetalleId;                                        //DTRePagos.Rows[intFila]["pago_iActuacionDetalleId"].ToString()

        //        prmParameterActuacion[2] = new SqlParameter("@pago_dFechaOperacion", SqlDbType.DateTime);
        //        prmParameterActuacion[2].Value = DTRePagos.Rows[intFila]["pago_dFechaOperacion"].ToString();

        //        prmParameterActuacion[3] = new SqlParameter("@pago_sMonedaLocalId", SqlDbType.SmallInt);
        //        prmParameterActuacion[3].Value = DTRePagos.Rows[intFila]["pago_sMonedaLocalId"].ToString();

        //        prmParameterActuacion[4] = new SqlParameter("@pago_FMontoMonedaLocal", SqlDbType.Float);
        //        prmParameterActuacion[4].Value = DTRePagos.Rows[intFila]["pago_FMontoMonedaLocal"].ToString();

        //        prmParameterActuacion[5] = new SqlParameter("@pago_FMontoSolesConsulares", SqlDbType.Float);
        //        prmParameterActuacion[5].Value = DTRePagos.Rows[intFila]["pago_FMontoSolesConsulares"].ToString();

        //        prmParameterActuacion[6] = new SqlParameter("@pago_FTipCambioBancario", SqlDbType.Float);
        //        prmParameterActuacion[6].Value = DTRePagos.Rows[intFila]["pago_FTipCambioBancario"].ToString();

        //        prmParameterActuacion[7] = new SqlParameter("@pago_FTipCambioConsular", SqlDbType.Float);
        //        prmParameterActuacion[7].Value = DTRePagos.Rows[intFila]["pago_FTipCambioConsular"].ToString();

        //        if (Convert.ToInt16(DTRePagos.Rows[intFila]["pago_sBancoId"].ToString()) == 0)
        //        {
        //            prmParameterActuacion[8] = new SqlParameter("@pago_sBancoId", SqlDbType.SmallInt);
        //            prmParameterActuacion[8].Value = null;
        //        }
        //        else
        //        {
        //            prmParameterActuacion[8] = new SqlParameter("@pago_sBancoId", SqlDbType.SmallInt);
        //            prmParameterActuacion[8].Value = DTRePagos.Rows[intFila]["pago_sBancoId"].ToString();
        //        }

        //        prmParameterActuacion[9] = new SqlParameter("@pago_vBancoNumeroOperacion", SqlDbType.VarChar);
        //        prmParameterActuacion[9].Value = DTRePagos.Rows[intFila]["pago_vBancoNumeroOperacion"].ToString();

        //        prmParameterActuacion[10] = new SqlParameter("@pago_bPagadoFlag", SqlDbType.Bit);
        //        prmParameterActuacion[10].Value = DTRePagos.Rows[intFila]["pago_bPagadoFlag"].ToString();

        //        prmParameterActuacion[11] = new SqlParameter("@pago_vComentario", SqlDbType.VarChar);
        //        prmParameterActuacion[11].Value = DTRePagos.Rows[intFila]["pago_vComentario"].ToString();

        //        prmParameterActuacion[12] = new SqlParameter("@pago_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterActuacion[12].Value = DTRePagos.Rows[intFila]["pago_sOficinaConsularId"].ToString();

        //        prmParameterActuacion[13] = new SqlParameter("@pago_sUsuarioCreacion", SqlDbType.SmallInt);
        //        prmParameterActuacion[13].Value = DTRePagos.Rows[intFila]["pago_sUsuarioCreacion"].ToString();

        //        prmParameterActuacion[14] = new SqlParameter("@pago_vIPCreacion", SqlDbType.VarChar);
        //        prmParameterActuacion[14].Value = DTRePagos.Rows[intFila]["pago_vIPCreacion"].ToString();

        //        prmParameterActuacion[15] = new SqlParameter("@pago_vHostName", SqlDbType.VarChar);
        //        prmParameterActuacion[15].Value = DTRePagos.Rows[intFila]["pago_vHostName"].ToString();

        //        prmParameterActuacion[16] = new SqlParameter("@pago_vNumeroVoucher", SqlDbType.VarChar);
        //        prmParameterActuacion[16].Value = DTRePagos.Rows[intFila]["pago_vNumeroVoucher"].ToString();

        //        LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName, CommandType.StoredProcedure, "PN_REGISTRO.USP_RE_PAGO_ADICIONAR", prmParameterActuacion);

        //        if (iActuacionDetalleId != 0)
        //        {
        //            intValor = (int)Enumerador.enmResultadoQuery.OK;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        intValor = (int)Enumerador.enmResultadoQuery.ERR;
        //        throw ex;
        //    }

        //    return intValor;
        //}

        //------------------------------------------------------------------
        //Fecha: 20/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        private int InsertarPagoDesdeJudicial(DataTable DTRePagos, Int64 iActuacionDetalleId, int intFilaActual)
        {
            int intFila = intFilaActual;
            int intValor = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pago_sPagoTipoId", SqlDbType.SmallInt).Value = DTRePagos.Rows[intFila]["pago_sPagoTipoId"].ToString();
                        cmd.Parameters.Add("@pago_iActuacionDetalleId", SqlDbType.BigInt).Value = iActuacionDetalleId;
                        cmd.Parameters.Add("@pago_dFechaOperacion", SqlDbType.DateTime).Value = DTRePagos.Rows[intFila]["pago_dFechaOperacion"].ToString();
                        cmd.Parameters.Add("@pago_sMonedaLocalId", SqlDbType.SmallInt).Value = DTRePagos.Rows[intFila]["pago_sMonedaLocalId"].ToString();
                        cmd.Parameters.Add("@pago_FMontoMonedaLocal", SqlDbType.Float).Value = DTRePagos.Rows[intFila]["pago_FMontoMonedaLocal"].ToString();
                        cmd.Parameters.Add("@pago_FMontoSolesConsulares", SqlDbType.Float).Value = DTRePagos.Rows[intFila]["pago_FMontoSolesConsulares"].ToString();
                        cmd.Parameters.Add("@pago_FTipCambioBancario", SqlDbType.Float).Value = DTRePagos.Rows[intFila]["pago_FTipCambioBancario"].ToString();
                        cmd.Parameters.Add("@pago_FTipCambioConsular", SqlDbType.Float).Value = DTRePagos.Rows[intFila]["pago_FTipCambioConsular"].ToString();
                        if (Convert.ToInt16(DTRePagos.Rows[intFila]["pago_sBancoId"].ToString()) == 0)
                        {
                            cmd.Parameters.Add("@pago_sBancoId", SqlDbType.SmallInt).Value = null;
                        }
                        else
                        {
                            cmd.Parameters.Add("@pago_sBancoId", SqlDbType.SmallInt).Value = DTRePagos.Rows[intFila]["pago_sBancoId"].ToString();
                        }
                        cmd.Parameters.Add("@pago_vBancoNumeroOperacion", SqlDbType.VarChar).Value = DTRePagos.Rows[intFila]["pago_vBancoNumeroOperacion"].ToString();
                        cmd.Parameters.Add("@pago_bPagadoFlag", SqlDbType.Bit).Value = DTRePagos.Rows[intFila]["pago_bPagadoFlag"].ToString();
                        cmd.Parameters.Add("@pago_vComentario", SqlDbType.VarChar).Value = DTRePagos.Rows[intFila]["pago_vComentario"].ToString();
                        cmd.Parameters.Add("@pago_sOficinaConsularId", SqlDbType.SmallInt).Value = DTRePagos.Rows[intFila]["pago_sOficinaConsularId"].ToString();
                        cmd.Parameters.Add("@pago_sUsuarioCreacion", SqlDbType.SmallInt).Value = DTRePagos.Rows[intFila]["pago_sUsuarioCreacion"].ToString();
                        cmd.Parameters.Add("@pago_vIPCreacion", SqlDbType.VarChar).Value = DTRePagos.Rows[intFila]["pago_vIPCreacion"].ToString();
                        cmd.Parameters.Add("@pago_vHostName", SqlDbType.VarChar).Value = DTRePagos.Rows[intFila]["pago_vHostName"].ToString();
                        cmd.Parameters.Add("@pago_vNumeroVoucher", SqlDbType.VarChar).Value = DTRePagos.Rows[intFila]["pago_vNumeroVoucher"].ToString();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }



                if (iActuacionDetalleId != 0)
                {
                    intValor = (int)Enumerador.enmResultadoQuery.OK;
                }
            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intValor;
        }



        //**************************************************************************************

        public int Insertar(RE_ACTUACION ObjActuacBE,
                            DataTable DtDetActuaciones,
                            RE_PAGO ObjPagoBE,
                            ref long LonActuacionId)
        {
            long LonResultQueryPago, LonResultQueryActuacion, LonNumberRowsActuacionDetalleOK = 0;
            bool bolCancelar = false;

            try
            {
                SqlParameter[] prmParameterActuacion = new SqlParameter[11];

                prmParameterActuacion[0] = new SqlParameter("@actu_iActuacionId", SqlDbType.BigInt);
                prmParameterActuacion[0].Direction = ParameterDirection.Output;

                prmParameterActuacion[1] = new SqlParameter("@actu_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterActuacion[1].Value = ObjActuacBE.actu_sOficinaConsularId;

                prmParameterActuacion[2] = new SqlParameter("@actu_FCantidad", SqlDbType.Float);
                prmParameterActuacion[2].Value = ObjActuacBE.actu_FCantidad;

                prmParameterActuacion[3] = new SqlParameter("@actu_iPersonaRecurrenteId", SqlDbType.BigInt);
                prmParameterActuacion[3].Value = ObjActuacBE.actu_iPersonaRecurrenteId;

                prmParameterActuacion[4] = new SqlParameter("@actu_dFechaRegistro", SqlDbType.DateTime);
                prmParameterActuacion[4].Value = ObjActuacBE.actu_dFechaRegistro;

                prmParameterActuacion[5] = new SqlParameter("@actu_sEstado", SqlDbType.SmallInt);
                prmParameterActuacion[5].Value = ObjActuacBE.actu_sEstado;

                prmParameterActuacion[6] = new SqlParameter("@actu_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterActuacion[6].Value = ObjActuacBE.actu_sUsuarioCreacion;

                prmParameterActuacion[7] = new SqlParameter("@actu_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterActuacion[7].Value = ObjActuacBE.actu_vIPCreacion;

                prmParameterActuacion[8] = new SqlParameter("@actu_vHostName", SqlDbType.VarChar, 20);
                prmParameterActuacion[8].Value = Util.ObtenerHostName();

                prmParameterActuacion[9] = new SqlParameter("@actu_iEmpresaRecurrenteId", SqlDbType.BigInt);
                prmParameterActuacion[9].Value = ObjActuacBE.actu_iEmpresaRecurrenteId;

                prmParameterActuacion[10] = new SqlParameter("@actu_sCiudadItinerante", SqlDbType.SmallInt);
                prmParameterActuacion[10].Value = ObjActuacBE.actu_sCiudadItinerante;

                LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_ACTUACION_ADICIONAR",
                                                                    prmParameterActuacion);

                LonActuacionId = (long)prmParameterActuacion[0].Value;

                if (LonActuacionId > 0)
                {
                    ObjActuacBE.actu_iActuacionId = LonActuacionId;

                    long LonDetalleAct = 0;
                    if (ObjActuacBE.actu_iActuacionId > 0)
                    {
                        SqlParameter[] prmParameterActuacionDetalle;
                        if (DtDetActuaciones != null)
                        {
                            foreach (DataRow dr in DtDetActuaciones.Rows)
                            {
                                //prmParameterActuacionDetalle = new SqlParameter[22];
                                //--------------------------------------------
                                // Creador por: Miguel Angel Márquez Beltrán
                                // Fecha: 15-08-2016
                                // Objetivo: Adicionar la columna Clasificacion
                                //--------------------------------------------
                                prmParameterActuacionDetalle = new SqlParameter[23];
                                //--------------------------------------------


                                prmParameterActuacionDetalle[0] = new SqlParameter("@acde_iActuacionId", SqlDbType.BigInt);
                                prmParameterActuacionDetalle[0].Value = ObjActuacBE.actu_iActuacionId;

                                prmParameterActuacionDetalle[1] = new SqlParameter("@acde_sTarifarioId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[1].Value = dr[0];

                                prmParameterActuacionDetalle[2] = new SqlParameter("@acde_sItem", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[2].Value = dr[1];

                                prmParameterActuacionDetalle[3] = new SqlParameter("@acde_dFechaRegistro", SqlDbType.DateTime);
                                prmParameterActuacionDetalle[3].Value = dr[2];

                                prmParameterActuacionDetalle[4] = new SqlParameter("@acde_bRequisitosFlag", SqlDbType.Bit);
                                prmParameterActuacionDetalle[4].Value = dr[3];

                                prmParameterActuacionDetalle[5] = new SqlParameter("@acde_IVinculacionInsumoId", SqlDbType.Int);
                                prmParameterActuacionDetalle[5].Value = dr[4];

                                prmParameterActuacionDetalle[6] = new SqlParameter("@acde_dVinculacionFecha", SqlDbType.DateTime);
                                prmParameterActuacionDetalle[6].Value = dr[5];

                                prmParameterActuacionDetalle[7] = new SqlParameter("@acde_ICorrelativoActuacion", SqlDbType.Int);
                                prmParameterActuacionDetalle[7].Value = dr[6];

                                prmParameterActuacionDetalle[8] = new SqlParameter("@acde_ICorrelativoTarifario", SqlDbType.Int);
                                prmParameterActuacionDetalle[8].Value = dr[7];

                                prmParameterActuacionDetalle[9] = new SqlParameter("@acde_sFuncionarioFirmanteId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[9].Value = dr[8];

                                prmParameterActuacionDetalle[10] = new SqlParameter("@acde_sFuncionarioContactoId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[10].Value = dr[9];

                                prmParameterActuacionDetalle[11] = new SqlParameter("@acde_bImpresionFlag", SqlDbType.Bit);
                                prmParameterActuacionDetalle[11].Value = dr[10];

                                if (dr[11] == null)
                                {
                                    prmParameterActuacionDetalle[12] = new SqlParameter("@acde_dImpresionFecha", SqlDbType.DateTime);
                                    prmParameterActuacionDetalle[12].Value = DBNull.Value;
                                }
                                else
                                {
                                    prmParameterActuacionDetalle[12] = new SqlParameter("@acde_dImpresionFecha", SqlDbType.DateTime);
                                    prmParameterActuacionDetalle[12].Value = dr[11];
                                }

                                prmParameterActuacionDetalle[13] = new SqlParameter("@acde_sImpresionFuncionarioId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[13].Value = dr[12];

                                prmParameterActuacionDetalle[14] = new SqlParameter("@acde_vNotas", SqlDbType.VarChar, 1000);
                                prmParameterActuacionDetalle[14].Value = dr[13];

                                prmParameterActuacionDetalle[15] = new SqlParameter("@acde_sEstadoId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[15].Value = ObjActuacBE.actu_sEstado;

                                prmParameterActuacionDetalle[16] = new SqlParameter("@acde_sOficinaConsularId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[16].Value = ObjActuacBE.actu_sOficinaConsularId;

                                prmParameterActuacionDetalle[17] = new SqlParameter("@acde_sUsuarioCreacion", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[17].Value = ObjActuacBE.actu_sUsuarioCreacion;

                                prmParameterActuacionDetalle[18] = new SqlParameter("@acde_vIPCreacion", SqlDbType.VarChar, 50);
                                prmParameterActuacionDetalle[18].Value = ObjActuacBE.actu_vIPCreacion;

                                prmParameterActuacionDetalle[19] = new SqlParameter("@acde_vHostName", SqlDbType.VarChar, 20);
                                prmParameterActuacionDetalle[19].Value = Util.ObtenerHostName();

                                prmParameterActuacionDetalle[20] = new SqlParameter("@acde_iReferenciaId", SqlDbType.BigInt, 20);
                                if (ObjPagoBE.pago_iActuacionDetalleId != 0)
                                    prmParameterActuacionDetalle[20].Value = ObjPagoBE.pago_iActuacionDetalleId;
                                else prmParameterActuacionDetalle[20].Value = null;

                                prmParameterActuacionDetalle[21] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.BigInt);
                                prmParameterActuacionDetalle[21].Direction = ParameterDirection.Output;

                                //--------------------------------------------
                                // Creador por: Miguel Angel Márquez Beltrán
                                // Fecha: 15-08-2016
                                // Objetivo: Adicionar la columna Clasificacion
                                //--------------------------------------------
                                prmParameterActuacionDetalle[22] = new SqlParameter("@acde_sClasificacionTarifaId", SqlDbType.SmallInt);
                                prmParameterActuacionDetalle[22].Value = dr[14];
                                //--------------------------------------------


                                LonNumberRowsActuacionDetalleOK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                            CommandType.StoredProcedure,
                                                                                            "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_ADICIONAR",
                                                                                            prmParameterActuacionDetalle);

                                LonDetalleAct = (long)prmParameterActuacionDetalle[21].Value;

                                if (LonDetalleAct > 0)
                                {
                                    SqlParameter[] prmParameterPago = new SqlParameter[19];

                                    prmParameterPago[0] = new SqlParameter("@pago_sPagoTipoId", SqlDbType.SmallInt);
                                    prmParameterPago[0].Value = ObjPagoBE.pago_sPagoTipoId;

                                    prmParameterPago[1] = new SqlParameter("@pago_iActuacionDetalleId", SqlDbType.BigInt);
                                    prmParameterPago[1].Value = LonDetalleAct;

                                    prmParameterPago[2] = new SqlParameter("@pago_dFechaOperacion", SqlDbType.DateTime);
                                    prmParameterPago[2].Value = ObjPagoBE.pago_dFechaOperacion;

                                    prmParameterPago[3] = new SqlParameter("@pago_sMonedaLocalId", SqlDbType.SmallInt);
                                    prmParameterPago[3].Value = ObjPagoBE.pago_sMonedaLocalId;

                                    prmParameterPago[4] = new SqlParameter("@pago_FMontoMonedaLocal", SqlDbType.Float);
                                    prmParameterPago[4].Value = ObjPagoBE.pago_FMontoMonedaLocal;

                                    prmParameterPago[5] = new SqlParameter("@pago_FMontoSolesConsulares", SqlDbType.Float);
                                    prmParameterPago[5].Value = ObjPagoBE.pago_FMontoSolesConsulares;

                                    prmParameterPago[6] = new SqlParameter("@pago_FTipCambioBancario", SqlDbType.Float);
                                    prmParameterPago[6].Value = ObjPagoBE.pago_FTipCambioBancario;

                                    prmParameterPago[7] = new SqlParameter("@pago_FTipCambioConsular", SqlDbType.Float);
                                    prmParameterPago[7].Value = ObjPagoBE.pago_FTipCambioConsular;

                                    prmParameterPago[8] = new SqlParameter("@pago_sBancoId", SqlDbType.SmallInt);
                                    prmParameterPago[8].Value = ObjPagoBE.pago_sBancoId;

                                    prmParameterPago[9] = new SqlParameter("@pago_vBancoNumeroOperacion", SqlDbType.VarChar, 50);
                                    prmParameterPago[9].Value = ObjPagoBE.pago_vBancoNumeroOperacion;

                                    prmParameterPago[10] = new SqlParameter("@pago_bPagadoFlag", SqlDbType.Bit);
                                    prmParameterPago[10].Value = ObjPagoBE.pago_bPagadoFlag;

                                    prmParameterPago[11] = new SqlParameter("@pago_vComentario", SqlDbType.VarChar, 1000);
                                    prmParameterPago[11].Value = ObjPagoBE.pago_vComentario;

                                    prmParameterPago[12] = new SqlParameter("@pago_sOficinaConsularId", SqlDbType.SmallInt);
                                    prmParameterPago[12].Value = ObjActuacBE.actu_sOficinaConsularId;

                                    prmParameterPago[13] = new SqlParameter("@pago_sUsuarioCreacion", SqlDbType.SmallInt);
                                    prmParameterPago[13].Value = ObjActuacBE.actu_sUsuarioCreacion;

                                    prmParameterPago[14] = new SqlParameter("@pago_vIPCreacion", SqlDbType.VarChar, 50);
                                    prmParameterPago[14].Value = ObjActuacBE.actu_vIPCreacion;

                                    prmParameterPago[15] = new SqlParameter("@pago_vHostName", SqlDbType.VarChar, 20);
                                    prmParameterPago[15].Value = Util.ObtenerHostName();

                                    prmParameterPago[16] = new SqlParameter("@pago_vNumeroVoucher", SqlDbType.VarChar, 20);
                                    prmParameterPago[16].Value = DBNull.Value;

                                    prmParameterPago[17] = new SqlParameter("@pago_vSustentoTipoPago", SqlDbType.VarChar, 200);
                                    prmParameterPago[17].Value = ObjPagoBE.pago_vSustentoTipoPago;

                                    prmParameterPago[18] = new SqlParameter("@pago_iNormaTarifarioId", SqlDbType.BigInt);
                                    prmParameterPago[18].Value = ObjPagoBE.pago_iNormaTarifarioId;

                                    LonResultQueryPago = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                    CommandType.StoredProcedure,
                                                                                    "PN_REGISTRO.USP_RE_PAGO_ADICIONAR",
                                                                                    prmParameterPago);

                                    ObjPagoBE.pago_iActuacionDetalleId = LonDetalleAct;
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int EliminarParticipante(Int64 Participante)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_ELIMINAR_PARTICIPANTES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acpa_iActuacionParticipanteId", Participante));
                        
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Actualizar(RE_ACTUACION ObjActuacBE,
                              RE_ACTUACIONDETALLE ObjActuacDetBE, Int16 clasificacionTarifa = 0)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionDetalleId", ObjActuacDetBE.acde_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@acde_vNotas", ObjActuacDetBE.acde_vNotas));
                        cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", ObjActuacBE.actu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@actu_sUsuarioModificacion", ObjActuacBE.actu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vIPModificacion", ObjActuacBE.actu_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@acde_sClasificacionTarifaId", clasificacionTarifa));
                            
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Reasignar(RE_ACTUACION ObjActuacBE, Int16 sTarifarioID, ref string strMensaje,Int64 Acde_iActuacionDetalleId = 0)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_REASIGNAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@actu_iActuacionId", ObjActuacBE.actu_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", ObjActuacBE.actu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", ObjActuacBE.actu_iPersonaRecurrenteId));
                        cmd.Parameters.Add(new SqlParameter("@actu_sEstado", ObjActuacBE.actu_sEstado));
                        cmd.Parameters.Add(new SqlParameter("@actu_sUsuarioModificacion", ObjActuacBE.actu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vIPModificacion", ObjActuacBE.actu_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@acde_sTarifarioId", sTarifarioID));
                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionDetalleId", Acde_iActuacionDetalleId));

                        SqlParameter sqlParMensaje = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 200);
                        sqlParMensaje.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = Convert.ToString(sqlParMensaje.Value);
                    }
                }               
                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ActualizarEstado(RE_ACTUACION ObjActuacBE,
                                    RE_ACTUACIONDETALLE ObjActuacDetBE)
        {
            long LonResultQueryActuacion;

            try
            {
                SqlParameter[] prmParameterActuacion = new SqlParameter[7];

                prmParameterActuacion[0] = new SqlParameter("@actu_iActuacionId", SqlDbType.BigInt);
                prmParameterActuacion[0].Value = ObjActuacBE.actu_iActuacionId;

                prmParameterActuacion[1] = new SqlParameter("@actu_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterActuacion[1].Value = ObjActuacBE.actu_sOficinaConsularId;

                prmParameterActuacion[2] = new SqlParameter("@acde_sTarifarioId", SqlDbType.SmallInt);
                prmParameterActuacion[2].Value = ObjActuacDetBE.acde_sTarifarioId;

                prmParameterActuacion[3] = new SqlParameter("@actu_sEstado", SqlDbType.SmallInt);
                prmParameterActuacion[3].Value = ObjActuacBE.actu_sEstado;

                prmParameterActuacion[4] = new SqlParameter("@actu_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterActuacion[4].Value = ObjActuacBE.actu_sUsuarioModificacion;

                prmParameterActuacion[5] = new SqlParameter("@actu_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterActuacion[5].Value = ObjActuacBE.actu_vIPModificacion;

                prmParameterActuacion[6] = new SqlParameter("@actu_vHostName", SqlDbType.VarChar, 20);
                prmParameterActuacion[6].Value = Util.ObtenerHostName();

                LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_ACTUACION_ACTUALIZAR_ESTADO",
                                                                    prmParameterActuacion);
                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 09/09/2016
        // Objetivo: Se adiciono dos parametros de entrada(@acde_IFuncionarioAnulaId y @acde_vMotivoAnulacion)
        //           en vez de: @actu_vUsuarioAutoriza y  @actu_vMotivos
        //------------------------------------------------------------------------

        //public int Anular(RE_ACTUACION ObjActuacBE,
        //                  RE_ACTUACIONDETALLE ObjActuacDetBE)
        //{
        //    long LonResultQueryActuacion;

        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[8];

        //        prmParameterActuacion[0] = new SqlParameter("@actu_iActuacionId", SqlDbType.BigInt);
        //        prmParameterActuacion[0].Value = ObjActuacBE.actu_iActuacionId;

        //        prmParameterActuacion[1] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterActuacion[1].Value = ObjActuacDetBE.acde_iActuacionDetalleId;

        //        prmParameterActuacion[2] = new SqlParameter("@actu_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterActuacion[2].Value = ObjActuacBE.actu_sOficinaConsularId;

        //        prmParameterActuacion[3] = new SqlParameter("@actu_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameterActuacion[3].Value = ObjActuacBE.actu_sUsuarioModificacion;

        //        prmParameterActuacion[4] = new SqlParameter("@actu_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameterActuacion[4].Value = ObjActuacBE.actu_vIPModificacion;

        //        prmParameterActuacion[5] = new SqlParameter("@actu_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterActuacion[5].Value = Util.ObtenerHostName();

        //        prmParameterActuacion[6] = new SqlParameter("@acde_IFuncionarioAnulaId", SqlDbType.Int);
        //        prmParameterActuacion[6].Value = ObjActuacDetBE.acde_IFuncionarioAnulaId;

        //        prmParameterActuacion[7] = new SqlParameter("@acde_vMotivoAnulacion", SqlDbType.VarChar, 500);
        //        prmParameterActuacion[7].Value = ObjActuacDetBE.acde_vMotivoAnulacion;

        //        LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                          CommandType.StoredProcedure,
        //                                                          "PN_REGISTRO.USP_RE_ACTUACION_ELIMINAR",
        //                                                          prmParameterActuacion);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 23/12/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------

        public int Anular(RE_ACTUACION ObjActuacBE,
                          RE_ACTUACIONDETALLE ObjActuacDetBE)
        {
            int intValor = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@actu_iActuacionId", SqlDbType.BigInt).Value = ObjActuacBE.actu_iActuacionId;
                        cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjActuacDetBE.acde_iActuacionDetalleId;
                        cmd.Parameters.Add("@actu_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjActuacBE.actu_sOficinaConsularId;
                        cmd.Parameters.Add("@actu_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjActuacBE.actu_sUsuarioModificacion;
                        cmd.Parameters.Add("@actu_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjActuacBE.actu_vIPModificacion;
                        cmd.Parameters.Add("@actu_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@acde_IFuncionarioAnulaId", SqlDbType.Int).Value = ObjActuacDetBE.acde_IFuncionarioAnulaId;
                        cmd.Parameters.Add("@acde_vMotivoAnulacion", SqlDbType.VarChar, 500).Value = ObjActuacDetBE.acde_vMotivoAnulacion;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                intValor = (int)Enumerador.enmResultadoQuery.OK;
                
            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intValor;
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 07/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public int VincularAutoadhesivo(Int64 intActuacionId, Int64 intActuacionDetalleId,
            int intInsumoTipoId, string strCodAutoadhesivo,
            DateTime datFechaVinculacion, bool bolImpreso,
            DateTime datFechaImpresion, int intFuncionarioImprimeId,
            int intOficinaConsularId, int intUsuarioModificacionId, ref string strMensaje, Int16 codigoCiudadItinerante = 0)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_VINCULAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@iActuacionId", SqlDbType.BigInt).Value = intActuacionId;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = intActuacionDetalleId;
                        cmd.Parameters.Add("@sInsumoTipoId", SqlDbType.SmallInt).Value = intInsumoTipoId;
                        cmd.Parameters.Add("@vCodigoAutoadhesivo", SqlDbType.VarChar, 20).Value = strCodAutoadhesivo;
                        cmd.Parameters.Add("@dFechaVinculacion", SqlDbType.DateTime).Value = datFechaVinculacion;
                        cmd.Parameters.Add("@bImpresionFlag", SqlDbType.Bit).Value = bolImpreso;
                        cmd.Parameters.Add("@dFechaImpresion", SqlDbType.DateTime).Value = datFechaImpresion;
                        cmd.Parameters.Add("@sImpresionFuncionarioId", SqlDbType.SmallInt).Value = intFuncionarioImprimeId;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@sUsuarioModificacion", SqlDbType.SmallInt).Value = intUsuarioModificacionId;
                        cmd.Parameters.Add("@vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@actu_sCiudadItinerante", SqlDbType.SmallInt).Value = codigoCiudadItinerante;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@strMensaje", SqlDbType.VarChar, 200);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = lReturn1.Value.ToString();
                    }
                }
                #region Comentada
                #endregion
                if (strMensaje.Trim().Equals(""))
                {
                    return (int) Enumerador.enmResultadoQuery.OK;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int USP_RE_ACTUACIONINSUMODETALLE_ACTUALIZAR_IMPRESION(Int64 iActuacionInsumoDetalleId, Boolean bFlagImpresion,Int16 aide_sUsuarioCreacion, Int16 sOficinaConsular,  ref string strMensaje)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_ACTUACIONINSUMODETALLE_ACTUALIZAR_IMPRESION]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@aide_iActuacionInsumoDetalleId", iActuacionInsumoDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@aide_bFlagImpresion", bFlagImpresion));
                        cmd.Parameters.Add(new SqlParameter("@aide_sUsuarioCreacion", aide_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@aide_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsular", sOficinaConsular));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                 strMensaje = String.Empty;
                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (SqlException ex)
            {
                strMensaje = ex.Message.ToString();
                return (int)Enumerador.enmResultadoQuery.ERR;
            }
            catch (Exception e) {
                strMensaje = e.Message.ToString();
                return (int)Enumerador.enmResultadoQuery.ERR;
            }

             
        }
        public int Vincular_Insumo_Migratorio(Int64 intActuacionId, Int64 intActuacionDetalleId,
            int intInsumoTipoId, string strCodAutoadhesivo,
            DateTime datFechaVinculacion, bool bolImpreso,
            DateTime datFechaImpresion, int intFuncionarioImprimeId,
            int intOficinaConsularId, int intUsuarioModificacionId, string strCodAutoadhesivo_Separado, ref string strMensaje)
        {
            int intValor = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_VINCULAR_MIGRATORIO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@iActuacionId", SqlDbType.BigInt).Value = intActuacionId;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = intActuacionDetalleId;
                        cmd.Parameters.Add("@sInsumoTipoId", SqlDbType.SmallInt).Value = intInsumoTipoId;
                        cmd.Parameters.Add("@vCodigoAutoadhesivo", SqlDbType.VarChar).Value = strCodAutoadhesivo;
                        cmd.Parameters.Add("@dFechaVinculacion", SqlDbType.DateTime).Value = datFechaVinculacion;
                        cmd.Parameters.Add("@bImpresionFlag", SqlDbType.Bit).Value = bolImpreso;
                        cmd.Parameters.Add("@dFechaImpresion", SqlDbType.DateTime).Value = datFechaImpresion;
                        cmd.Parameters.Add("@sImpresionFuncionarioId", SqlDbType.SmallInt).Value = intFuncionarioImprimeId;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@sUsuarioModificacion", SqlDbType.SmallInt).Value = intUsuarioModificacionId;
                        cmd.Parameters.Add("@vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@vCodigoAutoadhesivoOld", SqlDbType.VarChar, 12).Value = strCodAutoadhesivo_Separado;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@strMensaje", SqlDbType.VarChar, 200);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = lReturn1.Value.ToString();

                    }
                }

                intValor = (int)Enumerador.enmResultadoQuery.OK;
                
            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intValor;
        }

        //public string ObtenerFormularioPorTarifa(Int16 intTarifaId, int intAccionId)
        //{
        //    string strFormulario = string.Empty;
        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[4];

        //        prmParameterActuacion[0] = new SqlParameter("@sTarifarioId", SqlDbType.SmallInt);
        //        prmParameterActuacion[0].Value = intTarifaId;

        //        prmParameterActuacion[1] = new SqlParameter("@sAccionId", SqlDbType.SmallInt);
        //        prmParameterActuacion[1].Value = intAccionId;

        //        prmParameterActuacion[2] = new SqlParameter("@vFormulario", SqlDbType.VarChar, 100);
        //        prmParameterActuacion[2].Direction = ParameterDirection.Output;

        //        prmParameterActuacion[3] = new SqlParameter("@vTabsInicial", SqlDbType.VarChar, 10);
        //        prmParameterActuacion[3].Direction = ParameterDirection.Output;

        //        SqlHelper.ExecuteNonQuery(StrConnectionName, CommandType.StoredProcedure,
        //                        "PN_REGISTRO.USP_RE_ACTUACION_FORMATO", prmParameterActuacion);

        //        string strNombreFormulario = prmParameterActuacion[2].Value.ToString();
        //        string strTabsHabilita = prmParameterActuacion[3].Value.ToString();
        //        strFormulario = strNombreFormulario + "-" + strTabsHabilita;

        //        return strFormulario;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 23/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------
  
        public string ObtenerFormularioPorTarifa(Int16 intTarifaId, int intAccionId)
        {
            string strFormulario = "";
            
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_FORMATO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = intTarifaId;
                        cmd.Parameters.Add("@sAccionId", SqlDbType.SmallInt).Value = intAccionId;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@vFormulario", SqlDbType.VarChar, 100);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@vTabsInicial", SqlDbType.VarChar, 10);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        string strNombreFormulario = lReturn1.Value.ToString();
                        string strTabsHabilita = lReturn2.Value.ToString();
                        strFormulario = strNombreFormulario + "-" + strTabsHabilita;
                    }
                }
                                

                return strFormulario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        //public DataTable Obtener_ActuacionInsumoDetalleAll(Int64 iActuacionDetalleId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = iActuacionDetalleId;

        //        prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[1].Value = ICurrentPag;

        //        prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[2].Value = IPageSize;

        //        prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[3].Direction = ParameterDirection.Output;

        //        prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                             "PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_CONSULTAR_MIGRATORIO",
        //                                             prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        ITotalRecords = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value);
        //        ITotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);

        //        return DtResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DtResult = null;
        //        DsResult = null;
        //    }
        //}
        //------------------------------------------------------------------
        //Fecha: 23/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataTable Obtener_ActuacionInsumoDetalleAll(Int64 iActuacionDetalleId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_CONSULTAR_MIGRATORIO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = iActuacionDetalleId;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = ICurrentPag;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IPageSize;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                        }
                        DtResult = DsResult.Tables[0];

                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);

                        return DtResult;
                    }
                }                               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }

        public DataTable Obtener_ActuacionInsumoDetalle(Int64 iActuacionDetalleId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@iActuacionDetalleId", iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                        }
                        DtResult = DsResult.Tables[0];

                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);

                        return DtResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }

        //public DataTable Obtener_ActuacionInsumo(Int64 iActuacionId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@iActuacionId", SqlDbType.BigInt);
        //        prmParameter[0].Value = iActuacionId;

        //        prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[1].Value = ICurrentPag;

        //        prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[2].Value = IPageSize;

        //        prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[3].Direction = ParameterDirection.Output;

        //        prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                             "PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_CONSULTAR_NOTARIAL",
        //                                             prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        ITotalRecords = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value);
        //        ITotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);

        //        return DtResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DtResult = null;
        //        DsResult = null;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 23/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        
        public DataTable Obtener_ActuacionInsumo(Int64 iActuacionId, Int32 ICurrentPag, Int32 IPageSize, ref Int32 ITotalRecords, ref Int32 ITotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_CONSULTAR_NOTARIAL", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionId", SqlDbType.BigInt).Value = iActuacionId;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = ICurrentPag;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IPageSize;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                        }
                        DtResult = DsResult.Tables[0];

                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);

                        return DtResult;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }

        // MDIAZ - 12/03/2015
        public Int64 InsertarActuacionDetalle(BE.RE_ACTUACIONDETALLE objActuacionDetalleBE)
        {
            Int64 intActuacionDetalleId = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionId", objActuacionDetalleBE.acde_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sTarifarioId", objActuacionDetalleBE.acde_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sItem", objActuacionDetalleBE.acde_sItem));
                        cmd.Parameters.Add(new SqlParameter("@acde_dFechaRegistro", objActuacionDetalleBE.acde_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@acde_bRequisitosFlag", objActuacionDetalleBE.acde_bRequisitosFlag));
                        cmd.Parameters.Add(new SqlParameter("@acde_ICorrelativoActuacion", objActuacionDetalleBE.acde_ICorrelativoActuacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_ICorrelativoTarifario", objActuacionDetalleBE.acde_ICorrelativoTarifario));
                        cmd.Parameters.Add(new SqlParameter("@acde_vNotas", objActuacionDetalleBE.acde_vNotas));
                        cmd.Parameters.Add(new SqlParameter("@acde_sEstadoId", objActuacionDetalleBE.acde_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sOficinaConsularId", objActuacionDetalleBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sUsuarioCreacion", objActuacionDetalleBE.acde_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_vIPCreacion", objActuacionDetalleBE.acde_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_vHostName", Util.ObtenerHostName()));

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intActuacionDetalleId = Convert.ToInt64(lReturn.Value);
                    }
                }
            }
            catch
            {
                throw;
            }

            return intActuacionDetalleId;
        }

        public int InsertarPago(BE.RE_PAGO objPagoBE)
        {
            int intResultado = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pago_sPagoTipoId", objPagoBE.pago_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", objPagoBE.pago_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaOperacion", objPagoBE.pago_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sMonedaLocalId", objPagoBE.pago_sMonedaLocalId));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoMonedaLocal", objPagoBE.pago_FMontoMonedaLocal));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoSolesConsulares", objPagoBE.pago_FMontoSolesConsulares));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioBancario", objPagoBE.pago_FTipCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioConsular", objPagoBE.pago_FTipCambioConsular));

                        if (objPagoBE.pago_sBancoId != null)
                        {
                            if (objPagoBE.pago_sBancoId != 0)
                            {
                                cmd.Parameters.Add(new SqlParameter("@pago_sBancoId", objPagoBE.pago_sBancoId));
                            }

                        }

                        cmd.Parameters.Add(new SqlParameter("@pago_vBancoNumeroOperacion", objPagoBE.pago_vBancoNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_bPagadoFlag", objPagoBE.pago_bPagadoFlag));
                        cmd.Parameters.Add(new SqlParameter("@pago_vComentario", objPagoBE.pago_vComentario));
                        cmd.Parameters.Add(new SqlParameter("@pago_sOficinaConsularId", objPagoBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@pago_sUsuarioCreacion", objPagoBE.pago_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vIPCreacion", objPagoBE.pago_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@pago_vNumeroVoucher", objPagoBE.pago_vNumeroVoucher));
                        cmd.Parameters.Add(new SqlParameter("@pago_vSustentoTipoPago", objPagoBE.pago_vSustentoTipoPago));
                        cmd.Parameters.Add(new SqlParameter("@pago_iNormaTarifarioId", objPagoBE.pago_iNormaTarifarioId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return intResultado;
        }

        public BE.MRE.RE_ACTUACIONINSUMODETALLE InsertarVinculacionInsumo(BE.MRE.RE_ACTUACIONINSUMODETALLE ActuacionInsumoDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@aide_iActuacionDetalleId", ActuacionInsumoDetalle.aide_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@aide_iInsumoId", ActuacionInsumoDetalle.aide_iInsumoId));
                        cmd.Parameters.Add(new SqlParameter("@aide_dFechaVinculacion", ActuacionInsumoDetalle.aide_dFechaVinculacion));
                        cmd.Parameters.Add(new SqlParameter("@aide_sUsuarioVinculacionId", ActuacionInsumoDetalle.aide_sUsuarioVinculacionId));
                        cmd.Parameters.Add(new SqlParameter("@aide_bFlagImpresion", ActuacionInsumoDetalle.aide_bFlagImpresion));
                        if (ActuacionInsumoDetalle.aide_dFechaImpresion != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@aide_dFechaImpresion", ActuacionInsumoDetalle.aide_dFechaImpresion));
                        else
                            cmd.Parameters.Add(new SqlParameter("@aide_dFechaImpresion", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@aide_cEstado", ActuacionInsumoDetalle.aide_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@aide_sUsuarioCreacion", ActuacionInsumoDetalle.aide_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@aide_vIPCreacion", ActuacionInsumoDetalle.aide_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@aide_dFechaCreacion", ActuacionInsumoDetalle.aide_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsular", ActuacionInsumoDetalle.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActuacionInsumoDetalle.HostName));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@aide_iActuacionInsumoDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActuacionInsumoDetalle.aide_iActuacionInsumoDetalleId = Convert.ToInt64(lReturn.Value);
                        ActuacionInsumoDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActuacionInsumoDetalle.Error = true;
                ActuacionInsumoDetalle.Message = exec.Message.ToString();
            }

            return ActuacionInsumoDetalle;
        }

        public string ActualizarTarifarioCorrelativo(SGAC.BE.MRE.RE_CORRELATIVO objCorrelativoBE, ref int iResultado)
        {
            string strMensaje = string.Empty;
            try
            {

                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_TARIFARIO_CORRELATIVO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@corr_sPeriodo", objCorrelativoBE.corr_sPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@corr_sOficinaConsularId", objCorrelativoBE.corr_sOficinaConsularId));

                        if (objCorrelativoBE.corr_sTarifarioId == 0)
                            cmd.Parameters.Add(new SqlParameter("@corr_sTarifarioId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@corr_sTarifarioId", objCorrelativoBE.corr_sTarifarioId));

                        cmd.Parameters.Add(new SqlParameter("@corr_ICorrelativo", objCorrelativoBE.corr_ICorrelativo));                                             
                        
                        cmd.Parameters.Add(new SqlParameter("@corr_sUsuarioCreacion", objCorrelativoBE.corr_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@corr_vIPCreacion", objCorrelativoBE.corr_vIPCreacion));

                        SqlParameter lReturn = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 200);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        iResultado = 1;      
                  
                        strMensaje = lReturn.Value.ToString();
                    }
                }

                
            }
            catch (Exception ex)
            {
                iResultado = 0;
                strError = ex.Message;
                objCorrelativoBE.Error = true;

            }

            return strMensaje;          
        }

        public DataTable ObtenerActuacionDetalleDeActuacion(Int64 intActuacionId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_NOTARIAL", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionId", intActuacionId));
                        cmd.Connection.Open();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                        }
                        DtResult = DsResult.Tables[0];

                        return DtResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }
        //---------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Actualiza Flag de envio de correo
        //---------------------------------------------------------------------------------------
        public int ActualizarFlagEnvioCorreo(RE_ACTUACION ObjActuacBE)
        {
            //long LonResultQueryActuacion;
            int intValor = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_MODIFICAR_ENVIO_CORREO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@actu_iActuacionId", SqlDbType.BigInt).Value = ObjActuacBE.actu_iActuacionId;
                        cmd.Parameters.Add("@actu_sOficinaConsularId", SqlDbType.BigInt).Value = ObjActuacBE.actu_sOficinaConsularId;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
                intValor = (int)Enumerador.enmResultadoQuery.OK;
                //SqlParameter[] prmParameterActuacion = new SqlParameter[2];

                //prmParameterActuacion[0] = new SqlParameter("@actu_iActuacionId", SqlDbType.BigInt);
                //prmParameterActuacion[0].Value = ObjActuacBE.actu_iActuacionId;

                //prmParameterActuacion[1] = new SqlParameter("@actu_sOficinaConsularId", SqlDbType.SmallInt);
                //prmParameterActuacion[1].Value = ObjActuacBE.actu_sOficinaConsularId;

                //LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
                //                                                    CommandType.StoredProcedure,
                //                                                    "PN_REGISTRO.USP_RE_ACTUACION_MODIFICAR_ENVIO_CORREO",
                //                                                    prmParameterActuacion);
                //return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                intValor = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intValor;
        }

        public DataTable Verificar_FichaRegistral(Int64 iActuacionDetalleId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_VERIFICAR_EXISTENCIA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@iActuacionDetalleId", iActuacionDetalleId));

                        cmd.Connection.Open();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                        }
                        DtResult = DsResult.Tables[0];

                        return DtResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }

        public int ReactivarActuacion(Int64 acde_iActuacionDetalleId, Int64 acde_iActuacionId, Int16 SUSUARIOMODIFICACION, Int16 SOFICINACONSULARID,string observacion)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_REACTIVAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_acde_iActuacionDetalleId", acde_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@P_actu_iActuacionId", acde_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@FIRE_SUSUARIOMODIFICACION", SUSUARIOMODIFICACION));
                        cmd.Parameters.Add(new SqlParameter("@FIRE_VIPMODIFICACION", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@OFCO_SOFICINACONSULARID", SOFICINACONSULARID));
                        cmd.Parameters.Add(new SqlParameter("@P_VOBSERVACION", observacion));
                        


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        return (int)Enumerador.enmResultadoQuery.OK;
                  
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //---------------------------------------------    
        //Fecha: 28/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Registrar la actuacion Detalle.
        //---------------------------------------------    

        public int Registro_ActuacionDetalle(SGAC.BE.MRE.RE_ACTUACIONDETALLE objAD, Int16 intsOficinaConsularId)
        {
            Int64 intActuacionDetalleId = 0;

            try
            {
                string strHostName = Util.ObtenerHostName();

                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    #region USP_RE_ACTUACIONDETALLE_ADICIONAR

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acde_iActuacionId", SqlDbType.BigInt).Value = objAD.acde_iActuacionId;
                        cmd.Parameters.Add("@acde_sTarifarioId", SqlDbType.SmallInt).Value = objAD.acde_sTarifarioId;
                        cmd.Parameters.Add("@acde_sItem", SqlDbType.SmallInt).Value = objAD.acde_sItem;
                        cmd.Parameters.Add("@acde_dFechaRegistro", SqlDbType.DateTime).Value = objAD.acde_dFechaRegistro;
                        cmd.Parameters.Add("@acde_bRequisitosFlag", SqlDbType.Bit).Value = objAD.acde_bRequisitosFlag;
                        cmd.Parameters.Add("@acde_ICorrelativoActuacion", SqlDbType.Int).Value = objAD.acde_ICorrelativoActuacion;
                        cmd.Parameters.Add("@acde_ICorrelativoTarifario", SqlDbType.Int).Value = objAD.acde_ICorrelativoTarifario;
                        cmd.Parameters.Add("@acde_vNotas", SqlDbType.VarChar, 1000).Value = objAD.acde_vNotas;
                        cmd.Parameters.Add("@acde_sEstadoId", SqlDbType.SmallInt).Value = objAD.acde_sEstadoId;
                        cmd.Parameters.Add("@acde_sOficinaConsularId", SqlDbType.SmallInt).Value = intsOficinaConsularId;
                        cmd.Parameters.Add("@acde_sUsuarioCreacion", SqlDbType.SmallInt).Value = objAD.acde_sUsuarioCreacion;
                        cmd.Parameters.Add("@acde_vIPCreacion", SqlDbType.VarChar, 50).Value = objAD.acde_vIPCreacion;
                        cmd.Parameters.Add("@acde_vHostName", SqlDbType.VarChar, 20).Value = strHostName;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intActuacionDetalleId = Convert.ToInt64(lReturn1.Value);
                        objAD.acde_iActuacionDetalleId = intActuacionDetalleId;
                        objAD.Error = false;
                    }
                    #endregion

                }
                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                objAD.Error = true;
                objAD.Message = exec.Message.ToString();
                throw exec;
            }
        }

        //---------------------------------------------    
        //Fecha: 28/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Registrar el pago.
        //---------------------------------------------    

        public int Registro_Pago(SGAC.BE.MRE.RE_PAGO objPago, Int16 intsOficinaConsularId, Int64 intActuacionDetalleId)
        {

            try
            {
                string strHostName = Util.ObtenerHostName();

                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {                        
                   #region USP_RE_PAGO_ADICIONAR

                        using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_ADICIONAR", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@pago_sPagoTipoId", SqlDbType.SmallInt).Value = objPago.pago_sPagoTipoId;
                            cmd.Parameters.Add("@pago_iActuacionDetalleId", SqlDbType.BigInt).Value = intActuacionDetalleId;
                            cmd.Parameters.Add("@pago_dFechaOperacion", SqlDbType.DateTime).Value = objPago.pago_dFechaOperacion;
                            cmd.Parameters.Add("@pago_sMonedaLocalId", SqlDbType.SmallInt).Value = objPago.pago_sMonedaLocalId;
                            cmd.Parameters.Add("@pago_FMontoMonedaLocal", SqlDbType.Float).Value = objPago.pago_FMontoMonedaLocal;
                            cmd.Parameters.Add("@pago_FMontoSolesConsulares", SqlDbType.Float).Value = objPago.pago_FMontoSolesConsulares;
                            cmd.Parameters.Add("@pago_FTipCambioBancario", SqlDbType.Float).Value = objPago.pago_FTipCambioBancario;
                            cmd.Parameters.Add("@pago_FTipCambioConsular", SqlDbType.Float).Value = objPago.pago_FTipCambioConsular;

                            if (Convert.ToInt16(objPago.pago_sBancoId.ToString()) == 0)
                            {
                                cmd.Parameters.Add("@pago_sBancoId", SqlDbType.SmallInt).Value = null;
                            }
                            else
                            {
                                cmd.Parameters.Add("@pago_sBancoId", SqlDbType.SmallInt).Value = objPago.pago_sBancoId.ToString();
                            }
                            cmd.Parameters.Add("@pago_vBancoNumeroOperacion", SqlDbType.VarChar, 50).Value = objPago.pago_vBancoNumeroOperacion;
                            cmd.Parameters.Add("@pago_bPagadoFlag", SqlDbType.Bit).Value = objPago.pago_bPagadoFlag;
                            cmd.Parameters.Add("@pago_vComentario", SqlDbType.VarChar, 1000).Value = objPago.pago_vComentario;
                            cmd.Parameters.Add("@pago_sOficinaConsularId", SqlDbType.SmallInt).Value = intsOficinaConsularId;
                            cmd.Parameters.Add("@pago_sUsuarioCreacion", SqlDbType.SmallInt).Value = objPago.pago_sUsuarioCreacion;
                            cmd.Parameters.Add("@pago_vIPCreacion", SqlDbType.VarChar, 50).Value = objPago.pago_vIPCreacion;
                            cmd.Parameters.Add("@pago_vHostName", SqlDbType.VarChar, 20).Value = strHostName;
                            cmd.Parameters.Add("@pago_vNumeroVoucher", SqlDbType.VarChar, 20).Value = objPago.pago_vNumeroVoucher;
                            cmd.Parameters.Add("@pago_vSustentoTipoPago", SqlDbType.VarChar, 200).Value = objPago.pago_vSustentoTipoPago;
                            cmd.Parameters.Add("@pago_iNormaTarifarioId", SqlDbType.BigInt).Value = objPago.pago_iNormaTarifarioId;

                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();

                            objPago.Error = false;

                        }
                        #endregion                       
                }
                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                objPago.Error = true;
                objPago.Message = exec.Message.ToString();
                
                throw exec;
            }
        }

        //---------------------------------------------    
        //Fecha: 28/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Registrar el Acto Notarial Detalle
        //---------------------------------------------    

        public int Registro_ActoNotarialDetalle(SGAC.BE.MRE.RE_ACTONOTARIALDETALLE objANDE, Int16 intsOficinaConsularId,
                            Int64 intActuacionId, Int64 intActuacionDetalleId)
        {
           
            try
            {
                string strHostName = Util.ObtenerHostName();

                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
               
                    #region USP_RE_ACTONOTARIALDETALLE_ADICIONAR

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDETALLE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ande_iActuacionId", SqlDbType.BigInt).Value = intActuacionId;
                        cmd.Parameters.Add("@ande_iActuacionDetalleId", SqlDbType.BigInt).Value = intActuacionDetalleId;
                        cmd.Parameters.Add("@ande_sOficinaConsularId", SqlDbType.SmallInt).Value = intsOficinaConsularId;
                        cmd.Parameters.Add("@ande_sTipoFormatoId", SqlDbType.SmallInt).Value = objANDE.ande_sTipoFormatoId;

                        if (objANDE.ande_IFuncionarioAutorizadorId == null)
                        {
                            cmd.Parameters.Add("@ande_IFuncionarioAutorizadorId", SqlDbType.Int).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@ande_IFuncionarioAutorizadorId", SqlDbType.Int).Value = objANDE.ande_IFuncionarioAutorizadorId;
                        }

                        if (objANDE.ande_vNumeroOficio == null)
                        {
                            cmd.Parameters.Add("@ande_vNumeroOficio", SqlDbType.VarChar, 20).Value = "";
                        }
                        else
                        {
                            cmd.Parameters.Add("@ande_vNumeroOficio", SqlDbType.VarChar, 20).Value = objANDE.ande_vNumeroOficio;
                        }
                        cmd.Parameters.Add("@ande_sNumeroFoja", SqlDbType.SmallInt).Value = objANDE.ande_sNumeroFoja;
                        cmd.Parameters.Add("@ande_vPresentanteNombre", SqlDbType.VarChar, 200).Value = objANDE.ande_vPresentanteNombre;
                        cmd.Parameters.Add("@ande_sPresentanteTipoDocumento", SqlDbType.SmallInt).Value = objANDE.ande_sPresentanteTipoDocumento;
                        cmd.Parameters.Add("@ande_vPresentanteNumeroDocumento", SqlDbType.VarChar, 20).Value = objANDE.ande_vPresentanteNumeroDocumento;
                        cmd.Parameters.Add("@ande_sPresentanteGenero", SqlDbType.SmallInt).Value = objANDE.ande_sPresentanteGenero;

                        if (objANDE.ande_dFechaExtension != DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@ande_dFechaExtension", SqlDbType.DateTime).Value = objANDE.ande_dFechaExtension;
                        }

                        cmd.Parameters.Add("@ande_sUsuarioCreacion", SqlDbType.SmallInt).Value = objANDE.ande_sUsuarioCreacion;
                        cmd.Parameters.Add("@ande_vIPCreacion", SqlDbType.VarChar, 50).Value = objANDE.ande_vIPCreacion;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = strHostName;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ande_iActoNotarialDetalleId", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        objANDE.ande_iActoNotarialDetalleId = Convert.ToInt64(lReturn1.Value);
                        objANDE.Error = false;
                    }

                    #endregion
                       
                }
                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                objANDE.Error = true;
                objANDE.Message = exec.Message.ToString();
                throw exec;
            }
        }
//---------------------------------------------    
    }
}
