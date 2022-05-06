using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.DA.MRE
{
    using System.Configuration;
    using System.Reflection;
    using System.Data;
    using System.Data.SqlClient;

    using SGAC.BE.MRE;

    public class RE_RESIDENCIA_DA
    {
        public RE_RESIDENCIA obtener(RE_RESIDENCIA residencia)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_RESIDENCIA_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (residencia.resi_iResidenciaId != 0) cmd.Parameters.Add(new SqlParameter("@resi_iResidenciaId", residencia.resi_iResidenciaId));
                        if (residencia.resi_sResidenciaTipoId != 0) cmd.Parameters.Add(new SqlParameter("@resi_sResidenciaTipoId", residencia.resi_sResidenciaTipoId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)residencia.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(residencia, loReader[col], null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                residencia.Error = true;
                residencia.Message = exec.Message.ToString();
            }
            return residencia;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
