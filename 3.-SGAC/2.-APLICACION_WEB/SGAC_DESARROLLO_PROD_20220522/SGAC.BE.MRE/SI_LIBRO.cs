using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_LIBRO : BaseBussinesObject
    {
        public SI_LIBRO() { }

        public short libr_sLibroId { get; set; }
        public short libr_sOficinaConsularId { get; set; }
        public short libr_sPeriodo { get; set; }
        public short libr_sTipoLibroId { get; set; }
        public int libr_INumeroEscritura { get; set; }
        public int libr_INumeroLibro { get; set; }
        public int libr_INumeroFolioInicial { get; set; }
        public int libr_INumeroFolioActual { get; set; }
        public int libr_INumeroFolioTotal { get; set; }
        //-------------------------------------------------------
        //Fecha: 23/06/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Adicionar el atributo: libr_vRangoFoliosNC
        //-------------------------------------------------------
        public string libr_vRangoFoliosNC { get; set; }
        //-------------------------------------------------------
        public string libr_cEstado { get; set; }
        public short libr_sUsuarioCreacion { get; set; }
        public string libr_vIPCreacion { get; set; }
        public DateTime libr_dFechaCreacion { get; set; }
        public Nullable<short> libr_sUsuarioModificacion { get; set; }
        public string libr_vIPModificacion { get; set; }
        public Nullable<DateTime> libr_dFechaModificacion { get; set; }
    }
}