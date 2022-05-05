using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SGAC.Accesorios;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
    using SGAC.BE.MRE;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;

    public class RE_ACTONOTARIAL_PRIMIGENIA_DA
    {
        public string strError = string.Empty;

        public RE_ACTONOTARIAL_PRIMIGENIA insertar(RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigenia)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_PRIMIGENIA_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALID", ActoNotarialPrimigenia.anpr_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@P_CANIOESCRITURA", ActoNotarialPrimigenia.anpr_cAnioEscritura));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROESCRITURAPUBLICA", ActoNotarialPrimigenia.anpr_vNumeroEscrituraPublica));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULAR_EPID", ActoNotarialPrimigenia.anpr_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_DFECHAEXPEDICION", ActoNotarialPrimigenia.anpr_dFechaExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@P_VTIPOACTONOTARIAL", ActoNotarialPrimigenia.anpr_vTipoActoNotarial));
                        cmd.Parameters.Add(new SqlParameter("@P_VNOTARIA", ActoNotarialPrimigenia.anpr_vNotaria));

                        cmd.Parameters.Add(new SqlParameter("@P_SUSUARIO_CREACION", ActoNotarialPrimigenia.anpr_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VIP_CREACION", ActoNotarialPrimigenia.anpr_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", ActoNotarialPrimigenia.OficinaConsultar));

                        if (ActoNotarialPrimigenia.iActoNotarialReferencialId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALREFERENCIALID", ActoNotarialPrimigenia.iActoNotarialReferencialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALREFERENCIALID", DBNull.Value));
                        }
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@P_IACTONOTARIALPRIMIGENIAID", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialPrimigenia.anpr_iActoNotarialPrimigeniaId = Convert.ToInt64(lReturn.Value);
                        ActoNotarialPrimigenia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarialPrimigenia.Error = true;
                ActoNotarialPrimigenia.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarialPrimigenia.Error = true;
                ActoNotarialPrimigenia.Message = exec.Message.ToString();
            }
            return ActoNotarialPrimigenia;
        }

        public RE_ACTONOTARIAL_PRIMIGENIA actualizar(RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigenia)
        {
            try
            {

                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_PRIMIGENIA_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cnx.Open();

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALPRIMIGENIAID", ActoNotarialPrimigenia.anpr_iActoNotarialPrimigeniaId));
                        cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALID", ActoNotarialPrimigenia.anpr_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@P_CANIOESCRITURA", ActoNotarialPrimigenia.anpr_cAnioEscritura));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROESCRITURAPUBLICA", ActoNotarialPrimigenia.anpr_vNumeroEscrituraPublica));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULAR_EPID", ActoNotarialPrimigenia.anpr_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_DFECHAEXPEDICION", ActoNotarialPrimigenia.anpr_dFechaExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@P_VTIPOACTONOTARIAL", ActoNotarialPrimigenia.anpr_vTipoActoNotarial));
                        cmd.Parameters.Add(new SqlParameter("@P_VNOTARIA", ActoNotarialPrimigenia.anpr_vNotaria));
                        cmd.Parameters.Add(new SqlParameter("@P_CESTADO", ActoNotarialPrimigenia.anpr_cEstado));

                        cmd.Parameters.Add(new SqlParameter("@P_SUSUARIO_MODIFICACION", ActoNotarialPrimigenia.anpr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VIP_MODIFICACION", ActoNotarialPrimigenia.anpr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", ActoNotarialPrimigenia.OficinaConsultar));


                        if (ActoNotarialPrimigenia.iActoNotarialReferencialId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALREFERENCIALID", ActoNotarialPrimigenia.iActoNotarialReferencialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALREFERENCIALID", DBNull.Value));
                        }


                        #endregion

                        cmd.ExecuteNonQuery();
                        ActoNotarialPrimigenia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarialPrimigenia.Error = true;
                ActoNotarialPrimigenia.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarialPrimigenia.Error = true;
                ActoNotarialPrimigenia.Message = exec.Message.ToString();
            }
            return ActoNotarialPrimigenia;
        }

        public RE_ACTONOTARIAL_PRIMIGENIA anular(RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigenia)
        {
            try
            {

                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_PRIMIGENIA_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cnx.Open();

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALID", ActoNotarialPrimigenia.anpr_iActoNotarialId));

                        cmd.Parameters.Add(new SqlParameter("@P_SUSUARIO_MODIFICACION", ActoNotarialPrimigenia.anpr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VIP_MODIFICACION", ActoNotarialPrimigenia.anpr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", ActoNotarialPrimigenia.OficinaConsultar));


                        #endregion

                        cmd.ExecuteNonQuery();
                        ActoNotarialPrimigenia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarialPrimigenia.Error = true;
                ActoNotarialPrimigenia.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarialPrimigenia.Error = true;
                ActoNotarialPrimigenia.Message = exec.Message.ToString();
            }
            return ActoNotarialPrimigenia;
        }

        public DataTable Consultar(long iActoNotarialPrimigeniaId, long iActoNotarialId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_ACTONOTARIAL_PRIMIGENIA_CONSULTAR_MRE]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@P_iActoNotarialPrimigeniaId", SqlDbType.BigInt).Value = iActoNotarialPrimigeniaId;
                        cmd.Parameters.Add("@P_iActoNotarialId", SqlDbType.BigInt).Value = iActoNotarialId;

                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];

        }
    //---------------------------------------
    }
}
