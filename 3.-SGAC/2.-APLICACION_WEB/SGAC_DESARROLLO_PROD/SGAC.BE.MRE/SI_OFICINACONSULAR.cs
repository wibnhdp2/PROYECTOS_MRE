using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_OFICINACONSULAR
    {
        public short ofco_sOficinaConsularId { get; set; }
        public string ofco_vCodigo { get; set; }
        public short ofco_sCategoriaId { get; set; }
        public string ofco_vNombre { get; set; }
        //public string ofco_vAbreviatura { get; set; }
        public string ofco_vDireccion { get; set; }
        public string ofco_vTelefono { get; set; }
        public string ofco_vSiglas { get; set; }        
        public string ofco_cUbigeoCodigo { get; set; }
        public Nullable<short> ofco_sMonedaId { get; set; }
        public Nullable<short> ofco_sPorcentajeTipoCambio { get; set; }
        public string ofco_cHorarioAtencion { get; set; }
        public double ofco_fDiferenciaHoraria { get; set; }
        public Nullable<short> ofco_sZonaHoraria { get; set; }
        public short ofco_sHorarioVerano { get; set; }
        public bool ofco_bASNFlag { get; set; }
        public bool ofco_bJefaturaFlag { get; set; }
        public bool ofco_bRemesaLimaFlag { get; set; }
        public string ofco_vDocumentosEmite { get; set; }
        public Nullable<short> ofco_sReferenciaId { get; set; }
        public string ofco_vPrivilegiosEspeciales { get; set; }
        public string ofco_vSitioWeb { get; set; }
        public string ofco_vRangoInicialIP { get; set; }
        public string ofco_vRangoFinIP { get; set; }
        public string ofco_vCodigoLocal { get; set; }
        public string ofco_cEstado { get; set; }
        public short ofco_sUsuarioCreacion { get; set; }
        public string ofco_vIPCreacion { get; set; }
        public System.DateTime ofco_dFechaCreacion { get; set; }
        public Nullable<short> ofco_sUsuarioModificacion { get; set; }
        public string ofco_vIPModificacion { get; set; }
        public Nullable<System.DateTime> ofco_dFechaModificacion { get; set; }
        public Nullable<short> ofco_sOficinaDependeDe { get; set; }
        //----------------------------------------------------
        //Fecha: 27/04/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Atributo de elecciones.
        //Requerimiento solicitado por Rita Huambachano.
        //----------------------------------------------------
        public bool ofco_bElecciones { get; set; }
        //----------------------------------------------------
        //----------------------------------------------------
        //Fecha: 27/04/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Atributos de latidu y lonjgitud
        //Requerimiento solicitado por Rita Huambachano y marleny.
        //----------------------------------------------------
        public string ofco_nvLatitud { get; set; }
        public string ofco_nvLongitud { get; set; }
        //----------------------------------------------------
    }
}
