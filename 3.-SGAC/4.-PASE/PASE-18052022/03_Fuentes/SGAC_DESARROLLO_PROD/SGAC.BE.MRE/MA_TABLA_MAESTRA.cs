using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class MA_TABLA_MAESTRA
    {
        public int tama_sTablaId { get; set; }
        public int tama_sRegistroId { get; set; }
        public string tama_cCodigo { get; set; }
        public string tama_vGrupo { get; set; }
        public string tama_vNombre { get; set; }
        public string tama_vDescripcionCorta { get; set; }
        public string tama_vDescripcion { get; set; }
        public string tama_vDescripcionLarga { get; set; }
        public string tama_vSimbolo { get; set; }
        public int tama_sOficinaConsularId { get; set; }
        public int tama_sUsuarioCreacion { get; set; }
        public string tama_vIPCreacion { get; set; }
        public int tama_sUsuarioModificacion { get; set; }
        public string tama_vIPModificacion { get; set; }
        public string tama_cEstado { get; set; }
    }
}
