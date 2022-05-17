using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//-----------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Ocupación.
//-----------------------------------------------

namespace SGAC.Configuracion.Maestro.BL
{
    public class OcupacionConsultaBL
    {
        public DataTable Consultar_Ocupacion(int intOcupacionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.OcupacionConsultaDA objDA = new OcupacionConsultaDA();

            try
            {
                return objDA.Consultar_Ocupacion(intOcupacionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
