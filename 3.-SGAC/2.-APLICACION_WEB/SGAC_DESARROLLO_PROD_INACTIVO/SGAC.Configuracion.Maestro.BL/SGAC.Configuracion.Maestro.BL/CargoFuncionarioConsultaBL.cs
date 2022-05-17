using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//-------------------------------------------------------------
//Fecha: 05/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Cargo-Funcionario.
//-------------------------------------------------------------
namespace SGAC.Configuracion.Maestro.BL
{
    public class CargoFuncionarioConsultaBL
    {
        public DataTable Consultar_CargoFuncionario(short intCargoFuncionarioId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.CargoFuncionarioConsultaDA objDA = new CargoFuncionarioConsultaDA();

            try
            {
                return objDA.Consultar_CargoFuncionario(intCargoFuncionarioId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
