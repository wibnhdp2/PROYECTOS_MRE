using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//---------------------------------------------------------
//Fecha: 14/01/2021
//Autor: Miguel Márquez Beltrán
//Motivo: Crear la clase entidad: SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO
//---------------------------------------------------------

namespace SUNARP.BE
{
    public class SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO : BaseBussinesObject
    {
        public Int64 sise_iSolicitudSeguimientoId { get; set; }
        public Int64 sise_iSolicitudInscripcionId { get; set; }
        public DateTime sise_dFechaRegistro { get; set; }
        public short sise_sEstadoId { get; set; }
        public string sise_vObservacion { get; set; }
        public short sise_sUsuarioCreacion { get; set; }
        public string sise_vIPCreacion { get; set; }
        public DateTime sise_dFechaCreacion { get; set; }
        public Nullable<short> sise_sUsuarioModificacion { get; set; }
        public string sise_vIPModificacion { get; set; }
        public Nullable<System.DateTime> sise_dFechaModificacion { get; set; }
    }
}
