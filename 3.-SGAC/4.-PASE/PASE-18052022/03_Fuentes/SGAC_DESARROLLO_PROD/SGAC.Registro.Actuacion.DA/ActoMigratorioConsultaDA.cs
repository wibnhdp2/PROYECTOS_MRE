using System;
using SGAC.BE.MRE.Custom;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using SGAC.BE.MRE;
using System.Collections.Generic;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoMigratorioConsultaDA : SGAC.BE.RE_GENERAL
    {
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public Int64 Consultar_Correlativo(Int64 acmi_sTipoDocumentoMigratorioId)
        {
            Int64 i_Resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_OBTENER_CORRELATIVO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", acmi_sTipoDocumentoMigratorioId));

                        SqlParameter lReturn = cmd.Parameters.Add("acmi_vNumeroExpediente", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        i_Resultado = Convert.ToInt64(lReturn.Value);
                        
                    }
                }
            }
            catch (SqlException exec)
            {
                i_Resultado = -1;
            }
            return i_Resultado;
        }

        public Int64 Consultar_Correlativo_Tipo_Doc_Migratorio(Int64 exp_sTipoDocMigId)
        {
            Int64 i_Resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_EXPEDIENTE_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@exp_sTipoDocMigId", exp_sTipoDocMigId));
                        SqlParameter lReturn = cmd.Parameters.Add("acmi_vNumeroExpediente", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        i_Resultado = Convert.ToInt64(lReturn.Value);

                    }
                }
            }
            catch (SqlException exec)
            {
                i_Resultado = -1;
            }
            return i_Resultado;
        }

        public CBE_PERSONA Consultar_Persona(long lngPersonaId)
        {
            CBE_PERSONA obj_Resultado = new CBE_PERSONA();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ACTOMIGRATORIO_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pers_iPersonaId", lngPersonaId));

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        if (ds_Objeto.Tables.Count > 0)
                        {
                            for (int col = 0; col <= ds_Objeto.Tables.Count - 1; col++)
                            {
                                for (int row = 0; row <= ds_Objeto.Tables[col].Rows.Count - 1; row++)
                                {
                                    switch (ds_Objeto.Tables[col].TableName)
                                    {
                                        case "Table":                                            

                                            obj_Resultado.pers_dNacimientoFecha = DateTime.MinValue;
                                            obj_Resultado.pers_cNacimientoLugar = string.Empty;

                                            if (ds_Objeto.Tables[col].Rows[row]["pers_dNacimientoFecha"] != null)
                                            {
                                                if (ds_Objeto.Tables[col].Rows[row]["pers_dNacimientoFecha"].ToString() != string.Empty)
                                                    obj_Resultado.pers_dNacimientoFecha = Convert.ToDateTime(ds_Objeto.Tables[col].Rows[row]["pers_dNacimientoFecha"]);
                                            }
                                            if (ds_Objeto.Tables[col].Rows[row]["pers_cNacimientoLugar"] != null)
                                                obj_Resultado.pers_cNacimientoLugar = ds_Objeto.Tables[col].Rows[row]["pers_cNacimientoLugar"].ToString();
                                            obj_Resultado.pers_sGeneroId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pers_sGeneroId"]);
                                            obj_Resultado.pers_sEstadoCivilId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pers_sEstadoCivilId"]);
                                            obj_Resultado.pers_sColorOjosId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pers_sColorOjosId"]);
                                            obj_Resultado.pers_sColorCabelloId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pers_sColorCabelloId"]);
                                            obj_Resultado.pers_sOcupacionId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pers_sOcupacionId"]);
                                            obj_Resultado.pers_vApellidoPaterno = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["pers_vApellidoPaterno"]);
                                            obj_Resultado.pers_vApellidoMaterno = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["pers_vApellidoMaterno"]);
                                            obj_Resultado.pers_vNombres = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["pers_vNombres"]);
                                            obj_Resultado.pers_iPersonaId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pers_iPersonaId"]);
                                            if (ds_Objeto.Tables[col].Rows[row]["pers_vEstatura"] != null)
                                                obj_Resultado.pers_vEstatura = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["pers_vEstatura"]);

                                            //obj_Resultado.Identificacion..MA_DOCUMENTO_IDENTIDAD = new BE.MA_DOCUMENTO_IDENTIDAD
                                            //{
                                            //    doid_sTipoDocumentoIdentidadId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["peid_sDocumentoTipoId"]),
                                            //    doid_vDescripcionCorta = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["peid_vDocumentoNumero"])
                                            //};
                                            obj_Resultado.Identificacion.peid_sDocumentoTipoId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["peid_sDocumentoTipoId"]);
                                            obj_Resultado.Identificacion.peid_vDocumentoNumero = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["peid_vDocumentoNumero"]);
                                            
                                            break;
                                        case "Table1":
                                            obj_Resultado.FILIACIONES.Add(new CBE_FILIACION
                                            {
                                                pefi_iPersonaFilacionId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pefi_iPersonaFilacionId"]),
                                                pefi_iPersonaId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pefi_iPersonaId"]),
                                                pefi_vNombreFiliacion = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["pefi_vNombreFiliacion"]),
                                                pefi_sTipoFilacionId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pefi_sTipoFilacionId"])
                                            });
                                            
                                            break;
                                        case "Table2":
                                            obj_Resultado.FILIACIONES.Add(new CBE_FILIACION
                                            {
                                                pefi_iPersonaFilacionId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pefi_iPersonaFilacionId"]),
                                                pefi_iPersonaId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pefi_iPersonaId"]),
                                                pefi_vNombreFiliacion = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["pefi_vNombreFiliacion"]),
                                                pefi_sTipoFilacionId = ToNullInt16(ds_Objeto.Tables[col].Rows[row]["pefi_sTipoFilacionId"])
                                            });

                                            break;
                                        case "Table3":
                                            obj_Resultado.RESIDENCIAS.Add(new BE.RE_RESIDENCIA
                                            {
                                                resi_iResidenciaId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pere_iResidenciaId"]),
                                                resi_cResidenciaUbigeo = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_cResidenciaUbigeo"]),
                                                resi_vResidenciaDireccion = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_vResidenciaDireccion"]),
                                                resi_vResidenciaTelefono = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_vResidenciaTelefono"]),
                                                resi_cEstado = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_cTipo"])
                                            });
                                            break;
                                        case "Table4":
                                            obj_Resultado.RESIDENCIAS.Add(new BE.RE_RESIDENCIA
                                            {
                                                resi_iResidenciaId = ToNullInt64(ds_Objeto.Tables[col].Rows[row]["pere_iResidenciaId"]),
                                                resi_cResidenciaUbigeo = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_cResidenciaUbigeo"]),
                                                resi_vResidenciaDireccion = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_vResidenciaDireccion"]),
                                                resi_vResidenciaTelefono = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_vResidenciaTelefono"]),
                                                resi_cEstado = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["resi_cTipo"])
                                            });

                                            break;
                                        case "Table5":
                                            obj_Resultado.IDENTIFICACION = new BE.RE_PERSONAIDENTIFICACION();
                                            obj_Resultado.IDENTIFICACION.OficinaConsularId = Convert.ToInt16(ds_Objeto.Tables[col].Rows[row]["actu_sOficinaConsularId"]);
                                            obj_Resultado.IDENTIFICACION.peid_dFecExpedicion = Convert.ToDateTime(ds_Objeto.Tables[col].Rows[row]["acmi_dFechaExpedicion"]);
                                            obj_Resultado.IDENTIFICACION.peid_dFecRenovacion = Convert.ToDateTime(ds_Objeto.Tables[col].Rows[row]["acmi_dFechaExpiracion"]);
                                            obj_Resultado.IDENTIFICACION.peid_vDocumentoNumero = Convert.ToString(ds_Objeto.Tables[col].Rows[row]["acmi_vNumeroDocumento"]);
                                            obj_Resultado.IDENTIFICACION.peid_sDocumentoTipoId = Convert.ToInt16(ds_Objeto.Tables[col].Rows[row]["vTipoDocumento"]);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = new CBE_PERSONA();
                obj_Resultado.Message = exec.Message;
            }
            return obj_Resultado;
        }        
        public RE_ACTOMIGRATORIO Consultar_Acto_Migratorio(long acmi_iActuacionDetalleId)
        {
            RE_ACTOMIGRATORIO obj_Resultado = new RE_ACTOMIGRATORIO();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActuacionDetalleId", acmi_iActuacionDetalleId));

                        cmd.Connection.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            obj_Resultado.acmi_iActoMigratorioId = ToNullInt64(dr["acmi_iActoMigratorioId"]);
                            obj_Resultado.acmi_iActuacionDetalleId = ToNullInt64(dr["acmi_iActuacionDetalleId"]);
                            obj_Resultado.acmi_IFuncionarioId = Convert.ToInt32(dr["acmi_IFuncionarioId"]);
                            obj_Resultado.acmi_sTipoDocumentoMigratorioId = ToNullInt16(dr["acmi_sTipoDocumentoMigratorioId"]);
                            obj_Resultado.acmi_iActoMigratorioId = ToNullInt64(dr["acmi_iActoMigratorioId"]);
                            obj_Resultado.acmi_sTipoId = ToNullInt16(dr["acmi_sTipoId"]);
                            obj_Resultado.acmi_sSubTipoId = ToNullInt16(dr["acmi_sSubTipoId"]);
                            obj_Resultado.acmi_vNumeroExpediente = Convert.ToString(dr["acmi_vNumeroExpediente"]);
                            obj_Resultado.acmi_vNumeroLamina = Convert.ToString(dr["acmi_vNumeroLamina"]);
                            obj_Resultado.acmi_dFechaExpedicion = Convert.ToDateTime(dr["acmi_dFechaExpedicion"]);
                            obj_Resultado.acmi_dFechaExpiracion = Convert.ToDateTime(dr["acmi_dFechaExpiracion"]);
                            obj_Resultado.acmi_vNumeroDocumento = Convert.ToString(dr["acmi_vNumeroDocumento"]);
                            obj_Resultado.acmi_vNumeroDocumentoAnterior = Convert.ToString(dr["acmi_vNumeroDocumentoAnterior"]);
                            obj_Resultado.acmi_vObservaciones = Convert.ToString(dr["acmi_vObservaciones"]);
                            obj_Resultado.acmi_sEstadoId = ToNullInt16(dr["acmi_sEstadoId"]);
                            obj_Resultado.OficinaConsultar = ToNullInt16(dr["actu_sOficinaConsularId"]);
                            obj_Resultado.acmi_sPaisId = ToNullInt16(dr["acmi_sPaisDestinoId"]);
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = new RE_ACTOMIGRATORIO();
                obj_Resultado.Message = exec.Message;
            }
            return obj_Resultado;
        }

        public RE_ACTOMIGRATORIOFORMATO Consultar_Acto_Migratorio_Formato(long acmi_iActoMigratorioId)
        {
            RE_ACTOMIGRATORIOFORMATO obj_Resultado = null;

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIOFORMATO_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amfr_iActoMigratorioId", acmi_iActoMigratorioId));

                        cmd.Connection.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            obj_Resultado = new RE_ACTOMIGRATORIOFORMATO();
                            obj_Resultado.amfr_iActoMigratorioFormatoId = ToNullInt64(dr["amfr_iActoMigratorioFormatoId"]);
                            obj_Resultado.amfr_iActoMigratorioId = ToNullInt64(dr["amfr_iActoMigratorioId"]);
                            obj_Resultado.amfr_sTitularFamiliaId = ToNullInt16(dr["amfr_sTitularFamiliaId"]);
                            obj_Resultado.amfr_vVisaCodificacion = Convert.ToString(dr["amfr_vVisaCodificacion"]);
                            obj_Resultado.amfr_sDiasPermanencia = ToNullInt16(dr["amfr_sDiasPermanencia"]);
                            obj_Resultado.amfr_sPasaporteRevalidarOficinaConsularId = ToNullInt16(dr["amfr_sPasaporteRevalidarOficinaConsularId"]);
                            obj_Resultado.amfr_sPasaporteRevalidarOficinaMigracionId = ToNullInt16(dr["amfr_sPasaporteRevalidarOficinaMigracionId"]);
                            obj_Resultado.amfr_dPasaporteRevalidarFechaExpedicion = ToNullDateTime(dr["amfr_dPasaporteRevalidarFechaExpedicion"]);
                            obj_Resultado.amfr_dPasaporteRevalidarFechaExpiracion = ToNullDateTime(dr["amfr_dPasaporteRevalidarFechaExpiracion"]);

                            obj_Resultado.amfr_sTipoAutorizacionId = ToNullInt16(dr["amfr_sTipoAutorizacionId"]);
                            obj_Resultado.amfr_sTipoDocumentoRREEId = ToNullInt16(dr["amfr_sTipoDocumentoRREEId"]);
                            obj_Resultado.amfr_vNumDocumentoRREE = Convert.ToString(dr["amfr_vNumDocumentoRREE"]);
                            obj_Resultado.amfr_dFechaRREE = ToNullDateTime(dr["amfr_dFechaRREE"]);
                            obj_Resultado.amfr_sTipoDocumentoDIGEMINId = ToNullInt16(dr["amfr_sTipoDocumentoDIGEMINId"]);
                            obj_Resultado.amfr_vNumDocumentoDIGEMIN = Convert.ToString(dr["amfr_vNumDocumentoDIGEMIN"]);

                            obj_Resultado.amfr_dFechaDIGEMIN = ToNullDateTime(dr["amfr_dFechaDIGEMIN"]);
                            obj_Resultado.amfr_vCargoFuncionario = Convert.ToString(dr["amfr_vCargoFuncionario"]);
                            obj_Resultado.amfr_sCargoDiplomaticoId = ToNullInt16(dr["amfr_sCargoDiplomaticoId"]);
                            obj_Resultado.amfr_vMotivoVisaDiplomatica = Convert.ToString(dr["amfr_vMotivoVisaDiplomatica"]);
                            obj_Resultado.amfr_vInstitucionSolicitaVisaDiplomatica = Convert.ToString(dr["amfr_vInstitucionSolicitaVisaDiplomatica"]);
                            obj_Resultado.amfr_vInstitucionRealizaVisaDiplomatica = Convert.ToString(dr["amfr_vInstitucionRealizaVisaDiplomatica"]);
                            obj_Resultado.amfr_sCancilleriaSolicitaAutorizacionId = ToNullInt16(dr["amfr_sCancilleriaSolicitaAutorizacionId"]);
                            obj_Resultado.amfr_vDocumentoAutoriza = Convert.ToString(dr["amfr_vDocumentoAutoriza"]);

                            obj_Resultado.amfr_vMedioComunicacionPrensa = Convert.ToString(dr["amfr_vMedioComunicacionPrensa"]);
                            obj_Resultado.amfr_sCargoPrensaId = ToNullInt16(dr["amfr_sCargoPrensaId"]);
                            obj_Resultado.amfr_vMotivoPrensa = Convert.ToString(dr["amfr_vMotivoPrensa"]);
                            obj_Resultado.amfr_vObservaciones = Convert.ToString(dr["amfr_vObservaciones"]);

                            obj_Resultado.amfr_bAcuerdoProgramaFlag = ToNullBoolean(dr["amfr_bAcuerdoProgramaFlag"]);
                            obj_Resultado.amfr_sTipoNumeroPasaporteId = Convert.ToString(dr["amfr_sTipoNumeroPasaporteId"]);
                            obj_Resultado.amfr_vNumeroPasaporte = Convert.ToString(dr["amfr_vNumeroPasaporte"]);

                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = null;
                obj_Resultado.Message = exec.Message;
            }
            return obj_Resultado;
        }

        public DataTable FormatoMigratorio(Int64 iActuacionDetalleId, Int64 iActoMigratorioId)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {

                    da = new SqlDataAdapter("PN_REPORTES.USP_RP_MIGRATORIO", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = @iActuacionDetalleId;
                    da.SelectCommand.Parameters.Add("@iActoMigratorioId", SqlDbType.BigInt).Value = @iActoMigratorioId;

                    da.Fill(ds, "Formato");
                    dt = ds.Tables["Formato"];

                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable FormatoMigratorio_Lamina(Int64 iActuacionDetalleId, Int64 iActoMigratorioId)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {

                    da = new SqlDataAdapter("PN_REPORTES.USP_RP_MIGRATORIO_LAMINA", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = @iActuacionDetalleId;
                    da.SelectCommand.Parameters.Add("@iActoMigratorioId", SqlDbType.BigInt).Value = @iActoMigratorioId;

                    da.Fill(ds, "Formato");
                    dt = ds.Tables["Formato"];

                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable FormatoMigratorio_Baja(Int64 iActuacionDetalleId, Int64 iActoMigratorioId, Int64 iActoMigratorioHistorico)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {

                    da = new SqlDataAdapter("PN_REPORTES.USP_RP_MIGRATORIO_BAJA", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = @iActuacionDetalleId;
                    da.SelectCommand.Parameters.Add("@iActoMigratorioId", SqlDbType.BigInt).Value = @iActoMigratorioId;
                    da.SelectCommand.Parameters.Add("@amhi_iActoMigratorioHistoricoId", SqlDbType.BigInt).Value = @iActoMigratorioHistorico;

                    da.Fill(ds, "Formato");
                    dt = ds.Tables["Formato"];

                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable FormatoMigratorio_Anulado(Int64 iActuacionDetalleId, Int64 iActoMigratorioId, Int64 iActoMigratorioHistorico)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {

                    da = new SqlDataAdapter("PN_REPORTES.USP_RP_MIGRATORIO_ANULAR", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = @iActuacionDetalleId;
                    da.SelectCommand.Parameters.Add("@iActoMigratorioId", SqlDbType.BigInt).Value = @iActoMigratorioId;
                    da.SelectCommand.Parameters.Add("@amhi_iActoMigratorioHistoricoId", SqlDbType.BigInt).Value = @iActoMigratorioHistorico;

                    da.Fill(ds, "Formato");
                    dt = ds.Tables["Formato"];

                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable ConsultarDocumentosMigratorios(
            BE.MRE.RE_ACTOMIGRATORIO objActoMigratorio, BE.MRE.RE_PERSONA objPersona, 
            int? intOficinaConsularId, int intPaginaActual, int intPaginaCantidad,
            ref int intTotalRegistros,  ref int intTotalPaginas)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (intOficinaConsularId == null) cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", intOficinaConsularId));
                        if (objActoMigratorio.acmi_sTipoDocumentoMigratorioId == 0) cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", objActoMigratorio.acmi_sTipoDocumentoMigratorioId));
                        if (objActoMigratorio.acmi_sTipoId == 0) cmd.Parameters.Add(new SqlParameter("@acmi_sTipoId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_sTipoId", objActoMigratorio.acmi_sTipoId));
                        if (objActoMigratorio.acmi_sSubTipoId == 0) cmd.Parameters.Add(new SqlParameter("@acmi_sSubTipoId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_sSubTipoId", objActoMigratorio.acmi_sSubTipoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumento", objActoMigratorio.acmi_vNumeroDocumento));
                        if (objActoMigratorio.acmi_sEstadoId == 0) cmd.Parameters.Add(new SqlParameter("@acmi_sEstadoId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_sEstadoId", objActoMigratorio.acmi_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroExpediente", objActoMigratorio.acmi_vNumeroExpediente));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", objPersona.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", objPersona.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", objPersona.pers_vNombres));
                        if (objActoMigratorio.acmi_dFechaExpedicion == DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpedicion", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpedicion", objActoMigratorio.acmi_dFechaExpedicion));
                        if (objActoMigratorio.acmi_dFechaExpiracion == DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpiracion", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpiracion", objActoMigratorio.acmi_dFechaExpiracion));
                        if (objActoMigratorio.acmi_dFechaCreacion == DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@acmi_dFechaCreacion", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_dFechaCreacion", objActoMigratorio.acmi_dFechaCreacion));
                        if (objActoMigratorio.acmi_dFechaModificacion == DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@acmi_dFechaCreacionFin", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_dFechaCreacionFin", objActoMigratorio.acmi_dFechaModificacion));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@ITamanioPagina", intPaginaCantidad));
                        #region Output                        
                        SqlParameter parTotalRegistros = cmd.Parameters.Add(new SqlParameter("@ITotalRegistros", SqlDbType.Int));
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPaginas = cmd.Parameters.Add(new SqlParameter("@ITotalPaginas", SqlDbType.Int));
                        parTotalPaginas.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];

                        intTotalRegistros = Convert.ToInt32(parTotalRegistros.Value);
                        intTotalPaginas = Convert.ToInt32(parTotalPaginas.Value);
                    }
                }
            }
            catch (SqlException ex)
            {                
                throw ex;
            }
            return dt;
        }

        public DataTable Consultar_Detalle_Bajas(long acmi_iActoMigratorioId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_CONSULTAR_DETALLE_BAJA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActoMigratorioId ", acmi_iActoMigratorioId));
                        

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
            catch (SqlException ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
