using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_RUNE
    {
        public CBE_RUNE()
        {
            this.PERSONA = new RE_PERSONA();
            this.REGISTROUNICO = new RE_REGISTROUNICO();
            this.PERSONAFOTO = new RE_PERSONAFOTO();
            this.PERSONAIDENTIFICACION = new List<RE_PERSONAIDENTIFICACION>();

            this.RESIDENCIAS = new List<RE_RESIDENCIA>();   // 10/04/2015
            this.FILIACIONES = new List<CBE_FILIACION>();   // 10/04/2015

            this.RESIDENCIA = new RE_RESIDENCIA();
            this.PERSONARESIDENCIA = new List<RE_PERSONARESIDENCIA>();
            this.PERSONAFILIACION = new List<RE_PERSONAFILIACION>();            
            this.ACTUACION = new RE_ACTUACION();
            this.ACTUACIONDETALLE = new RE_ACTUACIONDETALLE();
            this.PAGO = new RE_PAGO();
            this.CORRELATIVO = new RE_CORRELATIVO();
        }
        public RE_PERSONA PERSONA { get; set; }
        public RE_REGISTROUNICO REGISTROUNICO { get; set; }
        public RE_PERSONAFOTO PERSONAFOTO { get; set; }
        public List<RE_PERSONAIDENTIFICACION> PERSONAIDENTIFICACION { get; set; }
        public RE_RESIDENCIA RESIDENCIA { get; set; }
        public List<RE_PERSONARESIDENCIA> PERSONARESIDENCIA { get; set; }
        public List<RE_PERSONAFILIACION> PERSONAFILIACION { get; set; }        
        public RE_ACTUACION ACTUACION { get; set; }
        public RE_ACTUACIONDETALLE ACTUACIONDETALLE { get; set; }
        public RE_PAGO PAGO { get; set; }
        public RE_CORRELATIVO CORRELATIVO { get; set; }

        public List<RE_RESIDENCIA> RESIDENCIAS { get; set; }    // 10/04/2015
        public List<CBE_FILIACION> FILIACIONES { get; set; }    // 10/04/2015
    }
}
