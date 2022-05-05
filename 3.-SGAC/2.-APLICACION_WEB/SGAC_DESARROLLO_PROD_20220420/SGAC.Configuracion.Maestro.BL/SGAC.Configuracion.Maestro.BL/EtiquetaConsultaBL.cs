using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Configuracion.Maestro.DA;

namespace SGAC.Configuracion.Maestro.BL
{
    public class EtiquetaConsultaBL
    {
        public DataTable Consultar(Int64 intEtiquetaID,
                                   Int16 intPlantillaID,
                                   string strEtiqueta,
                                   string strEstado,
                                   int IntPageSize,
                                   int IntCurrentPage,
                                   string strContar,
                                   ref int IntTotalPages,
                                   ref int IntTotalCount)
        {
            DA.EtiquetaConsultaDA objDA = new EtiquetaConsultaDA();

            try
            {
                return objDA.Consultar(intEtiquetaID, intPlantillaID, strEtiqueta, strEstado,
                                                    IntPageSize, IntCurrentPage, strContar, ref IntTotalPages, ref IntTotalCount);
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
