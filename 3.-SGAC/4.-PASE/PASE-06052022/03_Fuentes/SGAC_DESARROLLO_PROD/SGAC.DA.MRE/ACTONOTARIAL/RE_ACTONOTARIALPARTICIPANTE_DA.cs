using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;

namespace SGAC.DA.MRE.ACTONOTARIAL
{    
    public class RE_ACTONOTARIALPARTICIPANTE_DA
    {
        public string strError = string.Empty;

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE obtener(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante) {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (participante.anpa_iActoNotarialParticipanteId != 0) cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialParticipanteId", participante.anpa_iActoNotarialParticipanteId));
	                    if (participante.anpa_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialId", participante.anpa_iActoNotarialId));
	                    if (participante.anpa_iPersonaId != 0) cmd.Parameters.Add(new SqlParameter("@anpa_iPersonaId", participante.anpa_iPersonaId));
	                    if (participante.anpa_iEmpresaId != 0) cmd.Parameters.Add(new SqlParameter("@anpa_iEmpresaId", participante.anpa_iEmpresaId));
                        if (participante.anpa_sTipoParticipanteId != 0) cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", participante.anpa_sTipoParticipanteId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)participante.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(participante, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }
                        participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message.ToString();
                participante.Error = true;
                participante.Message = exec.Message.ToString();
            }
            return participante;        
        }

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE actualizarFechaSuscripcion(BE.MRE.Custom.CBE_PARTICIPANTE participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_ACTUALIZAR_DFECHASUSCRIPCION_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialParticipanteId", participante.anpa_iActoNotarialParticipanteId));

                        if (participante.anpa_dFechaSuscripcion != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_dFechaSuscripcion", participante.anpa_dFechaSuscripcion));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_dFechaSuscripcion", null));
                        }

                        cmd.Parameters.Add(new SqlParameter("@anpa_sUsuarioModificacion", participante.anpa_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@anpa_vIPModificacion", SGAC.Accesorios.Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", participante.OficinaConsultar));

                        if (participante.HostName != null && participante.HostName != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@vHostName", participante.HostName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));


                        #endregion
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message.ToString();
                participante.Error = true;
                participante.Message = exec.Message.ToString();
            }
            return participante;
        }


        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE actualizar(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
	                    cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialParticipanteId", participante.anpa_iActoNotarialParticipanteId));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialId", participante.anpa_iActoNotarialId));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_iPersonaId", participante.anpa_iPersonaId));
                        if (participante.anpa_iEmpresaId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_iEmpresaId", participante.anpa_iEmpresaId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@anpa_iReferenciaParticipanteId", participante.anpa_iReferenciaParticipanteId));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", participante.anpa_sTipoParticipanteId));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_bFlagFirma", participante.anpa_bFlagFirma));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_bFlagHuella", participante.anpa_bFlagHuella));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_cEstado", participante.anpa_cEstado));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_sUsuarioModificacion", participante.anpa_sUsuarioModificacion));
	                    cmd.Parameters.Add(new SqlParameter("@anpa_vIPModificacion", participante.anpa_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@anpa_dFechaModificacion", participante.anpa_dFechaModificacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", 1));

                        if (participante.HostName != null && participante.HostName != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@vHostName", participante.HostName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        if (participante.anpa_vNumeroEscrituraPublica != null && participante.anpa_vNumeroEscrituraPublica != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroEscrituraPublica", participante.anpa_vNumeroEscrituraPublica));
                        else
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroEscrituraPublica", DBNull.Value));

                        if (participante.anpa_vNumeroPartida != null && participante.anpa_vNumeroPartida != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroPartida", participante.anpa_vNumeroPartida));
                        else
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroPartida", DBNull.Value));

                        #endregion
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message.ToString();
                participante.Error = true;
                participante.Message = exec.Message.ToString();
                throw exec;
            }
            return participante;
        }

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE insertar(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante) {
            try 
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion())) 
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_ADICIONAR",cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

		                cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialId", participante.anpa_iActoNotarialId));
                        if (participante.anpa_iPersonaId !=0) cmd.Parameters.Add(new SqlParameter("@anpa_iPersonaId", participante.anpa_iPersonaId));
                        if (participante.anpa_iEmpresaId !=0) cmd.Parameters.Add(new SqlParameter("@anpa_iEmpresaId", participante.anpa_iEmpresaId));
		                cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", participante.anpa_sTipoParticipanteId));
		                cmd.Parameters.Add(new SqlParameter("@anpa_bFlagFirma", participante.anpa_bFlagFirma));
		                cmd.Parameters.Add(new SqlParameter("@anpa_bFlagHuella", participante.anpa_bFlagHuella));
		                cmd.Parameters.Add(new SqlParameter("@anpa_cEstado", participante.anpa_cEstado));

                        if (participante.anpa_iReferenciaParticipanteId.HasValue)
                            cmd.Parameters.Add(new SqlParameter("@anpa_iReferenciaParticipanteId", participante.anpa_iReferenciaParticipanteId));
                        else
                            cmd.Parameters.Add(new SqlParameter("@anpa_iReferenciaParticipanteId", DBNull.Value));

		                cmd.Parameters.Add(new SqlParameter("@anpa_sUsuarioCreacion", participante.anpa_sUsuarioCreacion));
		                cmd.Parameters.Add(new SqlParameter("@anpa_vIPCreacion", participante.anpa_vIPCreacion));

                        if (participante.anpa_dFechaCreacion != DateTime.MinValue)
		                    cmd.Parameters.Add(new SqlParameter("@anpa_dFechaCreacion", participante.anpa_dFechaCreacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", participante.OficinaConsultar));

                        if (participante.HostName != null && participante.HostName != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@vHostName", participante.HostName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        if (participante.anpa_vNumeroEscrituraPublica != null && participante.anpa_vNumeroEscrituraPublica != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroEscrituraPublica", participante.anpa_vNumeroEscrituraPublica));
                        else
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroEscrituraPublica", DBNull.Value));


                        if (participante.anpa_vNumeroPartida != null && participante.anpa_vNumeroPartida != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroPartida", participante.anpa_vNumeroPartida));
                        else
                            cmd.Parameters.Add(new SqlParameter("@anpa_vNumeroPartida", DBNull.Value));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@anpa_iActoNotarialParticipanteId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        participante.anpa_iActoNotarialParticipanteId = Convert.ToInt64(lReturn.Value);
                        participante.Error = false;
                    }
                }
            }
            catch (SqlException exec) 
            {
                strError = exec.Message.ToString();
                participante.Error = true;
                participante.Message = exec.Message.ToString();
                throw exec;
            }
            return participante;
        }

        public List<BE.MRE.RE_ACTONOTARIALPARTICIPANTE> listado(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante) {
            List<BE.MRE.RE_ACTONOTARIALPARTICIPANTE> lResult = new List<RE_ACTONOTARIALPARTICIPANTE>();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion())){
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_OBTENER", cnx)){
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (participante.anpa_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialId", participante.anpa_iActoNotarialId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader()){
                            while (loReader.Read()){
                                BE.MRE.RE_ACTONOTARIALPARTICIPANTE Item = new BE.MRE.RE_ACTONOTARIALPARTICIPANTE();
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull"){
                                        PropertyInfo pInfo = (PropertyInfo)Item.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) { pInfo.SetValue(Item, loReader[col], null); }
                                    }
                                }
                                lResult.Add(Item);
                            }
                        }
                        participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message.ToString();
                participante.Error = true;
                participante.Message = exec.Message.ToString();
            }
            
            return lResult;
        }
        public List<CBE_PRESENTANTE> listaPresentante(CBE_PRESENTANTE participante)
        {
            List<CBE_PRESENTANTE> lResult = new List<CBE_PRESENTANTE>();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_PRESENTANTE_LISTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (participante.anpr_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@anpr_iActoNotarialId", participante.anpr_iActoNotarialId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                CBE_PRESENTANTE b = new CBE_PRESENTANTE();
                                b.anpr_iActoNotarialPresentanteId = Convert.ToInt64(loReader["anpr_iActoNotarialPresentanteId"]);
                                b.anpr_iActoNotarialId = Convert.ToInt64(loReader["anpr_iActoNotarialId"]);
                                b.anpr_sTipoPresentante = Convert.ToInt16(loReader["anpr_sTipoPresentante"]);
                                b.anpr_sTipoPresentante_desc = Convert.ToString(loReader["anpr_sTipoPresentante_desc"]);
                                b.anpr_vPresentanteNombre = Convert.ToString(loReader["anpr_vPresentanteNombre"]);
                                b.anpr_sPresentanteTipoDocumento = Convert.ToInt16(loReader["anpr_sPresentanteTipoDocumento"]);
                                b.anpr_vPresentanteTipoDocumento_desc = Convert.ToString(loReader["anpr_vPresentanteTipoDocumento_desc"]);
                                b.anpr_vPresentanteNumeroDocumento = Convert.ToString(loReader["anpr_vPresentanteNumeroDocumento"]);
                                b.anpr_sPresentanteGenero = Convert.ToInt16(loReader["anpr_sPresentanteGenero"]);
                                b.anpr_vPresentanteGenero_desc = Convert.ToString(loReader["anpr_vPresentanteGenero_desc"]);  

                                lResult.Add(b);
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message.ToString();
            }

            return lResult;
        }

        public DataTable ObtenerParticipantesExtraprotocolar(Int64 anpa_iActoNotarialId)
        {
            DataTable dtResult = new DataTable();
            try
            {

                String StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_OBTENER", cnn))
                    {

                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@anpa_iActoNotarialId", SqlDbType.BigInt).Value = anpa_iActoNotarialId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable VerificarRegistroParticipantesExtraprotocolar(Int64 anpa_iActoNotarialId, Int16 acno_sTipoActoNotarialId, Int64 actu_iPersonaRecurrenteId, Int16 acno_sSubTipoActoNotarialId)
        {
            DataTable dtResult = new DataTable();
            try
            {

                String StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_VERIFICAR", cnn))
                    {

                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@anpa_iActoNotarialId", SqlDbType.BigInt).Value = anpa_iActoNotarialId;
                        cmd.Parameters.Add("@acno_sTipoActoNotarialId", SqlDbType.BigInt).Value = acno_sTipoActoNotarialId;
                        cmd.Parameters.Add("@actu_iPersonaRecurrenteId", SqlDbType.BigInt).Value = actu_iPersonaRecurrenteId;
                        cmd.Parameters.Add("@acno_sSubTipoActoNotarialId", SqlDbType.BigInt).Value = acno_sSubTipoActoNotarialId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------
        //Fecha: 18/10/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Anular el participante
        //------------------------------------------------

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE AnularParticipante(RE_ACTONOTARIALPARTICIPANTE participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialParticipanteId", participante.anpa_iActoNotarialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialId", participante.anpa_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@anpa_iPersonaId", participante.anpa_iPersonaId));

                        cmd.Parameters.Add(new SqlParameter("@anpa_cEstado", participante.anpa_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@anpa_sUsuarioModificacion", participante.anpa_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@anpa_vIPModificacion", participante.anpa_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@anpa_dFechaModificacion", participante.anpa_dFechaModificacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", participante.OficinaConsultar));

                        if (participante.HostName != null && participante.HostName != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@vHostName", participante.HostName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        #endregion
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message.ToString();
                participante.Error = true;
                participante.Message = exec.Message.ToString();
                throw exec;
            }
            return participante;
        }
        public Int64 AnularPresentante(CBE_PRESENTANTE presentante)
        {
            Int64 result = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPRESENTANTE_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                         #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@anpr_iActoNotarialPresentanteId", presentante.anpr_iActoNotarialPresentanteId));
                        cmd.Parameters.Add(new SqlParameter("@anpr_iActoNotarialId", presentante.anpr_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sTipoPresentante", "0"));
                        cmd.Parameters.Add(new SqlParameter("@anpr_vPresentanteNombre", "0"));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sPresentanteTipoDocumento", "0"));
                        cmd.Parameters.Add(new SqlParameter("@anpr_vPresentanteNumeroDocumento", "0"));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sPresentanteGenero", "0"));
                        cmd.Parameters.Add(new SqlParameter("@anpr_cEtsado", "0"));
                        cmd.Parameters.Add(new SqlParameter("@anpr_sUsuario", presentante.anpr_sUsuario));
                        cmd.Parameters.Add(new SqlParameter("@anpr_vIP", presentante.anpr_vIP));
                        cmd.Parameters.Add(new SqlParameter("@operacion", "DEL"));
                        SqlParameter lReturn = cmd.Parameters.Add("@RESULT", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result = (Int64)cmd.Parameters["@RESULT"].Value;
                    }
                }
            }
            catch (SqlException exec)
            {
                result=-1;
                throw exec;
            }
            return result;
        }


        //------------------------------------------------
        //Fecha: 03/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Consultar el idioma diferente al castellano
        //          del primer participante.
        //------------------------------------------------
        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE ObtenerIdiomaPrimerParticipanteOtorgante(Int64 iActoNotarialId)
        {
            BE.MRE.RE_ACTONOTARIALPARTICIPANTE ParticipanteBE = new RE_ACTONOTARIALPARTICIPANTE();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_CONSULTAR_OTORGANTE_IDIOMA_MRE", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_anpa_iActoNotarialId", iActoNotarialId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                ParticipanteBE.anpa_iPersonaId = Convert.ToInt64(loReader["anpa_iPersonaId"].ToString());
                                ParticipanteBE.anpa_iActoNotarialParticipanteId = Convert.ToInt16(loReader["anpa_sTipoParticipanteId"].ToString());
                                ParticipanteBE.TipoParticipante = loReader["TipoParticipante"].ToString();
                                ParticipanteBE.IdiomaId = Convert.ToInt16(loReader["IdiomaId"].ToString());
                                ParticipanteBE.Idioma = loReader["Idioma"].ToString();
                            }
                        }

                        ParticipanteBE.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ParticipanteBE.Error = true;
                ParticipanteBE.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ParticipanteBE.Error = true;
                ParticipanteBE.Message = exec.Message.ToString();
            }
            return ParticipanteBE;
        }


        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
