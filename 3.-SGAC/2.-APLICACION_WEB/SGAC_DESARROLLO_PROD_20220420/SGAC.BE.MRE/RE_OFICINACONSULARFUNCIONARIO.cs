using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_OFICINACONSULARFUNCIONARIO : BaseBussinesObject
    {        
        public short ocfu_sOfiConFuncionarioId {get; set;}        
        public Nullable<short> ocfu_sOficinaConsularId {get; set;}
        public string ocfu_vDocumentoNumero { get; set; }
        public string ocfu_vNombreFuncionario { get; set; }
        public string ocfu_vApellidoPaternoFuncionario { get; set; }
        public string ocfu_vApellidoMaternoFuncionario { get; set; }
        public string ocfu_vCargo { get; set; }        
        public Nullable<int> ocfu_IFuncionarioId {get; set;}
        public Nullable<short> ocfu_sCargoFuncionarioId { get; set; }        
        public string ocfu_cEstado {get; set;}
        public Nullable<short> ocfu_sUsuarioCreacion {get; set;}       
        public string ocfu_vIPCreacion {get; set;}        
        public System.DateTime ocfu_dFechaCreacion {get; set;}       
        public Nullable<short> ocfu_sUsuarioModificacion {get; set;}        
        public string ocfu_vIPModificacion {get; set;}
        public Nullable<System.DateTime> ocfu_dFechaModificacion { get; set; }
        public Int16? sGeneroId { get; set; }
        public String cGenero { get; set; }
    }
}
