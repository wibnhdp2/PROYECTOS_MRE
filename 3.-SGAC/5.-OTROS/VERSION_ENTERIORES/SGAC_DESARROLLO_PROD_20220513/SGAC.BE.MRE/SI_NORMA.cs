using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_NORMA : BaseBussinesObject
    {
       public SI_NORMA() { }

       public short norm_sNormaId { get; set; }
       public short norm_sTipoNormaId { get; set; }
       public short norm_sObjetoNormaId { get; set; }
       public string norm_vNumeroArticulo { get; set; }
       public string norm_vInciso { get; set; }
       public string norm_vNombreArticulo { get; set; }
       public string norm_vDescripcionCorta { get; set; }
       public string norm_vDescripcion { get; set; }
       public Nullable<DateTime> norm_dVigenciaInicio { get; set; }
       public Nullable<DateTime> norm_dVigenciaFin { get; set; }
       public short norm_sEstadoId { get; set; }
       public short norm_sUsuarioCreacion { get; set; }
       public string norm_vIPCreacion { get; set; }
       public Nullable<short> norm_sUsuarioModificacion { get; set; }
       public string norm_vIPModificacion { get; set; }
       public short norm_sGrupoNormaId { get; set; }
    }
}
