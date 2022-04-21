using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActaJudicialMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActaJudicialMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActaJudicialMantenimientoDA()
        {
            GC.Collect();
        }

        //public int Insertar(List<BE.RE_ACTAJUDICIAL> ACTAjUDICIAL_LISTA, Int16 sOficinaConsularId, string vHostName)
        //{
        //    int acjd_iActaJudicialId = 0;
        //    long LonResultQueryActuacion = 0;
        //    int intFila = 0;
        //    int intResultado = 0;
        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[15];

        //        for (intFila = 0; intFila <= ACTAjUDICIAL_LISTA.Count() - 1; intFila++)
        //        {
        //            if (ACTAjUDICIAL_LISTA[intFila].acjd_iActaJudicialId == 0)
        //            {
        //                prmParameterActuacion[0] = new SqlParameter("@acjd_iActaJudicialId", SqlDbType.BigInt);
        //                prmParameterActuacion[0].Direction = ParameterDirection.Output;
        //            }
        //            else
        //            {
        //                prmParameterActuacion[0] = new SqlParameter("@acjd_iActaJudicialId", SqlDbType.BigInt);
        //                prmParameterActuacion[0].Value = ACTAjUDICIAL_LISTA[intFila].acjd_iActaJudicialId;
        //            }

        //            prmParameterActuacion[1] = new SqlParameter("@acjd_iActoJudicialNotificacionId", SqlDbType.BigInt);
        //            prmParameterActuacion[1].Value = ACTAjUDICIAL_LISTA[intFila].acjd_iActoJudicialNotificacionId;  // intActoJudicialParticipanteId;

        //            prmParameterActuacion[2] = new SqlParameter("@acjd_sTipoActaId", SqlDbType.SmallInt);
        //            prmParameterActuacion[2].Value = ACTAjUDICIAL_LISTA[intFila].acjd_sTipoActaId;

        //            prmParameterActuacion[3] = new SqlParameter("@acjd_IFuncionarioFirmanteId", SqlDbType.Int);
        //            prmParameterActuacion[3].Value = ACTAjUDICIAL_LISTA[intFila].acjd_IFuncionarioFirmanteId;

        //            prmParameterActuacion[4] = new SqlParameter("@acjd_dFechaHoraActa", SqlDbType.DateTime);
        //            prmParameterActuacion[4].Value = ACTAjUDICIAL_LISTA[intFila].acjd_dFechaHoraActa;

        //            prmParameterActuacion[5] = new SqlParameter("@acjd_vCuerpoActa", SqlDbType.VarChar, 2000);
        //            prmParameterActuacion[5].Value = ACTAjUDICIAL_LISTA[intFila].acjd_vCuerpoActa;

        //            prmParameterActuacion[6] = new SqlParameter("@acjd_vResultado", SqlDbType.VarChar, 2000);
        //            prmParameterActuacion[6].Value = ACTAjUDICIAL_LISTA[intFila].acjd_vResultado;

        //            prmParameterActuacion[7] = new SqlParameter("@acjd_vObservaciones", SqlDbType.VarChar, 2000);
        //            prmParameterActuacion[7].Value = ACTAjUDICIAL_LISTA[intFila].acjd_vObservaciones;

        //            prmParameterActuacion[8] = new SqlParameter("@acjd_sEstadoId", SqlDbType.SmallInt);
        //            prmParameterActuacion[8].Value = ACTAjUDICIAL_LISTA[intFila].acjd_sEstadoId;

        //            prmParameterActuacion[9] = new SqlParameter("@acjd_vResponsable", SqlDbType.VarChar, 100);
        //            prmParameterActuacion[9].Value = ACTAjUDICIAL_LISTA[intFila].acjd_vResponsable;

        //            if (ACTAjUDICIAL_LISTA[intFila].acjd_iActaJudicialId == 0)
        //            {
        //                prmParameterActuacion[10] = new SqlParameter("@acjd_sUsuarioCreacion", SqlDbType.SmallInt);
        //                prmParameterActuacion[10].Value = ACTAjUDICIAL_LISTA[intFila].acjd_sUsuarioCreacion;

        //                prmParameterActuacion[11] = new SqlParameter("@acjd_vIPCreacion", SqlDbType.VarChar, 50);
        //                prmParameterActuacion[11].Value = ACTAjUDICIAL_LISTA[intFila].acjd_vIPCreacion;

        //                prmParameterActuacion[12] = new SqlParameter("@acjd_dFechaCreacion", SqlDbType.DateTime);
        //                prmParameterActuacion[12].Value = DateTime.Now;

        //                prmParameterActuacion[13] = new SqlParameter("@sOficinaConsularId", SqlDbType.SmallInt);
        //                prmParameterActuacion[13].Value = sOficinaConsularId;

        //                prmParameterActuacion[14] = new SqlParameter("@vHostName", SqlDbType.VarChar, 20);
        //                prmParameterActuacion[14].Value = vHostName;

        //                LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_ACTAJUDICIAL_ADICIONAR",
        //                                            prmParameterActuacion);

        //                acjd_iActaJudicialId = Convert.ToInt32(prmParameterActuacion[0].Value);
        //            }
        //            else
        //            {
        //                prmParameterActuacion[10] = new SqlParameter("@acjd_sUsuarioModificacion", SqlDbType.SmallInt);
        //                prmParameterActuacion[10].Value = ACTAjUDICIAL_LISTA[intFila].acjd_sUsuarioModificacion;

        //                prmParameterActuacion[11] = new SqlParameter("@acjd_vIPModificacion", SqlDbType.VarChar, 50);
        //                prmParameterActuacion[11].Value = ACTAjUDICIAL_LISTA[intFila].acjd_vIPModificacion;

        //                prmParameterActuacion[12] = new SqlParameter("@acjd_dFechaModificacion", SqlDbType.DateTime);
        //                prmParameterActuacion[12].Value = DateTime.Now;

        //                prmParameterActuacion[13] = new SqlParameter("@sOficinaConsularId", SqlDbType.SmallInt);
        //                prmParameterActuacion[13].Value = sOficinaConsularId;

        //                prmParameterActuacion[14] = new SqlParameter("@vHostName", SqlDbType.VarChar, 20);
        //                prmParameterActuacion[14].Value = vHostName;

        //                LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_ACTAJUDICIAL_ACTUALIZAR",
        //                                            prmParameterActuacion);

        //                acjd_iActaJudicialId = Convert.ToInt32(prmParameterActuacion[0].Value);
        //            }

        //            if (acjd_iActaJudicialId != 0)          // PREGUNTAMOS SI SE ESTA DEVOLVIENDO EL ID DEL REGISTRO, ESO SIGNIFICA QUE SE HA GRABADO EL REGISTRO
        //            {
        //                intResultado = 1;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        intResultado = 0;
        //        throw ex;
        //    }

        //    return intResultado;
        //}

        //public int Actualizar_Acto_General(long iActuacionId,
        //    long iActuacionDetalleId, long sOficinaConsularId)
        //{
        //    int intResultado = 0;
        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[3];

        //        prmParameterActuacion[0] = new SqlParameter("@actu_iActuacionId", SqlDbType.BigInt);
        //        prmParameterActuacion[0].Value = iActuacionId;

        //        prmParameterActuacion[1] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterActuacion[1].Value = iActuacionDetalleId;

        //        prmParameterActuacion[2] = new SqlParameter("@actu_sOficinaConsularId", SqlDbType.BigInt);
        //        prmParameterActuacion[2].Value = sOficinaConsularId;

        //        intResultado = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                    CommandType.StoredProcedure,
        //                                    "PN_REGISTRO.USP_RE_ACTUACION_MODIFICAR_ESTADO",
        //                                    prmParameterActuacion);

        //        if (intResultado != 0)
        //        {
        //            intResultado = 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        intResultado = 0;
        //        throw ex;
        //    }

        //    return intResultado;
        //}

        //public int Actualizar_Estado(Int64 iActaJudicialId, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        //{
        //    int intResultado = 0;
        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[6];

        //        prmParameterActuacion[0] = new SqlParameter("@acjd_iActaJudicialId", SqlDbType.BigInt);
        //        prmParameterActuacion[0].Value = iActaJudicialId;

        //        prmParameterActuacion[1] = new SqlParameter("@acjd_sEstadoId", SqlDbType.SmallInt);
        //        prmParameterActuacion[1].Value = sEstadoId;

        //        prmParameterActuacion[2] = new SqlParameter("@acjd_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameterActuacion[2].Value = sUsuarioModificacion;

        //        prmParameterActuacion[3] = new SqlParameter("@acjd_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameterActuacion[3].Value = vIPModificacion;

        //        prmParameterActuacion[4] = new SqlParameter("@sOficinaConsularId", SqlDbType.VarChar, 50);
        //        prmParameterActuacion[4].Value = sOficinaConsularId;

        //        prmParameterActuacion[5] = new SqlParameter("@vHostName ", SqlDbType.VarChar, 50);
        //        prmParameterActuacion[5].Value = vHostName;

        //        intResultado = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                    CommandType.StoredProcedure,
        //                                    "PN_REGISTRO.USP_RE_ACTAJUDICIAL_ACTUALIZAR_ESTADO",
        //                                    prmParameterActuacion);
        //        if (intResultado != 0)
        //        {
        //            intResultado = 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        intResultado = 0;
        //        throw ex;
        //    }

        //    return intResultado;
        //}
    }
}