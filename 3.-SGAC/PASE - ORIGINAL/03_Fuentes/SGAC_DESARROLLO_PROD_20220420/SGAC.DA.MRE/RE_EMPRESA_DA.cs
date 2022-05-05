using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SGAC.BE.MRE;
namespace SGAC.DA.MRE
{
    public class RE_EMPRESA_DA
    {
        public RE_EMPRESA insertar(RE_EMPRESA empresa)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@empr_sTipoEmpresaId", empresa.empr_sTipoEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@empr_sTipoDocumentoId", empresa.empr_sTipoDocumentoId));
                        cmd.Parameters.Add(new SqlParameter("@empr_vRazonSocial", empresa.empr_vRazonSocial));
                        cmd.Parameters.Add(new SqlParameter("@empr_vNumeroDocumento", empresa.empr_vNumeroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@empr_vActividadComercial", empresa.empr_vActividadComercial));
                        cmd.Parameters.Add(new SqlParameter("@empr_vTelefono", empresa.empr_vTelefono));
                        cmd.Parameters.Add(new SqlParameter("@empr_vCorreo", empresa.empr_vCorreo));
                        cmd.Parameters.Add(new SqlParameter("@empr_sUsuarioCreacion", empresa.empr_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@empr_vIPCreacion", empresa.empr_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@empr_sOficinaConsularId", empresa.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@empr_vHostName", empresa.HostName));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@empr_iEmpresaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        empresa.empr_iEmpresaId = Convert.ToInt64(lReturn.Value);
                        empresa.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                empresa.Error = true;
                empresa.Message = exec.Message.ToString();
            }
            return empresa;
        }

        public RE_EMPRESA obtener(RE_EMPRESA empresa) { 
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion())){
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_BUSCAR", cnx)){
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        if (empresa.empr_iEmpresaId !=0) cmd.Parameters.Add(new SqlParameter("@empr_iEmpresaId", empresa.empr_iEmpresaId));
                        if (empresa.empr_sTipoEmpresaId !=0) cmd.Parameters.Add(new SqlParameter("@empr_sTipoEmpresaId", empresa.empr_sTipoEmpresaId));
                        if (empresa.empr_sTipoDocumentoId != 0) cmd.Parameters.Add(new SqlParameter("@empr_sTipoDocumentoId", empresa.empr_sTipoDocumentoId));
                        if (empresa.empr_vRazonSocial !=null) cmd.Parameters.Add(new SqlParameter("@empr_vRazonSocial", empresa.empr_vRazonSocial));
                        if (empresa.empr_vNumeroDocumento != null) cmd.Parameters.Add(new SqlParameter("@empr_vNumeroDocumento", empresa.empr_vNumeroDocumento));
                        if (empresa.empr_vActividadComercial != null) cmd.Parameters.Add(new SqlParameter("@empr_vActividadComercial", empresa.empr_vActividadComercial));
                        #endregion
                        cnx.Open();
                        using (SqlDataReader loReader = cmd.ExecuteReader()) {
                            while (loReader.Read()) {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull"){
                                        PropertyInfo pInfo = (PropertyInfo)empresa.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(empresa, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }
                        //empresa.
                        empresa.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                empresa.Error = true;
                empresa.Message = exec.Message.ToString();
            }
            return empresa;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
