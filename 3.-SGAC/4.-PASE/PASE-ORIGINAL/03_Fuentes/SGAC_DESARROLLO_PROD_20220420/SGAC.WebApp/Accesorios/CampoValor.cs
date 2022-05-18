using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAC.WebApp.Accesorios
{
    public class CampoValor
    {

        public CampoValor()
        {
        }

        public CampoValor(string nombreCampo, string valor)
        {
            _vNombreCampo = nombreCampo;
            _vValor = valor;
        }

        private string _vNombreCampo;

        public string vNombreCampo
        {
            get { return _vNombreCampo; }
            set { _vNombreCampo = value; }
        }

        private string _vValor;

        public string vValor
        {
            get { return _vValor; }
            set { _vValor = value; }
        }

    }
}