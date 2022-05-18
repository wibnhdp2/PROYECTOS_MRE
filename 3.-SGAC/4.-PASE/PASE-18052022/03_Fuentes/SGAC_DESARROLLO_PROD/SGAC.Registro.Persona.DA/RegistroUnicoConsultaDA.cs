using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SGAC.Registro.Persona.DA
{
    public class RegistroUnicoConsultaDA
    {
        ~RegistroUnicoConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public BE.MRE.RE_REGISTROUNICO Obtener(BE.MRE.RE_REGISTROUNICO registrounico)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROUNICO_BUSCAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (registrounico.reun_iRegistroUnicoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@reun_iRegistroUnicoId", registrounico.reun_iRegistroUnicoId));
                        if (registrounico.reun_iPersonaId != 0)
                            cmd.Parameters.Add(new SqlParameter("@reun_iPersonaId", registrounico.reun_iPersonaId));

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col < loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)registrounico.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(registrounico, loReader[col], null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                registrounico.Error = true;
                registrounico.Message = ex.Message;
            }
            return registrounico;
        }
    }
}
