using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PARTICIPANTE
    {

        public int sTipoActuacionId { get; set; }

        public Int64 iParticipanteId { get; set; }

        public Int64 iActuacionDetId { get; set; }
        public Int64 iActoNotarialId { get; set; }
        public Int64 iActoJudicialId { get; set; }

        public Int16 sTipoParticipanteId { get; set; }
        public string vTipoParticipante { get; set; }
 
        public Int16 sTipoDatoId { get; set; }
        public Int16 sTipoVinculoId { get; set; }
       
        public DateTime dFechaLlegadaValija { get; set; }
        public int sOficinaConsularDestinoId { get; set; }
    
        public bool bolFirma { get; set; }
        public bool bolHuella { get; set; }

        public int sTipoPersonaId { get; set; }
        public Int64 iPersonaId { get; set; }
        public Int64 iEmpresaId { get; set; }
        public Int16 sTipoDocumentoId { get; set; }
        public string vTipoDocumento { get; set; }
        public string vNumeroDocumento { get; set; }

        public int sNacionalidadId { get; set; }
        public int sGeneroId { get; set; }
        public string vNombres { get; set; }
        public string vPrimerApellido { get; set; }
        public string vSegundoApellido { get; set; }
        public DateTime? pers_dNacimientoFecha { get; set; }
        public string pers_cNacimientoLugar { get; set; }

        public string vDireccion { get; set; }
        public string vUbigeo { get; set; }
        public Int32 ICentroPobladoId { get; set; }

        public String cEstado { get; set; }
        public Int16 sUsuarioCreacion { get; set; }
        public string vIPCreacion { get; set; }
        public Int16 sUsuarioModificacion { get; set; }
        public string vIPModificacion { get; set; }
        public Int16 sOficinaConsularId { get; set; }
        public string vHostname { get; set; }

        public bool pers_bFallecidoFlag { get; set; }
        public DateTime pers_dFechaDefuncion { get; set; }
        public string pers_cUbigeoDefuncion { get; set; }

        public Int64 pere_iResidenciaId { get; set; }

        public Boolean bExisteRune { get; set; }

        public Int32 pers_sEstadoCivilId { get; set; }


        public Boolean acpa_bDocumentoFlag { get; set; }

        public String acpa_vDatosParticipante { get; set; }

        public String vNumeroDocumentoCompleto { get; set; }

        public String vNombresCompletos { get; set; }

        public Int64 Item { get; set; }
    }
}
