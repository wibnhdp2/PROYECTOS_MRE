using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SGAC.BE.MRE;
using System.Data.SqlClient;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.CuentaCorriente.DA
{
    public class CajaChicaMantenimientoDA
    {
        public string ErrMessage { get; set; }

        ~CajaChicaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public CO_MOVIMIENTOCAJACHICA Insert(CO_MOVIMIENTOCAJACHICA pobjBE)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_MOVIMIENTOCAJACHICA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@moca_sTipoMovimientoId", pobjBE.moca_sTipoMovimientoId));
                        cmd.Parameters.Add(new SqlParameter("@moca_sOficinaConsularId", pobjBE.moca_sOficinaConsularId));

                        if(pobjBE.moca_vNumeroOperacion != null)
                            cmd.Parameters.Add(new SqlParameter("@moca_vNumeroOperacion", pobjBE.moca_vNumeroOperacion));
                        else
                            cmd.Parameters.Add(new SqlParameter("@moca_vNumeroOperacion", DBNull.Value));

                        if (pobjBE.moca_vNumeroComprobante != null)
                            cmd.Parameters.Add(new SqlParameter("@moca_vNumeroComprobante", pobjBE.moca_vNumeroComprobante));
                        else
                            cmd.Parameters.Add(new SqlParameter("@moca_vNumeroComprobante", DBNull.Value));

                        if (pobjBE.moca_sBancoId.HasValue)
                            cmd.Parameters.Add(new SqlParameter("@moca_sBancoId", pobjBE.moca_sBancoId));
                        else
                            cmd.Parameters.Add(new SqlParameter("@moca_sBancoId", DBNull.Value));

                        if (pobjBE.moca_fMontoOperacion != null)
                            cmd.Parameters.Add(new SqlParameter("@moca_fMontoOperacion", pobjBE.moca_fMontoOperacion));
                        else
                            cmd.Parameters.Add(new SqlParameter("@moca_fMontoOperacion", DBNull.Value));
                        
                        cmd.Parameters.Add(new SqlParameter("@moca_fMonto", pobjBE.moca_fMonto));
                        cmd.Parameters.Add(new SqlParameter("@moca_dFechaRegistro", pobjBE.moca_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@moca_sUsuarioCreacion", pobjBE.moca_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@moca_vIPCreacion", pobjBE.moca_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@moca_vHostName", Util.ObtenerHostName()));

                        SqlParameter iMovimientoCajaChicaIdReturn = cmd.Parameters.Add("@moca_iMovimientoCajaChicaId", SqlDbType.Int);
                        iMovimientoCajaChicaIdReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        pobjBE.Error = false;
                        pobjBE.moca_iMovimientoCajaChicaId = Convert.ToInt16(iMovimientoCajaChicaIdReturn.Value);


                    }
                }
            }
            catch (Exception ex)
            {
                pobjBE.Error = true;
                ErrMessage = ex.StackTrace.ToString();
                throw ex;
            }

            return pobjBE;
        }

        public CO_MOVIMIENTOCAJACHICA Update(CO_MOVIMIENTOCAJACHICA pobjBE)
        {
            return pobjBE;
        }

        public int Delete(CO_MOVIMIENTOCAJACHICA pobjBE)
        {
            return 0;
        }
    }
}
