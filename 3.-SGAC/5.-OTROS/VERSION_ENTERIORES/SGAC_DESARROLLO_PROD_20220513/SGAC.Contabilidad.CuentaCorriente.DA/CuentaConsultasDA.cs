using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Contabilidad.CuentaCorriente.DA
{
    public class CuentaConsultasDA
    {
        ~CuentaConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        //---------------------------------------------
        //Fecha: 11/02/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Se adicionan dos parametros:
        //         strPeriodo y  iTransaccionId.
        //---------------------------------------------
        public DataTable ObtenerPorNroCuenta(int IOficinaConsularId, int iBancoId, string strNroCuenta, Int16 iCodCuentaCorriente = 0, string strPeriodo="", long iTransaccionId=0)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_CONSULTAR_POR_NROCUENTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", IOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sBancoId", iBancoId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vNroCuenta", strNroCuenta));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sCuentaCorrienteId", iCodCuentaCorriente));
                        cmd.Parameters.Add(new SqlParameter("@tran_cPeriodo", strPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@tran_iTransaccionId", iTransaccionId));

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

        public System.Data.DataTable ObtenerPorOficinaConsular(int iOficinaConsular)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_CONSULTAR_POR_OFICINA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", iOficinaConsular));                        

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

        public DataTable Consultar(int intOficinaConsularId,
                                   int intBancoId,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas,
                                   string strEstado = "A")
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sBancoId", intBancoId));                       

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));
                        cmd.Parameters.Add(new SqlParameter("@cuco_cEstado", strEstado));

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

        public int Existe(int IntCtaId, string StrNroCuenta, int IntBancoId, int IntOperacion)
        {
            int Rspta = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_EXISTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sCuentaCorrienteId", IntCtaId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vNumero", StrNroCuenta));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sBancoId", IntBancoId));
                        cmd.Parameters.Add(new SqlParameter("@IOperacion", IntOperacion));

                        SqlParameter lReturn = cmd.Parameters.Add("@Rspta", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Rspta = Convert.ToInt32(lReturn.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                Rspta = -1;
                throw exec;
            }

            return Rspta;
        }

        public double ObtenerSaldo(Int16 sCuentaCorrienteId)
        {


            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_CONSULTAR_SALDO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sCuentaCorrienteId", sCuentaCorrienteId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dt = dsObjeto.Tables[0];
                        }

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                return Convert.ToDouble(dt.Rows[0]["fSaldo"].ToString());
                            }
                        }

                        return 0;

                    }
                }
            }
            catch (SqlException exec)
            {
                return 0;
            }
        }
    }
}
