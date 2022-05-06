using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    public class RE_TARIFA_PAGO
    {
        public int sTarifarioId { get; set; }
        public string vTarifa { get; set; }
        public string vTarifaDescripcion { get; set; }
        public string vTarifaDescripcionLarga { get; set; }
        public DateTime datFechaRegistro { get; set; }
        public Int32 tari_sSeccionId { get; set; }
        public Int16 sTipoActuacion { get; set; }

        public Int16 sTipoPagoId { get; set; }
        public double dblCantidad { get; set; }

        public string vNumeroOperacion { get; set; }
        public Int16 sBancoId { get; set; }
        public DateTime datFechaPago { get; set; }
        public double dblMontoCancelado { get; set; }

        public double dblMontoSolesConsulares { get; set; }
        public double dblMontoMonedaLocal { get; set; }
        public double dblTotalSolesConsulares { get; set; }
        public double fCosto { get; set; }
        public double dblTotalMonedaLocal { get; set; }
        public string vObservaciones { get; set; }

        public String vMonedaLocal { get; set; }

        public DateTime datFechaRegistroActuacion { get; set; }
        //--------------------------------------------
        // Creador por: Miguel Angel Márquez Beltrán
        // Fecha: 15-08-2016
        // Objetivo: Adicionar la columna Clasificacion
        // Referencia: Requerimiento No.001_2.doc
        //--------------------------------------------
        public double dblClasificacion { get; set; }

        //--------------------------------------------
        // Creador por: Miguel Angel Márquez Beltrán
        // Fecha: 27-10-2016
        // Objetivo: Adicionar la columna Norma del Tarifario
        //--------------------------------------------
        public double dblNormaTarifario { get; set; }

        //-------------------------------------------------------------
        // Creador por: Miguel Angel Márquez Beltrán
        // Fecha: 25/02/2019
        // Objetivo: Adicionar la columna Sustento del tipo de pago
        //-------------------------------------------------------------
        public string vSustentoTipoPago { get; set; }

    }
}
