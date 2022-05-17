using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SE_USUARIO
    {
        public short usua_sUsuarioId { get; set; }
        public Nullable<short> usua_sEmpresaId { get; set; }
        public string usua_vAlias { get; set; }
        public string usua_vApellidoPaterno { get; set; }
        public string usua_vApellidoMaterno { get; set; }
        public string usua_vNombres { get; set; }
        public Nullable<short> usua_sPersonaTipoId { get; set; }
        public Nullable<short> usua_sDocumentoTipoId { get; set; }
        public string usua_vDocumentoNumero { get; set; }
        public string usua_vCorreoElectronico { get; set; }
        public string usua_vTelefono { get; set; }
        public string usua_vDireccion { get; set; }
        public Nullable<short> usua_sReferenciaId { get; set; }
        public Nullable<System.DateTime> usua_dFechaCaducidad { get; set; }
        public string usua_vDireccionIP { get; set; }
        public string usua_cEstado { get; set; }
        public bool usua_bSesionActiva { get; set; }
        public bool usua_bBloqueoActiva { get; set; }
        public bool usua_bNotificaRemesa { get; set; }
        public short usua_sUsuarioCreacion { get; set; }
        public string usua_vIPCreacion { get; set; }
        public System.DateTime usua_dFechaCreacion { get; set; }
        public Nullable<short> usua_sUsuarioModificacion { get; set; }
        public string usua_vIPModificacion { get; set; }
        public Nullable<System.DateTime> usua_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }
    }
}
