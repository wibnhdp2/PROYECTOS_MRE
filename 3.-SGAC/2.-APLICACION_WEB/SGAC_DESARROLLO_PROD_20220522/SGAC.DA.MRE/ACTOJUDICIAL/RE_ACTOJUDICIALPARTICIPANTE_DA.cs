using System;
using System.Configuration;
using System.Data;
using SGAC.Accesorios;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SGAC.DA.MRE.ACTOJUDICIAL
{
    public class RE_ACTOJUDICIALPARTICIPANTE_DA
    {
        public string strError = "";

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public Boolean eliminar_actojudicialparticipante(int intParticipanteId)
        {
            bool booResult = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_ELIMINAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialParticipanteId", intParticipanteId));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        booResult = true;

                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                booResult = false;
            }
            return booResult;
        }

        public SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE Insertar(ref SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE Participante)
        {
            bool booResult = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        #region Creando Parametros

                        SqlParameter lReturn = cmd.Parameters.Add("@ajpa_iActoJudicialParticipanteId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                                                
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialId", Participante.ajpa_iActoJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sTipoParticipanteId", Participante.ajpa_sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sOficinaConsularDestinoId", Participante.ajpa_sOficinaConsularDestinoId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sTipoPersonaId", Participante.ajpa_sTipoPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iPersonaId", Participante.ajpa_iPersonaId));

                        cmd.Parameters.Add(new SqlParameter("@ajpa_iEmpresaId", Participante.ajpa_iEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaAceptacionExpediente", Participante.ajpa_dFechaAceptacionExpediente));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sEstadoId", Participante.ajpa_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sUsuarioCreacion", Participante.ajpa_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_vIPCreacion", Participante.ajpa_vIPCreacion));

                        cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaCreacion", Participante.ajpa_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActuacionDetalleId", Participante.ajpa_iActuacionDetalleId));

                        //------------------------------------------------------------------
                        // Autor: Miguel Márquez Beltrán
                        // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                        //           y la fecha de salida de valija
                        // Fecha: 13/01/2017
                        //------------------------------------------------------------------

                        if (Participante.ajpa_dFechaLlegadaValija != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaLlegadaValija", Participante.ajpa_dFechaLlegadaValija));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaLlegadaValija", DBNull.Value));
                        }
                        //------------------------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", Participante.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Participante.HostName));

                        if (Participante.ajpa_vNumeroHojaRemision != null)
                            cmd.Parameters.Add(new SqlParameter("@ajpa_vNumeroHojaRemision", Participante.ajpa_vNumeroHojaRemision));
                        else
                            cmd.Parameters.Add(new SqlParameter("@ajpa_vNumeroHojaRemision", DBNull.Value));
                                                	
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        booResult = true;
                        Participante.ajpa_iActoJudicialParticipanteId = Convert.ToInt64(lReturn.Value);
                        Participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                booResult = false;

                Participante.Error = true;
                Participante.Message = exec.Message.ToString();
            }
            return Participante;
        }
        
        public bool Actualizar(ref SGAC.BE.MRE.RE_ACTOJUDICIALPARTICIPANTE Participante)
        {
            bool booResult = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                                                
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialParticipanteId", Participante.ajpa_iActoJudicialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialId", Participante.ajpa_iActoJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sTipoParticipanteId", Participante.ajpa_sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sOficinaConsularDestinoId", Participante.ajpa_sOficinaConsularDestinoId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sTipoPersonaId", Participante.ajpa_sTipoPersonaId));
                        
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iPersonaId", Participante.ajpa_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iEmpresaId", Participante.ajpa_iEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaAceptacionExpediente", Participante.ajpa_dFechaAceptacionExpediente));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sEstadoId", Participante.ajpa_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sUsuarioModificacion", Participante.ajpa_sUsuarioModificacion));

                        cmd.Parameters.Add(new SqlParameter("@ajpa_vIPModificacion", Participante.ajpa_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaModificacion", Participante.ajpa_dFechaModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActuacionDetalleId", Participante.ajpa_iActuacionDetalleId));
                        //------------------------------------------------------------------
                        // Autor: Miguel Márquez Beltrán
                        // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                        //           y la fecha de salida de valija
                        // Fecha: 13/01/2017
                        //------------------------------------------------------------------
                        if (Participante.ajpa_dFechaLlegadaValija != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaLlegadaValija", Participante.ajpa_dFechaLlegadaValija));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajpa_dFechaLlegadaValija", DBNull.Value));
                        }
                        //------------------------------------------------------------------
                        
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", Participante.OficinaConsultar));
                        
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Participante.HostName));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_vNumeroHojaRemision", Participante.ajpa_vNumeroHojaRemision.ToUpper()));
                        
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        booResult = true;
                        Participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                booResult = false;
                
                Participante.Error = true;
                Participante.Message = exec.Message.ToString();

            }
            return booResult;
        }
                
        public DataTable Obtener(Int64 iActoJudicialId, Int16 sTipoParticipanteId, Int16? IntOficinaConsular)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialId", iActoJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sTipoParticipanteId", sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sOficinaConsularDestinoId", IntOficinaConsular));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

            return DtResult;
        }

        public bool ActualizarEstadoParticipante(Int64 iActoJudicialParticipanteId, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        {
            bool booResult = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_ACTUALIZAR_ESTADO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialParticipanteId", iActoJudicialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sEstadoId", sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sUsuarioModificacion", sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_vIPModificacion", vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", vHostName));
        
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        booResult = true;
                    }
                }
            }
            catch (SqlException exec)
            {

                booResult = false;
                strError = exec.Message.ToString();

            }
            return booResult;
        }
        
        public bool ActualizarEstadoActa(Int64 iActoJudicialParticipanteId, bool sEstado, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        {
            bool booResult = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_ACTUALIZAR_ESTADO_ACTA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialParticipanteId", iActoJudicialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_bActaFlag", sEstado));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_sUsuarioModificacion", sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajpa_vIPModificacion", vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", vHostName));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        booResult = true;
                    }
                }
            }
            catch (SqlException exec)
            {
                booResult = false;
                strError = exec.Message.ToString();

            }
            return booResult;
        }

        public bool ParticipanteActasCerradas(Int64 intActoJudicialid)
        {
            bool booResult = false;
            DataTable DtResult = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_CONSULTAR_ESTADO_ACTA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@iActoJudicialId", intActoJudicialid));
                        cmd.Connection.Open();
                        	         
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];

                        if (DtResult.Rows.Count == 0)           // SI NO HAY REGISTROS QUIERE DECIR QUE TODOS LAS ACTAS DE LOS PARTICIPANTES FUERON CERRADAS
                        {
                            booResult = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }
            return booResult;
        }
    }
}
