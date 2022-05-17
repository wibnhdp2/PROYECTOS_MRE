using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_STOCK_ALMACEN : BaseBussinesObject
    {
        public SI_STOCK_ALMACEN() { }

        public short stck_sStockId { get; set; }
        public short stck_sOficinaConsularId { get; set; }
        public short stck_sInsumoId { get; set; }
        public int stck_INumeroStockMinimo { get; set; }
        public string stck_cEstado { get; set; }
        public short stck_sUsuarioCreacion { get; set; }
        public string stck_vIPCreacion { get; set; }
        public DateTime stck_dFechaCreacion { get; set; }
        public Nullable<short> stck_sUsuarioModificacion { get; set; }
        public string stck_vIPModificacion { get; set; }
        public Nullable<DateTime> stck_dFechaModificacion { get; set; }
    }
}
