using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.BE.MRE;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.DA 
{
    public class TarifarioRequisitoMantenimientoDA
    {
        ~TarifarioRequisitoMantenimientoDA()
        {
            GC.Collect(); 
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(SI_TARIFARIOREQUISITO pobjBe, ref int intTarifarioRequisitoId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIOREQUISITO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tare_sTarifarioId", pobjBe.tare_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sRequisitoId", pobjBe.tare_sRequisitoId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sTipoActaId", pobjBe.tare_sTipoActaId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sCondicionId", pobjBe.tare_sCondicionId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sUsuarioCreacion", pobjBe.tare_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tare_vIPCreacion", pobjBe.tare_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tare_sOficinaConsularId", pobjBe.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tare_vHostName", Util.ObtenerHostName()));                        

                        SqlParameter lReturn = cmd.Parameters.Add("@tare_sTarifarioRequisitoId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intTarifarioRequisitoId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        public void Update(SI_TARIFARIOREQUISITO pobjBe, ref int intTarifarioRequisitoId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIOREQUISITO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@tare_sTarifarioId", pobjBe.tare_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sRequisitoId", pobjBe.tare_sRequisitoId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sTipoActaId", pobjBe.tare_sTipoActaId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sCondicionId", pobjBe.tare_sCondicionId));
                        cmd.Parameters.Add(new SqlParameter("@tare_sUsuarioModificacion", pobjBe.tare_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tare_vIPModificacion", pobjBe.tare_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tare_sOficinaConsularId", pobjBe.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tare_vHostName", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@tare_sTarifarioRequisitoId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intTarifarioRequisitoId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        public void Delete(SI_TARIFARIOREQUISITO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIOREQUISITO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                               
                        cmd.Parameters.Add(new SqlParameter("@tare_sTarifarioId", pobjBe.tare_sTarifarioId));                        
                        cmd.Parameters.Add(new SqlParameter("@tare_sUsuarioModificacion", pobjBe.tare_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tare_vIPModificacion", pobjBe.tare_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tare_sOficinaConsularId", pobjBe.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tare_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }
    }
}
