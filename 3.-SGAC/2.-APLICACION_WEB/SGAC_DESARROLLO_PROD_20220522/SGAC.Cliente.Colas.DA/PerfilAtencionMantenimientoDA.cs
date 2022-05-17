using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.DA
{
    public class PerfilAtencionMantenimientoDA
    {
        ~PerfilAtencionMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(CL_PERFILATENCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_PERFILATENCION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@peat_sOficinaConsularId", pobjBE.peat_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@peat_vDescripcion", pobjBE.peat_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@peat_INumeroTicket", pobjBE.peat_INumeroTicket));
                        cmd.Parameters.Add(new SqlParameter("@peat_ITiempoAtencion", pobjBE.peat_ITiempoAtencion));
                        cmd.Parameters.Add(new SqlParameter("@peat_sUsuarioCreacion", pobjBE.peat_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@peat_vIPCreacion", pobjBE.peat_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@peat_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@peat_IPerfilId", pobjBE.peat_IPerfilId)).Direction = ParameterDirection.Output;

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


        public int Actualizar(CL_PERFILATENCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_PERFILATENCION_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@peat_IPerfilId", pobjBE.peat_IPerfilId));
                        cmd.Parameters.Add(new SqlParameter("@peat_sOficinaConsularId", pobjBE.peat_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@peat_vDescripcion", pobjBE.peat_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@peat_INumeroTicket", pobjBE.peat_INumeroTicket));
                        cmd.Parameters.Add(new SqlParameter("@peat_ITiempoAtencion", pobjBE.peat_ITiempoAtencion));
                        cmd.Parameters.Add(new SqlParameter("@peat_sUsuarioModificacion", pobjBE.peat_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@peat_vIPModificacion", pobjBE.peat_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@peat_vHostName", Util.ObtenerHostName()));

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

        public int Eliminar(CL_PERFILATENCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_PERFILATENCION_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@peat_IPerfilId", pobjBE.peat_IPerfilId));
                        cmd.Parameters.Add(new SqlParameter("@peat_sOficinaConsularId", pobjBE.peat_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@peat_sUsuarioModificacion", pobjBE.peat_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@peat_vIPModificacion", pobjBE.peat_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@peat_vHostName", Util.ObtenerHostName()));
                        
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