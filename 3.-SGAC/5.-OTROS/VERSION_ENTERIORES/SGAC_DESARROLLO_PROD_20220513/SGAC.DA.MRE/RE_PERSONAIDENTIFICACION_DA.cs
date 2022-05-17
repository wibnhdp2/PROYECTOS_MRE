using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using System.Configuration;
using System.Reflection;

namespace SGAC.DA.MRE
{
    public class RE_PERSONAIDENTIFICACION_DA
    {
        public RE_PERSONAIDENTIFICACION obtener(RE_PERSONAIDENTIFICACION identificacion) {
            try{
                using (SqlConnection cnx = new SqlConnection(this.conexion())){
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ACTO_PROTOCOLAR_BUSCAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (identificacion.peid_iPersonaIdentificacionId !=0) cmd.Parameters.Add(new SqlParameter("@peid_iPersonaIdentificacionId", identificacion.peid_iPersonaIdentificacionId));
                        else cmd.Parameters.Add(new SqlParameter("@peid_iPersonaIdentificacionId", DBNull.Value));
                        if (identificacion.peid_iPersonaId !=0) cmd.Parameters.Add(new SqlParameter("@peid_iPersonaId", identificacion.peid_iPersonaId));
                        else cmd.Parameters.Add(new SqlParameter("@peid_iPersonaId", DBNull.Value));
                        if (identificacion.peid_sDocumentoTipoId !=0) cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", identificacion.peid_sDocumentoTipoId));
                        if (identificacion.peid_vDocumentoNumero != null) cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", identificacion.peid_vDocumentoNumero));

                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader()){
                            while (loReader.Read()){
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull") {
                                        PropertyInfo pInfo = (PropertyInfo)identificacion.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(identificacion, loReader[col], null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException exec){
                identificacion.Error = true;
                identificacion.Message = exec.Message.ToString();
                }
            return identificacion;
            }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
