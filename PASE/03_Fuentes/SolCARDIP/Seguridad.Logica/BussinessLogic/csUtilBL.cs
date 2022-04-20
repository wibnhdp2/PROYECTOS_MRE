using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessADO;

namespace Seguridad.Logica.BussinessLogic
{

    public class csUtilBL
    {
        public DateTime obtenerDateTime(string stryyyymmdd)
        {
            csUtilADO objUtilADO = new csUtilADO();
            return objUtilADO.obtenerDateTime(stryyyymmdd);
        }
        public string obtenerFechaHoraMinutoAdicional()
        {
            csUtilADO objUtilADO = new csUtilADO();
            return objUtilADO.obtenerFechaHoraMinutoAdicional();
        }
        public string obtenerFechaHoraActual()
        {
            csUtilADO objUtilADO = new csUtilADO();
            return objUtilADO.obtenerFechaHoraActual();
        }
        public string obtenerFechaHoraMenorIgual(string strFechaHoraMinAdicional)
        {
            csUtilADO objUtilADO = new csUtilADO();
            return objUtilADO.obtenerFechaHoraMenorIgual(strFechaHoraMinAdicional);
        }
    }
}
