using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SE_FORMULARIO : BaseBussinesObject
    {     
        public short form_sFormularioId { get; set; }        
        public short form_sAplicacionId { get; set; }       
        public Nullable<short> form_sComponenteId { get; set; }        
        public string form_vNombre { get; set; }         
        public Nullable<short> form_sReferenciaId { get; set; }        
        public string form_vRuta { get; set; }         
        public string form_vImagen { get; set; }         
        public short form_sOrden { get; set; }         
        public bool form_bVisible  { get; set; }      
        public string form_cEstado { get; set; }         
        public short form_sUsuarioCreacion { get; set; }         
        public string form_vIPCreacion { get; set; }         
        public System.DateTime form_dFechaCreacion { get; set; }        
        public Nullable<short> form_sUsuarioModificacion { get; set; }         
        public string form_vIPModificacion { get; set; }
        public Nullable<System.DateTime> form_dFechaModificacion { get; set; }
        public Int16 OficinaConsularId { get; set; }
    }
}
