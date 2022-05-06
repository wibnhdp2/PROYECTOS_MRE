using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;


    public class PDFComun
    {
        public float marginLeft { get; set; }
        public float marginRight { get; set; }
        public float marginTop { get; set; }
        public float marginBottom { get; set; }
        public iTextSharp.text.Rectangle pagesize { get; set; }
        public string Consulado { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string TituloFiltro { get; set; }
        public string TituloFiltroDerecha { get; set; }
        public bool ImprimirNumeroPagina { get; set; }

        public iTextSharp.text.pdf.PdfPTable pdftableCabeza { get; set; }
        public iTextSharp.text.pdf.PdfPTable pdftableDetalle { get; set; }
        public iTextSharp.text.pdf.PdfPTable pdftableResumen { get; set; }
        public iTextSharp.text.pdf.PdfPTable pdftablePie { get; set; }

        

        public byte[] CrearDocumentoPDF()
        {
            try
            {
                Byte[] FileBuffer = null;
                

                iTextSharp.text.FontFactory.RegisterDirectories();
                iTextSharp.text.Document document = new iTextSharp.text.Document(pagesize, marginLeft, marginRight, marginTop, marginBottom);

                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                

                using (var ms = new MemoryStream())
                {
                    //StreamWriter str = new StreamWriter(ms, Encoding.Default);
                    //str.Write(strHTML);
                    //str.Flush();

                    ms.Position = 0;

                    StreamReader oStreamReader = new StreamReader(ms, System.Text.Encoding.Default);

                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);
                    
                    document.Open();
                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;




                    float intYposDetalle = 465;

                   
                    //----------------

                    float fHeight = 0;
                    int intRowStart = 0;
                    int intRowEnd = 0;
                    
                    for (int i = 0; i < pdftableDetalle.Rows.Count; i++)
                    {

                        fHeight += pdftableDetalle.GetRowHeight(i);

                        if (fHeight + pdftableDetalle.GetRowHeight(i+1) > 360)
                        {
                            intRowEnd = i;
                            //-------------------------------------------------
                            document.NewPage();

                            AdicionarImagenTitulo(ref document, ref cb);    
                            //-------------------------------------------------
                            cb.BeginText();
                            pdftableDetalle.WriteSelectedRows(intRowStart, intRowEnd, 10, intYposDetalle, cb);
                            cb.EndText();
                            //----------------
                            if (intRowEnd == pdftableDetalle.Rows.Count - 1)
                            {
                                //-------------------------------------------------
                                cb.BeginText();
                                pdftableResumen.WriteSelectedRows(0, pdftableResumen.Rows.Count, 10, 140, cb);
                                cb.EndText();
                                //-------------------------------------------------
                            }
                            //----------------
                            cb.BeginText();
                            pdftablePie.WriteSelectedRows(0, pdftablePie.Rows.Count, 10, 70, cb);
                            cb.EndText();
                            //----------------         

                            intRowStart = i;
                            fHeight = 0;
                        }
                    }
                    //if (intRowStart < pdftableDetalle.Rows.Count - 1)
                    if (intRowStart < pdftableDetalle.Rows.Count )
                    {
                        //-------------------------------------------------
                        document.NewPage();

                        AdicionarImagenTitulo(ref document, ref cb);
                        //-------------------------------------------------                        
                        intRowEnd = pdftableDetalle.Rows.Count;


                        cb.BeginText();
                        pdftableDetalle.WriteSelectedRows(intRowStart, intRowEnd, 10, intYposDetalle, cb);
                        cb.EndText();
                        //-------------------------------------------------
                        fHeight = 0;
                        for (int i = intRowStart; i < intRowEnd + 1; i++)
                        {
                            fHeight += pdftableDetalle.GetRowHeight(i);
                        }
                        if (fHeight > 300)
                        {
                            //----------------
                            cb.BeginText();
                            pdftablePie.WriteSelectedRows(0, pdftablePie.Rows.Count, 10, 70, cb);
                            cb.EndText();
                            //---------------- 
                            document.NewPage();
                            AdicionarImagenTitulo(ref document, ref cb);  
                            fHeight = 0;
                        }
                        float intYposResumen = pagesize.Height - 475 - fHeight + 80;

                        cb.BeginText();
                        pdftableResumen.WriteSelectedRows(0, pdftableResumen.Rows.Count, 10, intYposResumen, cb);
                        cb.EndText();
                        //-------------------------------------------------
                        cb.BeginText();
                        pdftablePie.WriteSelectedRows(0, pdftablePie.Rows.Count, 10, 70, cb);
                        cb.EndText();
                        //----------------         
                    }

                    //----------------         
                    document.Close();

                    FileBuffer = ms.ToArray();

                    if (ImprimirNumeroPagina)
                    {
                        FileBuffer = EnumerarPDF(FileBuffer).ToArray();
                    }

                    oStreamReader.Close();
                    oStreamReader.Dispose();
                }
                return FileBuffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void AdicionarImagenTitulo(ref iTextSharp.text.Document document, ref iTextSharp.text.pdf.PdfContentByte cb)
        {
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/arialn.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);
            iTextSharp.text.pdf.BaseFont baseFontBold = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/arialnb.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);

            float fImageHeight = 63f;
            float fImageWidth = 289f;
            string imgLogo = HttpContext.Current.Server.MapPath("~/Images/img_reporte_logo.png");

            #region Imagen

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgLogo);
            img.SetAbsolutePosition(marginLeft, document.PageSize.Height - 70);
            img.ScaleAbsoluteHeight(fImageHeight);
            img.ScaleAbsoluteWidth(fImageWidth);
            img.ScalePercent(60f);
            document.Add(img);

            #endregion

            //----------------
            cb.BeginText();
            cb.SetFontAndSize(baseFontBold, 6);
            cb.SetTextMatrix(marginLeft + 10, document.PageSize.Height - 80);
            cb.ShowText(Consulado);
            cb.EndText();
            //----------------
            cb.BeginText();
            cb.SetFontAndSize(baseFontBold, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, SubTitulo, (document.PageSize.Left + document.PageSize.Right) / 2, document.PageSize.Height - 85, 0);
            cb.EndText();
            //----------------
            cb.BeginText();
            cb.SetFontAndSize(baseFontBold, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Titulo, (document.PageSize.Left + document.PageSize.Right) / 2, document.PageSize.Height - 100, 0);
            cb.EndText();
            //----------------
            cb.BeginText();
            cb.SetFontAndSize(baseFontBold, 9);
            cb.SetTextMatrix(marginLeft, document.PageSize.Height - 115);
            cb.ShowText(TituloFiltro);
            cb.EndText();
            //----------------
            cb.BeginText();
            cb.SetFontAndSize(baseFontBold, 9);
            cb.SetTextMatrix(document.PageSize.Width - 150, document.PageSize.Height - 115);
            cb.ShowText(TituloFiltroDerecha);
            cb.EndText();
            //----------------
            cb.BeginText();
            pdftableCabeza.WriteSelectedRows(0, pdftableCabeza.Rows.Count, 10, 477, cb);
            cb.EndText();

        }


        private MemoryStream EnumerarPDF(Byte[] dataPDF)
        {
            MemoryStream ms = null;

            using (ms = new MemoryStream())
            {
                PdfReader reader = new PdfReader(dataPDF);
                PdfStamper pdfStamper = new PdfStamper(reader, ms);

                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts/arialn.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                iTextSharp.text.Font fontArial_9 = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.Rectangle rect = reader.GetPageSizeWithRotation(1);


                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte pdfContentByte = pdfStamper.GetOverContent(i);

                    pdfContentByte.BeginText();
                    pdfContentByte.SetFontAndSize(baseFont, 9);

                    string strTexto = "Página: " + i.ToString() + " de " + reader.NumberOfPages.ToString();

                    pdfContentByte.SetTextMatrix((rect.Width / 2) - (new iTextSharp.text.Chunk(strTexto,fontArial_9).GetWidthPoint() / 2f) , 45);
                    pdfContentByte.ShowText(strTexto);
                    pdfContentByte.EndText();
                }
                
                
                pdfStamper.Close();
            }
            return ms;
        }

        public string GetUniqueUploadFileName(string uploadPath, string fileName)
        {
            String strScript = String.Empty;
            try
            {
                string filepath = uploadPath + "/" + fileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                do
                {
                    Random rnd = new Random();
                    int temp = rnd.Next(1000, 1000000);
                    filenamewithoutext += "_" + temp;
                    fileName = filenamewithoutext + fileext;
                    filepath = uploadPath + fileName;

                } while (File.Exists(filepath));

                return filepath;
            }
            catch (Exception ex)
            {

                //strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", ex.Message, false, 200, 400);
                //Comun.EjecutarScript(Page, strScript);
                return ex.Message;
            }
        }

        public void Celdas(ref PdfPTable pdftable, List<PDFCeldas> listaColumnas, BaseFont bfont, float fsizeFont, float NivelGris, bool bExisteBordes)
        {
            PdfPCell pdfpCelda;

            for (int y = 0; y < listaColumnas.Count; y++)
            {
                for (int i = 0; i < listaColumnas[y]._RepetirColumna; i++)
                {
                    pdfpCelda = new PdfPCell();

                    if (bExisteBordes)
                    {
                        pdfpCelda.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                        pdfpCelda.BorderColor = BaseColor.BLACK;
                        pdfpCelda.BorderWidth = 1f;
                    }
                    else
                    {
                        if (listaColumnas[y]._Border > 0)
                        {
                            pdfpCelda.BorderWidth = 1f;
                            pdfpCelda.Border = listaColumnas[y]._Border;
                        }
                        else
                        {
                            pdfpCelda.BorderWidth = 0f;
                        }
                    }


                    pdfpCelda.GrayFill = NivelGris;
                    pdfpCelda.Phrase = new Phrase(listaColumnas[y]._texto, new Font(bfont, fsizeFont, Font.NORMAL, BaseColor.BLACK));
                    pdfpCelda.VerticalAlignment = listaColumnas[y]._AlineamientoVertical;
                    pdfpCelda.HorizontalAlignment = listaColumnas[y]._AlineamientoHorizontal;
                    pdfpCelda.PaddingBottom = listaColumnas[y]._PaddingBottom; //5                    
                    pdfpCelda.PaddingRight = listaColumnas[y]._PaddingRight;
                    pdfpCelda.Colspan = listaColumnas[y]._Colspan;
                    pdftable.AddCell(pdfpCelda);

                }
            }

        }

    }
    