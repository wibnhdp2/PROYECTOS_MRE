using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beCarneIdentidadPrincipal
    {
        public beCarneIdentidad CarneIdentidad { get; set; }
        public bePersona Persona { get; set; }
        public bePersonaidentificacion PersonaIdentificacion { get; set; }
        public beResidencia Residencia { get; set; }
        public bePersonaresidencia PersonaResidencia { get; set; }
        public bePersonaHistorico PersonaHistorico { get; set; }
        public beCarneIdentidadHistorico CarneIdentidadHistorico { get; set; }
        public beMovimientoCarneIdentidad MovimientoCarne { get; set; }
        // CONSULTA
        public List<beCarneIdentidad> ListaConsulta { get; set; }
        public bePaginacion Paginacion { get; set; }
        //CONSULTA REGISTRO
        public bePais Pais { get; set; }
        public beUbicaciongeografica UbiGeo { get; set; }
        public beOficinaconsularExtranjera OficinaConsularExtranjera { get; set; }
        public beCalidadMigratoria CalidadMigratoriaPri { get; set; }
        public beCalidadMigratoria CalidadMigratoriaSec { get; set; }
        // REGISTRO PREVIO
        public beRegistroPrevio RegistroPrevio { get; set; }
        public beRegistroPrevio RegistroPrevioEdicion { get; set; }
        // RELACION DEPENDENCIA
        public beCarneIdentidad titularTitDep { get; set; }
        public beRegistroLinea RegistroLinea { get; set; }
        public beCarneIdentidadRelacionDependencia RelacionDependencia { get; set; }
        public beCarneIdentidad RelacionDependenciaEdicion { get; set; } // CONSULTA_EDICION
        public beCarneIdentidadRelacionDependencia RelacionDependenciaGen { get; set; }
    }
}
