using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
//--------------------------------------------------
//Fecha: 15/03/2017
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar la tabla PS_SISTEMA.SI_Pais
//--------------------------------------------------

namespace SGAC.Configuracion.Sistema.BL
{

    public class PaisConsultasBL
    {
        public DataTable Consultar_Pais(int intPaisId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar,
            ref int IntTotalCount, ref int IntTotalPages, short sContinenteId = 0, string strNombrePais = "", short sIdiomaId = 0)
        {
            DA.PaisConsultasDA objDA = new PaisConsultasDA();

            try
            {
                return objDA.Consultar_Pais(intPaisId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalCount, ref IntTotalPages, sContinenteId, strNombrePais, sIdiomaId);
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
