using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using iTextSharp.text.pdf;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.BL;
using System.Configuration;

namespace SGAC.WebApp.Accesorios
{
    public class DocumentoiTextSharp
    {

        #region Constuctores

        public DocumentoiTextSharp(System.Web.UI.Page page, string sCuerpoHtml, string sLogoRuta)
        {
            _page = page;
            _sCuerpoHtml = sCuerpoHtml;
            _sLogoRuta = sLogoRuta;
        }

        #endregion

        #region Campos

        System.Web.UI.Page _page;

        const float fConstDocumentHeight = 842f;
        const float fConstDocumentWidth = 595f;

        #endregion

        #region Propiedades

        private List<string> _listOtorgantes = new List<string>();

        public List<string> ListOtorgantes
        {
            get { return _listOtorgantes; }
            set { _listOtorgantes = value; }
        }
        //----------------------------------------------------------------
        //Fecha: 28/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Imprimir Testimonio y la Escritura Pública
        //----------------------------------------------------------------

        private List<string> _listOtorgantesEC = new List<string>();

        public List<string> ListOtorgantesEC
        {
            get { return _listOtorgantesEC; }
            set { _listOtorgantesEC = value; }
        }

        private List<string> _listApoderados = new List<string>();

        public List<string> ListApoderados
        {
            get { return _listApoderados; }
            set { _listApoderados = value; }
        }

        //----------------------------------------------------------------
        //Fecha: 28/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Imprimir Testimonio y la Escritura Pública
        //----------------------------------------------------------------
        private List<string> _listApoderadosEC = new List<string>();

        public List<string> ListApoderadosEC
        {
            get { return _listApoderadosEC; }
            set { _listApoderadosEC = value; }
        }

        private List<string> _listInterpretes = new List<string>();

        public List<string> ListInterpretes
        {
            get { return _listInterpretes; }
            set { _listInterpretes = value; }
        }

        private List<string> _listInterpretesEC = new List<string>();

        public List<string> ListInterpretesEC
        {
            get { return _listInterpretesEC; }
            set { _listInterpretesEC = value; }
        }

        private List<string> _listTestigos = new List<string>();

        public List<string> ListTestigos
        {
            get { return _listTestigos; }
            set { _listTestigos = value; }
        }

        private List<string> _listTestigosEC = new List<string>();

        public List<string> ListTestigosEC
        {
            get { return _listTestigosEC; }
            set { _listTestigosEC = value; }
        }



        private List<ImagenNotarial> _listImagenes = new List<ImagenNotarial>();

        public List<ImagenNotarial> ListImagenes
        {
            get { return _listImagenes; }
            set
            {

                if (value == null)
                    _listImagenes = new List<ImagenNotarial>();
                else
                    _listImagenes = value;
            }
        }

        private bool _bEsBorrador = false;

        public bool bEsBorrador
        {
            get { return _bEsBorrador; }
            set { _bEsBorrador = value; }
        }

        private bool _bEsTestimonio = false;

        public bool bEsTestimonio
        {
            get { return _bEsTestimonio; }
            set { _bEsTestimonio = value; }
        }

        private bool _bEsEscrituraPublica;

        public bool bEsEscrituraPublica
        {
            get { return _bEsEscrituraPublica; }
            set { _bEsEscrituraPublica = value; }
        }

        private bool _bEsParte = false;

        public bool bEsParte
        {
            get { return _bEsParte; }
            set { _bEsParte = value; }
        }

        private bool _bEsVistaPrevia = false;

        public bool bEsVistaPrevia
        {
            get { return _bEsVistaPrevia; }
            set { _bEsVistaPrevia = value; }
        }

        private string _sMinutaEP = string.Empty;

        public string SMinutaEP
        {
            get { return _sMinutaEP; }
            set { _sMinutaEP = value; }
        }

        private string _sEtiquetaMinuta = string.Empty;

        public string sEtiquetaMinuta
        {
            get { return _sEtiquetaMinuta;  }
            set { _sEtiquetaMinuta = value; }
        }

        private string _sNombrePresentante = string.Empty;

        public string sNombrePresentante
        {
            get { return _sNombrePresentante; }
            set { _sNombrePresentante = value; }
        }

        private string _NumeroEP = string.Empty;

        public string NumeroEP
        {
            get { return _NumeroEP; }
            set { _NumeroEP = value; }
        }

        private string _TipoActoNotarial = string.Empty;

        public string TipoActoNotarial
        {
            get { return _TipoActoNotarial; }
            set { _TipoActoNotarial = value; }
        }

        List<TextoPosicionadoITextSharp> _listTextoPosicionadoiTextSharp = new List<TextoPosicionadoITextSharp>();

        public List<TextoPosicionadoITextSharp> ListTextoPosicionadoiTextSharp
        {
            get { return _listTextoPosicionadoiTextSharp; }
            set { _listTextoPosicionadoiTextSharp = value; }
        }

        List<DocumentoFirma> _listDocumentoFirma = new List<DocumentoFirma>();

        public List<DocumentoFirma> ListDocumentoFirma
        {
            get { return _listDocumentoFirma; }
            set { _listDocumentoFirma = value; }
        }

        List<DocumentoFirma> _listDocumentoFirmaEC = new List<DocumentoFirma>();

        public List<DocumentoFirma> ListDocumentoFirmaEC
        {
            get { return _listDocumentoFirmaEC; }
            set { _listDocumentoFirmaEC = value; }
        }



        List<String> _listPalabrasOmitirTextoNotarial = new List<String>();

        public List<String> ListPalabrasOmitirTextoNotarial
        {
            get { return _listPalabrasOmitirTextoNotarial; }
            set { _listPalabrasOmitirTextoNotarial = value; }
        }

        private iTextSharp.text.pdf.BaseFont _cuerpoBaseFont;

        public iTextSharp.text.pdf.BaseFont CuerpoBaseFont
        {
            get { return _cuerpoBaseFont; }
            set { _cuerpoBaseFont = value; }
        }

        //-------------------------------------------------------------
        //Fecha: 10/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: personalizar el Font del Titulo
        //-------------------------------------------------------------
        private iTextSharp.text.pdf.BaseFont _tituloBaseFont;

        public iTextSharp.text.pdf.BaseFont TituloBaseFont
        {
            get { return _tituloBaseFont; }
            set { _tituloBaseFont = value; }
        }
        //-------------------------------------------------------------


        string _sTitulo = string.Empty;

        public string sTitulo
        {
            get { return _sTitulo; }
            set { _sTitulo = value; }
        }

        string _sTituloEC = string.Empty;

        public string sTituloEC
        {
            get { return _sTituloEC; }
            set { _sTituloEC = value; }
        }

        string _sFecha = string.Empty;
        public string sFecha
        {
            get { return _sFecha; }
            set { _sFecha = value; }
        }

        string _sCuerpoHtml;

        string _sCuerpoHtmlEC;
        public string sCuerpoHtmlEC
        {
            get { return _sCuerpoHtmlEC; }
            set { _sCuerpoHtmlEC = value; }
        }

        float fDocumentPosition = 0;

        float _fMarginLeft = 100;

        public float FMarginLeft
        {
            get { return _fMarginLeft; }
            set { _fMarginLeft = value; }
        }

        float _fMarginRight = 100;

        public float FMarginRight
        {
            get { return _fMarginRight; }
            set { _fMarginRight = value; }
        }

        float _fMarginTop = 100;

        public float FMarginTop
        {
            get { return _fMarginTop; }
            set { _fMarginTop = value; }
        }

        float _fMarginBottom = 100;

        public float FMarginBottom
        {
            get { return _fMarginBottom; }
            set { _fMarginBottom = value; }
        }

        float _fDocumentTextArea;

        public float fDocumentTextArea
        {
            get
            {
                return fConstDocumentWidth - (FMarginLeft + FMarginRight);
            }

        }

        float FCuerpoLeading
        {
            get
            {


                return (fDocumentPosition - (_fMarginBottom)) / (_fCuerpoFontSize * _iLineNumber);
            }

        }
      float _fCuerpoFontSize = 12;
      

        public float FCuerpoFontSize
        {
            get { return _fCuerpoFontSize; }
            set { _fCuerpoFontSize = value; }
        }

        float _iLineNumber = 25f;

        public float iLineNumber
        {
            get { return _iLineNumber; }
            set { _iLineNumber = value; }
        }

        int _iLineNumberGeneral = 25;


        string _sLogoRuta = string.Empty;

        public string SLogoRuta
        {
            get { return _sLogoRuta; }
            set { _sLogoRuta = value; }
        }

        string _nombreConsulado = string.Empty;

        public string NombreConsulado
        {
            get { return _nombreConsulado; }
            set { _nombreConsulado = value; }
        }

        private int _iCantOtorgantes = 1;

        public int iCantOtorgantes
        {
            get { return _iCantOtorgantes; }
            set { _iCantOtorgantes = value; }
        }

        
        private int _iFojaActual = 0;

        public int iFojaActual
        {
            get { return _iFojaActual; }
            set { _iFojaActual = value; }
        }

        private int _iFojaRestante = 0;

        public int iFojaRestante
        {
            get { return _iFojaRestante; }
            set { _iFojaRestante = value; }
        }

        bool _bAplicaCierreTextoNotarial = true;

        bool _bGenerarDocumentoAutomaticamente = true;

        public bool bGenerarDocumentoAutomaticamente
        {
            get { return _bGenerarDocumentoAutomaticamente; }
            set { _bGenerarDocumentoAutomaticamente = value; }
        }

        public bool bAplicaCierreTextoNotarial
        {
            get { return _bAplicaCierreTextoNotarial; }
            set { _bAplicaCierreTextoNotarial = value; }
        }

        string _nombreFuncionario = string.Empty;

        public string NombreFuncionario
        {
            get { return _nombreFuncionario; }
            set { _nombreFuncionario = value; }
        }

        int _iPageNumber = 1;

        public int IPageNumber
        {
            get { return _iPageNumber; }
        }
        bool _bEsSolaUnaHoja = false;

        public bool bEsSolaUnaHoja
        {
            get { return _bEsSolaUnaHoja; }
            set { _bEsSolaUnaHoja = value; }
        }
        #endregion

        #region Propiedades Autoadhesivo
        private bool _Notarial = false;
        public bool Notarial
        {
            get { return _Notarial; }
            set { _Notarial = value; }
        }

        private Int64 _ActuacionId = 0;
        public Int64 ActuacionId
        {
            get { return _ActuacionId; }
            set { _ActuacionId = value; }
        }

        private Int64 _ActuacionDetalleId = 0;
        public Int64 ActuacionDetalleId
        {
            get { return _ActuacionDetalleId; }
            set { _ActuacionDetalleId = value; }
        }

        private int _CantidadTarifas = 1;
        public int CantidadTarifas
        {
            get { return _CantidadTarifas; }
            set { _CantidadTarifas = value; }
        }
        #endregion

        #region Enumerador
        public enum ModoProceso
        {
            Binario = 0,
            Texto = 1,
            Automatico = 2
        };
        public enum ErrorCorreccion
        {
            Nivel0 = 0,
            Nivel1 = 1,
            Nivel2 = 2,
            Nivel3 = 3,
            Nivel4 = 4,
            Nivel5 = 5,
            Nivel6 = 6,
            Nivel7 = 7,
            Nivel8 = 8
        };
        public enum Fuente
        {
            MW6_PDF417R3 = 1,
            MW6_PDF417R4 = 2,
            MW6_PDF417R5 = 3,
            MW6_PDF417R6 = 4
        }
        #endregion

        #region Metodos


        private void CrearTabla(DataTable tabla, iTextSharp.text.Document document, List<AlinearColumnaTabla> listColumnaAlineacion = null)
        {
            try
            {
                iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font _fontGeneralB = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);

                PdfPTable table = new PdfPTable(tabla.Columns.Count);

                table.WidthPercentage = 100;
                float[] fWidths = new float[tabla.Columns.Count];

                if (listColumnaAlineacion == null)
                    listColumnaAlineacion = new List<AlinearColumnaTabla>();

                int y = 0;
                foreach (DataColumn dc in tabla.Columns)
                {
                    string nombreColumna = dc.Caption.Replace("_", " ").ToUpper();
                    PdfPCell cell = new PdfPCell(new iTextSharp.text.Phrase(nombreColumna, _fontGeneralB));
                    cell.BorderWidth = 0;
                    float ancho = new iTextSharp.text.Chunk(nombreColumna, _fontGeneralB).GetWidthPoint();

                    #region Alineacion



                    var auxAlineacion = listColumnaAlineacion.Where(x => x.vNombreColumna.ToLower() == dc.ColumnName.ToLower());
                    if (auxAlineacion.Count() > 0)
                    {
                        cell.HorizontalAlignment = auxAlineacion.ElementAt(0).iAlineacion;
                    }
                    else
                    {
                        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    }

                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                    #endregion


                    cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    fWidths[y] = ancho;
                    table.AddCell(cell);
                    y++;
                }

                foreach (DataRow dr in tabla.Rows)
                {
                    for (int i = 0; i < tabla.Columns.Count; i++)
                    {
                        PdfPCell cell = new PdfPCell(new iTextSharp.text.Phrase(dr[i].ToString(), _fontGeneral));
                        cell.BorderWidth = 0;
                        float ancho = new iTextSharp.text.Chunk(dr[i].ToString(), _fontGeneral).GetWidthPoint();

                        #region Alineacion

                        var auxAlineacion = listColumnaAlineacion.Where(x => x.vNombreColumna.ToLower() == tabla.Columns[i].ColumnName.ToLower());
                        if (auxAlineacion.Count() > 0)
                        {
                            cell.HorizontalAlignment = auxAlineacion.ElementAt(0).iAlineacion;
                        }
                        else
                        {
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        }

                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                        #endregion

                        if (ancho > fWidths[i])
                            fWidths[i] = ancho;
                        table.AddCell(cell);
                    }
                }

                table.SetWidths(fWidths);


                document.Add(table);
            }
            catch (Exception ex)
            {
                Comun.EjecutarScript(this._page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "DocumentoiTextSharp: CrearTabla/", ex.Message));
            }
        }

        #region Contabilidad: Estado de Cuenta
        public void CrearResumenBancario(DataTable tabla, string vNumeroCuenta, DateTime dFecha, string sMonedaId)
        {
            try
            {
                #region Inicializando Variables

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                float fMargenIzquierdaDoc = 50;
                float fMargenDerechaDoc = 50;
                float fMargenInferiorDoc = 100;
                float fMargenSuperiorDoc = 100;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                #endregion

                using (var ms = new MemoryStream())
                {

                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    ");
                    str.Flush();


                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);


                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    if (bEsBorrador)
                    {
                        PdfWriterEvents writerEvent = new PdfWriterEvents("Hoja borrador. Sin valor legal");
                        writer.PageEvent = writerEvent;
                    }

                    document.Open();

                    iTextSharp.text.Font _fontGeneralB = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);


                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    float fPosicion = fMargenSuperiorDoc;

                    iTextSharp.text.Paragraph oParagraph = new iTextSharp.text.Paragraph();

                    DataTable dtMoneda = comun_Part1.ObtenerParametrosPorGrupo(HttpContext.Current.Session, Enumerador.enmMaestro.MONEDA);
                    var auxMoneda = dtMoneda.AsEnumerable().Where(x => x["id"].ToString() == sMonedaId);
                    string vSimboloMoneda = string.Empty;
                    if (auxMoneda.Count() > 0)
                    {
                        vSimboloMoneda = auxMoneda.ElementAt(0)["valor"].ToString();
                    }


                    List<AlinearColumnaTabla> listAlineacionColumna = new List<AlinearColumnaTabla>();
                    listAlineacionColumna.Add(new AlinearColumnaTabla("Monto", PdfPCell.ALIGN_RIGHT));



                    oParagraph.Add(new iTextSharp.text.Chunk("REPORTE RESUMEN DE BANCOS\n\n", _fontGeneralB));
                    oParagraph.Alignment = PdfPCell.ALIGN_CENTER;
                    document.Add(oParagraph);

                    oParagraph.Clear();


                    #region ingresos

                    oParagraph.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    oParagraph.Add(new iTextSharp.text.Chunk("Cuenta N° " + vNumeroCuenta + "\n", _fontGeneral));
                    oParagraph.Add(new iTextSharp.text.Chunk("Saldo según estado bancario al " +
                        dFecha.Day.ToString().PadLeft(2, '0') + " de " + Comun.ObtenerMesNombre(dFecha.Month) + " de " + dFecha.Year + "\n", _fontGeneral));
                    oParagraph.Add(new iTextSharp.text.Chunk("\nMás:" + "\n", _fontGeneral));
                    oParagraph.Add(new iTextSharp.text.Chunk("INGRESOS" + "\n\n", _fontGeneralB));
                    document.Add(oParagraph);
                    oParagraph.Clear();

                    DataTable dt = new DataTable();
                    if (tabla.Rows.Count > 0)
                    {
                        dt = tabla.AsEnumerable().Where(x => x["tran_sMovimientoTipoId"].ToString() ==
                        Convert.ToInt16(Enumerador.enmTipoMovimientoTransaccion.INGRESOS).ToString()).CopyToDataTable();

                        dt.Columns.Remove("tran_sMovimientoTipoId");
                        dt.Columns.Remove("tran_sTipoId");
                        dt.Columns.Remove("movimiento");
                        dt.Columns["Monto"].Caption += " (" + vSimboloMoneda + ")";

                        CrearTabla(dt, document, listAlineacionColumna);
                    }

                    frase = new iTextSharp.text.Phrase();
                    frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 10.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_RIGHT, 1));
                    parrafo.Add(frase);
                    document.Add(parrafo);
                    oParagraph.Clear();

                    decimal ingresos = 0;
                    if (dt.Rows.Count > 0)
                    {
                        ingresos = dt.AsEnumerable().Sum(x => Convert.ToDecimal(x["Monto"]));
                    }

                    oParagraph.Add(new iTextSharp.text.Chunk(("\n" + string.Format("{0:0.000}", ingresos)) + "\n\n", _fontGeneralB));
                    oParagraph.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;

                    document.Add(oParagraph);
                    oParagraph.Clear();
                    oParagraph.Alignment = iTextSharp.text.Element.ALIGN_LEFT;

                    #endregion

                    #region Egresos


                    oParagraph.Add(new iTextSharp.text.Chunk("Menos:" + "\n", _fontGeneral));
                    oParagraph.Add(new iTextSharp.text.Chunk("EGRESOS\n\n", _fontGeneralB));
                    document.Add(oParagraph);
                    oParagraph.Clear();

                    if (tabla.Rows.Count > 0)
                    {
                        var auxEgreso = tabla.AsEnumerable().Where(x => x["tran_sMovimientoTipoId"].ToString() ==
                                               Convert.ToInt16(Enumerador.enmTipoMovimientoTransaccion.SALIDAS).ToString());
                        if (auxEgreso.Count() > 0)
                        {
                            dt = tabla.AsEnumerable().Where(x => x["tran_sMovimientoTipoId"].ToString() ==
                           Convert.ToInt16(Enumerador.enmTipoMovimientoTransaccion.SALIDAS).ToString()).CopyToDataTable();
                            dt.Columns.Remove("tran_sMovimientoTipoId");
                            dt.Columns.Remove("tran_sTipoId");
                            dt.Columns.Remove("movimiento");
                            dt.Columns["Monto"].Caption += " (" + vSimboloMoneda + ")";
                        }
                        else
                        {
                            dt.Clear();
                            dt.Rows.Add("", "0");

                        }

                        CrearTabla(dt, document, listAlineacionColumna);
                    }

                    frase = new iTextSharp.text.Phrase();
                    frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 10.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_RIGHT, 1));
                    parrafo.Add(frase);
                    document.Add(parrafo);
                    oParagraph.Clear();

                    decimal egresos = 0;
                    if (dt.Rows.Count > 0)
                    {
                        egresos = dt.AsEnumerable().Sum(x => Convert.ToDecimal(x["Monto"]));
                    }

                    oParagraph.Add(new iTextSharp.text.Chunk(("\n" + string.Format("{0:0.000}", egresos)) + "\n\n", _fontGeneralB));
                    oParagraph.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    document.Add(oParagraph);
                    oParagraph.Clear();

                    #endregion

                    #region Saldo

                    dt = new DataTable();
                    dt.Columns.Add("Saldo");
                    dt.Columns[0].Caption += " (" + vSimboloMoneda + ")";

                    listAlineacionColumna = new List<AlinearColumnaTabla>();
                    listAlineacionColumna.Add(new AlinearColumnaTabla("saldo", PdfPCell.ALIGN_RIGHT));

                    CrearTabla(dt, document, listAlineacionColumna);

                    oParagraph.Add(new iTextSharp.text.Chunk(string.Format("{0:0.000}", (ingresos - egresos)) + "\n\n", _fontGeneralB));
                    oParagraph.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    document.Add(oParagraph);
                    oParagraph.Clear();

                    #endregion

                    document.Close();

                    #region Impresion en Navegador

                    Byte[] FileBuffer = ms.ToArray();
                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                        if (bGenerarDocumentoAutomaticamente)
                        {
                            GenerarDocumentoPDF();
                        }
                    }

                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();

                    #endregion

                }
            }
            catch (Exception ex)
            {
                Comun.EjecutarScript(this._page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "DocumentoiTextSharp: CrearResumenBancario/", ex.Message));
            }
        }

        #endregion

        public void CrearAutoAdhesivo()
        {
            try
            {
                #region Inicializando Variables

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                bool bAplicarImagen = false;

                float fImageMargin = 10f;

                float fMargenIzquierdaDoc = 0;
                float fMargenDerechaDoc = 0;
                float fMargenInferiorDoc = 0;
                float fMargenSuperiorDoc = 0;//(842 - (_fCuerpoFontSize * fTextLeading * _iLineNumber) - fMargenInferiorDoc);


                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(297f, 419.5f), fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);
                //document.PageSize.BackgroundColor = new iTextSharp.text.BaseColor(0, 255, 255);

                iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                #endregion

                using (var ms = new MemoryStream())
                {

                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    ");
                    str.Flush();


                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);


                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    if (bEsBorrador)
                    {
                        PdfWriterEvents writerEvent = new PdfWriterEvents("Hoja borrador. Sin valor legal");
                        writer.PageEvent = writerEvent;
                    }

                    document.Open();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    float fPosicion = fMargenSuperiorDoc;
                    float fTextSize = 0;

                    #region Imagen

                    bool bAplicaFondo = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["AplicarImagenFondo"].ToString());

                    if (bAplicaFondo)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/Autoadhesivo.jpg"));
                        img.SetAbsolutePosition(0, 0);
                        img.ScaleAbsoluteHeight(419.5f);
                        img.ScaleAbsoluteWidth(297f);
                        document.Add(img);
                    }

                    #endregion


                    float fMargenIzquierda = float.Parse(System.Configuration.ConfigurationManager.AppSettings["MarginLeft"].ToString());
                    float fMargenSuperior = float.Parse(System.Configuration.ConfigurationManager.AppSettings["MarginTop"].ToString());
                    float fFontSize = float.Parse(System.Configuration.ConfigurationManager.AppSettings["FontSize"].ToString());
                    float fInterlineado = float.Parse(System.Configuration.ConfigurationManager.AppSettings["LineSpacing"].ToString());
                    float fBarcodeHeight = float.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeHeight"].ToString());
                    float fBarcodeWidth = float.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeWidth"].ToString());
                    float fBarcodeLeft = float.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeLeft"].ToString());
                    float fBarcodeTop = float.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeTop"].ToString());
                    float fBarCodeFontSize = float.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeFontSize"].ToString());
                    float fNombreConsuladoAncho = float.Parse(System.Configuration.ConfigurationManager.AppSettings["NombreConsuladoAncho"].ToString());


                    string vLinea1Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea1Titulo"].ToString();
                    bool bLinea1TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea1TituloNegrita"].ToString());
                    bool bLinea1TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea1TextoNegrita"].ToString());

                    string vLinea2Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea2Titulo"].ToString();
                    bool bLinea2TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea2TituloNegrita"].ToString());
                    bool bLinea2TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea2TextoNegrita"].ToString());

                    string vLinea3Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea3Titulo"].ToString();
                    bool bLinea3TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea3TituloNegrita"].ToString());
                    bool bLinea3TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea3TextoNegrita"].ToString());

                    string vLinea4Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea4Titulo"].ToString();
                    bool bLinea4TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea4TituloNegrita"].ToString());
                    bool bLinea4TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea4TextoNegrita"].ToString());

                    string vLinea5Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea5Titulo"].ToString();
                    bool bLinea5TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea5TituloNegrita"].ToString());
                    bool bLinea5TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea5TextoNegrita"].ToString());

                    string vLinea6Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea6Titulo"].ToString();
                    bool bLinea6TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea6TituloNegrita"].ToString());
                    bool bLinea6TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea6TextoNegrita"].ToString());

                    string vLinea7Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea7Titulo"].ToString();
                    bool bLinea7TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea7TituloNegrita"].ToString());
                    bool bLinea7TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea7TextoNegrita"].ToString());

                    string vLinea8Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea8Titulo"].ToString();
                    bool bLinea8TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea8TituloNegrita"].ToString());
                    bool bLinea8TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea8TextoNegrita"].ToString());



                    PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/arialn.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                    iTextSharp.text.pdf.BaseFont baseFontB = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/arialnb.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                    #region Data : tipo Acto
                    ActuacionConsultaBL objBL = new ActuacionConsultaBL();
                    DataTable dtDatos;
                    if (_Notarial)
                    {
                        dtDatos = objBL.ObtenerDatosAutoadhesivoNotarial(_ActuacionId, _ActuacionDetalleId, _CantidadTarifas);
                    }
                    else
                    {
                        dtDatos = objBL.ActuacionAutuadhesivo(_ActuacionDetalleId);
                    }
                    #endregion

                    if (dtDatos != null)
                    {
                        if (dtDatos.Rows.Count > 0)
                        {
                            String TarifaAutoAdhesivo = string.Empty;
                            if (dtDatos.Rows[0]["numTarifa"].ToString().Equals("0"))
                            {
                                TarifaAutoAdhesivo = dtDatos.Rows[0]["letraTarifa"].ToString();
                            }
                            else
                            {
                                TarifaAutoAdhesivo = dtDatos.Rows[0]["numTarifa"].ToString().PadLeft(2, '0') +
                                                        dtDatos.Rows[0]["letraTarifa"].ToString();
                                //                            + " - Corr: " +
                                //                            dtDatos.Rows[0]["iCorrtarifa"].ToString().PadLeft(4, '0');
                            }

                            Double t_1 = Math.Round((Convert.ToDouble(dtDatos.Rows[0]["fMontoSolesConsulares"]) / (dtDatos.Rows.Count)), 2);
                            Double t_2 = Math.Round((Convert.ToDouble(dtDatos.Rows[0]["fMontoMonedaLocal"]) / (dtDatos.Rows.Count)), 2);

                            String t1 = String.Format("{0:#,###0.00}", t_1);
                            String t2 = String.Format("{0:#,###0.00}", t_2);


                            short sMultiplo = 0;

                            #region NombreConsulado

                            string texto = string.Empty;

                            float pos = 0;
                            float tamPalabra = 0;
                            float ancho = fNombreConsuladoAncho;

                            NombreConsulado = dtDatos.Rows[0]["vNomOficConsul"].ToString();
                            float posxAcumulado = tamPalabra;
                            iTextSharp.text.Font fontConsulado = new iTextSharp.text.Font(baseFont);
                            foreach (string palabra in NombreConsulado.Split(' '))
                            {
                                tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                                if (posxAcumulado + tamPalabra > ancho)
                                {

                                    //cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 105 + pos);
                                    //cb.ShowText(texto.Trim());



                                    EscribirTexto(texto.Trim(), cb, document, baseFontB, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - fInterlineado * sMultiplo);
                                    EscribirTexto(texto.Trim(), cb, document, baseFontB, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior - 190) - fInterlineado * sMultiplo);
                                    texto = string.Empty;

                                    posxAcumulado = 0;
                                    sMultiplo++;
                                }

                                posxAcumulado += tamPalabra;
                                posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                                texto += " " + palabra;
                            }

                            if (texto.Trim() != string.Empty)
                            {
                                if (sMultiplo < 2)
                                {
                                    EscribirTexto(texto.Trim(), cb, document, baseFontB, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - fInterlineado * sMultiplo);
                                    EscribirTexto(texto.Trim(), cb, document, baseFontB, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior - 190) - fInterlineado * sMultiplo);
                                    sMultiplo++;
                                }
                                //cb.SetTextMatrix(fImageMargin + (ancho / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 105 + pos);
                                //cb.ShowText(texto.Trim());
                            }

                            #endregion

                            #region Linea 1

                            if (vLinea1Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea1Titulo,
                                    cb, document, bLinea1TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda,
                                    (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 1)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea1Titulo, bLinea1TituloNegrita) +
                                Convert.ToDateTime(dtDatos.Rows[0]["dFecha"]).ToString("MMM-dd-yyyy").ToUpper(), cb, document, bLinea1TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 1)));

                            #endregion

                            #region Linea 2

                            if (vLinea2Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea2Titulo, cb, document, bLinea2TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 2)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea2Titulo, bLinea2TituloNegrita) +
                                dtDatos.Rows[0]["vSolicitante"].ToString(), cb, document, bLinea2TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 2)));


                            #endregion

                            #region Linea 3

                            if (vLinea3Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea3Titulo, cb, document, bLinea3TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 3)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea3Titulo, bLinea3TituloNegrita) +
                                dtDatos.Rows[0]["vNombreActuacion"].ToString(), cb, document, bLinea3TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 3)));

                            #endregion

                            #region Linea 4

                            if (vLinea4Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea4Titulo, cb, document, bLinea4TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 4)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea4Titulo, bLinea4TituloNegrita) + dtDatos.Rows[0]["iCorrTarifa"].ToString(), cb, document, bLinea4TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 4)));

                            #endregion

                            #region Linea 5

                            if (vLinea5Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea5Titulo, cb, document, bLinea5TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 5)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea5Titulo, bLinea5TituloNegrita) + dtDatos.Rows[0]["vRGE"].ToString(), cb, document, bLinea5TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 5)));

                            #endregion

                            #region Linea 6

                            if (vLinea6Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea6Titulo, cb, document, bLinea6TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 6)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea6Titulo, bLinea6TituloNegrita) + TarifaAutoAdhesivo, cb, document, bLinea6TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 6)));


                            #endregion

                            #region Linea 7

                            if (vLinea7Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea7Titulo, cb, document, bLinea7TituloNegrita ? baseFontB : bLinea1TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 7)));

                            }


                            EscribirTexto(ObtenerEspacio(vLinea7Titulo, bLinea7TituloNegrita) + t1 + " SOLES CONSULARES", cb, document, bLinea7TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 7)));


                            #endregion

                            #region Linea 8

                            if (vLinea8Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea8Titulo, cb, document, bLinea8TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                                EscribirTexto(ObtenerEspacio(vLinea8Titulo, bLinea8TituloNegrita) + t2 + " " + dtDatos.Rows[0]["vMoneda"], cb, document, baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                            }
                            else
                            {
                                EscribirTexto(vLinea8Titulo, cb, document, baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                                EscribirTexto(ObtenerEspacio(vLinea7Titulo, bLinea7TituloNegrita) + t2 + " " + dtDatos.Rows[0]["vMoneda"], cb, document, bLinea8TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                            }





                            #endregion                            _iPageNumber = cb.PdfWriter.PageNumber;





                            #region Imagen Codigo


                            DataRow drSeleccionado = dtDatos.Rows[0];
                            string strCadenaCodifica = string.Empty;

                            strCadenaCodifica = drSeleccionado["vNomOficConsul"].ToString() + "|" +
                                            Convert.ToDateTime(drSeleccionado["dFecha"]).ToString("dd/MM/yyyy") + "|" +
                                            drSeleccionado["vRGE"].ToString() + "|" +
                                            drSeleccionado["vSolicitante"].ToString();
                            strCadenaCodifica = strCadenaCodifica + "|" +
                                drSeleccionado["vNombreActuacion"].ToString() + "|" +
                                //DtDatosSticker.Rows[0]["numTarifa"].ToString().PadRight(2, '0') + "/" + DtDatosSticker.Rows[0]["iCorrtarifa"].ToString().PadRight(4, '0') + "|" + 
                                drSeleccionado["iCorrtarifa"].ToString() + "|" +
                                drSeleccionado["fMontoSolesConsulares"].ToString() + " SOLES CONSULARES";

                            strCadenaCodifica = strCadenaCodifica + "|" +
                                drSeleccionado["fMontoMonedaLocal"].ToString() + " " +
                                drSeleccionado["vMoneda"].ToString() + "|" +
                                drSeleccionado["sUsuarioAlias"].ToString() + "|" +
                                DateTime.Now.ToString("MMM-dd-yyyy") + "|";

                            System.Drawing.Bitmap objLienzo;
                            string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                            string imgCodePath = uploadPath + @"\PDF" + DateTime.Now.Ticks.ToString() + ".bmp";
                            objLienzo = GenerarImagenPDF417(strCadenaCodifica, fBarCodeFontSize);
                            objLienzo.Save(imgCodePath, System.Drawing.Imaging.ImageFormat.Png);

                            iTextSharp.text.Image imgCode = iTextSharp.text.Image.GetInstance(imgCodePath);
                            imgCode.SetAbsolutePosition(fMargenIzquierda + fBarcodeLeft, (document.PageSize.Height - fMargenSuperior - 10) - (fInterlineado * 8) - 30 - 15 - fBarcodeTop);
                            imgCode.ScaleAbsoluteHeight(fBarcodeHeight);
                            imgCode.ScaleAbsoluteWidth(fBarcodeWidth);
                            document.Add(imgCode);

                            #endregion

                            fMargenSuperior += 190;


                            #region Linea 1

                            if (vLinea1Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea1Titulo,
                                    cb, document, bLinea1TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda,
                                    (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 1)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea1Titulo, bLinea1TituloNegrita) +
                                Convert.ToDateTime(dtDatos.Rows[0]["dFecha"]).ToString("MMM-dd-yyyy").ToUpper(), cb, document, bLinea1TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 1)));

                            #endregion

                            #region Linea 2

                            if (vLinea2Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea2Titulo, cb, document, bLinea2TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 2)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea2Titulo, bLinea2TituloNegrita) +
                                dtDatos.Rows[0]["vSolicitante"].ToString(), cb, document, bLinea2TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 2)));


                            #endregion

                            #region Linea 3

                            if (vLinea3Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea3Titulo, cb, document, bLinea3TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 3)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea3Titulo, bLinea3TituloNegrita) +
                                dtDatos.Rows[0]["vNombreActuacion"].ToString(), cb, document, bLinea3TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 3)));

                            #endregion

                            #region Linea 4

                            if (vLinea4Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea4Titulo, cb, document, bLinea4TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 4)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea4Titulo, bLinea4TituloNegrita) + dtDatos.Rows[0]["iCorrTarifa"].ToString(), cb, document, bLinea4TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 4)));

                            #endregion

                            #region Linea 5

                            if (vLinea5Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea5Titulo, cb, document, bLinea5TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 5)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea5Titulo, bLinea5TituloNegrita) + dtDatos.Rows[0]["vRGE"].ToString(), cb, document, bLinea5TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 5)));

                            #endregion

                            #region Linea 6

                            if (vLinea6Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea6Titulo, cb, document, bLinea6TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 6)));

                            }

                            EscribirTexto(ObtenerEspacio(vLinea6Titulo, bLinea6TituloNegrita) + TarifaAutoAdhesivo, cb, document, bLinea6TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 6)));


                            #endregion

                            #region Linea 7

                            if (vLinea7Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea7Titulo, cb, document, bLinea7TituloNegrita ? baseFontB : bLinea1TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 7)));

                            }


                            EscribirTexto(ObtenerEspacio(vLinea7Titulo, bLinea7TituloNegrita) + t1 + " SOLES CONSULARES", cb, document, bLinea7TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 7)));


                            #endregion

                            #region Linea 8

                            if (vLinea8Titulo != string.Empty)
                            {
                                EscribirTexto(vLinea8Titulo, cb, document, bLinea8TituloNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                                EscribirTexto(ObtenerEspacio(vLinea8Titulo, bLinea8TituloNegrita) + t2 + " " + dtDatos.Rows[0]["vMoneda"], cb, document, baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                            }
                            else
                            {
                                EscribirTexto(vLinea8Titulo, cb, document, baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                                EscribirTexto(ObtenerEspacio(vLinea7Titulo, bLinea7TituloNegrita) + t2 + " " + dtDatos.Rows[0]["vMoneda"], cb, document, bLinea8TextoNegrita ? baseFontB : baseFont, fFontSize, fMargenIzquierda, (document.PageSize.Height - fMargenSuperior) - (fInterlineado * (sMultiplo + 8)));
                            }





                            #endregion                            _iPageNumber = cb.PdfWriter.PageNumber;

                            document.Close();

                            #region Impresion en Navegador

                            Byte[] FileBuffer = ms.ToArray();
                            if (FileBuffer != null)
                            {
                                System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                                if (bGenerarDocumentoAutomaticamente)
                                {
                                    GenerarDocumentoPDF();
                                }
                            }

                            str.Close();
                            oStreamReader.Close();
                            oStreamReader.Dispose();

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Comun.EjecutarScript(this._page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "DocumentoiTextSharp: CrearAutoAdhesivo/", ex.Message));
            }
        }

        private string ObtenerEspacio(string texto, bool bEsNegrita)
        {
            float multiplo = 0.8f;

            if (bEsNegrita)
                multiplo = 0.9f;

            if (texto.Length >= 18)
                multiplo -= 0.025f;
            else if (texto.Length <= 6)
                multiplo += 0.025f;


            return string.Empty.PadLeft(texto.Length + (int)(texto.Length * multiplo));
        }


        static Bitmap GenerarImagenPDF417(string strTexto, float fontHeight)
        {

            Bitmap objLienzo;
            Graphics objGraficar;
            System.Drawing.Font objFont;
            Point objCoordenadas;
            SolidBrush objPincelFondo;
            SolidBrush objPincelTexto;



            GenerarCodigo(ref strTexto, ModoProceso.Binario, ErrorCorreccion.Nivel4, 2, 6, false, false, (int)fontHeight, Fuente.MW6_PDF417R4);

            objLienzo = new Bitmap(800, 400);
            objGraficar = Graphics.FromImage(objLienzo);
            objFont = new System.Drawing.Font("MW6 PDF417R4", fontHeight);
            objCoordenadas = new Point(5, 5);
            objPincelFondo = new SolidBrush(System.Drawing.Color.Transparent);
            objPincelTexto = new SolidBrush(System.Drawing.Color.Black);

            objGraficar.FillRectangle(objPincelFondo, 0, 0, 800, 400);
            objGraficar.DrawString(strTexto, objFont, objPincelTexto, objCoordenadas);

            return objLienzo;
        }

        static void GenerarCodigo(ref string strMensaje, ModoProceso objModoProceso, ErrorCorreccion objErroCorreccion,
                              int intNumeroFilas, int intNumeroColumnas, bool TruncateSimbolo, bool HandlerTilder, int intTamañoFuente,
                              Fuente NumeroFuente)
        {
            MW6PDF417.Font PDF417FontObj = new MW6PDF417.Font();

            string Message = strMensaje;
            int Mode = (int)objModoProceso;
            int ECLevel = (int)objErroCorreccion;
            int Rows = intNumeroFilas;
            int Columns = intNumeroColumnas;

            bool TruncateSymbol = TruncateSimbolo;
            bool HandleTilde = HandlerTilder;

            // Encode data using PDF417
            PDF417FontObj.Encode(Message, Mode, ECLevel, Rows, Columns, TruncateSymbol, HandleTilde);

            // How many rows?
            int RowCount = PDF417FontObj.GetRows();

            // Produce string for PDF417 font
            string EncodedMsg = "" + System.Convert.ToChar(13) + System.Convert.ToChar(10);
            for (int i = 0; i < RowCount; i++)
            {
                EncodedMsg = EncodedMsg + PDF417FontObj.GetRowStringAt(i);
                EncodedMsg = EncodedMsg + System.Convert.ToChar(13) + System.Convert.ToChar(10);
            }

            strMensaje = EncodedMsg;
        }

        private void EscribirTexto(string vTexto, PdfContentByte cb, iTextSharp.text.Document document, BaseFont font, float fontSize, float posX, float posY)
        {

            cb.BeginText();
            cb.SetFontAndSize(font, fontSize);
            cb.SetTextMatrix(posX, posY);
            cb.ShowText(vTexto);

            cb.EndText();
        }

        public void CrearDocumentoPDF()
        {
            try
            {
                

                #region Inicializando Variables

                if (bEsEscrituraPublica || bEsParte)
                {
                    _fCuerpoFontSize = 10.5f;
                }
                if (bEsSolaUnaHoja)
                {
                    _iLineNumberGeneral = 50;
                }
                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();
                //---------------------------------------
                //Fecha:28/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Declarar font en negrita
                //---------------------------------------
                iTextSharp.text.Font _fontGeneralB = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);
                iTextSharp.text.pdf.BaseFont bfontCourier = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.COURIER_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                iTextSharp.text.Font _fontTituloB = new iTextSharp.text.Font(bfontCourier, 16);
                //---------------------------------------

                bool bAplicarImagen = false;

                float fImageMargin = 45f;// 110.9601f;
                float fMargenInferiorDoc = 100;
                float fMargenSuperiorDoc = 80;//(842 - (_fCuerpoFontSize * fTextLeading * _iLineNumber) - fMargenInferiorDoc);
                float fImageWidth = 57.77f;

                float fMargenIzquierdaDoc = 100;
                if (ConfigurationManager.AppSettings["FNMargenIzquierdo"] != null)
                {
                    fMargenIzquierdaDoc = float.Parse(ConfigurationManager.AppSettings["FNMargenIzquierdo"].ToString());
                    _fMarginLeft = float.Parse(ConfigurationManager.AppSettings["FNMargenIzquierdo"].ToString());
                }
                float fMargenDerechaDoc = 100;
                if (ConfigurationManager.AppSettings["FNMargenDerecho"] != null)
                {
                    fMargenDerechaDoc = float.Parse(ConfigurationManager.AppSettings["FNMargenDerecho"].ToString());
                    _fMarginRight = float.Parse(ConfigurationManager.AppSettings["FNMargenDerecho"].ToString());
                }



                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                #endregion


                using (var ms = new MemoryStream())
                {

                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + _sCuerpoHtml);
                    str.Flush();


                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);


                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);


                    if (bEsBorrador)
                    {
                        PdfWriterEvents writerEvent = new PdfWriterEvents("Hoja borrador. Sin valor legal");
                        writer.PageEvent = writerEvent;
                    }


                    document.Open();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    float fPosicion = fMargenSuperiorDoc;
                    float fTextSize = 0;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(SLogoRuta);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 120);
                    img.ScaleAbsoluteHeight(85f);
                    img.ScaleAbsoluteWidth(fImageWidth);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);


                    cb.BeginText();


                    cb.SetFontAndSize(bfTimes, 6);

                    string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;

                    if (NombreConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                    {
                        ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                        NombreConsulado = NombreConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                    }


                    int iPosicionComa = NombreConsulado.IndexOf(",");

                    if (iPosicionComa >= 0)
                        NombreConsulado = NombreConsulado.Substring(0, iPosicionComa);



                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in NombreConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {

                            cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f)-10, document.PageSize.Height - 130 + pos);
                            cb.ShowText(texto.Trim());
                            texto = string.Empty;

                            pos -= 10;
                            posxAcumulado = 0;
                        }

                        posxAcumulado += tamPalabra;
                        posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                        texto += " " + palabra;
                    }

                    if (texto.Trim() != string.Empty)
                    {
                        cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f)-10, document.PageSize.Height - 130 + pos);
                        cb.ShowText(texto.Trim());
                    }

                    cb.EndText();

                    #endregion

                    int lineaMultiplo = 1;
                    float fDocumentHeight = 842;
                    float fLineaHeight = 12 + 1.5f;

                    #region Si_Es_EscrituraPublica

                    if (bEsEscrituraPublica)
                    {

                        #region Cabecera

                        TextoPosicionadoITextSharp textoPosicionadoTitulo = new TextoPosicionadoITextSharp();
                        textoPosicionadoTitulo.FontSize = 16;
                        if (_tituloBaseFont == null)
                        {
                            if (sTitulo.Length > 0)
                            {
                                textoPosicionadoTitulo.BaseFont = bfontCourier;
                                textoPosicionadoTitulo.FontFamily = _fontTituloB;
                            }
                            else
                            {
                                textoPosicionadoTitulo.BaseFont = CuerpoBaseFont;
                                textoPosicionadoTitulo.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            }
                        }
                        else
                        {
                            textoPosicionadoTitulo.BaseFont = TituloBaseFont;
                            textoPosicionadoTitulo.FontFamily = new iTextSharp.text.Font(_tituloBaseFont, _fCuerpoFontSize);
                        }

                        //----------------------------------------
                        //Fecha:16/02/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Lineas de espaciado adicional
                        //          para el testimonio.
                        //----------------------------------------
                        if (bEsTestimonio)
                        {
                            lineaMultiplo++;
                            lineaMultiplo++;
                            lineaMultiplo++;
                            lineaMultiplo++;
                        }

                        textoPosicionadoTitulo.FXPosition = fMargenIzquierdaDoc;
                        textoPosicionadoTitulo.FYPosition = (fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1));
                        textoPosicionadoTitulo.TextAligment = HorizontalAlign.Center;
                        textoPosicionadoTitulo.Texto = sTitulo;

                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoTitulo);

                        #endregion
                        //----------------------------------------
                        //Fecha:16/02/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Lineas de espaciado adicional
                        //          para el testimonio.
                        //----------------------------------------
                        if (!bEsTestimonio)
                        {
                            lineaMultiplo = 3;
                        }

                        #region ValidacionTipoActoNotarial

                        if (bEsEscrituraPublica)
                        {
                            //-------------------------------------
                            //Fecha: 19/09/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Quitar el siguiente texto cuando el Nro.minuta es S/N.
                            //Nro.Requerimiento: N° 029-2017 de fecha: 18/09/2017.
                            //-------------------------------------

                            if (_sMinutaEP != "S/N")
                            {
                                TextoPosicionadoITextSharp textoPosicionadoNumero = new TextoPosicionadoITextSharp();
                                textoPosicionadoNumero.FontSize = _fCuerpoFontSize;
                                textoPosicionadoNumero.BaseFont = CuerpoBaseFont;
                                textoPosicionadoNumero.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);


                                //----------------------------------------------------
                                textoPosicionadoNumero.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                textoPosicionadoNumero.FXPosition = fMargenIzquierdaDoc;
                                textoPosicionadoNumero.TextAligment = HorizontalAlign.Left;
                                textoPosicionadoNumero.Texto = _NumeroEP != string.Empty ? "NÚMERO: " + _NumeroEP : string.Empty;
                                //----------------------------------------------------


                                //TextoPosicionadoITextSharp textoPosicionadoMinuta = new TextoPosicionadoITextSharp();
                                //textoPosicionadoMinuta.FontSize = _fCuerpoFontSize;
                                //textoPosicionadoMinuta.BaseFont = CuerpoBaseFont;
                                //textoPosicionadoMinuta.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                ////----------------------------------------------------
                                //textoPosicionadoMinuta.FXPosition = fMargenIzquierdaDoc;
                                //textoPosicionadoMinuta.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                //textoPosicionadoMinuta.TextAligment = HorizontalAlign.Right;
                                //textoPosicionadoMinuta.Texto = _sMinutaEP != string.Empty ? "MINUTA: " + _sMinutaEP : string.Empty;
                                //----------------------------------------------------

                                _listTextoPosicionadoiTextSharp.Add(textoPosicionadoNumero);

                                //_listTextoPosicionadoiTextSharp.Add(textoPosicionadoMinuta);
                            }

                        }
                        else
                        {

                            //----------------------------------------
                            //Fecha:16/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Lineas de espaciado adicional
                            //          para el testimonio.
                            //----------------------------------------
                            if (bEsTestimonio)
                            {
                                lineaMultiplo++;
                                lineaMultiplo++;
                                lineaMultiplo++;
                            }
                            //-----------------------------------------------------------
                            //Fecha:16/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Establecer la letra en negrita para testimonio
                            //-----------------------------------------------------------                           

                            TextoPosicionadoITextSharp textoPosicionTipoEscrituraPublica = new TextoPosicionadoITextSharp();
                            textoPosicionTipoEscrituraPublica.FontSize = _fCuerpoFontSize;
                            textoPosicionTipoEscrituraPublica.BaseFont = CuerpoBaseFont;
                            textoPosicionTipoEscrituraPublica.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionTipoEscrituraPublica.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionTipoEscrituraPublica.FXPosition = fMargenIzquierdaDoc;
                            textoPosicionTipoEscrituraPublica.TextAligment = HorizontalAlign.Left;
                            textoPosicionTipoEscrituraPublica.Texto = _TipoActoNotarial != string.Empty ? "ESCRITURA PÚBLICA DE:" : string.Empty;
                            _listTextoPosicionadoiTextSharp.Add(textoPosicionTipoEscrituraPublica);

                            #region Existe_TipoActoNotarial
                            if (_TipoActoNotarial.Length > 0)
                            {
                                textoPosicionTipoEscrituraPublica = new TextoPosicionadoITextSharp();
                                textoPosicionTipoEscrituraPublica.FontSize = _fCuerpoFontSize;
                                if (bEsTestimonio)
                                {
                                    textoPosicionTipoEscrituraPublica.BaseFont = TituloBaseFont;
                                }
                                else
                                {
                                    textoPosicionTipoEscrituraPublica.BaseFont = CuerpoBaseFont;
                                }
                                textoPosicionTipoEscrituraPublica.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                textoPosicionTipoEscrituraPublica.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                textoPosicionTipoEscrituraPublica.FXPosition = fMargenIzquierdaDoc + 170;
                                textoPosicionTipoEscrituraPublica.TextAligment = HorizontalAlign.NotSet;
                                textoPosicionTipoEscrituraPublica.Texto = _TipoActoNotarial;
                                _listTextoPosicionadoiTextSharp.Add(textoPosicionTipoEscrituraPublica);
                            }
                            #endregion

                            lineaMultiplo++;
                        }
                        #endregion

                        lineaMultiplo++;


                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        #region Lista_Otorgantes


                        if (_listOtorgantes.Count > 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                            textoPosicionado.FontSize = _fCuerpoFontSize;
                            textoPosicionado.BaseFont = CuerpoBaseFont;
                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                            if (TipoActoNotarial.Length == 0)
                            {
                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                if (_listOtorgantes.Count == 1)
                                    textoPosicionado.Texto = "QUE OTORGA:";
                                else
                                    textoPosicionado.Texto = "QUE OTORGAN:";

                            }
                            else
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Indicar el subtitulo "Otorgado por:".
                                //-------------------------------------------------------------

                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                textoPosicionado.Texto = "OTORGADO POR:";
                            }

                            if (TipoActoNotarial.Length == 0)
                            { lineaMultiplo++; }


                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                            #region Si_No_Existe_ActoNotarial
                            if (TipoActoNotarial.Length == 0)
                            {
                                for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantes.Count; i++)
                                {
                                    string otorgante = _listOtorgantes[i].Trim();
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;
                                    textoPosicionado.BaseFont = bfontCourier;
                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);

                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                    textoPosicionado.Texto = otorgante;

                                    lineaMultiplo++;
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                            }
                            #endregion

                            #region Si_Existe_ActoNotarial_Otorgantes

                            if (TipoActoNotarial.Length > 0)
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                //-------------------------------------------------------------      

                                string strcadOtorgantes = "";
                                List<string> strArrayOtorgantes = new List<string>();
                                int intPar = 0;
                                string strCadenaIzquierda = "";
                                int intAnchoCadenaNombres = 43;
                                int intUltimoEspacio = 0;

                                #region ForOtorgantes
                                for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantes.Count; i++)
                                {
                                    intPar++;
                                    string otorgante = _listOtorgantes[i].Trim();
                                    if (_listOtorgantes.Count > 2)
                                    {
                                        if (i + 2 < _listOtorgantes.Count)
                                        {
                                            strcadOtorgantes = strcadOtorgantes + otorgante + ", ";
                                        }
                                        else
                                        {
                                            strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                        }
                                    }
                                    else
                                    {
                                        strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                    }
                                    if (intPar % 2 == 0)
                                    {
                                        if (strcadOtorgantes.Length > intAnchoCadenaNombres)
                                        {
                                            intUltimoEspacio = strcadOtorgantes.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                            strCadenaIzquierda = strcadOtorgantes.Substring(0, intUltimoEspacio);
                                            strArrayOtorgantes.Add(strCadenaIzquierda);
                                            strcadOtorgantes = strcadOtorgantes.Substring(intUltimoEspacio + 1);
                                        }
                                        else
                                        {
                                            strArrayOtorgantes.Add(strcadOtorgantes);
                                            strcadOtorgantes = "";
                                        }
                                    }
                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                                #endregion

                                if (_listOtorgantes.Count > 2)
                                {
                                    strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 3);
                                }
                                else
                                {
                                    strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 2);
                                }
                                if (strcadOtorgantes.Length > intAnchoCadenaNombres)
                                {
                                    intUltimoEspacio = strcadOtorgantes.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                    strCadenaIzquierda = strcadOtorgantes.Substring(0, intUltimoEspacio);
                                    strArrayOtorgantes.Add(strCadenaIzquierda);
                                    strcadOtorgantes = strcadOtorgantes.Substring(intUltimoEspacio + 1);

                                    strArrayOtorgantes.Add(strcadOtorgantes);
                                }
                                else
                                {
                                    strArrayOtorgantes.Add(strcadOtorgantes);
                                }

                                strcadOtorgantes = "";

                                #region ForOtorgantes
                                for (int i = 0; i < strArrayOtorgantes.Count; i++)
                                {
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;

                                    //-----------------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Establecer la letra en negrita para testimonio
                                    //-----------------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        textoPosicionado.BaseFont = TituloBaseFont;
                                    }
                                    else
                                    {
                                        textoPosicionado.BaseFont = CuerpoBaseFont;
                                    }


                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                    textoPosicionado.TextAligment = HorizontalAlign.NotSet;
                                    textoPosicionado.Texto = strArrayOtorgantes[i].ToString();
                                    textoPosicionado.FXPosition = fMargenIzquierdaDoc + 100;
                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                    lineaMultiplo++;
                                }
                                #endregion

                                strArrayOtorgantes.Clear();
                            }
                            #endregion
                        }

                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        lineaMultiplo++;

                        #region Si_Existe_ListaApoderados

                        if (_listApoderados.Count > 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                            textoPosicionado.FontSize = _fCuerpoFontSize;
                            textoPosicionado.BaseFont = CuerpoBaseFont;
                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                            if (TipoActoNotarial.Length == 0)
                            {
                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                            }
                            else
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda el texto: "a favor de".
                                //-------------------------------------------------------------

                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                            }
                            textoPosicionado.Texto = "A FAVOR DE:";

                            //---------------------------------------
                            //Fecha: 27/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Solo si no es testimonio aumentar una linea
                            //      si no en la misma linea.
                            //---------------------------------------
                            if (!bEsTestimonio)
                            {
                                lineaMultiplo++;
                            }

                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                            #region Si_No_Existe_TipoActoNotarial
                            if (TipoActoNotarial.Length == 0)
                            {
                                for (int i = (_iPageNumber - 1) * 10; i < _listApoderados.Count; i++)
                                {
                                    string apoderado = _listApoderados[i].Trim();

                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;
                                    textoPosicionado.BaseFont = bfontCourier;
                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                    textoPosicionado.Texto = apoderado;

                                    lineaMultiplo++;
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);


                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                            }
                            #endregion

                            #region Si_Existe_TipoActoNotarial
                            if (TipoActoNotarial.Length > 0)
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda a los apoderados.
                                //-------------------------------------------------------------
                                string strcadApoderados = "";
                                List<string> strArrayApoderados = new List<string>();
                                int intPar = 0;
                                string strCadenaIzquierda = "";
                                int intAnchoCadenaNombres = 43;
                                int intUltimoEspacio = 0;

                                #region ForApoderados
                                for (int i = (_iPageNumber - 1) * 10; i < _listApoderados.Count; i++)
                                {
                                    intPar++;
                                    string apoderado = _listApoderados[i].Trim();
                                    if (_listApoderados.Count > 2)
                                    {
                                        if (i + 2 < _listApoderados.Count)
                                        {
                                            strcadApoderados = strcadApoderados + apoderado + ", ";
                                        }
                                        else
                                        {
                                            strcadApoderados = strcadApoderados + apoderado + " Y ";
                                        }
                                    }
                                    else
                                    {
                                        strcadApoderados = strcadApoderados + apoderado + " Y ";
                                    }
                                    if (intPar % 2 == 0)
                                    {
                                        if (strcadApoderados.Length > intAnchoCadenaNombres)
                                        {
                                            intUltimoEspacio = strcadApoderados.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                            strCadenaIzquierda = strcadApoderados.Substring(0, intUltimoEspacio);
                                            strArrayApoderados.Add(strCadenaIzquierda);
                                            strcadApoderados = strcadApoderados.Substring(intUltimoEspacio + 1);
                                        }
                                        else
                                        {
                                            strArrayApoderados.Add(strcadApoderados);
                                            strcadApoderados = "";
                                        }
                                    }
                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                                #endregion
                                if (_listApoderados.Count > 2)
                                {
                                    strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 3);
                                }
                                else
                                {
                                    strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 2);
                                }
                                if (strcadApoderados.Length > intAnchoCadenaNombres)
                                {
                                    intUltimoEspacio = strcadApoderados.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                    strCadenaIzquierda = strcadApoderados.Substring(0, intUltimoEspacio);
                                    strArrayApoderados.Add(strCadenaIzquierda);
                                    strcadApoderados = strcadApoderados.Substring(intUltimoEspacio + 1);

                                    strArrayApoderados.Add(strcadApoderados);
                                }
                                else
                                {
                                    strArrayApoderados.Add(strcadApoderados);
                                }

                                strcadApoderados = "";

                                #region Apoderados

                                for (int i = 0; i < strArrayApoderados.Count; i++)
                                {
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;
                                    //-----------------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Establecer la letra en negrita para testimonio
                                    //-----------------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        textoPosicionado.BaseFont = TituloBaseFont;
                                    }
                                    else
                                    {
                                        textoPosicionado.BaseFont = CuerpoBaseFont;
                                    }

                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                    textoPosicionado.TextAligment = HorizontalAlign.NotSet;
                                    textoPosicionado.Texto = strArrayApoderados[i].ToString();
                                    textoPosicionado.FXPosition = fMargenIzquierdaDoc + 100;
                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                    lineaMultiplo++;
                                }
                                #endregion
                                strArrayApoderados.Clear();
                            }
                            #endregion
                        }
                        #endregion


                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        if (TipoActoNotarial.Length == 0)
                        { lineaMultiplo++; }

                        #region LineaDivisora

                        TextoPosicionadoITextSharp textoPosicionadoITS = new TextoPosicionadoITextSharp();
                        textoPosicionadoITS.FontSize = _fCuerpoFontSize;
                        textoPosicionadoITS.BaseFont = CuerpoBaseFont;
                        textoPosicionadoITS.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                        textoPosicionadoITS.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                        textoPosicionadoITS.FXPosition = fMargenIzquierdaDoc;
                        //textoPosicionadoITS.FXPosition = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;
                        textoPosicionadoITS.TextAligment = HorizontalAlign.Center;

                        string strraya = new string('_', 67);
                        textoPosicionadoITS.Texto = strraya;
                        //textoPosicionadoITS.Texto = "___________________________________________________________________";                        

                        lineaMultiplo++;
                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoITS);

                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                    }

                    #endregion

                    int iContadorLineas = 0;

                    #region Cabecera Parte : Fecha
                    //if (bEsParte)
                    //{
                    //    iContadorLineas = 1;

                    //    TextoPosicionadoITextSharp textoPosicionadoFecha = new TextoPosicionadoITextSharp();
                    //    textoPosicionadoFecha.FontSize = _fCuerpoFontSize;
                    //    textoPosicionadoFecha.BaseFont = CuerpoBaseFont;
                    //    textoPosicionadoFecha.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                    //    textoPosicionadoFecha.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                    //    textoPosicionadoFecha.TextAligment = HorizontalAlign.Right;
                    //    textoPosicionadoFecha.Texto = sFecha;

                    //    _listTextoPosicionadoiTextSharp.Add(textoPosicionadoFecha);

                    //    PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                    //    _listTextoPosicionadoiTextSharp.Clear();
                    //}
                    #endregion

                    #region Cuerpo Documento

                   

                    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                    while ((int)fDocumentPosition + (_fCuerpoFontSize * FCuerpoLeading) < ((int)writer.GetVerticalPosition(false)))
                    {
                        oParagraph = new iTextSharp.text.Paragraph();
                        oParagraph.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                        oParagraph.SetLeading(0.0f, 1.42f);
                        document.Add(oParagraph);

                    }

                    _iPageNumber = 1;

                    

                    bool bExistImage = false;
                    bool bEsNombreOtorgante = false;
                   

                    for (int k = 0; k < objects.Count; k++)
                    {

                        oIElement = (iTextSharp.text.IElement)objects[k];

                        bool bBorrarParagraph = true;

                        if (((iTextSharp.text.IElement)objects[k]).Chunks.Count == 0)
                        {
                            oParagraph.Clear();
                            oParagraph.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                            bBorrarParagraph = false;
                        }

                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            if (bBorrarParagraph)
                            {
                                oParagraph = new iTextSharp.text.Paragraph();
                                oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;
                                oParagraph.Font = _fontGeneral;
                                //-------------------------------------------------------
                                //Fecha: 28/03/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Poner la primera linea del texto de 
                                //          Escritura Pública en negrita.
                                //-------------------------------------------------------
                                if (bEsEscrituraPublica)
                                {
                                    if (k == 0)
                                    { oParagraph.Font = _fontGeneralB; }
                                    
                                }
                                //-------------------------------------------------------
                            }
                            bBorrarParagraph = true;

                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            #region Chunks

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {

                                if (bEsParte)
                                    strContent = oIElement.Chunks[z].Content.ToString();
                                else
                                {
                                    //-----------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Si es testimonio no convertir a mayusculas.
                                    //-----------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        strContent = oIElement.Chunks[z].Content.ToString();
                                    }
                                    else
                                    {
                                        strContent = oIElement.Chunks[z].Content.ToString().ToUpper();
                                    }
                                }

                                if (strContent.Contains("#IMAGEN#"))
                                {
                                    strContent = strContent.Replace("#IMAGEN#", "");
                                    bExistImage = true;
                                }


                                while (strContent.Contains(" .") || strContent.Contains(" ,") ||
                                    strContent.Contains(" ;") || strContent.Contains(" :"))
                                {
                                    strContent = strContent.Replace(" .", ".").Replace(" ,", ",").Replace(" :", ":").Replace(" ;", ";");
                                }

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

                                    if (strContent != "\n")
                                    {
                                        strContent = strContent.Trim();
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        continue;
                                    }

                                    if (z == oIElement.Chunks.Count - 1)
                                    {
                                        List<string> listTextos = new List<string>();
                                        List<iTextSharp.text.Font> listFonts = new List<iTextSharp.text.Font>();
                                        string textoNotarialCierre = string.Empty;
                                        int intNumeroElementos = 0;
                                        string strContenido = string.Empty;

                                        foreach (iTextSharp.text.Chunk ch in oIElement.Chunks)
                                        {
                                            if (ch.Content.Contains("#IMAGEN#"))
                                            {
                                                strContenido = string.Empty;
                                            }
                                            strContenido = ch.Content.Replace("#IMAGEN#", "");


                                            if (strContenido == string.Empty)
                                                continue;

                                            if (strContenido != "\n")
                                                listTextos.Add(strContenido.Trim());
                                            else
                                                listTextos.Add("\n");


                                            ch.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);

                                            listFonts.Add(ch.Font);
                                            intNumeroElementos = intNumeroElementos + 1;
                                        }


                                        bool bEscribirTextoCierro = true;

                                        if (_listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in _listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim().Contains(palabra))
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }

                                        }

                                        if (bEscribirTextoCierro)
                                        {
                                            if (listTextos.Count > 0)
                                                textoNotarialCierre = ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                        }


                                        if (textoNotarialCierre != string.Empty)
                                        {
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(oIElement.Chunks[z].Font);
                                            font.SetStyle(0);
                                            oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                        }

                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(" ", oIElement.Chunks[z].Font));
                                    }

                                    #endregion
                                }

                            }


                            bool bBorrar = false;
                            for (int i = oParagraph.Chunks.Count - 1; i >= 0; i--)
                            {
                                if (oParagraph.Chunks[i].Content == string.Empty)
                                    continue;

                                if (!bBorrar)
                                {
                                    if (oParagraph.Chunks[i].Content[0] == '.' || oParagraph.Chunks[i].Content[0] == ',' ||
                                        oParagraph.Chunks[i].Content[0] == ':' || oParagraph.Chunks[i].Content[0] == ';')
                                    {
                                        bBorrar = true;
                                    }
                                }
                                else
                                {
                                    oParagraph.RemoveAt(i);
                                    bBorrar = false;
                                }

                            }

                            #endregion

                            #region LineCheck
                        
                        LineCheck:

                            float fLineaWidth = 0;
                            float fPalabraWidth = 0;
                            string sLineaAcumulada = string.Empty;
                            List<string> listLineas = new List<string>();

                            #region Parrafo

                            foreach (iTextSharp.text.Chunk par in oParagraph.Chunks)
                            {
                                if (par.Content == "\n")
                                {
                                    listLineas.Add("\n");
                                    continue;
                                }

                                if (!bEsParte)
                                {
                                    //-----------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Si es testimonio no convertir a mayusculas.
                                    //-----------------------------------------------------                                    
                                    if (bEsTestimonio)
                                    {
                                        #region esTestimonio
                                        foreach (string palabra in par.Content.Trim().Split(' '))
                                        {
                                            fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                            if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                            {
                                                if (sLineaAcumulada.Trim() != string.Empty)
                                                {
                                                    listLineas.Add(sLineaAcumulada.Trim());
                                                }

                                                else
                                                    listLineas.Add("\n");
                                                fLineaWidth = 0;
                                                sLineaAcumulada = string.Empty;
                                            }

                                            if (palabra.Trim() != string.Empty)
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                                fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //-----------------------------------------------------
                                        // Fecha: 18/04/2017
                                        // Autor: Miguel Márquez Beltrán
                                        // Objetivo: Poner en negrita el texto central.
                                        //-----------------------------------------------------                                                                                
                                        bool bEstablecerNegritaOtorgante = false;

                                        if (bEsNombreOtorgante)
                                        { bEstablecerNegritaOtorgante = true; }
                                        
                                        //=================================================================
                                        foreach (string palabra in par.Content.ToUpper().Trim().Split(' '))
                                        {
                                            //------------------------------------------------------------------
                                            //Fecha: 29/03/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Poner en negrita la palabra: CONCLUSIÓN:
                                            //------------------------------------------------------------------

                                            if (bEsEscrituraPublica && palabra.Equals("CONCLUSIÓN:"))
                                            { fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneralB).GetWidthPoint(); }
                                            else
                                            {                                              
                                                if (bEsEscrituraPublica && palabra.IndexOf("COMPARECE:") > -1)
                                                { bEsNombreOtorgante = true; }

                                                if (bEstablecerNegritaOtorgante)
                                                { fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneralB).GetWidthPoint(); }
                                                else
                                                {
                                                   fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();
                                                }
                                               
                                            }

                                            #region Linea Acumulada
                                            //=======================================                                                                                                                                   
                                            if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                            {
                                                if (sLineaAcumulada.Trim() != string.Empty)
                                                { listLineas.Add(sLineaAcumulada.Trim()); }

                                                else
                                                    listLineas.Add("\n");
                                                fLineaWidth = 0;
                                                sLineaAcumulada = string.Empty;
                                            }
                                            #endregion
                                            //=======================================

                                            if (palabra.Trim() != string.Empty)
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                                //=======================================
                                                if (bEstablecerNegritaOtorgante)
                                                {
                                                    fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneralB).GetWidthPoint();
                                                    if (palabra.IndexOf("NACIONALIDAD") > -1)
                                                    {
                                                        bEsNombreOtorgante = false;
                                                        bEstablecerNegritaOtorgante = false;
                                                    }
                                                }
                                                else
                                                {                                                   
                                                    fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();                                                   
                                                }
                                                //================================
                                            }
                                            //=======================================
                                        }
                                        //=================================================================Fin foreach

                                    }
                                }
                                else
                                {
                                    #region Parte

                                    foreach (string palabra in par.Content.Trim().Split(' '))
                                    {
                                        fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                        if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                        {
                                            if (sLineaAcumulada.Trim() != string.Empty)
                                            {
                                                listLineas.Add(sLineaAcumulada.Trim());
                                            }

                                            else
                                                listLineas.Add("\n");
                                            fLineaWidth = 0;
                                            sLineaAcumulada = string.Empty;
                                        }

                                        if (palabra.Trim() != string.Empty)
                                        {
                                            if (palabra.IndexOf("=") > -1)
                                            {
                                                sLineaAcumulada += palabra.Trim();
                                            }
                                            else
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                            }
                                            
                                            fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                        }
                                    }
                                    #endregion
                                }
                            }
                            //======================================================Fin foreach
                            #endregion

                            if (sLineaAcumulada.Trim() != string.Empty)
                            {
                                listLineas.Add(sLineaAcumulada.Trim());
                            }
                            else
                                listLineas.Add("\n");


                            #endregion

                            oParagraph.SetLeading(0.0f, FCuerpoLeading);

                            iContadorLineas += listLineas.Count;

                            if (iContadorLineas >= _iLineNumberGeneral)
                            {
                                #region ContadorLineas

                                int iLineasSobrantes = iContadorLineas - _iLineNumberGeneral;
                                int iUltimaLineaPrimerTrozo = listLineas.Count - iLineasSobrantes;
                                string lineasPrimeraTrozo = string.Empty;
                                string lineasSegundaTrozo = string.Empty;

                                
                                listLineas[iUltimaLineaPrimerTrozo - 1] = ObtenerLineaJustificada(
                                    new iTextSharp.text.Chunk(listLineas[iUltimaLineaPrimerTrozo - 1],
                                        _fontGeneral),
                                        document.PageSize.Width - (_fMarginLeft + _fMarginRight));
                                

                                for (int i = 0; i < listLineas.Count; i++)
                                {
                                    if (i < iUltimaLineaPrimerTrozo)
                                    {
                                        lineasPrimeraTrozo += listLineas[i] + " ";
                                    }
                                    else
                                    {
                                        lineasSegundaTrozo += listLineas[i] + " ";
                                    }
                                }


                                bool bNuevaPagina = true;
                                if (writer.GetVerticalPosition(false) != 742)
                                    bNuevaPagina = false;

                                oParagraph.Clear();

                                if (bEsParte)
                                {
                                    if (lineasPrimeraTrozo.IndexOf("CONCLUSIÓN:") > -1)
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk("CONCLUSIÓN:", _fontGeneralB));
                                        lineasPrimeraTrozo = lineasPrimeraTrozo.Replace("CONCLUSIÓN:", "");

                                        //if (lineasPrimeraTrozo.IndexOf(NombreFuncionario) > -1)
                                        //{
                                        //    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                        //}
                                    }                                    
                                }

                                //--------------------------------------------------
                                //Fecha: 29/03/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Poner en negrita la palabra: CONCLUSIÓN
                                //--------------------------------------------------
                                if (bEsEscrituraPublica && lineasPrimeraTrozo.IndexOf("CONCLUSIÓN:") > -1)
                                {
                                    oParagraph.Add(new iTextSharp.text.Chunk("CONCLUSIÓN:", _fontGeneralB));
                                    lineasPrimeraTrozo = lineasPrimeraTrozo.Replace("CONCLUSIÓN:", "");                                    

                                    //if (lineasPrimeraTrozo.IndexOf(NombreFuncionario) > -1)
                                    //{
                                    //    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);                                        
                                    //}                                    
                                                                                                        
                                    //------------------------------------------------
                                }

                              
                                string strQueConsteEl = "QUE CONSTE";

                                if (bEsEscrituraPublica && lineasPrimeraTrozo.IndexOf(strQueConsteEl) > -1)
                                {
                                    int intIndiceInicialQueConsteEl = lineasPrimeraTrozo.IndexOf(strQueConsteEl);
                                    int intIndiceFinalQueConsteEl = intIndiceInicialQueConsteEl + strQueConsteEl.Length;
                                    string strQueOtorga = "";

                                    strQueOtorga = "QUE";
                                    

                                    string strCadena = lineasPrimeraTrozo.Substring(intIndiceFinalQueConsteEl);
                                    int intIndiceInicialQueOtorga = strCadena.IndexOf(strQueOtorga);

                                    string strTipoActoNotarial = strCadena.Substring(0, intIndiceInicialQueOtorga);

                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strTipoActoNotarial, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);

                                    //string strOtorgante = "";

                                    //for (int i = 0; i < _listOtorgantes.Count; i++)
                                    //{
                                    //    strOtorgante = _listOtorgantes[i].Trim() + ",";
                                    //    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                    //}
                                    
                                }

                                int intInicioArticulo = lineasPrimeraTrozo.IndexOf("INSERTO:");
                                int intFinArticulo = lineasPrimeraTrozo.IndexOf(".-");

                                if (intInicioArticulo > -1 && intFinArticulo > -1)
                                {
                                    string strNombreArticulo = lineasPrimeraTrozo.Substring(intInicioArticulo, intFinArticulo);
                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombreArticulo, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                }
                                //--------------------------------------------------
                                //Poner negrita al nombre del otorgante
                                //--------------------------------------------------

                                if (lineasPrimeraTrozo.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") == -1 && lineasPrimeraTrozo.IndexOf("IMPRESIÓN DACTILAR:") == -1)
                                {
                                    //---------------------------------------------------------------
                                    string strQueConste = "QUE CONSTE";
                                    int intIndiceInicialQueConsteEl = lineasPrimeraTrozo.IndexOf(strQueConste);
                                    int intIndiceFinalQueConsteEl = intIndiceInicialQueConsteEl + strQueConste.Length;
                                    string strQueOtorga = "QUE";
                                    if (lineasPrimeraTrozo.IndexOf(strQueConste) > -1)
                                    {
                                        string strCadena = lineasPrimeraTrozo.Substring(intIndiceFinalQueConsteEl);
                                        int intIndiceInicialQueOtorga = strCadena.IndexOf(strQueOtorga);

                                        string strTipoActoNotarial = strCadena.Substring(0, intIndiceInicialQueOtorga);

                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strTipoActoNotarial, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);

                                    }
                                }
                                 if (bEsEscrituraPublica && (lineasPrimeraTrozo.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1 || lineasPrimeraTrozo.IndexOf("IMPRESIÓN DACTILAR:") > -1))
                                {
                                    if (lineasPrimeraTrozo.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1)
                                    {
                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita("FIRMA E IMPRESIÓN DACTILAR:", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                    }
                                    if (lineasPrimeraTrozo.IndexOf("IMPRESIÓN DACTILAR:") > -1)
                                    {
                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita("IMPRESIÓN DACTILAR:", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                    }

                                    
                                    string strOtorgante = "";
                                    //---------------------------------------------
                                    for (int i = 0; i < _listOtorgantes.Count; i++)
                                    {                                      
                                        strOtorgante = _listOtorgantes[i].Trim();

                                        if (lineasPrimeraTrozo.IndexOf(strOtorgante) > -1)
                                        {
                                            lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                        }                                                                                                                       
                                    }
                                    string[] strNombres = null;
                                    if (lineasPrimeraTrozo.Length > 0)
                                    {
                                        //---------------------------------------------
                                        for (int i = 0; i < _listOtorgantes.Count; i++)
                                        {
                                            strOtorgante = _listOtorgantes[i].Trim();
                                            strNombres = strOtorgante.Split(' ');

                                            for (int x = 0; x < strNombres.Length; x++)
                                            {
                                                if (lineasPrimeraTrozo.Contains(strNombres[x] + "  "))
                                                {
                                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + "  ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                }
                                                else
                                                {
                                                    if (lineasPrimeraTrozo.Contains(strNombres[x] + " "))
                                                    {
                                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + " ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //---------------------------------------------------
                                    if (iLineasSobrantes == 0)
                                    {
                                        if (lineasPrimeraTrozo.LastIndexOf("==== ") > -1)
                                        {
                                            lineasPrimeraTrozo = lineasPrimeraTrozo.Substring(0, lineasPrimeraTrozo.Length - 5);
                                            oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                        }
                                        else
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                        }
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                    }
                                }
                                else
                                {
                                    string strLibro = "LIBRO";
                                    int intIndiceLibro = lineasPrimeraTrozo.IndexOf(strLibro);

                                    if (intIndiceLibro > -1)
                                    {
                                        //string strParte1 = lineasPrimeraTrozo.Substring(0, intIndiceLibro + strLibro.Length);
                                        //string strDelRegistro = "DEL REGISTRO";                                        

                                        //oParagraph.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));
                                        //lineasPrimeraTrozo = lineasPrimeraTrozo.Replace(strParte1, "");

                                        //int intDelRegistro = lineasPrimeraTrozo.IndexOf(strDelRegistro);

                                        //if (intDelRegistro > -1)
                                        //{
                                        //    string strNroLibro = lineasPrimeraTrozo.Substring(0, intDelRegistro);

                                        //    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNroLibro, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                        //}

                                        oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                    }
                                    else
                                    {
                                        if (bEsParte)
                                        {
                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                        }
                                        else
                                        {                                                
                                            if (lineasPrimeraTrozo.IndexOf("FIRMA:") > -1 && lineasPrimeraTrozo.IndexOf(NombreFuncionario) > -1)
                                            {
                                                lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                            }
                                            if (lineasPrimeraTrozo.Length > 0)
                                            {
                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                            }
                                        }
                                    }
                                }                                


                                document.Add(oParagraph);


                                if (bNuevaPagina)
                                { document.NewPage(); }


                                _iLineNumber = 25.7f;
                                _iPageNumber++;

                                lineaMultiplo = 0;

                                if (bEsEscrituraPublica)
                                {
                                    #region Otorgantes y Apoderados

                                    if ((_listOtorgantes.Count > (_iPageNumber - 1) * 10) || (_listApoderados.Count > (_iPageNumber - 1) * 10))
                                    {
                                        fDocumentHeight = 842;
                                        fLineaHeight = 12 + 1.5f;


                                        #region Otorgantes


                                        if (_listOtorgantes.Count > (_iPageNumber - 1) * 10)
                                        {
                                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                                            textoPosicionado.FontSize = _fCuerpoFontSize;
                                            textoPosicionado.BaseFont = CuerpoBaseFont;
                                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;

                                            if (TipoActoNotarial.Length == 0 &&  bEsEscrituraPublica)
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                textoPosicionado.Texto = "QUE OTORGA:";
                                            }
                                            else
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Indicar el texto: "Otorgado por".
                                                //-------------------------------------------------------------

                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = "OTORGADO POR:";
                                            }
                                            lineaMultiplo++;

                                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                            string strcadOtorgantes = "";

                                            for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantes.Count; i++)
                                            {
                                                string otorgante = _listOtorgantes[i].Trim();
                                                textoPosicionado = new TextoPosicionadoITextSharp();
                                                textoPosicionado.FontSize = _fCuerpoFontSize;
                                                textoPosicionado.BaseFont = CuerpoBaseFont;
                                                textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                                //-------------------------------------------------------------

                                                if (bEsTestimonio)
                                                {
                                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                    textoPosicionado.Texto = otorgante;

                                                    lineaMultiplo++;
                                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                                }
                                                else
                                                {
                                                    strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                                }


                                                if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                                    break;
                                            }

                                            if (TipoActoNotarial.Length > 0 && bEsTestimonio)
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                                //-------------------------------------------------------------
                                                strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 3);
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = strcadOtorgantes;
                                                textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                                _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                            }
                                        }


                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                        #region Apoderados

                                        if (_listApoderados.Count > (_iPageNumber - 1) * 10)
                                        {
                                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                                            textoPosicionado.FontSize = _fCuerpoFontSize;
                                            textoPosicionado.BaseFont = CuerpoBaseFont;
                                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                            //-------------------------------------------------------------
                                            //Fecha: 10/02/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Alinear a la izquierda a los apoderados.
                                            //-------------------------------------------------------------

                                            if (bEsEscrituraPublica)
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                            }
                                            textoPosicionado.Texto = "A FAVOR DE:";

                                            lineaMultiplo++;

                                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                            string strcadApoderados = "";

                                            for (int i = (_iPageNumber - 1) * 10; i < _listApoderados.Count; i++)
                                            {
                                                string apoderado = _listApoderados[i].Trim();
                                                textoPosicionado = new TextoPosicionadoITextSharp();
                                                textoPosicionado.FontSize = _fCuerpoFontSize;
                                                textoPosicionado.BaseFont = CuerpoBaseFont;
                                                textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                                if (bEsTestimonio)
                                                {
                                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                    textoPosicionado.Texto = apoderado;

                                                    lineaMultiplo++;
                                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                                }
                                                else
                                                {
                                                    strcadApoderados = strcadApoderados + apoderado + " Y ";
                                                }


                                                if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                                    break;
                                            }
                                            if (bEsTestimonio)
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los apoderados.
                                                //-------------------------------------------------------------
                                                strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 3);
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = strcadApoderados;
                                                textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                                _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                            }

                                        }


                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                        #region LineaDivisora

                                        TextoPosicionadoITextSharp textoPosicionadoITS = new TextoPosicionadoITextSharp();
                                        textoPosicionadoITS.FontSize = _fCuerpoFontSize;
                                        textoPosicionadoITS.BaseFont = CuerpoBaseFont;
                                        textoPosicionadoITS.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                        textoPosicionadoITS.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                        textoPosicionadoITS.TextAligment = HorizontalAlign.Center;
                                        textoPosicionadoITS.Texto = "________________________________________________________";

                                        lineaMultiplo++;
                                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoITS);

                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                    }

                                    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);


                                    while ((int)fDocumentPosition < ((int)writer.GetVerticalPosition(false)))
                                    {
                                        oParagraph.Clear();
                                        oParagraph.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                                        oParagraph.SetLeading(0.0f, 1.42f);
                                        document.Add(oParagraph);
                                    }

                                    #endregion
                                }

                                if (lineasSegundaTrozo.Trim() != string.Empty)
                                {
                                    oParagraph.Clear();
                                    oParagraph.SetLeading(0.0f, FCuerpoLeading);


                                    string strLosPoderdantes = "LOS PODERDANTES, ";
                                    string strAquienEnAdelanteSeLeDenominara = "A QUIEN EN ADELANTE SE LE DENOMINARÁ";
                                    string strElApoderado = "EL APODERADO.";

                                    int intIndiceLosPoderdantes = lineasSegundaTrozo.IndexOf(strLosPoderdantes);
                                    int intIndiceAquienEnAdelanteSeLeDenominara = lineasSegundaTrozo.IndexOf(strAquienEnAdelanteSeLeDenominara);
                                    int intIndiceElApoderado = lineasSegundaTrozo.IndexOf(strElApoderado);                                    

                                    if (intIndiceLosPoderdantes > -1 && intIndiceAquienEnAdelanteSeLeDenominara > -1 && intIndiceElApoderado > -1)
                                    {
                                        //--------------------------------------
                                        //Poner en negrita a los otorgantes
                                        //--------------------------------------
                                        //string strOtorgante = "";

                                        //for (int i = 0; i < _listOtorgantes.Count; i++)
                                        //{
                                        //    strOtorgante = _listOtorgantes[i].Trim() + ", ";
                                        //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                        //}

                                        oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                    }
                                    else
                                    {

                                        string strFueElDescrito_ = "FUE EL DESCRITO";
                                        int intIndiceInicialFueElDescrito_ = lineasSegundaTrozo.IndexOf(strFueElDescrito_);

                                        string strNombres = "NOMBRE ";
                                        int intIndiceInicialNombre = lineasSegundaTrozo.IndexOf(strNombres);

                                        string strPoderdantesDeNombres = "PODERDANTES DE NOMBRES";
                                        int intIndicePoderdantesDeNombres = lineasSegundaTrozo.IndexOf(strPoderdantesDeNombres);

                                        if (intIndiceInicialNombre == -1 && intIndiceInicialFueElDescrito_ > -1 && intIndicePoderdantesDeNombres == -1)
                                        {
                                            string strParte1 = lineasSegundaTrozo.Substring(0, intIndiceInicialFueElDescrito_);
                                            oParagraph.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));

                                            string strParte2 = lineasSegundaTrozo.Substring(intIndiceInicialFueElDescrito_);
                                            oParagraph.Add(new iTextSharp.text.Chunk(strParte2.Trim(), _fontGeneral));
                                            lineasSegundaTrozo = "";
                                        }
                                        else
                                        {
                                            if (intIndiceInicialNombre > -1 && intIndiceInicialFueElDescrito_ > -1)
                                            {
                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                            }
                                            else
                                            {
                                                string strAQuienEnAdelanteSeLeDenominaran = "A QUIEN EN ADELANTE SE LES DENOMINARÁN";
                                                int intIndiceInicialAQuienEnAdelanteSeLeDenominaran = lineasSegundaTrozo.IndexOf(strAQuienEnAdelanteSeLeDenominaran);
                                                string strQueOtorga = "QUE OTORGA";
                                                int intQueOtorga = lineasSegundaTrozo.IndexOf(strQueOtorga);

                                                if (intIndiceInicialAQuienEnAdelanteSeLeDenominaran > -1 && intQueOtorga > -1)
                                                {
                                                    string strQueConsteEl_Tipo = "QUE CONSTE";
                                                                                                    

                                                    string strParte0 = lineasSegundaTrozo.Substring(0, intQueOtorga);

                                                    string strTipoEscritura = strParte0.Substring(strQueConsteEl_Tipo.Length);

                                                    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strTipoEscritura, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                    
                                                    
                                                    intIndiceInicialAQuienEnAdelanteSeLeDenominaran = lineasSegundaTrozo.IndexOf(strAQuienEnAdelanteSeLeDenominaran);

                                                    string strParte1 = lineasSegundaTrozo.Substring(0, intIndiceInicialAQuienEnAdelanteSeLeDenominaran + strAQuienEnAdelanteSeLeDenominaran.Length);

                                                    //--------------------------------------
                                                    //Poner en negrita a los otorgantes
                                                    //--------------------------------------
                                                    //string strOtorgante = "";

                                                    //for (int i = 0; i < _listOtorgantes.Count; i++)
                                                    //{
                                                    //    strOtorgante = _listOtorgantes[i].Trim() + ", ";
                                                    //    strParte1 = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, strParte1, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                    //}

                                                    


                                                    //--------------------------------------
                                                    oParagraph.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));

                                                    string strParte2 = lineasSegundaTrozo.Substring(intIndiceInicialAQuienEnAdelanteSeLeDenominaran + strAQuienEnAdelanteSeLeDenominaran.Length);
                                                    int intIndicePunto = strParte2.IndexOf(".");
                                                    string strApoderados = strParte2.Substring(0, intIndicePunto);
                                                    string strParte3 = strParte2.Substring(intIndicePunto);

                                                    oParagraph.Add(new iTextSharp.text.Chunk(strApoderados, _fontGeneralB));
                                                    oParagraph.Add(new iTextSharp.text.Chunk(strParte3, _fontGeneral));
                                                }
                                                else
                                                {
                                                    string strDelLibro = "DEL LIBRO";
                                                    int intDelLibro = lineasSegundaTrozo.IndexOf(strDelLibro);

                                                    string strDelRegistro = "DEL REGISTRO";
                                                    int intDelRegistro = lineasSegundaTrozo.IndexOf(strDelRegistro);

                                                    if (lineasSegundaTrozo.IndexOf("INSTRUMENTOS") > -1 && intDelLibro > -1 && intDelRegistro > -1)
                                                    {

                                                        string strParte1 = lineasSegundaTrozo.Substring(0, intDelLibro + strDelLibro.Length);
                                                        oParagraph.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));
                                                        lineasSegundaTrozo = lineasSegundaTrozo.Replace(strParte1, "");

                                                        intDelRegistro = lineasSegundaTrozo.IndexOf(strDelRegistro);
                                                        string strNroLibro = lineasSegundaTrozo.Substring(0, intDelRegistro);
                                                        string strParte2 = lineasSegundaTrozo.Substring(intDelRegistro);

                                                        oParagraph.Add(new iTextSharp.text.Chunk(strNroLibro, _fontGeneralB));
                                                        oParagraph.Add(new iTextSharp.text.Chunk(strParte2, _fontGeneral));
                                                    }
                                                    else
                                                    {
                                                        string strDeLoQueDoyFe = "DE LO QUE DOY FE.";
                                                        int intIndiceInicialDeLoQueDoyFe = lineasSegundaTrozo.IndexOf(strDeLoQueDoyFe);

                                                        string strElDia = "EL DÍA";
                                                        int intIndiceInicialElDia = lineasSegundaTrozo.IndexOf(strElDia);

                                                        if (intIndiceInicialDeLoQueDoyFe > -1 && intIndiceInicialElDia > -1)
                                                        {
                                                            intIndiceInicialElDia = intIndiceInicialElDia + strElDia.Length;

                                                            string strDiaMesAnio = lineasSegundaTrozo.Substring(intIndiceInicialElDia, intIndiceInicialDeLoQueDoyFe - intIndiceInicialElDia);
                                                            lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strDiaMesAnio, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                            oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                                        }
                                                        else
                                                        {

                                                            string strAQuienEnAdelanteSeLe = "A QUIEN EN ADELANTE SE";
                                                            string strElApoderadoy = "EL APODERADO.";

                                                            int intIndiceAQuienEnAdelanteSeLe = lineasSegundaTrozo.IndexOf(strAQuienEnAdelanteSeLe);
                                                            int intIndiceElApoderadoy = lineasSegundaTrozo.IndexOf(strElApoderadoy);

                                                            if (intIndiceAQuienEnAdelanteSeLe > -1 && intIndiceElApoderadoy > -1)
                                                            {

                                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                            }
                                                            else
                                                            {                                                                                                                                
                                                                string strEnAdelanteSeDenominaran = "EN ADELANTE SE LES DENOMINARÁN ";
                                                                string strLosPoderdantesx = "LOS PODERDANTES, ";
                                                                string strAFavorDe = "A FAVOR DE";
                                                                string strAQuienAdelanteSelesDenominaran = "A QUIENES EN ADELANTE SE LES DENOMINARÁN";
                                                                string strLosApoderados = "LOS APODERADOS.";

                                                                int intIndiceEnAdelanteSeDenominaran = lineasSegundaTrozo.IndexOf(strEnAdelanteSeDenominaran);
                                                                int intIndiceLosPoderdantesx = lineasSegundaTrozo.IndexOf(strLosPoderdantesx);
                                                                int intIndiceAFavorDe = lineasSegundaTrozo.IndexOf(strAFavorDe);
                                                                int intIndiceAQuienAdelanteSelesDenominaran = lineasSegundaTrozo.IndexOf(strAQuienAdelanteSelesDenominaran);
                                                                int intIndiceLosApoderados = lineasSegundaTrozo.IndexOf(strLosApoderados);

                                                                if (intIndiceEnAdelanteSeDenominaran > -1 && intIndiceLosPoderdantesx > -1 &&
                                                                    intIndiceAFavorDe > -1 && intIndiceAQuienAdelanteSelesDenominaran > -1 && intIndiceLosApoderados > -1)
                                                                {
                                                                    oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                }
                                                                else
                                                                {                                                                    

                                                                    string strSeLeDenominaraLaApoderada = "SE LE DENOMINARÁ LA APODERADA. ";
                                                                    string strSeLeDenominaraLaApoderado = "SE LE SDENOMINARÁ EL APODERADO. ";

                                                                    int intIndiceSeLeDenominaraLaApoderada = lineasSegundaTrozo.IndexOf(strSeLeDenominaraLaApoderada);
                                                                    int intIndiceSeLeDenominaraLaApoderado = lineasSegundaTrozo.IndexOf(strSeLeDenominaraLaApoderado);

                                                                    if (intIndiceSeLeDenominaraLaApoderada > -1 || intIndiceSeLeDenominaraLaApoderado > -1)
                                                                    {
                                                                       
                                                                        oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                    }
                                                                    else
                                                                    {
                                                                        //-----------------------------
                                                                        // PODERDANTES DE NOMBRES
                                                                        //-----------------------------
                                                                       

                                                                        if (lineasSegundaTrozo.IndexOf("PODERDANTE") > -1 || lineasSegundaTrozo.IndexOf("PODERDANTES") > -1)
                                                                        {
                                                                            //---------------------------------------------------------------------------------
                                                                            //Fecha: 25-03-2020
                                                                            //Autor: Miguel Márquez Beltrán
                                                                            //Objetivo: Poner en negrita el nombre del intérprete.
                                                                            //---------------------------------------------------------------------------------
                                                                            if (lineasSegundaTrozo.IndexOf("TRADUCIDA SIMULTÁNEAMENTE") > -1)
                                                                            {
                                                                                //string strinterprete = "";
                                                                                //for (int i = 0; i < _listInterpretes.Count; i++)
                                                                                //{
                                                                                //    strinterprete = _listInterpretes[i].Trim() + ", ";
                                                                                //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strinterprete, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                                //}
                                                                            }
                                                                            else
                                                                            { 
                                                                            

                                                                                //--------------------------------------
                                                                                //Poner en negrita a los otorgantes
                                                                                //--------------------------------------
                                                                                //string strOtorgante = "";

                                                                                //for (int i = 0; i < _listOtorgantes.Count; i++)
                                                                                //{
                                                                                //    strOtorgante = _listOtorgantes[i].Trim() + ", ";
                                                                                //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                                //}
                                                                            }
                                                                            if (lineasSegundaTrozo.IndexOf("SU FIRMA Y SU HUELLA DACTILAR. =======================================") > -1)
                                                                            {
                                                                                int intLast = lineasSegundaTrozo.Trim().Length - 11;
                                                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim().Substring(0,intLast), _fontGeneral));
                                                                            }
                                                                            else
                                                                            {
                                                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                            }
                                                                        }
                                                                        else
                                                                        {

                                                                            string strAQuienesEnAdelanteSeLesDenominaranLosApoderados = "A QUIENES EN ADELANTE SE LES DENOMINARÁN LOS APODERADOS.";
                                                                            int intAQuienesEnAdelanteSeLesDenominaranLosApoderados = lineasSegundaTrozo.IndexOf(strAQuienesEnAdelanteSeLesDenominaranLosApoderados);

                                                                            string strAQuienesEnAdelanteSeLesDenominaranLasApoderadas = "A QUIENES EN ADELANTE SE LES DENOMINARÁN LAS APODERADAS.";
                                                                            int intAQuienesEnAdelanteSeLesDenominaranLasApoderadas = lineasSegundaTrozo.IndexOf(strAQuienesEnAdelanteSeLesDenominaranLasApoderadas);

                                                                            if (intAQuienesEnAdelanteSeLesDenominaranLosApoderados > -1 || intAQuienesEnAdelanteSeLesDenominaranLasApoderadas > -1)
                                                                            {
                                                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lineasSegundaTrozo.IndexOf("CON FECHA") > -1 && lineasSegundaTrozo.IndexOf(".") > -1)
                                                                                {
                                                                                    string[] strNombreInterprete = null;
                                                                                    string strInterprete = "";
                                                                                    for (int i = 0; i < _listInterpretes.Count ; i++)
                                                                                    {
                                                                                        strInterprete = _listInterpretes[i].Trim();
                                                                                        strNombreInterprete = strInterprete.Split(' ');
                                                                                        for (int x = 0; x < strNombreInterprete.Length; x++)
                                                                                        {
                                                                                            if (lineasSegundaTrozo.Contains(strNombreInterprete[x] + " "))
                                                                                            {
                                                                                                lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombreInterprete[x] + " ", lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }

                                                                                oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }                                                    
                                                }
                                            }
                                        }
                                    }
                                    
                                    document.Add(oParagraph);
                                }

                                iContadorLineas = iLineasSobrantes;

                                while (iContadorLineas >= _iLineNumberGeneral)
                                {
                                    iContadorLineas -= _iLineNumberGeneral;
                                }

                                if (iContadorLineas == 0)
                                {
                                    document.NewPage();
                                }

                                #endregion
                            }
                            else
                            {
                                string textoHoja = string.Empty;

                                for (int i = 0; i < listLineas.Count; i++)
                                { textoHoja += listLineas[i] + " "; }

                                oParagraph.Clear();

                                //-----------------------------------------------------
                                // Fecha: 18/04/2017
                                // Autor: Miguel Márquez Beltrán
                                // Objetivo: Poner en negrita el texto central.
                                //-----------------------------------------------------               
                                //====================================================================
                                
                                if (bEsEscrituraPublica || bEsParte)
                                {
                                    if (bEsParte)
                                    {
                                        if (textoHoja.IndexOf("ESCRITURA PÚBLICA") == 0 && textoHoja.IndexOf("LIBRO") > -1 && textoHoja.IndexOf("AÑO") > -1)
                                        {                                           
                                            textoHoja = RetornaTextoPosteriorAsignacionNegrita(textoHoja, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                        }
                                        if (textoHoja == "ATENTAMENTE, ")
                                        {
                                            oParagraph.Add(new iTextSharp.text.Chunk("\n\n"));
                                        }
                                    }
                                    
                                    #region EscrituraPublica

                                    if (textoHoja.IndexOf("CONCLUSIÓN:") > -1)
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk("CONCLUSIÓN:", _fontGeneralB));

                                        textoHoja = textoHoja.Replace("CONCLUSIÓN:", "");

                                        //textoHoja = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario + ", ", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);


                                        oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                    }
                                    else
                                    {                                       
                                            //--------------------------------------------------
                                            //Fecha: 02/05/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Poner negrita el nombre del artículo
                                            //--------------------------------------------------
                                            int intInicioArticulo = textoHoja.IndexOf("INSERTO:");
                                            int intFinArticulo = textoHoja.IndexOf(".-");
                                            if (intInicioArticulo > -1 && intFinArticulo > -1)
                                            {
                                                string strNombreArticulo = textoHoja.Substring(intInicioArticulo, intFinArticulo);
                                                oParagraph.Add(new iTextSharp.text.Chunk(strNombreArticulo, _fontGeneralB));

                                                textoHoja = textoHoja.Replace(strNombreArticulo, "");
                                                oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));

                                            }
                                            else
                                            {
                                                //--------------------------------------------------
                                                //Fecha: 14/10/2021
                                                //Autor: Miguel Márquez Beltrán
                                                //Motivo: Poner negrita a los nuevos insertos de 
                                                //          Renuncia a la Nacionalidad.
                                                //--------------------------------------------------
                                                string strConstitucionPoliticaPeruArt53 = "CONSTITUCIÓN POLÍTICA DEL PERÚ: ARTÍCULO 53.- ADQUISICIÓN Y RENUNCIA DE LA NACIONALIDAD. ";

                                                if (bEsEscrituraPublica && textoHoja.IndexOf(strConstitucionPoliticaPeruArt53) > -1)
                                                {
                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita(strConstitucionPoliticaPeruArt53, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                }
                                                string strPerdidaNacionalidad = "LEY N° 26574.- LEY DE NACIONALIDAD.- ARTÍCULO 7.- PERDIDA DE LA NACIONALIDAD. ";
                                                if (bEsEscrituraPublica && textoHoja.IndexOf(strPerdidaNacionalidad) > -1)
                                                {
                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita(strPerdidaNacionalidad, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                }
                                                //--------------------------------------------------

                                                if (bEsEscrituraPublica && textoHoja.IndexOf("CONCLUYE LA SUSCRIPCIÓN") > -1)
                                                {
                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);

                                                    string strElDia = "EL DÍA";
                                                    int intIndiceInicialElDia = textoHoja.IndexOf(strElDia) + strElDia.Length;
                                                    string strDeLoQueDoyFe = "DE LO QUE DOY FE.";
                                                    int intIndiceInicialDeLoQueDoyFe = textoHoja.IndexOf(strDeLoQueDoyFe);
                                                    string strDiaMesAnio = textoHoja.Substring(intIndiceInicialElDia, intIndiceInicialDeLoQueDoyFe - intIndiceInicialElDia);

                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita(strDiaMesAnio, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);

                                                    oParagraph.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                }
                                                else
                                                {
                                                    if ((bEsEscrituraPublica || bEsParte) && textoHoja.IndexOf("FIRMA:") > -1)
                                                    {
                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                        oParagraph.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                    }
                                                    else
                                                    {
                                                        #region Señor Consul

                                                        string strSeniorConsul = "SEÑOR(A) CÓNSUL:";

                                                        if ((bEsEscrituraPublica || bEsParte) && textoHoja.IndexOf(strSeniorConsul) > -1)
                                                        {
                                                            //oParagraph.Add(new iTextSharp.text.Chunk(strSeniorConsul, _fontGeneralB));

                                                            //textoHoja = textoHoja.Replace(strSeniorConsul, "");
                                                            oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                                        }
                                                        else
                                                        {
                                                            if (textoHoja.IndexOf("NACIONALIDAD") > -1 || textoHoja.IndexOf("A FAVOR DE") > -1)
                                                            {
                                                                if (textoHoja.IndexOf("A FAVOR DE") == -1)
                                                                {                                                                   
                                                                    oParagraph.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                                }
                                                                else
                                                                {
                                                                    #region AFavor
                                                                    if (bEsEscrituraPublica)
                                                                    {
                                                                        
                                                                        oParagraph.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                                    }
                                                                    else
                                                                    {
                                                                        #region EsParte
                                                                        if (bEsParte)
                                                                        {
                                                                            int intSirvase = textoHoja.IndexOf("SÍRVASE");
                                                                            int intConsteEl = textoHoja.IndexOf("CONSTE");

                                                                            if (intSirvase > -1 && intConsteEl > -1)
                                                                            {
                                                                                textoHoja = RetornaTextoPosteriorAsignacionNegrita(_sTitulo, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                            }

                                                                            //string strOtorgante = "";
                                                                            //for (int i = 0; i < _listOtorgantes.Count; i++)
                                                                            //{
                                                                            //    strOtorgante = _listOtorgantes[i].Trim() + ",";

                                                                            //    if (textoHoja.IndexOf(strOtorgante) > -1)
                                                                            //    {
                                                                            //        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                            //    }
                                                                            //}
                                                                                                                                                       
                                                                            oParagraph.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                                            //-----------------------------------------------------
                                                                            //Fecha: 30/12/2021
                                                                            //Autor: Miguel Márquez Beltrán
                                                                            //Motivo: Cierre de la primera página.
                                                                            //-----------------------------------------------------
                                                                            if (textoHoja.Trim().Contains("ATENTAMENTE,"))
                                                                            {
                                                                                document.Add(oParagraph);

                                                                                document.NewPage();
                                                                                textoHoja = "";
                                                                                oParagraph.Clear();
                                                                                iContadorLineas = 0;
                                                                            }
                                                                            //-----------------------------------------------------
                                                                        }
                                                                        else
                                                                        {
                                                                            oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                                                        }
                                                                        #endregion
                                                                    }
                                                                    #endregion
                                                                }
                                                            }
                                                            else
                                                            {
                                                                #region FIRMA E IMPRESIÓN DACTILAR
                                                                if ((bEsEscrituraPublica || bEsParte) && (textoHoja.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1 || textoHoja.IndexOf("IMPRESIÓN DACTILAR:") > -1))
                                                                {
                                                                    if (textoHoja.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1)
                                                                    {
                                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita("FIRMA E IMPRESIÓN DACTILAR:", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                    }
                                                                    if (textoHoja.IndexOf("IMPRESIÓN DACTILAR:") > -1)
                                                                    {
                                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita("IMPRESIÓN DACTILAR:", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                    }

                                                                    string strOtorgante = "";

                                                                    for (int i = 0; i < _listOtorgantes.Count; i++)
                                                                    {
                                                                        strOtorgante = _listOtorgantes[i].Trim();
                                                                        if (textoHoja.Contains(strOtorgante))
                                                                        {
                                                                            textoHoja = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                        }
                                                                    }
                                                                    string strInterprete = "";

                                                                    for (int i = 0; i < _listInterpretes.Count; i++)
                                                                    {
                                                                        strInterprete = _listInterpretes[i].Trim();
                                                                        if (textoHoja.Contains(strInterprete))
                                                                        {
                                                                            textoHoja = RetornaTextoPosteriorAsignacionNegrita(strInterprete, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                        }
                                                                    }
                                                                    string strTestigo = "";

                                                                    for (int i = 0; i < _listTestigos.Count; i++)
                                                                    {
                                                                        strTestigo = _listTestigos[i].Trim();
                                                                        if (textoHoja.Contains(strTestigo))
                                                                        {
                                                                            textoHoja = RetornaTextoPosteriorAsignacionNegrita(strTestigo, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                        }
                                                                    }
                                                                    oParagraph.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                                }
                                                                else
                                                                {
                                                                    #region LA PRESENTE ESCRITURA PÚBLICA
                                                                    int intIndiceInicialLaPteEscPublica = textoHoja.IndexOf("LA PRESENTE ESCRITURA PÚBLICA");
                                                                    int intIndiceInicialLibro = textoHoja.IndexOf("LIBRO");

                                                                    if ((bEsEscrituraPublica || bEsParte) && intIndiceInicialLaPteEscPublica > -1 && intIndiceInicialLibro > -1)
                                                                    {
                                                                        string strParte1 = textoHoja.Substring(0, intIndiceInicialLibro);

                                                                        oParagraph.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));

                                                                        int intIndiceInicialRegInstPublicos = textoHoja.IndexOf("DEL REGISTRO DE INSTRUMENTOS PÚBLICOS");

                                                                        string strLibro = textoHoja.Substring(intIndiceInicialLibro, intIndiceInicialRegInstPublicos - intIndiceInicialLibro);

                                                                        oParagraph.Add(new iTextSharp.text.Chunk(strLibro, _fontGeneralB));

                                                                        string strParte2 = textoHoja.Substring(intIndiceInicialRegInstPublicos);

                                                                        if (strParte2.IndexOf("DE ESTE CONSULADO. =") > -1)
                                                                        {                                                                           
                                                                            oParagraph.Add(new iTextSharp.text.Chunk(strParte2.Trim(), _fontGeneral));                                                                           
                                                                        }
                                                                        else
                                                                        {
                                                                            oParagraph.Add(new iTextSharp.text.Chunk(strParte2, _fontGeneral));
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        #region EsParte
                                                                        if (bEsParte)
                                                                        {
                                                                            string strConcluyeLaSuscripcion = "CONCLUYE LA SUSCRIPCIÓN";
                                                                            int intIndiceConcluyeLaSuscripcion = textoHoja.IndexOf(strConcluyeLaSuscripcion);

                                                                            if (intIndiceConcluyeLaSuscripcion > -1)
                                                                            {
                                                                                textoHoja = RetornaTextoPosteriorAsignacionNegrita(_nombreFuncionario + ", ", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                            }

                                                                            //string strOtorgante = "";

                                                                            //for (int i = 0; i < _listOtorgantes.Count; i++)
                                                                            //{
                                                                            //    strOtorgante = _listOtorgantes[i].Trim();
                                                                            //    if (textoHoja.IndexOf(strOtorgante) > -1)
                                                                            //    {
                                                                            //        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraph);
                                                                            //    }
                                                                            //}

                                                                            if (textoHoja.Trim().Length > 0)
                                                                            {
                                                                                //----------------------------------
                                                                                //Fecha: 23/09/2021
                                                                                //Autor: Miguel Márquez Beltrán
                                                                                //Motivo: Adicionar margen superior.
                                                                                //----------------------------------
                                                                                if (textoHoja.Trim().Contains("OFICIO"))
                                                                                {
                                                                                    //oParagraph.Add(new iTextSharp.text.Chunk("\n\n", _fontGeneral));
                                                                                    iContadorLineas = 5;
                                                                                }

                                                                                oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                                                                //-----------------------------------------------------
                                                                                //Fecha: 30/12/2021
                                                                                //Autor: Miguel Márquez Beltrán
                                                                                //Motivo: Cierre de la primera página.
                                                                                //-----------------------------------------------------
                                                                                if (textoHoja.Trim().Contains("ATENTAMENTE,"))
                                                                                {
                                                                                    document.Add(oParagraph);

                                                                                    document.NewPage();
                                                                                    textoHoja = "";
                                                                                    oParagraph.Clear();
                                                                                    iContadorLineas = 0;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //--------- JONATAN SILVA ------------

                                                                            oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                                                        }
                                                                        #endregion
                                                                    }
                                                                    #endregion
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }                                                                                                
                                            }
                                    }

                                    #endregion
                                }
                                else
                                { oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral)); }
                                //====================================================================Fin Si es Escritura Pública

                                document.Add(oParagraph);

                            }

                            //--------------------------------------------------
                            #region Imagen

                            bool bNuevaPaginaParaImagen = true;

                            if (bExistImage)
                            {
                                bExistImage = false;
                                if (_listImagenes.Count > 0)
                                {

                                    oParagraph.Clear();

                                    int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                                    bAplicarImagen = true;

                                    if (iNumLineas > 0 && iNumLineas < 25)
                                    {
                                        for (int y = 0; y < iNumLineas-1; y++)
                                        {

                                            string linea = string.Empty;
                                            for (int x = 0; x < 67; x++)
                                            {
                                                linea += "=";
                                            }

                                            oParagraph.Add(new iTextSharp.text.Chunk(linea + " ", _fontGeneral));

                                        }

                                        goto LineCheck;

                                    }

                                    bNuevaPaginaParaImagen = false;
                                }
                            }

                            if (bAplicarImagen)
                            {

                                foreach (ImagenNotarial imagen in _listImagenes)
                                {
                                    if (bNuevaPaginaParaImagen)
                                        document.NewPage();
                                    else
                                        bNuevaPaginaParaImagen = true;

                                    #region Imagen

                                    cb = writer.DirectContent;
                                    cb.Rectangle(fMargenIzquierdaDoc, fMargenInferiorDoc, fAnchoAreaTexto, fDocumentHeight - (fMargenInferiorDoc + fMargenSuperiorDoc));
                                    cb.Stroke();

                                    string imgPath = string.Empty;


                                    if (File.Exists(imagen.vRuta))
                                    {
                                        imgPath = imagen.vRuta;
                                    }
                                    else
                                    {
                                        imgPath = HttpContext.Current.Server.MapPath("~/Images/img_noDisponible.jpg");
                                    }

                                    iTextSharp.text.Image newImg = iTextSharp.text.Image.GetInstance(imgPath);

                                    float imgHeight = newImg.Height;
                                    float imgWidth = newImg.Width;
                                    float factorRelacion = 1;
                                    bool bHeightMayor = false;

                                    if (imgHeight > imgWidth)
                                    {
                                        factorRelacion = imgHeight / imgWidth;
                                        bHeightMayor = true;
                                    }
                                    else
                                    {
                                        factorRelacion = imgWidth / imgHeight;
                                        bHeightMayor = false;
                                    }


                                    bool bHeightAdecuado = false;
                                    bool bWidthAdecuado = false;


                                    while (!bHeightAdecuado || !bWidthAdecuado)
                                    {

                                        if (!(imgHeight > fDocumentHeight - (fMargenInferiorDoc + fMargenSuperiorDoc)))
                                            bHeightAdecuado = true;


                                        if (!(imgWidth > fAnchoAreaTexto))
                                            bWidthAdecuado = true;

                                        if (bHeightAdecuado != bWidthAdecuado || (!bHeightAdecuado && !bWidthAdecuado))
                                        {
                                            if (bHeightMayor)
                                            {
                                                imgWidth -= 1;
                                                imgHeight -= factorRelacion;
                                            }
                                            else
                                            {
                                                imgHeight -= 1;
                                                imgWidth -= factorRelacion;
                                            }
                                        }
                                    }



                                    newImg.SetAbsolutePosition((document.PageSize.Width / 2) - (imgWidth / 2) + 36,
                                        (document.PageSize.Height / 2) - (imgHeight / 2));
                                    newImg.ScaleToFitHeight = false;
                                    newImg.ScaleToFitLineWhenOverflow = false;
                                    newImg.ScaleAbsoluteHeight(imgHeight);
                                    newImg.ScaleAbsoluteWidth(imgWidth);
                                    document.Add(newImg);

                                    #endregion



                                    parrafo = new iTextSharp.text.Paragraph();
                                    parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                                    parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                                    frase = new iTextSharp.text.Phrase();
                                    frase.Add(new iTextSharp.text.Chunk("\n" + imagen.vTitulo));
                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }

                                document.NewPage();
                                bAplicarImagen = false;

                            }
                            //--------------------------------------------------
                            #endregion
                        }
                    }
                    //=============================Fin Objects
                    #endregion

                    #region Firmas

                    if (_listDocumentoFirma != null)
                    {
                        parrafo = new iTextSharp.text.Paragraph();
                        frase = new iTextSharp.text.Phrase();

                        #region BloqueDocFirma_Otorgante_Anticipante_Vendedor_Donante

                        foreach (DocumentoFirma docFirma in _listDocumentoFirma)
                        {
                            //if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                            //    docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) ||
                            //    docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO) ||
                            //    docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                            //    docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO) ||                                
                            //    docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                            //    docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) 

                            if (ObtenerIniciaRecibe(Convert.ToInt16(docFirma.sTipoParticipante)) == "INICIA")                                 
                            {                                
                                parrafo = new iTextSharp.text.Paragraph();
                                frase = new iTextSharp.text.Phrase();

                                if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 170))
                                {
                                    frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }
                                else
                                {
                                    _iPageNumber++;
                                    //while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 150))
                                    //{
                                    //   document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                    //}

                                    //-------------------------------------------------
                                    int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                                    if (iNumLineas >= 0 && iNumLineas < 25)
                                    {
                                        if (bEsVistaPrevia)
                                        {
                                            for (int y = 0; y < iNumLineas; y++)
                                            {
                                                string linea = string.Empty;
                                                //for (int x = 0; x < 67; x++)
                                                //{
                                                //    linea += "=";
                                                //}
                                                //document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(linea, _fontGeneral)));
                                                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", _fontGeneral)));
                                            }
                                        }
                                        else
                                        {
                                            for (int y = 0; y < iNumLineas-1; y++)
                                            {
                                                string linea = string.Empty;
                                                //for (int x = 0; x < 67; x++)
                                                //{
                                                //    linea += "=";
                                                //}
                                                //document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(linea, _fontGeneral)));
                                                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", _fontGeneral)));
                                            }
                                        }
                                        
                                        if ((bEsVistaPrevia==true && iNumLineas <= 4) || (bEsVistaPrevia==false && iNumLineas <=3))                                     
                                        {
                                            document.NewPage();
                                            document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                        }
                                    }
                                    //-------------------------------------------------
                                }

                                if (docFirma.bIncapacitado == false)
                                {
                                    parrafo = new iTextSharp.text.Paragraph();
                                    frase = new iTextSharp.text.Phrase();

                                    frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 40.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }
                                if (docFirma.bAplicaHuellaDigital)
                                {
                                    cb = writer.DirectContent;
                                    //cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 65f, 80f);
                                    cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 40f, 50f);
                                    cb.Stroke();
                                }

                                parrafo = new iTextSharp.text.Paragraph();
                                parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);
                                frase = new iTextSharp.text.Phrase();
                                frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto.Replace(",", "")));
                                frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                                parrafo.Add(frase);
                                document.Add(parrafo);

                            }
                        }
                        #endregion

                        #region BloqueDocFirma_Testigo_a_Ruego_Interprete

                        foreach (DocumentoFirma docFirma in _listDocumentoFirma)
                        {
                            if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||
                                docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                            {
                               

                                parrafo = new iTextSharp.text.Paragraph();
                                frase = new iTextSharp.text.Phrase();

                                if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 170))
                                {
                                    frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }
                                else
                                {
                                    _iPageNumber++;
                                    //while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 150))
                                    //{
                                    //   document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                    //}

                                    //-------------------------------------------------
                                    int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                                    
                                    if (iNumLineas >= 0 && iNumLineas < 25)
                                    {
                                        if (bEsVistaPrevia)
                                        {
                                            for (int y = 0; y < iNumLineas; y++)
                                            {
                                                string linea = string.Empty;
                                                //for (int x = 0; x < 67; x++)
                                                //{
                                                //    linea += "=";
                                                //}
                                                //document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(linea, _fontGeneral)));
                                                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", _fontGeneral)));
                                            }
                                        }
                                        else
                                        {
                                            for (int y = 0; y < iNumLineas-1; y++)
                                            {
                                                string linea = string.Empty;
                                                //for (int x = 0; x < 67; x++)
                                                //{
                                                //    linea += "=";
                                                //}
                                                //document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(linea, _fontGeneral)));
                                                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", _fontGeneral)));
                                            }
                                        }
                                        //=======================================
                                        //Fecha: 20/04/2020
                                        //Autor: Miguel Márquez Beltrán
                                        //Motivo: Se adicionó la linea:                                        
                                        //=======================================
                                        if ((bEsVistaPrevia == true && iNumLineas <= 4) || (bEsVistaPrevia == false && iNumLineas <= 3))                                        
                                        {
                                            document.NewPage();
                                            document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                        }
                                    }
                                    //-------------------------------------------------
                                }

                                parrafo = new iTextSharp.text.Paragraph();
                                frase = new iTextSharp.text.Phrase();

                                frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 40.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                                parrafo.Add(frase);
                                document.Add(parrafo);

                                if (docFirma.bAplicaHuellaDigital)
                                {
                                    cb = writer.DirectContent;
                                    //cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 65f, 80f);
                                    cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 40f, 50f);
                                    cb.Stroke();
                                }

                                parrafo = new iTextSharp.text.Paragraph();
                                parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);
                                frase = new iTextSharp.text.Phrase();
                                frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto.Replace(",", "")));
                                frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                                parrafo.Add(frase);
                                document.Add(parrafo);
                            }
                        }

                        
                        #endregion

                        //---------------------------------------------------------------
                        //Fecha: 13/01/2022
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar nueva página cuando los
                        //          números de líneas que sobran son menores a 10.
                        //---------------------------------------------------------------
                        if (bEsEscrituraPublica)
                        {
                            int iNoLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                            if (bEsVistaPrevia == true && iNoLineas <= 10 || bEsVistaPrevia == false && iNoLineas <= 9)
                            {
                                document.NewPage();
                                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                            }
                        }
                        //---------------------------------------------------------------

                    }

                    #endregion

                    _iPageNumber = cb.PdfWriter.PageNumber;

                    document.Close();

                    #region Impresion en Navegador


                    Byte[] FileBuffer = ms.ToArray();

                    //---------------------------------------------------------------
                    //Fecha: 25/04/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Enumerar cada pagina del documento PDF
                    //---------------------------------------------------------------
                    bool bGenerarDocumentoPDF = true;

                    if (bEsEscrituraPublica)
                    {
                        if (!bEsTestimonio)
                        {
                            if (!bEsVistaPrevia)
                            {
                                int intNumerofojas = ObtenerNumeroFojas(FileBuffer);
                                if (iFojaRestante < intNumerofojas)
                                { bGenerarDocumentoPDF = false; }
                            }
                            if (bGenerarDocumentoPDF)
                            {
                                FileBuffer = EnumerarPDF(FileBuffer).ToArray();                                
                            }
                            FileBuffer = EtiquetaMinuta(FileBuffer, 1).ToArray();
                        }
                    }
                    //---------------------------------------------------------------

                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                        if (bGenerarDocumentoAutomaticamente)
                        {                            
                            if (bGenerarDocumentoPDF)
                            {
                                GenerarDocumentoPDF();
                            }
                        }
                    }

                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();



                    #endregion

                }
            }
            catch (Exception ex)
            {
                Comun.EjecutarScript(this._page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "DocumentoiTextSharp: CrearDocumentoPDF/", ex.Message));
            }
        }


    
        //----------------------------------------------------------------
        //Fecha: 28/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Imprimir Testimonio y la Escritura Pública
        //----------------------------------------------------------------

        public void CrearTestimonioEscrituraPublicaPDF()
        {
            try
            {
                #region Inicializando Variables

                _fCuerpoFontSize = 12;

                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();
                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();

                bool bAplicarImagen = false;

                float fImageMargin = 45f;// 110.9601f;
                float fMargenInferiorDoc = 100;
                float fMargenSuperiorDoc = 80;//(842 - (_fCuerpoFontSize * fTextLeading * _iLineNumber) - fMargenInferiorDoc);
                float fImageWidth = 57.77f;

                float fMargenIzquierdaDoc = 100;
                if (ConfigurationManager.AppSettings["FNMargenIzquierdo"] != null)
                {
                    fMargenIzquierdaDoc = float.Parse(ConfigurationManager.AppSettings["FNMargenIzquierdo"].ToString());
                    _fMarginLeft = float.Parse(ConfigurationManager.AppSettings["FNMargenIzquierdo"].ToString());
                }
                float fMargenDerechaDoc = 100;
                if (ConfigurationManager.AppSettings["FNMargenDerecho"] != null)
                {
                    fMargenDerechaDoc = float.Parse(ConfigurationManager.AppSettings["FNMargenDerecho"].ToString());
                    _fMarginRight = float.Parse(ConfigurationManager.AppSettings["FNMargenDerecho"].ToString());
                }



                iTextSharp.text.IElement oIElement;
                iTextSharp.text.Paragraph oParagraph = null;

                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, fMargenSuperiorDoc, fMargenInferiorDoc);

                iTextSharp.text.Font _fontGeneral = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);


                #endregion


                using (var ms = new MemoryStream())
                {

                    StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    str.Write("                    " + _sCuerpoHtml);
                    str.Flush();


                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);


                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                    if (bEsBorrador)
                    {
                        PdfWriterEvents writerEvent = new PdfWriterEvents("Hoja borrador. Sin valor legal");
                        writer.PageEvent = writerEvent;
                    }

                    document.Open();

                    objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);
                    float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    float fPosicion = fMargenSuperiorDoc;
                    float fTextSize = 0;

                    #region Imagen

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(SLogoRuta);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 120);
                    img.ScaleAbsoluteHeight(85f);
                    img.ScaleAbsoluteWidth(fImageWidth);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                    iTextSharp.text.Font fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);


                    cb.BeginText();


                    cb.SetFontAndSize(bfTimes, 6);

                    string texto = string.Empty;

                    float pos = 0;
                    float tamPalabra = 0;
                    float ancho = 80f;

                    if (NombreConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                    {
                        ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                        NombreConsulado = NombreConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                    }


                    int iPosicionComa = NombreConsulado.IndexOf(",");

                    if (iPosicionComa >= 0)
                        NombreConsulado = NombreConsulado.Substring(0, iPosicionComa);



                    float posxAcumulado = tamPalabra;

                    foreach (string palabra in NombreConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {

                            cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 130 + pos);
                            cb.ShowText(texto.Trim());
                            texto = string.Empty;

                            pos -= 10;
                            posxAcumulado = 0;
                        }

                        posxAcumulado += tamPalabra;
                        posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                        texto += " " + palabra;
                    }

                    if (texto.Trim() != string.Empty)
                    {
                        cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f), document.PageSize.Height - 130 + pos);
                        cb.ShowText(texto.Trim());
                    }

                    cb.EndText();

                    #endregion

                    int lineaMultiplo = 1;
                    float fDocumentHeight = 842;
                    float fLineaHeight = 12 + 1.5f;

                    #region Si_Es_EscrituraPublica

                    if (bEsEscrituraPublica)
                    {
                        #region Cabecera

                        TextoPosicionadoITextSharp textoPosicionadoTitulo = new TextoPosicionadoITextSharp();
                        textoPosicionadoTitulo.FontSize = 16;
                        if (_tituloBaseFont == null)
                        {
                            textoPosicionadoTitulo.BaseFont = CuerpoBaseFont;
                            textoPosicionadoTitulo.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                        }
                        else
                        {
                            textoPosicionadoTitulo.BaseFont = TituloBaseFont;
                            textoPosicionadoTitulo.FontFamily = new iTextSharp.text.Font(_tituloBaseFont, _fCuerpoFontSize);
                        }

                        //----------------------------------------
                        //Fecha:16/02/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Lineas de espaciado adicional
                        //          para el testimonio.
                        //----------------------------------------
                        if (bEsTestimonio)
                        {
                            lineaMultiplo++;
                            lineaMultiplo++;
                            lineaMultiplo++;
                            lineaMultiplo++;
                        }

                        textoPosicionadoTitulo.FXPosition = fMargenIzquierdaDoc;
                        textoPosicionadoTitulo.FYPosition = (fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1));
                        textoPosicionadoTitulo.TextAligment = HorizontalAlign.Center;
                        textoPosicionadoTitulo.Texto = sTitulo;

                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoTitulo);

                        #endregion
                        //----------------------------------------
                        //Fecha:16/02/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Lineas de espaciado adicional
                        //          para el testimonio.
                        //----------------------------------------
                        if (!bEsTestimonio)
                        {
                            lineaMultiplo = 3;
                        }

                        #region ValidacionTipoActoNotarial

                        if (TipoActoNotarial.Length == 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionadoNumero = new TextoPosicionadoITextSharp();
                            textoPosicionadoNumero.FontSize = _fCuerpoFontSize;
                            textoPosicionadoNumero.BaseFont = CuerpoBaseFont;
                            textoPosicionadoNumero.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                            textoPosicionadoNumero.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionadoNumero.FXPosition = fMargenIzquierdaDoc;
                            textoPosicionadoNumero.TextAligment = HorizontalAlign.Left;
                            textoPosicionadoNumero.Texto = _NumeroEP != string.Empty ? "NÚMERO: " + _NumeroEP : string.Empty;

                            TextoPosicionadoITextSharp textoPosicionadoMinuta = new TextoPosicionadoITextSharp();
                            textoPosicionadoMinuta.FontSize = _fCuerpoFontSize;
                            textoPosicionadoMinuta.BaseFont = CuerpoBaseFont;
                            textoPosicionadoMinuta.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                            textoPosicionadoMinuta.FXPosition = fMargenIzquierdaDoc;

                            textoPosicionadoMinuta.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionadoMinuta.TextAligment = HorizontalAlign.Right;
                            textoPosicionadoMinuta.Texto = _sMinutaEP != string.Empty ? "MINUTA: " + _sMinutaEP : string.Empty;

                            _listTextoPosicionadoiTextSharp.Add(textoPosicionadoNumero);

                            _listTextoPosicionadoiTextSharp.Add(textoPosicionadoMinuta);

                        }
                        else
                        {

                            //----------------------------------------
                            //Fecha:16/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Lineas de espaciado adicional
                            //          para el testimonio.
                            //----------------------------------------
                            if (bEsTestimonio)
                            {
                                lineaMultiplo++;
                                lineaMultiplo++;
                                lineaMultiplo++;
                            }
                            //-----------------------------------------------------------
                            //Fecha:16/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Establecer la letra en negrita para testimonio
                            //-----------------------------------------------------------                           

                            TextoPosicionadoITextSharp textoPosicionTipoEscrituraPublica = new TextoPosicionadoITextSharp();
                            textoPosicionTipoEscrituraPublica.FontSize = _fCuerpoFontSize;
                            textoPosicionTipoEscrituraPublica.BaseFont = CuerpoBaseFont;
                            textoPosicionTipoEscrituraPublica.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionTipoEscrituraPublica.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionTipoEscrituraPublica.FXPosition = fMargenIzquierdaDoc;
                            textoPosicionTipoEscrituraPublica.TextAligment = HorizontalAlign.Left;
                            textoPosicionTipoEscrituraPublica.Texto = _TipoActoNotarial != string.Empty ? "ESCRITURA PÚBLICA DE:" : string.Empty;
                            _listTextoPosicionadoiTextSharp.Add(textoPosicionTipoEscrituraPublica);

                            #region Existe_TipoActoNotarial
                            if (_TipoActoNotarial.Length > 0)
                            {
                                textoPosicionTipoEscrituraPublica = new TextoPosicionadoITextSharp();
                                textoPosicionTipoEscrituraPublica.FontSize = _fCuerpoFontSize;
                                if (bEsTestimonio)
                                {
                                    textoPosicionTipoEscrituraPublica.BaseFont = TituloBaseFont;
                                }
                                else
                                {
                                    textoPosicionTipoEscrituraPublica.BaseFont = CuerpoBaseFont;
                                }
                                textoPosicionTipoEscrituraPublica.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                textoPosicionTipoEscrituraPublica.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                textoPosicionTipoEscrituraPublica.FXPosition = fMargenIzquierdaDoc + 170;
                                textoPosicionTipoEscrituraPublica.TextAligment = HorizontalAlign.NotSet;
                                textoPosicionTipoEscrituraPublica.Texto = TipoActoNotarial;
                                _listTextoPosicionadoiTextSharp.Add(textoPosicionTipoEscrituraPublica);
                            }
                            #endregion

                            lineaMultiplo++;
                        }
                        #endregion

                        lineaMultiplo++;


                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        #region Lista_Otorgantes


                        if (_listOtorgantes.Count > 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                            textoPosicionado.FontSize = 12;
                            textoPosicionado.BaseFont = CuerpoBaseFont;
                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);


                            if (TipoActoNotarial.Length == 0)
                            {
                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                if (_listOtorgantes.Count == 1)
                                    textoPosicionado.Texto = "QUE OTORGA:";
                                else
                                    textoPosicionado.Texto = "QUE OTORGAN:";

                            }
                            else
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Indicar el subtitulo "Otorgado por:".
                                //-------------------------------------------------------------

                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                textoPosicionado.Texto = "OTORGADO POR:";
                            }

                            if (TipoActoNotarial.Length == 0)
                            { lineaMultiplo++; }



                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                            #region Si_No_Existe_ActoNotarial
                            if (TipoActoNotarial.Length == 0)
                            {
                                for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantes.Count; i++)
                                {
                                    string otorgante = _listOtorgantes[i].Trim();
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = 12;
                                    textoPosicionado.BaseFont = CuerpoBaseFont;
                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                    textoPosicionado.Texto = otorgante;

                                    lineaMultiplo++;
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                            }
                            #endregion

                            #region Si_Existe_ActoNotarial

                            if (TipoActoNotarial.Length > 0)
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                //-------------------------------------------------------------      

                                string strcadOtorgantes = "";
                                List<string> strArrayOtorgantes = new List<string>();
                                int intPar = 0;
                                string strCadenaIzquierda = "";
                                int intAnchoCadenaNombres = 43;
                                int intUltimoEspacio = 0;

                                #region ForOtorgantes
                                for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantes.Count; i++)
                                {
                                    intPar++;
                                    string otorgante = _listOtorgantes[i].Trim();
                                    if (_listOtorgantes.Count > 2)
                                    {
                                        if (i + 2 < _listOtorgantes.Count)
                                        {
                                            strcadOtorgantes = strcadOtorgantes + otorgante + ", ";
                                        }
                                        else
                                        {
                                            strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                        }
                                    }
                                    else
                                    {
                                        strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                    }
                                    if (intPar % 2 == 0)
                                    {
                                        if (strcadOtorgantes.Length > intAnchoCadenaNombres)
                                        {
                                            intUltimoEspacio = strcadOtorgantes.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                            strCadenaIzquierda = strcadOtorgantes.Substring(0, intUltimoEspacio);
                                            strArrayOtorgantes.Add(strCadenaIzquierda);
                                            strcadOtorgantes = strcadOtorgantes.Substring(intUltimoEspacio + 1);
                                        }
                                        else
                                        {
                                            strArrayOtorgantes.Add(strcadOtorgantes);
                                            strcadOtorgantes = "";
                                        }
                                    }
                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                                #endregion

                                if (_listOtorgantes.Count > 2)
                                {
                                    strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 3);
                                }
                                else
                                {
                                    strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 2);
                                }
                                if (strcadOtorgantes.Length > intAnchoCadenaNombres)
                                {
                                    intUltimoEspacio = strcadOtorgantes.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                    strCadenaIzquierda = strcadOtorgantes.Substring(0, intUltimoEspacio);
                                    strArrayOtorgantes.Add(strCadenaIzquierda);
                                    strcadOtorgantes = strcadOtorgantes.Substring(intUltimoEspacio + 1);

                                    strArrayOtorgantes.Add(strcadOtorgantes);
                                }
                                else
                                {
                                    strArrayOtorgantes.Add(strcadOtorgantes);
                                }

                                strcadOtorgantes = "";

                                #region ForOtorgantes
                                for (int i = 0; i < strArrayOtorgantes.Count; i++)
                                {
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = 12;

                                    //-----------------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Establecer la letra en negrita para testimonio
                                    //-----------------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        textoPosicionado.BaseFont = TituloBaseFont;
                                    }
                                    else
                                    {
                                        textoPosicionado.BaseFont = CuerpoBaseFont;
                                    }


                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                    textoPosicionado.TextAligment = HorizontalAlign.NotSet;
                                    textoPosicionado.Texto = strArrayOtorgantes[i].ToString();
                                    textoPosicionado.FXPosition = fMargenIzquierdaDoc + 100;
                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                    lineaMultiplo++;
                                }
                                #endregion

                                strArrayOtorgantes.Clear();
                            }
                            #endregion
                        }

                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        lineaMultiplo++;

                        #region Si_Existe_ListaApoderados

                        if (_listApoderados.Count > 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                            textoPosicionado.FontSize = 12;
                            textoPosicionado.BaseFont = CuerpoBaseFont;
                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                            if (TipoActoNotarial.Length == 0)
                            {
                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                            }
                            else
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda el texto: "a favor de".
                                //-------------------------------------------------------------

                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                            }
                            textoPosicionado.Texto = "A FAVOR DE:";

                            //---------------------------------------
                            //Fecha: 27/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Solo si no es testimonio aumentar una linea
                            //      si no en la misma linea.
                            //---------------------------------------
                            if (!bEsTestimonio)
                            {
                                lineaMultiplo++;
                            }

                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                            #region Si_No_Existe_TipoActoNotarial
                            if (TipoActoNotarial.Length == 0)
                            {
                                for (int i = (_iPageNumber - 1) * 10; i < _listApoderados.Count; i++)
                                {
                                    string apoderado = _listApoderados[i].Trim();

                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = 12;
                                    textoPosicionado.BaseFont = CuerpoBaseFont;
                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                    textoPosicionado.Texto = apoderado;

                                    lineaMultiplo++;
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);


                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                            }
                            #endregion

                            #region Si_Existe_TipoActoNotarial
                            if (TipoActoNotarial.Length > 0)
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda a los apoderados.
                                //-------------------------------------------------------------
                                string strcadApoderados = "";
                                List<string> strArrayApoderados = new List<string>();
                                int intPar = 0;
                                string strCadenaIzquierda = "";
                                int intAnchoCadenaNombres = 43;
                                int intUltimoEspacio = 0;

                                #region ForApoderados
                                for (int i = (_iPageNumber - 1) * 10; i < _listApoderados.Count; i++)
                                {
                                    intPar++;
                                    string apoderado = _listApoderados[i].Trim();
                                    if (_listApoderados.Count > 2)
                                    {
                                        if (i + 2 < _listApoderados.Count)
                                        {
                                            strcadApoderados = strcadApoderados + apoderado + ", ";
                                        }
                                        else
                                        {
                                            strcadApoderados = strcadApoderados + apoderado + " Y ";
                                        }
                                    }
                                    else
                                    {
                                        strcadApoderados = strcadApoderados + apoderado + " Y ";
                                    }
                                    if (intPar % 2 == 0)
                                    {
                                        if (strcadApoderados.Length > intAnchoCadenaNombres)
                                        {
                                            intUltimoEspacio = strcadApoderados.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                            strCadenaIzquierda = strcadApoderados.Substring(0, intUltimoEspacio);
                                            strArrayApoderados.Add(strCadenaIzquierda);
                                            strcadApoderados = strcadApoderados.Substring(intUltimoEspacio + 1);
                                        }
                                        else
                                        {
                                            strArrayApoderados.Add(strcadApoderados);
                                            strcadApoderados = "";
                                        }
                                    }
                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                                #endregion
                                if (_listApoderados.Count > 2)
                                {
                                    strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 3);
                                }
                                else
                                {
                                    strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 2);
                                }
                                if (strcadApoderados.Length > intAnchoCadenaNombres)
                                {
                                    intUltimoEspacio = strcadApoderados.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                    strCadenaIzquierda = strcadApoderados.Substring(0, intUltimoEspacio);
                                    strArrayApoderados.Add(strCadenaIzquierda);
                                    strcadApoderados = strcadApoderados.Substring(intUltimoEspacio + 1);

                                    strArrayApoderados.Add(strcadApoderados);
                                }
                                else
                                {
                                    strArrayApoderados.Add(strcadApoderados);
                                }

                                strcadApoderados = "";

                                #region Apoderados

                                for (int i = 0; i < strArrayApoderados.Count; i++)
                                {
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = 12;
                                    //-----------------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Establecer la letra en negrita para testimonio
                                    //-----------------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        textoPosicionado.BaseFont = TituloBaseFont;
                                    }
                                    else
                                    {
                                        textoPosicionado.BaseFont = CuerpoBaseFont;
                                    }

                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                    textoPosicionado.TextAligment = HorizontalAlign.NotSet;
                                    textoPosicionado.Texto = strArrayApoderados[i].ToString();
                                    textoPosicionado.FXPosition = fMargenIzquierdaDoc + 100;
                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                    lineaMultiplo++;
                                }
                                #endregion
                                strArrayApoderados.Clear();
                            }
                            #endregion
                        }
                        #endregion


                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        //lineaMultiplo++; 

                        #region LineaDivisora

                        TextoPosicionadoITextSharp textoPosicionadoITS = new TextoPosicionadoITextSharp();
                        textoPosicionadoITS.FontSize = _fCuerpoFontSize;
                        textoPosicionadoITS.BaseFont = CuerpoBaseFont;
                        textoPosicionadoITS.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                        textoPosicionadoITS.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                        textoPosicionadoITS.FXPosition = fMargenIzquierdaDoc;
                        //textoPosicionadoITS.FXPosition = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;
                        textoPosicionadoITS.TextAligment = HorizontalAlign.Center;
                        
                        textoPosicionadoITS.Texto = "_______________________________________________________________";                        

                        lineaMultiplo++;
                        lineaMultiplo++;
                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoITS);

                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                    }

                    #endregion

                    int iContadorLineas = 0;

                    #region Cabecera Parte : Fecha
                    if (bEsParte)
                    {
                        iContadorLineas = 1;

                        TextoPosicionadoITextSharp textoPosicionadoFecha = new TextoPosicionadoITextSharp();
                        textoPosicionadoFecha.FontSize = _fCuerpoFontSize;
                        textoPosicionadoFecha.BaseFont = CuerpoBaseFont;
                        textoPosicionadoFecha.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                        textoPosicionadoFecha.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                        textoPosicionadoFecha.TextAligment = HorizontalAlign.Right;
                        textoPosicionadoFecha.Texto = sFecha;

                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoFecha);

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();
                    }
                    #endregion

                    #region Cuerpo Documento


                    EnumerarFoja(cb, document);


                    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                    while ((int)fDocumentPosition + (_fCuerpoFontSize * FCuerpoLeading) < ((int)writer.GetVerticalPosition(false)))
                    {
                        oParagraph = new iTextSharp.text.Paragraph();
                        oParagraph.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                        oParagraph.SetLeading(0.0f, 1.42f);
                        document.Add(oParagraph);
                    }

                    _iPageNumber = 1;

                    //-------------------------------------------------------------
                    //Fecha: 10/02/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Eliminar el salto de página para el testimonio
                    //-------------------------------------------------------------
                    //if (_bEsTestimonio)
                    //{
                    //    document.NewPage();
                    //    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc;
                    //    _iPageNumber++;
                    //}
                    //-------------------------------------------------------------

                    bool bExistImage = false;


                    for (int k = 0; k < objects.Count; k++)
                    {

                        oIElement = (iTextSharp.text.IElement)objects[k];

                        bool bBorrarParagraph = true;

                        if (((iTextSharp.text.IElement)objects[k]).Chunks.Count == 0)
                        {
                            oParagraph.Clear();
                            oParagraph.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                            bBorrarParagraph = false;
                        }



                        if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            if (bBorrarParagraph)
                            {
                                oParagraph = new iTextSharp.text.Paragraph();
                                oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;
                                oParagraph.Font = _fontGeneral;
                            }
                            bBorrarParagraph = true;

                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);


                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {

                                if (bEsParte)
                                    strContent = oIElement.Chunks[z].Content.ToString();
                                else
                                {
                                    //-----------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Si es testimonio no convertir a mayusuculas.
                                    //-----------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        strContent = oIElement.Chunks[z].Content.ToString();
                                    }
                                    else
                                    {
                                        strContent = oIElement.Chunks[z].Content.ToString().ToUpper();
                                    }
                                }



                                if (strContent.Contains("#IMAGEN#"))
                                {
                                    strContent = strContent.Replace("#IMAGEN#", "");
                                    bExistImage = true;
                                }



                                while (strContent.Contains(" .") || strContent.Contains(" ,") ||
                                    strContent.Contains(" ;") || strContent.Contains(" :"))
                                {
                                    strContent = strContent.Replace(" .", ".").Replace(" ,", ",").Replace(" :", ":").Replace(" ;", ";");
                                }

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

                                    if (strContent != "\n")
                                    {
                                        strContent = strContent.Trim();
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        continue;
                                    }

                                    if (z == oIElement.Chunks.Count - 1)
                                    {
                                        List<string> listTextos = new List<string>();
                                        List<iTextSharp.text.Font> listFonts = new List<iTextSharp.text.Font>();
                                        string textoNotarialCierre = string.Empty;

                                        string strContenido = string.Empty;
                                        foreach (iTextSharp.text.Chunk ch in oIElement.Chunks)
                                        {
                                            if (ch.Content.Contains("#IMAGEN#"))
                                            {
                                                strContenido = string.Empty;
                                            }
                                            strContenido = ch.Content.Replace("#IMAGEN#", "");


                                            if (strContenido == string.Empty)
                                                continue;

                                            if (strContenido != "\n")
                                                listTextos.Add(strContenido.Trim());
                                            else
                                                listTextos.Add("\n");

                                            ch.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                                            listFonts.Add(ch.Font);

                                        }


                                        bool bEscribirTextoCierro = true;

                                        if (_listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in _listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim().Contains(palabra))
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                        }

                                        if (bEscribirTextoCierro)
                                        {
                                            if (listTextos.Count > 0)
                                                textoNotarialCierre = ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                            if (bEsTestimonio)
                                            {
                                                if (textoNotarialCierre != string.Empty)
                                                {
                                                    textoNotarialCierre = textoNotarialCierre.Substring(0, textoNotarialCierre.Length - 1);
                                                }
                                            }
                                        }


                                        if (textoNotarialCierre != string.Empty)
                                        {
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(oIElement.Chunks[z].Font);
                                            font.SetStyle(0);
                                            oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                        }

                                    }
                                    else
                                    {
                                        oParagraph.Add(new iTextSharp.text.Chunk(" ", oIElement.Chunks[z].Font));
                                    }

                                    #endregion
                                }


                            }


                            bool bBorrar = false;
                            for (int i = oParagraph.Chunks.Count - 1; i >= 0; i--)
                            {
                                if (oParagraph.Chunks[i].Content == string.Empty)
                                    continue;

                                if (!bBorrar)
                                {
                                    if (oParagraph.Chunks[i].Content[0] == '.' || oParagraph.Chunks[i].Content[0] == ',' ||
                                        oParagraph.Chunks[i].Content[0] == ':' || oParagraph.Chunks[i].Content[0] == ';')
                                    {
                                        bBorrar = true;
                                    }
                                }
                                else
                                {
                                    oParagraph.RemoveAt(i);
                                    bBorrar = false;
                                }

                            }

                            #region LineCheck
                        
                        LineCheck:

                            float fLineaWidth = 0;
                            float fPalabraWidth = 0;
                            string sLineaAcumulada = string.Empty;
                            List<string> listLineas = new List<string>();


                            foreach (iTextSharp.text.Chunk par in oParagraph.Chunks)
                            {
                                if (par.Content == "\n")
                                {
                                    listLineas.Add("\n");
                                    continue;
                                }

                                if (!bEsParte)
                                {
                                    //-----------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Si es testimonio no convertir a mayusuculas.
                                    //-----------------------------------------------------

                                    if (bEsTestimonio)
                                    {
                                        foreach (string palabra in par.Content.Trim().Split(' '))
                                        {
                                            fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                            if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                            {
                                                if (sLineaAcumulada.Trim() != string.Empty)
                                                {
                                                    listLineas.Add(sLineaAcumulada.Trim());
                                                }

                                                else
                                                    listLineas.Add("\n");
                                                fLineaWidth = 0;
                                                sLineaAcumulada = string.Empty;
                                            }

                                            if (palabra.Trim() != string.Empty)
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                                fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                            }
                                        }

                                    }
                                    else
                                    {
                                        foreach (string palabra in par.Content.ToUpper().Trim().Split(' '))
                                        {
                                            fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                            if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                            {
                                                if (sLineaAcumulada.Trim() != string.Empty)
                                                {
                                                    listLineas.Add(sLineaAcumulada.Trim());
                                                }

                                                else
                                                    listLineas.Add("\n");
                                                fLineaWidth = 0;
                                                sLineaAcumulada = string.Empty;
                                            }

                                            if (palabra.Trim() != string.Empty)
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                                fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                            }
                                        }

                                    }
                                }
                                else
                                {

                                    foreach (string palabra in par.Content.Trim().Split(' '))
                                    {
                                        fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                        if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                        {
                                            if (sLineaAcumulada.Trim() != string.Empty)
                                            {
                                                listLineas.Add(sLineaAcumulada.Trim());
                                            }

                                            else
                                                listLineas.Add("\n");
                                            fLineaWidth = 0;
                                            sLineaAcumulada = string.Empty;
                                        }

                                        if (palabra.Trim() != string.Empty)
                                        {
                                            sLineaAcumulada += palabra.Trim() + " ";
                                            fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                        }
                                    }

                                }
                            }


                            if (sLineaAcumulada.Trim() != string.Empty)
                            {
                                listLineas.Add(sLineaAcumulada.Trim());
                            }
                            else
                                listLineas.Add("\n");



                            #endregion




                            oParagraph.SetLeading(0.0f, FCuerpoLeading);

                            iContadorLineas += listLineas.Count;


                            if (iContadorLineas >= _iLineNumberGeneral)
                            {
                                int iLineasSobrantes = iContadorLineas - _iLineNumberGeneral;
                                int iUltimaLineaPrimerTrozo = listLineas.Count - iLineasSobrantes;
                                string lineasPrimeraTrozo = string.Empty;
                                string lineasSegundaTrozo = string.Empty;


                                listLineas[iUltimaLineaPrimerTrozo - 1] = ObtenerLineaJustificada(
                                    new iTextSharp.text.Chunk(listLineas[iUltimaLineaPrimerTrozo - 1],
                                        _fontGeneral),
                                        document.PageSize.Width - (_fMarginLeft + _fMarginRight));

                                for (int i = 0; i < listLineas.Count; i++)
                                {

                                    if (i < iUltimaLineaPrimerTrozo)
                                    {
                                        lineasPrimeraTrozo += listLineas[i] + " ";
                                    }
                                    else
                                    {
                                        lineasSegundaTrozo += listLineas[i] + " ";
                                    }
                                }


                                bool bNuevaPagina = true;
                                if (writer.GetVerticalPosition(false) != 742)
                                    bNuevaPagina = false;

                                oParagraph.Clear();
                                oParagraph.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo.Trim(), _fontGeneral));
                                document.Add(oParagraph);


                                if (bNuevaPagina)
                                {
                                    document.NewPage();

                                }



                                _iLineNumber = 25.7f;
                                _iPageNumber++;

                                lineaMultiplo = 0;

                                if (bEsEscrituraPublica)
                                {
                                    if ((_listOtorgantes.Count > (_iPageNumber - 1) * 10) || (_listApoderados.Count > (_iPageNumber - 1) * 10))
                                    {

                                        fDocumentHeight = 842;
                                        fLineaHeight = 12 + 1.5f;


                                        #region Otorgantes


                                        if (_listOtorgantes.Count > (_iPageNumber - 1) * 10)
                                        {
                                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                                            textoPosicionado.FontSize = 12;
                                            textoPosicionado.BaseFont = CuerpoBaseFont;
                                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;

                                            if (TipoActoNotarial.Length == 0)
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                textoPosicionado.Texto = "QUE OTORGA:";
                                            }
                                            else
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Indicar el texto: "Otorgado por".
                                                //-------------------------------------------------------------

                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = "OTORGADO POR:";
                                            }
                                            lineaMultiplo++;

                                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                            string strcadOtorgantes = "";

                                            for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantes.Count; i++)
                                            {
                                                string otorgante = _listOtorgantes[i].Trim();
                                                textoPosicionado = new TextoPosicionadoITextSharp();
                                                textoPosicionado.FontSize = 12;
                                                textoPosicionado.BaseFont = CuerpoBaseFont;
                                                textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                                //-------------------------------------------------------------

                                                if (TipoActoNotarial.Length == 0)
                                                {
                                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                    textoPosicionado.Texto = otorgante;

                                                    lineaMultiplo++;
                                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                                }
                                                else
                                                {
                                                    strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                                }


                                                if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                                    break;
                                            }

                                            if (TipoActoNotarial.Length > 0)
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                                //-------------------------------------------------------------
                                                strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 3);
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = strcadOtorgantes;
                                                textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                                _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                            }
                                        }


                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                        #region Apoderados

                                        if (_listApoderados.Count > (_iPageNumber - 1) * 10)
                                        {
                                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                                            textoPosicionado.FontSize = 12;
                                            textoPosicionado.BaseFont = CuerpoBaseFont;
                                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                            //-------------------------------------------------------------
                                            //Fecha: 10/02/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Alinear a la izquierda a los apoderados.
                                            //-------------------------------------------------------------

                                            if (TipoActoNotarial.Length == 0)
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                            }
                                            textoPosicionado.Texto = "A FAVOR DE:";

                                            lineaMultiplo++;

                                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                            string strcadApoderados = "";

                                            for (int i = (_iPageNumber - 1) * 10; i < _listApoderados.Count; i++)
                                            {
                                                string apoderado = _listApoderados[i].Trim();
                                                textoPosicionado = new TextoPosicionadoITextSharp();
                                                textoPosicionado.FontSize = 12;
                                                textoPosicionado.BaseFont = CuerpoBaseFont;
                                                textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                                if (TipoActoNotarial.Length == 0)
                                                {
                                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                    textoPosicionado.Texto = apoderado;

                                                    lineaMultiplo++;
                                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                                }
                                                else
                                                {
                                                    strcadApoderados = strcadApoderados + apoderado + " Y ";
                                                }


                                                if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                                    break;
                                            }
                                            if (TipoActoNotarial.Length > 0)
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los apoderados.
                                                //-------------------------------------------------------------
                                                strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 3);
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = strcadApoderados;
                                                textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                                _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                            }

                                        }


                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                        #region LineaDivisora

                                        TextoPosicionadoITextSharp textoPosicionadoITS = new TextoPosicionadoITextSharp();
                                        textoPosicionadoITS.FontSize = 12;
                                        textoPosicionadoITS.BaseFont = CuerpoBaseFont;
                                        textoPosicionadoITS.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                        textoPosicionadoITS.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                        textoPosicionadoITS.TextAligment = HorizontalAlign.Center;
                                        textoPosicionadoITS.Texto = "______________________________________________________";

                                        lineaMultiplo++;
                                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoITS);

                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();


                                    }

                                    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);


                                    while ((int)fDocumentPosition < ((int)writer.GetVerticalPosition(false)))
                                    {
                                        oParagraph.Clear();
                                        oParagraph.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                                        oParagraph.SetLeading(0.0f, 1.42f);
                                        document.Add(oParagraph);


                                    }

                                }

                                if (lineasSegundaTrozo.Trim() != string.Empty)
                                {
                                    oParagraph.Clear();
                                    oParagraph.SetLeading(0.0f, FCuerpoLeading);
                                    oParagraph.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                    document.Add(oParagraph);
                                }

                                iContadorLineas = iLineasSobrantes;

                                while (iContadorLineas >= _iLineNumberGeneral)
                                {
                                    iContadorLineas -= _iLineNumberGeneral;
                                }

                                if (iContadorLineas == 0)
                                {
                                    document.NewPage();
                                }
                                EnumerarFoja(cb, document);

                            }
                            else
                            {

                                string textoHoja = string.Empty;

                                for (int i = 0; i < listLineas.Count; i++)
                                {

                                    textoHoja += listLineas[i] + " ";

                                }

                                oParagraph.Clear();
                                oParagraph.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                document.Add(oParagraph);

                            }

                            bool bNuevaPaginaParaImagen = true;


                            if (bExistImage)
                            {
                                bExistImage = false;
                                if (_listImagenes.Count > 0)
                                {

                                    oParagraph.Clear();

                                    int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                                    bAplicarImagen = true;

                                    if (iNumLineas > 0 && iNumLineas < 25)
                                    {
                                        for (int y = 0; y < iNumLineas; y++)
                                        {

                                            string linea = string.Empty;
                                            for (int x = 0; x < 54; x++)
                                            {
                                                linea += "=";
                                            }

                                            oParagraph.Add(new iTextSharp.text.Chunk(linea + " ", _fontGeneral));

                                        }

                                        goto LineCheck;

                                    }

                                    bNuevaPaginaParaImagen = false;
                                }
                            }

                            if (bAplicarImagen)
                            {

                                foreach (ImagenNotarial imagen in _listImagenes)
                                {
                                    if (bNuevaPaginaParaImagen)
                                        document.NewPage();
                                    else
                                        bNuevaPaginaParaImagen = true;

                                    #region Imagen

                                    cb = writer.DirectContent;
                                    cb.Rectangle(fMargenIzquierdaDoc, fMargenInferiorDoc, fAnchoAreaTexto, fDocumentHeight - (fMargenInferiorDoc + fMargenSuperiorDoc));
                                    cb.Stroke();



                                    string imgPath = string.Empty;


                                    if (File.Exists(imagen.vRuta))
                                    {
                                        imgPath = imagen.vRuta;
                                    }
                                    else
                                    {
                                        imgPath = HttpContext.Current.Server.MapPath("~/Images/img_noDisponible.jpg");
                                    }

                                    iTextSharp.text.Image newImg = iTextSharp.text.Image.GetInstance(imgPath);

                                    float imgHeight = newImg.Height;
                                    float imgWidth = newImg.Width;
                                    float factorRelacion = 1;
                                    bool bHeightMayor = false;

                                    if (imgHeight > imgWidth)
                                    {
                                        factorRelacion = imgHeight / imgWidth;
                                        bHeightMayor = true;
                                    }
                                    else
                                    {
                                        factorRelacion = imgWidth / imgHeight;
                                        bHeightMayor = false;
                                    }


                                    bool bHeightAdecuado = false;
                                    bool bWidthAdecuado = false;



                                    while (!bHeightAdecuado || !bWidthAdecuado)
                                    {

                                        if (!(imgHeight > fDocumentHeight - (fMargenInferiorDoc + fMargenSuperiorDoc)))
                                            bHeightAdecuado = true;


                                        if (!(imgWidth > fAnchoAreaTexto))
                                            bWidthAdecuado = true;


                                        if (bHeightAdecuado != bWidthAdecuado || (!bHeightAdecuado && !bWidthAdecuado))
                                        {
                                            if (bHeightMayor)
                                            {
                                                imgWidth -= 1;
                                                imgHeight -= factorRelacion;
                                            }
                                            else
                                            {
                                                imgHeight -= 1;
                                                imgWidth -= factorRelacion;
                                            }
                                        }

                                    }



                                    newImg.SetAbsolutePosition((document.PageSize.Width / 2) - (imgWidth / 2) + 36,
                                        (document.PageSize.Height / 2) - (imgHeight / 2));
                                    newImg.ScaleToFitHeight = false;
                                    newImg.ScaleToFitLineWhenOverflow = false;
                                    newImg.ScaleAbsoluteHeight(imgHeight);
                                    newImg.ScaleAbsoluteWidth(imgWidth);
                                    document.Add(newImg);

                                    #endregion



                                    parrafo = new iTextSharp.text.Paragraph();
                                    parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                                    parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                                    frase = new iTextSharp.text.Phrase();
                                    frase.Add(new iTextSharp.text.Chunk("\n" + imagen.vTitulo));
                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }

                                document.NewPage();
                                bAplicarImagen = false;
                                EnumerarFoja(cb, document);
                            }


                        }
                    }

                    #endregion

                    #region Firmas

                    if (_listDocumentoFirma != null)
                    {
                        parrafo = new iTextSharp.text.Paragraph();
                        frase = new iTextSharp.text.Phrase();

                        foreach (DocumentoFirma docFirma in _listDocumentoFirma)
                        {
                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();

                            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 120))
                            {

                                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                                parrafo.Add(frase);
                                document.Add(parrafo);
                            }
                            else
                            {
                                _iPageNumber++;
                                while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 120))
                                {
                                    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                                }
                                EnumerarFoja(cb, document);
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 50.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                            if (docFirma.bAplicaHuellaDigital)
                            {
                                cb = writer.DirectContent;
                                cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 65f, 80f);
                                cb.Stroke();
                            }

                            parrafo = new iTextSharp.text.Paragraph();
                            parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                            frase = new iTextSharp.text.Phrase();
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto.Replace(",", "")));
                            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                            parrafo.Add(frase);
                            document.Add(parrafo);

                        }
                    }

                    #endregion

                    //==========================================================================
                    //Datos de la Escritura Pública
                    //--------------------------------------------------------------------------
                    TipoActoNotarial = "";
                    document.NewPage();

                    lineaMultiplo = 1;
                    fDocumentHeight = 842;
                    fLineaHeight = 12 + 1.5f;
                    fImageMargin = 45f;// 110.9601f;
                    fMargenInferiorDoc = 100;
                    fMargenSuperiorDoc = 80;//(842 - (_fCuerpoFontSize * fTextLeading * _iLineNumber) - fMargenInferiorDoc);
                    fImageWidth = 57.77f;
                    fMargenIzquierdaDoc = 100;

                    if (ConfigurationManager.AppSettings["FNMargenIzquierdo"] != null)
                    {
                        fMargenIzquierdaDoc = float.Parse(ConfigurationManager.AppSettings["FNMargenIzquierdo"].ToString());
                        _fMarginLeft = float.Parse(ConfigurationManager.AppSettings["FNMargenIzquierdo"].ToString());
                    }
                    fMargenDerechaDoc = 100;
                    if (ConfigurationManager.AppSettings["FNMargenDerecho"] != null)
                    {
                        fMargenDerechaDoc = float.Parse(ConfigurationManager.AppSettings["FNMargenDerecho"].ToString());
                        _fMarginRight = float.Parse(ConfigurationManager.AppSettings["FNMargenDerecho"].ToString());
                    }

                    bEsEscrituraPublica = true;
                    bEsTestimonio = false;

                   
                    _fCuerpoFontSize = 10.5f;
                   
                    #region Imagen

                    img = iTextSharp.text.Image.GetInstance(SLogoRuta);
                    img.SetAbsolutePosition(fImageMargin, document.PageSize.Height - 120);
                    img.ScaleAbsoluteHeight(85f);
                    img.ScaleAbsoluteWidth(fImageWidth);
                    document.Add(img);

                    #endregion

                    #region Consulado Imagen

                    cb = writer.DirectContent;
                    _fontGeneral = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                    bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                    iTextSharp.text.Font _fontGeneralB = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.pdf.BaseFont bfontCourier = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.COURIER_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                    iTextSharp.text.Font _fontTituloB = new iTextSharp.text.Font(bfontCourier, 16);
                    fontConsulado = iTextSharp.text.FontFactory.GetFont("Arial", 6);


                    cb.BeginText();


                    cb.SetFontAndSize(bfTimes, 6);

                    texto = string.Empty;

                    pos = 0;
                    tamPalabra = 0;
                    ancho = 80f;

                    if (NombreConsulado.ToUpper().Contains("CONSULADO GENERAL DEL PERÚ"))
                    {
                        ancho = new iTextSharp.text.Chunk("CONSULADO GENERAL DEL PERÚ", fontConsulado).GetWidthPoint() + 5;
                        NombreConsulado = NombreConsulado.ToUpper().Replace("PERÚ EN", "PERÚ");
                    }


                    iPosicionComa = NombreConsulado.IndexOf(",");

                    if (iPosicionComa >= 0)
                        NombreConsulado = NombreConsulado.Substring(0, iPosicionComa);



                    posxAcumulado = tamPalabra;

                    foreach (string palabra in NombreConsulado.Split(' '))
                    {
                        tamPalabra = new iTextSharp.text.Chunk(palabra.Trim(), fontConsulado).GetWidthPoint();

                        if (posxAcumulado + tamPalabra > ancho)
                        {

                            cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f)-10, document.PageSize.Height - 130 + pos);
                            cb.ShowText(texto.Trim());
                            texto = string.Empty;

                            pos -= 10;
                            posxAcumulado = 0;
                        }

                        posxAcumulado += tamPalabra;
                        posxAcumulado += new iTextSharp.text.Chunk(" ", fontConsulado).GetWidthPoint();
                        texto += " " + palabra;
                    }

                    if (texto.Trim() != string.Empty)
                    {
                        cb.SetTextMatrix(fImageMargin + (fImageWidth / 2) - (new iTextSharp.text.Chunk(texto.Trim(), fontConsulado).GetWidthPoint() / 2f)-10, document.PageSize.Height - 130 + pos);
                        cb.ShowText(texto.Trim());
                    }

                    cb.EndText();

                    #endregion

                    lineaMultiplo = 1;
                    fDocumentHeight = 842;
                    fLineaHeight = 12 + 1.5f;

                    #region Si_Es_EscrituraPublica

                    if (bEsEscrituraPublica)
                    {
                        #region Cabecera

                        TextoPosicionadoITextSharp textoPosicionadoTitulo = new TextoPosicionadoITextSharp();
                        textoPosicionadoTitulo.FontSize = 16;
                        if (_tituloBaseFont == null)
                        {
                            if (sTituloEC.Length > 0)
                            {
                                textoPosicionadoTitulo.BaseFont = bfontCourier;
                                textoPosicionadoTitulo.FontFamily = _fontTituloB;
                            }
                            else
                            {
                                textoPosicionadoTitulo.BaseFont = CuerpoBaseFont;
                                textoPosicionadoTitulo.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            }
                        }
                        else
                        {
                            textoPosicionadoTitulo.BaseFont = TituloBaseFont;
                            textoPosicionadoTitulo.FontFamily = new iTextSharp.text.Font(_tituloBaseFont, _fCuerpoFontSize);
                        }

                       

                        textoPosicionadoTitulo.FXPosition = fMargenIzquierdaDoc;
                        textoPosicionadoTitulo.FYPosition = (fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1));
                        textoPosicionadoTitulo.TextAligment = HorizontalAlign.Center;
                        textoPosicionadoTitulo.Texto = sTituloEC;

                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoTitulo);

                        #endregion

                        //----------------------------------------
                        //Fecha:16/02/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Lineas de espaciado adicional
                        //          para el testimonio.
                        //----------------------------------------
                        if (!bEsTestimonio)
                        {
                            lineaMultiplo = 3;
                        }

                        #region ValidacionTipoActoNotarial

                        if (TipoActoNotarial.Length == 0)
                        {
                            if (_sMinutaEP != "S/N")
                            {
                                TextoPosicionadoITextSharp textoPosicionadoNumero = new TextoPosicionadoITextSharp();
                                textoPosicionadoNumero.FontSize = _fCuerpoFontSize;
                                textoPosicionadoNumero.BaseFont = CuerpoBaseFont;
                                textoPosicionadoNumero.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                textoPosicionadoNumero.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                textoPosicionadoNumero.FXPosition = fMargenIzquierdaDoc;
                                textoPosicionadoNumero.TextAligment = HorizontalAlign.Left;
                                textoPosicionadoNumero.Texto = _NumeroEP != string.Empty ? "NÚMERO: " + _NumeroEP : string.Empty;

                                //TextoPosicionadoITextSharp textoPosicionadoMinuta = new TextoPosicionadoITextSharp();
                                //textoPosicionadoMinuta.FontSize = _fCuerpoFontSize;
                                //textoPosicionadoMinuta.BaseFont = CuerpoBaseFont;
                                //textoPosicionadoMinuta.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                //textoPosicionadoMinuta.FXPosition = fMargenIzquierdaDoc;

                                //textoPosicionadoMinuta.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                //textoPosicionadoMinuta.TextAligment = HorizontalAlign.Right;
                                //textoPosicionadoMinuta.Texto = _sMinutaEP != string.Empty ? "MINUTA: " + _sMinutaEP : string.Empty;

                                _listTextoPosicionadoiTextSharp.Add(textoPosicionadoNumero);

                               // _listTextoPosicionadoiTextSharp.Add(textoPosicionadoMinuta);
                            }
                        }
                        else
                        {

                           
                            //-----------------------------------------------------------
                            //Fecha:16/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Establecer la letra en negrita para testimonio
                            //-----------------------------------------------------------                           

                            TextoPosicionadoITextSharp textoPosicionTipoEscrituraPublica = new TextoPosicionadoITextSharp();
                            textoPosicionTipoEscrituraPublica.FontSize = _fCuerpoFontSize;
                            textoPosicionTipoEscrituraPublica.BaseFont = CuerpoBaseFont;
                            textoPosicionTipoEscrituraPublica.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionTipoEscrituraPublica.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionTipoEscrituraPublica.FXPosition = fMargenIzquierdaDoc;
                            textoPosicionTipoEscrituraPublica.TextAligment = HorizontalAlign.Left;
                            textoPosicionTipoEscrituraPublica.Texto = _TipoActoNotarial != string.Empty ? "ESCRITURA PÚBLICA DE:" : string.Empty;
                            _listTextoPosicionadoiTextSharp.Add(textoPosicionTipoEscrituraPublica);

                            #region Existe_TipoActoNotarial
                            if (_TipoActoNotarial.Length > 0)
                            {
                                textoPosicionTipoEscrituraPublica = new TextoPosicionadoITextSharp();
                                textoPosicionTipoEscrituraPublica.FontSize = _fCuerpoFontSize;
                                if (bEsTestimonio)
                                {
                                    textoPosicionTipoEscrituraPublica.BaseFont = TituloBaseFont;
                                }
                                else
                                {
                                    textoPosicionTipoEscrituraPublica.BaseFont = CuerpoBaseFont;
                                }
                                textoPosicionTipoEscrituraPublica.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                textoPosicionTipoEscrituraPublica.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                textoPosicionTipoEscrituraPublica.FXPosition = fMargenIzquierdaDoc + 170;
                                textoPosicionTipoEscrituraPublica.TextAligment = HorizontalAlign.NotSet;
                                textoPosicionTipoEscrituraPublica.Texto = _TipoActoNotarial;
                                _listTextoPosicionadoiTextSharp.Add(textoPosicionTipoEscrituraPublica);
                            }
                            #endregion

                            lineaMultiplo++;
                        }
                        #endregion

                        lineaMultiplo++;


                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        #region Lista_Otorgantes


                        if (_listOtorgantesEC.Count > 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                            textoPosicionado.FontSize = _fCuerpoFontSize;
                            textoPosicionado.BaseFont = CuerpoBaseFont;
                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                            if (TipoActoNotarial.Length == 0)
                            {
                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                if (_listOtorgantesEC.Count == 1)
                                    textoPosicionado.Texto = "QUE OTORGA:";
                                else
                                    textoPosicionado.Texto = "QUE OTORGAN:";

                            }
                            else
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Indicar el subtitulo "Otorgado por:".
                                //-------------------------------------------------------------

                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                textoPosicionado.Texto = "OTORGADO POR:";
                            }

                            if (TipoActoNotarial.Length == 0)
                            { lineaMultiplo++; }

                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                            #region Si_No_Existe_ActoNotarial

                            if (TipoActoNotarial.Length == 0)
                            {
                                for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantesEC.Count; i++)
                                {
                                    string otorgante = _listOtorgantesEC[i].Trim();
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;
                                    textoPosicionado.BaseFont = bfontCourier;
                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);

                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                    textoPosicionado.Texto = otorgante;

                                    lineaMultiplo++;
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                            }
                            #endregion

                            #region Si_Existe_ActoNotarial_Otorgantes

                            if (TipoActoNotarial.Length > 0)
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                //-------------------------------------------------------------      

                                string strcadOtorgantes = "";
                                List<string> strArrayOtorgantes = new List<string>();
                                int intPar = 0;
                                string strCadenaIzquierda = "";
                                int intAnchoCadenaNombres = 43;
                                int intUltimoEspacio = 0;

                                #region ForOtorgantes
                                for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantesEC.Count; i++)
                                {
                                    intPar++;
                                    string otorgante = _listOtorgantesEC[i].Trim();
                                    if (_listOtorgantesEC.Count > 2)
                                    {
                                        if (i + 2 < _listOtorgantesEC.Count)
                                        {
                                            strcadOtorgantes = strcadOtorgantes + otorgante + ", ";
                                        }
                                        else
                                        {
                                            strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                        }
                                    }
                                    else
                                    {
                                        strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                    }
                                    if (intPar % 2 == 0)
                                    {
                                        if (strcadOtorgantes.Length > intAnchoCadenaNombres)
                                        {
                                            intUltimoEspacio = strcadOtorgantes.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                            strCadenaIzquierda = strcadOtorgantes.Substring(0, intUltimoEspacio);
                                            strArrayOtorgantes.Add(strCadenaIzquierda);
                                            strcadOtorgantes = strcadOtorgantes.Substring(intUltimoEspacio + 1);
                                        }
                                        else
                                        {
                                            strArrayOtorgantes.Add(strcadOtorgantes);
                                            strcadOtorgantes = "";
                                        }
                                    }
                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                                #endregion

                                if (_listOtorgantesEC.Count > 2)
                                {
                                    strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 3);
                                }
                                else
                                {
                                    strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 2);
                                }
                                if (strcadOtorgantes.Length > intAnchoCadenaNombres)
                                {
                                    intUltimoEspacio = strcadOtorgantes.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                    strCadenaIzquierda = strcadOtorgantes.Substring(0, intUltimoEspacio);
                                    strArrayOtorgantes.Add(strCadenaIzquierda);
                                    strcadOtorgantes = strcadOtorgantes.Substring(intUltimoEspacio + 1);

                                    strArrayOtorgantes.Add(strcadOtorgantes);
                                }
                                else
                                {
                                    strArrayOtorgantes.Add(strcadOtorgantes);
                                }

                                strcadOtorgantes = "";

                                #region ForOtorgantes
                                for (int i = 0; i < strArrayOtorgantes.Count; i++)
                                {
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;

                                    //-----------------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Establecer la letra en negrita para testimonio
                                    //-----------------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        textoPosicionado.BaseFont = TituloBaseFont;
                                    }
                                    else
                                    {
                                        textoPosicionado.BaseFont = CuerpoBaseFont;
                                    }


                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                    textoPosicionado.TextAligment = HorizontalAlign.NotSet;
                                    textoPosicionado.Texto = strArrayOtorgantes[i].ToString();
                                    textoPosicionado.FXPosition = fMargenIzquierdaDoc + 100;
                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                    lineaMultiplo++;
                                }
                                #endregion

                                strArrayOtorgantes.Clear();
                            }
                            #endregion

                        }

                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        lineaMultiplo++;

                        #region Si_Existe_ListaApoderados

                        if (_listApoderadosEC.Count > 0)
                        {
                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                            textoPosicionado.FontSize = _fCuerpoFontSize;
                            textoPosicionado.BaseFont = CuerpoBaseFont;
                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                            textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                            if (TipoActoNotarial.Length == 0)
                            {
                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                            }
                            else
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda el texto: "a favor de".
                                //-------------------------------------------------------------

                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                            }
                            textoPosicionado.Texto = "A FAVOR DE:";

                            //---------------------------------------
                            //Fecha: 27/02/2017
                            //Autor: Miguel Márquez Beltrán
                            //Objetivo: Solo si no es testimonio aumentar una linea
                            //      si no en la misma linea.
                            //---------------------------------------
                            if (!bEsTestimonio)
                            {
                                lineaMultiplo++;
                            }

                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                            #region Si_No_Existe_TipoActoNotarial
                            if (TipoActoNotarial.Length == 0)
                            {
                                for (int i = (_iPageNumber - 1) * 10; i < _listApoderadosEC.Count; i++)
                                {
                                    string apoderado = _listApoderadosEC[i].Trim();

                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;
                                    textoPosicionado.BaseFont = bfontCourier;
                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                    textoPosicionado.Texto = apoderado;

                                    lineaMultiplo++;
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);


                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                            }
                            #endregion

                            #region Si_Existe_TipoActoNotarial
                            if (TipoActoNotarial.Length > 0)
                            {
                                //-------------------------------------------------------------
                                //Fecha: 10/02/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Alinear a la izquierda a los apoderados.
                                //-------------------------------------------------------------
                                string strcadApoderados = "";
                                List<string> strArrayApoderados = new List<string>();
                                int intPar = 0;
                                string strCadenaIzquierda = "";
                                int intAnchoCadenaNombres = 43;
                                int intUltimoEspacio = 0;

                                #region ForApoderados
                                for (int i = (_iPageNumber - 1) * 10; i < _listApoderadosEC.Count; i++)
                                {
                                    intPar++;
                                    string apoderado = _listApoderadosEC[i].Trim();
                                    if (_listApoderadosEC.Count > 2)
                                    {
                                        if (i + 2 < _listApoderadosEC.Count)
                                        {
                                            strcadApoderados = strcadApoderados + apoderado + ", ";
                                        }
                                        else
                                        {
                                            strcadApoderados = strcadApoderados + apoderado + " Y ";
                                        }
                                    }
                                    else
                                    {
                                        strcadApoderados = strcadApoderados + apoderado + " Y ";
                                    }
                                    if (intPar % 2 == 0)
                                    {
                                        if (strcadApoderados.Length > intAnchoCadenaNombres)
                                        {
                                            intUltimoEspacio = strcadApoderados.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                            strCadenaIzquierda = strcadApoderados.Substring(0, intUltimoEspacio);
                                            strArrayApoderados.Add(strCadenaIzquierda);
                                            strcadApoderados = strcadApoderados.Substring(intUltimoEspacio + 1);
                                        }
                                        else
                                        {
                                            strArrayApoderados.Add(strcadApoderados);
                                            strcadApoderados = "";
                                        }
                                    }
                                    if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                        break;
                                }
                                #endregion
                                if (_listApoderadosEC.Count > 2)
                                {
                                    strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 3);
                                }
                                else
                                {
                                    strcadApoderados = strcadApoderados.Substring(0, strcadApoderados.Length - 2);
                                }
                                if (strcadApoderados.Length > intAnchoCadenaNombres)
                                {
                                    intUltimoEspacio = strcadApoderados.Substring(0, intAnchoCadenaNombres).LastIndexOf(" ");
                                    strCadenaIzquierda = strcadApoderados.Substring(0, intUltimoEspacio);
                                    strArrayApoderados.Add(strCadenaIzquierda);
                                    strcadApoderados = strcadApoderados.Substring(intUltimoEspacio + 1);

                                    strArrayApoderados.Add(strcadApoderados);
                                }
                                else
                                {
                                    strArrayApoderados.Add(strcadApoderados);
                                }

                                strcadApoderados = "";

                                #region Apoderados

                                for (int i = 0; i < strArrayApoderados.Count; i++)
                                {
                                    textoPosicionado = new TextoPosicionadoITextSharp();
                                    textoPosicionado.FontSize = _fCuerpoFontSize;
                                    //-----------------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Establecer la letra en negrita para testimonio
                                    //-----------------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        textoPosicionado.BaseFont = TituloBaseFont;
                                    }
                                    else
                                    {
                                        textoPosicionado.BaseFont = CuerpoBaseFont;
                                    }

                                    textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                    textoPosicionado.TextAligment = HorizontalAlign.NotSet;
                                    textoPosicionado.Texto = strArrayApoderados[i].ToString();
                                    textoPosicionado.FXPosition = fMargenIzquierdaDoc + 100;
                                    textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                    lineaMultiplo++;
                                }
                                #endregion
                                strArrayApoderados.Clear();
                            }
                            #endregion

                        }
                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                        if (TipoActoNotarial.Length == 0)
                        { lineaMultiplo++; }

                        #region LineaDivisora

                        TextoPosicionadoITextSharp textoPosicionadoITS = new TextoPosicionadoITextSharp();
                        textoPosicionadoITS.FontSize = _fCuerpoFontSize;
                        textoPosicionadoITS.BaseFont = CuerpoBaseFont;
                        textoPosicionadoITS.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                        textoPosicionadoITS.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                        textoPosicionadoITS.FXPosition = fMargenIzquierdaDoc;
                        //textoPosicionadoITS.FXPosition = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;
                        textoPosicionadoITS.TextAligment = HorizontalAlign.Center;

                        string strraya = new string('_', 67);
                        textoPosicionadoITS.Texto = strraya;
                        //textoPosicionadoITS.Texto = "___________________________________________________________________";                        
                        lineaMultiplo++;
                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoITS);

                        #endregion

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();

                    }
                    #endregion

                    iContadorLineas = 0;

                    #region Cabecera Parte : Fecha
                    if (bEsParte)
                    {
                        iContadorLineas = 1;

                        TextoPosicionadoITextSharp textoPosicionadoFecha = new TextoPosicionadoITextSharp();
                        textoPosicionadoFecha.FontSize = _fCuerpoFontSize;
                        textoPosicionadoFecha.BaseFont = CuerpoBaseFont;
                        textoPosicionadoFecha.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);

                        textoPosicionadoFecha.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                        textoPosicionadoFecha.TextAligment = HorizontalAlign.Right;
                        textoPosicionadoFecha.Texto = sFecha;

                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoFecha);

                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                        _listTextoPosicionadoiTextSharp.Clear();
                    }
                    #endregion

                    #region Cuerpo Documento


                    MemoryStream msEC = new MemoryStream();
                    StreamWriter strEC = new StreamWriter(msEC, Encoding.Default);
                    strEC.Write("                    " + _sCuerpoHtmlEC);
                    strEC.Flush();

                    msEC.Position = 0;

                    StreamReader oStreamReaderEC = new StreamReader(msEC, System.Text.Encoding.Default);
                    iTextSharp.text.html.simpleparser.StyleSheet stylesEC = new iTextSharp.text.html.simpleparser.StyleSheet();

                    List<iTextSharp.text.IElement> objectsEC;
                    iTextSharp.text.Paragraph oParagraphEC = null;
                    objectsEC = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReaderEC, stylesEC);
                    fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                    fPosicion = fMargenSuperiorDoc;
                    fTextSize = 0;
                    
                    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);

                    while ((int)fDocumentPosition + (_fCuerpoFontSize * FCuerpoLeading) < ((int)writer.GetVerticalPosition(false)))
                    {
                        oParagraphEC = new iTextSharp.text.Paragraph();
                        oParagraphEC.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                        oParagraphEC.SetLeading(0.0f, 1.42f);
                        document.Add(oParagraphEC);
                    }

                    _iPageNumber = 1;


                    bExistImage = false;
                    bool bEsNombreOtorgante = false;
                   

                    for (int k = 0; k < objectsEC.Count; k++)
                    {

                        oIElement = (iTextSharp.text.IElement)objectsEC[k];

                        bool bBorrarParagraph = true;

                        if (((iTextSharp.text.IElement)objectsEC[k]).Chunks.Count == 0)
                        {
                            oParagraphEC.Clear();
                            oParagraphEC.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                            bBorrarParagraph = false;
                        }



                        if (objectsEC[k].GetType().FullName == "iTextSharp.text.Paragraph")
                        {
                            if (bBorrarParagraph)
                            {
                                oParagraphEC = new iTextSharp.text.Paragraph();
                                oParagraphEC.Alignment = ((iTextSharp.text.Paragraph)objectsEC[k]).Alignment;
                                oParagraphEC.Font = _fontGeneral;
                                //-------------------------------------------------------
                                //Fecha: 28/03/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Poner la primera linea del texto de 
                                //          Escritura Pública en negrita.
                                //-------------------------------------------------------
                                if (bEsEscrituraPublica)
                                {
                                    if (k == 0)
                                    { oParagraphEC.Font = _fontGeneralB; }
                                }
                                //-------------------------------------------------------
                            }
                            bBorrarParagraph = true;

                            cb = writer.DirectContent;
                            iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);

                            #region Chunks

                            for (int z = 0; z < oIElement.Chunks.Count; z++)
                            {

                                if (bEsParte)
                                    strContent = oIElement.Chunks[z].Content.ToString();
                                else
                                {

                                    //-----------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Si es testimonio no convertir a mayusculas.
                                    //-----------------------------------------------------
                                    if (bEsTestimonio)
                                    {
                                        strContent = oIElement.Chunks[z].Content.ToString();
                                    }
                                    else
                                    {
                                        strContent = oIElement.Chunks[z].Content.ToString().ToUpper();
                                    }

                                }


                                if (strContent.Contains("#IMAGEN#"))
                                {
                                    strContent = strContent.Replace("#IMAGEN#", "");
                                    bExistImage = true;
                                }



                                while (strContent.Contains(" .") || strContent.Contains(" ,") ||
                                    strContent.Contains(" ;") || strContent.Contains(" :"))
                                {
                                    strContent = strContent.Replace(" .", ".").Replace(" ,", ",").Replace(" :", ":").Replace(" ;", ";");
                                }

                                if (!bAplicaCierreTextoNotarial)
                                {
                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                }
                                else
                                {
                                    #region Aplica cierre Texto Notarial, es decir, "============"

                                    if (strContent != "\n")
                                    {
                                        strContent = strContent.Trim();
                                        if (z > 1)
                                        {
                                            if (oIElement.Chunks[z - 1].Content.ToString().IndexOf("PODERDANTES DE NOMBRES") > -1)
                                            {
                                                oParagraphEC.Add(new iTextSharp.text.Chunk(strContent, _fontGeneralB));
                                            }
                                            else
                                            {
                                                bool bExisteOtorgante = false;

                                                for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                {
                                                    if (strContent.Contains(_listOtorgantesEC[i].ToString()))
                                                    {
                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strContent, _fontGeneralB));
                                                        bExisteOtorgante = true;
                                                        break;
                                                    }
                                                }
                                                if (bExisteOtorgante == false)
                                                {
                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        }
                                    }
                                    else
                                    {
                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));
                                        continue;
                                    }

                                    if (z == oIElement.Chunks.Count - 1)
                                    {
                                        List<string> listTextos = new List<string>();
                                        List<iTextSharp.text.Font> listFonts = new List<iTextSharp.text.Font>();
                                        string textoNotarialCierre = string.Empty;
                                        int intNumeroElementos = 0;
                                        string strContenido = string.Empty;

                                        foreach (iTextSharp.text.Chunk ch in oIElement.Chunks)
                                        {
                                            if (ch.Content.Contains("#IMAGEN#"))
                                            {
                                                strContenido = string.Empty;
                                            }
                                            strContenido = ch.Content.Replace("#IMAGEN#", "");


                                            if (strContenido == string.Empty)
                                                continue;

                                            if (strContenido != "\n")
                                                listTextos.Add(strContenido.Trim());
                                            else
                                                listTextos.Add("\n");

                                            ch.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                                            listFonts.Add(ch.Font);
                                            intNumeroElementos = intNumeroElementos + 1;
                                        }


                                        bool bEscribirTextoCierro = true;

                                        if (_listPalabrasOmitirTextoNotarial != null)
                                        {

                                            foreach (string palabra in _listPalabrasOmitirTextoNotarial)
                                            {
                                                if (strContent.Trim() != string.Empty && strContent.Trim().Contains(palabra))
                                                {
                                                    bEscribirTextoCierro = false;
                                                    break;
                                                }

                                            }
                                        }

                                        if (bEscribirTextoCierro)
                                        {
                                            if (listTextos.Count > 0)
                                                textoNotarialCierre = ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);
                                        }


                                        if (textoNotarialCierre != string.Empty)
                                        {
                                            iTextSharp.text.Font font = new iTextSharp.text.Font(oIElement.Chunks[z].Font);
                                            font.SetStyle(0);
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));
                                        }

                                    }
                                    else
                                    {
                                        oParagraphEC.Add(new iTextSharp.text.Chunk(" ", oIElement.Chunks[z].Font));
                                    }

                                    #endregion
                                }

                            }


                            bool bBorrar = false;
                            for (int i = oParagraphEC.Chunks.Count - 1; i >= 0; i--)
                            {
                                if (oParagraphEC.Chunks[i].Content == string.Empty)
                                    continue;

                                if (!bBorrar)
                                {
                                    if (oParagraphEC.Chunks[i].Content[0] == '.' || oParagraphEC.Chunks[i].Content[0] == ',' ||
                                        oParagraphEC.Chunks[i].Content[0] == ':' || oParagraphEC.Chunks[i].Content[0] == ';')
                                    {
                                        bBorrar = true;
                                    }
                                }
                                else
                                {
                                    oParagraphEC.RemoveAt(i);
                                    bBorrar = false;
                                }

                            }

                            #endregion

                            #region LineCheck
                        // QUE ES?
                        LineCheck:

                            float fLineaWidth = 0;
                            float fPalabraWidth = 0;
                            string sLineaAcumulada = string.Empty;
                            List<string> listLineas = new List<string>();

                            #region Parrafo

                            foreach (iTextSharp.text.Chunk par in oParagraphEC.Chunks)
                            {
                                if (par.Content == "\n")
                                {
                                    listLineas.Add("\n");
                                    continue;
                                }

                                if (!bEsParte)
                                {
                                    //-----------------------------------------------------
                                    //Fecha:16/02/2017
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Si es testimonio no convertir a mayusculas.
                                    //-----------------------------------------------------         
                                    if (bEsTestimonio)
                                    {
                                        #region esTestimonio
                                        foreach (string palabra in par.Content.ToUpper().Trim().Split(' '))
                                        {
                                            fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                            if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                            {
                                                if (sLineaAcumulada.Trim() != string.Empty)
                                                {
                                                    listLineas.Add(sLineaAcumulada.Trim());
                                                }

                                                else
                                                    listLineas.Add("\n");
                                                fLineaWidth = 0;
                                                sLineaAcumulada = string.Empty;
                                            }

                                            if (palabra.Trim() != string.Empty)
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                                fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //-----------------------------------------------------
                                        // Fecha: 18/04/2017
                                        // Autor: Miguel Márquez Beltrán
                                        // Objetivo: Poner en negrita el texto central.
                                        //-----------------------------------------------------                                                                                
                                        bool bEstablecerNegritaOtorgante = false;

                                        if (bEsNombreOtorgante)
                                        { bEstablecerNegritaOtorgante = true; }
                                       
                                        //=================================================================
                                        foreach (string palabra in par.Content.ToUpper().Trim().Split(' '))
                                        {
                                            //------------------------------------------------------------------
                                            //Fecha: 29/03/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Poner en negrita la palabra: CONCLUSIÓN:
                                            //------------------------------------------------------------------

                                            if (bEsEscrituraPublica && palabra.Equals("CONCLUSIÓN:"))
                                            { fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneralB).GetWidthPoint(); }
                                            else
                                            {
                                               
                                                if (bEsEscrituraPublica && palabra.IndexOf("COMPARECE:") > -1)
                                                { bEsNombreOtorgante = true; }

                                                if (bEstablecerNegritaOtorgante)
                                                { fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneralB).GetWidthPoint(); }
                                                else
                                                {
                                                    fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();
                                                }
                                                
                                            }

                                            #region Linea Acumulada
                                            //=======================================                                                                                                                                   
                                            if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                            {
                                                if (sLineaAcumulada.Trim() != string.Empty)
                                                { listLineas.Add(sLineaAcumulada.Trim()); }

                                                else
                                                    listLineas.Add("\n");
                                                fLineaWidth = 0;
                                                sLineaAcumulada = string.Empty;
                                            }
                                            #endregion
                                            //=======================================

                                            if (palabra.Trim() != string.Empty)
                                            {
                                                sLineaAcumulada += palabra.Trim() + " ";
                                                //=======================================
                                                if (bEstablecerNegritaOtorgante)
                                                {
                                                    fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneralB).GetWidthPoint();
                                                    if (palabra.IndexOf("NACIONALIDAD") > -1)
                                                    {
                                                        bEsNombreOtorgante = false;
                                                        bEstablecerNegritaOtorgante = false;
                                                    }
                                                }
                                                else
                                                {
                                                     fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint(); 
                                                }
                                                //================================
                                            }
                                            //=======================================
                                        }
                                        //=================================================================Fin foreach


                                    }
                                }
                                else
                                {
                                    #region Parte

                                    foreach (string palabra in par.Content.Trim().Split(' '))
                                    {
                                        fPalabraWidth = new iTextSharp.text.Chunk(palabra, _fontGeneral).GetWidthPoint();

                                        if (fLineaWidth + fPalabraWidth > document.PageSize.Width - (_fMarginLeft + _fMarginRight))
                                        {
                                            if (sLineaAcumulada.Trim() != string.Empty)
                                            {
                                                listLineas.Add(sLineaAcumulada.Trim());
                                            }

                                            else
                                                listLineas.Add("\n");
                                            fLineaWidth = 0;
                                            sLineaAcumulada = string.Empty;
                                        }

                                        if (palabra.Trim() != string.Empty)
                                        {
                                            sLineaAcumulada += palabra.Trim() + " ";
                                            fLineaWidth += fPalabraWidth + new iTextSharp.text.Chunk(" ", _fontGeneral).GetWidthPoint();

                                        }
                                    }
                                    #endregion
                                }
                            }
                            //======================================================Fin foreach
                            #endregion

                            if (sLineaAcumulada.Trim() != string.Empty)
                            {
                                listLineas.Add(sLineaAcumulada.Trim());
                            }
                            else
                                listLineas.Add("\n");


                            #endregion


                            oParagraphEC.SetLeading(0.0f, FCuerpoLeading);

                            iContadorLineas += listLineas.Count;


                            if (iContadorLineas >= _iLineNumberGeneral)
                            {
                                #region ContadorLineas

                                int iLineasSobrantes = iContadorLineas - _iLineNumberGeneral;
                                int iUltimaLineaPrimerTrozo = listLineas.Count - iLineasSobrantes;
                                string lineasPrimeraTrozo = string.Empty;
                                string lineasSegundaTrozo = string.Empty;


                                listLineas[iUltimaLineaPrimerTrozo - 1] = ObtenerLineaJustificada(
                                    new iTextSharp.text.Chunk(listLineas[iUltimaLineaPrimerTrozo - 1],
                                        _fontGeneral),
                                        document.PageSize.Width - (_fMarginLeft + _fMarginRight));

                                for (int i = 0; i < listLineas.Count; i++)
                                {
                                    if (i < iUltimaLineaPrimerTrozo)
                                    {
                                        lineasPrimeraTrozo += listLineas[i] + " ";
                                    }
                                    else
                                    {
                                        lineasSegundaTrozo += listLineas[i] + " ";
                                    }
                                }


                                bool bNuevaPagina = true;
                                if (writer.GetVerticalPosition(false) != 742)
                                    bNuevaPagina = false;

                                oParagraphEC.Clear();

                                //--------------------------------------------------
                                //Fecha: 29/03/2017
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Poner en negrita la palabra: CONCLUSIÓN
                                //--------------------------------------------------
                                if (bEsEscrituraPublica && lineasPrimeraTrozo.IndexOf("CONCLUSIÓN:") > -1)
                                {
                                    oParagraphEC.Add(new iTextSharp.text.Chunk("CONCLUSIÓN:", _fontGeneralB));
                                    lineasPrimeraTrozo = lineasPrimeraTrozo.Replace("CONCLUSIÓN:", "");

                                    //if (lineasPrimeraTrozo.IndexOf(NombreFuncionario) > -1)
                                    //{
                                    //    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                    //}

                                    //-------------------------------------------------------------
                                    //Fecha: 26/02/2020
                                    //Autor: Miguel Márquez Beltrán
                                    //Objetivo: Poner en negrita el nombre dle interprete.
                                    //-------------------------------------------------------------
                                    if (lineasPrimeraTrozo.IndexOf("INTÉRPRETE,") > -1)
                                    {
                                        string strinterprete = "";
                                        for (int i = 0; i < _listInterpretesEC.Count; i++)
                                        {
                                            strinterprete = _listInterpretesEC[i].Trim() + ", ";
                                            if (lineasPrimeraTrozo.Contains(strinterprete))
                                            {
                                                lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strinterprete, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                            }
                                        }
                                    }

                                    
                                    //------------------------------------------------
                                }
                                //--------------------------------------------------

                                string strQueConsteEl = "QUE CONSTE";

                                if (bEsEscrituraPublica && lineasPrimeraTrozo.IndexOf(strQueConsteEl) > -1)
                                {
                                    int intIndiceInicialQueConsteEl = lineasPrimeraTrozo.IndexOf(strQueConsteEl);
                                    int intIndiceFinalQueConsteEl = intIndiceInicialQueConsteEl + strQueConsteEl.Length;
                                    string strQueOtorga = "";

                                    strQueOtorga = "QUE";
                                    
                                    string strCadena = lineasPrimeraTrozo.Substring(intIndiceFinalQueConsteEl);
                                    int intIndiceInicialQueOtorga = strCadena.IndexOf(strQueOtorga);

                                    string strTipoActoNotarial = strCadena.Substring(0, intIndiceInicialQueOtorga);

                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strTipoActoNotarial, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);

                                    //string strOtorgante = "";

                                    //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                    //{
                                    //    strOtorgante = _listOtorgantesEC[i].Trim() + ", ";
                                    //    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                    //}
                                }

                                

                                int intInicioArticulo = lineasPrimeraTrozo.IndexOf("INSERTO:");
                                int intFinArticulo = lineasPrimeraTrozo.IndexOf(".-");

                                if (bEsEscrituraPublica && intInicioArticulo > -1 && intFinArticulo > -1)
                                {
                                    string strNombreArticulo = lineasPrimeraTrozo.Substring(intInicioArticulo, intFinArticulo);
                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombreArticulo, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                }
                               
                                
                                
                                if (bEsEscrituraPublica && lineasPrimeraTrozo.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1)
                                {
                                    if (lineasPrimeraTrozo.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1)
                                    {
                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita("FIRMA E IMPRESIÓN DACTILAR:", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                    }
                                    if (lineasPrimeraTrozo.IndexOf("IMPRESIÓN DACTILAR:") > -1)
                                    {
                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita("IMPRESIÓN DACTILAR:", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                    }


                                    string strOtorgante = "";
                                    //---------------------------------------------
                                    for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                    {
                                        strOtorgante = _listOtorgantesEC[i].Trim();

                                        if (lineasPrimeraTrozo.IndexOf(strOtorgante) > -1)
                                        {
                                            lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                        }
                                    }

                                    string[] strNombres = null;

                                    if (lineasPrimeraTrozo.Length > 0)
                                    {
                                        //---------------------------------------------
                                        for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                        {
                                            strOtorgante = _listOtorgantesEC[i].Trim();
                                            strNombres = strOtorgante.Split(' ');

                                            for (int x = 0; x < strNombres.Length; x++)
                                            {
                                                if (lineasPrimeraTrozo.Contains(strNombres[x] + "  "))
                                                {
                                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + "  ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                }
                                                else
                                                {
                                                    if (lineasPrimeraTrozo.Contains(strNombres[x] + " "))
                                                    {
                                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + " ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    string strInterprete = "";

                                    //---------------------------------------------------
                                    for (int i = 0; i < _listInterpretesEC.Count; i++)
                                    {
                                        strInterprete = _listInterpretesEC[i].Trim();

                                        if (lineasPrimeraTrozo.IndexOf(strInterprete) > -1)
                                        {
                                            lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strInterprete, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                        }
                                    }
                                    if (lineasPrimeraTrozo.Length > 0)
                                    {
                                        for (int i = 0; i < _listInterpretesEC.Count; i++)
                                        {
                                            strInterprete = _listInterpretesEC[i].Trim();
                                            strNombres = strInterprete.Split(' ');

                                            for (int x = 0; x < strNombres.Length; x++)
                                            {
                                                if (lineasPrimeraTrozo.Contains(strNombres[x] + "  "))
                                                {
                                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + "  ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                }
                                                else
                                                {
                                                    if (lineasPrimeraTrozo.Contains(strNombres[x] + " "))
                                                    {
                                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + " ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //---------------------------------------------------
                

                                    string strTestigo = "";

                                    //---------------------------------------------------
                                    for (int i = 0; i < _listTestigosEC.Count; i++)
                                    {
                                        strTestigo = _listTestigosEC[i].Trim();

                                        if (lineasPrimeraTrozo.IndexOf(strTestigo) > -1)
                                        {
                                            lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strTestigo, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                        }
                                    }
                                    if (lineasPrimeraTrozo.Length > 0)
                                    {
                                        for (int i = 0; i < _listTestigosEC.Count; i++)
                                        {
                                            strTestigo = _listTestigosEC[i].Trim();
                                            strNombres = strTestigo.Split(' ');

                                            for (int x = 0; x < strNombres.Length; x++)
                                            {
                                                if (lineasPrimeraTrozo.Contains(strNombres[x] + "  "))
                                                {
                                                    lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + "  ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                }
                                                else
                                                {
                                                    if (lineasPrimeraTrozo.Contains(strNombres[x] + " "))
                                                    {
                                                        lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNombres[x] + " ", lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //---------------------------------------------------
                           

                                    if (iLineasSobrantes == 0)
                                    {
                                        if (lineasPrimeraTrozo.LastIndexOf("==== ") > -1)
                                        {
                                            lineasPrimeraTrozo = lineasPrimeraTrozo.Substring(0, lineasPrimeraTrozo.Length - 5);
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                        }
                                        else
                                        {
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                        }
                                    }
                                    else
                                    {
                                        oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                    }
                                    
                                }
                                else
                                {
                                    string strLibro = "LIBRO";
                                    int intIndiceLibro = lineasPrimeraTrozo.IndexOf(strLibro);

                                    if (intIndiceLibro > -1)
                                    {
                                        string strParte1 = lineasPrimeraTrozo.Substring(0, intIndiceLibro + strLibro.Length);
                                        string strDelRegistro = "DEL REGISTRO";

                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));
                                        lineasPrimeraTrozo = lineasPrimeraTrozo.Replace(strParte1, "");

                                        int intDelRegistro = lineasPrimeraTrozo.IndexOf(strDelRegistro);

                                        if (intDelRegistro > -1)
                                        {
                                            string strNroLibro = lineasPrimeraTrozo.Substring(0, intDelRegistro);

                                            lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(strNroLibro, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                        }

                                        oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                    }
                                    else
                                    {
                                        if (bEsParte)
                                        {
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                        }
                                        else
                                        {

                                        string strDenominara = "DENOMINARÁ ";
                                        string strPoderdante = "PODERDANTE,";

                                        int intIndiceDenominara = lineasPrimeraTrozo.IndexOf(strDenominara);
                                        int intIndicePoderdante = lineasPrimeraTrozo.IndexOf(strPoderdante);

                                        if (bEsEscrituraPublica && intIndiceDenominara > -1 && intIndicePoderdante > -1)
                                        {
                                            
                                            if (lineasPrimeraTrozo.Length > 0)
                                            {
                                                oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                            }
                                        }
                                        else
                                        {
                                            string strAFavorDe = "A FAVOR DE ";
                                            int intAFavorDe = lineasPrimeraTrozo.IndexOf(strAFavorDe);

                                            if (intAFavorDe == 0)
                                            {

                                                if (lineasPrimeraTrozo.Trim().Length > 0)
                                                {
                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                                }
                                             }
                                             else
                                             {

                                                 if (lineasPrimeraTrozo.IndexOf("FIRMA:") > -1 && lineasPrimeraTrozo.IndexOf(NombreFuncionario) > -1)
                                                 {
                                                     lineasPrimeraTrozo = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, lineasPrimeraTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                 }
                                                 if (lineasPrimeraTrozo.Length > 0)
                                                 {
                                                     oParagraphEC.Add(new iTextSharp.text.Chunk(lineasPrimeraTrozo, _fontGeneral));
                                                 }
                                             }
                                          }
                                       }
                                    }                                   
                                }                                

                                //--------------------------------------------------                                

                                document.Add(oParagraphEC);


                                if (bNuevaPagina)
                                { document.NewPage(); }


                                _iLineNumber = 25.7f;
                                _iPageNumber++;

                                lineaMultiplo = 0;

                                if (bEsEscrituraPublica)
                                {
                                    #region Otorgantes y Apoderados

                                    if ((_listOtorgantesEC.Count > (_iPageNumber - 1) * 10) || (_listApoderadosEC.Count > (_iPageNumber - 1) * 10))
                                    {
                                        fDocumentHeight = 842;
                                        fLineaHeight = 12 + 1.5f;


                                        #region Otorgantes


                                        if (_listOtorgantesEC.Count > (_iPageNumber - 1) * 10)
                                        {
                                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                                            textoPosicionado.FontSize = _fCuerpoFontSize;
                                            textoPosicionado.BaseFont = CuerpoBaseFont;
                                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;

                                            if (TipoActoNotarial.Length == 0 && bEsEscrituraPublica)
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                textoPosicionado.Texto = "QUE OTORGA:";
                                            }
                                            else
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Indicar el texto: "Otorgado por".
                                                //-------------------------------------------------------------

                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = "OTORGADO POR:";
                                            }
                                            lineaMultiplo++;

                                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                            string strcadOtorgantes = "";

                                            for (int i = (_iPageNumber - 1) * 10; i < _listOtorgantesEC.Count; i++)
                                            {
                                                string otorgante = _listOtorgantesEC[i].Trim();
                                                textoPosicionado = new TextoPosicionadoITextSharp();
                                                textoPosicionado.FontSize = _fCuerpoFontSize;
                                                textoPosicionado.BaseFont = CuerpoBaseFont;
                                                textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                                //-------------------------------------------------------------
                                                if (bEsTestimonio)
                                                {
                                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                    textoPosicionado.Texto = otorgante;

                                                    lineaMultiplo++;
                                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                                }
                                                else
                                                {
                                                    strcadOtorgantes = strcadOtorgantes + otorgante + " Y ";
                                                }

                                                if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                                    break;
                                            }

                                            if (TipoActoNotarial.Length > 0 && bEsTestimonio)
                                            {
                                                //-------------------------------------------------------------
                                                //Fecha: 10/02/2017
                                                //Autor: Miguel Márquez Beltrán
                                                //Objetivo: Alinear a la izquierda a los otorgantes.
                                                //-------------------------------------------------------------
                                                strcadOtorgantes = strcadOtorgantes.Substring(0, strcadOtorgantes.Length - 3);
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                                textoPosicionado.Texto = strcadOtorgantes;
                                                textoPosicionado.FXPosition = fMargenIzquierdaDoc;
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);
                                                _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                            }
                                        }


                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                        #region Apoderados

                                        if (_listApoderadosEC.Count > (_iPageNumber - 1) * 10)
                                        {
                                            TextoPosicionadoITextSharp textoPosicionado = new TextoPosicionadoITextSharp();
                                            textoPosicionado.FontSize = _fCuerpoFontSize;
                                            textoPosicionado.BaseFont = CuerpoBaseFont;
                                            textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                            textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                            //-------------------------------------------------------------
                                            //Fecha: 10/02/2017
                                            //Autor: Miguel Márquez Beltrán
                                            //Objetivo: Alinear a la izquierda a los apoderados.
                                            //-------------------------------------------------------------

                                            if (bEsEscrituraPublica)
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                textoPosicionado.TextAligment = HorizontalAlign.Left;
                                            }
                                            textoPosicionado.Texto = "A FAVOR DE:";

                                            lineaMultiplo++;

                                            _listTextoPosicionadoiTextSharp.Add(textoPosicionado);

                                            string strcadApoderados = "";

                                            for (int i = (_iPageNumber - 1) * 10; i < _listApoderadosEC.Count; i++)
                                            {
                                                string apoderado = _listApoderadosEC[i].Trim();
                                                textoPosicionado = new TextoPosicionadoITextSharp();
                                                textoPosicionado.FontSize = _fCuerpoFontSize;
                                                textoPosicionado.BaseFont = CuerpoBaseFont;
                                                textoPosicionado.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                                textoPosicionado.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                                if (bEsTestimonio)
                                                {
                                                    textoPosicionado.TextAligment = HorizontalAlign.Center;
                                                    textoPosicionado.Texto = apoderado;

                                                    lineaMultiplo++;
                                                    _listTextoPosicionadoiTextSharp.Add(textoPosicionado);
                                                }
                                                else
                                                {
                                                    strcadApoderados = strcadApoderados + apoderado + " Y ";
                                                }


                                                if (_listTextoPosicionadoiTextSharp.Count >= 13)
                                                    break;
                                            }
                                            

                                        }


                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();

                                        #region LineaDivisora

                                        TextoPosicionadoITextSharp textoPosicionadoITS = new TextoPosicionadoITextSharp();
                                        textoPosicionadoITS.FontSize = _fCuerpoFontSize;
                                        textoPosicionadoITS.BaseFont = CuerpoBaseFont;
                                        textoPosicionadoITS.FontFamily = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize);
                                        textoPosicionadoITS.FYPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * lineaMultiplo;
                                        textoPosicionadoITS.TextAligment = HorizontalAlign.Center;
                                        textoPosicionadoITS.Texto = "________________________________________________________";

                                        lineaMultiplo++;
                                        _listTextoPosicionadoiTextSharp.Add(textoPosicionadoITS);

                                        #endregion

                                        PosicionarTexto(_listTextoPosicionadoiTextSharp, cb, _iPageNumber);
                                        _listTextoPosicionadoiTextSharp.Clear();


                                    }

                                    fDocumentPosition = fDocumentHeight - fMargenSuperiorDoc - fLineaHeight * (lineaMultiplo - 1);


                                    while ((int)fDocumentPosition < ((int)writer.GetVerticalPosition(false)))
                                    {
                                        oParagraphEC.Clear();
                                        oParagraphEC.Add(new iTextSharp.text.Chunk("\n", _fontGeneral));
                                        oParagraphEC.SetLeading(0.0f, 1.42f);
                                        document.Add(oParagraphEC);
                                    }
                                    #endregion
                                }

                                if (lineasSegundaTrozo.Trim() != string.Empty)
                                {
                                    oParagraphEC.Clear();
                                    oParagraphEC.SetLeading(0.0f, FCuerpoLeading);


                                    string strLosPoderdantes = "LOS PODERDANTES, ";
                                    string strAquienEnAdelanteSeLeDenominara = "A QUIEN EN ADELANTE SE LE DENOMINARÁ";
                                    string strElApoderado = "EL APODERADO.";

                                    int intIndiceLosPoderdantes = lineasSegundaTrozo.IndexOf(strLosPoderdantes);
                                    int intIndiceAquienEnAdelanteSeLeDenominara = lineasSegundaTrozo.IndexOf(strAquienEnAdelanteSeLeDenominara);
                                    int intIndiceElApoderado = lineasSegundaTrozo.IndexOf(strElApoderado);

                                    if (intIndiceLosPoderdantes > -1 && intIndiceAquienEnAdelanteSeLeDenominara > -1 && intIndiceElApoderado > -1)
                                    {
                                        //--------------------------------------
                                        //Poner en negrita a los otorgantes
                                        //--------------------------------------
                                        //string strOtorgante = "";

                                        //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                        //{
                                        //    strOtorgante = _listOtorgantesEC[i].Trim() + ", ";
                                        //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                        //}

                                       


                                        oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                    }
                                    else
                                    {

                                        string strFueElDescrito_ = "FUE EL DESCRITO";
                                        int intIndiceInicialFueElDescrito_ = lineasSegundaTrozo.IndexOf(strFueElDescrito_);

                                        string strNombres = "NOMBRE ";
                                        int intIndiceInicialNombre = lineasSegundaTrozo.IndexOf(strNombres);

                                        string strPoderdantesDeNombres = "PODERDANTES DE NOMBRES";
                                        int intIndicePoderdantesDeNombres = lineasSegundaTrozo.IndexOf(strPoderdantesDeNombres);

                                        if (intIndiceInicialNombre == -1 && intIndiceInicialFueElDescrito_ > -1 && intIndicePoderdantesDeNombres == -1)
                                        {
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                        }
                                        else
                                        {
                                            if (intIndiceInicialNombre > -1 && intIndiceInicialFueElDescrito_ > -1)
                                            {
                                                oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                            }
                                            else
                                            {
                                                string strAQuienEnAdelanteSeLeDenominaran = "A QUIEN EN ADELANTE SE LES DENOMINARÁN";
                                                int intIndiceInicialAQuienEnAdelanteSeLeDenominaran = lineasSegundaTrozo.IndexOf(strAQuienEnAdelanteSeLeDenominaran);
                                                string strQueOtorga = "QUE OTORGA";
                                                int intQueOtorga = lineasSegundaTrozo.IndexOf(strQueOtorga);

                                                if (intIndiceInicialAQuienEnAdelanteSeLeDenominaran > -1 && intQueOtorga > -1)
                                                {
                                                    string strQueConsteEl_Tipo = "QUE CONSTE";

                                                    string strParte0 = lineasSegundaTrozo.Substring(0, intQueOtorga);

                                                    string strTipoEscritura = strParte0.Substring(strQueConsteEl_Tipo.Length);

                                                    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strTipoEscritura, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);


                                                    intIndiceInicialAQuienEnAdelanteSeLeDenominaran = lineasSegundaTrozo.IndexOf(strAQuienEnAdelanteSeLeDenominaran);

                                                    string strParte1 = lineasSegundaTrozo.Substring(0, intIndiceInicialAQuienEnAdelanteSeLeDenominaran + strAQuienEnAdelanteSeLeDenominaran.Length);
                                                                                                        
                                                    //--------------------------------------
                                                    //Poner en negrita a los otorgantes
                                                    //--------------------------------------
                                                    //string strOtorgante = "";

                                                    //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                    //{
                                                    //    strOtorgante = _listOtorgantesEC[i].Trim() + ", ";
                                                    //    strParte1 = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, strParte1, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                    //}

                                                    //--------------------------------------
                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));

                                                    string strParte2 = lineasSegundaTrozo.Substring(intIndiceInicialAQuienEnAdelanteSeLeDenominaran + strAQuienEnAdelanteSeLeDenominaran.Length);
                                                    int intIndicePunto = strParte2.IndexOf(".");
                                                    string strApoderados = strParte2.Substring(0, intIndicePunto);
                                                    string strParte3 = strParte2.Substring(intIndicePunto);

                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strApoderados, _fontGeneralB));
                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strParte3, _fontGeneral));
                                                }
                                                else
                                                {
                                                    string strDelLibro = "DEL LIBRO";
                                                    int intDelLibro = lineasSegundaTrozo.IndexOf(strDelLibro);

                                                    string strDelRegistro = "DEL REGISTRO";
                                                    int intDelRegistro = lineasSegundaTrozo.IndexOf(strDelRegistro);

                                                    if (lineasSegundaTrozo.IndexOf("INSTRUMENTOS") > -1 && intDelLibro > -1 && intDelRegistro > -1)
                                                    {

                                                        string strParte1 = lineasSegundaTrozo.Substring(0, intDelLibro + strDelLibro.Length);
                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));
                                                        lineasSegundaTrozo = lineasSegundaTrozo.Replace(strParte1, "");

                                                        intDelRegistro = lineasSegundaTrozo.IndexOf(strDelRegistro);
                                                        string strNroLibro = lineasSegundaTrozo.Substring(0, intDelRegistro);
                                                        string strParte2 = lineasSegundaTrozo.Substring(intDelRegistro);

                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strNroLibro, _fontGeneralB));
                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strParte2, _fontGeneral));
                                                    }
                                                    else
                                                    {
                                                        string strDeLoQueDoyFe = "DE LO QUE DOY FE.";
                                                        int intIndiceInicialDeLoQueDoyFe = lineasSegundaTrozo.IndexOf(strDeLoQueDoyFe);

                                                        string strElDia = "EL DÍA";
                                                        int intIndiceInicialElDia = lineasSegundaTrozo.IndexOf(strElDia);

                                                        if (intIndiceInicialDeLoQueDoyFe > -1 && intIndiceInicialElDia > -1)
                                                        {
                                                            intIndiceInicialElDia = intIndiceInicialElDia + strElDia.Length;

                                                            string strDiaMesAnio = lineasSegundaTrozo.Substring(intIndiceInicialElDia, intIndiceInicialDeLoQueDoyFe - intIndiceInicialElDia);
                                                            lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strDiaMesAnio, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo, _fontGeneral));
                                                        }
                                                        else
                                                        {

                                                            if (lineasSegundaTrozo.IndexOf("QUE CONSTE EL PODER GENERAL QUE OTORGAN ") > -1)
                                                            {
                                                                lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita("PODER GENERAL", lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                //string strOtorgante = "";
                                                                //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                                //{
                                                                //    strOtorgante = _listOtorgantesEC[i].Trim() + ", ";
                                                                //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                //}
                                                            }
                                                            //---------JONATAN SILVA 2------------
                                                            if (lineasSegundaTrozo.IndexOf("DENOMINARÁ") > -1 || lineasSegundaTrozo.IndexOf("DENOMINARÁN") > -1)
                                                            {
                                                                if (lineasSegundaTrozo.IndexOf("QUE CONSTE") > -1 && lineasSegundaTrozo.IndexOf("QUE OTORGA") > -1)
                                                                {
                                                                    oParagraphEC.Add(new iTextSharp.text.Chunk("QUE CONSTE ", _fontGeneral));
                                                                    lineasSegundaTrozo = lineasSegundaTrozo.Replace("QUE CONSTE ", "");

                                                                    int intIndiceQueOtorga = lineasSegundaTrozo.IndexOf("QUE OTORGA");
                                                                    string strTitulo = lineasSegundaTrozo.Substring(0, intIndiceQueOtorga);
                                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strTitulo, _fontGeneralB));
                                                                    lineasSegundaTrozo = lineasSegundaTrozo.Replace(strTitulo, "");

                                                                    //string strOtorgante = "";

                                                                    //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                                    //{
                                                                    //    strOtorgante = _listOtorgantesEC[i].Trim() + ", ";
                                                                    //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                    //}

                                                                }
                                                                
                                                            }


                                                            string strAQuienEnAdelanteSeLe = "A QUIEN EN ADELANTE SE LE";
                                                            string strElApoderadoy = "EL APODERADO.";

                                                            int intIndiceAQuienEnAdelanteSeLe = lineasSegundaTrozo.IndexOf(strAQuienEnAdelanteSeLe);
                                                            int intIndiceElApoderadoy = lineasSegundaTrozo.IndexOf(strElApoderadoy);

                                                            if (intIndiceAQuienEnAdelanteSeLe > -1 && intIndiceElApoderadoy > -1)
                                                            {

                                                                oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                            }
                                                            else
                                                            {
                                                                string strEnAdelanteSeDenominaran = "EN ADELANTE SE LES DENOMINARÁN ";
                                                                string strLosPoderdantesx = "LOS PODERDANTES, ";
                                                                string strAFavorDe = "A FAVOR DE";
                                                                string strAQuienAdelanteSelesDenominaran = "A QUIENES EN ADELANTE SE LES DENOMINARÁN";
                                                                string strLosApoderados = "LOS APODERADOS.";

                                                                int intIndiceEnAdelanteSeDenominaran = lineasSegundaTrozo.IndexOf(strEnAdelanteSeDenominaran);
                                                                int intIndiceLosPoderdantesx = lineasSegundaTrozo.IndexOf(strLosPoderdantesx);
                                                                int intIndiceAFavorDe = lineasSegundaTrozo.IndexOf(strAFavorDe);
                                                                int intIndiceAQuienAdelanteSelesDenominaran = lineasSegundaTrozo.IndexOf(strAQuienAdelanteSelesDenominaran);
                                                                int intIndiceLosApoderados = lineasSegundaTrozo.IndexOf(strLosApoderados);

                                                                if (intIndiceEnAdelanteSeDenominaran > -1 && intIndiceLosPoderdantesx > -1 &&
                                                                    intIndiceAFavorDe > -1 && intIndiceAQuienAdelanteSelesDenominaran > -1 && intIndiceLosApoderados > -1)
                                                                {
                                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                }
                                                                else
                                                                {


                                                                    string strSeLeDenominaraLaApoderada = "SE LE DENOMINARÁ LA APODERADA.";
                                                                    string strSeLeDenominaraLaApoderado = "SE LE DENOMINARÁ EL APODERADO.";

                                                                    int intIndiceSeLeDenominaraLaApoderada = lineasSegundaTrozo.IndexOf(strSeLeDenominaraLaApoderada);
                                                                    int intIndiceSeLeDenominaraLaApoderado = lineasSegundaTrozo.IndexOf(strSeLeDenominaraLaApoderado);

                                                                    if (intIndiceSeLeDenominaraLaApoderada > -1 || intIndiceSeLeDenominaraLaApoderado > -1)
                                                                    {
                                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                    }
                                                                    else
                                                                    {
                                                                        //-----------------------------
                                                                        // PODERDANTES DE NOMBRES
                                                                        //-----------------------------

                                                                        if (lineasSegundaTrozo.IndexOf("PODERDANTE") > -1 || lineasSegundaTrozo.IndexOf("PODERDANTES") > -1)
                                                                        {
                                                                            if (lineasSegundaTrozo.IndexOf("TRADUCIDA SIMULTÁNEAMENTE") > -1)
                                                                            {
                                                                                string strinterprete = "";
                                                                                for (int i = 0; i < _listInterpretesEC.Count; i++)
                                                                                {
                                                                                    strinterprete = _listInterpretesEC[i].Trim() + ", ";
                                                                                    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strinterprete, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                //--------------------------------------
                                                                                //Poner en negrita a los otorgantes
                                                                                //--------------------------------------
                                                                                //string strOtorgante = "";

                                                                                //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                                                //{
                                                                                //    strOtorgante = _listOtorgantesEC[i].Trim() + ", ";
                                                                                //    lineasSegundaTrozo = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, lineasSegundaTrozo, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                                //}
                                                                            }
                                                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                        }
                                                                        else
                                                                        {
                                                                            oParagraphEC.Add(new iTextSharp.text.Chunk(lineasSegundaTrozo.Trim(), _fontGeneral));
                                                                        }

                                                                    }
                                                                }
                                                            }                                                                                                                        
                                                        }
                                                    }
                                                } 
                                            }
                                        }
                                        
                                    }

                                    document.Add(oParagraphEC);
                                }

                                iContadorLineas = iLineasSobrantes;

                                while (iContadorLineas >= _iLineNumberGeneral)
                                {
                                    iContadorLineas -= _iLineNumberGeneral;
                                }

                                if (iContadorLineas == 0)
                                {
                                    document.NewPage();
                                }
                                #endregion
                            }
                            else
                            {
                                string textoHoja = string.Empty;

                                for (int i = 0; i < listLineas.Count; i++)
                                { textoHoja += listLineas[i] + " "; }

                                oParagraphEC.Clear();
                                //-----------------------------------------------------
                                // Fecha: 18/04/2017
                                // Autor: Miguel Márquez Beltrán
                                // Objetivo: Poner en negrita el texto central.
                                //-----------------------------------------------------               
                                //====================================================================
                               
                                if (bEsEscrituraPublica)
                                {
                                    #region EscrituraPublica

                                    if (textoHoja.IndexOf("CONCLUSIÓN:") > -1)
                                    {
                                        oParagraphEC.Add(new iTextSharp.text.Chunk("CONCLUSIÓN:", _fontGeneralB));

                                        textoHoja = textoHoja.Replace("CONCLUSIÓN:", "");

                                        //------------------------------------
                                        //Poner negrita el nombre del consul.
                                        //------------------------------------

                                       // textoHoja = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario + ", ", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                        //---------------------------------------------------------
                                        

                                        oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                        //------------------------------------------------
                                    }
                                    else
                                    {
                                        //--------------------------------------------------
                                        //Fecha: 02/05/2017
                                        //Autor: Miguel Márquez Beltrán
                                        //Objetivo: Poner negrita el nombre del artículo
                                        //--------------------------------------------------
                                        int intInicioArticulo = textoHoja.IndexOf("INSERTO:");
                                        int intFinArticulo = textoHoja.IndexOf(".-");
                                        if (intInicioArticulo > -1 && intFinArticulo > -1)
                                        {
                                            string strNombreArticulo = textoHoja.Substring(intInicioArticulo, intFinArticulo);
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(strNombreArticulo, _fontGeneralB));

                                            textoHoja = textoHoja.Replace(strNombreArticulo, "");
                                            oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));

                                        }
                                        else
                                        {
                                            //--------------------------------------------------
                                            //Fecha: 14/10/2021
                                            //Autor: Miguel Márquez Beltrán
                                            //Motivo: Poner negrita a los nuevos insertos de 
                                            //          Renuncia a la Nacionalidad.
                                            //--------------------------------------------------
                                            string strConstitucionPoliticaPeruArt53 = "CONSTITUCIÓN POLÍTICA DEL PERÚ: ARTÍCULO 53.- ADQUISICIÓN Y RENUNCIA DE LA NACIONALIDAD. ";

                                            if (bEsEscrituraPublica && textoHoja.IndexOf(strConstitucionPoliticaPeruArt53) > -1)
                                            {
                                                textoHoja = RetornaTextoPosteriorAsignacionNegrita(strConstitucionPoliticaPeruArt53, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                            }
                                            string strPerdidaNacionalidad = "LEY N° 26574.- LEY DE NACIONALIDAD.- ARTÍCULO 7.- PERDIDA DE LA NACIONALIDAD. ";
                                            if (bEsEscrituraPublica && textoHoja.IndexOf(strPerdidaNacionalidad) > -1)
                                            {
                                                textoHoja = RetornaTextoPosteriorAsignacionNegrita(strPerdidaNacionalidad, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                            }
                                            //--------------------------------------------------

                                            if (bEsEscrituraPublica && textoHoja.IndexOf("CONCLUYE LA SUSCRIPCIÓN") > -1)
                                            {
                                                textoHoja = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);

                                                string strElDia = "EL DÍA";
                                                int intIndiceInicialElDia = textoHoja.IndexOf(strElDia) + strElDia.Length;
                                                string strDeLoQueDoyFe = "DE LO QUE DOY FE.";
                                                int intIndiceInicialDeLoQueDoyFe = textoHoja.IndexOf(strDeLoQueDoyFe);
                                                string strDiaMesAnio = textoHoja.Substring(intIndiceInicialElDia, intIndiceInicialDeLoQueDoyFe - intIndiceInicialElDia);

                                                textoHoja = RetornaTextoPosteriorAsignacionNegrita(strDiaMesAnio, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);

                                                oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                            }
                                            else
                                            {
                                                if ((bEsEscrituraPublica || bEsParte) && textoHoja.IndexOf("FIRMA:") > -1)
                                                {
                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita(NombreFuncionario, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                }
                                                else
                                                {
                                                    #region Señor Consul

                                                    string strSeniorConsul = "SEÑOR(A) CÓNSUL:";

                                                    if ((bEsEscrituraPublica || bEsParte) && textoHoja.IndexOf(strSeniorConsul) > -1)
                                                    {
                                                        //oParagraphEC.Add(new iTextSharp.text.Chunk(strSeniorConsul, _fontGeneralB));

                                                        //textoHoja = textoHoja.Replace(strSeniorConsul, "");
                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                                    }
                                                    else
                                                    {
                                                        if (textoHoja.IndexOf("NACIONALIDAD") > -1 || textoHoja.IndexOf("A FAVOR DE") > -1)
                                                        {
                                                            if (textoHoja.IndexOf("A FAVOR DE") == -1)
                                                            {
                                                                //string strOtorgante = "";

                                                                //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                                //{
                                                                //    strOtorgante = _listOtorgantesEC[i].Trim() + ",";
                                                                //    textoHoja = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                //}

                                                                //if (textoHoja.IndexOf("DESIGNA A:") > -1)
                                                                //{
                                                                //    string strInterprete = "";

                                                                //    for (int i = 0; i < _listInterpretesEC.Count; i++)
                                                                //    {
                                                                //        strInterprete = _listInterpretesEC[i].Trim() + ",";
                                                                //        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strInterprete, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                //    }
                                                                //}
                                                                //if (textoHoja.IndexOf("INTERVIENE:") > -1)
                                                                //{
                                                                //    string strTestigo = "";

                                                                //    for (int i = 0; i < _listTestigosEC.Count; i++)
                                                                //    {
                                                                //        strTestigo = _listTestigosEC[i].Trim() + ",";
                                                                //        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strTestigo, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                //    }
                                                                //}
                                                                oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                            }
                                                            else
                                                            {
                                                                #region AFavor
                                                                if (bEsEscrituraPublica)
                                                                {
                                                                    string strQueConsteEl = "QUE CONSTE";

                                                                    if (textoHoja.IndexOf(strQueConsteEl) > -1)
                                                                    {
                                                                        int intIndiceInicialQueConsteEl = textoHoja.IndexOf(strQueConsteEl);
                                                                        int intIndiceFinalQueConsteEl = intIndiceInicialQueConsteEl + strQueConsteEl.Length;
                                                                        string strQueOtorga = "";

                                                                        strQueOtorga = "QUE";

                                                                        string strCadena = textoHoja.Substring(intIndiceFinalQueConsteEl);
                                                                        int intIndiceInicialQueOtorga = strCadena.IndexOf(strQueOtorga);

                                                                        string strTipoActoNotarial = strCadena.Substring(0, intIndiceInicialQueOtorga);

                                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strTipoActoNotarial, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);

                                                                        //string strOtorgante = "";

                                                                        //for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                                        //{
                                                                        //    strOtorgante = _listOtorgantesEC[i].Trim() + ",";
                                                                        //    textoHoja = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                        //}
                                                                    }


                                                                    string strInterprete = "";

                                                                    for (int i = 0; i < _listInterpretesEC.Count; i++)
                                                                    {
                                                                        strInterprete = _listInterpretesEC[i].Trim() + ",";
                                                                        if (textoHoja.Contains(strInterprete))
                                                                        {
                                                                            textoHoja = RetornaTextoPosteriorAsignacionNegrita(strInterprete, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                        }
                                                                    }
                                                                    string strTestigo = "";

                                                                    for (int i = 0; i < _listTestigosEC.Count; i++)
                                                                    {
                                                                        strTestigo = _listTestigosEC[i].Trim() + ",";
                                                                        if (textoHoja.Contains(strTestigo))
                                                                        {
                                                                            textoHoja = RetornaTextoPosteriorAsignacionNegrita(strTestigo, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                        }
                                                                    }

                                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));
                                                                }

                                                                #endregion
                                                            }
                                                        }
                                                        else
                                                        {
                                                            #region FIRMA E IMPRESIÓN DACTILAR

                                                            if (bEsEscrituraPublica && (textoHoja.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1 || textoHoja.IndexOf("IMPRESIÓN DACTILAR:") > -1))
                                                            {
                                                                if (textoHoja.IndexOf("FIRMA E IMPRESIÓN DACTILAR:") > -1)
                                                                {
                                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita("FIRMA E IMPRESIÓN DACTILAR:", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                }
                                                                if (textoHoja.IndexOf("IMPRESIÓN DACTILAR:") > -1)
                                                                {
                                                                    textoHoja = RetornaTextoPosteriorAsignacionNegrita("IMPRESIÓN DACTILAR:", textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                }
                                                                string strOtorgante = "";

                                                                for (int i = 0; i < _listOtorgantesEC.Count; i++)
                                                                {
                                                                    strOtorgante = _listOtorgantesEC[i].Trim();
                                                                    if (textoHoja.Contains(strOtorgante))
                                                                    {
                                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strOtorgante, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                    }
                                                                }
                                                                string strInterprete = "";

                                                                for (int i = 0; i < _listInterpretesEC.Count; i++)
                                                                {
                                                                    strInterprete = _listInterpretesEC[i].Trim();
                                                                    if (textoHoja.Contains(strInterprete))
                                                                    {
                                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strInterprete, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                    }
                                                                }
                                                                string strTestigo = "";

                                                                for (int i = 0; i < _listTestigosEC.Count; i++)
                                                                {
                                                                    strTestigo = _listTestigosEC[i].Trim();
                                                                    if (textoHoja.Contains(strTestigo))
                                                                    {
                                                                        textoHoja = RetornaTextoPosteriorAsignacionNegrita(strTestigo, textoHoja, _fontGeneral, _fontGeneralB, ref oParagraphEC);
                                                                    }
                                                                }
                                                                oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja, _fontGeneral));                                                                                                                                
                                                                
                                                            }
                                                            else
                                                            {
                                                                #region LA PRESENTE ESCRITURA PÚBLICA
                                                                int intIndiceInicialLaPteEscPublica = textoHoja.IndexOf("LA PRESENTE ESCRITURA PÚBLICA");
                                                                int intIndiceInicialLibro = textoHoja.IndexOf("LIBRO");

                                                                if (bEsEscrituraPublica && intIndiceInicialLaPteEscPublica > -1 && intIndiceInicialLibro > -1)
                                                                {
                                                                    string strParte1 = textoHoja.Substring(0, intIndiceInicialLibro);

                                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strParte1, _fontGeneral));

                                                                    int intIndiceInicialRegInstPublicos = textoHoja.IndexOf("DEL REGISTRO DE INSTRUMENTOS PÚBLICOS");

                                                                    string strLibro = textoHoja.Substring(intIndiceInicialLibro, intIndiceInicialRegInstPublicos - intIndiceInicialLibro);

                                                                    oParagraphEC.Add(new iTextSharp.text.Chunk(strLibro, _fontGeneralB));

                                                                    string strParte2 = textoHoja.Substring(intIndiceInicialRegInstPublicos);

                                                                    if (strParte2.IndexOf("DE ESTE CONSULADO. =") > -1)
                                                                    {
                                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strParte2.Trim(), _fontGeneral));
                                                                    }
                                                                    else
                                                                    {
                                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(strParte2, _fontGeneral));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    
                                                                    if (bEsParte)
                                                                    {
                                                                        
                                                                    }
                                                                    else
                                                                    {
                                                                        oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral));
                                                                    }
                                                                                                                                     
                                                                }
                                                                #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                   
                                    #endregion
                                }
                                else
                                { oParagraphEC.Add(new iTextSharp.text.Chunk(textoHoja.Trim(), _fontGeneral)); }
                                //====================================================================Fin Si es Escritura Pública

                                document.Add(oParagraphEC);

                            }

                            //--------------------------------------------------
                            #region Imagen

                            bool bNuevaPaginaParaImagen = true;

                            if (bExistImage)
                            {
                                bExistImage = false;
                                if (_listImagenes.Count > 0)
                                {
                                    oParagraphEC.Clear();

                                    int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                                    bAplicarImagen = true;

                                    if (iNumLineas > 0 && iNumLineas < 25)
                                    {
                                        for (int y = 0; y < iNumLineas; y++)
                                        {
                                            string linea = string.Empty;
                                            for (int x = 0; x < 67; x++)
                                            {
                                                linea += "=";
                                            }

                                            oParagraphEC.Add(new iTextSharp.text.Chunk(linea + " ", _fontGeneral));

                                        }

                                        goto LineCheck;

                                    }

                                    bNuevaPaginaParaImagen = false;
                                }
                            }

                            if (bAplicarImagen)
                            {

                                foreach (ImagenNotarial imagen in _listImagenes)
                                {
                                    if (bNuevaPaginaParaImagen)
                                        document.NewPage();
                                    else
                                        bNuevaPaginaParaImagen = true;

                                    #region Imagen

                                    cb = writer.DirectContent;
                                    cb.Rectangle(fMargenIzquierdaDoc, fMargenInferiorDoc, fAnchoAreaTexto, fDocumentHeight - (fMargenInferiorDoc + fMargenSuperiorDoc));
                                    cb.Stroke();


                                    string imgPath = string.Empty;


                                    if (File.Exists(imagen.vRuta))
                                    {
                                        imgPath = imagen.vRuta;
                                    }
                                    else
                                    {
                                        imgPath = HttpContext.Current.Server.MapPath("~/Images/img_noDisponible.jpg");
                                    }

                                    iTextSharp.text.Image newImg = iTextSharp.text.Image.GetInstance(imgPath);

                                    float imgHeight = newImg.Height;
                                    float imgWidth = newImg.Width;
                                    float factorRelacion = 1;
                                    bool bHeightMayor = false;

                                    if (imgHeight > imgWidth)
                                    {
                                        factorRelacion = imgHeight / imgWidth;
                                        bHeightMayor = true;
                                    }
                                    else
                                    {
                                        factorRelacion = imgWidth / imgHeight;
                                        bHeightMayor = false;
                                    }


                                    bool bHeightAdecuado = false;
                                    bool bWidthAdecuado = false;


                                    while (!bHeightAdecuado || !bWidthAdecuado)
                                    {

                                        if (!(imgHeight > fDocumentHeight - (fMargenInferiorDoc + fMargenSuperiorDoc)))
                                            bHeightAdecuado = true;


                                        if (!(imgWidth > fAnchoAreaTexto))
                                            bWidthAdecuado = true;


                                        if (bHeightAdecuado != bWidthAdecuado || (!bHeightAdecuado && !bWidthAdecuado))
                                        {
                                            if (bHeightMayor)
                                            {
                                                imgWidth -= 1;
                                                imgHeight -= factorRelacion;
                                            }
                                            else
                                            {
                                                imgHeight -= 1;
                                                imgWidth -= factorRelacion;
                                            }
                                        }

                                    }



                                    newImg.SetAbsolutePosition((document.PageSize.Width / 2) - (imgWidth / 2) + 36,
                                        (document.PageSize.Height / 2) - (imgHeight / 2));
                                    newImg.ScaleToFitHeight = false;
                                    newImg.ScaleToFitLineWhenOverflow = false;
                                    newImg.ScaleAbsoluteHeight(imgHeight);
                                    newImg.ScaleAbsoluteWidth(imgWidth);
                                    document.Add(newImg);

                                    #endregion


                                    parrafo = new iTextSharp.text.Paragraph();
                                    parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                                    parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL);
                                    frase = new iTextSharp.text.Phrase();
                                    frase.Add(new iTextSharp.text.Chunk("\n" + imagen.vTitulo));
                                    parrafo.Add(frase);
                                    document.Add(parrafo);
                                }

                                document.NewPage();
                                bAplicarImagen = false;

                            }
                            //--------------------------------------------------
                            #endregion

                        }
                    }
                    //=============================Fin Objects
                    #endregion

                    #region Firmas

                    //-------------------------------------------
                    //Fecha: 11/01/2022
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: No debe generar el área de firmas en el testimonio
                    //Documento: OBSERVACIONES_SGAC_10012022.
                    //-------------------------------------------
                    //if (_listDocumentoFirmaEC != null)
                    //{
                    //    parrafo = new iTextSharp.text.Paragraph();
                    //    frase = new iTextSharp.text.Phrase();

                    //    #region BloqueDocFirma_Otorgante_Vendedor_Anticipante
                        
                    //    foreach (DocumentoFirma docFirma in _listDocumentoFirmaEC)
                    //    {
                    //        if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.OTORGANTE) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPANTE) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.ANTICIPADO) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.VENDEDOR) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.COMPRADOR) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONANTE) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.DONATARIO))
                    //        {
                    //            parrafo = new iTextSharp.text.Paragraph();
                    //            frase = new iTextSharp.text.Phrase();

                    //            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 170))
                    //            {

                    //                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                    //                parrafo.Add(frase);
                    //                document.Add(parrafo);
                    //            }
                    //            else
                    //            {
                    //                _iPageNumber++;
                    //                //while (writer.GetVerticalPosition(false) < (fMargenSuperiorDoc + 120))
                    //                //{
                    //                //    document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                    //                //}
                    //                // EnumerarFoja(cb, document);
                    //                int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                    //                if (iNumLineas >= 0 && iNumLineas < 25)
                    //                {
                    //                    for (int y = 0; y < iNumLineas; y++)
                    //                    {
                    //                        string linea = string.Empty;
                    //                        //for (int x = 0; x < 67; x++)
                    //                        //{
                    //                        //    linea += "=";
                    //                        //}
                    //                        //document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(linea, _fontGeneral)));
                    //                        document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", _fontGeneral)));
                    //                    }
                    //                    if (iNumLineas <= 4)
                    //                    {
                    //                        document.NewPage();
                    //                        document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                    //                    }
                    //                }

                    //            }

                    //            if (docFirma.bIncapacitado == false)
                    //            {
                    //                parrafo = new iTextSharp.text.Paragraph();
                    //                frase = new iTextSharp.text.Phrase();

                    //                frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 40.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                    //                parrafo.Add(frase);
                    //                document.Add(parrafo);
                    //            }
                    //            if (docFirma.bAplicaHuellaDigital)
                    //            {
                    //                cb = writer.DirectContent;
                    //                cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 40f, 50f);
                    //                cb.Stroke();
                    //            }

                    //            parrafo = new iTextSharp.text.Paragraph();
                    //            parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);
                    //            frase = new iTextSharp.text.Phrase();
                    //            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto.Replace(",", "")));
                    //            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                    //            parrafo.Add(frase);
                    //            document.Add(parrafo);

                    //        }
                    //    }

                    //    #endregion

                    //    #region BloqueDocFirma_Testigo_a_Ruego_Interprete

                    //    foreach (DocumentoFirma docFirma in _listDocumentoFirmaEC)
                    //    {

                    //        if (docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.TESTIGO_A_RUEGO) ||
                    //            docFirma.sTipoParticipante == Convert.ToInt16(Enumerador.enmNotarialProtocolarTipoParticipante.INTERPRETE))
                    //        {

                    //            parrafo = new iTextSharp.text.Paragraph();
                    //            frase = new iTextSharp.text.Phrase();

                    //            if (writer.GetVerticalPosition(false) >= (fMargenSuperiorDoc + 170))
                    //            {

                    //                frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n"));
                    //                parrafo.Add(frase);
                    //                document.Add(parrafo);
                    //            }
                    //            else
                    //            {
                    //                _iPageNumber++;
                    //                //-------------------------------------------------
                    //                int iNumLineas = (int)Math.Truncate((decimal)((writer.GetVerticalPosition(false) - fMargenInferiorDoc) / (FCuerpoLeading * _fCuerpoFontSize)));
                    //                if (iNumLineas >= 0 && iNumLineas < 25)
                    //                {
                    //                    for (int y = 0; y < iNumLineas; y++)
                    //                    {
                    //                        string linea = string.Empty;
                    //                        //for (int x = 0; x < 67; x++)
                    //                        //{
                    //                        //    linea += "=";
                    //                        //}
                    //                        //document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(linea, _fontGeneral)));
                    //                        document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n", _fontGeneral)));
                    //                    }
                    //                    if (iNumLineas <= 4)
                    //                    {
                    //                        document.NewPage();
                    //                        document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));
                    //                    }
                    //                }
                    //                //-------------------------------------------------
                    //            }

                    //            parrafo = new iTextSharp.text.Paragraph();
                    //            frase = new iTextSharp.text.Phrase();

                    //            frase.Add(new iTextSharp.text.pdf.draw.LineSeparator(1F, 40.0F, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_LEFT, 1));
                    //            parrafo.Add(frase);
                    //            document.Add(parrafo);

                    //            if (docFirma.bAplicaHuellaDigital)
                    //            {
                    //                cb = writer.DirectContent;
                    //                //cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 65f, 80f);
                    //                cb.Rectangle(document.PageSize.Width - 270, writer.GetVerticalPosition(false) - 15, 40f, 50f);
                    //                cb.Stroke();
                    //            }

                    //            parrafo = new iTextSharp.text.Paragraph();
                    //            parrafo.Font = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);
                    //            frase = new iTextSharp.text.Phrase();
                    //            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNombreCompleto.Replace(",", "")));
                    //            frase.Add(new iTextSharp.text.Chunk("\n" + docFirma.vNroDocumentoCompleto));
                    //            parrafo.Add(frase);
                    //            document.Add(parrafo);
                    //        }
                    //    }
                    //    #endregion
                    //}

                    #endregion
                    //==========================================================================
                    _iPageNumber = cb.PdfWriter.PageNumber;

                    document.Close();

                    #region Impresion en Navegador


                    Byte[] FileBuffer = ms.ToArray();

                    
                    //---------------------------------------------------------------
                    //Fecha: 25/04/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Enumerar cada pagina del documento PDF
                    //---------------------------------------------------------------
                    if (bEsEscrituraPublica)
                    {
                        if (!bEsTestimonio)
                        {
                            FileBuffer = EnumerarPDF(FileBuffer, true).ToArray();
                            FileBuffer = EtiquetaMinuta(FileBuffer, 2).ToArray();
                        }                        
                    }
                    //---------------------------------------------------------------

                    if (FileBuffer != null)
                    {
                        System.Web.HttpContext.Current.Session["binaryData"] = FileBuffer;
                        if (bGenerarDocumentoAutomaticamente)
                        {
                            GenerarDocumentoPDF();
                        }
                    }


                    str.Close();
                    oStreamReader.Close();
                    oStreamReader.Dispose();


                    #endregion

                }
            }
            catch (Exception ex)
            {
                Comun.EjecutarScript(this._page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "DocumentoiTextSharp: CrearTestimonioEscrituraPublicaPDF/", ex.Message));
            }
        }

        private void EnumerarFoja(PdfContentByte cb, iTextSharp.text.Document document)
        {            

            if (bEsEscrituraPublica)
            {
                if (!bEsTestimonio)
                {
                    cb.BeginText();

                    iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                    cb.SetFontAndSize(CuerpoBaseFont, 10);

                    //cb.SetTextMatrix(document.PageSize.Width - 50, document.PageSize.Height - 50);
                    cb.SetTextMatrix(document.PageSize.Width - 105, document.PageSize.Height - 50);

                    _iFojaActual++;

                    if (!bEsVistaPrevia)
                    {
                        cb.ShowText("Foja: " + _iFojaActual.ToString());
                        //cb.ShowText(_iFojaActual.ToString());
                    }
                    cb.EndText();
                }
            }
        }
        
        void PosicionarTexto(List<TextoPosicionadoITextSharp> _listTextoPosicionadoiTextSharp, PdfContentByte cb, int iPageNumber, Boolean bTitulo = false)
        {
            float posicionCentrada = 0;
            int cont = 0;
            cb.BeginText();

            for (int i = 0; i < _listTextoPosicionadoiTextSharp.Count; i++)
            {
                TextoPosicionadoITextSharp textoPosicionado = _listTextoPosicionadoiTextSharp[i];

                cb.SetFontAndSize(textoPosicionado.BaseFont, textoPosicionado.FontSize);

                float tamanoPalabra = new iTextSharp.text.Chunk(textoPosicionado.Texto, textoPosicionado.FontFamily).GetWidthPoint();

                if (textoPosicionado.TextAligment == HorizontalAlign.NotSet)
                {
                    cb.SetTextMatrix(textoPosicionado.FXPosition, textoPosicionado.FYPosition);
                }
                else if (textoPosicionado.TextAligment == HorizontalAlign.Left)
                {
                    cb.SetTextMatrix(_fMarginLeft, textoPosicionado.FYPosition);
                }
                else if (textoPosicionado.TextAligment == HorizontalAlign.Right)
                {
                    float posicionDerecha = (fConstDocumentWidth - _fMarginRight) - tamanoPalabra;
                    cb.SetTextMatrix(posicionDerecha, textoPosicionado.FYPosition);
                }
                else if (textoPosicionado.TextAligment == HorizontalAlign.Center)
                {
                    if (bTitulo)
                    {
                        posicionCentrada = ((fConstDocumentWidth / 2) - (tamanoPalabra / 2)) + ((_fMarginLeft - _fMarginRight) / 2);
                        cb.SetTextMatrix(posicionCentrada, textoPosicionado.FYPosition);
                    }
                    else {
                        posicionCentrada = ((fConstDocumentWidth / 2) - (tamanoPalabra / 2)) + ((_fMarginLeft - _fMarginRight) / 2);
                        cb.SetTextMatrix(posicionCentrada, textoPosicionado.FYPosition);
                    }
                }

                if (new iTextSharp.text.Chunk(textoPosicionado.Texto, textoPosicionado.FontFamily).GetWidthPoint() < fDocumentTextArea)
                {
                    cb.ShowText(textoPosicionado.Texto);
                }
                else
                {
                    float fPalabraAncho = 0;
                    float fLineaAncho = 0;
                    string lineaAcumulada = string.Empty;
                    int iLineas = 0;
                    foreach (string palabra in textoPosicionado.Texto.Split(' '))
                    {
                        fPalabraAncho = new iTextSharp.text.Chunk(palabra, new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL)).GetWidthPoint();

                        if (fLineaAncho + fPalabraAncho > fConstDocumentWidth - (_fMarginLeft + _fMarginRight + 20))
                        {
                            posicionCentrada = ((fConstDocumentWidth / 2) - (fLineaAncho / 2)) + ((_fMarginLeft - _fMarginRight) / 2);
                            cb.SetTextMatrix(posicionCentrada, textoPosicionado.FYPosition - ((textoPosicionado.FontSize + FCuerpoLeading) * iLineas));
                            cb.ShowText(lineaAcumulada);

                            fLineaAncho = 0;

                            lineaAcumulada = string.Empty;
                            iLineas++;
                        }

                        if (palabra.Trim() != string.Empty)
                        {
                            lineaAcumulada += palabra.Trim() + " ";
                            fLineaAncho += fPalabraAncho + new iTextSharp.text.Chunk(" ", new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.NORMAL)).GetWidthPoint();

                        }
                    }


                    posicionCentrada = ((fConstDocumentWidth / 2) - (fLineaAncho / 2)) + ((_fMarginLeft - _fMarginRight) / 2);
                    cb.SetTextMatrix(posicionCentrada, textoPosicionado.FYPosition - ((textoPosicionado.FontSize + FCuerpoLeading) * iLineas));
                    cb.ShowText(lineaAcumulada);

                }

                cont++;

                if (cont >= 11)
                {
                    break;
                }
            }

            cb.EndText();


        }

        static string ObtenerLineaJustificada(iTextSharp.text.Chunk chunk, float fLineaWidth)
        {
            string linea = chunk.Content.Trim();
            int intContador = 0;

            while (true)
            {
                intContador++;
                List<int> listPosiciones = new List<int>();

                int pos = 0;
                foreach (char letra in linea)
                {
                    if (letra == ' ')
                    {
                        listPosiciones.Add(pos);
                    }
                    pos++;
                }

                if (listPosiciones.Count == 0)
                    return linea;

                for (int i = listPosiciones.Count - 1; i >= 0; i--)
                {
                    if (new iTextSharp.text.Chunk(linea + " ", chunk.Font).GetWidthPoint() > fLineaWidth)
                    {
                        return linea;
                    }

                    linea = linea.Insert(listPosiciones[i], " ");
                }

                if (intContador > 10)
                    throw new Exception("DocumentoiTextSharp/ObtenerLineaJustificada/intContador es mayor a 10.");                    

            }
        }

        public static string ObtenerTextoNotarialCierre(List<string> listTextos, float fAnchoAreaDocumento, List<iTextSharp.text.Font> listFonts)
        {
            float posx = 0;
            float posxAcumulado = 0;
            string textoAcumulado = string.Empty;
            string parrafo = string.Empty;
            int indiceTexto = 0;

            try
            {
                foreach (string texto in listTextos)
                {
                    if (texto == string.Empty)
                        continue;

                    if (parrafo.Length > 0)
                    {
                        if ((texto[0] == '.' || texto[0] == ',' || texto[0] == ':' || texto[0] == ';') && parrafo[parrafo.Length - 1] == ' ')
                        {
                            parrafo = parrafo.Remove(parrafo.Length - 1, 1);

                        }
                    }
                    parrafo += texto;

                    if (texto.Trim() != "." && texto.Trim() != "," && texto.Trim() != ":" && texto.Trim() != ";")
                    {
                        parrafo += " ";
                    }
                }

                float posAcumuladaResidual = 0;

                int intContador = 0;
                for (int i = 0; i < parrafo.Length; i++)
                {
                    char letra = parrafo[i];

                    iTextSharp.text.Font font = listFonts[ObtenerIndiceTextoEvaluado(listTextos, indiceTexto - 1)];
                    //posx = new iTextSharp.text.Chunk(textoAcumulado + texto.Trim(), font).GetWidthPoint();
                    posxAcumulado += new iTextSharp.text.Chunk(letra.ToString(), font).GetWidthPoint();
                    posAcumuladaResidual = posxAcumulado;
                    posx = posxAcumulado;

                    if (posx > fAnchoAreaDocumento)
                    {
                        if (letra != ' ' && letra != (char)160)
                        {
                            intContador = 0;
                            do
                            {
                                intContador++;
                                i--;
                                indiceTexto--;

                                if (intContador == 30)
                                {
                                    throw new Exception("DocumentoiTextSharp/ObtenerTextoNotarialCierre/IntContador es igual a 30.");
                                }
                            } while (parrafo[i] != ' ');
                        }

                        textoAcumulado = string.Empty;
                        posxAcumulado = 0;
                        posx = posxAcumulado;
                        indiceTexto++;
                        continue;

                    }

                    textoAcumulado += letra.ToString();


                    indiceTexto++;

                }

                float posDiferencia = fAnchoAreaDocumento - posAcumuladaResidual;
                bool bMinuta = false;

                if (textoAcumulado == "MINUTA: ")
                {
                    bMinuta = true;
                }

                textoAcumulado = string.Empty;
                posx = 0;
                iTextSharp.text.Font font2 = new iTextSharp.text.Font(listFonts[0]);
                font2.SetStyle(0);

                if (new iTextSharp.text.Chunk("=", font2).GetWidthPoint() < posDiferencia)
                {
                    while (new iTextSharp.text.Chunk(textoAcumulado + "=", font2).GetWidthPoint() < posDiferencia)
                    {

                        textoAcumulado += "=";
                    }
                }


                //if (new iTextSharp.text.Chunk("=", font2).GetWidthPoint() < posDiferencia)
                //{
                //    textoAcumulado += "=";
                //}


                if (bMinuta)
                {
                    textoAcumulado = textoAcumulado.Remove(0, 1);
                }

                return textoAcumulado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int ObtenerIndiceTextoEvaluado(List<string> listTextos, int indiceTexto)
        {

            int indice = 0;
            int longitudEvaluada = 0;
            foreach (string texto in listTextos)
            {
                if (texto.Length + longitudEvaluada > indiceTexto)
                {
                    return indice;
                }


                longitudEvaluada += texto.Length + 1;
                indice++;

                if (indice == listTextos.Count - 1)
                    return indice;
            }

            return indice;
        }

        public void GenerarDocumentoPDF()
        {
            string strUrl = "../Accesorios/VisorPDF.aspx";
            string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
            Comun.EjecutarScript(_page, strScript);
        }

        #endregion

        #region General

        public static void CreateFilePDFConformidad(DataTable TablaText, string HtmlPath, string PdfPath, string imgServerPAth, List<object[]> listFirmas, bool bAplicarCierreTexto = false)
        {

            try
            {

                if (!File.Exists(HtmlPath))

                    return;

                if (File.Exists(PdfPath))

                    File.Delete(PdfPath);

                float fMargenIzquierdaDoc = 80;

                float fMargenDerechaDoc = 80;


                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, fMargenIzquierdaDoc, fMargenDerechaDoc, 100, 80);

                StreamReader oStreamReader = new StreamReader(HtmlPath, System.Text.Encoding.Default);



                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();

                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);



                iTextSharp.text.IElement oIElement;

                iTextSharp.text.Paragraph oParagraph = null;

                iTextSharp.text.pdf.PdfPTable oPdfPTable;

                iTextSharp.text.pdf.PdfPRow oPdfPRow;

                iTextSharp.text.pdf.PdfPCell oPdfPCell = null;

                iTextSharp.text.Chunk oChunk;

                List<iTextSharp.text.IElement> objects;

                string strContent = string.Empty;



                iTextSharp.text.FontFactory.RegisterDirectories();

                document.Open();



                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));



                document.Open();



                document.NewPage();



                objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);



                float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;




                for (int k = 0; k < objects.Count; k++)
                {

                    oIElement = (iTextSharp.text.IElement)objects[k];

                    if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                    {

                        oParagraph = new iTextSharp.text.Paragraph();

                        oParagraph.Alignment = ((iTextSharp.text.Paragraph)objects[k]).Alignment;



                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;

                        iTextSharp.text.pdf.ColumnText ct = new iTextSharp.text.pdf.ColumnText(cb);



                        for (int z = 0; z < oIElement.Chunks.Count; z++)
                        {

                            strContent = ReplaceTexto(oIElement.Chunks[z].Content.ToString(), TablaText);



                            if (strContent != "\n")
                            {

                                strContent = strContent.Trim();

                                oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));

                            }

                            else
                            {

                                oParagraph.Add(new iTextSharp.text.Chunk(strContent, oIElement.Chunks[z].Font));

                                continue;

                            }



                            if (z == oIElement.Chunks.Count - 1)
                            {

                                List<string> listTextos = new List<string>();

                                List<iTextSharp.text.Font> listFonts = new List<iTextSharp.text.Font>();

                                string textoNotarialCierre = string.Empty;



                                foreach (iTextSharp.text.Chunk ch in oIElement.Chunks)
                                {

                                    listTextos.Add(ch.Content.Trim());

                                    listFonts.Add(ch.Font);



                                }



                                if (strContent.Trim() != string.Empty &&

                                     strContent.Trim() != "PODER FUERA DE REGISTRO" &&

                                     strContent.Trim() != "CONCLUSIÓN:" &&

                                    bAplicarCierreTexto)
                                {

                                    textoNotarialCierre = Comun.ObtenerTextoNotarialCierre(listTextos, fAnchoAreaTexto, listFonts);

                                }







                                if (textoNotarialCierre != string.Empty)
                                {

                                    iTextSharp.text.Font font = new iTextSharp.text.Font(oIElement.Chunks[z].Font);

                                    font.SetStyle(0);

                                    oParagraph.Add(new iTextSharp.text.Chunk(textoNotarialCierre, font));

                                }

                            }

                            else
                            {

                                oParagraph.Add(new iTextSharp.text.Chunk(" ", oIElement.Chunks[z].Font));

                            }

                        }


                        oParagraph.SetLeading(0.0f, 1.5f);

                        document.Add(oParagraph);

                    }

                    else if (objects[k].GetType().FullName == "iTextSharp.text.pdf.PdfPTable")
                    {

                        oPdfPTable = (iTextSharp.text.pdf.PdfPTable)objects[k];



                        iTextSharp.text.pdf.PdfPTable oNewPdfPTable = new iTextSharp.text.pdf.PdfPTable(oPdfPTable.NumberOfColumns);

                        int[] DimensionColumna = new int[oPdfPTable.NumberOfColumns];

                        int aux;

                        oNewPdfPTable.WidthPercentage = 100;

                        string imgFirma1 = string.Empty;

                        string imgFirma2 = string.Empty;



                        iTextSharp.text.Image jpg = null;



                        for (int row = 0; row < oPdfPTable.Rows.Count; row++)
                        {

                            for (int cell = 0; cell < oPdfPTable.Rows[row].GetCells().Length; cell++)
                            {

                                oPdfPCell = oPdfPTable.Rows[row].GetCells()[cell];

                                oParagraph = new iTextSharp.text.Paragraph();



                                for (int paragraph = 0; paragraph < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements.Count; paragraph++)
                                {

                                    for (int chunk = 0; chunk < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements[paragraph].Chunks.Count; chunk++)
                                    {

                                        if (!oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma1]") &

                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma2]"))
                                        {

                                            strContent = ReplaceTexto(oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content, TablaText);

                                            oParagraph.Add(new iTextSharp.text.Chunk(strContent, oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Font));



                                            aux = strContent.Length;



                                            if (aux > DimensionColumna[cell])

                                                DimensionColumna[cell] = aux;

                                            oParagraph.Leading = 12;

                                        }

                                    }

                                }

                                aux = 0;

                            }

                            oPdfPCell.CompositeElements.Clear();

                            oPdfPCell.AddElement(oParagraph);



                            if (jpg != null)
                            {

                                oPdfPCell.AddElement(jpg);

                                jpg = null;

                            }

                            oNewPdfPTable.AddCell(oPdfPCell);

                        }

                    }

                }



                iTextSharp.text.Paragraph parrafo = new iTextSharp.text.Paragraph();

                iTextSharp.text.Phrase frase = new iTextSharp.text.Phrase();





                foreach (object[] firma in listFirmas)
                {

                    parrafo = new iTextSharp.text.Paragraph();

                    frase = new iTextSharp.text.Phrase();



                    if (writer.GetVerticalPosition(false) >= 220)
                    {



                        frase.Add(new iTextSharp.text.Chunk("\n\n\n\n\n\n"));

                        parrafo.Add(frase);

                        document.Add(parrafo);

                    }

                    else
                    {



                        while (writer.GetVerticalPosition(false) < 220)
                        {

                            document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("\n")));

                        }

                    }


                    if ((bool)firma[2])
                    {

                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;

                        cb.Rectangle(document.PageSize.Width - 160, writer.GetVerticalPosition(false) - 10, 70f, 80f);

                        cb.Stroke();

                    }


                    parrafo = new iTextSharp.text.Paragraph();

                    parrafo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                    parrafo.Font = iTextSharp.text.FontFactory.GetFont("Arial");



                    frase = new iTextSharp.text.Phrase();

                    frase.Add(new iTextSharp.text.Chunk("\n" + "                                                                                                        Huella Digital"));

                    frase.Add(new iTextSharp.text.Chunk("\n\n" + "---------------------------------------------------------------"));

                    frase.Add(new iTextSharp.text.Chunk("\n" + firma[0].ToString().ToUpper()));

                    frase.Add(new iTextSharp.text.Chunk("\n" + firma[1].ToString()));

                    parrafo.Add(frase);

                    document.Add(parrafo);



                }





                document.Close();

                oStreamReader.Close();

            }

            catch (Exception ex)
            {

                throw ex;

            }

        }

        private static string ReplaceTexto(string oTexto, System.Data.DataTable dt)
        {
            string s_NewTexto = oTexto;
            int intFila = 0;
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        for (intFila = 0; intFila <= dt.Rows.Count - 1; intFila++)
                        {
                            string strcadBuscar = dt.Rows[intFila]["strCadenaBuscar"].ToString();
                            string strcadReemplaza = dt.Rows[intFila]["strCadenaReemplazar"].ToString();

                            s_NewTexto = s_NewTexto.Replace(strcadBuscar, strcadReemplaza);
                        }
                    }
                }
            }
            catch
            {
                s_NewTexto = oTexto;
            }

            return s_NewTexto;
        }

        #endregion

        //-----------------------------------------------------------
        //Fecha: 05/05/2017
        //Autor: Miguel Márquez 
        //Objetivo: Colocar el Número de folio en el documento PDF 
        //-----------------------------------------------------------

        private MemoryStream EnumerarPDF(Byte[] dataPDF, bool bEsTestimonioEscritura=false)
        {            
            MemoryStream ms = null;
            
            using (ms = new MemoryStream())
            {
                PdfReader reader = new PdfReader(dataPDF);
                PdfStamper pdfStamper = new PdfStamper(reader, ms);
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
                iTextSharp.text.Rectangle rect = reader.GetPageSizeWithRotation(1);
                
                
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte pdfContentByte = pdfStamper.GetOverContent(i);
             
                    pdfContentByte.BeginText();
                    pdfContentByte.SetFontAndSize(CuerpoBaseFont, 10);
                    pdfContentByte.SetTextMatrix(rect.Width - 105, rect.Height - 50);

                    if (!bEsVistaPrevia)
                    {
                        if (bEsTestimonioEscritura)
                        {
                            if (i > 1)
                            {
                                _iFojaActual++;
                                pdfContentByte.ShowText("FOJA: " + _iFojaActual.ToString());
                            }
                        }
                        else
                        {
                            _iFojaActual++;
                            pdfContentByte.ShowText("FOJA: " + _iFojaActual.ToString());
                        }
                    }
                    pdfContentByte.EndText();                    
                }
                pdfStamper.Close();
            }
            return ms;
        }

        private MemoryStream EtiquetaMinuta(Byte[] dataPDF, int intNumeroPagina)
        {
            MemoryStream ms = null;

            using (ms = new MemoryStream())
            {
                PdfReader reader = new PdfReader(dataPDF);
                PdfStamper pdfStamper = new PdfStamper(reader, ms);
                iTextSharp.text.Rectangle rect = reader.GetPageSizeWithRotation(1);
                iTextSharp.text.Font _fontGeneralB = new iTextSharp.text.Font(_cuerpoBaseFont, _fCuerpoFontSize, iTextSharp.text.Font.BOLD);

                if (reader.NumberOfPages > 0)
                {

                    PdfContentByte pdfContentByte = pdfStamper.GetOverContent(intNumeroPagina);

                    pdfContentByte.BeginText();
                    pdfContentByte.SetFontAndSize(_fontGeneralB.BaseFont, 12);
                    pdfContentByte.SetTextMatrix(rect.Width - 120, rect.Height - 30);

                    pdfContentByte.ShowText(sEtiquetaMinuta);
                        
                    pdfContentByte.EndText();
                }
                pdfStamper.Close();
            }
            return ms;
        }

        private int ObtenerNumeroFojas(Byte[] dataPDF)
        {
            int intNumeroFojas = 0;

            PdfReader reader = new PdfReader(dataPDF);
            intNumeroFojas = reader.NumberOfPages;

            return intNumeroFojas;
        }


        private string RetornaTextoPosteriorAsignacionNegrita(string strTextoBuscar, string strTexto, iTextSharp.text.Font _fontGeneral, iTextSharp.text.Font  _fontGeneralB, ref iTextSharp.text.Paragraph oParagraph)
        {
            string strTextoPosterior = string.Empty;

            int intInicioTexto = strTexto.IndexOf(strTextoBuscar);

            if (intInicioTexto > -1)
            {
                string strTextoAnterior = strTexto.Substring(0, intInicioTexto);

                if (strTextoAnterior.Trim().Length > 0)
                {
                    oParagraph.Add(new iTextSharp.text.Chunk(strTextoAnterior, _fontGeneral));
                }

                oParagraph.Add(new iTextSharp.text.Chunk(strTextoBuscar, _fontGeneralB));

                strTextoPosterior = strTexto.Substring(intInicioTexto + strTextoBuscar.Length);
            }
            else
            {
                strTextoPosterior = strTexto;
            }
            return strTextoPosterior;
        }


        public static byte[] MergeFiles(List<byte[]> sourceFiles)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            using (MemoryStream ms = new MemoryStream())
            {
                PdfCopy copy = new PdfCopy(document, ms);
                document.Open();
                int documentPageCounter = 0;

                // Iterate through all pdf documents
                for (int fileCounter = 0; fileCounter < sourceFiles.Count; fileCounter++)
                {
                    // Create pdf reader
                    PdfReader reader = new PdfReader(sourceFiles[fileCounter]);
                    int numberOfPages = reader.NumberOfPages;

                    // Iterate through all pages
                    for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        documentPageCounter++;
                        PdfImportedPage importedPage = copy.GetImportedPage(reader, currentPageIndex);
                        PdfCopy.PageStamp pageStamp = copy.CreatePageStamp(importedPage);

                        // Write header
                        //ColumnText.ShowTextAligned(pageStamp.GetOverContent(), iTextSharp.text.Element.ALIGN_CENTER,
                        //    new iTextSharp.text.Phrase("PDF Merger by Helvetic Solutions"), importedPage.Width / 2, importedPage.Height - 30,
                        //    importedPage.Width < importedPage.Height ? 0 : 1);

                        // Write footer
                        //ColumnText.ShowTextAligned(pageStamp.GetOverContent(), iTextSharp.text.Element.ALIGN_CENTER,
                        //    new iTextSharp.text.Phrase(String.Format("Page {0}", documentPageCounter)), importedPage.Width / 2, 30,
                        //    importedPage.Width < importedPage.Height ? 0 : 1);

                        pageStamp.AlterContents();

                        copy.AddPage(importedPage);
                    }

                    copy.FreeReader(reader);
                    reader.Close();
                }

                document.Close();
                return ms.GetBuffer();
            }
        }

        static string ObtenerIniciaRecibe(short iParticipanteId)
        {
            string strIniciaRecibe = "";

            if (HttpContext.Current.Session["vwparticipantes"] != null)
            {
                DataTable dtparticipantes = new DataTable();
                dtparticipantes = (DataTable)HttpContext.Current.Session["vwparticipantes"];

                for (int i = 0; i < dtparticipantes.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dtparticipantes.Rows[i]["PARA_SPARAMETROID"].ToString()) == iParticipanteId)
                    {
                        strIniciaRecibe = dtparticipantes.Rows[i]["PARA_VVALOR"].ToString().ToUpper().Trim();
                        break;
                    }
                }
            }
            return strIniciaRecibe;
        }
        //------------------------------------
    }


    class PdfWriterEvents : IPdfPageEvent
    {
        string watermarkText = string.Empty;

        public PdfWriterEvents(string watermark)
        {
            watermarkText = watermark;
        }

        public void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document document) { }
        public void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document) { }
        public void OnStartPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            float fontSize = 50;
            float xPosition = 300;
            float yPosition = 400;
            float angle = 45;
            try
            {
                PdfContentByte under = writer.DirectContentUnder;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                under.BeginText();
                under.SetColorFill(iTextSharp.text.BaseColor.GRAY);
                under.SetFontAndSize(baseFont, fontSize);
                under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
                under.EndText();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        public void OnEndPage(PdfWriter writer, iTextSharp.text.Document document) { }
        public void OnParagraph(PdfWriter writer, iTextSharp.text.Document document, float paragraphPosition) { }
        public void OnParagraphEnd(PdfWriter writer, iTextSharp.text.Document document, float paragraphPosition) { }
        public void OnChapter(PdfWriter writer, iTextSharp.text.Document document, float paragraphPosition, iTextSharp.text.Paragraph title) { }
        public void OnChapterEnd(PdfWriter writer, iTextSharp.text.Document document, float paragraphPosition) { }
        public void OnSection(PdfWriter writer, iTextSharp.text.Document document, float paragraphPosition, int depth, iTextSharp.text.Paragraph title) { }
        public void OnSectionEnd(PdfWriter writer, iTextSharp.text.Document document, float paragraphPosition) { }
        public void OnGenericTag(PdfWriter writer, iTextSharp.text.Document document, iTextSharp.text.Rectangle rect, String text) { }

    }

    class AlinearColumnaTabla
    {

        public AlinearColumnaTabla(string __vNombreColumna, int __iAlineacion)
        {
            _vNombreColumna = __vNombreColumna;
            _iAlineacion = __iAlineacion;
        }

        string _vNombreColumna;

        public string vNombreColumna
        {
            get { return _vNombreColumna; }
            set { _vNombreColumna = value; }
        }

        int _iAlineacion;

        public int iAlineacion
        {
            get { return _iAlineacion; }
            set { _iAlineacion = value; }
        }
    }
   
   
}
