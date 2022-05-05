using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_SERVICIO
    {
        public short serv_sServicioId { get; set; }
        public Nullable<short> serv_sOficinaConsularId { get; set; }
        public string serv_vDescripcion { get; set; }
        public Nullable<int> serv_IOrden { get; set; }
        public Nullable<short> serv_sServicioIdCab { get; set; }
        public Nullable<short> serv_sTipo { get; set; }
        public string serv_cEstado { get; set; }
        public short serv_sUsuarioCreacion { get; set; }
        public string serv_vIPCreacion { get; set; }
        public System.DateTime serv_dFechaCreacion { get; set; }
        public Nullable<short> serv_sUsuarioModificacion { get; set; }
        public string serv_vIPModificacion { get; set; }
        public Nullable<System.DateTime> serv_dFechaModificacion { get; set; }
        //------------------------------------------------------
        //Fecha: 23/02/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Adicionar el atributo: serv_vServicioDireccion
        //------------------------------------------------------
        public string serv_vServicioDireccion { get; set; }
    }
}
