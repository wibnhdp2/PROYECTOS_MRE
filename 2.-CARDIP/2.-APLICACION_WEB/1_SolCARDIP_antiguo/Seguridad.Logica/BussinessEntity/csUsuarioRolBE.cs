using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csUsuarioRolBE
    {
        #region Atributos
        private String _sUsuarioRolId;
        private String _sUsuarioId;
        private String _sGrupoId;
        private String _sRolConfiguracionId;
        private String _sOficinaConsularId;
        private String _sAcceso;
        private String _sEstado;
        private String _sIPCreacion;
        private String _sIPModificacion;
        private String _sUsuarioModificacion;
        private String _sUsuarioCreacion;

        #endregion
        #region Propiedades
        public String UsuarioRolId
        {
            get { return _sUsuarioRolId; }
            set { _sUsuarioRolId = value; }
        }
        public String UsuarioId
        {
            get { return _sUsuarioId; }
            set { _sUsuarioId = value; }
        }
        public String GrupoId
        {
            get { return _sGrupoId; }
            set { _sGrupoId = value; }
        }
        public String RolConfiguracionId
        {
            get { return _sRolConfiguracionId; }
            set { _sRolConfiguracionId = value; }
        }
        public String OficinaConsularId
        {
            get { return _sOficinaConsularId; }
            set { _sOficinaConsularId = value; }
        }
        public String Acceso
        {
            get { return _sAcceso; }
            set { _sAcceso = value; }
        }
        public String Estado
        {
            get { return _sEstado; }
            set { _sEstado = value; }
        }
        public String IPCreacion
        {
            get { return _sIPCreacion; }
            set { _sIPCreacion = value; }
        }
        public String IPModificacion
        {
            get { return _sIPModificacion; }
            set { _sIPModificacion = value; }
        }
        public String UsuarioCreacion
        {
            get { return _sUsuarioCreacion; }
            set { _sUsuarioCreacion = value; }
        }
        public String UsuarioModificacion
        {
            get { return _sUsuarioModificacion; }
            set { _sUsuarioModificacion = value; }
        }
        #endregion
    }
}
