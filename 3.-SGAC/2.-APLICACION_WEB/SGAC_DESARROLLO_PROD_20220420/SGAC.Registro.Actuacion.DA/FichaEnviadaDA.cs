using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SGAC.BE.MRE;
//--------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase de acceso a datos de la ficha enviada
//--------------------------------------------------------

namespace SGAC.Registro.Actuacion.DA
{
    public class FichaEnviadaDA
    {
        public DataTable Consultar(long intFichaEnviadaId, long intGuiaDespachoId, long intFichaRegistralId,
                                          int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_ENVIADA_CONSULTAR_TITULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_FIEN_IFICHAENVIADAID", intFichaEnviadaId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIEN_IGUIADESPACHOID", intGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIEN_IFICHAREGISTRALID", intFichaRegistralId));


                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
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

        public RE_FICHAENVIADA insertar(RE_FICHAENVIADA fichaEnviada)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAENVIADA_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@FIEN_IGUIADESPACHOID", fichaEnviada.fien_iGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@FIEN_IFICHAREGISTRALID", fichaEnviada.fien_iFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@FIEN_SUSUARIOCREACION", fichaEnviada.fien_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@FIEN_VIPCREACION", fichaEnviada.fien_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@OFCO_SOFICINACONSULARID", fichaEnviada.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@FIEN_IFICHAENVIADAID", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        fichaEnviada.fien_iFichaEnviadaId = Convert.ToInt64(lReturn.Value);
                        fichaEnviada.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                fichaEnviada.Error = true;
                fichaEnviada.Message = exec.Message.ToString();
            }
            return fichaEnviada;
        }

        public RE_FICHAENVIADA anular(RE_FICHAENVIADA fichaEnviada)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAENVIADA_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@FIEN_IFICHAENVIADAID", fichaEnviada.fien_iFichaEnviadaId));
                        cmd.Parameters.Add(new SqlParameter("@FIEN_SUSUARIOMODIFICACION", fichaEnviada.fien_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@FIEN_VIPMODIFICACION", fichaEnviada.fien_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@OFCO_SOFICINACONSULARID", fichaEnviada.OficinaConsultar));
                        #endregion
                       
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        fichaEnviada.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                fichaEnviada.Error = true;
                fichaEnviada.Message = exec.Message.ToString();
            }
            return fichaEnviada;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
