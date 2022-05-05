using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//------------------------------------------------------
//Fecha: 05/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Base Percepción.
//------------------------------------------------------

namespace SGAC.Configuracion.Maestro.BL
{
    public class BasePercepcionConsultaBL
    {
        public DataTable Consultar_BasePercepcion(short intBasePercepcionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.BasePercepcionConsultaDA objDA = new BasePercepcionConsultaDA();

            try
            {
                return objDA.Consultar_BasePercepcion(intBasePercepcionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
//-----------------------------------------
    }
}
