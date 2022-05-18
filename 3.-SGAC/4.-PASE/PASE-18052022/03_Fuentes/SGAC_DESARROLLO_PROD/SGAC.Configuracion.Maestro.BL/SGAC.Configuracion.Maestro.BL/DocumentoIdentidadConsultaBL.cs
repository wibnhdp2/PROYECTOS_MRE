using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//-------------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Documentos.
//-------------------------------------------------
namespace SGAC.Configuracion.Maestro.BL
{
    public class DocumentoIdentidadConsultaBL
    {
        public DataTable Consultar_DocumentoIdentidad(short intDocumentoIdentidadId, string strDescripcionCorta, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.DocumentoIdentidadConsultaDA objDA = new DocumentoIdentidadConsultaDA();

            try
            {
                return objDA.Consultar_DocumentosIdentidad(intDocumentoIdentidadId, strDescripcionCorta, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
