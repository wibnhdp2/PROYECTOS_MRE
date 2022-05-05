using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
    public class RE_ACTONOTARIALDETALLE_DA
    {
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public BE.MRE.RE_ACTONOTARIALDETALLE insertar(BE.MRE.RE_ACTONOTARIALDETALLE ActoNotarialDetalle, Int64 intActuacionId)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDETALLE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ande_iActuacionId", intActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@ande_iActuacionDetalleId", ActoNotarialDetalle.ande_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@ande_sOficinaConsularId", ActoNotarialDetalle.ande_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ande_sTipoFormatoId", ActoNotarialDetalle.ande_sTipoFormatoId));

                        if (ActoNotarialDetalle.ande_IFuncionarioAutorizadorId == null)
                            ActoNotarialDetalle.ande_IFuncionarioAutorizadorId = 0;
                        cmd.Parameters.Add(new SqlParameter("@ande_IFuncionarioAutorizadorId", ActoNotarialDetalle.ande_IFuncionarioAutorizadorId));
                        if (ActoNotarialDetalle.ande_vNumeroOficio == null)
                            ActoNotarialDetalle.ande_vNumeroOficio = "";
                        cmd.Parameters.Add(new SqlParameter("@ande_vNumeroOficio", ActoNotarialDetalle.ande_vNumeroOficio));
                        cmd.Parameters.Add(new SqlParameter("@ande_sNumeroFoja", ActoNotarialDetalle.ande_sNumeroFoja));
                        cmd.Parameters.Add(new SqlParameter("@ande_vPresentanteNombre", ActoNotarialDetalle.ande_vPresentanteNombre));
                        cmd.Parameters.Add(new SqlParameter("@ande_sPresentanteTipoDocumento", ActoNotarialDetalle.ande_sPresentanteTipoDocumento));
                        cmd.Parameters.Add(new SqlParameter("@ande_vPresentanteNumeroDocumento", ActoNotarialDetalle.ande_vPresentanteNumeroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@ande_sPresentanteGenero", ActoNotarialDetalle.ande_sPresentanteGenero));
                        if (ActoNotarialDetalle.ande_dFechaExtension != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@ande_dFechaExtension", ActoNotarialDetalle.ande_dFechaExtension));
                        cmd.Parameters.Add(new SqlParameter("@ande_sUsuarioCreacion", ActoNotarialDetalle.ande_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ande_vIPCreacion", ActoNotarialDetalle.ande_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ande_iActoNotarialDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialDetalle.ande_iActoNotarialDetalleId = Convert.ToInt64(lReturn.Value);
                        ActoNotarialDetalle.Error = false;
                    }
                }
            }
            catch(Exception ex)
            {
                ActoNotarialDetalle.Error = true;
                ActoNotarialDetalle.Message = ex.Message;
            }

            return ActoNotarialDetalle;
        }

        public BE.MRE.RE_ACTONOTARIALDETALLE actualizar(BE.MRE.RE_ACTONOTARIALDETALLE ActoNotarialDetalle)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDETALLE_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (ActoNotarialDetalle.ande_sPresentanteTipoDocumento != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ande_vPresentanteNombre", ActoNotarialDetalle.ande_vPresentanteNombre));
                            cmd.Parameters.Add(new SqlParameter("@ande_sPresentanteTipoDocumento", ActoNotarialDetalle.ande_sPresentanteTipoDocumento));
                            cmd.Parameters.Add(new SqlParameter("@ande_vPresentanteNumeroDocumento", ActoNotarialDetalle.ande_vPresentanteNumeroDocumento));
                            cmd.Parameters.Add(new SqlParameter("@ande_sPresentanteGenero", ActoNotarialDetalle.ande_sPresentanteGenero));
                            cmd.Parameters.Add(new SqlParameter("@ande_IFuncionarioAutorizadorId", ActoNotarialDetalle.ande_IFuncionarioAutorizadorId));
                        }
                        
                        cmd.Parameters.Add(new SqlParameter("@ande_iActoNotarialDetalleId", ActoNotarialDetalle.ande_iActoNotarialDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@ande_sOficinaConsularId", ActoNotarialDetalle.ande_sOficinaConsularId));                        

                        if (ActoNotarialDetalle.ande_vNumeroOficio == null)
                            ActoNotarialDetalle.ande_vNumeroOficio = "";

                        cmd.Parameters.Add(new SqlParameter("@ande_vNumeroOficio", ActoNotarialDetalle.ande_vNumeroOficio));
                        cmd.Parameters.Add(new SqlParameter("@ande_sUsuarioModificacion", ActoNotarialDetalle.ande_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ande_vIPModificacion", ActoNotarialDetalle.ande_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialDetalle.Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ActoNotarialDetalle.Error = true;
                ActoNotarialDetalle.Message = ex.Message;
            }

            return ActoNotarialDetalle;
        }
    }
}
