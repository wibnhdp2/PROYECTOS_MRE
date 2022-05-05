using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;

namespace SGAC.Configuracion.Sistema.BL
{
    public class ArticuloLeyConsultaBL
    {
        public DataTable ObtenerTextoLey(int iFuenteId)
        {
            ArticuloLeyConsultaDA objDA = new ArticuloLeyConsultaDA();

            try
            {
                return objDA.ObtenerTextoLey(iFuenteId);
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
