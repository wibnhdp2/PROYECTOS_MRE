using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csRolConfiguracionBE
    {
        #region Atributos
        private String _sRolConfiguracion;
        private String _sAplicacionId;
        private String _sRolTipoId;
        private String _sRolOpcion;
        private String _sRolOpcionOld;
        private String _sNombre;
        private String _sHorario;
        private String _sEstado;
        private String _sIPCreacion;
        private String _sIPModificacion;
        private String _sUsuarioModificacion;
        private String _sUsuarioCreacion;
        #endregion
        #region Propiedades
        public String RolConfiguracion
        {
            get { return _sRolConfiguracion; }
            set { _sRolConfiguracion = value; }
        }
        public String AplicacionId
        {
            get { return _sAplicacionId; }
            set { _sAplicacionId = value; }
        }
        public String RolTipoId
        {
            get { return _sRolTipoId; }
            set { _sRolTipoId = value; }
        }
        public String RolOpcion
        {
            get { return _sRolOpcion; }
            set { _sRolOpcion = value; }
        }
        public String RolOpcionOld
        {
            get { return _sRolOpcionOld; }
            set { _sRolOpcionOld = value; }
        }
        public String Nombre
        {
            get { return _sNombre; }
            set { _sNombre = value; }
        }
        public String Horario
        {
            get { return _sHorario; }
            set { _sHorario = value; }
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
