using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using SGAC.BE.MRE;
using System.Data;
using System.Data.SqlClient;

using SGAC.Accesorios;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
    public class RE_ACTONOTARIAL_NORMAS_DA
    {
        public BE.MRE.RE_ACTONOTARIAL_NORMA insertar(BE.MRE.RE_ACTONOTARIAL_NORMA ActoNotarialNorma)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_PN_REGISTRO_RE_ACTONOTARIAL_NORMAS_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALID", ActoNotarialNorma.anra_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@P_SNORMAID", ActoNotarialNorma.anra_sNormaId));

                        cmd.Parameters.Add(new SqlParameter("@P_SUSUARIO_CREACION", ActoNotarialNorma.anra_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VIP_CREACION", ActoNotarialNorma.anra_vIPCreacion));

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", ActoNotarialNorma.OficinaConsultar));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@P_IACTONOTARIALNORMAID", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialNorma.anra_iActoNotarialNormaId = Convert.ToInt64(lReturn.Value);
                        ActoNotarialNorma.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoNotarialNorma.Error = true;
                ActoNotarialNorma.Message = exec.Message.ToString();
            }
            return ActoNotarialNorma;
        }

        public BE.MRE.RE_ACTONOTARIAL_NORMA anular(BE.MRE.RE_ACTONOTARIAL_NORMA ActoNotarialNorma)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_PN_REGISTRO_RE_ACTONOTARIAL_NORMAS_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALNORMAID", ActoNotarialNorma.anra_iActoNotarialNormaId));

                        cmd.Parameters.Add(new SqlParameter("@P_SUSUARIO_MODIFICACION", ActoNotarialNorma.anra_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VIP_MODIFICACION", ActoNotarialNorma.anra_vIPModificacion));

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", ActoNotarialNorma.OficinaConsultar));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialNorma.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoNotarialNorma.Error = true;
                ActoNotarialNorma.Message = exec.Message.ToString();
            }
            return ActoNotarialNorma;
        }

        public DataTable ObtenerNormas(BE.MRE.RE_ACTONOTARIAL_NORMA normas)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_NORMAS_CONSULTAR_MRE", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (normas.anra_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@P_IACTONOTARIALID", normas.anra_iActoNotarialId));
                        #endregion

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (SqlException exec)
            {
                normas.Error = true;
                normas.Message = exec.Message.ToString();
            }

            return dt;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
//----------------------------------------------
    }
}
