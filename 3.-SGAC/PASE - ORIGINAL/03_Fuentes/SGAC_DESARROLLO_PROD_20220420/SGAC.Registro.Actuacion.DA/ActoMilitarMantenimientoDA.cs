using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoMilitarMantenimientoDA
    {
        public string strError = string.Empty;

        private string strConnectionName()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoMilitarMantenimientoDA()
        {
            GC.Collect();
        }

        public Int16 Insertar(BE.RE_REGISTROMILITAR objBE, BE.RE_PERSONA objBEP)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_ADICIONAR", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@remi_iActuacionDetalleId", SqlDbType.BigInt).Value = objBE.remi_iActuacionDetalleId;
                        cmd.Parameters.Add("@remi_sCalificacionMilitarId", SqlDbType.SmallInt).Value = objBE.remi_sCalificacionMilitarId;
                        cmd.Parameters.Add("@remi_sInstitucionMilitarId", SqlDbType.SmallInt).Value = objBE.remi_sInstitucionMilitarId;
                        cmd.Parameters.Add("@remi_IFuncionarioId", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@remi_sServicioReservaId", SqlDbType.SmallInt).Value = objBE.remi_sServicioReservaId;
                        cmd.Parameters.Add("@remi_vClase", SqlDbType.VarChar, 200).Value = objBE.remi_vClase;
                        cmd.Parameters.Add("@remi_vLibro", SqlDbType.VarChar, 50).Value = objBE.remi_vLibro;
                        cmd.Parameters.Add("@remi_sFolio", SqlDbType.SmallInt).Value = objBE.remi_sFolio;
                        cmd.Parameters.Add("@remi_sNumeroHijos", SqlDbType.SmallInt).Value = objBE.remi_sNumeroHijos;
                        cmd.Parameters.Add("@remi_IUsuarioAprobacionId", SqlDbType.Int).Value = objBE.remi_IUsuarioAprobacionId;
                        cmd.Parameters.Add("@remi_vIPAprobacion", SqlDbType.VarChar, 50).Value = objBE.remi_vIPAprobacion;
                        cmd.Parameters.Add("@remi_dFechaAprobacion", SqlDbType.DateTime).Value = objBE.remi_dFechaAprobacion;
                        cmd.Parameters.Add("@remi_bDigitalizadoFlag", SqlDbType.Bit).Value = objBE.remi_bDigitalizadoFlag;
                        cmd.Parameters.Add("@remi_vObservaciones", SqlDbType.VarChar, 300).Value = objBE.remi_vObservaciones;
                        cmd.Parameters.Add("@remi_sUsuarioCreacion", SqlDbType.SmallInt).Value = objBE.remi_sUsuarioCreacion;
                        cmd.Parameters.Add("@remi_vIPCreacion", SqlDbType.VarChar, 50).Value = objBE.remi_vIPCreacion;
                        cmd.Parameters.Add("@remi_sOficinaConsularId", SqlDbType.SmallInt).Value = objBE.OficinaConsularId;
                        cmd.Parameters.Add("@remi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@pers_vApellidoPaterno", SqlDbType.VarChar, 100).Value = objBEP.pers_vApellidoPaterno;
                        cmd.Parameters.Add("@pers_vApellidoMaterno", SqlDbType.VarChar, 100).Value = objBEP.pers_vApellidoMaterno;
                        cmd.Parameters.Add("@pers_vNombres", SqlDbType.VarChar, 100).Value = objBEP.pers_vNombres;
                        cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.SmallInt).Value = objBEP.pers_sEstadoCivilId;
                        cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = objBEP.pers_sGeneroId;
                        cmd.Parameters.Add("@pers_sPeso", SqlDbType.SmallInt).Value = objBEP.pers_sPeso;
                        cmd.Parameters.Add("@pers_vEstatura", SqlDbType.Float).Value = objBEP.pers_vEstatura;
                        cmd.Parameters.Add("@pers_sColorTezId", SqlDbType.SmallInt).Value = objBEP.pers_sColorTezId;
                        cmd.Parameters.Add("@pers_sColorOjosId", SqlDbType.SmallInt).Value = objBEP.pers_sColorOjosId;
                        cmd.Parameters.Add("@pers_sGrupoSanguineoId", SqlDbType.SmallInt).Value = objBEP.pers_sGrupoSanguineoId;
                        cmd.Parameters.Add("@pers_vSenasParticulares", SqlDbType.VarChar, 1000).Value = objBEP.pers_vSenasParticulares;
                        cmd.Parameters.Add("@pers_sOcurrenciaTipoId", SqlDbType.SmallInt).Value = objBEP.pers_sOcurrenciaTipoId;
                        cmd.Parameters.Add("@pers_vLugarNacimiento", SqlDbType.VarChar, 150).Value = objBEP.pers_vLugarNacimiento;

                        if (objBEP.pers_IOcurrenciaCentroPobladoId.HasValue)
                            cmd.Parameters.Add("@pers_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = objBEP.pers_IOcurrenciaCentroPobladoId;
                        else
                            cmd.Parameters.Add("@pers_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;

                        cmd.Parameters.Add("@pers_dNacimientoFecha", SqlDbType.DateTime).Value = objBEP.pers_dNacimientoFecha;

                        if (objBEP.pers_cNacimientoLugar != null)
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = objBEP.pers_cNacimientoLugar;
                        else
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = DBNull.Value;

                        cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt).Value = objBEP.pers_iPersonaId;
                        cmd.Parameters.Add("@remi_sTipoSuscripcion", SqlDbType.SmallInt).Value = objBE.remi_sTipoSuscripcion;

                        SqlParameter lRegistroMilitarIdReturn = cmd.Parameters.Add("@remi_iRegistroMilitarId", SqlDbType.BigInt);
                        lRegistroMilitarIdReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lRegistroMilitarIdReturn.Value != null)
                        {
                            if (lRegistroMilitarIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                objBE.remi_iRegistroMilitarId = Convert.ToInt64(lRegistroMilitarIdReturn.Value);
                            }
                        }

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }


        }

        public Int16 Actualizar(BE.RE_REGISTROMILITAR objBE, BE.RE_PERSONA objBEP)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_ACTUALIZAR", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@remi_iRegistroMilitarId", SqlDbType.BigInt).Value = objBE.remi_iRegistroMilitarId;
                        cmd.Parameters.Add("@remi_iActuacionDetalleId", SqlDbType.BigInt).Value = objBE.remi_iActuacionDetalleId;
                        cmd.Parameters.Add("@remi_sCalificacionMilitarId", SqlDbType.SmallInt).Value = objBE.remi_sCalificacionMilitarId;
                        cmd.Parameters.Add("@remi_sInstitucionMilitarId", SqlDbType.SmallInt).Value = objBE.remi_sInstitucionMilitarId;
                        cmd.Parameters.Add("@remi_IFuncionarioId", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@remi_sServicioReservaId", SqlDbType.SmallInt).Value = objBE.remi_sServicioReservaId;
                        cmd.Parameters.Add("@remi_vClase", SqlDbType.VarChar, 200).Value = objBE.remi_vClase;
                        cmd.Parameters.Add("@remi_vLibro", SqlDbType.VarChar, 50).Value = objBE.remi_vLibro;
                        cmd.Parameters.Add("@remi_sFolio", SqlDbType.SmallInt).Value = objBE.remi_sFolio;
                        cmd.Parameters.Add("@remi_sNumeroHijos", SqlDbType.SmallInt).Value = objBE.remi_sNumeroHijos;
                        cmd.Parameters.Add("@remi_IUsuarioAprobacionId", SqlDbType.Int).Value = objBE.remi_IUsuarioAprobacionId;
                        cmd.Parameters.Add("@remi_vIPAprobacion", SqlDbType.VarChar, 50).Value = objBE.remi_vIPAprobacion;
                        cmd.Parameters.Add("@remi_dFechaAprobacion", SqlDbType.DateTime).Value = objBE.remi_dFechaAprobacion;
                        cmd.Parameters.Add("@remi_bDigitalizadoFlag", SqlDbType.Bit).Value = objBE.remi_bDigitalizadoFlag;
                        cmd.Parameters.Add("@remi_vObservaciones", SqlDbType.VarChar, 300).Value = objBE.remi_vObservaciones;
                        cmd.Parameters.Add("@remi_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBE.remi_sUsuarioCreacion;
                        cmd.Parameters.Add("@remi_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.remi_vIPCreacion;
                        cmd.Parameters.Add("@remi_sOficinaConsularId", SqlDbType.SmallInt).Value = objBE.OficinaConsularId;
                        cmd.Parameters.Add("@remi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@pers_vApellidoPaterno", SqlDbType.VarChar, 100).Value = objBEP.pers_vApellidoPaterno;
                        cmd.Parameters.Add("@pers_vApellidoMaterno", SqlDbType.VarChar, 100).Value = objBEP.pers_vApellidoMaterno;
                        cmd.Parameters.Add("@pers_vNombres", SqlDbType.VarChar, 100).Value = objBEP.pers_vNombres;
                        cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.SmallInt).Value = objBEP.pers_sEstadoCivilId;
                        cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = objBEP.pers_sGeneroId;
                        cmd.Parameters.Add("@pers_sPeso", SqlDbType.SmallInt).Value = objBEP.pers_sPeso;
                        cmd.Parameters.Add("@pers_vEstatura", SqlDbType.Float).Value = objBEP.pers_vEstatura;
                        cmd.Parameters.Add("@pers_sColorTezId", SqlDbType.SmallInt).Value = objBEP.pers_sColorTezId;
                        cmd.Parameters.Add("@pers_sColorOjosId", SqlDbType.SmallInt).Value = objBEP.pers_sColorOjosId;
                        cmd.Parameters.Add("@pers_sGrupoSanguineoId", SqlDbType.SmallInt).Value = objBEP.pers_sGrupoSanguineoId;
                        cmd.Parameters.Add("@pers_vSenasParticulares", SqlDbType.VarChar, 1000).Value = objBEP.pers_vSenasParticulares;
                        cmd.Parameters.Add("@pers_sOcurrenciaTipoId", SqlDbType.SmallInt).Value = objBEP.pers_sOcurrenciaTipoId;
                        cmd.Parameters.Add("@pers_vLugarNacimiento", SqlDbType.VarChar, 150).Value = objBEP.pers_vLugarNacimiento;

                        if (objBEP.pers_IOcurrenciaCentroPobladoId.HasValue)
                            cmd.Parameters.Add("@pers_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = objBEP.pers_IOcurrenciaCentroPobladoId;
                        else
                            cmd.Parameters.Add("@pers_IOcurrenciaCentroPobladoId", SqlDbType.Int).Value = DBNull.Value;


                        cmd.Parameters.Add("@pers_dNacimientoFecha", SqlDbType.DateTime).Value = objBEP.pers_dNacimientoFecha;

                        if (objBEP.pers_cNacimientoLugar != null)
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = objBEP.pers_cNacimientoLugar;
                        else
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = DBNull.Value;

                        cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt).Value = objBEP.pers_iPersonaId;
                        cmd.Parameters.Add("@remi_sTipoSuscripcion", SqlDbType.SmallInt).Value = objBE.remi_sTipoSuscripcion;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

        }

        public Int16 Eliminar(BE.RE_REGISTROMILITAR objBE, int IntOficinaConsularId)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_ELIMINAR", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@remi_iRegistroMilitarId", SqlDbType.BigInt).Value = objBE.remi_iRegistroMilitarId;
                        cmd.Parameters.Add("@remi_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@remi_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBE.remi_sUsuarioModificacion;
                        cmd.Parameters.Add("@remi_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.remi_vIPModificacion;
                        cmd.Parameters.Add("@remi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();


                 
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

          
        }
    }
}