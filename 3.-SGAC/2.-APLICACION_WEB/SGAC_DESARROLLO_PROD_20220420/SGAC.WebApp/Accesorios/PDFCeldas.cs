using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using iTextSharp.text;

public class PDFCeldas
{
        public string _texto { get; set; }
        public int _AlineamientoVertical { get; set; }
        public int _AlineamientoHorizontal { get; set; }
        public short _RepetirColumna { get; set; }
        public float _PaddingBottom { get; set; }
        public float _PaddingRight { get; set; }
        public float _Padding { get; set; }
        public int _Colspan { get; set; }
        public int _Border { get; set; }

        public PDFCeldas(string texto, int AlineamientoVertical, int AlineamientoHorizontal, short RepetirColumna, float PaddingBottom, float PaddingRight, float Padding)
        {
            _texto = texto;
            _AlineamientoVertical = AlineamientoVertical;
            _AlineamientoHorizontal = AlineamientoHorizontal;
            _RepetirColumna = RepetirColumna;
            _PaddingBottom = PaddingBottom;
            _PaddingRight = PaddingRight;
            _Padding = Padding;
            _Colspan = 1;
            _Border = 0;
        }
        public PDFCeldas(string texto, int AlineamientoVertical, int AlineamientoHorizontal, short RepetirColumna, float PaddingBottom, float PaddingRight, float Padding, int Colspan)
        {
            _texto = texto;
            _AlineamientoVertical = AlineamientoVertical;
            _AlineamientoHorizontal = AlineamientoHorizontal;
            _RepetirColumna = RepetirColumna;
            _PaddingBottom = PaddingBottom;
            _PaddingRight = PaddingRight;
            _Padding = Padding;
            _Colspan = Colspan;
            _Border = 0;
        }
        public PDFCeldas(string texto, int AlineamientoVertical, int AlineamientoHorizontal, short RepetirColumna, float PaddingBottom, float PaddingRight, float Padding, int Colspan, int Border)
        {
            _texto = texto;
            _AlineamientoVertical = AlineamientoVertical;
            _AlineamientoHorizontal = AlineamientoHorizontal;
            _RepetirColumna = RepetirColumna;
            _PaddingBottom = PaddingBottom;
            _PaddingRight = PaddingRight;
            _Padding = Padding;
            _Colspan = Colspan;
            _Border = Border;
        }

        public PDFCeldas(short RepetirColumna, float PaddingBottom)
        {
            _texto = "";
            _AlineamientoVertical = Element.ALIGN_MIDDLE;
            _AlineamientoHorizontal = Element.ALIGN_MIDDLE;
            _RepetirColumna = RepetirColumna;
            _PaddingBottom = PaddingBottom;
            _PaddingRight = 0f;
            _Padding = 0f;
            _Colspan = 1;
            _Border = 0;
        }

}
