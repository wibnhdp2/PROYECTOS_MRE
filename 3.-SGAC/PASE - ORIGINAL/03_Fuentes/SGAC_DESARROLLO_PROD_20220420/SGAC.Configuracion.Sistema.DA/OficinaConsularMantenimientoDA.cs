using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class OficinaConsularMantenimientoDA
    {
        public string strError { get; set; }

        ~OficinaConsularMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(SI_OFICINACONSULAR pobjBe, ref int intOficinaConsularId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (pobjBe.ofco_vCodigo == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigo", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigo", pobjBe.ofco_vCodigo));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_sCategoriaId", pobjBe.ofco_sCategoriaId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vNombre", pobjBe.ofco_vNombre));
                        

                        if (pobjBe.ofco_vDireccion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDireccion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDireccion", pobjBe.ofco_vDireccion));
                        }

                        if (pobjBe.ofco_vTelefono == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vTelefono", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vTelefono", pobjBe.ofco_vTelefono));
                        }

                        if (pobjBe.ofco_vSiglas == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSiglas", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSiglas", pobjBe.ofco_vSiglas));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_cUbigeoCodigo", pobjBe.ofco_cUbigeoCodigo));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sMonedaId", pobjBe.ofco_sMonedaId));

                        if (pobjBe.ofco_sPorcentajeTipoCambio == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sPorcentajeTipoCambio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sPorcentajeTipoCambio", pobjBe.ofco_sPorcentajeTipoCambio));
                        }

                        if (pobjBe.ofco_cHorarioAtencion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_cHorarioAtencion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_cHorarioAtencion", pobjBe.ofco_cHorarioAtencion));
                        }

                        if (pobjBe.ofco_fDiferenciaHoraria == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_fDiferenciaHoraria", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_fDiferenciaHoraria", pobjBe.ofco_fDiferenciaHoraria));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_sZonaHoraria", pobjBe.ofco_sZonaHoraria));


                        if (pobjBe.ofco_sHorarioVerano == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sHorarioVerano", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sHorarioVerano", pobjBe.ofco_sHorarioVerano));
                        }

                        if (pobjBe.ofco_bASNFlag == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("ofco_bASNFlag", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("ofco_bASNFlag", pobjBe.ofco_bASNFlag));
                        }

                        if (pobjBe.ofco_bJefaturaFlag == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bJefaturaFlag", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bJefaturaFlag", pobjBe.ofco_bJefaturaFlag));
                        }

                        if (pobjBe.ofco_bRemesaLimaFlag == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bRemesaLimaFlag", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bRemesaLimaFlag", pobjBe.ofco_bRemesaLimaFlag));
                        }

                        if (pobjBe.ofco_vDocumentosEmite == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDocumentosEmite", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDocumentosEmite", pobjBe.ofco_vDocumentosEmite));
                        }

                        if (pobjBe.ofco_sReferenciaId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sReferenciaId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sReferenciaId", pobjBe.ofco_sReferenciaId));
                        }

                        if (pobjBe.ofco_vPrivilegiosEspeciales == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vPrivilegiosEspeciales", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vPrivilegiosEspeciales", pobjBe.ofco_vPrivilegiosEspeciales));
                        }

                        if (pobjBe.ofco_vSitioWeb == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSitioWeb", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSitioWeb", pobjBe.ofco_vSitioWeb));
                        }

                        if (pobjBe.ofco_vRangoInicialIP == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoInicialIP", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoInicialIP", pobjBe.ofco_vRangoInicialIP));
                        }

                        if (pobjBe.ofco_vRangoFinIP == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoFinIP", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoFinIP", pobjBe.ofco_vRangoFinIP));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_sUsuarioCreacion", pobjBe.ofco_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vIPCreacion", pobjBe.ofco_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vHostName", Util.ObtenerHostName()));
                        
                        
                        if (pobjBe.ofco_vCodigoLocal == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigoLocal", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigoLocal", pobjBe.ofco_vCodigoLocal));
                        }
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaDependeDe", pobjBe.ofco_sOficinaDependeDe));

                        cmd.Parameters.Add(new SqlParameter("@ofco_nvLatitud", pobjBe.ofco_nvLatitud));
                        cmd.Parameters.Add(new SqlParameter("@ofco_nvLongitud", pobjBe.ofco_nvLongitud));

                        SqlParameter lReturn = cmd.Parameters.Add("@ofco_sOficinaConsularId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        //----------------------------------------------------
                        //Fecha: 27/04/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar al parametro: ofco_bElecciones
                        //Requerimiento solicitado por Rita Huambachano.
                        //----------------------------------------------------

                        if (pobjBe.ofco_bElecciones == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bElecciones", 0));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bElecciones", pobjBe.ofco_bElecciones));
                        }
                        //----------------------------------------------------
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intOficinaConsularId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }

        public void UpdateDependent(SI_OFICINACONSULAR pobjOfcDependiente)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_ACTUALIZAR_DEPENDIENTES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", pobjOfcDependiente.ofco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sReferencia", pobjOfcDependiente.ofco_sReferenciaId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sUsuarioModificacion", pobjOfcDependiente.ofco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vIPModificacion", pobjOfcDependiente.ofco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("vHostname", Util.ObtenerHostName()));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                strError = ex.StackTrace.ToString();
            }
        }

        public void DeleteDependent(SI_OFICINACONSULAR pobjOfcDependiente)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_ELIMINAR_DEPENDIENTES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", pobjOfcDependiente.ofco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sUsuarioModificacion", pobjOfcDependiente.ofco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vIPModificacion", pobjOfcDependiente.ofco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("vHostname", Util.ObtenerHostName()));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                strError = ex.StackTrace.ToString();
            }
        }


        public void Update(SI_OFICINACONSULAR pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", pobjBe.ofco_sOficinaConsularId));

                        if (pobjBe.ofco_vCodigo == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigo", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigo", pobjBe.ofco_vCodigo));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_sCategoriaId", pobjBe.ofco_sCategoriaId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vNombre", pobjBe.ofco_vNombre));
                       

                        if (pobjBe.ofco_vDireccion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDireccion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDireccion", pobjBe.ofco_vDireccion));
                        }

                        if (pobjBe.ofco_vTelefono == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vTelefono", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vTelefono", pobjBe.ofco_vTelefono));
                        }

                        if (pobjBe.ofco_vSiglas == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSiglas", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSiglas", pobjBe.ofco_vSiglas));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_cUbigeoCodigo", pobjBe.ofco_cUbigeoCodigo));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sMonedaId", pobjBe.ofco_sMonedaId));

                        if (pobjBe.ofco_sPorcentajeTipoCambio == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sPorcentajeTipoCambio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sPorcentajeTipoCambio", pobjBe.ofco_sPorcentajeTipoCambio));
                        }

                        if (pobjBe.ofco_cHorarioAtencion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_cHorarioAtencion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_cHorarioAtencion", pobjBe.ofco_cHorarioAtencion));
                        }

                        if (pobjBe.ofco_fDiferenciaHoraria == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_fDiferenciaHoraria", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_fDiferenciaHoraria", pobjBe.ofco_fDiferenciaHoraria));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_sZonaHoraria", pobjBe.ofco_sZonaHoraria));

                        if (pobjBe.ofco_sHorarioVerano == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sHorarioVerano", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sHorarioVerano", pobjBe.ofco_sHorarioVerano));
                        }

                        if (Convert.ToBoolean(pobjBe.ofco_bASNFlag) == false)
                        {
                            cmd.Parameters.Add(new SqlParameter("ofco_bASNFlag", false));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("ofco_bASNFlag", pobjBe.ofco_bASNFlag));
                        }

                        if (Convert.ToBoolean(pobjBe.ofco_bJefaturaFlag) == false)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bJefaturaFlag", false));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bJefaturaFlag", pobjBe.ofco_bJefaturaFlag));
                        }

                        if (Convert.ToBoolean(pobjBe.ofco_bRemesaLimaFlag) == false)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bRemesaLimaFlag", false));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bRemesaLimaFlag", pobjBe.ofco_bRemesaLimaFlag));
                        }

                        if (pobjBe.ofco_vDocumentosEmite == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDocumentosEmite", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vDocumentosEmite", pobjBe.ofco_vDocumentosEmite));
                        }

                        if (pobjBe.ofco_sReferenciaId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sReferenciaId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sReferenciaId", pobjBe.ofco_sReferenciaId));
                        }

                        if (pobjBe.ofco_vPrivilegiosEspeciales == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vPrivilegiosEspeciales", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vPrivilegiosEspeciales", pobjBe.ofco_vPrivilegiosEspeciales));
                        }

                        if (pobjBe.ofco_vSitioWeb == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSitioWeb", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vSitioWeb", pobjBe.ofco_vSitioWeb));
                        }

                        if (pobjBe.ofco_vRangoInicialIP == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoInicialIP", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoInicialIP", pobjBe.ofco_vRangoInicialIP));
                        }

                        if (pobjBe.ofco_vRangoFinIP == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoFinIP", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vRangoFinIP", pobjBe.ofco_vRangoFinIP));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ofco_nvLatitud", pobjBe.ofco_nvLatitud));
                        cmd.Parameters.Add(new SqlParameter("@ofco_nvLongitud", pobjBe.ofco_nvLongitud));

                        cmd.Parameters.Add(new SqlParameter("@ofco_sUsuarioModificacion", pobjBe.ofco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vIPModificacion", pobjBe.ofco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vHostName", Util.ObtenerHostName()));


                        if (pobjBe.ofco_vCodigoLocal == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigoLocal", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vCodigoLocal", pobjBe.ofco_vCodigoLocal));
                        }
                        cmd.Parameters.Add(new SqlParameter("@ofco_cEstado", pobjBe.ofco_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaDependeDe", pobjBe.ofco_sOficinaDependeDe));

                        //----------------------------------------------------
                        //Fecha: 27/04/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar al parametro: ofco_bElecciones
                        //Requerimiento solicitado por Rita Huambachano.
                        //----------------------------------------------------

                        if (Convert.ToBoolean(pobjBe.ofco_bElecciones) == false)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bElecciones", false));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_bElecciones", pobjBe.ofco_bElecciones));
                        }
                        //----------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }

        public void Delete(SI_OFICINACONSULAR pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", pobjBe.ofco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sUsuarioModificacion", pobjBe.ofco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vIPModificacion", pobjBe.ofco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }

        public void RegistrarMoneda(Int16 ofmo_sOficinaMonedaId, Int16 ofco_sOficinaConsularId, Int16 intMoneda, DateTime dFechaInicio, DateTime dFechaFin, Int16 UsuarioCreacion, string vIP,
            ref string resultado, ref bool Error)
        {
            Error = true;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINAMONEDA_REGISTRAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ofmo_sOficinaMonedaId", ofmo_sOficinaMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", ofco_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_sMonedaId", intMoneda));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_dFechaInicio", dFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_dFechaFin", dFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_sUsuarioCreacion", UsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_vIpCreacion", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@resultado", SqlDbType.VarChar,150);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        Error = false;
                        resultado = Convert.ToString(lReturn.Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }


        public void EliminarMoneda(Int16 ofmo_sOficinaMonedaId, Int16 UsuarioCreacion, string vIP,
            ref bool Error)
        {
            Error = true;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINAMONEDA_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ofmo_sOficinaMonedaId", ofmo_sOficinaMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_sUsuarioCreacion", UsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofmo_vIpCreacion", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }
    }
}
