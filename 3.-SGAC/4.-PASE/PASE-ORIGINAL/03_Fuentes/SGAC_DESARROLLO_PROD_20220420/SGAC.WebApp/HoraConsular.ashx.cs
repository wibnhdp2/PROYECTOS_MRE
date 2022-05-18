using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGAC.Accesorios;

namespace SGAC.WebApp
{
    /// <summary>
    /// Summary description for HoraConsular
    /// </summary>
    public class HoraConsular : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DateTime datHoraActualCliente = DateTime.Now;
            try {
                Double lDiferencia = Convert.ToDouble(context.Request["HorarioDiferencia"]);
                Int16 lHorarioVerano = Convert.ToInt16(context.Request["HorarioVerano"]);

                datHoraActualCliente = SGAC.Accesorios.Util.ObtenerFechaActual(lDiferencia, lHorarioVerano);
            }
            catch (Exception) {
                datHoraActualCliente = DateTime.Now;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(datHoraActualCliente.ToString("MMM-dd-yyyy HH:mm:ss"));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}