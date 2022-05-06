using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.DA.MRE
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;

    using SGAC.BE.MRE;

    public class RE_PERSONARESIDENCIA_DA
    {
        public RE_PERSONARESIDENCIA obtener(RE_PERSONARESIDENCIA residencia)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONARESIDENCIA_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (residencia.pere_iPersonaId !=0) cmd.Parameters.Add(new SqlParameter("@pere_iPersonaId", residencia.pere_iPersonaId));
	                    if (residencia.pere_iResidenciaId !=0) cmd.Parameters.Add(new SqlParameter("@pere_iResidenciaId", residencia.pere_iResidenciaId));
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

        public List<RE_PERSONARESIDENCIA> listado(RE_PERSONARESIDENCIA residencia) {
            List<RE_PERSONARESIDENCIA> lResult = new List<RE_PERSONARESIDENCIA>();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONARESIDENCIA_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (residencia.pere_iPersonaId != 0) cmd.Parameters.Add(new SqlParameter("@pere_iPersonaId", residencia.pere_iPersonaId));
                        if (residencia.pere_iResidenciaId != 0) cmd.Parameters.Add(new SqlParameter("@pere_iResidenciaId", residencia.pere_iResidenciaId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                RE_PERSONARESIDENCIA loPERSONARESIDENCIA = new RE_PERSONARESIDENCIA();
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)loPERSONARESIDENCIA.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(loPERSONARESIDENCIA, loReader[col], null);
                                    }
                                }

                                RE_RESIDENCIA lRE_RESIDENCIA = new RE_RESIDENCIA();
                                lRE_RESIDENCIA.resi_iResidenciaId = loPERSONARESIDENCIA.pere_iResidenciaId;
                                RE_RESIDENCIA_DA lRESIDENCIA_DA = new RE_RESIDENCIA_DA();
                                loPERSONARESIDENCIA.Residencia = lRESIDENCIA_DA.obtener(lRE_RESIDENCIA);

                                lResult.Add(loPERSONARESIDENCIA);
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                throw new IndexOutOfRangeException();

                //residencia.Error = true;
                //residencia.Message = exec.Message.ToString();
            }
            return lResult;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
