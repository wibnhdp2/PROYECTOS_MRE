using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class PaisMantenimientoDA
    {
        ~PaisMantenimientoDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(SI_PAIS pobjBe, int intOficinaConsular, ref string s_Mensaje)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
            s_Mensaje = "";

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PAIS_ADICIONAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pais_vNombre", pobjBe.pais_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@pais_vZonaHoraria", pobjBe.pais_vZonaHoraria));
                        cmd.Parameters.Add(new SqlParameter("@pais_vCapital", pobjBe.pais_vCapital));
                        cmd.Parameters.Add(new SqlParameter("@pais_vNacionalidad", pobjBe.pais_vNacionalidad));
                        cmd.Parameters.Add(new SqlParameter("@pais_vReferenciaMapa", pobjBe.pais_vReferenciaMapa));
                        cmd.Parameters.Add(new SqlParameter("@pais_sMonedaId", pobjBe.pais_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@pais_sContinenteId", pobjBe.pais_sContinenteId));
                        cmd.Parameters.Add(new SqlParameter("@pais_cletra_ISO_3166", pobjBe.pais_cLetra_ISO_3166));
                        cmd.Parameters.Add(new SqlParameter("@pais_sNumero_ISO_3166", pobjBe.pais_sNumero_ISO_3166));
                        cmd.Parameters.Add(new SqlParameter("@pais_vGentilicio_Mas", pobjBe.pais_vGentilicio_Mas));
                        cmd.Parameters.Add(new SqlParameter("@pais_vGentilicio_Fem", pobjBe.pais_vGentilicio_Fem));
                        cmd.Parameters.Add(new SqlParameter("@pais_sUsuarioCreacion", pobjBe.pais_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pais_vIPCreacion", pobjBe.pais_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pais_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@pais_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@pais_sIdioma", pobjBe.pais_sIdiomaId));

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

        public int Actualizar(SI_PAIS pobjBe, int intOficinaConsular)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PAIS_ACTUALIZAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pais_sPaisId", pobjBe.pais_sPaisId));
                        cmd.Parameters.Add(new SqlParameter("@pais_vNombre", pobjBe.pais_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@pais_vZonaHoraria", pobjBe.pais_vZonaHoraria));
                        cmd.Parameters.Add(new SqlParameter("@pais_vCapital", pobjBe.pais_vCapital));
                        cmd.Parameters.Add(new SqlParameter("@pais_vNacionalidad", pobjBe.pais_vNacionalidad));
                        cmd.Parameters.Add(new SqlParameter("@pais_vReferenciaMapa", pobjBe.pais_vReferenciaMapa));
                        cmd.Parameters.Add(new SqlParameter("@pais_sMonedaId", pobjBe.pais_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@pais_sContinenteId", pobjBe.pais_sContinenteId));
                        cmd.Parameters.Add(new SqlParameter("@pais_cletra_ISO_3166", pobjBe.pais_cLetra_ISO_3166));
                        cmd.Parameters.Add(new SqlParameter("@pais_sNumero_ISO_3166", pobjBe.pais_sNumero_ISO_3166));
                        cmd.Parameters.Add(new SqlParameter("@pais_vGentilicio_Mas", pobjBe.pais_vGentilicio_Mas));
                        cmd.Parameters.Add(new SqlParameter("@pais_vGentilicio_Fem", pobjBe.pais_vGentilicio_Fem));
                        cmd.Parameters.Add(new SqlParameter("@pais_sUsuarioModificacion", pobjBe.pais_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pais_vIPModificacion", pobjBe.pais_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pais_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@pais_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@pais_cEstado", pobjBe.pais_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@pais_sIdioma", pobjBe.pais_sIdiomaId));

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

        public int Anular(SI_PAIS pobjBe, int intOficinaConsular)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PAIS_ANULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pais_sPaisId", pobjBe.pais_sPaisId));
                        cmd.Parameters.Add(new SqlParameter("@pais_sUsuarioModificacion", pobjBe.pais_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pais_vIPModificacion", pobjBe.pais_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pais_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@pais_vHostName", Util.ObtenerHostName()));

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
