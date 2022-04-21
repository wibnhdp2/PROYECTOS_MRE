using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
    public class RE_ACTONOTARIALSEGUIMIENTO_DA
    {
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public BE.MRE.RE_ACTONOTARIALSEGUIMIENTO insertar(BE.MRE.RE_ACTONOTARIALSEGUIMIENTO ActoNotarialSeguimiento)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALSEGUIMIENTO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@anse_iActoNotarialId", ActoNotarialSeguimiento.anse_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@anse_sEstadoId", ActoNotarialSeguimiento.anse_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@anse_sUsuarioCreacion", ActoNotarialSeguimiento.anse_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@anse_vIPCreacion", ActoNotarialSeguimiento.anse_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoNotarialSeguimiento.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActoNotarialSeguimiento.HostName));

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@anse_iActoNotarialSeguimientoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarialSeguimiento.anse_iActoNotarialSeguimientoId = Convert.ToInt64(lReturn.Value);
                        ActoNotarialSeguimiento.Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ActoNotarialSeguimiento.Error = true;
                ActoNotarialSeguimiento.Message = ex.Message;
            }
            return ActoNotarialSeguimiento;
        }
    }
}
