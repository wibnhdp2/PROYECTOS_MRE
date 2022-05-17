using System;
using System.Linq;
using System.Text;
using DL.DAC;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SGAC.Accesorios;
using SGAC.BE.MRE;
//---------------------------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase de acceso a datos de la ficha registral de participantes
//---------------------------------------------------------------------------

namespace SGAC.Registro.Actuacion.DA
{

    public class FichaRegistralParticipanteDA
    {

        public DataTable ConsultarDocAdjuntos(long fido_iFichaRegistralID)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALDOCUMENTO_LISTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_fido_iFichaRegistralID", fido_iFichaRegistralID));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }
        public DataTable Consultar(long intFichaRegistralId, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRALPARTICIPANTE_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FIPA_IFICHAREGISTRALID", intFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public void insertar(ref RE_FICHAREGISTRALPARTICIPANTE fichaRegistralParticipante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    long LonResultQuery = 0;
                    long LonPersonaId = 0;

                    if (fichaRegistralParticipante.fipa_iPersonaId == 0)
                    {
                        LonResultQuery = InsertarPersona(ref fichaRegistralParticipante, ref LonPersonaId); 
                    }
                    else
                    {
                        LonResultQuery = ActualizarPersona(ref fichaRegistralParticipante);
                    }
                    if (LonResultQuery > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALPARTICIPANTE_ADICIONAR_MRE", cnx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            #region Creando Parametros
                            cmd.Parameters.Add(new SqlParameter("@fipa_iFichaRegistralId", fichaRegistralParticipante.fipa_iFichaRegistralId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_sTipoParticipanteId", fichaRegistralParticipante.fipa_sTipoParticipanteId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_iPersonaId", fichaRegistralParticipante.fipa_iPersonaId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_sUsuarioCreacion", fichaRegistralParticipante.fipa_sUsuarioCreacion));
                            cmd.Parameters.Add(new SqlParameter("@fipa_vIPCreacion", fichaRegistralParticipante.fipa_vIPCreacion));
                            cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistralParticipante.OficinaConsultar));
                            cmd.Parameters.Add(new SqlParameter("@fipa_bConsignaApellidoMaterno", fichaRegistralParticipante.fipa_bConsignaApellidoMaterno));
                            #endregion

                            #region Output
                            SqlParameter lReturn = cmd.Parameters.Add("@fipa_iFichaRegistralParticipanteId", SqlDbType.BigInt);
                            lReturn.Direction = ParameterDirection.Output;


                            #endregion

                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            fichaRegistralParticipante.fipa_iFichaRegistralParticipanteId = Convert.ToInt64(lReturn.Value);
                            fichaRegistralParticipante.Error = false;
                        }
                    }
                    else
                    {
                        fichaRegistralParticipante.Error = true;
                    }
                }
            }
            catch (SqlException exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.Message.ToString();
            }
        }

        public void actualizar(ref RE_FICHAREGISTRALPARTICIPANTE fichaRegistralParticipante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    long LonResultQuery = 0;
                    long LonPersonaId = 0;

                    if (fichaRegistralParticipante.fipa_iPersonaId == 0)
                    {
                        LonResultQuery = InsertarPersona(ref fichaRegistralParticipante, ref LonPersonaId);
                    }
                    else
                    {
                        LonResultQuery = ActualizarPersona(ref fichaRegistralParticipante);
                    }

                    if (LonResultQuery > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALPARTICIPANTE_ACTUALIZAR_MRE", cnx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            #region Creando Parametros
                            cmd.Parameters.Add(new SqlParameter("@fipa_iFichaRegistralParticipanteId", fichaRegistralParticipante.fipa_iFichaRegistralParticipanteId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_sTipoParticipanteId", fichaRegistralParticipante.fipa_sTipoParticipanteId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_iPersonaId", fichaRegistralParticipante.fipa_iPersonaId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_cEstado", fichaRegistralParticipante.fipa_cEstadoId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_sUsuarioModificacion", fichaRegistralParticipante.fipa_sUsuarioModificacion));
                            cmd.Parameters.Add(new SqlParameter("@fipa_vIPModificacion", fichaRegistralParticipante.fipa_vIPModificacion));
                            cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistralParticipante.OficinaConsultar));
                            cmd.Parameters.Add(new SqlParameter("@fipa_bConsignaApellidoMaterno", fichaRegistralParticipante.fipa_bConsignaApellidoMaterno));
                            #endregion

                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            fichaRegistralParticipante.Error = false;
                        }
                    }
                    else
                    {
                        fichaRegistralParticipante.Error = true;
                    }
                }

            }
            catch (SqlException exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.Message.ToString();
            }
        }

        public void anular(ref RE_FICHAREGISTRALPARTICIPANTE fichaRegistralParticipante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALPARTICIPANTE_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@fipa_iFichaRegistralParticipanteId", fichaRegistralParticipante.fipa_iFichaRegistralParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@fipa_sTipoParticipanteId", fichaRegistralParticipante.fipa_sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@fipa_iPersonaId", fichaRegistralParticipante.fipa_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@fipa_cEstado", fichaRegistralParticipante.fipa_cEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@fipa_sUsuarioModificacion", fichaRegistralParticipante.fipa_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@fipa_vIPModificacion", fichaRegistralParticipante.fipa_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistralParticipante.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@fipa_bConsignaApellidoMaterno", fichaRegistralParticipante.fipa_bConsignaApellidoMaterno));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        fichaRegistralParticipante.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.Message.ToString();
            }
        }


        private Int64 InsertarPersona(ref RE_FICHAREGISTRALPARTICIPANTE fichaRegistralParticipante, ref long LonPersonaId)
        {
            long LonResultQuery;
            int IntNumberRowsDet1OK = 0, IntNumberRowsDet2OK = 0;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[52];

                #region Insertar Persona

                prmParameterHeader[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[0].Direction = ParameterDirection.Output;

                prmParameterHeader[1] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = fichaRegistralParticipante.PERSONA.pers_sPersonaTipoId;

                prmParameterHeader[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[2].Value = fichaRegistralParticipante.PERSONA.pers_vApellidoPaterno;

                prmParameterHeader[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[3].Value = fichaRegistralParticipante.PERSONA.pers_vApellidoMaterno;

                prmParameterHeader[4] = new SqlParameter("@pers_vNombres", SqlDbType.NVarChar, 100);
                prmParameterHeader[4].Value = fichaRegistralParticipante.PERSONA.pers_vNombres;

                /**************Parametros de ingreso para la tabla PN_REGISTRO.PERSONAIDENTIFICACION*****************/
                prmParameterHeader[5] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[5].Value = fichaRegistralParticipante.PERSONA.Identificacion.peid_sDocumentoTipoId;

                prmParameterHeader[6] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[6].Value = fichaRegistralParticipante.PERSONA.Identificacion.peid_vDocumentoNumero;
                /****************************************************************************************************/

                if (fichaRegistralParticipante.PERSONA.pers_vCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = fichaRegistralParticipante.PERSONA.pers_vCorreoElectronico;
                }

                prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);

                if (fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha == null || fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha == DateTime.MinValue)
                {                    
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else if (fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha == DateTime.MinValue)
                {
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else 
                {
                    prmParameterHeader[8].Value = fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha;
                }

                /***************************Parametro del Lugar de nacimiento o Ubigeo*******************************/
                if (fichaRegistralParticipante.PERSONA.pers_cNacimientoLugar == "00")
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = fichaRegistralParticipante.PERSONA.pers_cNacimientoLugar;
                }
                /****************************************************************************************************/

                if (fichaRegistralParticipante.PERSONA.pers_sGeneroId == 0)
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = fichaRegistralParticipante.PERSONA.pers_sGeneroId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vObservaciones.Length == 0)
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = fichaRegistralParticipante.PERSONA.pers_vObservaciones;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sNacionalidadId == 0)
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                    prmParameterHeader[12].Value = fichaRegistralParticipante.PERSONA.pers_sNacionalidadId;
                }
                else
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                    prmParameterHeader[12].Value = fichaRegistralParticipante.PERSONA.pers_sNacionalidadId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = fichaRegistralParticipante.PERSONA.pers_sEstadoCivilId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sGradoInstruccionId == 0)
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = fichaRegistralParticipante.PERSONA.pers_sGradoInstruccionId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sOcupacionId == 0)
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = fichaRegistralParticipante.PERSONA.pers_sOcupacionId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sProfesionId == 0)
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = fichaRegistralParticipante.PERSONA.pers_sProfesionId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vApellidoCasada.Length == 0)
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = fichaRegistralParticipante.PERSONA.pers_vApellidoCasada;
                }

                //*************Parametros de ingreso para la tabla PN_REGISTRO.RE_REGISTROUNICO***********************/
                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaNombre.Length == 0)
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaNombre;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sEmergenciaRelacionId == 0)
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sEmergenciaRelacionId;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionLocal.Length == 0)
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionLocal;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCodigoPostal.Length == 0)
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCodigoPostal;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaTelefono.Length == 0)
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaTelefono;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionPeru.Length == 0)
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionPeru;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCorreoElectronico;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cViveExteriorDesde == "00")
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cViveExteriorDesde;
                }

                prmParameterHeader[26] = new SqlParameter("@reun_bPiensaRetornarAlPeru", SqlDbType.Bit);
                prmParameterHeader[26].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bPiensaRetornarAlPeru;

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cCuandoRetornaAlPeru == "00")
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cCuandoRetornaAlPeru;
                }

                prmParameterHeader[28] = new SqlParameter("@reun_bAfiliadoSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[28].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bAfiliadoSeguroSocial;

                prmParameterHeader[29] = new SqlParameter("@reun_bAfiliadoAFP", SqlDbType.Bit);
                prmParameterHeader[29].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bAfiliadoAFP;

                prmParameterHeader[30] = new SqlParameter("@reun_bAportaSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[30].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bAportaSeguroSocial;

                prmParameterHeader[31] = new SqlParameter("@reun_bBeneficiadoExterior", SqlDbType.Bit);
                prmParameterHeader[31].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bBeneficiadoExterior;

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionPeru == 0)
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionPeru;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionExtranjero == 0)
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionExtranjero;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vNombreConvenio.Length == 0)
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vNombreConvenio;
                }

                /****************************************************************************************************/

                prmParameterHeader[35] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[35].Value = fichaRegistralParticipante.OficinaConsultar;

                prmParameterHeader[36] = new SqlParameter("@pers_bGenera58A", SqlDbType.Bit);
                prmParameterHeader[36].Value = fichaRegistralParticipante.PERSONA.bGenera58A;

                prmParameterHeader[37] = new SqlParameter("@pers_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[37].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioCreacion;

                prmParameterHeader[38] = new SqlParameter("@pers_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[38].Value = fichaRegistralParticipante.PERSONA.pers_vIPCreacion;

                prmParameterHeader[39] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[39].Value = Util.ObtenerHostName();

                prmParameterHeader[40] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.Bit);
                prmParameterHeader[40].Value = fichaRegistralParticipante.PERSONA.pers_bFallecidoFlag;

                prmParameterHeader[41] = new SqlParameter("@pers_vSenasParticulares", SqlDbType.VarChar, 250);
                prmParameterHeader[41].Value = fichaRegistralParticipante.PERSONA.pers_vSenasParticulares;


                prmParameterHeader[42] = new SqlParameter("@pers_spaisid", SqlDbType.SmallInt);

                if (fichaRegistralParticipante.PERSONA.pers_sPaisId == 0)
                {
                    prmParameterHeader[42].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[42].Value = fichaRegistralParticipante.PERSONA.pers_sPaisId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vEstatura.Length == 0)
                {
                    prmParameterHeader[43] = new SqlParameter("@pers_vEstatura", SqlDbType.VarChar, 20);
                    prmParameterHeader[43].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[43] = new SqlParameter("@pers_vEstatura", SqlDbType.VarChar, 20);
                    prmParameterHeader[43].Value = fichaRegistralParticipante.PERSONA.pers_vEstatura;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sAnioEstudio == 0)
                {
                    prmParameterHeader[44] = new SqlParameter("@pers_sAnioEstudio", SqlDbType.SmallInt);
                    prmParameterHeader[44].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[44] = new SqlParameter("@pers_sAnioEstudio", SqlDbType.SmallInt);
                    prmParameterHeader[44].Value = fichaRegistralParticipante.PERSONA.pers_sAnioEstudio;
                }

                if (fichaRegistralParticipante.PERSONA.pers_cEstudioCompleto.Length == 0)
                {
                    prmParameterHeader[45] = new SqlParameter("@pers_cEstudioCompleto", SqlDbType.VarChar, 1);
                    prmParameterHeader[45].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[45] = new SqlParameter("@pers_cEstudioCompleto", SqlDbType.VarChar, 1);
                    prmParameterHeader[45].Value = fichaRegistralParticipante.PERSONA.pers_cEstudioCompleto;
                }


                if (fichaRegistralParticipante.PERSONA.pers_cDiscapacidad.Length == 0)
                {
                    prmParameterHeader[46] = new SqlParameter("@pers_cDiscapacidad", SqlDbType.VarChar, 1);
                    prmParameterHeader[46].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[46] = new SqlParameter("@pers_cDiscapacidad", SqlDbType.VarChar, 1);
                    prmParameterHeader[46].Value = fichaRegistralParticipante.PERSONA.pers_cDiscapacidad;
                }

                if (fichaRegistralParticipante.PERSONA.pers_cInterdicto.Length == 0)
                {
                    prmParameterHeader[47] = new SqlParameter("@pers_cInterdicto", SqlDbType.VarChar, 1);
                    prmParameterHeader[47].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[47] = new SqlParameter("@pers_cInterdicto", SqlDbType.VarChar, 1);
                    prmParameterHeader[47].Value = fichaRegistralParticipante.PERSONA.pers_cInterdicto;
                }

                if (fichaRegistralParticipante.PERSONA.pers_cDonaOrganos.Length == 0)
                {
                    prmParameterHeader[48] = new SqlParameter("@pers_cDonaOrganos", SqlDbType.VarChar, 1);
                    prmParameterHeader[48].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[48] = new SqlParameter("@pers_cDonaOrganos", SqlDbType.VarChar, 1);
                    prmParameterHeader[48].Value = fichaRegistralParticipante.PERSONA.pers_cDonaOrganos;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vNombreCurador.Length == 0)
                {
                    prmParameterHeader[49] = new SqlParameter("@pers_vNombreCurador", SqlDbType.VarChar, 100);
                    prmParameterHeader[49].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[49] = new SqlParameter("@pers_vNombreCurador", SqlDbType.VarChar, 100);
                    prmParameterHeader[49].Value = fichaRegistralParticipante.PERSONA.pers_vNombreCurador;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sTipoDeclarante == 0)
                {
                    prmParameterHeader[50] = new SqlParameter("@pers_sTipoDeclarante", SqlDbType.SmallInt);
                    prmParameterHeader[50].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[50] = new SqlParameter("@pers_sTipoDeclarante", SqlDbType.SmallInt);
                    prmParameterHeader[50].Value = fichaRegistralParticipante.PERSONA.pers_sTipoDeclarante;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sTipoTutorGuardador == 0)
                {
                    prmParameterHeader[51] = new SqlParameter("@pers_sTipoTutorGuardador", SqlDbType.SmallInt);
                    prmParameterHeader[51].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[51] = new SqlParameter("@pers_sTipoTutorGuardador", SqlDbType.SmallInt);
                    prmParameterHeader[51].Value = fichaRegistralParticipante.PERSONA.pers_sTipoTutorGuardador;
                }
                LonResultQuery = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ADICIONAR",
                                                           prmParameterHeader);

                #endregion Insertar Persona

                LonPersonaId = (long)prmParameterHeader[0].Value;

                if (LonPersonaId > 0)
                {
                    fichaRegistralParticipante.fipa_iPersonaId = LonPersonaId;
                    fichaRegistralParticipante.PERSONA.pers_iPersonaId = LonPersonaId;
                    #region Direcciones

                    SqlParameter[] prmParameterDirecciones;

                    prmParameterDirecciones = new SqlParameter[10];

                    prmParameterDirecciones[0] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                    prmParameterDirecciones[0].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_sResidenciaTipoId;

                    prmParameterDirecciones[1] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                    prmParameterDirecciones[1].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vResidenciaDireccion;

                    prmParameterDirecciones[2] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterDirecciones[2].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vCodigoPostal;

                    prmParameterDirecciones[3] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[3].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vResidenciaTelefono;

                    prmParameterDirecciones[4] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                    prmParameterDirecciones[4].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_cResidenciaUbigeo;

                    prmParameterDirecciones[5] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterDirecciones[5].Value = fichaRegistralParticipante.OficinaConsultar;

                    prmParameterDirecciones[6] = new SqlParameter("@resi_sUsuarioCreacion", SqlDbType.SmallInt);
                    prmParameterDirecciones[6].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioCreacion;

                    prmParameterDirecciones[7] = new SqlParameter("@resi_vIPCreacion", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[7].Value = fichaRegistralParticipante.PERSONA.pers_vIPCreacion;

                    prmParameterDirecciones[8] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                    prmParameterDirecciones[8].Value = Util.ObtenerHostName();

                    prmParameterDirecciones[9] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                    prmParameterDirecciones[9].Direction = ParameterDirection.Output;

                    IntNumberRowsDet1OK = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR",
                                                                    prmParameterDirecciones);

                    long LonResidenciaId = (long)prmParameterDirecciones[9].Value;

                    if (LonResidenciaId > 0)
                    {
                        fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_iResidenciaId = LonResidenciaId;

                        SqlParameter[] prmParameterDetail2;

                        prmParameterDetail2 = new SqlParameter[6];

                        prmParameterDetail2[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                        prmParameterDetail2[0].Value = fichaRegistralParticipante.PERSONA.pers_iPersonaId;

                        prmParameterDetail2[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                        prmParameterDetail2[1].Value = LonResidenciaId;

                        prmParameterDetail2[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                        prmParameterDetail2[2].Value = fichaRegistralParticipante.OficinaConsultar; 

                        prmParameterDetail2[3] = new SqlParameter("@pere_sUsuarioCreacion", SqlDbType.SmallInt);
                        prmParameterDetail2[3].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioCreacion;

                        prmParameterDetail2[4] = new SqlParameter("@pere_vIPCreacion", SqlDbType.VarChar, 50);
                        prmParameterDetail2[4].Value = fichaRegistralParticipante.PERSONA.pers_vIPCreacion;

                        prmParameterDetail2[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                        prmParameterDetail2[5].Value = Util.ObtenerHostName();

                        IntNumberRowsDet2OK = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                                        CommandType.StoredProcedure,
                                                                        "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR",
                                                                        prmParameterDetail2);
                    }

                    #endregion Direcciones
                }

                return LonPersonaId;
            }

            catch (SqlException exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.StackTrace.ToString();
                return -1;
            }
            catch (Exception exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.StackTrace.ToString();
                return -1;
            }
                              
        }

        private int ActualizarPersona(ref RE_FICHAREGISTRALPARTICIPANTE fichaRegistralParticipante)
        {
            int IntResultQuery;
            int IntNumberRowsDet1OK = 0;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[54];

                #region PERSONA
                prmParameterHeader[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[0].Value = fichaRegistralParticipante.PERSONA.pers_iPersonaId;

                prmParameterHeader[1] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = fichaRegistralParticipante.PERSONA.pers_sPersonaTipoId;

                prmParameterHeader[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.VarChar, 100);
                prmParameterHeader[2].Value = fichaRegistralParticipante.PERSONA.pers_vApellidoPaterno;

                prmParameterHeader[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.VarChar, 100);
                prmParameterHeader[3].Value = fichaRegistralParticipante.PERSONA.pers_vApellidoMaterno;

                prmParameterHeader[4] = new SqlParameter("@pers_vNombres", SqlDbType.VarChar, 100);
                prmParameterHeader[4].Value = fichaRegistralParticipante.PERSONA.pers_vNombres;

                /****************************************************************************************************/
                /**************Parametros de ingreso para la tabla PN_REGISTRO.PERSONAIDENTIFICACION*****************/
                /****************************************************************************************************/
                prmParameterHeader[6] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[6].Value = fichaRegistralParticipante.PERSONA.Identificacion.peid_sDocumentoTipoId;

                prmParameterHeader[7] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[7].Value = fichaRegistralParticipante.PERSONA.Identificacion.peid_vDocumentoNumero;
                /****************************************************************************************************/

                if (fichaRegistralParticipante.PERSONA.pers_vCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[8].Value = fichaRegistralParticipante.PERSONA.pers_vCorreoElectronico;
                }

                prmParameterHeader[9] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                if (fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha == null || fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha == DateTime.MinValue)
                {
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else if (fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha == DateTime.MinValue)
                {
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[9].Value = fichaRegistralParticipante.PERSONA.pers_dNacimientoFecha;
                }

                /****************************************************************************************************/
                /***************************Parametro del Lugar de nacimiento o Ubigeo*******************************/
                /****************************************************************************************************/
                if (fichaRegistralParticipante.PERSONA.pers_cNacimientoLugar == "0")
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[10].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[10].Value = fichaRegistralParticipante.PERSONA.pers_cNacimientoLugar;
                }
                /****************************************************************************************************/

                if (fichaRegistralParticipante.PERSONA.pers_sGeneroId == 0)
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[11].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[11].Value = fichaRegistralParticipante.PERSONA.pers_sGeneroId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vObservaciones.Length == 0)
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[12].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[12].Value = fichaRegistralParticipante.PERSONA.pers_vObservaciones;
                }

                prmParameterHeader[13] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                prmParameterHeader[13].Value = fichaRegistralParticipante.PERSONA.pers_sNacionalidadId;

                if (fichaRegistralParticipante.PERSONA.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = fichaRegistralParticipante.PERSONA.pers_sEstadoCivilId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sGradoInstruccionId == 0)
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = fichaRegistralParticipante.PERSONA.pers_sGradoInstruccionId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sOcupacionId == 0)
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = fichaRegistralParticipante.PERSONA.pers_sOcupacionId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sProfesionId == 0)
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[17].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[17].Value = fichaRegistralParticipante.PERSONA.pers_sProfesionId;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vApellidoCasada.Length == 0)
                {
                    prmParameterHeader[18] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.VarChar, 100);
                    prmParameterHeader[18].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[18] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.VarChar, 100);
                    prmParameterHeader[18].Value = fichaRegistralParticipante.PERSONA.pers_vApellidoCasada;
                }

                /****************************************************************************************************/
                /*************Parametros de ingreso para la tabla PN_REGISTRO.RE_REGISTROUNICO***********************/
                /****************************************************************************************************/
                prmParameterHeader[19] = new SqlParameter("@reun_iRegistroUnicoId", SqlDbType.BigInt);
                prmParameterHeader[19].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_iRegistroUnicoId;

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaNombre.Length == 0)
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[20].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[20].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaNombre;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sEmergenciaRelacionId == 0)
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[21].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[21].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sEmergenciaRelacionId;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionLocal.Length == 0)
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[22].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[22].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionLocal;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCodigoPostal.Length == 0)
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[23].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[23].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCodigoPostal;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaTelefono.Length == 0)
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[24].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[24].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaTelefono;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionPeru.Length == 0)
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[25].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[25].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaDireccionPeru;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[26] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[26].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[26] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[26].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vEmergenciaCorreoElectronico;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cViveExteriorDesde == "00")
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cViveExteriorDesde;
                }

                prmParameterHeader[28] = new SqlParameter("@reun_bPiensaRetornarAlPeru", SqlDbType.Bit);
                prmParameterHeader[28].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bPiensaRetornarAlPeru;

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cCuandoRetornaAlPeru == "00")
                {
                    prmParameterHeader[29] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[29].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[29] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[29].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_cCuandoRetornaAlPeru;
                }

                prmParameterHeader[30] = new SqlParameter("@reun_bAfiliadoSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[30].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bAfiliadoSeguroSocial;

                prmParameterHeader[31] = new SqlParameter("@reun_bAfiliadoAFP", SqlDbType.Bit);
                prmParameterHeader[31].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bAfiliadoAFP;

                prmParameterHeader[32] = new SqlParameter("@reun_bAportaSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[32].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bAportaSeguroSocial;

                prmParameterHeader[33] = new SqlParameter("@reun_bBeneficiadoExterior", SqlDbType.Bit);
                prmParameterHeader[33].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_bBeneficiadoExterior;

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionPeru == 0)
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[34].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[34].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionPeru;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionExtranjero == 0)
                {
                    prmParameterHeader[35] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[35].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[35] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[35].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_sOcupacionExtranjero;
                }

                if (fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vNombreConvenio.Length == 0)
                {
                    prmParameterHeader[36] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[36].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[36] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[36].Value = fichaRegistralParticipante.PERSONA.REGISTROUNICO.reun_vNombreConvenio;
                }

                /****************************************************************************************************/

                prmParameterHeader[37] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[37].Value = fichaRegistralParticipante.OficinaConsultar;

                prmParameterHeader[38] = new SqlParameter("@pers_bGenera58A", SqlDbType.Bit);
                prmParameterHeader[38].Value = fichaRegistralParticipante.PERSONA.bGenera58A;

                prmParameterHeader[39] = new SqlParameter("@pers_sUsuarioModificacion", SqlDbType.Int);
                prmParameterHeader[39].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioModificacion;

                prmParameterHeader[40] = new SqlParameter("@pers_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterHeader[40].Value = fichaRegistralParticipante.PERSONA.pers_vIPModificacion;

                prmParameterHeader[41] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[41].Value = Util.ObtenerHostName();

                prmParameterHeader[42] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.Bit);
                prmParameterHeader[42].Value = fichaRegistralParticipante.PERSONA.pers_bFallecidoFlag;

                prmParameterHeader[43] = new SqlParameter("@pers_vSenasParticulares", SqlDbType.VarChar, 250);
                prmParameterHeader[43].Value = fichaRegistralParticipante.PERSONA.pers_vSenasParticulares;

                prmParameterHeader[44] = new SqlParameter("@pers_spaisid", SqlDbType.SmallInt);

                if (fichaRegistralParticipante.PERSONA.pers_sPaisId == 0)
                {
                    prmParameterHeader[44].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[44].Value = fichaRegistralParticipante.PERSONA.pers_sPaisId;
                }
                if (fichaRegistralParticipante.PERSONA.pers_vEstatura.Length == 0)
                {
                    prmParameterHeader[45] = new SqlParameter("@pers_vEstatura", SqlDbType.VarChar, 20);
                    prmParameterHeader[45].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[45] = new SqlParameter("@pers_vEstatura", SqlDbType.VarChar, 20);
                    prmParameterHeader[45].Value = fichaRegistralParticipante.PERSONA.pers_vEstatura;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sAnioEstudio == 0)
                {
                    prmParameterHeader[46] = new SqlParameter("@pers_sAnioEstudio", SqlDbType.SmallInt);
                    prmParameterHeader[46].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[46] = new SqlParameter("@pers_sAnioEstudio", SqlDbType.SmallInt);
                    prmParameterHeader[46].Value = fichaRegistralParticipante.PERSONA.pers_sAnioEstudio;
                }

                if (fichaRegistralParticipante.PERSONA.pers_cEstudioCompleto.Length == 0)
                {
                    prmParameterHeader[47] = new SqlParameter("@pers_cEstudioCompleto", SqlDbType.VarChar, 1);
                    prmParameterHeader[47].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[47] = new SqlParameter("@pers_cEstudioCompleto", SqlDbType.VarChar, 1);
                    prmParameterHeader[47].Value = fichaRegistralParticipante.PERSONA.pers_cEstudioCompleto;
                }


                if (fichaRegistralParticipante.PERSONA.pers_cDiscapacidad.Length == 0)
                {
                    prmParameterHeader[48] = new SqlParameter("@pers_cDiscapacidad", SqlDbType.VarChar, 1);
                    prmParameterHeader[48].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[48] = new SqlParameter("@pers_cDiscapacidad", SqlDbType.VarChar, 1);
                    prmParameterHeader[48].Value = fichaRegistralParticipante.PERSONA.pers_cDiscapacidad;
                }

                if (fichaRegistralParticipante.PERSONA.pers_cInterdicto.Length == 0)
                {
                    prmParameterHeader[49] = new SqlParameter("@pers_cInterdicto", SqlDbType.VarChar, 1);
                    prmParameterHeader[49].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[49] = new SqlParameter("@pers_cInterdicto", SqlDbType.VarChar, 1);
                    prmParameterHeader[49].Value = fichaRegistralParticipante.PERSONA.pers_cInterdicto;
                }

                if (fichaRegistralParticipante.PERSONA.pers_cDonaOrganos.Length == 0)
                {
                    prmParameterHeader[50] = new SqlParameter("@pers_cDonaOrganos", SqlDbType.VarChar, 1);
                    prmParameterHeader[50].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[50] = new SqlParameter("@pers_cDonaOrganos", SqlDbType.VarChar, 1);
                    prmParameterHeader[50].Value = fichaRegistralParticipante.PERSONA.pers_cDonaOrganos;
                }

                if (fichaRegistralParticipante.PERSONA.pers_vNombreCurador.Length == 0)
                {
                    prmParameterHeader[51] = new SqlParameter("@pers_vNombreCurador", SqlDbType.VarChar, 100);
                    prmParameterHeader[51].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[51] = new SqlParameter("@pers_vNombreCurador", SqlDbType.VarChar, 100);
                    prmParameterHeader[51].Value = fichaRegistralParticipante.PERSONA.pers_vNombreCurador;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sTipoDeclarante == 0)
                {
                    prmParameterHeader[52] = new SqlParameter("@pers_sTipoDeclarante", SqlDbType.SmallInt);
                    prmParameterHeader[52].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[52] = new SqlParameter("@pers_sTipoDeclarante", SqlDbType.SmallInt);
                    prmParameterHeader[52].Value = fichaRegistralParticipante.PERSONA.pers_sTipoDeclarante;
                }

                if (fichaRegistralParticipante.PERSONA.pers_sTipoTutorGuardador == 0)
                {
                    prmParameterHeader[53] = new SqlParameter("@pers_sTipoTutorGuardador", SqlDbType.SmallInt);
                    prmParameterHeader[53].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[53] = new SqlParameter("@pers_sTipoTutorGuardador", SqlDbType.SmallInt);
                    prmParameterHeader[53].Value = fichaRegistralParticipante.PERSONA.pers_sTipoTutorGuardador;
                }
                #endregion
                IntResultQuery = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR",
                                                           prmParameterHeader);


                if (fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_iResidenciaId > 0)
                {
                    #region Direcciones
                    SqlParameter[] prmParameterDirecciones;
                    prmParameterDirecciones = new SqlParameter[10];

                    prmParameterDirecciones[0] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                    prmParameterDirecciones[0].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_iResidenciaId;


                    prmParameterDirecciones[1] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                    prmParameterDirecciones[1].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_sResidenciaTipoId;

                    prmParameterDirecciones[2] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                    prmParameterDirecciones[2].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vResidenciaDireccion;

                    prmParameterDirecciones[3] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterDirecciones[3].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vCodigoPostal;

                    prmParameterDirecciones[4] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[4].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vResidenciaTelefono;

                    prmParameterDirecciones[5] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                    prmParameterDirecciones[5].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_cResidenciaUbigeo;

                    prmParameterDirecciones[6] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterDirecciones[6].Value = fichaRegistralParticipante.OficinaConsultar;

                    prmParameterDirecciones[7] = new SqlParameter("@resi_sUsuarioModificacion", SqlDbType.SmallInt);
                    prmParameterDirecciones[7].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioModificacion;

                    prmParameterDirecciones[8] = new SqlParameter("@resi_vIPModificacion", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[8].Value = fichaRegistralParticipante.PERSONA.pers_vIPModificacion;

                    prmParameterDirecciones[9] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                    prmParameterDirecciones[9].Value = Util.ObtenerHostName();


                    IntNumberRowsDet1OK = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_RESIDENCIA_ACTUALIZAR",
                                                                    prmParameterDirecciones);

                    #endregion
                }
                else
                {
                    //----------------------
                    //Nueva Residencia
                    //----------------------
                    fichaRegistralParticipante.fipa_iPersonaId = fichaRegistralParticipante.PERSONA.pers_iPersonaId;
                    #region Direcciones
                    SqlParameter[] prmParameterDireccionesNueva;

                    prmParameterDireccionesNueva = new SqlParameter[10];

                    prmParameterDireccionesNueva[0] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                    prmParameterDireccionesNueva[0].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_sResidenciaTipoId;

                    prmParameterDireccionesNueva[1] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                    prmParameterDireccionesNueva[1].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vResidenciaDireccion;

                    prmParameterDireccionesNueva[2] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterDireccionesNueva[2].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vCodigoPostal;

                    prmParameterDireccionesNueva[3] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterDireccionesNueva[3].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_vResidenciaTelefono;

                    prmParameterDireccionesNueva[4] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                    prmParameterDireccionesNueva[4].Value = fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_cResidenciaUbigeo;

                    prmParameterDireccionesNueva[5] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterDireccionesNueva[5].Value = fichaRegistralParticipante.OficinaConsultar;

                    prmParameterDireccionesNueva[6] = new SqlParameter("@resi_sUsuarioCreacion", SqlDbType.SmallInt);
                    prmParameterDireccionesNueva[6].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioCreacion;

                    prmParameterDireccionesNueva[7] = new SqlParameter("@resi_vIPCreacion", SqlDbType.VarChar, 50);
                    prmParameterDireccionesNueva[7].Value = fichaRegistralParticipante.PERSONA.pers_vIPCreacion;

                    prmParameterDireccionesNueva[8] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                    prmParameterDireccionesNueva[8].Value = Util.ObtenerHostName();

                    prmParameterDireccionesNueva[9] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                    prmParameterDireccionesNueva[9].Direction = ParameterDirection.Output;

                    IntNumberRowsDet1OK = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR",
                                                                    prmParameterDireccionesNueva);

                    long LonResidenciaId = (long)prmParameterDireccionesNueva[9].Value;
                    if (LonResidenciaId > 0)
                    {
                        fichaRegistralParticipante.PERSONA.Residencias[0].Residencia.resi_iResidenciaId = LonResidenciaId;

                        SqlParameter[] prmParameterDetail2;

                        prmParameterDetail2 = new SqlParameter[6];

                        prmParameterDetail2[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                        prmParameterDetail2[0].Value = fichaRegistralParticipante.PERSONA.pers_iPersonaId;

                        prmParameterDetail2[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                        prmParameterDetail2[1].Value = LonResidenciaId;

                        prmParameterDetail2[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                        prmParameterDetail2[2].Value = fichaRegistralParticipante.OficinaConsultar;

                        prmParameterDetail2[3] = new SqlParameter("@pere_sUsuarioCreacion", SqlDbType.SmallInt);
                        prmParameterDetail2[3].Value = fichaRegistralParticipante.PERSONA.pers_sUsuarioCreacion;

                        prmParameterDetail2[4] = new SqlParameter("@pere_vIPCreacion", SqlDbType.VarChar, 50);
                        prmParameterDetail2[4].Value = fichaRegistralParticipante.PERSONA.pers_vIPCreacion;

                        prmParameterDetail2[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                        prmParameterDetail2[5].Value = Util.ObtenerHostName();

                        int IntNumberRowsDet2OK = SqlHelper.ExecuteNonQuery(this.conexion(),
                                                                        CommandType.StoredProcedure,
                                                                        "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR",
                                                                        prmParameterDetail2);
                    }
                }
                #endregion

                return (int)Enumerador.enmResultadoQuery.OK;
            }

            catch (SqlException exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.StackTrace.ToString();
                return -1;
            }
            catch (Exception exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.StackTrace.ToString();
                return -1;
            }
        }


        //---------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 10/03/2017
        // Objetivo: Solo agrega participante sin actualizar nada
        //---------------------------------------------------------------
        public void AgregarParticipante(ref RE_FICHAREGISTRALPARTICIPANTE fichaRegistralParticipante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                        using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALPARTICIPANTE_ADICIONAR_MRE", cnx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            #region Creando Parametros
                            cmd.Parameters.Add(new SqlParameter("@fipa_iFichaRegistralId", fichaRegistralParticipante.fipa_iFichaRegistralId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_sTipoParticipanteId", fichaRegistralParticipante.fipa_sTipoParticipanteId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_iPersonaId", fichaRegistralParticipante.fipa_iPersonaId));
                            cmd.Parameters.Add(new SqlParameter("@fipa_sUsuarioCreacion", fichaRegistralParticipante.fipa_sUsuarioCreacion));
                            cmd.Parameters.Add(new SqlParameter("@fipa_vIPCreacion", fichaRegistralParticipante.fipa_vIPCreacion));
                            cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistralParticipante.OficinaConsultar));
                            cmd.Parameters.Add(new SqlParameter("@fipa_bConsignaApellidoMaterno", fichaRegistralParticipante.fipa_bConsignaApellidoMaterno));
                            
                            #endregion

                            #region Output
                            SqlParameter lReturn = cmd.Parameters.Add("@fipa_iFichaRegistralParticipanteId", SqlDbType.BigInt);
                            lReturn.Direction = ParameterDirection.Output;


                            #endregion

                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            fichaRegistralParticipante.fipa_iFichaRegistralParticipanteId = Convert.ToInt64(lReturn.Value);
                            fichaRegistralParticipante.Error = false;
                        }
                }
            }
            catch (SqlException exec)
            {
                fichaRegistralParticipante.Error = true;
                fichaRegistralParticipante.Message = exec.Message.ToString();
            }
        }

        

        //---------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 18/02/2019
        // Objetivo: Solo agrega participante sin actualizar nada
        //---------------------------------------------------------------
        public string AgregarDocumentoAdjuntoFicha(long fido_iFichaRegistralID, Int16 fido_sDocumentoID, string fido_vNumDocumentoID, Int16 fido_sUsuarioModificacion, string fido_vIPModificacion)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALDOCUMENTO_REGISTRAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_fido_iFichaRegistralID", fido_iFichaRegistralID));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_sDocumentoID", fido_sDocumentoID));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_vNumDocumentoID", fido_vNumDocumentoID));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_sUsuarioModificacion", fido_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_vIPModificacion", fido_vIPModificacion));
                        #endregion


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                }
            }
            catch (SqlException exec)
            {
                return exec.ToString();
            }
        }
        //---------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 18/02/2019
        // Objetivo: Solo actualiza participante sin actualizar nada
        //---------------------------------------------------------------
        public string ActualizarDocumentoAdjuntoFicha(long fido_iFichaRegistralDocumentoID, Int16 fido_sDocumentoID, string fido_vNumDocumentoID, string fido_cEstado, Int16 fido_sUsuarioModificacion, string fido_vIPModificacion)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRALDOCUMENTO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_fido_iFichaRegistralDocumentoID", fido_iFichaRegistralDocumentoID));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_sDocumentoID", fido_sDocumentoID));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_vNumDocumentoID", fido_vNumDocumentoID));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_cEstado", fido_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_sUsuarioModificacion", fido_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_fido_vIPModificacion", fido_vIPModificacion));
                        #endregion


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                }
            }
            catch (SqlException exec)
            {
                return exec.ToString();
            }
        }
        
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

    }
}
