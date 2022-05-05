using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.DA
{
    public class TelevisorMantenimientoDA
    {
        ~TelevisorMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        public int Insertar(CL_TELEVISOR pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TELEVISOR_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@telv_sOficinaConsularId", pobjBE.telv_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@telv_vDescripcion", pobjBE.telv_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vMarca", pobjBE.telv_vMarca));
                        cmd.Parameters.Add(new SqlParameter("@telv_vModelo", pobjBE.telv_vModelo));
                        cmd.Parameters.Add(new SqlParameter("@telv_vSerie", pobjBE.telv_vSerie));
                        if (pobjBE.telv_vCaracteristicas == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@telv_vCaracteristicas", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@telv_vCaracteristicas", pobjBE.telv_vCaracteristicas));
                        }
                        cmd.Parameters.Add(new SqlParameter("@telv_sUsuarioCreacion", pobjBE.telv_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vIPCreacion", pobjBE.telv_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@telv_sTelevisorId", pobjBE.telv_sTelevisorId)).Direction = ParameterDirection.Output;

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

        public int Actualizar(CL_TELEVISOR pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TELEVISOR_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@telv_sTelevisorId", pobjBE.telv_sTelevisorId));
                        cmd.Parameters.Add(new SqlParameter("@telv_sOficinaConsularId", pobjBE.telv_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@telv_vDescripcion", pobjBE.telv_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vMarca", pobjBE.telv_vMarca));
                        cmd.Parameters.Add(new SqlParameter("@telv_vModelo", pobjBE.telv_vModelo));
                        cmd.Parameters.Add(new SqlParameter("@telv_vSerie", pobjBE.telv_vSerie));
                        if (pobjBE.telv_vCaracteristicas == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@telv_vCaracteristicas", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@telv_vCaracteristicas", pobjBE.telv_vCaracteristicas));
                        }
                        cmd.Parameters.Add(new SqlParameter("@telv_sUsuarioModificacion", pobjBE.telv_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vIPModificacion", pobjBE.telv_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vHostName", Util.ObtenerHostName()));


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

        public int Eliminar(CL_TELEVISOR pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TELEVISOR_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@telv_sTelevisorId", pobjBE.telv_sTelevisorId));
                        cmd.Parameters.Add(new SqlParameter("@telv_sOficinaConsularId", pobjBE.telv_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@telv_sUsuarioModificacion", pobjBE.telv_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vIPModificacion", pobjBE.telv_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@telv_vHostName", Util.ObtenerHostName()));

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
