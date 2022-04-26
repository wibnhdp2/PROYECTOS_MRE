using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beReporteDetallexCalidad
    {
        // PARAMETROS
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public short CalMigId { get; set; }
        public short EstadoId { get; set; }
        public short OfcoExId { get; set; }
        // RESULTADO
        public string NumeroCarne { get; set; }
        public string Estado { get; set; }
        public string CalidadMigratoria { get; set; }
        public string FechaReg { get; set; }
        public string FechaEmi { get; set; }
        public string FechaVen { get; set; }
        public string Titular { get; set; }
        public string TitDep { get; set; }
        public string PaisNac { get; set; }
        public string OficinaConsularEx { get; set; }
        public string Cargo { get; set; }

        public string TipoBusqueda { get; set; }

        public short StipoEmision { get; set; }
        public string TipoDoc { get; set; }
        public string Num_documento { get; set; }
        public string Num_solicitud { get; set; }
        public string Numero_carne { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public short Paisid { get; set; }
        public short TitulaFamiliar { get; set; }
        public short OfCoId { get; set; }
        public short Usuario { get; set; }

        public string Reli_stipo_emision { get; set; }
        public string Apellidos_nombres { get; set; }
        public string Fecha_nacimientofecha { get; set; }
        public string Documentonumero { get; set; }
        public string Para_sparametroid { get; set; }
        public string Status_mig { get; set; }
        public string Documento_titular { get; set; }
        public string Pers_spaisid { get; set; }
        public string Pais_nacionalidad { get; set; }
        public string Calidad_migratoria { get; set; }
        public string Caid_sestadoid { get; set; }
        public string Fecha_emision { get; set; }
        public string Fecha_vencimiento { get; set; }
        public string Oficina_consularid { get; set; }
        public string Oficina_consular_extranjera { get; set; }
        public string Caid_dfechacreacion { get; set; }
        public string Caid_susuariocreacion { get; set; }


    }
}
