using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionPagoConsultaBL
    {
        public DataTable ActuacionPagoObtener(long LonActuacionId)
        {
            ActuacionPagoConsultaDA objDA = new ActuacionPagoConsultaDA();
            try
            {
                return objDA.ActuacionPagoObtener(LonActuacionId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ObtenerPago(String intAcuacionId, Int16? intOficinaConsularId)
        {
            //ActuacionPagoConsultaDA objDA = new ActuacionPagoConsultaDA();
            SGAC.DA.MRE.ACTUACION.RE_PAGO_DA objDA = new SGAC.DA.MRE.ACTUACION.RE_PAGO_DA();
            try
            {
                return objDA.ObtenerPago(intAcuacionId, intOficinaConsularId); //objDA.ObtenerPago(intAcuacionId, intOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        //---------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 14/02/2017
        // Objetivo: Actualiza Detalle de la actuación y el monto
        //---------------------------------------------------------------------------------------
        public DataTable ActuacionPagoObtenerDetalle(long LonActuacionId)
        {
            ActuacionPagoConsultaDA objDA = new ActuacionPagoConsultaDA();
            try
            {
                return objDA.ActuacionPagoObtenerDetalle(LonActuacionId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        //---------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 21/11/2017
        // Objetivo: Obtengo si existe una operación con el mismo numero registrado con anterioridad
        //---------------------------------------------------------------------------------------
        public DataTable verificarOperacion(long? DetalleActuacion, Int16 CodBanco, string vNumOperacion, Int16 intTipoPago, Int16 sOficinaConsultar, DateTime dFechaOperacion)
        {
            ActuacionPagoConsultaDA objDA = new ActuacionPagoConsultaDA();
            try
            {
                return objDA.verificarOperacion(DetalleActuacion, CodBanco, vNumOperacion, intTipoPago, sOficinaConsultar, dFechaOperacion);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
    }
}