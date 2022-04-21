using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoJudicialNotificacionMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActoJudicialNotificacionMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoJudicialNotificacionMantenimientoDA()
        {
            GC.Collect();
        }

        //public int Eliminar(List<BE.RE_ACTOJUDICIALNOTIFICACION> NOTIFICACIONES_LISTA)
        //{
        //    long LonResultQueryActuacion = 0;
        //    int intFila = 0;
        //    int intResultado = 0;
        //    try
        //    {
        //        SqlParameter[] prmParameterActuacion = new SqlParameter[16];

        //        for (intFila = 0; intFila < NOTIFICACIONES_LISTA.Count() - 1; intFila++)
        //        {
        //            prmParameterActuacion[0] = new SqlParameter("@ajno_iActoJudicialNotificacionId", SqlDbType.Int);
        //            prmParameterActuacion[0].Value = NOTIFICACIONES_LISTA[intFila].ajno_iActoJudicialNotificacionId;

        //            LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                                CommandType.StoredProcedure,
        //                                                                "PN_REGISTRO.USP_RE_ACTOJUDICIALNOTIFICACION_ELIMINAR",
        //                                                                prmParameterActuacion);

        //            if (LonResultQueryActuacion != 0) // PREGUNTAMOS SI SE ESTA DEVOLVIENDO EL ID DEL REGISTRO, ESO SIGNIFICA QUE SE HA GRABADO EL REGISTRO
        //                intResultado = 1;
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