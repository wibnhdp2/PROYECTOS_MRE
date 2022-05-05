using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class PlantillaTraduccionMantenimientoDA
    {
        ~PlantillaTraduccionMantenimientoDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(RE_PLANTILLA_TRADUCCION pobjBe, ref string s_Mensaje)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
            s_Mensaje = "";

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PLANTILLA_TRADUCCION_ADICIONAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pltr_iEtiquetaId", pobjBe.pltr_iEtiquetaId));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sIdiomaId", pobjBe.pltr_sIdiomaId));
                        cmd.Parameters.Add(new SqlParameter("@pltr_vTraduccion", pobjBe.pltr_vTraduccion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sUsuarioCreacion", pobjBe.pltr_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_vIPCreacion", pobjBe.pltr_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sOficinaConsularId", pobjBe.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pltr_iPlantillaTraduccionId", pobjBe.pltr_iPlantillaTraduccionId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (SqlException sqlex)
            {
                s_Mensaje = sqlex.Message;
                intResultado = sqlex.ErrorCode;
            }
            return intResultado;
        }

        public int Actualizar(RE_PLANTILLA_TRADUCCION pobjBe, ref string s_Mensaje)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PLANTILLA_TRADUCCION_ACTUALIZAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pltr_iPlantillaTraduccionId", pobjBe.pltr_iPlantillaTraduccionId));
                        cmd.Parameters.Add(new SqlParameter("@pltr_iEtiquetaId", pobjBe.pltr_iEtiquetaId));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sIdiomaId", pobjBe.pltr_sIdiomaId));
                        cmd.Parameters.Add(new SqlParameter("@pltr_vTraduccion", pobjBe.pltr_vTraduccion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_cEstado", pobjBe.pltr_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sUsuarioModificacion", pobjBe.pltr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_vIPModificacion", pobjBe.pltr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sOficinaConsularId", pobjBe.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return intResultado;
        }

        public int Anular(RE_PLANTILLA_TRADUCCION pobjBe, ref string s_Mensaje)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PLANTILLA_TRADUCCION_ANULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pltr_iPlantillaTraduccionId", pobjBe.pltr_iPlantillaTraduccionId));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sUsuarioModificacion", pobjBe.pltr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_vIPModificacion", pobjBe.pltr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pltr_sOficinaConsularId", pobjBe.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return intResultado;
        }

    }
}
