using System;

namespace SGAC.WinApp
{
    public class CL_SERVICIO
    {
        private int _serv_sServicioId;
        private int? _serv_sOficinaConsularId = null;
        private string _serv_vDescripcion;
        private int _serv_IOrden;
        private Int16? _serv_sServicioIdCab = null;
        private int? _serv_sTipo = null;
        private string _serv_cEstado;
        private int? _serv_sUsuarioCreacion = null;
        private string _serv_vIPCreacion = null;
        private DateTime? _serv_dFechaCreacion = null;
        private int? _serv_sUsuarioModificacion = null;
        private string _serv_vIPModificacion = null;
        private DateTime? _serv_dFechaModificacion = null;

        public int serv_sServicioId
        {
            get { return _serv_sServicioId; }
            set { _serv_sServicioId = value; }
        }

        public int? serv_sOficinaConsularId
        {
            get { return _serv_sOficinaConsularId; }
            set { _serv_sOficinaConsularId = value; }
        }

        public string serv_vDescripcion
        {
            get { return _serv_vDescripcion; }
            set { _serv_vDescripcion = value; }
        }

        public int serv_IOrden
        {
            get { return _serv_IOrden; }
            set { _serv_IOrden = value; }
        }

        public Int16? serv_sServicioIdCab
        {
            get { return _serv_sServicioIdCab; }
            set { _serv_sServicioIdCab = value; }
        }

        public int? serv_sTipo
        {
            get { return _serv_sTipo; }
            set { _serv_sTipo = value; }
        }

        public string serv_cEstado
        {
            get { return _serv_cEstado; }
            set { _serv_cEstado = value; }
        }

        public int? serv_sUsuarioCreacion
        {
            get { return _serv_sUsuarioCreacion; }
            set { _serv_sUsuarioCreacion = value; }
        }

        public string serv_vIPCreacion
        {
            get { return _serv_vIPCreacion; }
            set { _serv_vIPCreacion = value; }
        }

        public DateTime? serv_dFechaCreacion
        {
            get { return _serv_dFechaCreacion; }
            set { _serv_dFechaCreacion = value; }
        }

        public int? serv_sUsuarioModificacion
        {
            get { return _serv_sUsuarioModificacion; }
            set { _serv_sUsuarioModificacion = value; }
        }

        public string serv_vIPModificacion
        {
            get { return _serv_vIPModificacion; }
            set { _serv_vIPModificacion = value; }
        }

        public DateTime? serv_dFechaModificacion
        {
            get { return _serv_dFechaModificacion; }
            set { _serv_dFechaModificacion = value; }
        }
    }
}