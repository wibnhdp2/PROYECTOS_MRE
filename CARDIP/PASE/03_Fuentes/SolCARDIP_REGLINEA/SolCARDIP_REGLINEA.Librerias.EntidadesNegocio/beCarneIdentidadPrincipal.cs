using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    public class beCarneIdentidadPrincipal
    {
        public beCarneIdentidad CarneIdentidad { get; set; }
        public bePersona Persona { get; set; }
        public bePersonaidentificacion PersonaIdentificacion { get; set; }
        public beResidencia Residencia { get; set; }
        public bePersonaresidencia PersonaResidencia { get; set; }
        // CONSULTA
        public List<beCarneIdentidad> ListaConsulta { get; set; }
        //CONSULTA REGISTRO
        public bePais Pais { get; set; }
        public beUbicaciongeografica UbiGeo { get; set; }
        public beOficinaconsularExtranjera OficinaConsularExtranjera { get; set; }
        public beCalidadMigratoria CalidadMigratoriaPri { get; set; }
        public beCalidadMigratoria CalidadMigratoriaSec { get; set; }
    }
}
