using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.Accesorios
{
    public class TextoPosicionadoITextSharp
    {

        

        private string _texto;

        public string Texto
        {
            get { return _texto; }
            set { _texto = value; }
        }

        private iTextSharp.text.pdf.BaseFont _baseFont;

        public iTextSharp.text.pdf.BaseFont BaseFont
        {
            get { return _baseFont; }
            set { _baseFont = value; }
        }

        private iTextSharp.text.Font _fontFamily;

        public iTextSharp.text.Font FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }

        private float _fontSize;

        public float FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        private float fXPosition;

        public float FXPosition
        {
            get { return fXPosition; }
            set { fXPosition = value; }
        }

        private float fYPosition;

        public float FYPosition
        {
            get { return fYPosition; }
            set { fYPosition = value; }
        }

        private System.Web.UI.WebControls.HorizontalAlign _textAligment;

        public System.Web.UI.WebControls.HorizontalAlign TextAligment
        {
            get { return _textAligment; }
            set { _textAligment = value; }
        }

        private bool _bRepetir = true;

        public bool bRepetir
        {
            get { return _bRepetir; }
            set { _bRepetir = value; }
        }




    }
}
