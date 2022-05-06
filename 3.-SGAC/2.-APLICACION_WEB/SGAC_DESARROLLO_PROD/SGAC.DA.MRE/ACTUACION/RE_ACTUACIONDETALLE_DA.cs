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
    public class RE_ACTUACIONDETALLE_DA
    {
        public string strError = string.Empty;

        public BE.MRE.RE_ACTUACIONDETALLE insertar(BE.MRE.RE_ACTUACIONDETALLE ActuacionDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_ADICIONAR_NOTARIAL", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionId", ActuacionDetalle.acde_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sTarifarioId", ActuacionDetalle.acde_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sItem", ActuacionDetalle.acde_sItem));
                        cmd.Parameters.Add(new SqlParameter("@acde_dFechaRegistro", ActuacionDetalle.acde_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@acde_bRequisitosFlag", ActuacionDetalle.acde_bRequisitosFlag));
                        cmd.Parameters.Add(new SqlParameter("@acde_ICorrelativoActuacion", ActuacionDetalle.acde_ICorrelativoActuacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_ICorrelativoTarifario", ActuacionDetalle.acde_ICorrelativoTarifario));
                        cmd.Parameters.Add(new SqlParameter("@acde_IFuncionarioFirmanteId", ActuacionDetalle.acde_IFuncionarioFirmanteId));
                        cmd.Parameters.Add(new SqlParameter("@acde_IFuncionarioContactoId", ActuacionDetalle.acde_IFuncionarioContactoId));
                        cmd.Parameters.Add(new SqlParameter("@acde_IImpresionFuncionarioId", ActuacionDetalle.acde_IImpresionFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@acde_vNotas", ActuacionDetalle.acde_vNotas));
                        cmd.Parameters.Add(new SqlParameter("@acde_IFuncionarioAnulaId", ActuacionDetalle.acde_IFuncionarioAnulaId));
                        cmd.Parameters.Add(new SqlParameter("@acde_vMotivoAnulacion", ActuacionDetalle.acde_vMotivoAnulacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_iReferenciaId", ActuacionDetalle.acde_iReferenciaId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sEstadoId", ActuacionDetalle.acde_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sUsuarioCreacion", ActuacionDetalle.acde_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_vIPCreacion", ActuacionDetalle.acde_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_dFechaCreacion", ActuacionDetalle.acde_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_sUsuarioModificacion", ActuacionDetalle.acde_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_vIPModificacion", ActuacionDetalle.acde_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_dFechaModificacion", ActuacionDetalle.acde_dFechaModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acde_sOficinaConsularId", ActuacionDetalle.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@acde_vHostName", ActuacionDetalle.HostName));
                        cmd.Parameters.Add(new SqlParameter("@acde_iActoJudicialParticipanteId", ActuacionDetalle.Message));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt);
                                                                   
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActuacionDetalle.acde_iActuacionDetalleId = Convert.ToInt64(lReturn.Value);
                        ActuacionDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActuacionDetalle.Error = true;
                ActuacionDetalle.Message = exec.Message.ToString();
            }

            return ActuacionDetalle;
        }

        public RE_ACTUACIONDETALLE obtener(RE_ACTUACIONDETALLE ActuacionDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_NOTARIAL", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;                       

                        #region Creando Parametros
                        if (ActuacionDetalle.acde_iActuacionDetalleId != 0) cmd.Parameters.Add(new SqlParameter("@acde_iActuacionDetalleId", ActuacionDetalle.acde_iActuacionDetalleId));
                        if (ActuacionDetalle.acde_iActuacionId != 0) cmd.Parameters.Add(new SqlParameter("@acde_iActuacionId", ActuacionDetalle.acde_iActuacionId));
                        if (ActuacionDetalle.acde_sTarifarioId != 0) cmd.Parameters.Add(new SqlParameter("@acde_sTarifarioId", ActuacionDetalle.acde_sTarifarioId));
                        if (ActuacionDetalle.acde_IFuncionarioFirmanteId != 0) cmd.Parameters.Add(new SqlParameter("@acde_IFuncionarioFirmanteId", ActuacionDetalle.acde_IFuncionarioFirmanteId));
                        if (ActuacionDetalle.acde_IFuncionarioContactoId != null) cmd.Parameters.Add(new SqlParameter("@acde_IFuncionarioContactoId", ActuacionDetalle.acde_IFuncionarioContactoId));
                        if (ActuacionDetalle.acde_IImpresionFuncionarioId != null) cmd.Parameters.Add(new SqlParameter("@acde_IImpresionFuncionarioId", ActuacionDetalle.acde_IImpresionFuncionarioId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)ActuacionDetalle.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(ActuacionDetalle, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }

                        ActuacionDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActuacionDetalle.Error = true;
                ActuacionDetalle.Message = exec.Message.ToString();
            }

            return ActuacionDetalle;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
