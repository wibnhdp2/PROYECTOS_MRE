using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_PARTICIPANTE: BE.MRE.RE_ACTONOTARIALPARTICIPANTE
    {
        public CBE_PARTICIPANTE(){
            this.Empresa = new RE_EMPRESA();
            this.Persona = new RE_PERSONA();
            this.Identificacion = new RE_PERSONAIDENTIFICACION();
            this.Residencia = new RE_RESIDENCIA();
            //this.ActoNotarialParticipante = new RE_ACTONOTARIALPARTICIPANTE();            
        }

        public RE_PERSONA Persona { get; set; }
        public RE_PERSONAIDENTIFICACION Identificacion { get; set; }
        public RE_RESIDENCIA Residencia { get; set; }
        //public RE_ACTONOTARIALPARTICIPANTE ActoNotarialParticipante { get; set; }
        public RE_EMPRESA Empresa { get; set; }

        public string pers_sGeneroId_desc { get; set; }
        public string pers_sEstadoCivilId_desc { get; set; }
        public string pers_sNacionalidadId_desc { get; set; }
        public string pers_sGentilicioId_desc { get; set; }
        public string pers_sOcupacionId_desc { get; set; }
        public string pers_sProfesionId_desc { get; set; }
        public string pers_sIdiomaNatalId_desc { get; set; }
        public string peid_sDocumentoTipoId_desc { get; set; }
        public string acpa_sTipoParticipanteId_desc { get; set; }
        //public Int32 acpa_sTipoParticipanteId { get; set; }
        public Int64 anpa_iActoNotarialParticipanteIdAux { get; set; }
        public DateTime anpa_dFechaSuscripcion { get; set; }
        //public Int16 TipoPersona { get; set; }
        //public string TipoPersona_desc { get; set; }
        ////public Int16 empr_sTipoDocumentoId { get; set; }
        ////public string empr_sTipoDocumentoId_desc { get; set; }
        ////public string empr_vNumeroDocumento { get; set; }
        ////public string empr_vRazonSocial { get; set; }
        //public string pers_vApellidoPaterno { get; set; }
        //public string pers_vApellidoMaterno { get; set; }
        //public string pers_vNombres { get; set; }
        //public Int16 pers_sGeneroId { get; set; }
        //public string pers_sGeneroId_desc { get; set; }
        //public Int16 pers_sEstadoCivilId { get; set; }
        //public string pers_sEstadoCivilId_desc { get; set; }
        //public string resi_vResidenciaDireccion { get; set; }
        //public Int16 UbigeoPais { get; set; }
        //public string UbigeoPais_desc { get; set; }
        //public Int16 UbigeoRegion { get; set; }
        //public string UbigeoRegion_desc { get; set; }
        //public Int16 UbigeoCiudad { get; set; }
        //public string UbigeoCiudad_desc { get; set; }
        //public Int16 pers_sNacionalidadId { get; set; }
        //public string pers_sNacionalidadId_desc { get; set; }
        //public string resi_vCodigoPostal { get; set; }
        //public Int16 anpa_sTipoParticipanteId { get; set; }
        //public string anpa_sTipoParticipanteId_desc { get; set; }
        //public Int16 pers_sProfesionId { get; set; }
        //public string pers_sProfesionId_desc { get; set; }
        //public Int16 pers_sIdiomaNatalId { get; set; }
        //public string pers_sIdiomaNatalId_desc { get; set; }

    }
}
