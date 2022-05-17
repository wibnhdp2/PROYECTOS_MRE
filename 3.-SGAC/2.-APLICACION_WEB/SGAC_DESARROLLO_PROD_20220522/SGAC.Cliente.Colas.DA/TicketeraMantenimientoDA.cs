using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.DA
{
    public class TicketeraMantenimientoDA
    {
        ~TicketeraMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        
        public int Insertar(ref CL_TICKETERA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TICKETERA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tira_sOficinaConsularId", pobjBE.tira_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tira_vNombre", pobjBE.tira_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@tira_vMarca", pobjBE.tira_vMarca));
                        cmd.Parameters.Add(new SqlParameter("@tira_vModelo", pobjBE.tira_vModelo));
                        cmd.Parameters.Add(new SqlParameter("@tira_vSerie", pobjBE.tira_vSerie));
                        if (pobjBE.tira_vCaracteristicas == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tira_vCaracteristicas", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tira_vCaracteristicas", pobjBE.tira_vCaracteristicas));
                        }
                        cmd.Parameters.Add(new SqlParameter("@tira_sUsuarioCreacion", pobjBE.tira_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tira_vIPCreacion", pobjBE.tira_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tira_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@tira_sTicketeraId", pobjBE.tira_sTicketeraId)).Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }

        public int Actualizar(CL_TICKETERA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TICKETERA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tira_sTicketeraId", pobjBE.tira_sTicketeraId));
                        cmd.Parameters.Add(new SqlParameter("@tira_sOficinaConsularId", pobjBE.tira_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tira_vNombre", pobjBE.tira_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@tira_vMarca", pobjBE.tira_vMarca));
                        cmd.Parameters.Add(new SqlParameter("@tira_vModelo", pobjBE.tira_vModelo));
                        cmd.Parameters.Add(new SqlParameter("@tira_vSerie", pobjBE.tira_vSerie));
                        if (pobjBE.tira_vCaracteristicas == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tira_vCaracteristicas", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tira_vCaracteristicas", pobjBE.tira_vCaracteristicas));
                        }
                        cmd.Parameters.Add(new SqlParameter("@tira_sUsuarioModificacion", pobjBE.tira_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tira_vIPModificacion", pobjBE.tira_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tira_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }

        public int Eliminar(CL_TICKETERA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TICKETERA_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tira_sTicketeraId", pobjBE.tira_sTicketeraId));
                        cmd.Parameters.Add(new SqlParameter("@tira_sOficinaConsularId", pobjBE.tira_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tira_sUsuarioModificacion", pobjBE.tira_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tira_vIPModificacion", pobjBE.tira_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tira_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }
    }
}