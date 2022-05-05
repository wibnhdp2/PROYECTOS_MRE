using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//---------------------------------------------------------
//Fecha: 05/10/2020
//Autor: Miguel Márquez Beltrán
//Motivo: Crear la clase entidad: SU_PARTICIPANTES
//---------------------------------------------------------

namespace SUNARP.BE
{
    public class SU_PARTICIPANTES : BaseBussinesObject
    {
        public Int64 PART_IPARTICIPANTEID { set; get; }
        public Int64 PART_ISOLICITUDINSCRIPCIONID { set; get; }
        public Int16 PART_STIPOPARTICIPANTEID { set; get; }
        public Int32 PART_IPERSONAIDENTIFICACIONID { set; get; }
        public string PART_CESTADO { set; get; }
        public Int16 PART_SUSUARIOCREACION { set; get; }
        public string PART_VIPCREACION { set; get; }
        public DateTime PART_DFECHACREACION { set; get; }
        public Int16 PART_SUSUARIOMODIFICACION { set; get; }
        public string PART_VIPMODIFICACION { set; get; }
        public DateTime PART_DFECHAMODIFICACION { set; get; }

    }
}
