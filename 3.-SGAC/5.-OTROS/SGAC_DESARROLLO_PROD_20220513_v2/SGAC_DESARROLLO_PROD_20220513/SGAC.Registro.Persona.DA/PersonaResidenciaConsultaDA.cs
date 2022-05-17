using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using System.Collections.Generic;
using System.Reflection;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaResidenciaConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public PersonaResidenciaConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PersonaResidenciaConsultaDA()
        {
            GC.Collect();
        }

        public DataTable Obtener(long LonPersonaId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameterDir = new SqlParameter[1];

                prmParameterDir[0] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameterDir[0].Value = LonPersonaId;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_GETDIRECCIONES",
                                                    prmParameterDir);

                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public List<BE.MRE.RE_PERSONARESIDENCIA> ObtenerLista(long lngPersonaId)
        {
            List<BE.MRE.RE_PERSONARESIDENCIA> lstResidencias = new List<BE.MRE.RE_PERSONARESIDENCIA>();
            BE.MRE.RE_PERSONARESIDENCIA objPersonaResidencia = new BE.MRE.RE_PERSONARESIDENCIA();

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONARESIDENCIA_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pere_iPersonaId", lngPersonaId));

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)objPersonaResidencia.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(objPersonaResidencia, loReader[col], null);
                                    }
                                }

                                objPersonaResidencia.Residencia.resi_iResidenciaId = objPersonaResidencia.pere_iResidenciaId;
                                objPersonaResidencia.Residencia = ObtenerResidencia(objPersonaResidencia.Residencia);
                                lstResidencias.Add(objPersonaResidencia);
                            }
                        }
                    }
                }
                 
            }
            catch (SqlException ex)
            {                
                throw;
            }

            return lstResidencias;
        }

        public List<BE.MRE.RE_PERSONARESIDENCIA> listado(BE.MRE.RE_PERSONARESIDENCIA residencia)
        {
            List<BE.MRE.RE_PERSONARESIDENCIA> lResult = new List<BE.MRE.RE_PERSONARESIDENCIA>();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
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
                                BE.MRE.RE_PERSONARESIDENCIA loPERSONARESIDENCIA = new BE.MRE.RE_PERSONARESIDENCIA();
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)loPERSONARESIDENCIA.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(loPERSONARESIDENCIA, loReader[col], null);
                                    }
                                }

                                //BE.MRE.RE_RESIDENCIA lRE_RESIDENCIA = new RE_RESIDENCIA();
                                //lRE_RESIDENCIA.resi_iResidenciaId = loPERSONARESIDENCIA.pere_iResidenciaId;
                                //BE.MRE.RE_RESIDENCIA_DA lRESIDENCIA_DA = new RE_RESIDENCIA_DA();
                                //loPERSONARESIDENCIA.Residencia = lRESIDENCIA_DA.obtener(lRE_RESIDENCIA);

                                //lResult.Add(loPERSONARESIDENCIA);
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

        public BE.MRE.RE_RESIDENCIA ObtenerResidencia(BE.MRE.RE_RESIDENCIA residencia)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_RESIDENCIA_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@resi_iResidenciaId", residencia.resi_iResidenciaId));

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
            catch (SqlException ex)
            {                
                throw;
            }
            return residencia;
        }
    }
}