using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTUACIONADJUNTO : BaseBussinesObject
    {
        public Int64 acad_iActuacionAdjuntoId {get;set;}
        public Int64 acad_iActuacionDetalleId { get;set;}
        public Int16 acad_sAdjuntoTipoId {get;set;}        
        public String usuario {get;set;}
        public String vAdjuntoTipo {get;set;}

        public String  vNombreArchivo {get;set;}
        public String vDescripcion { get; set; }

        public string acad_IDigitalizadoFuncionarioId { get; set; }
        public bool acad_bDigitalizadoCorrectaFlag { get; set; }
        public bool acad_bBloqueoAdjunto { get; set; }

        public string acad_cEstado { get; set; }
        public short acad_sUsuarioCreacion { get; set; }
        public string acad_vIPCreacion { get; set; }
        public DateTime? acad_dFechaCreacion { get; set; }
        public Nullable<short> acad_sUsuarioModificacion { get; set; }
        public string acad_vIPModificacion { get; set; }
        public Nullable<System.DateTime> acad_dFechaModificacion { get; set; }
       
    }
}
