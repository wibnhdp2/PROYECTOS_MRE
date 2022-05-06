using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SGAC.Contabilidad.CuentaCorriente.DA;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.CuentaCorriente.BL
{
    public class CajaChicaConsultasBL
    {

        public DataTable ObtenerMovimientosPorNumOperacion(Int16 intOficinaConsularId, Int16 intBancoId, string strNumOperacion,
            Int16 intTipoMovimiento = (Int16) Enumerador.enmTipoMovimientoTransaccion.SALIDAS)
        {
            return (new CajaChicaConsultasDA()).ObtenerMovimientosPorNumOperacion(intOficinaConsularId, intBancoId, strNumOperacion, intTipoMovimiento);
        }

        public double ObtenerSaldo(Int16 intOficinaConsularId)
        {
            return (new CajaChicaConsultasDA()).ObtenerSaldo(intOficinaConsularId);
        }

    }
}
