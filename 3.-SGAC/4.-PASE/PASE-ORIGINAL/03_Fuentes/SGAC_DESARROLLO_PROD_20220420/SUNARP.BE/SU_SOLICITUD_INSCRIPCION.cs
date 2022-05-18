using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//---------------------------------------------------------
//Fecha: 05/10/2020
//Autor: Miguel Márquez Beltrán
//Motivo: Crear la clase entidad: SU_SOLICITUD_INSCRIPCION
//---------------------------------------------------------

namespace SUNARP.BE
{
    public class SU_SOLICITUD_INSCRIPCION : BaseBussinesObject
    { 
        public Int64 acno_iActoNotarialId { get; set; }
        public Int16 OFICINACONSULARID { get; set; }
        public Int32 IFuncionarioId { get; set; }


        public Int64 SOIN_ISOLICITUDINSCRIPCIONID { set; get; }
        public string SOIN_VCUO { set; get; }
        public string SOIN_VCODZONAREGISTRAL { set; get; }
        public string SOIN_VCODOFICINAREGISTRA { set; get; }
        public Int64 SOIN_IACTUACIONID { set; get; }
        public string SOIN_VCODAREA { set; get; }
        public string SOIN_VDESAREA { set; get; }
        public string SOIN_VCODACTO { set; get; }
        public string SOIN_VDESACTO { set; get; }
        public string SOIN_VCODLIBRO { set; get; }
        public string SOIN_VDESLIBRO { set; get; }
        public string SOIN_VNOINSCRIBIR { set; get; }
        public string SOIN_VCODUSUARIO { set; get; }
        public string SOIN_VCODSERVICIO { set; get; }
        public string SOIN_VDESSERVICIO { set; get; }
        public string SOIN_VSECUENCIA { set; get; }
        public string SOIN_VTIPOINTRUMENTO { set; get; }
        public string SOIN_VDESTIPOINSTRUMENTO { set; get; }
        public string SOIN_VLUGAR { set; get; }
        public string SOIN_CFECHA { set; get; }
        public string SOIN_VCODPRESENTANTE { set; get; }
        public string SOIN_VAPEPATPRESENTANTE { set; get; }
        public string SOIN_VAPEMATPRESENTANTE { set; get; }
        public string SOIN_VNOMBREPRESENTANTE { set; get; }
        public string SOIN_CTIPDOCPRESENTANTE { set; get; }
        public string SOIN_VDESDOCPRESENTANTE { set; get; }
        public string SOIN_VNUMDOCPRESENTANTE { set; get; }
        public string SOIN_VCORREOPRESENTANTE { set; get; }
        public string SOIN_VCORREOSOLICITANTE { set; get; }
        public string SOIN_VPARTEFIRMADO { set; get; }
        public Int16 SOIN_SESTADOSUNARPID { set; get; }
        public string SOIN_VMOTIVOOBSERVACION { set; get; }
        public string SOIN_VANIOTITULOSUNARP { set; get; }
        public string SOIN_VNUMTITULOSUNARP { set; get; }
        public string SOIN_VNUMHOJAPRESENTACION { set; get; }
        public DateTime SOIN_DFECHAINSCRIPCIONSUNARP { set; get; }
        public DateTime SOIN_DFECHAPRESENTACIONSUNARP { set; get; }
        public DateTime SOIN_DFECHAVENCIMIENTOSUNARP { set; get; }
        public DateTime SOIN_DFECHAMAXREINGRESOSUNARP { set; get; }
        public string SOIN_VTIPREINGRESO { set; get; }
        public Int16 SOIN_SUSUARIOCREACION { set; get; }
        public string SOIN_VIPCREACION { set; get; }
        public DateTime SOIN_DFECHACREACION { set; get; }
        public Int16 SOIN_SUSUARIOMODIFICACION { set; get; }
        public string SOIN_VIPMODIFICACION { set; get; }
        public DateTime SOIN_DFECHAMODIFICACION { set; get; }

    }
}
