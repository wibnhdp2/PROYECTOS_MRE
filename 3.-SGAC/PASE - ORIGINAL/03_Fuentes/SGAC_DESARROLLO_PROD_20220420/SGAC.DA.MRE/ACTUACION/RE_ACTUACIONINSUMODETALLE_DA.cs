using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Configuration;
using System.Reflection;
using SGAC.Accesorios;

namespace SGAC.DA.MRE.ACTUACION
{
    public class RE_ACTUACIONINSUMODETALLE_DA
    {
        public BE.MRE.RE_ACTUACIONINSUMODETALLE insertar(BE.MRE.RE_ACTUACIONINSUMODETALLE ActuacionInsumoDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@aide_iActuacionDetalleId", ActuacionInsumoDetalle.aide_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@aide_iInsumoId", ActuacionInsumoDetalle.aide_iInsumoId));
                        cmd.Parameters.Add(new SqlParameter("@aide_dFechaVinculacion", ActuacionInsumoDetalle.aide_dFechaVinculacion));
                        cmd.Parameters.Add(new SqlParameter("@aide_sUsuarioVinculacionId", ActuacionInsumoDetalle.aide_sUsuarioVinculacionId));
                        cmd.Parameters.Add(new SqlParameter("@aide_bFlagImpresion", ActuacionInsumoDetalle.aide_bFlagImpresion));
                        cmd.Parameters.Add(new SqlParameter("@aide_dFechaImpresion", ActuacionInsumoDetalle.aide_dFechaImpresion));
                        cmd.Parameters.Add(new SqlParameter("@aide_cEstado", ActuacionInsumoDetalle.aide_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@aide_sUsuarioCreacion", ActuacionInsumoDetalle.aide_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@aide_vIPCreacion", ActuacionInsumoDetalle.aide_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsular", ActuacionInsumoDetalle.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@aide_iActuacionInsumoDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActuacionInsumoDetalle.aide_iActuacionInsumoDetalleId = Convert.ToInt64(lReturn.Value);
                        ActuacionInsumoDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActuacionInsumoDetalle.Error = true;
                ActuacionInsumoDetalle.Message = exec.Message.ToString();
                throw exec;
            }

            return ActuacionInsumoDetalle;
        }

        public RE_ACTUACIONINSUMODETALLE obtener(RE_ACTUACIONINSUMODETALLE ActuacionInsumoDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONINSUMODETALLE_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;                        

                        #region Creando Parametros
                        if (ActuacionInsumoDetalle.aide_iActuacionInsumoDetalleId != 0) cmd.Parameters.Add(new SqlParameter("@aide_iActuacionInsumoDetalleId", ActuacionInsumoDetalle.aide_iActuacionInsumoDetalleId));
                        if (ActuacionInsumoDetalle.aide_iActuacionDetalleId != 0) cmd.Parameters.Add(new SqlParameter("@aide_iActuacionDetalleId", ActuacionInsumoDetalle.aide_iActuacionDetalleId));
                        if (ActuacionInsumoDetalle.aide_iInsumoId != 0) cmd.Parameters.Add(new SqlParameter("@aide_iInsumoId", ActuacionInsumoDetalle.aide_iInsumoId));
                        if (ActuacionInsumoDetalle.aide_dFechaVinculacion != null) cmd.Parameters.Add(new SqlParameter("@aide_dFechaVinculacion", ActuacionInsumoDetalle.aide_dFechaVinculacion));
                        if (ActuacionInsumoDetalle.aide_bFlagImpresion != null) cmd.Parameters.Add(new SqlParameter("@aide_bFlagImpresion", ActuacionInsumoDetalle.aide_bFlagImpresion));
                        if (ActuacionInsumoDetalle.aide_dFechaImpresion != null) cmd.Parameters.Add(new SqlParameter("@aide_dFechaImpresion", ActuacionInsumoDetalle.aide_dFechaImpresion));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)ActuacionInsumoDetalle.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(ActuacionInsumoDetalle, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }

                        ActuacionInsumoDetalle.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActuacionInsumoDetalle.Error = true;
                ActuacionInsumoDetalle.Message = exec.Message.ToString();
            }

            return ActuacionInsumoDetalle;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
