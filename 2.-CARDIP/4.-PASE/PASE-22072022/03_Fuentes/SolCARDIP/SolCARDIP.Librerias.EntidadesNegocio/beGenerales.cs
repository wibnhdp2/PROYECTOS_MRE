using System;
using System.Collections.Generic;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beGenerales
    {
        public beSistema SistemaInfo { get; set; }
        public List<beEstado> ListaEstados { get; set; }
        public List<beOficinaconsular> ListaOficinasPeru { get; set; }
        public List<beOficinaconsular> ListaOficinasConsularesPeru { get; set; }
        public List<beOficinaconsularExtranjera> ListaOficinasConsularesExtranjeras { get; set; }
        public List<beCalidadMigratoria> ListaCalidadMigratoriaNivelPrincipal { get; set; }
        public List<beCalidadMigratoria> ListaCalidadMigratoriaNivelSecundario { get; set; }
        public List<beCategoriaFuncionario> ListaCategoriaFuncionario { get; set; }
        public List<beParametro> ListaParametroGenero { get; set; }
        public List<beParametro> ListaParametroEstadoCivil { get; set; }
        public List<beDocumentoIdentidad> ListaDocumentoIdentidad { get; set; }
        public List<beContinente> ListaContinentes { get; set; }
        public List<bePais> ListaPaises { get; set; }
        public List<beParametro> TitularDependienteParametros { get; set; }
        public List<beParametro> CategoriaOficinaExtranjera { get; set; }
        public List<beParametro> TipoObservacion { get; set; }
        public List<beParametro> TipoDuplicado { get; set; }
        public List<beUsuario> ListaUsuarios { get; set; }
        public List<beRolconfiguracion> ListaRoles { get; set; }
        public List<beParametro> TipoEntrada { get; set; }
        public List<beParametro> TipoAdjunto { get; set; }
        public List<beParametro> TipoArchivo { get; set; }
        public List<beEstado> ListaEstadosAdjunto { get; set; }
        public List<beEstado> ListaEstadosSolicitud { get; set; }
        public List<beParametro> TipoEmision { get; set; }
        public List<beEstado> ListaEstadosActaObs { get; set; }
        // REGISTRO EN LINEA
        public List<beParametro> ListaTipoInstitucion { get; set; }
        public List<beParametro> ListaCargoInstitucion { get; set; }
        public List<beParametro> ListaRelacionDependencia { get; set; }
        public List<beEstado> ListaEstadosRegLinea { get; set; }
        public List<beDocumentoIdentidad> ListaDocumentoIdentidadRegLinea { get; set; }
        public List<beMensajeEstado> ListaMensajeEstado { get; set; }
    }
}
