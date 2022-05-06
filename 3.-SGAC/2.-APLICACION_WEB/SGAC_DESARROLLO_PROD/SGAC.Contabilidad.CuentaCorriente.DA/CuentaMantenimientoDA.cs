using System;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using System.Configuration;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.CuentaCorriente.DA
{
    public class CuentaMantenimientoDA
    {
        ~CuentaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public CO_CUENTACORRIENTE Insert(CO_CUENTACORRIENTE pobjBE)
        {
            pobjBE.Message = string.Empty;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", pobjBE.cuco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sTipoId", pobjBE.cuco_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sBancoId", pobjBE.cuco_sBancoId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sMonedaId", pobjBE.cuco_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vNumero", pobjBE.cuco_vNumero));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vRepresentante", pobjBE.cuco_vRepresentante));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vSucursal", pobjBE.cuco_vSucursal));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sSituacionId", pobjBE.cuco_sSituacionId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vObservacion", pobjBE.cuco_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sUsuarioCreacion", pobjBE.cuco_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vIPCreacion", pobjBE.cuco_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vHostName", Util.ObtenerHostName()));

                        SqlParameter intCuentaCorrienteId = cmd.Parameters.Add("@cuco_sCuentaCorrienteId", SqlDbType.SmallInt);
                        intCuentaCorrienteId.Direction = ParameterDirection.Output;
                        SqlParameter strMensaje = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 200);
                        strMensaje.Direction = ParameterDirection.Output;                        

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        pobjBE.Error = false;
                        pobjBE.cuco_sCuentaCorrienteId = Convert.ToInt16(intCuentaCorrienteId.Value);
                        pobjBE.Message = Convert.ToString(strMensaje.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                pobjBE.Error = true;
                pobjBE.Message = ex.StackTrace.ToString();                
            }

            return pobjBE;
        }

        public CO_CUENTACORRIENTE Update(CO_CUENTACORRIENTE pobjBE)
        {
            pobjBE.Message = string.Empty;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sCuentaCorrienteId", pobjBE.cuco_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", pobjBE.cuco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sTipoId", pobjBE.cuco_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sBancoId", pobjBE.cuco_sBancoId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sMonedaId", pobjBE.cuco_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vNumero", pobjBE.cuco_vNumero));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vRepresentante", pobjBE.cuco_vRepresentante));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vSucursal", pobjBE.cuco_vSucursal));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sSituacionId", pobjBE.cuco_sSituacionId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vObservacion", pobjBE.cuco_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sUsuarioModificacion", pobjBE.cuco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vIPModificacion", pobjBE.cuco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@cuco_cEstado", pobjBE.cuco_cEstado));

                        SqlParameter strMensaje = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 200);
                        strMensaje.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        pobjBE.Message = strMensaje.Value.ToString();
                    }                    
                }
            }
            catch (Exception ex)
            {
                pobjBE.Error = true;
                pobjBE.Message = ex.StackTrace.ToString();
            }

            return pobjBE;
        }

        public int Delete(CO_CUENTACORRIENTE pobjBE)
        {
            pobjBE.Message = string.Empty;
            int intResultado = -1;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_CUENTACORRIENTE_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cuco_sCuentaCorrienteId", pobjBE.cuco_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sOficinaConsularId", pobjBE.cuco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@cuco_sUsuarioModificacion", pobjBE.cuco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vIPModificacion", pobjBE.cuco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@cuco_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = 1;
                    }                    
                }
            }
            catch (SqlException ex)
            {
                intResultado = -1;
                pobjBE.Message = ex.StackTrace.ToString();
            }

            return intResultado;
        }
    }
}
