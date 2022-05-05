using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class OficinaConsularFuncConsultasBL
    {
        public DataTable Consultar(int IntFuncionarioId,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages)
        {
            DA.OficinaConsularFuncConsultasDA objDA = new DA.OficinaConsularFuncConsultasDA();

            try
            {
                return objDA.Consultar(IntFuncionarioId,                                   
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

        public DataTable Obtener(int iOficinaConsularId)
        {
            DA.OficinaConsularFuncConsultasDA objDA = new DA.OficinaConsularFuncConsultasDA();

            try
            {
                return objDA.Obtener(iOficinaConsularId);
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
