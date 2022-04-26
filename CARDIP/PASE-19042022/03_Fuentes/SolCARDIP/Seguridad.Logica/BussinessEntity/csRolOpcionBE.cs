using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csRolOpcionBE
    {
        #region Atributos
        private String _sRolOpcionId;
        private String _sFormularioId;
        private String _sAcciones;
        private String _sEstado;
        private String _sIPCreacion;
        private String _sIPModificacion;
        private String _sUsuarioModificacion;
        private String _sUsuarioCreacion;
        #endregion
        #region Propiedades
        public String RolOpcionId
        {
            get { return _sRolOpcionId; }
            set { _sRolOpcionId = value; }
        }
        public String FormularioId
        {
            get { return _sFormularioId; }
            set { _sFormularioId = value; }
        }
        public String Acciones
        {
            get { return _sAcciones; }
            set { _sAcciones = value; }
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
