using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SUNARP.BE;

namespace SUNARP.Registro.Inscripcion.DA
{
    public class SolicitudSeguimientoMantenimientoDA
    {
        public SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO insertar(SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO seguimiento)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_SISE_ISOLICITUDINSCRIPCIONID", seguimiento.sise_iSolicitudInscripcionId));
                        cmd.Parameters.Add(new SqlParameter("@P_SISE_SESTADOID", seguimiento.sise_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@P_SISE_VOBSERVACION", seguimiento.sise_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SISE_SUSUARIO_CREACION", seguimiento.sise_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SISE_VIP_CREACION", seguimiento.sise_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", seguimiento.OficinaConsultar));
                        #endregion
                        #region Output
                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_SISE_ISOLICITUDSEGUIMIENTOID", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        seguimiento.sise_iSolicitudSeguimientoId = Convert.ToInt64(lReturn1.Value);
                        seguimiento.Error = false;

                    }
                }
            }
            catch (SqlException exec)
            {
                seguimiento.Error = true;
                seguimiento.Message = exec.Message.ToString();
            }
            return seguimiento;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
