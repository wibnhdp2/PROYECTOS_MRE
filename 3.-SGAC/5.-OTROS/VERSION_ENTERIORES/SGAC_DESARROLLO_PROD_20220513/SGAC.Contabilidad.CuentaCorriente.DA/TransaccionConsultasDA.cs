using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Contabilidad.CuentaCorriente.DA
{
    public class TransaccionConsultasDA
    {
        ~TransaccionConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ObtenerPorCuenta(int intOficinaConsularId,
                                          int intBancoId,
                                          string strNumeroCuenta,
                                          DateTime dFechaInicio,
                                          DateTime dFechaFin,
                                          int anioPeriodo,
                                          int mesPeriodo,
                                          string busqueda,
                                          string NroOpeacion,
                                          Int16 intCodCuentaCorriente,
                                          int intPaginaActual,
                                          int intPaginaCantidad,
                                          ref int intTotalRegistros,
                                          ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_CONSULTAR_POR_CUENTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sBancoId", intBancoId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vNumero", strNumeroCuenta));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaInicio", dFechaInicio.ToString("yyyy-MM-dd 00:00:00")));
                        cmd.Parameters.Add(new SqlParameter("@tran_dFechaFin", dFechaFin.ToString("yyyy-MM-dd 23:59:59")));
                        cmd.Parameters.Add(new SqlParameter("@tran_sPeriodoAnio", anioPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_sPeriodoMes", mesPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@cBusqueda", busqueda));
                        cmd.Parameters.Add(new SqlParameter("@tran_vNumeroOperacion", NroOpeacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sCuentaCorrienteId", intCodCuentaCorriente));



                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ObtenerBancoCuenta(int intOficinaConsularId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_LISTA_BANCO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", intOficinaConsularId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        //--------------------------------------------------------------
        //Fecha: 10/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Quitar los parametros de Tipo de Operación y 
        //          Tipo de Transacción. 
        //          DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //--------------------------------------------------------------

        public DataTable VerificarRegistroMasivo(string strXMLExcel,
                                   Int16 CuentaCorrienteId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_VERIFICAR_MASIVO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pXMLExcel", strXMLExcel));
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", CuentaCorrienteId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }


        public DataTable ListarConciliacionesPendientes(int intOficinaConsularId,
                                          Int16 intCuentaCorriente,
                                          Int64 TransaccionPadre)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_TRANSACCION_CONSULTAR_CONCILIACION_PENDIENTES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tran_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tran_sCuentaCorrienteId", intCuentaCorriente));
                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId_PADRE", TransaccionPadre));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }
    }
}
