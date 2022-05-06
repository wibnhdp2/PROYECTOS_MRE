using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//--------------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Estado Civil.
//--------------------------------------------------
namespace SGAC.Configuracion.Maestro.BL
{
    public class EstadoCivilConsultaBL
    {
        public DataTable Consultar_EstadoCivil(int intEstacoCivilId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.EstadoCivilConsultaDA objDA = new EstadoCivilConsultaDA();

            try
            {
                return objDA.Consultar_EstaCivil(intEstacoCivilId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
