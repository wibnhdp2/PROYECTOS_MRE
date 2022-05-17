using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_AUDITORIA
    {
        public Int16 audi_sFormularioId { get; set; } 
        public Int16 audi_sOperacionTipoId { get; set; } 
        public Int16 audi_sOperacionResultadoId { get; set; } 
        public Int16? audi_sTablaId { get; set; } 
        public Int16 audi_sUsuarioCreacion { get; set; } 
        public Int16 audi_sUsuarioModificacion { get; set; }
        public Int16 audi_dFechaCreacion { get; set; }
        public Int16 audi_dFechaModificacion { get; set; }
        public Int64 audi_iAuditoriaId { get; set; } 
        public Int16? audi_sClavePrimaria { get; set; } 
        public String audi_vCampos { get; set; } 
        public String audi_vValorNuevo { get; set; } 
        public String audi_vHostName { get; set; } 
        public String audi_vComentario { get; set; } 
        public String audi_vIPCreacion { get; set; }  
        public String audi_vIPModificacion { get; set; }
        public String audi_cEstado { get; set; }
        public String audi_vNombreRuta { get; set; }
        public String audi_vValoresTabla { get; set; }
        public String audi_vMensaje { get; set; }
        public Int16 audi_sOficinaConsularId { get; set; } 
    }
}
