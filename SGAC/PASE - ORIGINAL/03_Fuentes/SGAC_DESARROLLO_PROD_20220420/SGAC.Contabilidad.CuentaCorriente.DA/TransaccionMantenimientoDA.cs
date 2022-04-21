using System;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using System.Configuration;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.CuentaCorriente.DA
{
    public class TransaccionMantenimientoDA
    {
        ~TransaccionMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insert(CO_TRANSACCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                        

                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", pobjBE.tran_iTransaccionId)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", pobjBE.tran_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", pobjBE.tran_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMovimientoTipoId", pobjBE.tran_sMovimientoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sTipoId", pobjBE.tran_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMonedaId", pobjBE.tran_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMonto", pobjBE.tran_FMonto));
                        cmd.Parameters.Add(new SqlParameter("@tran_FSaldo", pobjBE.tran_FSaldo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vNumeroOperacion", pobjBE.tran_vNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaOperacion", pobjBE.tran_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodo", pobjBE.tran_cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vFuncionarioEnvio", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@tran_vObservacion", pobjBE.tran_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_sUsuarioCreacion", pobjBE.tran_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vIPCreacion", pobjBE.tran_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vHostName", Util.ObtenerHostName()));

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
        public long InsertNew(CO_TRANSACCION pobjBE)
        {
            long intResultado = (long)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ADICIONAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", pobjBE.tran_iTransaccionId)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", pobjBE.tran_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", pobjBE.tran_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMovimientoTipoId", pobjBE.tran_sMovimientoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sTipoId", pobjBE.tran_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMonedaId", pobjBE.tran_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMonto", pobjBE.tran_FMonto));
                        cmd.Parameters.Add(new SqlParameter("@tran_FSaldo", pobjBE.tran_FSaldo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vNumeroOperacion", pobjBE.tran_vNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaOperacion", pobjBE.tran_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodo", pobjBE.tran_cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vFuncionarioEnvio", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@tran_vObservacion", pobjBE.tran_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_sUsuarioCreacion", pobjBE.tran_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vIPCreacion", pobjBE.tran_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaDepenConsularId", pobjBE.tran_sOficinaDepenConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoRREE", pobjBE.tran_FMontoRREE));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoElectoral", pobjBE.tran_FMontoElectoral));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoPagadoLima", pobjBE.tran_FMontoPagadoLima));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoMilitar", pobjBE.tran_FMontoMilitar));
                        cmd.Parameters.Add(new SqlParameter("@tran_FRetencion", pobjBE.tran_FRetencion));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodoAnterior", pobjBE.tran_cPeriodoAnterior));
                        cmd.Parameters.Add(new SqlParameter("@tran_sEstadoDepositoConciliacion", pobjBE.tran_sEstadoDepositoConciliacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaConciliacion", pobjBE.tran_dFechaConciliacion));
                        SqlParameter lReturn1 = cmd.Parameters.Add("@tran_iTransaccionId", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = Convert.ToInt64(lReturn1.Value) == Convert.ToInt64(Enumerador.enmResultadoQuery.OKRESPUESTA) ? Convert.ToInt64(Enumerador.enmResultadoQuery.OKRESPUESTA) : Convert.ToInt64(Enumerador.enmResultadoQuery.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (long)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }
        public int Update(CO_TRANSACCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", pobjBE.tran_iTransaccionId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", pobjBE.tran_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", pobjBE.tran_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMovimientoTipoId", pobjBE.tran_sMovimientoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sTipoId", pobjBE.tran_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMonedaId", pobjBE.tran_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMonto", pobjBE.tran_FMonto));
                        cmd.Parameters.Add(new SqlParameter("@tran_FSaldo", pobjBE.tran_FSaldo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vNumeroOperacion", pobjBE.tran_vNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaOperacion", pobjBE.tran_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodo", pobjBE.tran_cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vFuncionarioEnvio", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@tran_vObservacion", pobjBE.tran_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_sUsuarioModificacion", pobjBE.tran_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vIPModificacion", pobjBE.tran_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vHostName", Util.ObtenerHostName()));

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
        public int UpdateNew(CO_TRANSACCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ACTUALIZAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", pobjBE.tran_iTransaccionId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", pobjBE.tran_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", pobjBE.tran_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMovimientoTipoId", pobjBE.tran_sMovimientoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sTipoId", pobjBE.tran_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sMonedaId", pobjBE.tran_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMonto", pobjBE.tran_FMonto));
                        cmd.Parameters.Add(new SqlParameter("@tran_FSaldo", pobjBE.tran_FSaldo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vNumeroOperacion", pobjBE.tran_vNumeroOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaOperacion", pobjBE.tran_dFechaOperacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodo", pobjBE.tran_cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_vFuncionarioEnvio", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@tran_vObservacion", pobjBE.tran_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_sUsuarioModificacion", pobjBE.tran_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vIPModificacion", pobjBE.tran_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaDepenConsularId", pobjBE.tran_sOficinaDepenConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoRREE", pobjBE.tran_FMontoRREE));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoElectoral", pobjBE.tran_FMontoElectoral));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoPagadoLima", pobjBE.tran_FMontoPagadoLima));
                        cmd.Parameters.Add(new SqlParameter("@tran_FMontoMilitar", pobjBE.tran_FMontoMilitar));
                        cmd.Parameters.Add(new SqlParameter("@tran_FRetencion", pobjBE.tran_FRetencion));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodoAnterior", pobjBE.tran_cPeriodoAnterior));
                        cmd.Parameters.Add(new SqlParameter("@tran_sEstadoDepositoConciliacion", pobjBE.tran_sEstadoDepositoConciliacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaConciliacion", pobjBE.tran_dFechaConciliacion));
                        SqlParameter lReturn1 = cmd.Parameters.Add("@tran_IResultado", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intResultado = Convert.ToInt32(lReturn1.Value) == (int)Enumerador.enmResultadoQuery.OKRESPUESTA ? (int)Enumerador.enmResultadoQuery.OKRESPUESTA : (int)Enumerador.enmResultadoQuery.OK;
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
        public int Delete(CO_TRANSACCION pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", pobjBE.tran_iTransaccionId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", pobjBE.tran_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sUsuarioModificacion", pobjBE.tran_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vIPModificacion", pobjBE.tran_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vHostName", Util.ObtenerHostName()));

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

        //--------------------------------------------------------------
        //Fecha: 10/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Quitar los parametros de Tipo de Operación y 
        //          Tipo de Transacción. 
        //          DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //--------------------------------------------------------------
        
        public int RegistroMasivoTransacciones(string strXMLExcel,
                                   Int16 sOficinaConsular,
                                   Int16 CuentaCorrienteId,
                                   string cPeriodo,
                                   Int16 sUsuarioCreacion,
                                   string vIPCreacion)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ADICIONAR_MASIVO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pXMLExcel", strXMLExcel));
                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", sOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", CuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodo", cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_sUsuarioCreacion", sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_vIPCreacion", vIPCreacion));

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

        public int ActualizarDatosConciliacion(Int16 sOficinaConsular,
                                   Int64 intTransaccion,
                                   DateTime dFechaConciliacion,
                                   Double NuevoMonto)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_ACTUALIZAR_CONCILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", sOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", intTransaccion));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaConciliacion", dFechaConciliacion));
                        cmd.Parameters.Add(new SqlParameter("@tran_fMonto", NuevoMonto));

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
