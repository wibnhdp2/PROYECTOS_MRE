using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using SGAC.BE.MRE;
using System.Data;
using System.Data.SqlClient;

using SGAC.Accesorios;
using System.Reflection;
using SGAC.BE.MRE.Custom;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
 
    public class RE_ACTONOTARIALCUERPO_DA
    {
        public BE.MRE.RE_ACTONOTARIALCUERPO insertar(BE.MRE.RE_ACTONOTARIALCUERPO ActoNotarialCuerpo)
        {
            try {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALCUERPO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ancu_iActoNotarialId", ActoNotarialCuerpo.ancu_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vCuerpo", ActoNotarialCuerpo.ancu_vCuerpo));

                        cmd.Parameters.Add(new SqlParameter("@ancu_vFirmaIlegible", ActoNotarialCuerpo.ancu_vFirmaIlegible));

                        cmd.Parameters.Add(new SqlParameter("@ancu_cEstado", ActoNotarialCuerpo.ancu_cEstado));

                        cmd.Parameters.Add(new SqlParameter("@ancu_sOficinaConsularId", ActoNotarialCuerpo.OficinaConsultar));

                        cmd.Parameters.Add(new SqlParameter("@ancu_sUsuarioCreacion", ActoNotarialCuerpo.ancu_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vIPCreacion", ActoNotarialCuerpo.ancu_vIPCreacion));

                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vTextoCentral", ActoNotarialCuerpo.ancu_vTextoCentral ));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vTextoAdicional", ActoNotarialCuerpo.ancu_vTextoAdicional));

                        cmd.Parameters.Add(new SqlParameter("@ancu_vTextoNormativo", ActoNotarialCuerpo.ancu_vTextoNormativo));

                        cmd.Parameters.Add(new SqlParameter("@ancu_vDL1049Articulo55C", ActoNotarialCuerpo.ancu_vDL1049Articulo55C));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ancu_iActoNotarialCuerpoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialCuerpo.ancu_iActoNotarialCuerpoId = Convert.ToInt64(lReturn.Value);
                        ActoNotarialCuerpo.Error = false;
                    }
                }
            }
            catch (SqlException exec) {
                ActoNotarialCuerpo.Error = true;
                ActoNotarialCuerpo.Message = exec.Message.ToString();
            }
            return ActoNotarialCuerpo;
        }
        public Int64 insertarPresentante(CBE_PRESENTANTE ActoNotarialCuerpo)
        {
            Int64 result = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPRESENTANTE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpr_iActoNotarialId", ActoNotarialCuerpo.anpr_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@anpr_iActoNotarialPresentanteId", ActoNotarialCuerpo.anpr_iActoNotarialPresentanteId));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sTipoPresentante", ActoNotarialCuerpo.anpr_sTipoPresentante));
                        cmd.Parameters.Add(new SqlParameter("@anpr_vPresentanteNombre", ActoNotarialCuerpo.anpr_vPresentanteNombre));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sPresentanteTipoDocumento", ActoNotarialCuerpo.anpr_sPresentanteTipoDocumento));
                        cmd.Parameters.Add(new SqlParameter("@anpr_vPresentanteNumeroDocumento", ActoNotarialCuerpo.anpr_vPresentanteNumeroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@anpr_cEtsado", ActoNotarialCuerpo.anpr_cEtsado));
                        
                        cmd.Parameters.Add(new SqlParameter("@anpr_sPresentanteGenero", ActoNotarialCuerpo.anpr_sPresentanteGenero));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sUsuario", ActoNotarialCuerpo.anpr_sUsuario));
                        cmd.Parameters.Add(new SqlParameter("@anpr_vIP", ActoNotarialCuerpo.anpr_vIP));
                        cmd.Parameters.Add(new SqlParameter("@operacion", ActoNotarialCuerpo.operacion));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@RESULT", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result = (Int64)cmd.Parameters["@RESULT"].Value;
                    }
                }
            }
            catch (SqlException exec)
            {
                 result = -1;
            }
            return result;
        }

        public BE.MRE.RE_ACTONOTARIALCUERPO actualizar(BE.MRE.RE_ACTONOTARIALCUERPO ActoNotarialCuerpo)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALCUERPO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ancu_iActoNotarialCuerpoId", ActoNotarialCuerpo.ancu_iActoNotarialCuerpoId));
                        cmd.Parameters.Add(new SqlParameter("@ancu_iActoNotarialId", ActoNotarialCuerpo.ancu_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vCuerpo", ActoNotarialCuerpo.ancu_vCuerpo));

                        cmd.Parameters.Add(new SqlParameter("@ancu_vFirmaIlegible", ActoNotarialCuerpo.ancu_vFirmaIlegible));

                        cmd.Parameters.Add(new SqlParameter("@ancu_cEstado", ActoNotarialCuerpo.ancu_cEstado));

                        cmd.Parameters.Add(new SqlParameter("@ancu_sOficinaConsularId", ActoNotarialCuerpo.OficinaConsultar));

                        cmd.Parameters.Add(new SqlParameter("@ancu_sUsuarioModificacion", ActoNotarialCuerpo.ancu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vIPModificacion", ActoNotarialCuerpo.ancu_vIPModificacion));

                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        
                        cmd.Parameters.Add(new SqlParameter("@ancu_vTextoCentral", ActoNotarialCuerpo.ancu_vTextoCentral));
                        cmd.Parameters.Add(new SqlParameter("@ancu_vTextoAdicional", ActoNotarialCuerpo.ancu_vTextoAdicional));

                        cmd.Parameters.Add(new SqlParameter("@ancu_vTextoNormativo", ActoNotarialCuerpo.ancu_vTextoNormativo));

                        cmd.Parameters.Add(new SqlParameter("@ancu_vDL1049Articulo55C", ActoNotarialCuerpo.ancu_vDL1049Articulo55C));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        ActoNotarialCuerpo.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoNotarialCuerpo.Error = true;
                ActoNotarialCuerpo.Message = exec.Message.ToString();
            }
            return ActoNotarialCuerpo;
        }

        public RE_ACTONOTARIALCUERPO obtener(RE_ACTONOTARIALCUERPO ActoNotarialCuerpo)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALCUERPO_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        if (ActoNotarialCuerpo.ancu_iActoNotarialCuerpoId != 0) cmd.Parameters.Add(new SqlParameter("@ancu_iActoNotarialCuerpoId", ActoNotarialCuerpo.ancu_iActoNotarialCuerpoId));
                        if (ActoNotarialCuerpo.ancu_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@ancu_iActoNotarialId", ActoNotarialCuerpo.ancu_iActoNotarialId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)ActoNotarialCuerpo.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(ActoNotarialCuerpo, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }
                        ActoNotarialCuerpo.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoNotarialCuerpo.Error = true;
                ActoNotarialCuerpo.Message = exec.Message.ToString();
            }
            return ActoNotarialCuerpo;                               
        }

        public List<RE_ACTONOTARIALCUERPO> paginado(RE_ACTONOTARIALCUERPO ActoNotarial)
        {
            return null;
        }

        string conexion(){
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
