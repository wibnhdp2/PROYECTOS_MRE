using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

namespace SGAC.WebApp.Accesorios
{
    public class Tarifa
    {
        public Tarifa()
        {

        }

        public BE.MRE.SI_TARIFARIO GetTarifario(DataTable dtTarifarioFiltrado, int intIndiceSeleccionado)
        {
            BE.MRE.SI_TARIFARIO objTarifarioBE = new BE.MRE.SI_TARIFARIO();

            objTarifarioBE.tari_sTarifarioId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sTarifarioId"]);
            objTarifarioBE.tari_sSeccionId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sSeccionId"]);
            objTarifarioBE.tari_sNumero = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sNumero"]);
            objTarifarioBE.tari_vLetra = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vLetra"].ToString();
            objTarifarioBE.tari_FCosto = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FCosto"]);
            objTarifarioBE.tari_vDescripcion = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vDescripcion"].ToString();
            objTarifarioBE.tari_vDescripcionCorta = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vDescripcionCorta"].ToString();

            objTarifarioBE.tari_sBasePercepcionId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sBasePercepcionId"]);
            objTarifarioBE.tari_sCalculoTipoId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sCalculoTipoId"]);
            objTarifarioBE.tari_vCalculoFormula = dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vCalculoFormula"].ToString();

            objTarifarioBE.tari_sTopeUnidadId = Convert.ToInt16(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_sTopeUnidadId"]);
            objTarifarioBE.tari_ITopeCantidad = Convert.ToInt32(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_ITopeCantidad"]);

            if (dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"] != System.DBNull.Value)
            {
                objTarifarioBE.tari_FMontoExceso = Convert.ToDouble(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_FMontoExceso"]);
            }
            else
            {
                objTarifarioBE.tari_FMontoExceso = 0;
            }

            objTarifarioBE.tari_bTarifarioDependienteFlag = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bTarifarioDependienteFlag"]);
            objTarifarioBE.tari_bHabilitaCantidad = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bHabilitaCantidad"]);

            objTarifarioBE.tari_vTipoPagoExcepcion = Convert.ToString(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_vTipoPagoExcepcion"]);
            objTarifarioBE.tari_bFlujoGeneral = Convert.ToBoolean(dtTarifarioFiltrado.Rows[intIndiceSeleccionado]["tari_bFlujoGeneral"]);

            return objTarifarioBE;
        }

        
    }
}