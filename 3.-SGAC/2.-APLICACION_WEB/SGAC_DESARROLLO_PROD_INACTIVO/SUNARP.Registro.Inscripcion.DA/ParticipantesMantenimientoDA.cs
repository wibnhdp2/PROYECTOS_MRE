using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SUNARP.BE;

//-----------------------------------------------------
//Fecha: 06/10/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Clase de acceso a datos de Participantes
//-----------------------------------------------------

namespace SUNARP.Registro.Inscripcion.DA
{
    public class ParticipantesMantenimientoDA
    {
        public SU_PARTICIPANTES insertar(SU_PARTICIPANTES participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_PARTICIPANTES_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_PART_IPERSONAIDENTIFICACIONID", participante.part_iPersonaIdentificacionId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_ISOLICITUDINSCRIPCIONID", participante.part_iSolicitudInscripcionId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_STIPOPARTICIPANTEID", participante.part_sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_SUSUARIO_CREACION", participante.part_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_VIP_CREACION", participante.part_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", participante.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@P_PART_IPARTICIPANTEID", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        participante.part_iParticipanteId = Convert.ToInt64(lReturn.Value);
                        participante.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                participante.Error = true;
                participante.Message = exec.Message.ToString();
            }
            return participante;
        }

        public SU_PARTICIPANTES actualizar(SU_PARTICIPANTES participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_PARTICIPANTES_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_PART_IPARTICIPANTEID", participante.part_iParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_IPERSONAIDENTIFICACIONID", participante.part_iPersonaIdentificacionId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_STIPOPARTICIPANTEID", participante.part_sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_CESTADO", participante.part_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_SUSUARIO_MODIFICACION", participante.part_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_VIP_MODIFICACION", participante.part_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", participante.OficinaConsultar));
                        #endregion
                       

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        participante.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                participante.Error = true;
                participante.Message = exec.Message.ToString();
            }
            return participante;
        }
        
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    

    }
}
