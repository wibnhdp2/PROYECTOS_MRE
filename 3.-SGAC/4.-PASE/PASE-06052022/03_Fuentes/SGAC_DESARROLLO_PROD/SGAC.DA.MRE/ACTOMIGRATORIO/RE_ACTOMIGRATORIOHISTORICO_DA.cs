using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SGAC.DA.MRE.ACTOMIGRATORIO
{
    public class RE_ACTOMIGRATORIOHISTORICO_DA
    {
        public RE_ACTOMIGRATORIOHISTORICO insertar(RE_ACTOMIGRATORIOHISTORICO ActoMigratorioHistorico)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIOHISTORICO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@amhi_iActoMigratorioId", ActoMigratorioHistorico.amhi_iActoMigratorioId));
                        if (ActoMigratorioHistorico.amhi_sTipoInsumoId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amhi_sTipoInsumoId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amhi_sTipoInsumoId", ActoMigratorioHistorico.amhi_sTipoInsumoId));

                        if (ActoMigratorioHistorico.amhi_IFuncionarioId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amhi_IFuncionarioId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amhi_IFuncionarioId", ActoMigratorioHistorico.amhi_IFuncionarioId));

                        if (ActoMigratorioHistorico.amhi_sInsumoId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amhi_sInsumoId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amhi_sInsumoId", ActoMigratorioHistorico.amhi_sInsumoId));

                        if (ActoMigratorioHistorico.amhi_sMotivoId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amhi_sMotivoId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amhi_sMotivoId", ActoMigratorioHistorico.amhi_sMotivoId));
                        cmd.Parameters.Add(new SqlParameter("@amhi_dFechaRegistro", ActoMigratorioHistorico.amhi_dFechaRegistro));
                        cmd.Parameters.Add(new SqlParameter("@amhi_vObservaciones", ActoMigratorioHistorico.amhi_vObservaciones));

                        cmd.Parameters.Add(new SqlParameter("@amhi_sEstadoId", ActoMigratorioHistorico.amhi_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@amhi_sUsuarioCreacion", ActoMigratorioHistorico.amhi_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@amhi_vIPCreacion", ActoMigratorioHistorico.amhi_vIPCreacion));

                        cmd.Parameters.Add(new SqlParameter("@amhi_sOficinaConsularId", ActoMigratorioHistorico.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@amhi_vHostName", ActoMigratorioHistorico.HostName));
                        

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@amhi_iActoMigratorioHistoricoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorioHistorico.amhi_iActoMigratorioHistoricoId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorioHistorico.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorioHistorico.Error = true;
                ActoMigratorioHistorico.Message = exec.Message.ToString();
            }
            return ActoMigratorioHistorico;
        }

        public int actualizar(RE_ACTOMIGRATORIOHISTORICO ActoMigratorioHistorico)
        {
            return 0;
        }

        public RE_ACTOMIGRATORIOHISTORICO obtener(RE_ACTOMIGRATORIOHISTORICO ActoMigratorioHistorico)
        {
            return null;
        }

        public List<RE_ACTOMIGRATORIOHISTORICO> paginado(RE_ACTOMIGRATORIOHISTORICO ActoMigratorioHistorico)
        {
            return null;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
