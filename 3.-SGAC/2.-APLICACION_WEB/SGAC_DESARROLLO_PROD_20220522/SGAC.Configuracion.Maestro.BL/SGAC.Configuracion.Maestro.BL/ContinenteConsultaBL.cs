using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//-----------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Continente.
//-----------------------------------------------

namespace SGAC.Configuracion.Maestro.BL
{
    public class ContinenteConsultaBL
    {
        public DataTable Consultar_Continente(int intContinenteId, string strNombre, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.ContinenteConsultaDA objDA = new ContinenteConsultaDA();

            try
            {
                return objDA.Consultar_Continente(intContinenteId, strNombre, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
