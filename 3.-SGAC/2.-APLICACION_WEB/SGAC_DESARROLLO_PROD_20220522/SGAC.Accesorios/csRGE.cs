using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



    public class csRGE
    {
        public string iCantAutoadhesivo { get; set; }

        public string iNumero { get; set; }
        public string iNumeroOrden { get; set; }
        public string dFecha { get; set; }
        public string vSolicitante { get; set; }
        public string vAutoadhesivoCod { get; set; }
        public string vTarifaDesc { get; set; }
        public string vTarifaNro { get; set; }
        public string iNumeroActuacion { get; set; }
        public string FMonedaExtranjera { get; set; }
        public string FSolesConsular { get; set; }
        public string FValorTCConsular { get; set; }
        public string vObservacion { get; set; }
        public string Itinerante { get; set; }
        public string PagadoLimaME { get; set; }
        public string PagadoLimaSC { get; set; }
    }

    public class csReporteRGE
    {
        public string Tipo { get; set; }
        public Int64 iNumero { get; set; }
        public Int64 iNumeroOrden { get; set; }
        public DateTime dFecha { get; set; }
        public string vSolicitante { get; set; }
        public string vApellidos { get; set; }
        public string vNombres { get; set; }
        public string vAutoadhesivoCod { get; set; }
        public string vTarifaDesc { get; set; }
        public string vTarifaNro { get; set; }
        public double FSolesConsular { get; set; }
        public double FMonedaExtranjera { get; set; }
        public double FValorTCConsular { get; set; }
        public string vObservacion { get; set; }
        public string pago_sPagoTipoId { get; set; }
        public string iNumeroActuacion { get; set; }
        public string acde_ICorrelativoActuacion { get; set; }
        public string iCantAutoadhesivo { get; set; }
        public double PagadoLimaME { get; set; }
        public double PagadoLimaSC { get; set; }
        public string Moneda_S { get; set; }
        public double TotalActEscPublicas { get; set; }
        public double PagoArubaSC { get; set; }
        public double PagadoOtrasIslasSC { get; set; }
        public string Itinerante { get; set; }
    }
