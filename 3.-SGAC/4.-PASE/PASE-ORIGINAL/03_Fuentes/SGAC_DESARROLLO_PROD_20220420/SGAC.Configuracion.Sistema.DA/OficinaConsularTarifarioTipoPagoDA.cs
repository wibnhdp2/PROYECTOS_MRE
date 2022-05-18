using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class OficinaConsularTarifarioTipoPagoDA
    {
        ~OficinaConsularTarifarioTipoPagoDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(Int16 intOficinaConsularId, Int16 intTipoPagoId, string strTarifaLetra, bool bExcepcion,
             int intPageSize, int intPageNumber, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_TARIFA_TIPO_PAGO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_STIPOPAGOID", intTipoPagoId));
                        cmd.Parameters.Add(new SqlParameter("@P_TARI_VNUMEROLETRA", strTarifaLetra));
                        cmd.Parameters.Add(new SqlParameter("@P_BEXCEPCION", bExcepcion));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageNumber));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn2.Value);
                        IntTotalPages = Convert.ToInt32(lReturn1.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public SI_OFICINA_TARIFA_TIPO_PAGO Insertar(SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPago)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_TARIFA_TIPO_PAGO_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofpa_sOficinaConsularId", objOficinaTarifaTipoPago.ofpa_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sTarifarioId", objOficinaTarifaTipoPago.ofpa_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sPagoTipoId", objOficinaTarifaTipoPago.ofpa_sPagoTipoId));

                        cmd.Parameters.Add(new SqlParameter("@ofpa_sUsuarioCreacion", objOficinaTarifaTipoPago.ofpa_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_vIPCreacion", objOficinaTarifaTipoPago.ofpa_vIPCreacion));


                        SqlParameter lReturn = cmd.Parameters.Add("@ofpa_iOficinaTipoPagoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objOficinaTarifaTipoPago.ofpa_iOficinaTipoPagoId = Convert.ToInt64(lReturn.Value);
                        objOficinaTarifaTipoPago.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objOficinaTarifaTipoPago.Error = true;
                objOficinaTarifaTipoPago.Message = ex.Message.ToString();
            }
            return objOficinaTarifaTipoPago;
        }

        public SI_OFICINA_TARIFA_TIPO_PAGO Actualizar(SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPago)
        {

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_TARIFA_TIPO_PAGO_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofpa_iOficinaTipoPagoId", objOficinaTarifaTipoPago.ofpa_iOficinaTipoPagoId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sOficinaConsularId", objOficinaTarifaTipoPago.ofpa_sOficinaConsularId));

                        cmd.Parameters.Add(new SqlParameter("@ofpa_sTarifarioId", objOficinaTarifaTipoPago.ofpa_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sPagoTipoId", objOficinaTarifaTipoPago.ofpa_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sUsuarioModificacion", objOficinaTarifaTipoPago.ofpa_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_vIPModificacion", objOficinaTarifaTipoPago.ofpa_vIPModificacion));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objOficinaTarifaTipoPago.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objOficinaTarifaTipoPago.Error = true;
                objOficinaTarifaTipoPago.Message = ex.Message.ToString();
            }
            return objOficinaTarifaTipoPago;
        }

        public SI_OFICINA_TARIFA_TIPO_PAGO Anular(SI_OFICINA_TARIFA_TIPO_PAGO objOficinaTarifaTipoPago)
        {

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_TARIFA_TIPO_PAGO_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofpa_sOficinaConsularId", objOficinaTarifaTipoPago.ofpa_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sTarifarioId", objOficinaTarifaTipoPago.ofpa_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sPagoTipoId", objOficinaTarifaTipoPago.ofpa_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_sUsuarioModificacion", objOficinaTarifaTipoPago.ofpa_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofpa_vIPModificacion", objOficinaTarifaTipoPago.ofpa_vIPModificacion));


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objOficinaTarifaTipoPago.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objOficinaTarifaTipoPago.Error = true;
                objOficinaTarifaTipoPago.Message = ex.Message.ToString();
            }
            return objOficinaTarifaTipoPago;
        }
    }
}
