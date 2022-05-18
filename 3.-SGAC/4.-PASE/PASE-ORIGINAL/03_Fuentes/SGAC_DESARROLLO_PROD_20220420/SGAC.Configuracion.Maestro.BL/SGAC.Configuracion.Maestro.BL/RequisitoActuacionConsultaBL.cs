using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//---------------------------------------------------------
//Fecha: 05/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Requisito_Actuacion.
//---------------------------------------------------------

namespace SGAC.Configuracion.Maestro.BL
{
    public class RequisitoActuacionConsultaBL
    {
        public DataTable Consultar_RequisitoActuacion(short intRequisitoActuacionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.RequisitoActuacionConsultaDA objDA = new RequisitoActuacionConsultaDA();

            try
            {
                return objDA.Consultar_RequisitoActuacion(intRequisitoActuacionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
