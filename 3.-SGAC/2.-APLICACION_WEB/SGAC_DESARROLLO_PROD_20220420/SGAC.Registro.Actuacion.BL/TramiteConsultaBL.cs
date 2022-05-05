using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class TramiteConsultaBL
    {
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Método sin uso.
        //----------------------------------------------------------
        #region Comentada
        //public DataTable ActuacionConsultaConsular(long lngPersonaId,
        //                                    int intSeccionId,
        //                                     DateTime FechaInicio,
        //                                     DateTime FechaFinal,
        //                                     int IntCurrentPage,
        //                                     int IntPageSize,
        //                                     ref int IntTotalCount,
        //                                     ref int IntTotalPages)
        //{
        //    TramiteConsultaDA objDA = new TramiteConsultaDA();
        //    try
        //    { 
        //        return objDA.ActuacionConsultaConsular(lngPersonaId,
        //                                        intSeccionId,
        //                                        FechaInicio,
        //                                        FechaFinal,
        //                                        IntCurrentPage,
        //                                        IntPageSize,
        //                                        ref IntTotalCount,
        //                                        ref IntTotalPages);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SGACExcepcion(ex.Message, ex.InnerException);
        //    }
        //    finally
        //    {
        //        objDA = null;
        //    }
        //}
        #endregion

        public DataTable ActuacionConsultaJudicial(long lngPersonaId,
                                             DateTime FechaInicio,
                                             DateTime FechaFinal,
                                             int IntCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {
            TramiteConsultaDA objDA = new TramiteConsultaDA();
            try
            {
                return objDA.ActuacionConsultaJudicial(lngPersonaId,
                                                FechaInicio,
                                                FechaFinal,
                                                IntCurrentPage,
                                                IntPageSize,
                                                ref IntTotalCount,
                                                ref IntTotalPages);
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

        public DataTable ActuacionConsultaNotarial(long lngPersonaId,
                                             DateTime FechaInicio,
                                             DateTime FechaFinal,
                                             int IntCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {
            TramiteConsultaDA objDA = new TramiteConsultaDA();
            try
            {
                return objDA.ActuacionConsultaNotarial(lngPersonaId,
                                                FechaInicio,
                                                FechaFinal,
                                                IntCurrentPage,
                                                IntPageSize,
                                                ref IntTotalCount,
                                                ref IntTotalPages);
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
