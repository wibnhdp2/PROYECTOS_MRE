using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class ExpedienteMantenimientoDA
    {
        ~ExpedienteMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SI_EXPEDIENTE Insertar(SI_EXPEDIENTE objExpediente)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_EXPEDIENTE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@exp_sOficinaConsularId", objExpediente.exp_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@exp_sPeriodo", objExpediente.exp_sPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@exp_sTipoDocMigId", objExpediente.exp_sTipoDocMigId));
                        cmd.Parameters.Add(new SqlParameter("@exp_iNumeroExpediente", objExpediente.exp_INumeroExpediente));
                        cmd.Parameters.Add(new SqlParameter("@exp_sUsuarioCreacion", objExpediente.exp_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@exp_vIPCreacion", objExpediente.exp_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@exp_vHostName", objExpediente.HostName));

                        SqlParameter lReturn = cmd.Parameters.Add("@exp_sExpedienteId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objExpediente.exp_sExpedienteId = Convert.ToInt16(lReturn.Value);
                        objExpediente.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objExpediente.Error = true;
                objExpediente.Message = ex.Message.ToString();
            }
            return objExpediente;
        }

        public SI_EXPEDIENTE Actualizar(SI_EXPEDIENTE objExpediente)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_EXPEDIENTE_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@exp_sExpedienteId", objExpediente.exp_sExpedienteId));
                        cmd.Parameters.Add(new SqlParameter("@exp_sOficinaConsularId", objExpediente.exp_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@exp_sPeriodo", objExpediente.exp_sPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@exp_sTipoDocMigId", objExpediente.exp_sTipoDocMigId));
                        cmd.Parameters.Add(new SqlParameter("@exp_iNumeroExpediente", objExpediente.exp_INumeroExpediente));
                        cmd.Parameters.Add(new SqlParameter("@exp_sUsuarioModificacion", objExpediente.exp_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@exp_vIPModificacion", objExpediente.exp_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@exp_vHostName", objExpediente.HostName));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objExpediente.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objExpediente.Error = true;
                objExpediente.Message = ex.Message;
            }
            return objExpediente;
        }

        public SI_EXPEDIENTE Eliminar(SI_EXPEDIENTE objExpediente)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_EXPEDIENTE_ELIMINAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@exp_sExpedienteId", objExpediente.exp_sExpedienteId));
                        cmd.Parameters.Add(new SqlParameter("@exp_sUsuarioModificacion", objExpediente.exp_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@exp_vIPModificacion", objExpediente.exp_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@exp_sOficinaConsularId", objExpediente.exp_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@exp_vHostName", objExpediente.HostName));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objExpediente.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objExpediente.Error = true;
                objExpediente.Message = ex.Message;
            }
            return objExpediente;
        }
    }
}
