using System;
using System.Data;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoMilitarConsultaBL
    {
        public DataTable Consultar(long LonActuacionDetalleId,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages)
        {
            DA.ActoMilitarConsultaDA objDA = new DA.ActoMilitarConsultaDA();

            try
            {
                return objDA.Consultar(LonActuacionDetalleId,
                                       StrCurrentPage,
                                       IntPageSize,
                                       ref IntTotalCount,
                                       ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable Consultar_Constancia(long LonActuacionDetalleId, long LonOficinaConsular)
        {
            DA.ActoMilitarConsultaDA objDA = new DA.ActoMilitarConsultaDA();

            try
            {
                return objDA.Consultar_Constancia(LonActuacionDetalleId, LonOficinaConsular);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable Consultar_Hoja(long LonActuacionDetalleId)
        {
            DA.ActoMilitarConsultaDA objDA = new DA.ActoMilitarConsultaDA();

            try
            {
                return objDA.Consultar_Inscripcion(LonActuacionDetalleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataSet Obtener(long LonActuacionDetalleId)
        {
            ActoMilitarConsultaDA objDA = new DA.ActoMilitarConsultaDA();

            try
            {
                return objDA.Obtener(LonActuacionDetalleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }
    }
}