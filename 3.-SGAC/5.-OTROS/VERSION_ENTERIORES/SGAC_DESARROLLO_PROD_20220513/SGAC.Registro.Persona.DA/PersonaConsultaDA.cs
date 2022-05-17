using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.BE;
using System.Reflection;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public PersonaConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PersonaConsultaDA()
        {
            GC.Collect();
        }



        public BE.MRE.RE_PERSONA USP_RE_PERSONA_OBTENERXIDPERSONA(Int64 iPersonaId)
        {
           BE.MRE.RE_PERSONA objBE = new BE.MRE.RE_PERSONA();
            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_PERSONA_OBTENERXIDPERSONA]", cnn))
                    {
                        cnn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt).Value = iPersonaId;

                        using (SqlDataReader sdr = cmd.ExecuteReader()) {

                            if (sdr.HasRows)
                            {
                                while (sdr.Read()) { 
                                    
                                    if(!sdr.IsDBNull(0))
                                            objBE.pers_iPersonaId = sdr.GetInt64(0);
                                    if(!sdr.IsDBNull(1))
                                            objBE.pers_sPersonaTipoId = sdr.GetInt16(1);
                                    if(!sdr.IsDBNull(2))
                                            objBE.Identificacion.peid_sDocumentoTipoId = sdr.GetInt16(2);
                                    if(!sdr.IsDBNull(3))
                                            objBE.Identificacion.peid_vDocumentoNumero = sdr.GetString(3);
                                    if(!sdr.IsDBNull(4))
                                            objBE.pers_vApellidoPaterno = sdr.GetString(4);
                                    if(!sdr.IsDBNull(5))
                                        objBE.pers_vApellidoMaterno = sdr.GetString(5);
                                    if(!sdr.IsDBNull(6))
                                        objBE.pers_vNombres = sdr.GetString(6);
                                    
                                    if(!sdr.IsDBNull(8))
                                        objBE.pers_vCorreoElectronico = sdr.GetString(8);
                                    if(!sdr.IsDBNull(9))
                                            objBE.pers_sGeneroId = sdr.GetInt16(9);
                                    if(!sdr.IsDBNull(10))
                                        objBE.pers_vObservaciones = sdr.GetString(10);
                                    if(!sdr.IsDBNull(11))
                                            objBE.pers_sNacionalidadId = Convert.ToInt16(sdr.GetInt32(11));
                                    if(!sdr.IsDBNull(12))
                                            objBE.pers_sEstadoCivilId = Convert.ToInt16(sdr.GetInt32(12));
                                    if(!sdr.IsDBNull(13))
                                            objBE.pers_sGradoInstruccionId = Convert.ToInt16(sdr.GetInt32(13));
                                    if(!sdr.IsDBNull(14))
                                            objBE.pers_sOcupacionId =Convert.ToInt16(sdr.GetInt32(14));
                                    if(!sdr.IsDBNull(15))
                                            objBE.pers_sProfesionId = Convert.ToInt16(sdr.GetInt32(15));
                                    if(!sdr.IsDBNull(16))
                                        objBE.pers_vApellidoCasada = sdr.GetString(16);
                                    if(!sdr.IsDBNull(17))
                                            objBE.pers_sPeso = Convert.ToInt16(sdr.GetInt32(17));
                                    if(!sdr.IsDBNull(18))
                                            objBE.pers_sOcurrenciaTipoId = Convert.ToInt16(sdr.GetInt32(18));


                                    if(!sdr.IsDBNull(19))
                                        objBE.pers_vLugarNacimiento = sdr.GetString(19);
                                    if(!sdr.IsDBNull(20))
                                            objBE.pers_dNacimientoFecha = sdr.GetDateTime(20);
                                    if(!sdr.IsDBNull(21))
                                        objBE.pers_cNacimientoLugar = sdr.GetString(21);
                                 
                                    /*if(!sdr.IsDBNull(22))
                                            objBE.pers_sColorCabelloId = sdr.GetInt16(22);
                                    if(!sdr.IsDBNull(23))
                                            objBE.pers_sColorOjosId = sdr.GetInt64(23);
                                    if(!sdr.IsDBNull(24))
                                            objBE.pers_sColorTezId = sdr.GetInt64(24);
                                    if(!sdr.IsDBNull(25))
                                            objBE.pers_vSenasParticulares = sdr.GetInt64(25);
                                    if(!sdr.IsDBNull(26))
                                            objBE.pers_sGrupoSanguineoId = sdr.GetInt64(26);
                                    if(!sdr.IsDBNull(27))
                                            objBE.pers_vEstatura = sdr.GetInt64(27);
                                     * */

                                    if(!sdr.IsDBNull(28))
                                        objBE.REGISTROUNICO.reun_iRegistroUnicoId = sdr.GetInt64(28);
                              				                     				      				                   
                                }

                                return objBE;
                            }
                            else
                            {
                                return null;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }

        public BE.MRE.RE_PERSONA obtener(BE.MRE.RE_PERSONA Persona)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_BUSCAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        if (Persona.pers_iPersonaId != 0) cmd.Parameters.Add(new SqlParameter("@pers_iPersonaId", Persona.pers_iPersonaId));                        
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)Persona.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(Persona, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }
                        Persona.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                Persona.Error = true;
                Persona.Message = exec.Message.ToString();
            }
            return Persona;
        }

        public DataTable Consultar(int IntTipo,
                                   string StrNroDoc,
                                   string StrApePat,
                                   string StrApeMat,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_CONSULTAR_ASN", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ITipoQ", IntTipo));
                        if (StrNroDoc.Length == 0) cmd.Parameters.Add(new SqlParameter("@vNroDocumento", string.Empty));
                        else cmd.Parameters.Add(new SqlParameter("@vNroDocumento", StrNroDoc));
                        if (StrApePat.Length == 0) cmd.Parameters.Add(new SqlParameter("@vApellidoPaterno", string.Empty));
                        else cmd.Parameters.Add(new SqlParameter("@vApellidoPaterno", StrApePat));
                        if (StrApeMat.Length == 0) cmd.Parameters.Add(new SqlParameter("@vApellidoMaterno", string.Empty));
                        else cmd.Parameters.Add(new SqlParameter("@vApellidoMaterno", StrApeMat));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataTable RecurrenteConsultar(int IntTipo,
                                             string StrNroDoc,
                                             string StrApePat,
                                             string StrApeMat,
                                             int IntCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[8];

                prmParameter[0] = new SqlParameter("@ITipoQ", SqlDbType.Int);
                prmParameter[0].Value = IntTipo;

                if (StrNroDoc.Length == 0)
                {
                    prmParameter[1] = new SqlParameter("@vNroDocumento", SqlDbType.VarChar, 20);
                    prmParameter[1].Value = "";
                }
                else
                {
                    prmParameter[1] = new SqlParameter("@vNroDocumento", SqlDbType.VarChar, 20);
                    prmParameter[1].Value = StrNroDoc;
                }

                if (StrApePat.Length == 0)
                {
                    prmParameter[2] = new SqlParameter("@vApellidoPaterno", SqlDbType.VarChar, 100);
                    prmParameter[2].Value = "";
                }
                else
                {
                    prmParameter[2] = new SqlParameter("@vApellidoPaterno", SqlDbType.VarChar, 100);
                    prmParameter[2].Value = StrApePat;
                }

                if (StrApeMat.Length == 0)
                {
                    prmParameter[3] = new SqlParameter("@vApellidoMaterno", SqlDbType.VarChar, 100);
                    prmParameter[3].Value = "";
                }
                else
                {
                    prmParameter[3] = new SqlParameter("@vApellidoMaterno", SqlDbType.VarChar, 100);
                    prmParameter[3].Value = StrApeMat;
                }

                prmParameter[4] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
                prmParameter[4].Value = IntCurrentPage;

                prmParameter[5] = new SqlParameter("@IPageSize", SqlDbType.Int);
                prmParameter[5].Value = IntPageSize;

                prmParameter[6] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
                prmParameter[6].Direction = ParameterDirection.Output;

                prmParameter[7] = new SqlParameter("@ITotalPages", SqlDbType.Int);
                prmParameter[7].Direction = ParameterDirection.Output;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONA_CONSULTAR",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];

                IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[6]).Value);
                IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[7]).Value);

                return DtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }

        public DataTable EmpresaConsultar(string StrNroDoc,
                                          string StrRazonSocial,
                                          int IntCurrentPage,
                                          int IntPageSize,
                                          ref int IntTotalCount,
                                          ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[6];

                prmParameter[0] = new SqlParameter("@empr_vNroDocumento", SqlDbType.VarChar, 20);
                prmParameter[0].Value = StrNroDoc;
                prmParameter[1] = new SqlParameter("@empr_vRazonSocial", SqlDbType.VarChar, 200);
                prmParameter[1].Value = StrRazonSocial;
                prmParameter[2] = new SqlParameter("@IPaginaActual", SqlDbType.Int);
                prmParameter[2].Value = IntCurrentPage;
                prmParameter[3] = new SqlParameter("@IPaginaCantidad", SqlDbType.Int);
                prmParameter[3].Value = IntPageSize;
                prmParameter[4] = new SqlParameter("@ITotalRegistros", SqlDbType.Int);
                prmParameter[4].Direction = ParameterDirection.Output;
                prmParameter[5] = new SqlParameter("@ITotalPaginas", SqlDbType.Int);
                prmParameter[5].Direction = ParameterDirection.Output;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_EMPRESA_CONSULTAR",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];

                IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);
                IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[5]).Value);

                return DtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
                DsResult = null;
            }
        }

        public DataTable PersonaGetById(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero="")
        {
            DataSet DsResult = null;
            DataTable DtResult = null;

            BE.RE_PERSONA objBE = new BE.RE_PERSONA();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[3];

                prmParameter[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameter[0].Value = LonPersonaId;

                prmParameter[1] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameter[1].Value = intDocumentoId;

                prmParameter[2] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameter[2].Value = strDocumentoNumero;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONA_OBTENERXIDPERSONA",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataTable PersonaGetById(string StrDNI)
        {
            DataSet DsResult = null;
            DataTable DtResult = null;

            BE.RE_PERSONA objBE = new BE.RE_PERSONA();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[1];

                prmParameter[0] = new SqlParameter("@peid_vDNI", SqlDbType.VarChar, 10);
                prmParameter[0].Value = StrDNI;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONA_OBTENERXNRODOC",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataSet Persona_Imprimir_Rune(BE.RE_PERSONA objBE)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection cn = new SqlConnection(StrConnectionName))
            {
                try
                {
                    da = new SqlDataAdapter("PN_REPORTES.USP_RP_FORMATO_RUNE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@PI_pers_iPersonaId", SqlDbType.Int).Value = objBE.pers_iPersonaId;
                    da.Fill(ds, "Persona");
                    dt = ds.Tables["Persona"];

                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int Tiene58A(long LonPersonaId)
        {
            int IntResult = 0;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[2];

                prmParameter[0] = new SqlParameter("@Rspta", SqlDbType.Int);
                prmParameter[0].Direction = ParameterDirection.Output;

                prmParameter[1] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameter[1].Value = LonPersonaId;

                SqlHelper.ExecuteNonQuery(StrConnectionName,
                                          CommandType.StoredProcedure,
                                          "PN_REGISTRO.USP_RE_PERSONA_TIENE_58A",
                                           prmParameter);

                IntResult = (int)prmParameter[0].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IntResult;
        }
        public DataTable Tiene58B(long LonPersonaId)
        {
            using (SqlConnection cn = new SqlConnection(StrConnectionName))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("[PN_REGISTRO].[USP_RE_PERSONA_TIENE_58B]", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@pers_iPersonaId", SqlDbType.Int).Value = LonPersonaId;
                    da.Fill(ds, "Resultado");
                    dt = ds.Tables["Resultado"];
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public DataTable Obtener_Persona(object[] arrParametros)
        {
         
            DataTable dt = new DataTable();

            EnPersona objEn = new EnPersona();
            objEn = (EnPersona)arrParametros[0];
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_OBTENER_TRAMITES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add("@ipers_iPersonaId", SqlDbType.Int).Value = objEn.iPersonaId;
                        cmd.Parameters.Add("@peid_sDocumentoTipoId", SqlDbType.Int).Value = objEn.sDocumentoTipoId;
                        cmd.Parameters.Add("@vDocumentoNumero", SqlDbType.VarChar, 20).Value = objEn.vDocumentoNumero;
                        cmd.Parameters.Add("@acpa_sTipoParticipanteId", SqlDbType.Int).Value = objEn.sTipoParticipanteId;
                        cmd.Parameters.Add("@acpa_sTipoDatoId", SqlDbType.Int).Value = objEn.sTipoDatoId;
                        cmd.Parameters.Add("@acpa_sTipoVinculoId", SqlDbType.Int).Value = objEn.sTipoVinculoId;

                        if (objEn.sResidenciaTipoId != null)
                        {
                            if (objEn.sResidenciaTipoId != 0)
                                cmd.Parameters.Add("@resi_sResidenciaTipoId", SqlDbType.Int).Value = objEn.sResidenciaTipoId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@resi_sResidenciaTipoId", SqlDbType.Int).Value = null;
                        }
                        cmd.Connection.Open();
                        DataSet ds = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds);
                            dt = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable Obtener_Empresa(object[] arrParametros)
        {
            RE_EmpresaFiltro objEn = new RE_EmpresaFiltro();

            objEn = (RE_EmpresaFiltro)arrParametros[0];

            using (SqlConnection cn = new SqlConnection(StrConnectionName))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_EMPRESA_OBTENER", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@empr_vNroDocumento", SqlDbType.VarChar, 20).Value = objEn.empr_vNumeroDocumento;
                    da.SelectCommand.Parameters.Add("@empr_vRazonSocial", SqlDbType.VarChar, 200).Value = objEn.empr_vRazonSocial;
                    da.SelectCommand.Parameters.Add("@empr_sTipoDocumentoId", SqlDbType.Int).Value = objEn.empr_sTipoDocumentoId;
                    da.SelectCommand.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = objEn.empr_iEmpresaId;

                    da.SelectCommand.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = objEn.iPaginaActual;
                    da.SelectCommand.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = objEn.iPaginaCantidad;

                    da.SelectCommand.Parameters.Add("@ITotalRegistros", SqlDbType.Int).Direction = ParameterDirection.Output;
                    da.SelectCommand.Parameters.Add("@ITotalPaginas", SqlDbType.Int).Direction = ParameterDirection.Output;

                    da.Fill(ds, "Empresa");
                    dt = ds.Tables["Empresa"];

                    objEn.iTotalRegistros = Convert.ToInt32(da.SelectCommand.Parameters["@ITotalRegistros"].Value.ToString());
                    objEn.iTotalPaginas = Convert.ToInt32(da.SelectCommand.Parameters["@ITotalPaginas"].Value.ToString());

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Obtener_Empresa_Por_Id(int intEmpresaId)
        {
            DataTable dtResult = new DataTable();
            DataSet DsResult = new DataSet();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[1];

                prmParameter[0] = new SqlParameter("@empr_sEmpresaId", SqlDbType.Int);
                prmParameter[0].Value = intEmpresaId;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                          CommandType.StoredProcedure,
                                          "PN_REGISTRO.USP_RE_EMPRESA_OBTENER_POR_ID",
                                           prmParameter);

                dtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtResult;
        }

        public bool PersonaPoseeActuaciones(short iTipoDocumento, string vNumeroDocumento)
        {
         

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_CANTIDAD_ACTUACIONES", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@iTipoDocumento", iTipoDocumento));
                        cmd.Parameters.Add(new SqlParameter("@vNumeroDocumento", vNumeroDocumento));
                        SqlParameter lCantidadActuaciones = cmd.Parameters.Add("@iCantidadActuaciones", SqlDbType.Int);
                        lCantidadActuaciones.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                       

                        if (Convert.ToInt32(lCantidadActuaciones.Value) > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public DataTable ObtenerDatosPersona(short iTipoDocumento, string vNumeroDocumento)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_BUSCAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", iTipoDocumento));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", vNumeroDocumento));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable ObtenerDatosPersonaFiliacion(short iTipoDocumento, string vNumeroDocumento)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ACTO_PROTOCOLAR_BUSCAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@peid_iPersonaIdentificacionId", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@peid_iPersonaId", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", iTipoDocumento));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", vNumeroDocumento));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        public DataTable ObtenerParticipacionesPersona(short iTipoDocumento, string vNumeroDocumento)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_CANTIDAD_PARTICIPACIONES_ACTUACIONES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@iTipoDocumento", iTipoDocumento));
                        cmd.Parameters.Add(new SqlParameter("@vNumeroDocumento", vNumeroDocumento));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable PersonaListarNacionalidades(Int64 iPersona)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.RE_PERSONANACIONALIDAD_LISTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_pers_iPersonaId", iPersona));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable PersonaListarUltimaNacionalidad(Int64 iPersona)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.RE_PERSONANACIONALIDAD_LISTAR_ULTIMO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_pers_iPersonaId", iPersona));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

    
    }
}