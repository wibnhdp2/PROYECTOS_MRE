using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.CuentaCorriente.DA
{
    public class CajaChicaConsultasDA
    {
        ~CajaChicaConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ObtenerMovimientosPorNumOperacion(Int16 intOficinaConsularId, Int16 intBancoId, string strNumOperacion,
            Int16 intTipoMovimiento = (Int16) Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_MOVIMIENTOCAJACHICA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MOCA_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@MOCA_sBancoId", intBancoId));
                        cmd.Parameters.Add(new SqlParameter("@MOCA_vNumeroOperacion", strNumOperacion));
                        cmd.Parameters.Add(new SqlParameter("@moca_sTipoMovimientoId", intTipoMovimiento));

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

        public double ObtenerSaldo(Int16 intOficinaConsularId)
        {


            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_MOVIMIENTOCAJACHICA_CONSULTAR_SALDO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@moca_sOficinaConsularId", intOficinaConsularId));

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
