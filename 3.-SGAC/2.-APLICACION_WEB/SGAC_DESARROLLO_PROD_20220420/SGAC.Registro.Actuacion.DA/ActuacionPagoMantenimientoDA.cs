using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionPagoMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionPagoMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionPagoMantenimientoDA()
        {
            GC.Collect();
        }

        public int Actualizar(RE_PAGO ObjPagoBE,
                              int IntOficinaConsularId)
        {
            try
            {

                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pago_iPagoId", ObjPagoBE.pago_iPagoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_sPagoTipoId", ObjPagoBE.pago_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", ObjPagoBE.pago_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaOperacion", ObjPagoBE.pago_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sMonedaLocalId", ObjPagoBE.pago_sMonedaLocalId));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoMonedaLocal", ObjPagoBE.pago_FMontoMonedaLocal));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoSolesConsulares", ObjPagoBE.pago_FMontoSolesConsulares));                        
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioBancario", ObjPagoBE.pago_FTipCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioConsular", ObjPagoBE.pago_FTipCambioConsular));
                        cmd.Parameters.Add(new SqlParameter("@pago_sBancoId", ObjPagoBE.pago_sBancoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_vBancoNumeroOperacion", ObjPagoBE.pago_vBancoNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vNumeroVoucher", ObjPagoBE.pago_vNumeroVoucher));
                        cmd.Parameters.Add(new SqlParameter("@pago_bPagadoFlag", ObjPagoBE.pago_bPagadoFlag));
                        cmd.Parameters.Add(new SqlParameter("@pago_vComentario", ObjPagoBE.pago_vComentario));
                        cmd.Parameters.Add(new SqlParameter("@pago_sOficinaConsularId", IntOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@pago_sUsuarioModificacion", ObjPagoBE.pago_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vIPModificacion", ObjPagoBE.pago_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vHostName", Util.ObtenerHostName()));                        
                        cmd.Parameters.Add(new SqlParameter("@pago_cEstado", "A"));
                        cmd.Parameters.Add(new SqlParameter("@pago_vSustentoTipoPago", ObjPagoBE.pago_vSustentoTipoPago));
                        cmd.Parameters.Add(new SqlParameter("@pago_iNormaTarifarioId", ObjPagoBE.pago_iNormaTarifarioId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                //SqlParameter[] prmParameterPago = new SqlParameter[19];
                //prmParameterPago[0] = new SqlParameter("@pago_iPagoId", SqlDbType.BigInt);
                //prmParameterPago[0].Value = ObjPagoBE.pago_iPagoId;
                //prmParameterPago[1] = new SqlParameter("@pago_sPagoTipoId", SqlDbType.SmallInt);
                //prmParameterPago[1].Value = ObjPagoBE.pago_sPagoTipoId;
                //prmParameterPago[2] = new SqlParameter("@pago_iActuacionDetalleId", SqlDbType.BigInt);
                //prmParameterPago[2].Value = ObjPagoBE.pago_iActuacionDetalleId;
                //prmParameterPago[3] = new SqlParameter("@pago_dFechaOperacion", SqlDbType.DateTime);
                //prmParameterPago[3].Value = ObjPagoBE.pago_dFechaOperacion;
                //prmParameterPago[4] = new SqlParameter("@pago_sMonedaLocalId", SqlDbType.SmallInt);
                //prmParameterPago[4].Value = ObjPagoBE.pago_sMonedaLocalId;
                //prmParameterPago[5] = new SqlParameter("@pago_FMontoMonedaLocal", SqlDbType.Float);
                //prmParameterPago[5].Value = ObjPagoBE.pago_FMontoMonedaLocal;
                //prmParameterPago[6] = new SqlParameter("@pago_FMontoSolesConsulares", SqlDbType.Float);
                //prmParameterPago[6].Value = ObjPagoBE.pago_FMontoSolesConsulares;
                //prmParameterPago[7] = new SqlParameter("@pago_FTipCambioBancario", SqlDbType.SmallInt);
                //prmParameterPago[7].Value = ObjPagoBE.pago_FTipCambioBancario;
                //prmParameterPago[8] = new SqlParameter("@pago_FTipCambioConsular", SqlDbType.SmallInt);
                //prmParameterPago[8].Value = ObjPagoBE.pago_FTipCambioConsular;
                //prmParameterPago[9] = new SqlParameter("@pago_sBancoId", SqlDbType.SmallInt);
                //prmParameterPago[9].Value = ObjPagoBE.pago_sBancoId;
                //prmParameterPago[10] = new SqlParameter("@pago_vBancoNumeroOperacion", SqlDbType.VarChar, 50);
                //prmParameterPago[10].Value = ObjPagoBE.pago_vBancoNumeroOperacion;
                //prmParameterPago[11] = new SqlParameter("@pago_bPagadoFlag", SqlDbType.Bit);
                //prmParameterPago[11].Value = ObjPagoBE.pago_bPagadoFlag;
                //prmParameterPago[12] = new SqlParameter("@pago_vComentario", SqlDbType.VarChar, 1000);
                //prmParameterPago[12].Value = ObjPagoBE.pago_vComentario;
                //prmParameterPago[13] = new SqlParameter("@pago_sOficinaConsularId", SqlDbType.SmallInt);
                //prmParameterPago[13].Value = IntOficinaConsularId;
                //prmParameterPago[14] = new SqlParameter("@pago_sUsuarioModificacion", SqlDbType.SmallInt);
                //prmParameterPago[14].Value = ObjPagoBE.pago_sUsuarioModificacion;
                //prmParameterPago[15] = new SqlParameter("@pago_vIPModificacion", SqlDbType.VarChar, 50);
                //prmParameterPago[15].Value = ObjPagoBE.pago_vIPModificacion;
                //prmParameterPago[16] = new SqlParameter("@pago_vHostName", SqlDbType.VarChar, 20);
                //prmParameterPago[16].Value = Util.ObtenerHostName();
                //prmParameterPago[17] = new SqlParameter("@pago_vNumeroVoucher", SqlDbType.VarChar, 20);
                //prmParameterPago[17].Value = ObjPagoBE.pago_vNumeroVoucher;
                //prmParameterPago[18] = new SqlParameter("@pago_cEstado", SqlDbType.Char, 1);
                //prmParameterPago[18].Value = "A";

                //LonResultQueryPago = SqlHelper.ExecuteNonQuery(StrConnectionName,
                //                                                CommandType.StoredProcedure,
                //                                                "PN_REGISTRO.USP_RE_PAGO_ACTUALIZAR",
                //                                                prmParameterPago);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}