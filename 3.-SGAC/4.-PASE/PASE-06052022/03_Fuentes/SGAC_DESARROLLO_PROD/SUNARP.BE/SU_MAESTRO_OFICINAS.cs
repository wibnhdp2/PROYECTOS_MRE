using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//---------------------------------------------------------
//Fecha: 05/10/2020
//Autor: Miguel Márquez Beltrán
//Motivo: Crear la clase entidad: SU_MAESTRO_OFICINAS
//---------------------------------------------------------
namespace SUNARP.BE
{
    public class SU_MAESTRO_OFICINAS : BaseBussinesObject
    {
        public short ofic_iOficinaId { get; set; }
        public string ofic_cCodigoZona { get; set; }
        public string ofic_cCodigoOficina { get; set; }
        public string ofic_vDescripcion { get; set; }
        //---------------------------------------
        public string ofic_cEstado { get; set; }
        public short ofic_sUsuarioCreacion { get; set; }
        public string ofic_vIPCreacion { get; set; }
        public DateTime ofic_dFechaCreacion { get; set; }
        public Nullable<short> ofic_sUsuarioModificacion { get; set; }
        public string ofic_vIPModificacion { get; set; }
        public Nullable<System.DateTime> ofic_dFechaModificacion { get; set; }
    }
}
