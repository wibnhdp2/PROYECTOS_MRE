using System;
using System.Collections.Generic;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    [Serializable]
    public class beGenerales
    {
        public List<beParametro> ListaParametroGenero { get; set; }
        public List<beParametro> ListaParametroEstadoCivil { get; set; }
        public List<bePais> ListaPaises { get; set; }
        public List<beParametro> TitularDependienteParametros { get; set; }
        public List<beParametro> TipoEmision { get; set; }
        public List<beParametro> CategoriaOficinaExtranjera { get; set; }
        // REGISTRO EN LINEA
        public List<beParametro> ListaTipoInstitucion { get; set; }
        public List<beParametro> ListaCargoInstitucion { get; set; }
        public List<beParametro> ListaRelacionDependencia { get; set; }
        public List<beEstado> ListaEstadosRegLinea { get; set; }
        public List<beDocumentoIdentidad> ListaDocumentoIdentidadRegLinea { get; set; }
        public List<beOficinaconsularExtranjera> ListaOficinasConsularesExtranjeras { get; set; }
        public List<beCalidadMigratoria> ListaCalidadMigratoriaNivelPrincipal { get; set; }
        public List<beCalidadMigratoria> ListaCalidadMigratoriaNivelSecundario { get; set; }

    }
}
