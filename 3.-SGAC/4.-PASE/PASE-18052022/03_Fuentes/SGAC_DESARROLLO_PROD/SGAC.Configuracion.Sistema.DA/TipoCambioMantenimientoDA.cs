using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class TipoCambioMantenimientoDA
    {
         ~TipoCambioMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        public int Insert(ref SI_TIPOCAMBIO_CONSULAR pobjBe)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_CONSULAR_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tico_dFecha", pobjBe.tico_dFecha));
                        cmd.Parameters.Add(new SqlParameter("@tico_dFechaFin", pobjBe.tico_dFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@tico_FValorBancario", pobjBe.tico_FValorBancario));
                        cmd.Parameters.Add(new SqlParameter("@tico_FPorcentaje", pobjBe.tico_FPorcentaje));
                        cmd.Parameters.Add(new SqlParameter("@tico_FPromedio", pobjBe.tico_FPromedio));
                        cmd.Parameters.Add(new SqlParameter("@tico_FValorConsular", pobjBe.tico_FValorConsular));
                        cmd.Parameters.Add(new SqlParameter("@tico_sOficinaConsularId", pobjBe.tico_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tico_sMonedaId", pobjBe.tico_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tico_sUsuarioCreacion", pobjBe.tico_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tico_vIPCreacion", pobjBe.tico_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tico_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@tico_iTipoCambioConsularId", pobjBe.tico_iTipoCambioConsularId)).Direction = ParameterDirection.Output;

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

        public int Update(SI_TIPOCAMBIO_CONSULAR pobjBe)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_CONSULAR_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tico_iTipoCambioConsularId", pobjBe.tico_iTipoCambioConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tico_dFecha", pobjBe.tico_dFecha));
                        cmd.Parameters.Add(new SqlParameter("@tico_FValorBancario", pobjBe.tico_FValorBancario));
                        cmd.Parameters.Add(new SqlParameter("@tico_FPorcentaje", pobjBe.tico_FPorcentaje));
                        cmd.Parameters.Add(new SqlParameter("@tico_FPromedio", pobjBe.tico_FPromedio));
                        cmd.Parameters.Add(new SqlParameter("@tico_FValorConsular", pobjBe.tico_FValorConsular));
                        cmd.Parameters.Add(new SqlParameter("@tico_sOficinaConsularId", pobjBe.tico_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tico_sMonedaId", pobjBe.tico_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tico_sUsuarioModificacion", pobjBe.tico_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tico_vIPModificacion", pobjBe.tico_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tico_vHostName", Util.ObtenerHostName()));

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

        public int Delete(SI_TIPOCAMBIO_CONSULAR pobjBe)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_CONSULAR_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tico_iTipoCambioConsularId", pobjBe.tico_iTipoCambioConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tico_sOficinaConsularId", pobjBe.tico_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tico_sUsuarioModificacion", pobjBe.tico_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tico_vIPModificacion", pobjBe.tico_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tico_vHostName", Util.ObtenerHostName()));


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