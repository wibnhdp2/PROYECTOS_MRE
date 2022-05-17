using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Persona.BL;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoCivilMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActoCivilMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoCivilMantenimientoDA()
        {
            GC.Collect();
        }

        public int Insertar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE,
                            int IntOficinaConsularId,
                            ref long LonRegCivId)
        {
            PersonaMantenimientoBL ObjPersonaBL = new PersonaMantenimientoBL();
            RE_PERSONA ObjPersBE = new RE_PERSONA();
            RE_PERSONAIDENTIFICACION ObjPersIdentBE = new RE_PERSONAIDENTIFICACION();
            RE_REGISTROUNICO ObjRegistroUnicoBE = new RE_REGISTROUNICO();

            int intResultado = 0;

            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_ADICIONAR", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        String strHostName = Util.ObtenerHostName();

                        cmd.Parameters.Add("@reci_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjRegCivBE.reci_iActuacionDetalleId;

                        cmd.Parameters.Add("@reci_sTipoActaId", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sTipoActaId;

                        if (ObjRegCivBE.reci_vNumeroCUI == null) { cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar,20).Value = DBNull.Value; }
                        else if (ObjRegCivBE.reci_vNumeroCUI.Length == 0) { cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar, 20).Value = DBNull.Value; }
                        else { cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar, 20).Value = ObjRegCivBE.reci_vNumeroCUI; }

                        if (ObjRegCivBE.reci_vNumeroActa == null) { cmd.Parameters.Add("@reci_vNumeroActa", SqlDbType.VarChar, 20).Value = DBNull.Value; }
                        else if (ObjRegCivBE.reci_vNumeroActa.Length == 0) { cmd.Parameters.Add("@reci_vNumeroActa", SqlDbType.VarChar, 20).Value = DBNull.Value; }
                        else { cmd.Parameters.Add("@reci_vNumeroActa", SqlDbType.VarChar, 20).Value = ObjRegCivBE.reci_vNumeroActa; }

                        cmd.Parameters.Add("@reci_dFechaRegistro", SqlDbType.DateTime).Value = ObjRegCivBE.reci_dFechaRegistro;


                        if (ObjRegCivBE.reci_cOficinaRegistralUbigeo == null) { cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char,6).Value = DBNull.Value; }
                        else if (ObjRegCivBE.reci_cOficinaRegistralUbigeo == string.Empty) { cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char,6).Value = DBNull.Value; }
                        else if (ObjRegCivBE.reci_cOficinaRegistralUbigeo == "000000") { cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char,6).Value = DBNull.Value; }
                        else { cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char,6).Value = ObjRegCivBE.reci_cOficinaRegistralUbigeo; }


                      
                        if (ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId == null) cmd.Parameters.Add("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                        else if (ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId == 0) cmd.Parameters.Add("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                        else cmd.Parameters.Add("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int).Value = ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId;

                        cmd.Parameters.Add("@reci_dFechaHoraOcurrenciaActo", SqlDbType.DateTime).Value = ObjRegCivBE.reci_dFechaHoraOcurrenciaActo;

                        cmd.Parameters.Add("@reci_sOcurrenciaTipoId", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sOcurrenciaTipoId;

                        if (ObjRegCivBE.reci_vOcurrenciaLugar != null) cmd.Parameters.Add("@reci_vOcurrenciaLugar", SqlDbType.VarChar, 70).Value = ObjRegCivBE.reci_vOcurrenciaLugar;
                        else cmd.Parameters.Add("@reci_vOcurrenciaLugar", SqlDbType.VarChar).Value = String.Empty;

                        if (ObjRegCivBE.reci_cOcurrenciaUbigeo == null) cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char,6).Value = DBNull.Value;
                        else if (ObjRegCivBE.reci_cOcurrenciaUbigeo == String.Empty) cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char,6).Value = DBNull.Value;
                        else if (ObjRegCivBE.reci_cOcurrenciaUbigeo == "000000")  cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char,6).Value = DBNull.Value;
                        else cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char,6).Value = ObjRegCivBE.reci_cOcurrenciaUbigeo;

                        
                        if (ObjRegCivBE.reci_IOcurrenciaCentroPobladoId == 0) cmd.Parameters.Add("@reci_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                        else cmd.Parameters.Add("@reci_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = ObjRegCivBE.reci_IOcurrenciaCentroPobladoId;

                        if (ObjRegCivBE.reci_vNumeroExpedienteMatrimonio != null) cmd.Parameters.Add("@reci_vNumeroExpedienteMatrimonio", SqlDbType.VarChar, 100).Value = ObjRegCivBE.reci_vNumeroExpedienteMatrimonio;
                        else cmd.Parameters.Add("@reci_vNumeroExpedienteMatrimonio", SqlDbType.VarChar, 100).Value = DBNull.Value;


                        if (ObjRegCivBE.reci_IAprobacionUsuarioId != null)
                        {
                            if (ObjRegCivBE.reci_IAprobacionUsuarioId == 0) cmd.Parameters.Add("@reci_IAprobacionUsuarioId", SqlDbType.Int).Value = DBNull.Value;
                            else cmd.Parameters.Add("@reci_IAprobacionUsuarioId", SqlDbType.Int).Value = ObjRegCivBE.reci_IAprobacionUsuarioId;
                        }
                        else cmd.Parameters.Add("@reci_IAprobacionUsuarioId", SqlDbType.Int).Value = DBNull.Value;


                        if (ObjRegCivBE.reci_vIPAprobacion != null)
                        {
                            if (ObjRegCivBE.reci_vIPAprobacion.Length == 0) cmd.Parameters.Add("@reci_vIPAprobacion", SqlDbType.VarChar, 50).Value = DBNull.Value;
                            else cmd.Parameters.Add("@reci_vIPAprobacion", SqlDbType.VarChar, 50).Value = ObjRegCivBE.reci_vIPAprobacion;
                        }
                        else cmd.Parameters.Add("@reci_vIPAprobacion", SqlDbType.VarChar, 50).Value = DBNull.Value;

                        
                        if (!ObjRegCivBE.reci_dFechaAprobacion.Equals(System.DateTime.MinValue)) cmd.Parameters.Add("@reci_dFechaAprobacion", SqlDbType.DateTime).Value = ObjRegCivBE.reci_dFechaAprobacion;
                        else cmd.Parameters.Add("@reci_dFechaAprobacion", SqlDbType.DateTime).Value = DBNull.Value;

                         
                        if (ObjRegCivBE.reci_bDigitalizadoFlag != null) cmd.Parameters.Add("@reci_bDigitalizadoFlag", SqlDbType.Bit).Value = ObjRegCivBE.reci_bDigitalizadoFlag;
                        else cmd.Parameters.Add("@reci_bDigitalizadoFlag", SqlDbType.Bit).Value = 0;

                     
                        if (ObjRegCivBE.reci_vCargoCelebrante != null) cmd.Parameters.Add("@reci_vCargoCelebrante", SqlDbType.VarChar).Value = ObjRegCivBE.reci_vCargoCelebrante;
                        else cmd.Parameters.Add("@reci_vCargoCelebrante", SqlDbType.VarChar).Value = DBNull.Value;

                        
                        if (ObjRegCivBE.reci_vLibro == null)
                            cmd.Parameters.Add("@reci_vLibro", SqlDbType.VarChar).Value = DBNull.Value;
                        else if (ObjRegCivBE.reci_vLibro.Length == 0)
                            cmd.Parameters.Add("@reci_vLibro", SqlDbType.VarChar).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@reci_vLibro", SqlDbType.VarChar).Value = ObjRegCivBE.reci_vLibro;

                        if (ObjRegCivBE.reci_bAnotacionFlag != null)
                            cmd.Parameters.Add("@reci_bAnotacionFlag", SqlDbType.Bit).Value = ObjRegCivBE.reci_bAnotacionFlag;
                        else
                            cmd.Parameters.Add("@reci_bAnotacionFlag", SqlDbType.Bit).Value = 0;

                        if (ObjRegCivBE.reci_vObservaciones == null) cmd.Parameters.Add("@reci_vObservaciones", SqlDbType.VarChar,300).Value = DBNull.Value;
                        else  cmd.Parameters.Add("@reci_vObservaciones", SqlDbType.VarChar,300).Value = ObjRegCivBE.reci_vObservaciones;

                        cmd.Parameters.Add("@reci_sUsuarioCreacion", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sUsuarioCreacion;
                        cmd.Parameters.Add("@reci_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjRegCivBE.reci_vIPCreacion;
                        cmd.Parameters.Add("@reci_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@reci_vHostName", SqlDbType.VarChar, 20).Value = strHostName;

                        cmd.Parameters.Add("@reci_cConCUI", SqlDbType.Char, 1).Value = ObjRegCivBE.reci_cConCUI;
                        cmd.Parameters.Add("@reci_cReconocimientoAdopcion", SqlDbType.Char, 1).Value = ObjRegCivBE.reci_cReconocimientoAdopcion;
                        cmd.Parameters.Add("@reci_cReconstitucionReposicion", SqlDbType.Char, 1).Value = ObjRegCivBE.reci_cReconstitucionReposicion;

                        if (ObjRegCivBE.reci_iNumeroActaAnterior != null)
                        {
                            cmd.Parameters.Add("@reci_iNumeroActaAnterior", SqlDbType.Int, 1).Value = ObjRegCivBE.reci_iNumeroActaAnterior;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_iNumeroActaAnterior", SqlDbType.Int, 1).Value = null;
                        }
                        cmd.Parameters.Add("@reci_vTitular", SqlDbType.VarChar, 200).Value = ObjRegCivBE.reci_vTitular;

                        if (ObjRegCivBE.reci_bInscripcionOficio != null)
                            cmd.Parameters.Add("@reci_bInscripcionOficio", SqlDbType.Bit).Value = ObjRegCivBE.reci_bInscripcionOficio;
                        else
                            cmd.Parameters.Add("@reci_bInscripcionOficio", SqlDbType.Bit).Value = 0;


                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@reci_iRegistroCivilId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        LonRegCivId = Convert.ToInt64(lReturn.Value); 
                        intResultado = 1;

                        #endregion
                    }
                }
                 
            }
            catch (SqlException exec)
            {
                intResultado = -1;
                ObjRegCivBE.Error = true;
                ObjRegCivBE.Message = "ERROR:" + exec.Message.ToString() + "TRACE:" + exec.StackTrace.ToString();

            }
            catch (Exception exec)
            {
                intResultado = -1;
                ObjRegCivBE.Error = true;
                ObjRegCivBE.Message = "ERROR:" + exec.Message.ToString() + "TRACE:" + exec.StackTrace.ToString();

            }
            return intResultado;
        }

        //public int Actualizar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE,
        //                      int IntOficinaConsularId,
        //                      DataTable DtRegDirecciones)
        //{
        //    PersonaMantenimientoBL ObjPersonaBL = new PersonaMantenimientoBL();
        //    RE_PERSONA ObjPersBE = new RE_PERSONA();
        //    RE_PERSONAIDENTIFICACION ObjPersIdentBE = new RE_PERSONAIDENTIFICACION();
        //    RE_REGISTROUNICO ObjRegistroUnicoBE = new RE_REGISTROUNICO();

        //    int IntResultQuery = 0;
        //    int intResultado = 0;

        //    try
        //    {
        //        #region Registro Civil actualización ...

        //        SqlParameter[] prmParameter = new SqlParameter[31];

        //        prmParameter[0] = new SqlParameter("@reci_iRegistroCivilId", SqlDbType.BigInt);
        //        prmParameter[0].Value = ObjRegCivBE.reci_iRegistroCivilId;

        //        prmParameter[1] = new SqlParameter("@reci_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[1].Value = ObjRegCivBE.reci_iActuacionDetalleId;

        //        prmParameter[2] = new SqlParameter("@reci_sTipoActaId", SqlDbType.SmallInt);
        //        prmParameter[2].Value = ObjRegCivBE.reci_sTipoActaId;

        //        prmParameter[3] = new SqlParameter("@reci_vNumeroCUI", SqlDbType.VarChar, 20);
        //        if (ObjRegCivBE.reci_vNumeroCUI != null)
        //        {
        //            if (ObjRegCivBE.reci_vNumeroCUI.Length == 0)
        //                prmParameter[3].Value = DBNull.Value;
        //            else
        //                prmParameter[3].Value = ObjRegCivBE.reci_vNumeroCUI;
        //        }
        //        else
        //        {
        //            prmParameter[3].Value = DBNull.Value;
        //        }

        //        prmParameter[4] = new SqlParameter("@reci_vNumeroActa", SqlDbType.VarChar);
        //        prmParameter[4].Value = ObjRegCivBE.reci_vNumeroActa;

        //        prmParameter[5] = new SqlParameter("@reci_dFechaRegistro", SqlDbType.DateTime);
        //        prmParameter[5].Value = ObjRegCivBE.reci_dFechaRegistro;

        //        prmParameter[6] = new SqlParameter("@reci_cOficinaRegistralUbigeo", SqlDbType.Char, 6);
        //        if (ObjRegCivBE.reci_cOficinaRegistralUbigeo != null)
        //        {
        //            if (ObjRegCivBE.reci_cOficinaRegistralUbigeo == "000000")
        //                prmParameter[6].Value = DBNull.Value;
        //            else
        //                prmParameter[6].Value = ObjRegCivBE.reci_cOficinaRegistralUbigeo;
        //        }
        //        else
        //        {
        //            prmParameter[6].Value = DBNull.Value;
        //        }

        //        prmParameter[7] = new SqlParameter("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int);
        //        if (ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId != null)
        //        {
        //            if (ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId == 0)
        //                prmParameter[7].Value = DBNull.Value;
        //            else
        //                prmParameter[7].Value = ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId;
        //        }
        //        else
        //        {
        //            prmParameter[7].Value = DBNull.Value;
        //        }

        //        prmParameter[8] = new SqlParameter("@reci_dFechaHoraOcurrenciaActo", SqlDbType.DateTime);
        //        if (ObjRegCivBE.reci_dFechaHoraOcurrenciaActo != null)
        //        {
        //            if (ObjRegCivBE.reci_dFechaHoraOcurrenciaActo != DateTime.MinValue)
        //                prmParameter[8].Value = ObjRegCivBE.reci_dFechaHoraOcurrenciaActo;
        //            else
        //                prmParameter[8].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            prmParameter[8].Value = DBNull.Value;
        //        }

        //        prmParameter[9] = new SqlParameter("@reci_sOcurrenciaTipoId", SqlDbType.SmallInt);
        //        if (ObjRegCivBE.reci_sOcurrenciaTipoId != null)
        //        {
        //            if (ObjRegCivBE.reci_sOcurrenciaTipoId != 0)
        //                prmParameter[9].Value = ObjRegCivBE.reci_sOcurrenciaTipoId;
        //            else
        //                prmParameter[9].Value = DBNull.Value;
        //        }
        //        else
        //            prmParameter[9].Value = DBNull.Value;

        //        prmParameter[10] = new SqlParameter("@reci_vOcurrenciaLugar", SqlDbType.VarChar, 70);
        //        if (ObjRegCivBE.reci_vOcurrenciaLugar != null)
        //        {
        //            prmParameter[10].Value = ObjRegCivBE.reci_vOcurrenciaLugar;
        //        }
        //        else
        //        {
        //            prmParameter[10].Value = DBNull.Value;
        //        }

        //        prmParameter[11] = new SqlParameter("@reci_cOcurrenciaUbigeo", SqlDbType.Char, 6);
        //        if (ObjRegCivBE.reci_cOcurrenciaUbigeo != null)
        //        {
        //            if (ObjRegCivBE.reci_cOcurrenciaUbigeo != string.Empty)
        //            {
        //                if (Convert.ToInt64(ObjRegCivBE.reci_cOcurrenciaUbigeo) == 0)
        //                    prmParameter[11].Value = DBNull.Value;
        //                else
        //                    prmParameter[11].Value = ObjRegCivBE.reci_cOcurrenciaUbigeo;
        //            }
        //            else
        //                prmParameter[11].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            prmParameter[11].Value = DBNull.Value;
        //        }

        //        prmParameter[12] = new SqlParameter("@reci_IOcurrenciaCentroPobladoId", SqlDbType.Int);
        //        if (ObjRegCivBE.reci_IOcurrenciaCentroPobladoId != null)
        //        {
        //            if (ObjRegCivBE.reci_IOcurrenciaCentroPobladoId == 0)
        //                prmParameter[12].Value = DBNull.Value;
        //            else
        //                prmParameter[12].Value = ObjRegCivBE.reci_IOcurrenciaCentroPobladoId;
        //        }
        //        else
        //        {
        //            prmParameter[12].Value = DBNull.Value;
        //        }
        //        if (ObjRegCivBE.reci_vNumeroExpedienteMatrimonio == null) { ObjRegCivBE.reci_vNumeroExpedienteMatrimonio = ""; }
        //        if (ObjRegCivBE.reci_vNumeroExpedienteMatrimonio.Length == 0)
        //        {
        //            prmParameter[13] = new SqlParameter("@reci_vNumeroExpedienteMatrimonio", SqlDbType.VarChar, 100);
        //            prmParameter[13].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            prmParameter[13] = new SqlParameter("@reci_vNumeroExpedienteMatrimonio", SqlDbType.VarChar, 100);
        //            prmParameter[13].Value = ObjRegCivBE.reci_vNumeroExpedienteMatrimonio;
        //        }

        //        prmParameter[14] = new SqlParameter("@reci_IAprobacionUsuarioId", SqlDbType.Int);
        //        if (ObjRegCivBE.reci_IAprobacionUsuarioId != null)
        //        {
        //            if (ObjRegCivBE.reci_IAprobacionUsuarioId == 0)
        //                prmParameter[14].Value = DBNull.Value;
        //            else
        //                prmParameter[14].Value = ObjRegCivBE.reci_IAprobacionUsuarioId;
        //        }
        //        else
        //        {
        //            prmParameter[14].Value = DBNull.Value;
        //        }

        //        prmParameter[15] = new SqlParameter("@reci_vIPAprobacion", SqlDbType.VarChar, 50);
        //        if (ObjRegCivBE.reci_vIPAprobacion != null)
        //        {
        //            if (ObjRegCivBE.reci_vIPAprobacion.Length == 0)
        //                prmParameter[15].Value = DBNull.Value;
        //            else
        //                prmParameter[15].Value = ObjRegCivBE.reci_vIPAprobacion;
        //        }
        //        else
        //            prmParameter[15].Value = DBNull.Value;

        //        prmParameter[16] = new SqlParameter("@reci_dFechaAprobacion", SqlDbType.DateTime);
        //        if (ObjRegCivBE.reci_dFechaAprobacion != null)
        //        {
        //            if (!ObjRegCivBE.reci_dFechaAprobacion.Equals(System.DateTime.MinValue))
        //                prmParameter[16].Value = ObjRegCivBE.reci_dFechaAprobacion;
        //            else
        //                prmParameter[16].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            prmParameter[16].Value = DBNull.Value;
        //        }

        //        prmParameter[17] = new SqlParameter("@reci_bDigitalizadoFlag", SqlDbType.Bit);
        //        if (ObjRegCivBE.reci_bDigitalizadoFlag != null)
        //            prmParameter[17].Value = ObjRegCivBE.reci_bDigitalizadoFlag;
        //        else
        //            prmParameter[17].Value = DBNull.Value;

        //        if (ObjRegCivBE.reci_vCargoCelebrante == null) { ObjRegCivBE.reci_vCargoCelebrante = ""; }
        //        if (ObjRegCivBE.reci_vCargoCelebrante.Length == 0)
        //        {
        //            prmParameter[18] = new SqlParameter("@reci_vCargoCelebrante", SqlDbType.VarChar, 100);
        //            prmParameter[18].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            prmParameter[18] = new SqlParameter("@reci_vCargoCelebrante", SqlDbType.VarChar, 100);
        //            prmParameter[18].Value = ObjRegCivBE.reci_vCargoCelebrante;
        //        }

        //        prmParameter[19] = new SqlParameter("@reci_vLibro", SqlDbType.VarChar, 50);
        //        if (ObjRegCivBE.reci_vLibro != null)
        //        {
        //            if (ObjRegCivBE.reci_vLibro.Length == 0)
        //            {
        //                prmParameter[19].Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                prmParameter[19].Value = ObjRegCivBE.reci_vLibro;
        //            }
        //        }
        //        else
        //        {
        //            prmParameter[19].Value = DBNull.Value;
        //        }

        //        prmParameter[20] = new SqlParameter("@reci_bAnotacionFlag", SqlDbType.Bit);
        //        prmParameter[20].Value = ObjRegCivBE.reci_bAnotacionFlag;

        //        if (ObjRegCivBE.reci_vObservaciones == null) { ObjRegCivBE.reci_vObservaciones = ""; }
        //        if (ObjRegCivBE.reci_vObservaciones.Length == 0)
        //        {
        //            prmParameter[21] = new SqlParameter("@reci_vObservaciones", SqlDbType.Text);
        //            prmParameter[21].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            prmParameter[21] = new SqlParameter("@reci_vObservaciones", SqlDbType.Text);
        //            prmParameter[21].Value = ObjRegCivBE.reci_vObservaciones;
        //        }

        //        prmParameter[22] = new SqlParameter("@reci_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameter[22].Value = ObjRegCivBE.reci_sUsuarioModificacion;

        //        prmParameter[23] = new SqlParameter("@reci_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameter[23].Value = ObjRegCivBE.reci_vIPModificacion;

        //        prmParameter[24] = new SqlParameter("@reci_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameter[24].Value = IntOficinaConsularId;

        //        prmParameter[25] = new SqlParameter("@reci_vHostName", SqlDbType.VarChar, 20);
        //        prmParameter[25].Value = Util.ObtenerHostName();

        //        prmParameter[26] = new SqlParameter("@reci_cConCUI", SqlDbType.Char, 1);
        //        prmParameter[26].Value = ObjRegCivBE.reci_cConCUI;

        //        prmParameter[27] = new SqlParameter("@reci_cReconocimientoAdopcion", SqlDbType.Char, 1);
        //        prmParameter[27].Value = ObjRegCivBE.reci_cReconocimientoAdopcion;

        //        prmParameter[28] = new SqlParameter("@reci_cReconstitucionReposicion", SqlDbType.Char, 1);
        //        prmParameter[28].Value = ObjRegCivBE.reci_cReconstitucionReposicion;

        //        prmParameter[29] = new SqlParameter("@reci_iNumeroActaAnterior", SqlDbType.Int);
        //        if (ObjRegCivBE.reci_iNumeroActaAnterior != null)
        //        {
        //            prmParameter[29].Value = ObjRegCivBE.reci_iNumeroActaAnterior;
        //        }
        //        else
        //        {
        //            prmParameter[29].Value = null;
        //        }

        //        prmParameter[30] = new SqlParameter("@reci_vTitular", SqlDbType.VarChar, 200);
        //        prmParameter[30].Value = ObjRegCivBE.reci_vTitular;


        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                   CommandType.StoredProcedure,
        //                                                   "PN_REGISTRO.USP_RE_REGISTROCIVIL_ACTUALIZAR",
        //                                                   prmParameter);

        //        #endregion Registro Civil actualización ...

        //        long LonRegCivId = ObjRegCivBE.reci_iRegistroCivilId;

        //        intResultado = 1;
        //    }
        
        //    catch (SqlException ex)
        //    {
        //        intResultado = -1;
        //        ObjRegCivBE.Error = true;
        //        ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        intResultado = -1;
        //        ObjRegCivBE.Error = true;
        //        ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
        //        throw ex;
        //    }
        //    return intResultado;
        //}
        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public int Actualizar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE,
                              int IntOficinaConsularId,
                              DataTable DtRegDirecciones)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            PersonaMantenimientoBL ObjPersonaBL = new PersonaMantenimientoBL();
            RE_PERSONA ObjPersBE = new RE_PERSONA();
            RE_PERSONAIDENTIFICACION ObjPersIdentBE = new RE_PERSONAIDENTIFICACION();
            RE_REGISTROUNICO ObjRegistroUnicoBE = new RE_REGISTROUNICO();

            int intResultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@reci_iRegistroCivilId", SqlDbType.BigInt).Value = ObjRegCivBE.reci_iRegistroCivilId;
                        cmd.Parameters.Add("@reci_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjRegCivBE.reci_iActuacionDetalleId;
                        cmd.Parameters.Add("@reci_sTipoActaId", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sTipoActaId;

                        if (ObjRegCivBE.reci_vNumeroCUI != null)
                        {
                            if (ObjRegCivBE.reci_vNumeroCUI.Length == 0)
                            {
                                cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar, 20).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar, 20).Value = ObjRegCivBE.reci_vNumeroCUI;
                            }
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar, 20).Value = DBNull.Value;
                        }
                        cmd.Parameters.Add("@reci_vNumeroActa", SqlDbType.VarChar, 10).Value = ObjRegCivBE.reci_vNumeroActa;
                        cmd.Parameters.Add("@reci_dFechaRegistro", SqlDbType.DateTime).Value = ObjRegCivBE.reci_dFechaRegistro;

                        if (ObjRegCivBE.reci_cOficinaRegistralUbigeo != null)
                        {
                            if (ObjRegCivBE.reci_cOficinaRegistralUbigeo == "000000")
                                cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char, 6).Value = ObjRegCivBE.reci_cOficinaRegistralUbigeo;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_cOficinaRegistralUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                        }

                        if (ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId != null)
                        {
                            if (ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId == 0)
                                cmd.Parameters.Add("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int).Value = ObjRegCivBE.reci_IOficinaRegistralCentroPobladoId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_IOficinaRegistralCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                        }

                        if (ObjRegCivBE.reci_dFechaHoraOcurrenciaActo != null)
                        {
                            if (ObjRegCivBE.reci_dFechaHoraOcurrenciaActo != DateTime.MinValue)
                                cmd.Parameters.Add("@reci_dFechaHoraOcurrenciaActo", SqlDbType.DateTime).Value = ObjRegCivBE.reci_dFechaHoraOcurrenciaActo;
                            else
                                cmd.Parameters.Add("@reci_dFechaHoraOcurrenciaActo", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_dFechaHoraOcurrenciaActo", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_sOcurrenciaTipoId != null)
                        {
                            if (ObjRegCivBE.reci_sOcurrenciaTipoId != 0)
                                cmd.Parameters.Add("@reci_sOcurrenciaTipoId", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sOcurrenciaTipoId;
                            else
                                cmd.Parameters.Add("@reci_sOcurrenciaTipoId", SqlDbType.SmallInt).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_sOcurrenciaTipoId", SqlDbType.SmallInt).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_vOcurrenciaLugar != null)
                        {
                            cmd.Parameters.Add("@reci_vOcurrenciaLugar", SqlDbType.VarChar, 70).Value = ObjRegCivBE.reci_vOcurrenciaLugar;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vOcurrenciaLugar", SqlDbType.VarChar, 70).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_cOcurrenciaUbigeo != null)
                        {
                            if (ObjRegCivBE.reci_cOcurrenciaUbigeo != string.Empty)
                            {
                                if (Convert.ToInt64(ObjRegCivBE.reci_cOcurrenciaUbigeo) == 0)
                                    cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                                else
                                    cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char, 6).Value = ObjRegCivBE.reci_cOcurrenciaUbigeo;
                            }
                            else
                                cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_cOcurrenciaUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_IOcurrenciaCentroPobladoId != null)
                        {
                            if (ObjRegCivBE.reci_IOcurrenciaCentroPobladoId == 0)
                                cmd.Parameters.Add("@reci_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@reci_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = ObjRegCivBE.reci_IOcurrenciaCentroPobladoId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_vNumeroExpedienteMatrimonio == null) { ObjRegCivBE.reci_vNumeroExpedienteMatrimonio = ""; }
                        if (ObjRegCivBE.reci_vNumeroExpedienteMatrimonio.Length == 0)
                        {
                            cmd.Parameters.Add("@reci_vNumeroExpedienteMatrimonio", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vNumeroExpedienteMatrimonio", SqlDbType.VarChar, 100).Value = ObjRegCivBE.reci_vNumeroExpedienteMatrimonio;
                        }
                        if (ObjRegCivBE.reci_IAprobacionUsuarioId != null)
                        {
                            if (ObjRegCivBE.reci_IAprobacionUsuarioId == 0)
                                cmd.Parameters.Add("@reci_IAprobacionUsuarioId", SqlDbType.Int).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@reci_IAprobacionUsuarioId", SqlDbType.Int).Value = ObjRegCivBE.reci_IAprobacionUsuarioId;
                        }
                        if (ObjRegCivBE.reci_vIPAprobacion != null)
                        {
                            if (ObjRegCivBE.reci_vIPAprobacion.Length == 0)
                                cmd.Parameters.Add("@reci_vIPAprobacion", SqlDbType.VarChar, 50).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@reci_vIPAprobacion", SqlDbType.VarChar, 50).Value = ObjRegCivBE.reci_vIPAprobacion;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vIPAprobacion", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_dFechaAprobacion != null)
                        {
                            if (!ObjRegCivBE.reci_dFechaAprobacion.Equals(System.DateTime.MinValue))
                                cmd.Parameters.Add("@reci_dFechaAprobacion", SqlDbType.DateTime).Value = ObjRegCivBE.reci_dFechaAprobacion;
                            else
                                cmd.Parameters.Add("@reci_dFechaAprobacion", SqlDbType.DateTime).Value = DBNull.Value;

                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_dFechaAprobacion", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_bDigitalizadoFlag != null)
                        {
                            cmd.Parameters.Add("@reci_bDigitalizadoFlag", SqlDbType.Bit).Value = ObjRegCivBE.reci_bDigitalizadoFlag;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_bDigitalizadoFlag", SqlDbType.Bit).Value = DBNull.Value;
                        }
                        if (ObjRegCivBE.reci_vCargoCelebrante == null) { ObjRegCivBE.reci_vCargoCelebrante = ""; }
                        if (ObjRegCivBE.reci_vCargoCelebrante.Length == 0)
                        {
                            cmd.Parameters.Add("@reci_vCargoCelebrante", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vCargoCelebrante", SqlDbType.VarChar, 100).Value = ObjRegCivBE.reci_vCargoCelebrante;
                        }
                        if (ObjRegCivBE.reci_vLibro != null)
                        {
                            if (ObjRegCivBE.reci_vLibro.Length == 0)
                            {
                                cmd.Parameters.Add("@reci_vLibro", SqlDbType.VarChar, 50).Value = DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.Add("@reci_vLibro", SqlDbType.VarChar, 50).Value = ObjRegCivBE.reci_vLibro;
                            }
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vLibro", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        }
                        cmd.Parameters.Add("@reci_bAnotacionFlag", SqlDbType.Bit).Value = ObjRegCivBE.reci_bAnotacionFlag;

                        if (ObjRegCivBE.reci_vObservaciones == null) { ObjRegCivBE.reci_vObservaciones = ""; }
                        if (ObjRegCivBE.reci_vObservaciones.Length == 0)
                        {
                            cmd.Parameters.Add("@reci_vObservaciones", SqlDbType.VarChar, 300).Value = DBNull.Value; 
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_vObservaciones", SqlDbType.VarChar, 300).Value = ObjRegCivBE.reci_vObservaciones;
                        }
                        cmd.Parameters.Add("@reci_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sUsuarioModificacion;
                        cmd.Parameters.Add("@reci_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjRegCivBE.reci_vIPModificacion;
                        cmd.Parameters.Add("@reci_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@reci_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@reci_cConCUI", SqlDbType.Char, 1).Value = ObjRegCivBE.reci_cConCUI;                        
                        cmd.Parameters.Add("@reci_cReconocimientoAdopcion", SqlDbType.Char, 1).Value = ObjRegCivBE.reci_cReconocimientoAdopcion;
                        cmd.Parameters.Add("@reci_cReconstitucionReposicion", SqlDbType.Char, 1).Value = ObjRegCivBE.reci_cReconstitucionReposicion;
                        if (ObjRegCivBE.reci_iNumeroActaAnterior != null)
                        {
                            cmd.Parameters.Add("@reci_iNumeroActaAnterior", SqlDbType.Int).Value = ObjRegCivBE.reci_iNumeroActaAnterior;
                        }
                        else
                        {
                            cmd.Parameters.Add("@reci_iNumeroActaAnterior", SqlDbType.Int).Value = DBNull.Value;
                        }
                        cmd.Parameters.Add("@reci_vTitular", SqlDbType.VarChar, 200).Value = ObjRegCivBE.reci_vTitular;

                        if (ObjRegCivBE.reci_bInscripcionOficio != null)
                            cmd.Parameters.Add("@reci_bInscripcionOficio", SqlDbType.Bit).Value = ObjRegCivBE.reci_bInscripcionOficio;
                        else
                            cmd.Parameters.Add("@reci_bInscripcionOficio", SqlDbType.Bit).Value = 0;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        //---------------------------------------
                    }
                }
               
                intResultado = 1;
            }

            catch (SqlException ex)
            {
                intResultado = -1;
                ObjRegCivBE.Error = true;
                ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
                throw ex;
            }
            catch (Exception ex)
            {
                intResultado = -1;
                ObjRegCivBE.Error = true;
                ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
                throw ex;
            }
            return intResultado;
        }
       
        
        
        private SqlParameter[] mtParticipante_actualizacion(RE_PARTICIPANTE participante)
        {
            SqlParameter[] prm_participante = new SqlParameter[10];

            #region Set : Parametros ...

            prm_participante[0] = new SqlParameter("@acpa_iActuacionParticipanteId", SqlDbType.BigInt);
            prm_participante[0].Value = participante.iParticipanteId;

            prm_participante[1] = new SqlParameter("@acpa_iActuacionDetalleId", SqlDbType.BigInt);
            prm_participante[1].Value = participante.iActuacionDetId;

            prm_participante[2] = new SqlParameter("@acpa_iPersonaId", SqlDbType.BigInt);
            prm_participante[2].Value = participante.iPersonaId;

            prm_participante[3] = new SqlParameter("@acpa_sTipoParticipanteId", SqlDbType.SmallInt);
            prm_participante[3].Value = participante.sTipoParticipanteId;

            prm_participante[4] = new SqlParameter("@acpa_sTipoDatoId", SqlDbType.SmallInt);
            prm_participante[4].Value = participante.sTipoDatoId;

            prm_participante[5] = new SqlParameter("@acpa_sTipoVinculoId", SqlDbType.SmallInt);
            prm_participante[5].Value = participante.sTipoVinculoId;

            prm_participante[6] = new SqlParameter("@acpa_sUsuarioModificacion", SqlDbType.SmallInt);
            prm_participante[6].Value = participante.sUsuarioModificacion;

            prm_participante[7] = new SqlParameter("@acpa_vIPModificacion", SqlDbType.VarChar, 50);
            prm_participante[7].Value = participante.vIPModificacion;

            prm_participante[8] = new SqlParameter("@acpa_sOficinaConsularId", SqlDbType.SmallInt);
            prm_participante[8].Value = participante.sOficinaConsularId;

            prm_participante[9] = new SqlParameter("@acpa_vHostName", SqlDbType.VarChar, 20);
            prm_participante[9].Value = Util.ObtenerHostName();

            #endregion Set : Parametros ...

            return prm_participante;
        }

        private SqlParameter[] mtParticipante_insersion(RE_PARTICIPANTE participante)
        {
            SqlParameter[] prm_participante = new SqlParameter[20];

            #region Set : Parametros ...

            prm_participante[0] = new SqlParameter("@acpa_iActuacionDetalleId", SqlDbType.BigInt);
            prm_participante[0].Value = participante.iActuacionDetId;

            prm_participante[1] = new SqlParameter("@acpa_iPersonaId", SqlDbType.BigInt);
            prm_participante[1].Value = participante.iPersonaId;

            prm_participante[2] = new SqlParameter("@acpa_sTipoParticipanteId", SqlDbType.SmallInt);
            prm_participante[2].Value = participante.sTipoParticipanteId;

            prm_participante[3] = new SqlParameter("@acpa_sTipoDatoId", SqlDbType.SmallInt);
            prm_participante[3].Value = participante.sTipoDatoId;

            prm_participante[4] = new SqlParameter("@acpa_sTipoVinculoId", SqlDbType.SmallInt);
            prm_participante[4].Value = participante.sTipoVinculoId;

            prm_participante[5] = new SqlParameter("@acpa_sUsuarioCreacion", SqlDbType.SmallInt);
            prm_participante[5].Value = participante.sUsuarioCreacion;

            prm_participante[6] = new SqlParameter("@acpa_vIPCreacion", SqlDbType.VarChar, 50);
            prm_participante[6].Value = participante.vIPCreacion;

            prm_participante[7] = new SqlParameter("@acpa_sOficinaConsularId", SqlDbType.SmallInt);
            prm_participante[7].Value = participante.sOficinaConsularId;

            prm_participante[8] = new SqlParameter("@acpa_vHostName", SqlDbType.VarChar, 20);
            prm_participante[8].Value = participante.vHostname;

            prm_participante[9] = new SqlParameter("@sTipoPersonaId", SqlDbType.SmallInt);
            prm_participante[9].Value = participante.sTipoPersonaId;

            prm_participante[10] = new SqlParameter("@sTipoDocumentoId", SqlDbType.SmallInt);
            prm_participante[10].Value = participante.sTipoDocumentoId;

            prm_participante[11] = new SqlParameter("@vNumeroDocumento", SqlDbType.VarChar, 20);
            prm_participante[11].Value = participante.vNumeroDocumento;

            prm_participante[12] = new SqlParameter("@sNacionalidadId", SqlDbType.SmallInt);
            prm_participante[12].Value = participante.sNacionalidadId;

            prm_participante[13] = new SqlParameter("@vNombres", SqlDbType.VarChar, 200);
            prm_participante[13].Value = participante.vNombres;

            prm_participante[14] = new SqlParameter("@vPrimerApellido", SqlDbType.VarChar, 100);
            prm_participante[14].Value = participante.vPrimerApellido;

            prm_participante[15] = new SqlParameter("@vSegundoApellido", SqlDbType.VarChar, 100);
            prm_participante[15].Value = participante.vSegundoApellido;

            prm_participante[16] = new SqlParameter("@vDireccion", SqlDbType.VarChar, 100);
            prm_participante[16].Value = participante.vDireccion;
            
            prm_participante[17] = new SqlParameter("@cUbigeo", SqlDbType.VarChar, 6);
            prm_participante[17].Value = participante.vUbigeo;

            prm_participante[18] = new SqlParameter("@ICentroPobladoId", SqlDbType.Int);
            prm_participante[18].Value = participante.ICentroPobladoId;

            prm_participante[19] = new SqlParameter("@acpa_iActuacionParticipanteId", SqlDbType.BigInt);
            prm_participante[19].Direction = ParameterDirection.Output;

            #endregion Set : Parametros ...

            return prm_participante;
        }

        //public int Eliminar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE, int IntOficinaConsularId)
        //{
        //    int IntResultQuery = 0;
        //    int intResultado = 0;

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@reci_iRegistroCivilId", SqlDbType.BigInt);
        //        prmParameter[0].Value = ObjRegCivBE.reci_iRegistroCivilId;

        //        prmParameter[1] = new SqlParameter("@reci_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameter[1].Value = ObjRegCivBE.reci_sUsuarioModificacion;

        //        prmParameter[2] = new SqlParameter("@reci_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameter[2].Value = ObjRegCivBE.reci_vIPModificacion;

        //        prmParameter[3] = new SqlParameter("@reci_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameter[3].Value = IntOficinaConsularId;

        //        prmParameter[4] = new SqlParameter("@reci_vHostName", SqlDbType.VarChar, 20);
        //        prmParameter[4].Value = Util.ObtenerHostName();

        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                   CommandType.StoredProcedure,
        //                                                   "PN_REGISTRO.USP_RE_REGISTROCIVIL_ELIMINAR",
        //                                                   prmParameter);

        //        intResultado = 1;
        //    }
        //    catch (SqlException ex)
        //    {
        //        intResultado = -1;
        //        ObjRegCivBE.Error = true;
        //        ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        intResultado = -1;
        //        ObjRegCivBE.Error = true;
        //        ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
        //        throw ex;
        //    }

        //    return intResultado;
        //}

        public int Eliminar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE, int IntOficinaConsularId)
        {
            int intResultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@reci_iRegistroCivilId", SqlDbType.BigInt).Value = ObjRegCivBE.reci_iRegistroCivilId;
                        cmd.Parameters.Add("@reci_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjRegCivBE.reci_sUsuarioModificacion;
                        cmd.Parameters.Add("@reci_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjRegCivBE.reci_vIPModificacion;
                        cmd.Parameters.Add("@reci_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@reci_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                               

                intResultado = 1;
            }
            catch (SqlException ex)
            {
                intResultado = -1;
                ObjRegCivBE.Error = true;
                ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
                throw ex;
            }
            catch (Exception ex)
            {
                intResultado = -1;
                ObjRegCivBE.Error = true;
                ObjRegCivBE.Message = "ERROR:" + ex.Message.ToString() + "TRACE:" + ex.StackTrace.ToString();
                throw ex;
            }

            return intResultado;
        }

    }
}