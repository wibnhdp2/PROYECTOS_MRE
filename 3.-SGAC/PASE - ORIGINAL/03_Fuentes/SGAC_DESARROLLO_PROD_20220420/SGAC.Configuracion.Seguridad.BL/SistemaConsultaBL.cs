using System;
using System.Data;
using SGAC.Configuracion.Seguridad.DA;

namespace SGAC.Configuracion.Seguridad.BL
{

    public class SistemaConsultaBL
    {
        private SistemaConsultaDA objDA;

        public DataTable ConsultarSistemasCargaInicial()
        {
            try
            {
                objDA = new SistemaConsultaDA();
                return objDA.ConsultarSistemasCargaInicial();
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
