using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class TipoActoProtocolarTarifarioMantenimientoDA
    {
        ~TipoActoProtocolarTarifarioMantenimientoDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO InsertarTipoActoProtocolarTarifario(SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifario)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO_ADICIONAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_acta_sTipoActoProtocolarId", objTipoActoProtocolarTarifario.acta_sTipoActoProtocolarId));
                        cmd.Parameters.Add(new SqlParameter("@P_acta_sTarifarioId", objTipoActoProtocolarTarifario.acta_sTarifarioId));

                        cmd.Parameters.Add(new SqlParameter("@P_acta_sUsuarioCreacion", objTipoActoProtocolarTarifario.acta_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_acta_vIPCreacion", objTipoActoProtocolarTarifario.acta_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_sOficinaConsularId", objTipoActoProtocolarTarifario.OficinaConsultar));


                        SqlParameter lReturn = cmd.Parameters.Add("@P_acta_iTipoActoProtocolarTarifarioId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objTipoActoProtocolarTarifario.acta_iTipoActoProtocolarTarifarioId = Convert.ToInt16(lReturn.Value);
                        objTipoActoProtocolarTarifario.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objTipoActoProtocolarTarifario.Error = true;
                objTipoActoProtocolarTarifario.Message = ex.Message.ToString();
            }
            return objTipoActoProtocolarTarifario;
        }

        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO AnularTipoActoProtocolarTarifario(SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifario)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO_ANULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_acta_iTipoActoProtocolarTarifarioId", objTipoActoProtocolarTarifario.acta_iTipoActoProtocolarTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@P_acta_sUsuarioModificacion", objTipoActoProtocolarTarifario.acta_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_acta_vIPModificacion", objTipoActoProtocolarTarifario.acta_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_sOficinaConsularId", objTipoActoProtocolarTarifario.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objTipoActoProtocolarTarifario.Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                objTipoActoProtocolarTarifario.Error = true;
                objTipoActoProtocolarTarifario.Message = ex.Message.ToString();
            }
            return objTipoActoProtocolarTarifario;
        }

        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO AnularTipoActoProtocolarTarifarioTodos(SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO objTipoActoProtocolarTarifario)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO_ANULAR_TODOS_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_acta_sTipoActoProtocolarId", objTipoActoProtocolarTarifario.acta_sTipoActoProtocolarId));
                        cmd.Parameters.Add(new SqlParameter("@P_acta_sUsuarioModificacion", objTipoActoProtocolarTarifario.acta_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_acta_vIPModificacion", objTipoActoProtocolarTarifario.acta_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_sOficinaConsularId", objTipoActoProtocolarTarifario.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objTipoActoProtocolarTarifario.Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                objTipoActoProtocolarTarifario.Error = true;
                objTipoActoProtocolarTarifario.Message = ex.Message.ToString();
            }
            return objTipoActoProtocolarTarifario;
        }

    }
}
