using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.BE.MRE;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.DA
{
    public class TarifarioMantenimientoDA
    {
        ~TarifarioMantenimientoDA()
        {
            GC.Collect(); 
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(SI_TARIFARIO pobjBe, ref int intTarifarioId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sSeccionId", pobjBe.tari_sSeccionId));

                        if (pobjBe.tari_sNumero != 0)
                            cmd.Parameters.Add(new SqlParameter("@tari_sNumero", pobjBe.tari_sNumero));

                        if (pobjBe.tari_vLetra == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vLetra", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vLetra", pobjBe.tari_vLetra));
                        }

                        cmd.Parameters.Add(new SqlParameter("@tari_vDescripcionCorta", pobjBe.tari_vDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@tari_vDescripcion", pobjBe.tari_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@tari_sBasePercepcionId", pobjBe.tari_sBasePercepcionId));
                        cmd.Parameters.Add(new SqlParameter("@tari_sCalculoTipoId", pobjBe.tari_sCalculoTipoId));

                        if (pobjBe.tari_vCalculoFormula == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vCalculoFormula", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vCalculoFormula", pobjBe.tari_vCalculoFormula));
                        }

                        cmd.Parameters.Add(new SqlParameter("@tari_FCosto", pobjBe.tari_FCosto));
                        cmd.Parameters.Add(new SqlParameter("@tari_sTopeUnidadId", pobjBe.tari_sTopeUnidadId));
                        cmd.Parameters.Add(new SqlParameter("@tari_ITopeCantidad", pobjBe.tari_ITopeCantidad));

                        if (pobjBe.tari_FMontoExceso == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_FMontoExceso", DBNull.Value));
                        }
                        else 
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_FMontoExceso", pobjBe.tari_FMontoExceso));
                        }

                        if (pobjBe.tari_bTarifarioDependienteFlag == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bTarifarioDependienteFlag", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bTarifarioDependienteFlag", pobjBe.tari_bTarifarioDependienteFlag));
                        }

                        if (pobjBe.tari_dVigenciaInicio.ToString().Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaInicio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaInicio", pobjBe.tari_dVigenciaInicio));
                        }

                        if (pobjBe.tari_dVigenciaFin.ToString().Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaFin", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaFin", pobjBe.tari_dVigenciaFin));
                        }

                        if (pobjBe.tari_bHabilitaCantidad == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bHabilitaCantidad", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bHabilitaCantidad", pobjBe.tari_bHabilitaCantidad));
                        }

                        cmd.Parameters.Add(new SqlParameter("@tari_sEstadoId", pobjBe.tari_sEstadoId));

                        cmd.Parameters.Add(new SqlParameter("@tari_sUsuarioCreacion", pobjBe.tari_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tari_vIPCreacion", pobjBe.tari_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tari_sOficinaConsularId", pobjBe.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tari_vHostName", Util.ObtenerHostName()));

                        if (pobjBe.tari_bFlagExcepcion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bFlagExcepcion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bFlagExcepcion", pobjBe.tari_bFlagExcepcion));
                        }

                        
                        SqlParameter lReturn = cmd.Parameters.Add("@tari_sTarifarioId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intTarifarioId = Convert.ToInt32(lReturn.Value);

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

        public void Update(SI_TARIFARIO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sTarifarioId", pobjBe.tari_sTarifarioId));

                        cmd.Parameters.Add(new SqlParameter("@tari_sSeccionId", pobjBe.tari_sSeccionId));

                        if (pobjBe.tari_sNumero != null)
                            cmd.Parameters.Add(new SqlParameter("@tari_sNumero", pobjBe.tari_sNumero));

                        if (pobjBe.tari_vLetra == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vLetra", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vLetra", pobjBe.tari_vLetra));
                        }

                        cmd.Parameters.Add(new SqlParameter("@tari_vDescripcionCorta", pobjBe.tari_vDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@tari_vDescripcion", pobjBe.tari_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@tari_sBasePercepcionId", pobjBe.tari_sBasePercepcionId));
                        cmd.Parameters.Add(new SqlParameter("@tari_sCalculoTipoId", pobjBe.tari_sCalculoTipoId));

                        if (pobjBe.tari_vCalculoFormula == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vCalculoFormula", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vCalculoFormula", pobjBe.tari_vCalculoFormula));
                        }

                        cmd.Parameters.Add(new SqlParameter("@tari_FCosto", pobjBe.tari_FCosto));
                        cmd.Parameters.Add(new SqlParameter("@tari_sTopeUnidadId", pobjBe.tari_sTopeUnidadId));
                        cmd.Parameters.Add(new SqlParameter("@tari_ITopeCantidad", pobjBe.tari_ITopeCantidad));

                        if (pobjBe.tari_FMontoExceso == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_FMontoExceso", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_FMontoExceso", pobjBe.tari_FMontoExceso));
                        }

                        if (pobjBe.tari_bTarifarioDependienteFlag == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bTarifarioDependienteFlag", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bTarifarioDependienteFlag", pobjBe.tari_bTarifarioDependienteFlag));
                        }

                        if (pobjBe.tari_dVigenciaInicio.ToString().Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaInicio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaInicio", pobjBe.tari_dVigenciaInicio));
                        }

                        if (pobjBe.tari_dVigenciaFin.ToString().Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaFin", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_dVigenciaFin", pobjBe.tari_dVigenciaFin));
                        }

                        if (pobjBe.tari_bHabilitaCantidad == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bHabilitaCantidad", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bHabilitaCantidad", pobjBe.tari_bHabilitaCantidad));
                        }

                        cmd.Parameters.Add(new SqlParameter("@tari_sEstadoId", pobjBe.tari_sEstadoId));

                        cmd.Parameters.Add(new SqlParameter("@tari_sUsuarioModificacion", pobjBe.tari_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tari_vIPModificacion", pobjBe.tari_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tari_sOficinaConsularId", pobjBe.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tari_vHostName", Util.ObtenerHostName()));
                        
                        if (pobjBe.tari_bFlagExcepcion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bFlagExcepcion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_bFlagExcepcion", pobjBe.tari_bFlagExcepcion));
                        }

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

        public void Delete(SI_TARIFARIO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sTarifarioId", pobjBe.tari_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@tari_sUsuarioModificacion", pobjBe.tari_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tari_vIPModificacion", pobjBe.tari_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tari_sOficinaConsularId", pobjBe.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tari_vHostName", Util.ObtenerHostName()));

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
