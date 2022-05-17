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
using SGAC.Accesorios;


namespace SGAC.DA.MRE.ACTUACION
{
    public class RE_ACTU_PAGO_DA
    {

        public string strError = string.Empty;
        public BE.MRE.RE_ACTUA_PAGO insertar(BE.MRE.RE_ACTUA_PAGO actu_pago)
        { 
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_PAGO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@iAJParticipanteID", actu_pago.AP_iAJparticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@iTarifarioID", actu_pago.AP_iTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@sPagoTipoId", actu_pago.AP_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@iPersonaRecurrenteId", actu_pago.AP_iPersonaRecurrenteId));
                        cmd.Parameters.Add(new SqlParameter("@iEmpresaRecurrenteId", actu_pago.AP_iEmpresaRecurrenteId));
                        cmd.Parameters.Add(new SqlParameter("@dFechaOperacion", actu_pago.AP_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@sMonedaLocalId", actu_pago.AP_sMonedaLocalId));
                        cmd.Parameters.Add(new SqlParameter("@FMontoMonedaLocal", actu_pago.AP_FMontoMonedaLocal));
                        cmd.Parameters.Add(new SqlParameter("@FMontoSolesConsulares", actu_pago.AP_FMontoSolesConsulares));
                        cmd.Parameters.Add(new SqlParameter("@FTipCambioBancario", actu_pago.AP_FTipCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@FTipCambioConsular", actu_pago.AP_FTipCambioConsular));
                        cmd.Parameters.Add(new SqlParameter("@sBancoId", actu_pago.AP_sBancoId));
                        cmd.Parameters.Add(new SqlParameter("@vBancoNumeroOperacion", actu_pago.AP_vBancoNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@bPagadoFlag", actu_pago.AP_bPagadoFlag));
                        cmd.Parameters.Add(new SqlParameter("@vComentario", actu_pago.AP_vComentario));
                        cmd.Parameters.Add(new SqlParameter("@vNumeroVoucher", actu_pago.AP_vNumeroVoucher));
                        cmd.Parameters.Add(new SqlParameter("@cEstado", actu_pago.AP_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@sUsuarioCreacion", actu_pago.AP_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vIPCreacion", actu_pago.AP_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@dFechaCreacion", actu_pago.AP_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@sUsuarioModificacion", actu_pago.AP_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vIPModificacion", actu_pago.AP_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@dFechaModificacion", actu_pago.AP_dFechaModificacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", actu_pago.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                       #endregion

                        #region Output
                        //SqlParameter lReturn = cmd.Parameters.Add("@pago_iPagoId", SqlDbType.BigInt);
                        //lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        //pago.pago_iPagoId = Convert.ToInt64(lReturn.Value);
                        actu_pago.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                actu_pago.Error = true;
                actu_pago.Message = exec.Message.ToString();
            }
            return actu_pago;
        }


        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
