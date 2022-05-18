using System;
using System.Collections.Generic;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_EMPRESARESIDENCIA : BaseBussinesObject 
    {
        public RE_EMPRESARESIDENCIA()
        {
            RESIDENCIA = new RE_RESIDENCIA();
        }

        public long emre_iEmpresaId { get; set; }
        public long emre_iResidenciaId { get; set; }
        public string emre_cEstado { get; set; }
        public short emre_sUsuarioCreacion { get; set; }
        public string emre_vIPCreacion { get; set; }
        public DateTime emre_dFechaCreacion { get; set; }
        public Nullable<short> emre_sUsuarioModificacion { get; set; }
        public string emre_vIPModificacion { get; set; }
        public Nullable<System.DateTime> emre_dFechaModificacion { get; set; }

        public RE_RESIDENCIA RESIDENCIA { get; set; }
    }
}
