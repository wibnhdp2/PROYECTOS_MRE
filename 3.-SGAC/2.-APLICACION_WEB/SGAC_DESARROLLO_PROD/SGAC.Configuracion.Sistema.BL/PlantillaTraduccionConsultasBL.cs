using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
//--------------------------------------------------------
//Fecha: 09/09/2019
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar la tabla: RE_PLANTILLA_TRADUCCION
//--------------------------------------------------------
namespace SGAC.Configuracion.Sistema.BL
{

    public class PlantillaTraduccionConsultasBL
    {
        public DataTable Consultar(Int64 intPlantillaTraduccionId, Int16 intPlantillaId, Int16 intIdiomaId, Int64 intEtiquetaId,
                string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DA.PlantillaTraduccionConsultasDA objDA = new PlantillaTraduccionConsultasDA();

            try
            {
                return objDA.Consultar(intPlantillaTraduccionId, intPlantillaId, intIdiomaId, intEtiquetaId,
                    strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalCount, ref IntTotalPages);
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
