using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SGAC.BE.MRE
{
    public class RE_PERSONA: BaseBussinesObject
    {
        public RE_PERSONA() {
            this.REGISTROUNICO = new RE_REGISTROUNICO();
            this.Identificacion = new RE_PERSONAIDENTIFICACION();
            // Lista de identificaciones??
            this.Residencias = new List<RE_PERSONARESIDENCIA>();
            this.FILIACIONES = new List<RE_PERSONAFILIACION>();
        }

        public Int64 pers_iPersonaId { get; set; }
        public Int16 pers_sEstadoCivilId { get; set; }
        public Int16 pers_sProfesionId { get; set; }
        public Int16 pers_sOcupacionId { get; set; }
        public Int16 pers_sIdiomaNatalId { get; set; }
        public Int16 pers_sGeneroId { get; set; }
        public Int16 pers_sGradoInstruccionId { get; set; }
        public String pers_vApellidoPaterno { get; set; }
        public String pers_vApellidoMaterno { get; set; }
        public String pers_vNombres { get; set; }
        public String pers_vCorreoElectronico { get; set; }

   
        public  DateTime pers_dNacimientoFecha { get; set; }

        public Int16 pers_sPersonaTipoId { get; set; }
        public String pers_cNacimientoLugar { get; set; }
        public String pers_vObservaciones { get; set; }
        public Int16 pers_sNacionalidadId { get; set; }
        public String pers_vDescripcionIncapacidad { get; set; }
        public bool pers_bIncapacidadFlag { get; set; }
        public String pers_vEstatura { get; set; }
        public Int16 pers_sColorTezId { get; set; }
        public Int16 pers_sColorOjosId { get; set; }
        public Int16 pers_sColorCabelloId { get; set; }
        public Int16 pers_sGrupoSanguineoId { get; set; }
        public String pers_vSenasParticulares { get; set; }
        public Boolean pers_bFallecidoFlag { get; set; }
        public DateTime? pers_dFechaDefuncion { get; set; }
        public String pers_cUbigeoDefuncion { get; set; }
        public String pers_vApellidoCasada { get; set; }
        public String pers_cEstado { get; set; }
        public Int16 pers_sUsuarioCreacion { get; set; }
        public String pers_vIPCreacion { get; set; }
        public DateTime pers_dFechaCreacion { get; set; }
        public Int16 pers_sUsuarioModificacion { get; set; }
        public String pers_vIPModificacion { get; set; }
        public DateTime? pers_dFechaModificacion { get; set; }
        public Int16 pers_sPeso { get; set; }
        public Int16 pers_sOcurrenciaTipoId { get; set; }
        public Int32 pers_IOcurrenciaCentroPobladoId { get; set; }
        public String pers_vLugarNacimiento { get; set; }
        public bool bGenera58A { get; set; }
        public Int16 OficinaConsularId { get; set; }

        public bool pers_bPadresPeruanos { get; set; }

        public Int16 pers_sAnioEstudio {get; set;}
        public string pers_cEstudioCompleto { get; set; }

        public string pers_cDiscapacidad { get; set; }
        public string pers_cInterdicto { get; set; }
        public string pers_vNombreCurador { get; set; }
        public string pers_cDonaOrganos { get; set; }
        public Int16 pers_sTipoDeclarante { get; set; }
        public Int16 pers_sTipoTutorGuardador { get; set; }

        public RE_REGISTROUNICO REGISTROUNICO { get; set; }
        public RE_PERSONAIDENTIFICACION Identificacion { get; set; }
        public List<RE_PERSONARESIDENCIA> Residencias { get; set; }
        public List<RE_PERSONAFILIACION> FILIACIONES { get; set; }

        public RE_RESIDENCIA _ResidenciaTop {get; set;}

        //---------------------------------------------------------------
        //Fecha: 15/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Adicionar el atributo código del Pais de Origen y 
        //          el gentilicio
        //---------------------------------------------------------------
        public Int16 pers_sPaisId { get; set; }
        public string pers_vGentilicio { get; set; }
        //---------------------------------------------------------------
        //---------------------------------------------------------------
        //Fecha: 21/04/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Indicador para poder hacer un registro sin usar la entidad
        //---------------------------------------------------------------
        public bool bIndicadorFormaRegistro { get; set; }

        public string vNacionalidad { get; set; }

        //public RE_PERSONA(RE_PERSONA persona) {
        //}

        //public void Upgrade(RE_PERSONA persona) {
        //    var local = this;
        //    foreach (PropertyInfo p in local.GetType().GetProperties())
        //    {
        //        PropertyInfo pInfo = (PropertyInfo)local.GetType().GetProperty(p.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        //        PropertyInfo pInfoRead = (PropertyInfo)local.GetType().GetProperty(p.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        //        pInfo.SetValue(local, pInfoRead.GetValue(persona, null), null);
        //    }
        //}
    }
}
