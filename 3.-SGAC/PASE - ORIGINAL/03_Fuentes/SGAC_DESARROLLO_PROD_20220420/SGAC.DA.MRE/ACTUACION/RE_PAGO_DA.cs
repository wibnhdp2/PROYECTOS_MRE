using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Configuration;
using System.Reflection;

namespace SGAC.DA.MRE.ACTUACION
{
    public class RE_PAGO_DA
    {
        public string strError = string.Empty;
        public BE.MRE.RE_PAGO insertar(BE.MRE.RE_PAGO pago)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_ADICIONAR_NOTARIAL", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros                        
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", pago.pago_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@pago_sPagoTipoId", pago.pago_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaOperacion", pago.pago_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sMonedaLocalId", pago.pago_sMonedaLocalId));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoMonedaLocal", pago.pago_FMontoMonedaLocal));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoSolesConsulares", pago.pago_FMontoSolesConsulares));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioBancario", pago.pago_FTipCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioConsular", pago.pago_FTipCambioConsular));
                        cmd.Parameters.Add(new SqlParameter("@pago_sBancoId", pago.pago_sBancoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_vBancoNumeroOperacion", pago.pago_vBancoNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_bPagadoFlag", pago.pago_bPagadoFlag));
                        cmd.Parameters.Add(new SqlParameter("@pago_vComentario", pago.pago_vComentario));
                        cmd.Parameters.Add(new SqlParameter("@pago_vNumeroVoucher", pago.pago_vNumeroVoucher));
                        cmd.Parameters.Add(new SqlParameter("@pago_cEstado", pago.pago_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@pago_sUsuarioCreacion", pago.pago_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vIPCreacion", pago.pago_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaCreacion", pago.pago_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sUsuarioModificacion", pago.pago_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vIPModificacion", pago.pago_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaModificacion", pago.pago_dFechaModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sOficinaConsularId", pago.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pago_vHostName", pago.HostName));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@pago_iPagoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        pago.pago_iPagoId = Convert.ToInt64(lReturn.Value);
                        pago.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                pago.Error = true;
                pago.Message = exec.Message.ToString();
            }
            return pago;
        }
        
        public BE.MRE.RE_PAGO Actualizar(BE.MRE.RE_PAGO pago)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@pago_sPagoTipoId", pago.pago_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", pago.pago_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaOperacion", pago.pago_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sMonedaLocalId", pago.pago_sMonedaLocalId));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoMonedaLocal", pago.pago_FMontoMonedaLocal));
                        cmd.Parameters.Add(new SqlParameter("@pago_FMontoSolesConsulares", pago.pago_FMontoSolesConsulares));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioBancario", pago.pago_FTipCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@pago_FTipCambioConsular", pago.pago_FTipCambioConsular));
                        cmd.Parameters.Add(new SqlParameter("@pago_sBancoId", pago.pago_sBancoId));
                        cmd.Parameters.Add(new SqlParameter("@pago_vBancoNumeroOperacion", pago.pago_vBancoNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_bPagadoFlag", pago.pago_bPagadoFlag));
                        cmd.Parameters.Add(new SqlParameter("@pago_vComentario", pago.pago_vComentario));
                        cmd.Parameters.Add(new SqlParameter("@pago_sOficinaConsularId", pago.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pago_sUsuarioModificacion", pago.pago_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vIPModificacion", pago.pago_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_vHostName", pago.HostName));
                        cmd.Parameters.Add(new SqlParameter("@pago_vNumeroVoucher", pago.pago_vNumeroVoucher));
                        cmd.Parameters.Add(new SqlParameter("@pago_cEstado", pago.pago_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@pago_iPagoId", pago.pago_iPagoId));
                        
                        #endregion
                        
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        pago.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                pago.Error = true;
                pago.Message = exec.Message.ToString();
            }
            return pago;
        }

        public RE_PAGO obtener(RE_PAGO pago)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_OBTENER_NOTARIAL", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (pago.pago_iPagoId != 0) cmd.Parameters.Add(new SqlParameter("@pago_iPagoId", pago.pago_iPagoId));
                        if (pago.pago_iActuacionDetalleId != 0) cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", pago.pago_iActuacionDetalleId));
                        if (pago.pago_sPagoTipoId != 0) cmd.Parameters.Add(new SqlParameter("@pago_sPagoTipoId", pago.pago_sPagoTipoId));
                        if (pago.pago_sBancoId != 0) cmd.Parameters.Add(new SqlParameter("@pago_sBancoId", pago.pago_sBancoId));
                        if (pago.pago_vBancoNumeroOperacion != null) cmd.Parameters.Add(new SqlParameter("@pago_vBancoNumeroOperacion", pago.pago_vBancoNumeroOperacion));                       
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)pago.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(pago, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }

                        pago.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                pago.Error = true;
                pago.Message = exec.Message.ToString();
            }

            return pago;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
                
        public DataTable ObtenerPago(String intAcuacionId, Int16? intOficinaConsularId)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_OBTENER_JUDICIAL", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionId", intAcuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sOficinaConsularId", intOficinaConsularId));
                        
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }

                return DtResult;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }
            finally
            {
                DtResult = null;
            }
        }
    }
}
