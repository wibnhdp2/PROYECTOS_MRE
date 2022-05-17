using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_FICHAREGISTRAL
    {
        public string strCodigoLocal { get; set; }
        public string strTipoDocTitular { get; set; }
        public string strNroDocTitular { get; set; }
        public string strFechaRegistro_DD { get; set; }
        public string strFechaRegistro_MM { get; set; }
        public string strFechaRegistro_YYYY { get; set; }
        public string strApPaternoTitular { get; set; }
        public string strApMaternoTitular { get; set; }
        public string strNombresTitular { get; set; }
        public string strDirDptoTitular { get; set; }
        public string strDirCodDptoTitular { get; set; }
        public string strDirProvTitular { get; set; }
        public string strDirCodProvTitular { get; set; }
        public string strDirDistTitular { get; set; }
        public string strDirCodDistTitular { get; set; }
        public string strDireccionTitular { get; set; }
        public string strCodigoPostalResidencia { get; set; }
        public string strSenasParticularesTitular { get; set; }
        public string strFecNacTitular_DD { get; set; }
        public string strFecNacTitular_MM { get; set; }
        public string strFecNacTitular_YYYY { get; set; }
        public string strNacDptoTitular { get; set; }
        public string strNacCodDptoTitular { get; set; }
        public string strNacProvTitular { get; set; }
        public string strNacCodProvTitular { get; set; }
        public string strNacDistTitular { get; set; }
        public string strNacCodDistTitular { get; set; }
        public string strApPaternoPadre { get; set; }
        public string strApMaternoPadre { get; set; }
        public string strNombresPadre { get; set; }
        public string strApPaternoMadre { get; set; }
        public string strApMaternoMadre { get; set; }
        public string strNombresMadre { get; set; }
        public string strApPaternoConyuge { get; set; }
        public string strApMaternoConyuge { get; set; }
        public string strNombresConyuge { get; set; }
        public string strApPaternoDeclarante { get; set; }
        public string strApMaternoDeclarante { get; set; }
        public string strNombresDeclarante { get; set; }
        public string strTelefonoTitular { get; set; }
        public string strCodigoLocalDestino { get; set; }
        public string strApCasadaTitular { get; set; }
        public string strCorreoElectronicoTitular { get; set; }
        /*nuevos campos*/
        public string strEstadoCivil { get; set; }
        public string strGENERO { get; set; }
        public string strGRADO_INSTRUCCION { get; set; }
        public string strEstaturaMetros { get; set; }
        public string strEstaturaCentimetros { get; set; }
        public string strANIO { get; set; }
        public string strEstudioCompleto { get; set; }
        public string strDiscapacidad { get; set; }
        public string strInterdicto { get; set; }
        public string strNombreCurador { get; set; }
        public string strDonaOrganos { get; set; }
        public string strTIPO_DECLARANTE { get; set; }
        public string strTIPO_TUTOR { get; set; }
        //--------------------------------------------------
        //Fecha: 03/03/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Adicionar nuevos campos.
        //---------------------------------------------------
        public string strTipoDocPadre { get; set; }
        public string strNroDocPadre { get; set; }
        public string strTipoDocMadre { get; set; }
        public string strNroDocMadre { get; set; }
        public string strTipoDocConyuge { get; set; }
        public string strNroDocConyuge { get; set; }
        public string strTipoDocDeclarante { get; set; }
        public string strNroDocDeclarante { get; set; }
        //---------------------------------------------------
        }
}
