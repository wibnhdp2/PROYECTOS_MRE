using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.Configuracion.Sistema.DA;
using System.Data;

namespace SGAC.Configuracion.Sistema.BL
{
    public class CircunscripcionConsultasBL
    {
        private CircunscripcionConsultasDA objDA;

        public DataTable Consultar(int intOficinaConsularId)
        {
            try
            {
                objDA = new CircunscripcionConsultasDA();
                return objDA.Consultar(intOficinaConsularId);
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
