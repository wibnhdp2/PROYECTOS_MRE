using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp
{
    public class UIConvert
    {
        public static bool GenerarPDF(DataTable dtDatosReemplazar, string strRutaHtml, string strRutaPDF)
        {          
            try
            {
                if (!File.Exists(strRutaHtml)) return false;
                if (File.Exists(strRutaPDF)) File.Delete(strRutaPDF);

                CreateFilePDF(dtDatosReemplazar, strRutaHtml, strRutaPDF);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool GenerarPDF_Registro_Civil(DataTable dtDatosReemplazar, string strRutaHtml, string strRutaPDF)
        {
            try
            {
                
                if (!File.Exists(strRutaHtml)) return false;
                if (File.Exists(strRutaPDF)) File.Delete(strRutaPDF);

                CreateFile_Acta_RegistroCivil(dtDatosReemplazar, strRutaHtml, strRutaPDF);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        private static void CreateFilePDF(System.Data.DataTable TablaText, string HtmlPath, string PdfPath)
        {
            try
            {
                Document document = new Document(PageSize.A4, 55, 55, 20, 6);
                StreamReader oStreamReader = new StreamReader(HtmlPath, System.Text.Encoding.Default);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                IElement oIElement;
                Paragraph oParagraph = null;
                PdfPTable oPdfPTable;
                PdfPRow oPdfPRow;
                PdfPCell oPdfPCell = null;
                Chunk oChunk;
                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;


                PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));
                document.Open();

                document.NewPage();

                objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);

                for (int k = 0; k < objects.Count; k++)
                {
                    oIElement = (IElement)objects[k];
                    if(objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                    {
                        oParagraph = new Paragraph();
                        oParagraph.Alignment = ((Paragraph)objects[k]).Alignment;

                        for (int z = 0; z < oIElement.Chunks.Count; z++)
                        {
                            strContent = ReplaceTexto(oIElement.Chunks[z].Content.ToString(), TablaText);

                            //strContent = ReemplazarCadena(dtTextoremplazar, strContent);

                            oParagraph.Add(new Chunk(strContent, oIElement.Chunks[z].Font));
                            oParagraph.Leading = 12;
                        }
                        document.Add(oParagraph);
                    }
                    else if (objects[k].GetType().FullName == "iTextSharp.text.pdf.PdfPTable")
                    {
                        oPdfPTable = (PdfPTable)objects[k];

                        PdfPTable oNewPdfPTable = new PdfPTable(oPdfPTable.NumberOfColumns);
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
                                oParagraph = new Paragraph();

                                for (int paragraph = 0; paragraph < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements.Count; paragraph++)
                                {
                                    for (int chunk = 0; chunk < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements[paragraph].Chunks.Count; chunk++)
                                    {
                                        if (!oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma1]") &
                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma2]") &
                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Logo]"))
                                        {
                                            strContent = ReplaceTexto(oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content, TablaText);
                                            oParagraph.Add(new Chunk(strContent, oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Font));

                                            aux = strContent.Length;

                                            if (aux > DimensionColumna[cell])
                                                DimensionColumna[cell] = aux;
                                            if (oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.ToUpper().Equals("[CONSULADO]"))
                                                oParagraph.Leading = 6;
                                            else
                                                oParagraph.Leading = 12;
                                        }
                                        else
                                        {
                                            if (oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content == "[Logo]")
                                            {
                                                foreach (DataRow dr in TablaText.Rows)
                                                {
                                                    if (dr["strCadenaBuscar"].ToString().ToUpper() == "[Logo]".ToUpper())
                                                        imgFirma2 = new MyBasePage().Ruta_Logo();
                                                }
                                                if (imgFirma2 != "")
                                                {
                                                    //jpg = iTextSharp.text.Image.GetInstance(imgFirma2);
                                                    //jpg.ScaleToFit(137.0F, 60.0F);
                                                    //jpg.SpacingBefore = 5.0F;
                                                    //jpg.SpacingAfter = 1.0F;
                                                    //jpg.Alignment = Element.ALIGN_LEFT;

                                                    jpg = iTextSharp.text.Image.GetInstance(imgFirma2);
                                                    jpg.SetAbsolutePosition(40f, document.PageSize.Height - 100);
                                                    jpg.ScaleAbsoluteHeight(85f);
                                                    jpg.ScaleAbsoluteWidth(40f);
                                                    document.Add(jpg);
                                                }
                                            }
                                        }
                                    }
                                    aux = 0;
                                }
                                oPdfPCell.CompositeElements.Clear();
                                oPdfPCell.AddElement(oParagraph);

                                if (jpg != null)
                                {
                                    //oPdfPCell.AddElement(jpg);
                                    jpg = null;
                                }
                                oNewPdfPTable.AddCell(oPdfPCell);
                            }
                            
                        }

                        document.Add(oNewPdfPTable);
                    }
                }
                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CreateFile_Acta_RegistroCivil(System.Data.DataTable TablaText, string HtmlPath, string PdfPath)
        {
            try
            {
                Document document = new Document(PageSize.A4, 55, 55, 20, 6);
                StreamReader oStreamReader = new StreamReader(HtmlPath, System.Text.Encoding.Default);

                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                IElement oIElement;
                Paragraph oParagraph = null;
                PdfPTable oPdfPTable;
                PdfPRow oPdfPRow;
                PdfPCell oPdfPCell = null;
                Chunk oChunk;
                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;


                PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));
                document.Open();

                document.NewPage();

                objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);

                for (int k = 0; k < objects.Count; k++)
                {
                    oIElement = (IElement)objects[k];
                    if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                    {
                        oParagraph = new Paragraph();
                        oParagraph.Alignment = ((Paragraph)objects[k]).Alignment;

                        for (int z = 0; z < oIElement.Chunks.Count; z++)
                        {
                            strContent = ReplaceTexto(oIElement.Chunks[z].Content.ToString(), TablaText);

                            oParagraph.Add(new Chunk(strContent, oIElement.Chunks[z].Font));
                            oParagraph.Leading = 12;
                        }
                        document.Add(oParagraph);
                    }
                    else if (objects[k].GetType().FullName == "iTextSharp.text.pdf.PdfPTable")
                    {
                        oPdfPTable = (PdfPTable)objects[k];

                        PdfPTable oNewPdfPTable = new PdfPTable(oPdfPTable.NumberOfColumns);
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
                                oParagraph = new Paragraph();

                                for (int paragraph = 0; paragraph < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements.Count; paragraph++)
                                {
                                    for (int chunk = 0; chunk < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements[paragraph].Chunks.Count; chunk++)
                                    {
                                        if (!oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma1]") &
                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma2]") &
                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Logo]"))
                                        {
                                            strContent = ReplaceTexto(oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content, TablaText);
                                            oParagraph.Add(new Chunk(strContent, oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Font));

                                            aux = strContent.Length;

                                            if (aux > DimensionColumna[cell])
                                                DimensionColumna[cell] = aux;
                                            if (oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.ToUpper().Equals("[CONSULADO]"))
                                                oParagraph.Leading = 6;
                                            else
                                                oParagraph.Leading = 12;
                                        }
                                        else
                                        {
                                            if (oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content == "[Logo]")
                                            {
                                                foreach (DataRow dr in TablaText.Rows)
                                                {
                                                    if (dr["strCadenaBuscar"].ToString().ToUpper() == "[Logo]".ToUpper())
                                                        imgFirma2 = new MyBasePage().Ruta_Logo();
                                                }
                                                if (imgFirma2 != "")
                                                {
                                                    //jpg = iTextSharp.text.Image.GetInstance(imgFirma2);
                                                    //jpg.ScaleToFit(137.0F, 60.0F);
                                                    //jpg.SpacingBefore = 5.0F;
                                                    //jpg.SpacingAfter = 1.0F;
                                                    //jpg.Alignment = Element.ALIGN_LEFT;

                                                    jpg = iTextSharp.text.Image.GetInstance(imgFirma2);
                                                    jpg.SetAbsolutePosition(40f, document.PageSize.Height - 100);
                                                    jpg.ScaleAbsoluteHeight(85f);
                                                    jpg.ScaleAbsoluteWidth(40f);
                                                    document.Add(jpg);

                                                }
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

                        document.Add(oNewPdfPTable);
                    }
                }
                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateFilePDFExtraProtocolar(System.Data.DataTable TablaText, string HtmlPath, string PdfPath, string imgServerPAth)
        {
            try
            {
                if (!File.Exists(HtmlPath)) 
                    return;
                if (File.Exists(PdfPath)) 
                    File.Delete(PdfPath);

                Document document = new Document(PageSize.A4, 80, 80, 100, 80);
                StreamReader oStreamReader = new StreamReader(HtmlPath, System.Text.Encoding.Default);



                iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

                IElement oIElement;
                Paragraph oParagraph = null;
                PdfPTable oPdfPTable;
                PdfPRow oPdfPRow;
                PdfPCell oPdfPCell = null;
                Chunk oChunk;
                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                FontFactory.RegisterDirectories();
                PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));
                document.Open();

                document.NewPage();

                objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);



                Image img = Image.GetInstance(imgServerPAth);
                img.SetAbsolutePosition(20, document.PageSize.Height - img.Height-20);
                document.Add(img);

                for (int k = 0; k < objects.Count; k++)
                {
                    oIElement = (IElement)objects[k];
                    if (objects[k].GetType().FullName == "iTextSharp.text.Paragraph")
                    {
                        oParagraph = new Paragraph();
                        oParagraph.Alignment = ((Paragraph)objects[k]).Alignment;

                        for (int z = 0; z < oIElement.Chunks.Count; z++)
                        {
                            strContent = ReplaceTexto(oIElement.Chunks[z].Content.ToString(), TablaText);

                            oParagraph.Add(new Chunk(strContent, oIElement.Chunks[z].Font));
                            oParagraph.SetLeading(0.0f, 1.5f);
                        }
                        document.Add(oParagraph);
                    }
                    else if (objects[k].GetType().FullName == "iTextSharp.text.pdf.PdfPTable")
                    {
                        oPdfPTable = (PdfPTable)objects[k];

                        PdfPTable oNewPdfPTable = new PdfPTable(oPdfPTable.NumberOfColumns);
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
                                oParagraph = new Paragraph();

                                for (int paragraph = 0; paragraph < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements.Count; paragraph++)
                                {
                                    for (int chunk = 0; chunk < oPdfPTable.Rows[row].GetCells()[cell].CompositeElements[paragraph].Chunks.Count; chunk++)
                                    {
                                        if (!oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma1]") &
                                            !oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content.Equals("[Firma2]"))
                                        {
                                            strContent = ReplaceTexto(oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Content, TablaText);
                                            oParagraph.Add(new Chunk(strContent, oPdfPCell.CompositeElements[paragraph].Chunks[chunk].Font));

                                            aux = strContent.Length;

                                            if (aux > DimensionColumna[cell])
                                                DimensionColumna[cell] = aux;
                                            oParagraph.Leading = 12;
                                        }
                                        else
                                        {
                                            //otro texto para las imagenes en caso de poner el sello, si no 
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
    }
}

