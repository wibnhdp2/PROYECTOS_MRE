using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTOMIGRATORIOFORMATO : BaseBussinesObject
    {
        
        public long amfr_iActoMigratorioFormatoId { get; set; }        
        public long amfr_iActoMigratorioId { get; set; }        
        public Nullable<short> amfr_sTitularFamiliaId { get; set; }        
        public bool amfr_bAcuerdoProgramaFlag { get; set; }        
        public string amfr_vVisaCodificacion { get; set; }        
        public Nullable<short> amfr_sDiasPermanencia { get; set; }
        public string amfr_sTipoNumeroPasaporteId { get; set; }
        public string amfr_vNumeroPasaporte { get; set; }
        public Nullable<short> amfr_sPasaporteRevalidarOficinaConsularId { get; set; }        
        public Nullable<short> amfr_sPasaporteRevalidarOficinaMigracionId { get; set; }       
        public Nullable<DateTime> amfr_dPasaporteRevalidarFechaExpedicion { get; set; }        
        public Nullable<DateTime> amfr_dPasaporteRevalidarFechaExpiracion { get; set; }       
        public Nullable<short> amfr_sTipoAutorizacionId { get; set; }        
        public Nullable<short> amfr_sTipoDocumentoRREEId { get; set; }        
        public string amfr_vNumDocumentoRREE { get; set; }        
        public Nullable<DateTime> amfr_dFechaRREE { get; set; }        
        public Nullable<short> amfr_sTipoDocumentoDIGEMINId { get; set; }        
        public string amfr_vNumDocumentoDIGEMIN { get; set; }        
        public Nullable<DateTime> amfr_dFechaDIGEMIN { get; set; }
        public string amfr_vCargoFuncionario { get; set; }        
        public Nullable<bool> amfr_bFlagVisaDiplomatica { get; set; }        
        public Nullable<short> amfr_sCargoDiplomaticoId { get; set; }        
        public string amfr_vMotivoVisaDiplomatica { get; set; }        
        public string amfr_vInstitucionSolicitaVisaDiplomatica { get; set; }        
        public string amfr_vInstitucionRealizaVisaDiplomatica { get; set; }        
        public Nullable<short> amfr_sCancilleriaSolicitaAutorizacionId { get; set; }        
        public string amfr_vDocumentoAutoriza { get; set; }        
        public Nullable<bool> amfr_bFlagVisaPrensa { get; set; }        
        public string amfr_vMedioComunicacionPrensa { get; set; }        
        public Nullable<short> amfr_sCargoPrensaId { get; set; }        
        public string amfr_vMotivoPrensa { get; set; }        
        public string amfr_vObservaciones { get; set; }        
        public string amfr_cEstado { get; set; }        
        public short amfr_sUsuarioCreacion { get; set; }        
        public string amfr_vIPCreacion { get; set; }        
        public DateTime amfr_dFechaCreacion { get; set; }        
        public Nullable<short> amfr_sUsuarioModificacion { get; set; }        
        public string amfr_vIPModificacion { get; set; }
        public Nullable<DateTime> amfr_dFechaModificacion { get; set; }        
    }
}
