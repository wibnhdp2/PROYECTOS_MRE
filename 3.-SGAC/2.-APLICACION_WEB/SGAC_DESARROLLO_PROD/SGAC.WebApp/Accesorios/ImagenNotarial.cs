using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAC.WebApp.Accesorios
{
    public class ImagenNotarial
    {
        string _vRuta;

        public string vRuta
        {
            get { return _vRuta; }
            set { _vRuta = value; }
        }


        string _vTitulo;

        public string vTitulo
        {
            get { return _vTitulo; }
            set { _vTitulo = value; }
        }
    }
}