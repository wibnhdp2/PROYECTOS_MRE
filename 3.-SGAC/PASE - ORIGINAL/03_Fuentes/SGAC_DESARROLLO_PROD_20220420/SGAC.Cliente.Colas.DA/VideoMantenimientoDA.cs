using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.DA
{
    public class VideoMantenimientoDA
    {
        ~VideoMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(CL_VIDEO pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VIDEO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vide_sOficinaConsularId", pobjBE.vide_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vide_vDescripcion", pobjBE.vide_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@vide_IOrden", pobjBE.vide_IOrden));
                        cmd.Parameters.Add(new SqlParameter("@vide_vUrl", pobjBE.vide_vUrl));
                        cmd.Parameters.Add(new SqlParameter("@vide_sUsuarioCreacion", pobjBE.vide_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vide_vIPCreacion", pobjBE.vide_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vide_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@vide_sVideoId", pobjBE.vide_sVideoId)).Direction = ParameterDirection.Output;


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


        public int Actualizar(CL_VIDEO pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VIDEO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vide_sVideoId", pobjBE.vide_sVideoId));
                        cmd.Parameters.Add(new SqlParameter("@vide_sOficinaConsularId", pobjBE.vide_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vide_vDescripcion", pobjBE.vide_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@vide_IOrden", pobjBE.vide_IOrden));
                        cmd.Parameters.Add(new SqlParameter("@vide_vUrl", pobjBE.vide_vUrl));
                        cmd.Parameters.Add(new SqlParameter("@vide_sUsuarioModificacion", pobjBE.vide_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vide_vIPModificacion", pobjBE.vide_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vide_vHostName", Util.ObtenerHostName()));

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

        public int Eliminar(CL_VIDEO pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VIDEO_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vide_sVideoId", pobjBE.vide_sVideoId));
                        cmd.Parameters.Add(new SqlParameter("@vide_sOficinaConsularId", pobjBE.vide_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vide_sUsuarioModificacion", pobjBE.vide_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vide_vIPModificacion", pobjBE.vide_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vide_vHostName", Util.ObtenerHostName()));

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
