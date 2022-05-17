using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//--------------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Sección.
//--------------------------------------------------

namespace SGAC.Configuracion.Maestro.BL
{
    public class SeccionConsultaBL
    {
        public DataTable Consultar_Seccion(short intSeccionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.SeccionConsultaDA objDA = new SeccionConsultaDA();

            try
            {
                return objDA.Consultar_Seccion(intSeccionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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

        //-----------------------------------
    }
}
