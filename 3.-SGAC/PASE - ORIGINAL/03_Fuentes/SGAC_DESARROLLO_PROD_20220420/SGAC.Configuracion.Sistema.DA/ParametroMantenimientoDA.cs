using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class ParametroMantenimientoDA
    {
        ~ParametroMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(SI_PARAMETRO pobjBe, int intOficinaConsular)
        {
            int IntResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@para_vGrupo", pobjBe.para_vGrupo));
                        cmd.Parameters.Add(new SqlParameter("@para_vDescripcion", pobjBe.para_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@para_vValor", pobjBe.para_vValor));
                        if (pobjBe.para_vReferencia == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_vReferencia", DBNull.Value));
                        }
                        else if (pobjBe.para_vReferencia.Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_vReferencia", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_vReferencia", pobjBe.para_vReferencia));
                        }

                        if (pobjBe.para_tOrden == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_tOrden", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_tOrden", pobjBe.para_tOrden));
                        }

                        cmd.Parameters.Add(new SqlParameter("@para_bVisible", pobjBe.para_bVisible));

                        if (pobjBe.para_dVigenciaInicio == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaInicio", DBNull.Value));
                        }
                        else if (pobjBe.para_dVigenciaInicio == DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaInicio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaInicio", pobjBe.para_dVigenciaInicio));
                        }

                        if (pobjBe.para_dVigenciaFin == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaFin", DBNull.Value));
                        }
                        else if (pobjBe.para_dVigenciaFin == DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaFin", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaFin", pobjBe.para_dVigenciaFin));
                        }
                                                                                              
                        cmd.Parameters.Add(new SqlParameter("@para_bPrecarga", pobjBe.para_bPrecarga));
                        cmd.Parameters.Add(new SqlParameter("@para_sUsuarioCreacion", pobjBe.para_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@para_vIPCreacion", pobjBe.para_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@para_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@para_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@para_sParametroId", pobjBe.para_sParametroId)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@para_bFlagExcepcion", pobjBe.para_bFlagExcepcion));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        IntResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                IntResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return IntResultado;
        }

        public int Actualizar(SI_PARAMETRO pobjBe, int intOficinaConsular)
        {
            int IntResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                                                
                        cmd.Parameters.Add(new SqlParameter("@para_sParametroId", pobjBe.para_sParametroId));
                        cmd.Parameters.Add(new SqlParameter("@para_vGrupo", pobjBe.para_vGrupo));
                        cmd.Parameters.Add(new SqlParameter("@para_vDescripcion", pobjBe.para_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@para_vValor", pobjBe.para_vValor));

                        if (pobjBe.para_vReferencia == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_vReferencia", DBNull.Value));
                        }
                        else if (pobjBe.para_vReferencia.Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_vReferencia", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_vReferencia", pobjBe.para_vReferencia));
                        }

                        if (pobjBe.para_tOrden == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_tOrden", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_tOrden", pobjBe.para_tOrden));
                        }

                        cmd.Parameters.Add(new SqlParameter("@para_bVisible", pobjBe.para_bVisible));

                        if (pobjBe.para_dVigenciaInicio == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaInicio", DBNull.Value));
                        }
                        else if (pobjBe.para_dVigenciaInicio == DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaInicio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaInicio", pobjBe.para_dVigenciaInicio));
                        }

                        if (pobjBe.para_dVigenciaFin == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaFin", DBNull.Value));
                        }
                        else if (pobjBe.para_dVigenciaFin == DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaFin", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@para_dVigenciaFin", pobjBe.para_dVigenciaFin));
                        }

                        cmd.Parameters.Add(new SqlParameter("@para_cEstado", pobjBe.para_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@para_bPrecarga", pobjBe.para_bPrecarga));
                        cmd.Parameters.Add(new SqlParameter("@para_sUsuarioModificacion", pobjBe.para_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@para_vIPModificacion", pobjBe.para_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@para_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@para_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@para_bFlagExcepcion", pobjBe.para_bFlagExcepcion));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        IntResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                IntResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return IntResultado;
        }

        public int Eliminar(SI_PARAMETRO pobjBe, int intOficinaConsular)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("para_sParametroId", pobjBe.para_sParametroId));
                        cmd.Parameters.Add(new SqlParameter("para_sUsuarioModificacion", pobjBe.para_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("para_vIPModificacion", pobjBe.para_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("para_sOficinaConsularId", intOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("para_vHostName", Util.ObtenerHostName()));

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
