using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using System.Configuration;
using System.Reflection;
using SGAC.Accesorios;

namespace SGAC.DA.MRE
{
    public class RE_REPRESENTANTELEGAL_DA
    {
        public List<RE_REPRESENTANTELEGAL> listado(RE_EMPRESA empresa) {
            List<RE_REPRESENTANTELEGAL> RepresentantesLegales = new List<RE_REPRESENTANTELEGAL>();
            try {
                using (SqlConnection cnx = new SqlConnection(this.conexion())) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_OBTENER", cnx)){
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@rele_iEmpresaId", empresa.empr_iEmpresaId));

                        #region Reflection Object
                        using (SqlDataReader loReader = cmd.ExecuteReader()){
                            while (loReader.Read()){
                                RE_REPRESENTANTELEGAL lRepresentanteLegal = new RE_REPRESENTANTELEGAL();
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull"){
                                        PropertyInfo pInfo = (PropertyInfo)lRepresentanteLegal.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(lRepresentanteLegal, loReader[col], null);
                                    }
                                }
                                RepresentantesLegales.Add(lRepresentanteLegal);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (SqlException exec) {
                throw exec;
            }
            return RepresentantesLegales;
        }

        public RE_REPRESENTANTELEGAL obtener(RE_REPRESENTANTELEGAL representante) {
            try {
                using (SqlConnection cnx = new SqlConnection(this.conexion())) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_OBTENER", cnx)){
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (representante.rele_iRepresentanteLegalId != 0) cmd.Parameters.Add(new SqlParameter("@rele_iRepresentanteLegalId", representante.rele_iRepresentanteLegalId));
                        if (representante.rele_iEmpresaId != 0) cmd.Parameters.Add(new SqlParameter("@rele_iEmpresaId", representante.rele_iEmpresaId));
                        if (representante.rele_iPersonaId != 0) cmd.Parameters.Add(new SqlParameter("@rele_iPersonaId", representante.rele_iPersonaId));
                        #endregion

                        #region Reflection Object
                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)representante.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(representante, loReader[col], null);
                                    }
                                }
                            }
                        } 
                        #endregion

                    }
                }
            }
            catch (SqlException exec) {
                representante.Error = true;
                representante.Message = exec.Message.ToString();
            }
            return representante;
        }

        public RE_REPRESENTANTELEGAL insertar(RE_REPRESENTANTELEGAL representante) {
            try {
                using (SqlConnection cnx = new SqlConnection(this.conexion())) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_INSERTAR", cnx)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@rele_iEmpresaId",representante.rele_iEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@rele_iPersonaId",representante.rele_iPersonaId));
                        if (representante.rele_dFechaInicio != DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@rele_dFechaInicio", representante.rele_dFechaInicio));
                        if (representante.rele_dFechaFin != DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@rele_dFechaFin", representante.rele_dFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@rele_cEstado", representante.rele_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@rele_sUsuarioCreacion", representante.rele_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@rele_vIPCreacion", representante.rele_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsular", representante.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        #endregion
                        
                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@rele_iRepresentanteLegalId", SqlDbType.BigInt);
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        representante.rele_iRepresentanteLegalId = Convert.ToInt64(lReturn.Value);
                        representante.Error = false;
                    }
                }
            }
            catch (SqlException exec) {
                representante.Error = true;
                representante.Message = exec.Message.ToString();
            }
            return representante;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
