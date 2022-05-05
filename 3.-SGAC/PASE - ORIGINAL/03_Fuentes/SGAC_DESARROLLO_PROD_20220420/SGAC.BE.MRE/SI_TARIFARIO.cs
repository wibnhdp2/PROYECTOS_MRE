using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    [Serializable]
    public class SI_TARIFARIO : BaseBussinesObject
    {   
        public short tari_sTarifarioId {get; set;}        
        public short tari_sSeccionId {get; set;}        
        public short tari_sNumero {get; set;}        
        public string tari_vLetra {get; set;}        
        public string tari_vDescripcionCorta {get; set;}        
        public string tari_vDescripcion {get; set;}        
        public Nullable<short> tari_sBasePercepcionId {get; set;}        
        public Nullable<short> tari_sCalculoTipoId {get; set;}        
        public string tari_vCalculoFormula {get; set;}        
        public Nullable<double> tari_FCosto {get; set;}        
        public Nullable<short> tari_sTopeUnidadId {get; set;}       
        public Nullable<int> tari_ITopeCantidad {get; set;}
        public Nullable<int> tari_ITopeCantidadMinima { get; set; }        
        public Nullable<double> tari_FMontoExceso {get; set;}        
        public Nullable<bool> tari_bTarifarioDependienteFlag {get; set;}        
        public Nullable<System.DateTime> tari_dVigenciaInicio {get; set;}        
        public Nullable<System.DateTime> tari_dVigenciaFin {get; set;}        
        public Nullable<bool> tari_bHabilitaCantidad {get; set;}
        public Nullable<bool> tari_bFlujoGeneral { get; set; }
        public string tari_vTipoPagoExcepcion { get; set; }
        public short tari_sEstadoId {get; set;}        
        public short tari_sUsuarioCreacion {get; set;}        
        public string tari_vIPCreacion {get; set;}        
        public System.DateTime tari_dFechaCreacion {get; set;}        
        public Nullable<short> tari_sUsuarioModificacion {get; set;}        
        public string tari_vIPModificacion {get; set;}
        public Nullable<System.DateTime> tari_dFechaModificacion { get; set; }

        public short OficinaConsularId { get; set; }
        public bool tari_bFlagExcepcion { get; set; }
    }
}
