using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Configuration;
using System.Reflection;

namespace SGAC.BE.MRE
{
    public class RE_REPRESENTANTELEGAL: BaseBussinesObject
    {
        public RE_REPRESENTANTELEGAL() {
            PERSONA = new RE_PERSONA();
        }

        public Int64    rele_iRepresentanteLegalId { get; set; }
        public Int64    rele_iEmpresaId	{ get; set; }
        public Int64    rele_iPersonaId { get; set; }
        public DateTime rele_dFechaInicio  { get; set; }
        public DateTime rele_dFechaFin { get; set; }
        public String   rele_cEstado { get; set; }
        public Int16    rele_sUsuarioCreacion { get; set; }
        public String   rele_vIPCreacion  { get; set; }
        public DateTime rele_dFechaCreacion  { get; set; }
        public Int16    rele_sUsuarioModificacion  { get; set; }
        public String   rele_vIPModificacion  { get; set; }
        public DateTime rele_dFechaModificacion { get; set; }

        public RE_PERSONA PERSONA { get; set; }
    }
}
