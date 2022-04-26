using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csUsuarioBE
    {
        #region Atributos
        private String _sUsuarioId;
        private String _sEmpresaId;
        private String _sAlias;
        private String _sClave;
        private String _sApellidoPaterno;
        private String _sApellidoMaterno;
        private String _sNombres;
        private String _sNombreCompleto;
        private String _sPersonaTipoId;
        private String _sDocumentoTipoId;
        private String _sDocumento;
        private String _sDocumentoNumero;
        private String _sCorreoElectronico;
        private String _sDireccionIP;
        private String _sEntidad;
        private String _sTelefono;
        private String _sDireccion;
        private String _sReferenciaId;
        private String _sFechaCaducidad;
        private String _sCodOficina;        
        private String _sNombrePais;
        private String _scodSistema;
        private String _scodusuarioRol;
        private String _scodRol;                                        
        private String _sAcceso;
        private String _scodAplicacion;
        private String _scodtipoRol;
        private String _sRolOpcion;
        private String _scodPais;
        private String _snombreRol;
        private String _sNombreOficina;
        private String _sNombreAcceso;
        private String _sNombreTipoRol;
        private String _sNombreSistema;                
        private String _sEstado;
        private String _sIPCreacion;
        private String _sIPModificacion;
        private String _sUsuarioModificacion;
        private String _sUsuarioCreacion;
        private string _sIPLocal;

        #endregion
        #region Propiedades
        public String UsuarioId
        {
            get { return _sUsuarioId; }
            set { _sUsuarioId = value; }
        }
        public String EmpresaId
        {
            get { return _sEmpresaId; }
            set { _sEmpresaId = value; }
        }
        public String Alias
        {
            get { return _sAlias; }
            set { _sAlias = value; }
        }
        public String Clave
        {
            get { return _sClave; }
            set { _sClave = value; }
        }
        public String ApellidoPaterno
        {
            get { return _sApellidoPaterno; }
            set { _sApellidoPaterno = value; }
        }
        public String ApellidoMaterno
        {
            get { return _sApellidoMaterno; }
            set { _sApellidoMaterno = value; }
        }
        public String Nombres
        {
            get { return _sNombres; }
            set { _sNombres = value; }
        }
        public String NombreCompleto
        {
            get { return _sNombreCompleto; }
            set { _sNombreCompleto = value; }
        }
        public String PersonaTipoId
        {
            get { return _sPersonaTipoId; }
            set { _sPersonaTipoId = value; }
        }
        public String DocumentoTipoId
        {
            get { return _sDocumentoTipoId; }
            set { _sDocumentoTipoId = value; }
        }
        public String Documento
        {
            get { return _sDocumento; }
            set { _sDocumento = value; }
        }
        public String DocumentoNumero
        {
            get { return _sDocumentoNumero; }
            set { _sDocumentoNumero = value; }
        }
        public String CorreoElectronico
        {
            get { return _sCorreoElectronico; }
            set { _sCorreoElectronico = value; }
        }
        public String DireccionIP
        {
            get { return _sDireccionIP; }
            set { _sDireccionIP = value; }
        }
        public String Entidad
        {
            get { return _sEntidad; }
            set { _sEntidad = value; }
        }
        public String Telefono
        {
            get { return _sTelefono; }
            set { _sTelefono = value; }
        }
        public String Direccion
        {
            get { return _sDireccion; }
            set { _sDireccion = value; }
        }
        public String ReferenciaId
        {
            get { return _sReferenciaId; }
            set { _sReferenciaId = value; }
        }
        public String FechaCaducidad
        {
            get { return _sFechaCaducidad; }
            set { _sFechaCaducidad = value; }
        }
        public String codOficina
        {
            get { return _sCodOficina; }
            set { _sCodOficina = value; }
        }
        public String NombreOficina
        {
            get { return _sNombreOficina; }
            set { _sNombreOficina = value; }
        }
        public String NombreAcceso
        {
            get { return _sNombreAcceso; }
            set { _sNombreAcceso = value; }
        }
        public String codPais
        {
            get { return _scodPais; }
            set { _scodPais = value; }
        }
        public String NombrePais
        {
            get { return _sNombrePais; }
            set { _sNombrePais = value; }
        }
        public String codSistema
        {
            get { return _scodSistema; }
            set { _scodSistema = value; }
        }
        public String codUsuarioRol
        {
            get { return _scodusuarioRol; }
            set { _scodusuarioRol = value; }
        }

        public String codRol
        {
            get { return _scodRol; }
            set { _scodRol = value; }
        }
        public String codTipoRol
        {
            get { return _scodtipoRol; }
            set { _scodtipoRol = value; }
        }
        public String RolOpcion
        {
            get { return _sRolOpcion; }
            set { _sRolOpcion = value; }
        }
        public String NombreRol
        {
            get { return _snombreRol; }
            set { _snombreRol = value; }
        }
        public String NombreTipoRol
        {
            get { return _sNombreTipoRol; }
            set { _sNombreTipoRol = value; }
        }

        public String Acceso
        {
            get { return _sAcceso; }
            set { _sAcceso = value; }
        }
        public String codAplicacion
        {
            get { return _scodAplicacion; }
            set { _scodAplicacion = value; }
        }
        public String NombreSistema
        {
            get { return _sNombreSistema; }
            set { _sNombreSistema = value; }
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

        public String IPLocal
        {
            get { return _sIPLocal; }
            set { _sIPLocal = value; }
        }

        
        #endregion

    }
}
