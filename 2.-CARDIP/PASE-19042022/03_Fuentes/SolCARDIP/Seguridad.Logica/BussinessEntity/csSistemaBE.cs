using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csSistemaBE
    {
        #region Atributos
        private String _sSistemaId;
        private String _snombre;
        private String _sdescripcion;
        private String _sabreviatura;
        private String _surlTest;
        private String _surlDesarrollo;
        private String _surlProduccion;
        private String _sorden;
        private String _sguid;
        private byte[] _bimagen;
        private String _sextension;
        private String _sEstado;
        private String _sIPCreacion;
        private String _sIPModificacion;
        private String _sUsuarioModificacion;
        private String _sUsuarioCreacion;
        #endregion

        #region Propiedades

        public String SistemaId
        {
            get { return _sSistemaId; }
            set { _sSistemaId = value; }
        }
        public String nombre
        {
            get { return _snombre; }
            set { _snombre = value; }
        }

        public String descripcion
        {
            get { return _sdescripcion; }
            set { _sdescripcion = value; }
        }
        public String abreviatura
        {
            get { return _sabreviatura; }
            set { _sabreviatura = value; }
        }
        public String urlTest
        {
            get { return _surlTest; }
            set { _surlTest = value; }
        }
        public String urlDesarrollo
        {
            get { return _surlDesarrollo; }
            set { _surlDesarrollo = value; }
        }
        public String urlProduccion
        {
            get { return _surlProduccion; }
            set { _surlProduccion = value; }
        }
        public String orden
        {
            get { return _sorden; }
            set { _sorden = value; }
        }
        public String guid
        {
            get { return _sguid; }
            set { _sguid = value; }
        }
        public byte[] imagen
        {
            get { return _bimagen; }
            set { _bimagen = value; }
        }
        public String extension
        {
            get { return _sextension; }
            set { _sextension = value; }
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
