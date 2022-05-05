using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTOMIGRATORIOHISTORICO : BaseBussinesObject
    {        
        public long amhi_iActoMigratorioHistoricoId { get; set; }        
        public long amhi_iActoMigratorioId { get; set; }        
        public short amhi_sTipoInsumoId { get; set; }        
        public Nullable<int> amhi_IFuncionarioId { get; set; }        
        public Int64 amhi_sInsumoId { get; set; }        
        public short amhi_sMotivoId { get; set; }        
        public DateTime amhi_dFechaRegistro { get; set; }       
        public string amhi_vObservaciones { get; set; }        
        public short amhi_sEstadoId { get; set; }        
        public short amhi_sUsuarioCreacion { get; set; }        
        public string amhi_vIPCreacion { get; set; }        
        public DateTime amhi_dFechaCreacion { get; set; }        
        public Nullable<short> amhi_sUsuarioModificacion { get; set; }        
        public string amhi_vIPModificacion { get; set; }
        public Nullable<DateTime> amhi_dFechaModificacion { get; set; }        
    }
}
