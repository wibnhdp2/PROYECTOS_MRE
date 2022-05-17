using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.Accesorios;
using SGAC.BE.MRE;

namespace SGAC.Cliente.Colas.DA
{
    public class VentanillaDetalleMantenimientoDA
    {
        ~VentanillaDetalleMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insertar(CL_VENTANILLASERVICIO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLASERVICIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vede_sVentanillaId", pobjBe.vede_sVentanillaId));
                        cmd.Parameters.Add(new SqlParameter("@vede_sServicioId", pobjBe.vede_sServicioId));
                        cmd.Parameters.Add(new SqlParameter("@vede_IObligatorio", pobjBe.vede_IObligatorio));
                        cmd.Parameters.Add(new SqlParameter("@vede_sUsuarioCreacion", pobjBe.vede_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vede_vIPCreacion", pobjBe.vede_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vede_sIdOfConsular", pobjBe.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

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

        public void Actualizar(CL_VENTANILLASERVICIO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLASERVICIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vede_sVentanillaId", pobjBe.vede_sVentanillaId));
                        cmd.Parameters.Add(new SqlParameter("@vede_sServicioId", pobjBe.vede_sServicioId));
                        cmd.Parameters.Add(new SqlParameter("@vede_IObligatorio", pobjBe.vede_IObligatorio));
                        cmd.Parameters.Add(new SqlParameter("@vede_sUsuarioModificacion", pobjBe.vede_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vede_vIPModificacion", pobjBe.vede_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vede_sIdOfConsular", pobjBe.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

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

        public void Eliminar(CL_VENTANILLASERVICIO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLASERVICIO_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vede_sVentanillaId", pobjBe.vede_sVentanillaId));
                        cmd.Parameters.Add(new SqlParameter("@vede_sUsuarioModificacion", pobjBe.vede_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vede_vIPModificacion", pobjBe.vede_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vede_sIdOfConsular", pobjBe.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

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
