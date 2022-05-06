using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class TipoCambioBancarioMantenimientoDA
    { 
        ~TipoCambioBancarioMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        public int Insert(ref SI_TIPOCAMBIO_BANCARIO pobjBe)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_BANCARIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tiba_dFecha", pobjBe.tiba_dFecha));
                        cmd.Parameters.Add(new SqlParameter("@tiba_dFechaFin", pobjBe.tiba_dFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@tiba_FValorBancario", pobjBe.tiba_FValorBancario));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sOficinaConsularId", pobjBe.tiba_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sMonedaId", pobjBe.tiba_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sUsuarioCreacion", pobjBe.tiba_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tiba_vIPCreacion", pobjBe.tiba_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tiba_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@tiba_iTipoCambioBancarioId", pobjBe.tiba_iTipoCambioBancarioId)).Direction = ParameterDirection.Output;

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

        public int Update(SI_TIPOCAMBIO_BANCARIO pobjBe)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_BANCARIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tiba_iTipoCambioBancarioId", pobjBe.tiba_iTipoCambioBancarioId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_dFecha", pobjBe.tiba_dFecha));
                        cmd.Parameters.Add(new SqlParameter("@tiba_FValorBancario", pobjBe.tiba_FValorBancario));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sOficinaConsularId", pobjBe.tiba_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sMonedaId", pobjBe.tiba_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sUsuarioModificacion", pobjBe.tiba_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tiba_vIPModificacion", pobjBe.tiba_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tiba_vHostName", Util.ObtenerHostName()));

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

        public int Delete(SI_TIPOCAMBIO_BANCARIO pobjBe)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_BANCARIO_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tiba_iTipoCambioBancarioId", pobjBe.tiba_iTipoCambioBancarioId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sOficinaConsularId", pobjBe.tiba_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_sUsuarioModificacion", pobjBe.tiba_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tiba_vIPModificacion", pobjBe.tiba_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tiba_vHostName", Util.ObtenerHostName()));

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