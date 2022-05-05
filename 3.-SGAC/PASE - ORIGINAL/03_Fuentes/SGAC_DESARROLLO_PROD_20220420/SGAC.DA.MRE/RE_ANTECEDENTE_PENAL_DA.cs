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

namespace SGAC.DA.MRE
{
    public class RE_ANTECEDENTE_PENAL_DA
    {
        public string strError = string.Empty;

        public BE.MRE.RE_ANTECEDENTE_PENAL insertar(BE.MRE.RE_ANTECEDENTE_PENAL antecedente)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpe_sOficinaConsularId", antecedente.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@anpe_iActuacionDetalleId", antecedente.anpe_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@anpe_vNumeroSolicitud", antecedente.anpe_vNumeroSolicitud));

                        if (antecedente.anpe_iFuncionarioId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_iFuncionarioId", antecedente.anpe_iFuncionarioId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_iFuncionarioId", null));
                        }
                        
                        cmd.Parameters.Add(new SqlParameter("@anpe_dFechaSolicitud", antecedente.anpe_dFechaSolicitud));

                        cmd.Parameters.Add(new SqlParameter("@anpe_vNumeroOficioRpta", antecedente.anpe_vNumeroOficioRpta));

                        if (antecedente.anpe_dFechaRespuesta != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_dFechaRespuesta", antecedente.anpe_dFechaRespuesta));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_dFechaRespuesta", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@anpe_vObservacion", antecedente.anpe_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@anpe_sUsuarioCreacion", antecedente.anpe_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@anpe_vIpCreacion", antecedente.anpe_vIpCreacion));
                        //---------------------------------------------------------------------------------------------------------
                        // FECHA: 11/09/2019
                        // AUTOR: MIGUEL MÁRQUEZ BELTRÁN
                        // MOTIVO: NUEVOS PARAMETROS
                        //---------------------------------------------------------------------------------------------------------
                        //[anpe_sSolicitaParaId], [anpe_cRegistraAntecedentesPenales],
                        //---------------------------------------------------------------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@anpe_sSolicitaParaId", antecedente.anpe_sSolicitaParaId));
                        cmd.Parameters.Add(new SqlParameter("@anpe_cRegistraAntecedentesPenales", antecedente.anpe_cRegistraAntecedentesPenales));
                        //---------------------------------------------------------------------------------------------------------
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@anpe_iAntecedentePenalId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        antecedente.anpe_iAntecedentePenalId = Convert.ToInt64(lReturn.Value);
                        antecedente.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                antecedente.Error = true;
                antecedente.Message = exec.Message.ToString();
            }
            return antecedente;
        }

        public BE.MRE.RE_ANTECEDENTE_PENAL actualizar(BE.MRE.RE_ANTECEDENTE_PENAL antecedente)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpe_iAntecedentePenalId", antecedente.anpe_iAntecedentePenalId));
                        cmd.Parameters.Add(new SqlParameter("@anpe_sOficinaConsularId", antecedente.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@anpe_vNumeroSolicitud", antecedente.anpe_vNumeroSolicitud));

                        if (antecedente.anpe_iFuncionarioId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_iFuncionarioId", antecedente.anpe_iFuncionarioId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_iFuncionarioId", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@anpe_dFechaSolicitud", antecedente.anpe_dFechaSolicitud));
                        cmd.Parameters.Add(new SqlParameter("@anpe_vNumeroOficioRpta", antecedente.anpe_vNumeroOficioRpta));

                        if (antecedente.anpe_dFechaRespuesta != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_dFechaRespuesta", antecedente.anpe_dFechaRespuesta));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpe_dFechaRespuesta", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@anpe_vObservacion", antecedente.anpe_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@anpe_sUsuarioModificacion", antecedente.anpe_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@anpe_vIpModificacion", antecedente.anpe_vIpModificacion));
                        //---------------------------------------------------------------------------------------------------------
                        // FECHA: 11/09/2019
                        // AUTOR: MIGUEL MÁRQUEZ BELTRÁN
                        // MOTIVO: NUEVOS PARAMETROS
                        //---------------------------------------------------------------------------------------------------------
                        //[anpe_sSolicitaParaId], [anpe_cRegistraAntecedentesPenales],
                        //---------------------------------------------------------------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@anpe_sSolicitaParaId", antecedente.anpe_sSolicitaParaId));
                        cmd.Parameters.Add(new SqlParameter("@anpe_cRegistraAntecedentesPenales", antecedente.anpe_cRegistraAntecedentesPenales));
                        //---------------------------------------------------------------------------------------------------------
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        antecedente.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                antecedente.Error = true;
                antecedente.Message = exec.Message.ToString();
            }
            return antecedente;
        }

        public BE.MRE.RE_ANTECEDENTE_PENAL anular(BE.MRE.RE_ANTECEDENTE_PENAL antecedente)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpe_iAntecedentePenalId", antecedente.anpe_iAntecedentePenalId));
                        cmd.Parameters.Add(new SqlParameter("@anpe_sOficinaConsularId", antecedente.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@anpe_sUsuarioModificacion", antecedente.anpe_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@anpe_vIpModificacion", antecedente.anpe_vIpModificacion));
                        #endregion


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        antecedente.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                antecedente.Error = true;
                antecedente.Message = exec.Message.ToString();
            }
            return antecedente;
        }

        public DataTable Consultar(long intDetalleActuacionid, short intOficinaConsularId, string strFechaInicial, string strFechaFinal, string strIsMSIAP_RGE)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_CONSULTAR_MRE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@P_iActuacionDetalleId", SqlDbType.BigInt).Value = intDetalleActuacionid;
                    da.SelectCommand.Parameters.Add("@P_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                    da.SelectCommand.Parameters.Add("@P_CFECHA_INICIAL", SqlDbType.Char, 8).Value = strFechaInicial;
                    da.SelectCommand.Parameters.Add("@P_CFECHA_FINAL", SqlDbType.Char, 8).Value = strFechaFinal;
                    da.SelectCommand.Parameters.Add("@P_IS_MSIAP_RGE", SqlDbType.Char, 1).Value = strIsMSIAP_RGE;
                    
                    da.Fill(ds, "Antecedente");
                    dt = ds.Tables["Antecedente"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable ReporteCertificadoConsular(long intAntecedentePenalId, Int16 intTipoDocumentoId)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_CERTIFICADO_CONSULAR_REPORTE_MRE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@P_IANTECEDENTEPENALID", SqlDbType.BigInt).Value = intAntecedentePenalId;
                    da.SelectCommand.Parameters.Add("@P_STIPODOCUMENTOID", SqlDbType.SmallInt).Value = intTipoDocumentoId;

                    da.Fill(ds, "CertificadoAntecedentePenal");
                    dt = ds.Tables["CertificadoAntecedentePenal"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataSet ObtenerDataSet(long intDetalleActuacionid, short intOficinaConsularId, string strFechaInicial, string strFechaFinal, string strIsMSIAP_RGE, Int16 Usuario = 0)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_CONSULTAR_MRE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@P_iActuacionDetalleId", SqlDbType.BigInt).Value = intDetalleActuacionid;
                    da.SelectCommand.Parameters.Add("@P_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                    da.SelectCommand.Parameters.Add("@P_CFECHA_INICIAL", SqlDbType.Char, 8).Value = strFechaInicial;
                    da.SelectCommand.Parameters.Add("@P_CFECHA_FINAL", SqlDbType.Char, 8).Value = strFechaFinal;
                    da.SelectCommand.Parameters.Add("@P_IS_MSIAP_RGE", SqlDbType.Char, 1).Value = strIsMSIAP_RGE;
                    da.SelectCommand.Parameters.Add("@P_USUARIO", SqlDbType.SmallInt).Value = Usuario;

                    da.Fill(ds, "Antecedente");
                    //dt = ds.Tables["Antecedente"];

                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
