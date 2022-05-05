using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_REGISTROUNICO : BaseBussinesObject
    {
        public Int64 reun_iRegistroUnicoId { get; set; }
        public Int64 reun_iPersonaId { get; set; }
        public String reun_vEmergenciaNombre { get; set; }
        public Int16 reun_sEmergenciaRelacionId { get; set; }
        public String reun_vEmergenciaDireccionLocal { get; set; }
        public String reun_vEmergenciaCodigoPostal { get; set; }
        public String reun_vEmergenciaTelefono { get; set; }
        public String reun_vEmergenciaDireccionPeru { get; set; }
        public String reun_vEmergenciaCorreoElectronico { get; set; }
        public String reun_cViveExteriorDesde { get; set; }
        public bool reun_bPiensaRetornarAlPeru { get; set; }
        public String reun_cCuandoRetornaAlPeru { get; set; }
        public bool reun_bAfiliadoSeguroSocial { get; set; }
        public bool reun_bAfiliadoAFP { get; set; }
        public bool reun_bAportaSeguroSocial { get; set; }
        public bool reun_bBeneficiadoExterior { get; set; }
        public Int16 reun_sOcupacionPeru { get; set; }
        public Int16 reun_sOcupacionExtranjero { get; set; }
        public String reun_vNombreConvenio { get; set; }
        public String reun_cEstado { get; set; }

        public Int16 reun_sUsuarioCreacion { get; set; }
        public String reun_vIPCreacion { get; set; }
        public Int16 reun_sUsuarioModificacion { get; set; }
        public String reun_vIPModificacion { get; set; }
    }
}
