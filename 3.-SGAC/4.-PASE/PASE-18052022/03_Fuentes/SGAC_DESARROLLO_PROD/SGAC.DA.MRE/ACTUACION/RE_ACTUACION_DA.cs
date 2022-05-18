using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Configuration;
using System.Reflection;

namespace SGAC.DA.MRE.ACTUACION
{
    public class RE_ACTUACION_DA 
    {
        public string strError = string.Empty;

        public RE_ACTUACION insertar_exhorto(RE_ACTUACION actuacion)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_ADICIONAR_EXHORTOS", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", actuacion.actu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@actu_FCantidad", actuacion.actu_FCantidad));

                        if (actuacion.actu_iPersonaRecurrenteId == 0)
                            cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", actuacion.actu_iPersonaRecurrenteId));

                        if (actuacion.actu_iEmpresaRecurrenteId == null)
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", DBNull.Value));
                        else if (actuacion.actu_iEmpresaRecurrenteId == 0)
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", actuacion.actu_iEmpresaRecurrenteId));


                        cmd.Parameters.Add(new SqlParameter("@actu_dFechaRegistro", actuacion.actu_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@actu_sEstado", actuacion.actu_sEstado));
                        cmd.Parameters.Add(new SqlParameter("@actu_sUsuarioCreacion", actuacion.actu_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vIPCreacion", actuacion.actu_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vHostName", ""));
                        cmd.Parameters.Add(new SqlParameter("@actu_sAJParticipanteID", actuacion.Message));
                        SqlParameter lReturn = cmd.Parameters.Add("@actu_iActuacionId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        actuacion.actu_iActuacionId = Convert.ToInt64(lReturn.Value);
                        actuacion.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                actuacion.Error = true;
                actuacion.Message = exec.Message.ToString();
            }
            return actuacion;
        }

        public RE_ACTUACION insertar(RE_ACTUACION actuacion) {
            try {
                using (SqlConnection cnx = new SqlConnection(this.conexion())) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_ADICIONAR",cnx)){
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", actuacion.actu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@actu_FCantidad",actuacion.actu_FCantidad));
                        
                        if (actuacion.actu_iPersonaRecurrenteId == 0)
                            cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", actuacion.actu_iPersonaRecurrenteId));

                        if (actuacion.actu_iEmpresaRecurrenteId == null)
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", DBNull.Value));
                        else if (actuacion.actu_iEmpresaRecurrenteId == 0)
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", actuacion.actu_iEmpresaRecurrenteId));


                        cmd.Parameters.Add(new SqlParameter("@actu_dFechaRegistro", actuacion.actu_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@actu_sEstado", actuacion.actu_sEstado));
                        cmd.Parameters.Add(new SqlParameter("@actu_sUsuarioCreacion", actuacion.actu_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vIPCreacion", actuacion.actu_vIPCreacion));

                        cmd.Parameters.Add(new SqlParameter("@actu_sCiudadItinerante", actuacion.actu_sCiudadItinerante));
                        cmd.Parameters.Add(new SqlParameter("@actu_vHostName", ""));
                        SqlParameter lReturn = cmd.Parameters.Add("@actu_iActuacionId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        actuacion.actu_iActuacionId = Convert.ToInt64(lReturn.Value);
                        actuacion.Error = false;
                    }
                }
            }
            catch (SqlException exec) {
                strError = exec.Message;
                actuacion.Error = true;
                actuacion.Message = exec.Message.ToString();
            }
            return actuacion;
        }

        public RE_ACTUACION Actualizar(RE_ACTUACION Actuacion)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.[USP_RE_ACTUACION_ACTUALIZAR_JUDICIAL]", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", Actuacion.actu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@actu_FCantidad", Actuacion.actu_FCantidad));

                        if (Actuacion.actu_iPersonaRecurrenteId == 0)
                            cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", Actuacion.actu_iPersonaRecurrenteId));

                        if (Actuacion.actu_iEmpresaRecurrenteId == 0)
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", Actuacion.actu_iEmpresaRecurrenteId));
                        
                        cmd.Parameters.Add(new SqlParameter("@actu_IFuncionarioId", Actuacion.actu_IFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@actu_dFechaRegistro", Actuacion.actu_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@actu_sEstado", Actuacion.actu_sEstado));
                        cmd.Parameters.Add(new SqlParameter("@actu_sUsuarioModificacion", Actuacion.actu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vIPModificacion", Actuacion.actu_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@actu_vHostName", Actuacion.HostName));
                        cmd.Parameters.Add(new SqlParameter("@actu_iActuacionId", Actuacion.actu_iActuacionId));
                                                
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Actuacion.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                Actuacion.Error = true;
                Actuacion.Message = exec.Message.ToString();
            }
            return Actuacion;
        }

        public RE_ACTUACION obtener(RE_ACTUACION Actuacion)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_OBTENER_NOTARIAL", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (Actuacion.actu_iActuacionId != 0) cmd.Parameters.Add(new SqlParameter("@actu_iActuacionId", Actuacion.actu_iActuacionId));
                        if (Actuacion.actu_sOficinaConsularId != 0) cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", Actuacion.actu_sOficinaConsularId));
                        if (Actuacion.actu_iPersonaRecurrenteId != 0) cmd.Parameters.Add(new SqlParameter("@actu_iPersonaRecurrenteId", Actuacion.actu_iPersonaRecurrenteId));
                        if (Actuacion.actu_iEmpresaRecurrenteId != 0) cmd.Parameters.Add(new SqlParameter("@actu_iEmpresaRecurrenteId", Actuacion.actu_iEmpresaRecurrenteId));
                        if (Actuacion.actu_IFuncionarioId != 0) cmd.Parameters.Add(new SqlParameter("@actu_IFuncionarioId", Actuacion.actu_IFuncionarioId));                        
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)Actuacion.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(Actuacion, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }

                        Actuacion.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                Actuacion.Error = true;
                Actuacion.Message = exec.Message.ToString();
            }

            return Actuacion;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
