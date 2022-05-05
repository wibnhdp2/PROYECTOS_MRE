using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class UbigeoMantenimientoDA
    {
        ~UbigeoMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(SI_UBICACIONGEOGRAFICA pobjBe, int intOficinaConsular, ref string s_Mensaje)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
            s_Mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ubge_cCodigo", pobjBe.ubge_cCodigo));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi01", pobjBe.ubge_cUbi01));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi02", pobjBe.ubge_cUbi02));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi03", pobjBe.ubge_cUbi03));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vDepartamento", pobjBe.ubge_vDepartamento));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vProvincia", pobjBe.ubge_vProvincia));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vDistrito", pobjBe.ubge_vDistrito));
                        cmd.Parameters.Add(new SqlParameter("@ubge_sUsuarioCreacion", pobjBe.ubge_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vIPCreacion", pobjBe.ubge_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ubge_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vSiglaPais", pobjBe.ubge_vSiglaPais));

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

        public int Actualizar(SI_UBICACIONGEOGRAFICA pobjBe, int intOficinaConsular)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ubge_cCodigo", pobjBe.ubge_cCodigo));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vDepartamento", pobjBe.ubge_vDepartamento));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vProvincia", pobjBe.ubge_vProvincia));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vDistrito", pobjBe.ubge_vDistrito));
                        cmd.Parameters.Add(new SqlParameter("@ubge_sUsuarioModificacion", pobjBe.ubge_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vIPModificacion", pobjBe.ubge_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ubge_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vSiglaPais", pobjBe.ubge_vSiglaPais));

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


        public int Eliminar(SI_UBICACIONGEOGRAFICA pobjBe, int intOficinaConsular)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ubge_cCodigo", pobjBe.ubge_cCodigo));
                        cmd.Parameters.Add(new SqlParameter("@ubge_sUsuarioModificacion", pobjBe.ubge_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vIPModificacion", pobjBe.ubge_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ubge_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@ubge_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@Estado", pobjBe.ubge_cEstado));
                        

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