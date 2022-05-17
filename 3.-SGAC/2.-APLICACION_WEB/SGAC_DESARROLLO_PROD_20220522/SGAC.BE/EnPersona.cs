using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    public class EnPersona
    {
        //--para el filtro--
        public Int64 iPersonaId { get; set; }
        public int sDocumentoTipoId { get; set; }
        public string vDocumentoNumero { get; set; }
        public int sTipoParticipanteId { get; set; }
        public int sTipoDatoId { get; set; }
        public int sTipoVinculoId { get; set; }
        //------------------

        public string vNombres { get; set; }
        public string vApellidoPaterno { get; set; }
        public string vApellidoMaterno { get; set; }
        public int sGeneroId { get; set; }
        public string vEstadoCivil { get; set; }
        public int sEstadoCivilId { get; set; }
        public string vColorTez { get; set; }
        public int sColorTezId { get; set; }
        public string vEstatura { get; set; }
        public string vColorOjos { get; set; }
        public int sColorOjosId { get; set; }
        public string vGrupoSanguineo { get; set; }
        public int sGrupoSanguineoId { get; set; }
        public string vSenasculares { get; set; }
        public string vPersonaTipo { get; set; }
        public int sPersonaTipoId { get; set; }

        public string vDocumentoTipo { get; set; }

        public string vNacionalidad { get; set; }
        public int sNacionalidadId { get; set; }
        public string vTipoParticipante { get; set; }

        public int iEmpresaId { get; set; }

        public string vTipoVinculo { get; set; }

        public string vDireccion { get; set; }
        public string vCorreoElectronico { get; set; }
        public string vTelefono { get; set; }

        public string vDptoCont { get; set; }
        public int iDptoContId { get; set; }
        public string vProvPais { get; set; }
        public int iProvPaisId { get; set; }
        public string vDistCiu { get; set; }
        public int iDistCiuId { get; set; }
        public int acpa_iResidenciaId { get; set; }
        public DateTime? pers_dNacimientoFecha { get; set; }
        public DateTime? pers_dNacimientoFechaCorta { get; set; }

        public String dFecNacimiento { get; set; }
        public String dFecNacimientoCorta { get; set; }

        public string  cNacimientoLugar { get; set; }

        public Int32 sResidenciaTipoId { get; set; }

      
        public String cDptoContId { get; set; }
        public String cProvPaisId { get; set; }
        public String cDistCiuId { get; set; }

        //------------------------------------------------------------
        //Autor: Miguel Angel Márquez Beltrán
        //Fecha: 13/12/2016
        //Objetivo: Utilizarlo para almacenar el lugar de nacimiento
        //------------------------------------------------------------
        public string vLugarNacimiento { get; set; }
        //------------------------------------------
        public string vApellidoCasada { get; set; }
    }
}
