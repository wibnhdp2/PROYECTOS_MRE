using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAC.WebApp.Accesorios
{
    public class DocumentoFirma
    {
        private string _vNombreCompleto;

        public string vNombreCompleto
        {
            get { return _vNombreCompleto; }
            set { _vNombreCompleto = value; }
        }

        private string _vNroDocumentoCompleto;

        public string vNroDocumentoCompleto
        {
            get { return _vNroDocumentoCompleto; }
            set { _vNroDocumentoCompleto = value; }
        }

        private bool _bAplicaHuellaDigital;

        public bool bAplicaHuellaDigital
        {
            get { return _bAplicaHuellaDigital; }
            set { _bAplicaHuellaDigital = value; }
        }

        private string _sFechaExpedicion;

        public string sFechaExpedicion
        {
            get { return _sFechaExpedicion; }
            set { _sFechaExpedicion = value; }
        }

        private bool _bImprimirFirma;

        public bool bImprimirFirma
        {
            get { return _bImprimirFirma; }
            set { _bImprimirFirma = value; }
        }

        private bool _bIncapacitado;

        public bool bIncapacitado
        {
            get { return _bIncapacitado; }
            set { _bIncapacitado = value; }
        }

        private string _vSubTipoActa;

        public string vSubTipoActa
        {
            get { return _vSubTipoActa; }
            set { _vSubTipoActa = value; }
        }

        private int _sTipoParticipante;

        public int sTipoParticipante
        {
            get { return _sTipoParticipante; }
            set { _sTipoParticipante = value; }
        }

        private string _vNombreCompletoRepresentar;

        public string vNombreCompletoRepresentar
        {
            get { return _vNombreCompletoRepresentar; }
            set { _vNombreCompletoRepresentar = value; }
        }

        private string _vNroDocumentoCompletoRepresentar;

        public string vNroDocumentoCompletoRepresentar
        {
            get { return _vNroDocumentoCompletoRepresentar; }
            set { _vNroDocumentoCompletoRepresentar = value; }
        }
    }
}
