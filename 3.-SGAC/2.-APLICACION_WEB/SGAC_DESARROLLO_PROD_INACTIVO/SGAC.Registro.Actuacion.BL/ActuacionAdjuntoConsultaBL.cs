using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.DA;
using System.Collections.Generic;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionAdjuntoConsultaBL
    {
        public DataTable ActuacionAdjuntosObtener(long LonActuacionId, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            ActuacionAdjuntoConsultaDA objDA = new ActuacionAdjuntoConsultaDA();
            try
            {
                DataTable dt = objDA.ActuacionAdjuntosObtener(LonActuacionId, StrCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages);
                return dt;
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                    objDA = null;
            }
        }
        /*SACOSTAC 31012017*/
        public DataTable ActuacionAdjuntosObtenerAdjuntos(long LonActuacionId, string strIdAdjunto, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            ActuacionAdjuntoConsultaDA objDA = new ActuacionAdjuntoConsultaDA();
            try
            {
                DataTable dt = objDA.ActuacionAdjuntosObtenerAdjuntos(LonActuacionId, strIdAdjunto, StrCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages);
                return dt;
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                    objDA = null;
            }
        }

        public List<BE.MRE.RE_ACTUACIONADJUNTO> ActuacionAdjuntoObtenerDigitalizados(Int64 iActuacionDetalleId) {

            ActuacionAdjuntoConsultaDA oActuacionAdjuntoConsultaDA = new ActuacionAdjuntoConsultaDA();
            return oActuacionAdjuntoConsultaDA.Actuaciondetalle_Obtener_Digitalizados(iActuacionDetalleId);
        }

        public DataTable ActuacionAdjuntosObtenerFoto(long LonActuacionId, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            ActuacionAdjuntoConsultaDA objDA = new ActuacionAdjuntoConsultaDA();
            try
            {
                DataTable dt = objDA.ActuacionAdjuntosObtenerFoto(LonActuacionId, StrCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages);
                return dt;
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                    objDA = null;
            }
        }

    }
}
