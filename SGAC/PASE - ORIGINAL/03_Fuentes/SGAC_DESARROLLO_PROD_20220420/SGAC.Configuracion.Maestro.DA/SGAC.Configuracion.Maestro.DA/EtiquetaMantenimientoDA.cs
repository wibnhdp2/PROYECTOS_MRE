using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Maestro.DA
{
    public class EtiquetaMantenimientoDA
    {
        ~EtiquetaMantenimientoDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(MA_ETIQUETA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ETIQUETA_ADICIONAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@etiq_sPlantillaId", pobjBE.etiq_sPlantillaId));
                        cmd.Parameters.Add(new SqlParameter("@etiq_tOrden", pobjBE.etiq_tOrden));
                        cmd.Parameters.Add(new SqlParameter("@etiq_vEtiqueta", pobjBE.etiq_vEtiqueta));
                        cmd.Parameters.Add(new SqlParameter("@etiq_sUsuarioCreacion", pobjBE.etiq_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@etiq_vIPCreacion", pobjBE.etiq_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@etiq_sOficinaConsularId", pobjBE.OficinaConsultar));

                        cmd.Parameters.Add(new SqlParameter("@etiq_iEtiquetaId", pobjBE.etiq_iEtiquetaId)).Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (SqlException ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intResultado;
        }

        public int Actualizar(MA_ETIQUETA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ETIQUETA_ACTUALIZAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@etiq_iEtiquetaId", pobjBE.etiq_iEtiquetaId));
                        cmd.Parameters.Add(new SqlParameter("@etiq_sPlantillaId", pobjBE.etiq_sPlantillaId));
                        cmd.Parameters.Add(new SqlParameter("@etiq_tOrden", pobjBE.etiq_tOrden));
                        cmd.Parameters.Add(new SqlParameter("@etiq_vEtiqueta", pobjBE.etiq_vEtiqueta));
                        cmd.Parameters.Add(new SqlParameter("@etiq_cEstado", pobjBE.etiq_cEstado));

                        cmd.Parameters.Add(new SqlParameter("@etiq_sUsuarioModificacion", pobjBE.etiq_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@etiq_vIPModificacion", pobjBE.etiq_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@etiq_sOficinaConsularId", pobjBE.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (SqlException ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intResultado;
        }

        public int Anular(MA_ETIQUETA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ETIQUETA_ANULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@etiq_iEtiquetaId", pobjBE.etiq_iEtiquetaId));

                        cmd.Parameters.Add(new SqlParameter("@etiq_sUsuarioModificacion", pobjBE.etiq_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@etiq_vIPModificacion", pobjBE.etiq_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@etiq_sOficinaConsularId", pobjBE.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (SqlException ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intResultado;
        }
    }
}
