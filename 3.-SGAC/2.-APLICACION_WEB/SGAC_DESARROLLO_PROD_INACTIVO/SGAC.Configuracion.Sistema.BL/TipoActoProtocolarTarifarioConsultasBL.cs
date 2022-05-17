using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;

namespace SGAC.Configuracion.Sistema.BL
{
    public class TipoActoProtocolarTarifarioConsultasBL
    {
        public DataTable Consultar_TipoActoProtocolarTarifario(short sTipoActoProtocolarTarifarioId, short sTipoActoProtocolarId, short sTarifarioId,
            int IntPageSize, string StrCurrentPage, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            TipoActoProtocolarTarifarioConsultasDA objDA = new TipoActoProtocolarTarifarioConsultasDA();

            try
            {
                return objDA.Consultar_TipoActoProtocolarTarifario(sTipoActoProtocolarTarifarioId, sTipoActoProtocolarId, sTarifarioId, IntPageSize, StrCurrentPage, strContar, ref IntTotalCount, ref IntTotalPages);
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

        public bool ConsultarExisteTipoActoProtocolarTarifario(short intTipoActoProtocolarId, short intTarifarioId)
        {
            TipoActoProtocolarTarifarioConsultasDA objDA = new TipoActoProtocolarTarifarioConsultasDA();

            try
            {
                return objDA.ConsultarExisteTipoActoProtocolarTarifario(intTipoActoProtocolarId, intTarifarioId);
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


        public DataTable Consultar_TipoActoProtocolar()
        {
            TipoActoProtocolarTarifarioConsultasDA objDA = new TipoActoProtocolarTarifarioConsultasDA();

            try
            {
                return objDA.Consultar_TipoActoProtocolar();
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
//-------------------------    
    }
}
