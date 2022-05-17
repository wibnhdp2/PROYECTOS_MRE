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
    public class RE_ANTECEDENTE_PENAL_DETALLE_DA
    {
        public string strError = string.Empty;

        public BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE insertar(BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE AntecedenteDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_DETALLE_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@apde_iAntecedentePenalId", AntecedenteDetalle.apde_iAntecedentePenalId));
                        cmd.Parameters.Add(new SqlParameter("@apde_vOrganoJurisdiccional", AntecedenteDetalle.apde_vOrganoJurisdiccional));
                        cmd.Parameters.Add(new SqlParameter("@apde_vNumeroExpediente", AntecedenteDetalle.apde_vNumeroExpediente));

                        if (AntecedenteDetalle.apde_dFechaSentencia != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaSentencia", AntecedenteDetalle.apde_dFechaSentencia));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaSentencia", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@apde_vDelito", AntecedenteDetalle.apde_vDelito));
                        cmd.Parameters.Add(new SqlParameter("@apde_cDuracion_Anios", AntecedenteDetalle.apde_cDuracion_Anios));
                        cmd.Parameters.Add(new SqlParameter("@apde_cDuracion_Meses", AntecedenteDetalle.apde_cDuracion_Meses));
                        cmd.Parameters.Add(new SqlParameter("@apde_cDuracion_Dias", AntecedenteDetalle.apde_cDuracion_Dias));
                        
                        if (AntecedenteDetalle.apde_dFechaVencimiento != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaVencimiento", AntecedenteDetalle.apde_dFechaVencimiento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaVencimiento", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@apde_vTipoPena", AntecedenteDetalle.apde_vTipoPena));
                        //---------------------------------------------------------------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@apde_sUsuarioCreacion", AntecedenteDetalle.apde_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@apde_vIpCreacion", AntecedenteDetalle.apde_vIpCreacion));
                        cmd.Parameters.Add(new SqlParameter("@apde_sOficinaConsularId", AntecedenteDetalle.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@apde_iAntecedentePenalDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        AntecedenteDetalle.apde_iAntecedentePenalDetalleId = Convert.ToInt64(lReturn.Value);
                        AntecedenteDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                AntecedenteDetalle.Error = true;
                AntecedenteDetalle.Message = exec.Message.ToString();
            }
            return AntecedenteDetalle;
        }

        public BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE actualizar(BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE AntecedenteDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_DETALLE_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@apde_iAntecedentePenalDetalleId", AntecedenteDetalle.apde_iAntecedentePenalDetalleId));

                        cmd.Parameters.Add(new SqlParameter("@apde_iAntecedentePenalId", AntecedenteDetalle.apde_iAntecedentePenalId));
                        cmd.Parameters.Add(new SqlParameter("@apde_vOrganoJurisdiccional", AntecedenteDetalle.apde_vOrganoJurisdiccional));
                        cmd.Parameters.Add(new SqlParameter("@apde_vNumeroExpediente", AntecedenteDetalle.apde_vNumeroExpediente));

                        if (AntecedenteDetalle.apde_dFechaSentencia != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaSentencia", AntecedenteDetalle.apde_dFechaSentencia));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaSentencia", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@apde_vDelito", AntecedenteDetalle.apde_vDelito));
                        cmd.Parameters.Add(new SqlParameter("@apde_cDuracion_Anios", AntecedenteDetalle.apde_cDuracion_Anios));
                        cmd.Parameters.Add(new SqlParameter("@apde_cDuracion_Meses", AntecedenteDetalle.apde_cDuracion_Meses));
                        cmd.Parameters.Add(new SqlParameter("@apde_cDuracion_Dias", AntecedenteDetalle.apde_cDuracion_Dias));

                        if (AntecedenteDetalle.apde_dFechaVencimiento != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaVencimiento", AntecedenteDetalle.apde_dFechaVencimiento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@apde_dFechaVencimiento", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@apde_vTipoPena", AntecedenteDetalle.apde_vTipoPena));

                        cmd.Parameters.Add(new SqlParameter("@apde_sUsuarioModificacion", AntecedenteDetalle.apde_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@apde_vIpModificacion", AntecedenteDetalle.apde_vIpModificacion));
                        cmd.Parameters.Add(new SqlParameter("@apde_sOficinaConsularId", AntecedenteDetalle.OficinaConsultar));
                        //---------------------------------------------------------------------------------------------------------
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        AntecedenteDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                AntecedenteDetalle.Error = true;
                AntecedenteDetalle.Message = exec.Message.ToString();
            }
            return AntecedenteDetalle;
        }

        public BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE anular(BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE AntecedenteDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_DETALLE_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@apde_iAntecedentePenalDetalleId", AntecedenteDetalle.apde_iAntecedentePenalDetalleId));

                        cmd.Parameters.Add(new SqlParameter("@apde_sUsuarioModificacion", AntecedenteDetalle.apde_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@apde_vIpModificacion", AntecedenteDetalle.apde_vIpModificacion));
                        cmd.Parameters.Add(new SqlParameter("@apde_sOficinaConsularId", AntecedenteDetalle.OficinaConsultar));
                        //---------------------------------------------------------------------------------------------------------
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        AntecedenteDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                AntecedenteDetalle.Error = true;
                AntecedenteDetalle.Message = exec.Message.ToString();
            }
            return AntecedenteDetalle;
        }

        public DataTable Consultar(Int64 intAntecedentePenalDetalleId, Int64 intAntecedentePenalId, string strNumeroExpediente,
                                    string strEstado, int IntPageSize, int intPageNumber, string strContar,
                                    ref int ITotalPages, ref int ITotalRecords)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANTECEDENTE_PENAL_DETALLE_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_IANTECEDENTEPENALDETALLEID", intAntecedentePenalDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@P_IANTECEDENTEPENALID", intAntecedentePenalId));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROEXPEDIENTE", strNumeroExpediente));
                        cmd.Parameters.Add(new SqlParameter("@P_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageNumber));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalPages = Convert.ToInt32(lReturn1.Value);
                        ITotalRecords = Convert.ToInt32(lReturn2.Value);                        
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
