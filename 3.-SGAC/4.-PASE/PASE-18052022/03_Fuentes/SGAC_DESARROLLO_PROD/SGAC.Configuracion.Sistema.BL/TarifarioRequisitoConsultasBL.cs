using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class TarifarioRequisitoConsultasBL
    {
        public DataTable Obtener(int iTarifarioId)
        {
            DA.TarifarioRequisitoConsultasDA objDA = new DA.TarifarioRequisitoConsultasDA();

            try
            {
                return objDA.Obtener(iTarifarioId);
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
