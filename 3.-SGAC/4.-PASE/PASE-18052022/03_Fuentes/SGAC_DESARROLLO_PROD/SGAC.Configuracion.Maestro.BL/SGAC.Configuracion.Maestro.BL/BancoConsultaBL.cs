using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;
//--------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Bancos.
//--------------------------------------------

namespace SGAC.Configuracion.Maestro.BL
{
    public class BancoConsultaBL
    {
        public DataTable Consultar_Banco(short intBancoId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.BancoConsultaDA objDA = new BancoConsultaDA();

            try
            {
                return objDA.Consultar_Banco(intBancoId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
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
