using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using System.Collections.Generic;
using System.Reflection;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaFiliacionConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public PersonaFiliacionConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PersonaFiliacionConsultaDA()
        {
            GC.Collect();
        }

        public DataTable Obtener(long LonPersonaId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameterDir = new SqlParameter[2];

                prmParameterDir[0] = new SqlParameter("@pefi_iPersonaId", SqlDbType.BigInt);
                prmParameterDir[0].Value = LonPersonaId;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONAFILIACION_OBTENER",
                                                    prmParameterDir);

                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public List<BE.MRE.RE_PERSONAFILIACION> ObtenerLista(long lngPersonaId)
        {
            List<BE.MRE.RE_PERSONAFILIACION> lstFiliacion = new List<BE.MRE.RE_PERSONAFILIACION>();
            BE.MRE.RE_PERSONAFILIACION objPersonaFiliacion = new BE.MRE.RE_PERSONAFILIACION();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONAFILIACION_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pefi_iPersonaId", lngPersonaId));

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                objPersonaFiliacion = new BE.MRE.RE_PERSONAFILIACION();
                                for (int col = 0; col < loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)objPersonaFiliacion.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(objPersonaFiliacion, loReader[col], null);
                                        }
                                    }
                                }
                                lstFiliacion.Add(objPersonaFiliacion);
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                //Persona.Error = true;
                //Persona.Message = exec.Message.ToString();
            }
            return lstFiliacion;
        }
    }
}