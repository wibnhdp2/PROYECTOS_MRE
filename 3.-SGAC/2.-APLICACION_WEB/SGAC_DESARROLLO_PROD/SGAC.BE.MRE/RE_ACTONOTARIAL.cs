using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIAL : BaseBussinesObject
    {
        public RE_ACTONOTARIAL(){
            this.acno_dFechaExtension = DateTime.MinValue;
            this.acno_dFechaAnulacion = DateTime.MinValue;
            this.acno_vNumeroColegiatura = string.Empty;
            this.acno_vDenominacion = string.Empty;
            this.acno_bDigitalizadoFlag = false;
            this.acno_bFlagMinuta = false;
            this.acno_vNumeroColegiatura = string.Empty;
            this.acno_vNumeroEscrituraPublica = string.Empty;
            this.acno_vItinerario = string.Empty;
            this.acno_vObservaciones = string.Empty;
            this.acno_cUbigeoDestino = string.Empty;
            this.acno_vMotivoAnulacion = string.Empty;
            this.acno_vAutorizacionTipo = string.Empty;
            this.acno_vNumeroLibro = string.Empty;
            this.acno_sNumeroHojas = 0;
            this.acno_nCostoEP = 0.0;
            this.acno_nCostoParte2 = 0.0;
            this.acno_nCostoTestimonio = 0.0;

            this.acno_bFlagLeidoAprobado = false;
            this.acno_sTamanoLetra = 7;
            //PaCambiar : trabajarlo con Cesar
            this.acno_sEstadoId = 10;
            this.Tipo_Acto = string.Empty;
            this.Sub_Tipo = string.Empty;
            this.iActoNotarialPrimigeniaId = 0;
            this.FechaExtension = string.Empty;
            this.iNumeroEscrituraPublica = 0;
        }


        public int iNumeroEscrituraPublica { get; set; }

        public Int64 acno_iActoNotarialId { get; set; }
        public Int64 acno_iActuacionId { get; set; }
        public Int16 acno_sTipoActoNotarialId { get; set; }
        
        [Required(ErrorMessage="Informacion requerida"), DisplayName("Tipo")]
        public Int16 acno_sSubTipoActoNotarialId { get; set; }

        public Int16 acno_sSubTipoActoNotarialExtraProtocolarId { get; set; }

        public string strTipoActoNotarial { get; set; }

        public Int16 acno_sOficinaConsularId { get; set; }
        public Int16 acno_sTamanoLetra { get; set; }
        public Int64 acno_iActoNotarialReferenciaId { get; set; }

        public Int32 acno_IFuncionarioAutorizadorId { get; set; }
        public Int32 acno_IFuncionarioCertificadorId { get; set; }
        //public Int16 acno_sAutorizacionTipoId { get; set; }
        public String acno_vAutorizacionTipo { get; set; }
        
        [StringLength(50), DisplayName("Denominación")]
        public string acno_vDenominacion { get; set; }
        public Boolean acno_bDigitalizadoFlag { get; set; }
        
        [DefaultValue(false)]
        public Boolean acno_bFlagMinuta { get; set; }
        public Boolean acno_bFlagLeidoAprobado { get; set; }
        public string acno_vNumeroColegiatura { get; set; }
        
        [Required(ErrorMessage = "Información requerida"), StringLength(50), DisplayName("Nro Escritura Publica Anterior")]
        public Int32 acno_iNumeroActoNotarial { get; set; }

        public string acno_vNumeroEscrituraPublica { get; set; }

        public string acno_vItinerario { get; set; }        
        
        public DateTime acno_dFechaExtension { get; set; }
        
        public string acno_vObservaciones { get; set; }
        public string acno_cUbigeoDestino { get; set; }
        public Int32 acno_IFuncionarioAnulaId { get; set; }
        
        public DateTime acno_dFechaAnulacion { get; set; }

        public string acno_vMotivoAnulacion { get; set; }
        public Int16 acno_sEstadoId { get; set; }
        public Int16 acno_sUsuarioCreacion { get; set; }
        public string acno_vIPCreacion { get; set; }
        public DateTime acno_dFechaCreacion { get; set; }
        public Int16 acno_sUsuarioModificacion { get; set; }
        public string acno_vIPModificacion { get; set; }
        public DateTime acno_dFechaModificacion { get; set; }

        public string acno_vNumeroLibro { get; set; }
        public Int16 acno_vNumeroFojaInicial { get; set; }
        public Int16 acno_vNumeroFojaFinal { get; set; }
        public Int16 acno_sNumeroHojas { get; set; }
        public Int16 sTipoLibroId { get; set; }
        public Double acno_nCostoEP { get; set; }
        public Double acno_nCostoParte2 { get; set; }
        public Double acno_nCostoTestimonio { get; set; }
        

        public String acno_vNumeroOficio { get; set; }
        public String acno_vRegistradorNombre { get; set; }
        public String acno_cRegistradorUbigeo { get; set; }
        public String acno_vPresentanteNombre { get; set; }
        public Int16 acno_sPresentanteTipoDocumento { get; set; }
        public String acno_vPresentanteNumeroDocumento { get; set; }
        public Int16 acno_sPresentanteGenero { get; set; }  
        public Int16 acno_sAccionSubTipoActoNotarialId { get; set; }

        public String acno_vNombreColegiatura { get; set; }
        public RE_ACTUACION ACTUACION { get; set; }

        public DateTime acno_dFechaConclusionFirma { get; set; }
        public Int16 acno_sCondicionTipoActoNotarialId { get; set; }
        public string acno_vPartidaRegistral { get; set; }
        //---------------------------------------------------------
        //Fecha: 06/09/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Se adiciona el atributo de oficina registral.
        //---------------------------------------------------------
        public Int16 acno_iOficinaRegistralId { get; set; }
        //---------------------------------------------------------
        //---------------------------------------------------------
        //Fecha: 10/11/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Se adiciona los atributos Tipo_Acto y Sub_Tipo.
        //---------------------------------------------------------
        public string Tipo_Acto { get; set; }
        public string Sub_Tipo { get; set; }
        public long iActoNotarialPrimigeniaId { get; set; }
        public string FechaExtension { get; set; }

        //--------------------------------------------------------------------
        //Fecha: 16/03/2022
        //Autor: Miguel Márquez Beltrán
        //Motivo: Se adiciona los atributos 
        //      acno_sAutorizacionDocumentoTipoId y acno_vAutorizacionDocumentoNumero. 
        //--------------------------------------------------------------------
        public short acno_sAutorizacionDocumentoTipoId { get; set; }
        public string acno_vAutorizacionDocumentoNumero { get; set; }
        //--------------------------------------------------------------------
    }
}
