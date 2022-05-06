using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_PAIS
    {
        public short pais_sPaisId { get; set; }
        public string pais_cUbigeo { get; set; }
        public string pais_vNombre { get; set; }
        public string pais_vZonaHoraria { get; set; }
        public string pais_vCapital { get; set; }
        public string pais_vNacionalidad { get; set; }
        public string pais_vReferenciaMapa { get; set; }
        public Nullable<short> pais_sMonedaId { get; set; }
        public string pais_vMonedaCodigo { get; set; }
        public string pais_vMonedaSimbolo { get; set; }
        public Nullable<short> pais_sContinenteId { get; set; }
        public string pais_cLetra_ISO_3166 { get; set; }
        public Nullable<short> pais_sNumero_ISO_3166 { get; set; }
        public string pais_vGentilicio_Mas { get; set; }
        public string pais_vGentilicio_Fem { get; set; }
        public string pais_cEstado { get; set; }
        public short pais_sUsuarioCreacion { get; set; }
        public string pais_vIPCreacion { get; set; }
        public DateTime pais_dFechaCreacion { get; set; }
        public Nullable<short> pais_sUsuarioModificacion { get; set; }
        public string pais_vIPModificacion { get; set; }
        public Nullable<System.DateTime> pais_dFechaModificacion { get; set; }
        public Nullable<short> pais_sIdiomaId { get; set; }
    }
}
