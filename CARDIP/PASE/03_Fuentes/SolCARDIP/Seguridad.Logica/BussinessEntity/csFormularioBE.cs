using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csFormularioBE
    {
        #region Atributos
        private String _sFormularioId;
        private String _sAplicacionId;
        private String _sComponenteId;
        private String _sNombre;
        private String _sReferenciaId;
        private String _sRuta;
        private String _sImagen;
        private String _sOrden;
        private bool _bolVisible;
        private String _sEstado;
        private String _sIPCreacion;
        private String _sIPModificacion;
        private String _sUsuarioModificacion;
        private String _sUsuarioCreacion;
        #endregion
        #region Propiedades
        public String FormularioId
        {
            get { return _sFormularioId; }
            set { _sFormularioId = value; }
        }
        public String AplicacionId
        {
            get { return _sAplicacionId; }
            set { _sAplicacionId = value; }
        }
        public String ComponenteId
        {
            get { return _sComponenteId; }
            set { _sComponenteId = value; }
        }
        public String Nombre
        {
            get { return _sNombre; }
            set { _sNombre = value; }
        }
        public String ReferenciaId
        {
            get { return _sReferenciaId; }
            set { _sReferenciaId = value; }
        }
        public String Ruta
        {
            get { return _sRuta; }
            set { _sRuta = value; }
        }
        public String Imagen
        {
            get { return _sImagen; }
            set { _sImagen = value; }
        }
        public String Orden
        {
            get { return _sOrden; }
            set { _sOrden = value; }
        }
        public bool Visible
        {
            get { return _bolVisible; }
            set { _bolVisible = value; }
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
