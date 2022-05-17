using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//---------------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Profesiones.
//---------------------------------------------------
namespace SGAC.Configuracion.Maestro.BL
{
    public class ProfesionConsultaBL
    {
        public DataTable Consultar_Profesion(short intProfesionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.ProfesionConsultaDA objDA = new ProfesionConsultaDA();

            try
            {
                return objDA.Consultar_Profesion(intProfesionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
