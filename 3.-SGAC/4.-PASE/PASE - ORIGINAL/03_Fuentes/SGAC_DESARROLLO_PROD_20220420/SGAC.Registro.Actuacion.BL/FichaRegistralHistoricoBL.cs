using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.DA.MRE;
using System.Data;
using SGAC.Registro.Actuacion.DA;
//---------------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase lógica de la ficha registral historica
//---------------------------------------------------------------
namespace SGAC.Registro.Actuacion.BL
{
    public class FichaRegistralHistoricoBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }


        public DataTable Consultar(long intFichaRegistralId, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.FichaRegistralHistoricoDA objDA = new FichaRegistralHistoricoDA();

            try
            {
                return objDA.Consultar(intFichaRegistralId, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
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
