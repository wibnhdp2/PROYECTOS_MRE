using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_RESIDENCIA: BaseBussinesObject
    {
        public RE_RESIDENCIA()
        {
        }
        public RE_RESIDENCIA(Int64 iResidenciaId, Int16 sResidenciaTipoId, String vResidenciaDireccion, String vCodigoPostal, String vResidenciaTelefono, String cResidenciaUbigeo, Int32 ICentroPobladoId)
        {
            this.resi_iResidenciaId = iResidenciaId;
            this.resi_sResidenciaTipoId = sResidenciaTipoId;
            this.resi_vResidenciaDireccion = vResidenciaDireccion;
            this.resi_vCodigoPostal = vCodigoPostal;
            this.resi_vResidenciaTelefono = vResidenciaTelefono;
            this.resi_cResidenciaUbigeo = cResidenciaUbigeo;
            this.resi_ICentroPobladoId = ICentroPobladoId;
        }
        public Int64 resi_iResidenciaId { get; set; }
        public Int16 resi_sResidenciaTipoId { get; set; }
        public String resi_vResidenciaDireccion { get; set; }
        public String resi_vCodigoPostal { get; set; }
        public String resi_vResidenciaTelefono { get; set; }
        public String resi_cResidenciaUbigeo { get; set; }
        public Int32 resi_ICentroPobladoId { get; set; }
        public String resi_cEstado { get; set; }
        public Int16 resi_sUsuarioCreacion { get; set; }
        public String resi_vIPCreacion { get; set; }
        public DateTime? resi_dFechaCreacion { get; set; }
        public Int16 resi_sUsuarioModificacion { get; set; }
        public String resi_vIPModificacion { get; set; }
        public DateTime? resi_dFechaModificacion { get; set; }

        // residencia peruana (1) o extranjera (2)
        public Int16 _resi_sTipoUbigeoId { get; set; }
    }
}
