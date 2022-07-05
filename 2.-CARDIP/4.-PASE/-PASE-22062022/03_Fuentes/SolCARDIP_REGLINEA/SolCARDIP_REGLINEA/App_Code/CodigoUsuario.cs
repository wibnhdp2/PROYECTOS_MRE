using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessLogic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Net;
using System.Globalization;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;
// PDF --------------------------------------
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
// ------------------------------------------

[Serializable]

public class CodigoUsuario
{
    brGeneral obrGeneral = new brGeneral();
    public char generarCaracter()
    {
        Random oAzar = new Random();
        int n = oAzar.Next(26) + 65;
        System.Threading.Thread.Sleep(15);
        return ((char)n);
    }

    public beGrafico obtenerGrafico(string cod, int width, int left)
    {
        beGrafico obeGrafico = new beGrafico();
        #region Random Color
        var arrColor1 = new List<string> { "#E7ECF2", "#7C8EA6", "#9CBAC4", "#CAE5D6", "#E5E4CA", "#B28F8A", "#C6B5C6" };
        var arrColor2 = new List<string> { "#21497C", "#303A46", "#057398", "#437A5B", "#95936B", "#90493F", "#766876" };
        Random colorRandom = new Random();
        int indexColor1 = colorRandom.Next(arrColor1.Count);
        int indexColor2 = colorRandom.Next(arrColor1.Count);
        string color1 = arrColor1[indexColor1];
        string color2 = arrColor2[indexColor2];
        Color _color1 = ColorTranslator.FromHtml("#7b012a");
        Color _color2 = ColorTranslator.FromHtml("#7b012a");
        #endregion
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, 80);
        LinearGradientBrush deg = new LinearGradientBrush(rect, _color1, _color2, LinearGradientMode.Vertical);
        Bitmap bmp = new Bitmap(width, 80);
        Graphics grafico = Graphics.FromImage(bmp);
        grafico.FillRectangle(deg, rect);
        StringBuilder sb = new StringBuilder();
        if (cod.Equals(""))
        {
            for (int i = 0; i < 5; i++)
            {
                sb.Append(generarCaracter());
            }
        }
        else
        {
            sb.Append(cod);
        }
        //ViewState["CodCaptcha"] = sb.ToString();
        string textoCaptcha = separarTexto(sb.ToString());
        grafico.DrawString(textoCaptcha, new System.Drawing.Font("Consolas", 30, FontStyle.Underline), Brushes.White, left, 10);
        MemoryStream ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);
        byte[] buffer = ms.ToArray();
        ms.Close();
        obeGrafico.buffer = buffer;
        obeGrafico.sb = sb;
        obeGrafico.codCaptcha = sb.ToString();
        return (obeGrafico);
        //imgCaptcha.Src = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(buffer));
        //txtCaptcha.Text = sb.ToString();
    }

    string separarTexto(string txtSB)
    {
        string texto = "";
        int len = txtSB.Length;
        for (int i = 0; i <= len - 1; i++)
        {
            texto = texto + txtSB.Substring(i, 1) + " ";
        }
        return (texto);
    }

    public string obtenerIP()
    {
        string strHostName = Dns.GetHostName();
        IPHostEntry IPEntry = Dns.GetHostEntry(strHostName);

        string strDireccionIP = IPEntry.AddressList.GetValue(1).ToString();
        return strDireccionIP;
    }

    public void colorCeldas(string colorCeldaHex, string colorTextoHex, object sender, GridViewRowEventArgs e)
    {
        GridView gridView = new GridView();
        Color _colorCelda = System.Drawing.ColorTranslator.FromHtml(colorCeldaHex);
        Color _colorTexto = System.Drawing.ColorTranslator.FromHtml(colorTextoHex);
        for (int n = 0; n < e.Row.Cells.Count; n++)
        {
            e.Row.Cells[n].BackColor = _colorCelda;
            e.Row.Cells[n].ForeColor = _colorTexto;
            e.Row.Cells[n].BorderStyle = BorderStyle.Solid;
            e.Row.Cells[n].BorderWidth = 1;
            e.Row.Cells[n].BorderColor = _colorTexto;
        }
    }

    public bePais obtenerDatosPais(short PaisId, List<bePais> listaPaises)
    {
        bePais obePais = new bePais();
        try
        {
            if (listaPaises.Count > 0)
            {
                obePais = listaPaises.Find(x => x.Paisid == PaisId);
            }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (obePais);
    }
   
    public List<beUbicaciongeografica> obtenerListaUbiGeo(string listaNro, string codigoUbigeo01, string codigoUbigeo02, List<beUbicaciongeografica> listaUbigeo)
    {
        List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
        switch (listaNro)
        {
            case "02":
                lbeUbicaciongeografica = listaUbigeo.FindAll(x => x.Ubi01.Equals(codigoUbigeo01) | x.Ubi02.Equals("00"));
                break;
            case "03":
                lbeUbicaciongeografica = listaUbigeo.FindAll(x => x.Ubi01.Equals(codigoUbigeo01) & x.Ubi02.Equals(codigoUbigeo02) | x.Ubi03.Equals("00"));
                break;
        }
        return (lbeUbicaciongeografica);
    }
    public List<beOficinaconsularExtranjera> obtenerOficinasConsularesExtranjeras(short CategoriaId, List<beOficinaconsularExtranjera> ListaOficinas)
    {
        List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();
        try
        {
            lbeOficinaconsularExtranjera = ListaOficinas.FindAll(x => x.Categoriaid == CategoriaId);
        }
        catch (Exception ex)
        {
            lbeOficinaconsularExtranjera = null;
            obrGeneral.grabarLog(ex);
        }
        return (lbeOficinaconsularExtranjera);
    }
    public List<beCalidadMigratoria> obtenerListaTitularDependiente(short Referencia, short TitularDependiente, short Genero, List<beCalidadMigratoria> listaCalidad)
    {
        List<beCalidadMigratoria> lbeCalidadMigratoria = new List<beCalidadMigratoria>();
        try
        {
            if (Genero > 0)
            {
                lbeCalidadMigratoria = listaCalidad.FindAll(x => x.ReferenciaId == Referencia & x.FlagTitularDependiente == TitularDependiente & x.GeneroId == Genero | x.CalidadMigratoriaid == 0 | x.GeneroId == 0);
            }
            else
            {
                lbeCalidadMigratoria = listaCalidad.FindAll(x => x.ReferenciaId == Referencia & x.FlagTitularDependiente == TitularDependiente & x.GeneroId > 0 | x.CalidadMigratoriaid == 0);
            }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (lbeCalidadMigratoria);
    }
    public string detectarCaracterEspecial(string cadena)
    {
        string cadenaConvertida = "";
        string[] arrCodigos = new string[] { "&#193;", "&#201;", "&#205;", "&#209;", "&#211;", "&#218;", "&#220;", "&#191;", "&#225;", "&#233;", "&#237;", "&#241;", "&#243;", "&#250;", "&#252;", "&#161;" };
        string[] arrLetras = new string[] { "Á", "É", "Í", "Ñ", "Ó", "Ú", "Ü", "¿", "á", "é", "í", "ñ", "ó", "ú", "ü", "¡" };
        for (int n = 0; n <= arrCodigos.Length - 1; n++)
        {
            cadenaConvertida = cadena.Replace(arrCodigos[n], arrLetras[n]);
            cadena = cadenaConvertida;
        }
        return cadenaConvertida;
    }

    public string LetraCapital(string CadenaTexto)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CadenaTexto);
    }

    public bool validarExtensionArchivo(string extensionValida, FileInfo archivoEvaluar)
    {
        bool exito = (archivoEvaluar.Extension.Equals(extensionValida) ? true : false);
        return exito;
    }

    public bool validarPesoArchivo(string pesoValido, FileUpload archivoEvaluar)
    {
        double pesoValidoConvert = double.Parse(pesoValido);
        double decFile = Math.Round((archivoEvaluar.FileContent.Length / 1024f) / 1024f, 2);
        bool exito = (decFile <= pesoValidoConvert ? true : false);
        return exito;
    }

    public bool validarPesoArchivo(string pesoValido, double PesoarchivoEvaluar)
    {
        double pesoValidoConvert = double.Parse(pesoValido);
        double decFile = Math.Round((PesoarchivoEvaluar / 1024f) / 1024f, 2);
        bool exito = (decFile <= pesoValidoConvert ? true : false);
        return exito;
    }
    public bool createDirectory(string Path)
    {
        bool exito = false;
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
        exito = Directory.Exists(Path);
        return exito;
    }

    public string getPathAdjuntos()
    {
        DateTime DtNow = DateTime.Now;
        string rutaAdjuntos = "error";
        rutaAdjuntos = obrGeneral.rutaAdjuntos + @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\";

        return (rutaAdjuntos);
    }

    public string getFileName()
    {
        DateTime DtNow = DateTime.Now;
        string fileName = "error";
        fileName = "RL" + DtNow.Year.ToString() + DtNow.Month.ToString("D2") + DtNow.Day.ToString("D2") + DtNow.Hour.ToString("D2") + DtNow.Minute.ToString("D2") +
        DtNow.Second.ToString("D2") + DtNow.Millisecond.ToString() + obrGeneral.fileExtFotografia;
        return (fileName);
    }

    public void DisableControls(Control parent, bool State)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = State;
            }
            if (c is TextBox)
            {
                ((TextBox)(c)).Enabled = State;
            }
            if (c is ImageButton)
            {
                ((ImageButton)(c)).Enabled = State;
            }
            if (c is FileUpload)
            {
                ((FileUpload)(c)).Enabled = State;
            }
            if (c is CheckBox)
            {
                ((CheckBox)(c)).Enabled = State;
            }
            DisableControls(c, State);
        }
    }

    public bool guardarImagen(string SavePath, string fileName, string base64String, FileUpload fileUpLoadImage)
    {
        bool exito = false;
        try
        {
            exito = createDirectory(SavePath);
            if (exito)
            {
                if (!base64String.Equals(""))
                {
                    // CONVIERTE A STREAM -------------------------------------------------------
                    //string base64String = (string)Session["tempImagen"];
                    byte[] imgByte = Convert.FromBase64String(base64String);
                    // VALIDA EL TIPO DE ARCHIVO ------------------------------------------------
                    if (fileUpLoadImage != null)
                    {
                        //FileUpload fileUpLoadImage = (FileUpload)Session["fileUploadImage"];
                        Regex reg = new Regex(@"(?i).*\.(gif|jpe?g|png|tif)$");
                        string uFile = fileUpLoadImage.FileName;
                        if (reg.IsMatch(uFile))
                        {
                            using (MemoryStream ms = new MemoryStream(imgByte))
                            {
                                Bitmap b = (Bitmap)Bitmap.FromStream(ms);
                                b.Save(SavePath + fileName, ImageFormat.Jpeg);
                                ms.Close();
                                if (File.Exists(SavePath + fileName)) { exito = true; }
                                else { exito = false; }
                            }
                        }
                        else
                        {
                            exito = false;
                        }
                    }
                    else
                    {
                        exito = false;
                    }
                }
                else
                {
                    exito = false;
                }
            }
        }
        catch (Exception ex)
        {
            exito = false;
            obrGeneral.grabarLog(ex);
        }
        return (exito);
    }

    protected PdfPCell nuevaCelda(string definicion, string dato, iTextSharp.text.Font fuente, int alineacion, int colspan, float border)
    {
        PdfPCell cell = new PdfPCell(new Phrase(definicion + dato, fuente));
        cell.BorderWidth = border;
        cell.Colspan = colspan;
        cell.HorizontalAlignment = alineacion;
        return (cell);
    }

    protected PdfPCell nuevaCeldaImagen(iTextSharp.text.Image imagenJpg, int alineacion, int colspan, float border, float iWidth, float iHeight)
    {
        PdfPCell cell = new PdfPCell(imagenJpg);
        imagenJpg.ScaleAbsolute(iWidth, iHeight);
        cell.BorderWidth = border;
        cell.Colspan = colspan;
        cell.HorizontalAlignment = alineacion;
        return (cell);
    }

    private void WriteImagen(iTextSharp.text.Document objPdfDocument, string strFileImage, int posX, int posY, bool imgDeProyecto = true, float scalaWidth = 100, float scalaHeigth = 250)
    {
        try
        {
            iTextSharp.text.Image objImagePdf;
            string physicalPath = "";
            if (imgDeProyecto)
            {
                physicalPath = System.Web.HttpContext.Current.Server.MapPath(strFileImage);
            }
            else
            {
                physicalPath = strFileImage;
            }
            // Crea la imagen
            objImagePdf = iTextSharp.text.Image.GetInstance(physicalPath);
            // Cambia el tamaño de la imagen
            objImagePdf.ScaleToFit(scalaWidth, scalaHeigth);
            // Se indica que la imagen debe almacenarse como fondo
            objImagePdf.Alignment = iTextSharp.text.Image.UNDERLYING;
            // Coloca la imagen en una posición absoluta
            objImagePdf.SetAbsolutePosition(posX, posY);
            // Imprime la imagen como fondo de página
            objPdfDocument.Add(objImagePdf);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public byte[] GenerarAnexosPDF(beRegistroLinea obeRegistroLinea)
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();
        iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
        doc.SetMargins(50, 50, 50, 50);
        MemoryStream ms = new MemoryStream();
        PdfWriter pdfw = PdfWriter.GetInstance(doc, ms);
        doc.Open();

        iTextSharp.text.Chunk saltoLinea1 = new iTextSharp.text.Chunk("\n");
        iTextSharp.text.Paragraph pSaltoLinea1 = new iTextSharp.text.Paragraph();
        pSaltoLinea1.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
        pSaltoLinea1.Add(saltoLinea1);

        iTextSharp.text.Chunk descripcion = new iTextSharp.text.Chunk("EL ANEXO NO SE PUEDE VISUALIZAR", iTextSharp.text.FontFactory.GetFont("VERDANA", 14, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Paragraph pdescripcion = new iTextSharp.text.Paragraph();
        pdescripcion.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
        pdescripcion.Add(descripcion);

        string FolderPath = obrGeneral.rutaAdjuntos;
        if (obeRegistroLinea.DpRutaAdjunto.Length > 0)
        {
            iTextSharp.text.Chunk titulo = new iTextSharp.text.Chunk("ANEXO 1: FOTOGRAFÍA", iTextSharp.text.FontFactory.GetFont("VERDANA", 14, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
            iTextSharp.text.Paragraph ptitulo = new iTextSharp.text.Paragraph();
            ptitulo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            ptitulo.Add(titulo);
            if (File.Exists(FolderPath + obeRegistroLinea.DpRutaAdjunto))
            {
                WriteImagen(doc, FolderPath + obeRegistroLinea.DpRutaAdjunto, 150, Convert.ToInt32(doc.PageSize.Width) - 200, false, 300, 300);
            }
            
            doc.Add(titulo);

        }
        if (obeRegistroLinea.DpRutaFirma.Length > 0)
        {
            iTextSharp.text.Chunk tituloAnexo2 = new iTextSharp.text.Chunk("ANEXO 2: FIRMA", iTextSharp.text.FontFactory.GetFont("VERDANA", 14, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
            iTextSharp.text.Paragraph ptituloAnexo2 = new iTextSharp.text.Paragraph();
            ptituloAnexo2.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            ptituloAnexo2.Add(tituloAnexo2);

            doc.NewPage();
            if (File.Exists(FolderPath + obeRegistroLinea.DpRutaFirma))
            {
                WriteImagen(doc, FolderPath + obeRegistroLinea.DpRutaFirma, 150, Convert.ToInt32(doc.PageSize.Width) - 200, false, 300, 300);
            }
            doc.Add(tituloAnexo2);
        }
        if (obeRegistroLinea.DpRutaPasaporte.Length > 0)
        {
            iTextSharp.text.Chunk tituloAnexo3 = new iTextSharp.text.Chunk("ANEXO 3: COPIA DE PASAPORTE", iTextSharp.text.FontFactory.GetFont("VERDANA", 14, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
            iTextSharp.text.Paragraph ptituloAnexo3 = new iTextSharp.text.Paragraph();
            ptituloAnexo3.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            ptituloAnexo3.Add(tituloAnexo3);

            doc.NewPage();
            if (File.Exists(FolderPath + obeRegistroLinea.DpRutaPasaporte))
            {
                WriteImagen(doc, FolderPath + obeRegistroLinea.DpRutaPasaporte, 150, Convert.ToInt32(doc.PageSize.Width) - 200, false, 300, 300);
            }
            doc.Add(tituloAnexo3);
        }

        doc.Close();
        pdfw.Close();

        byte[] pdfByte = ms.ToArray();

        if (obeRegistroLinea.DpRutaDenunciaPolicial.Length > 0)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(FolderPath + obeRegistroLinea.DpRutaDenunciaPolicial);
            pdfByte = Unir2PdfsByte(pdfByte, bytes); ;
        }
        return pdfByte;
    }
    public byte[] GenerarPDF(beRegistroLinea obeRegistroLinea,string nroCarnet = "--------",string tipoCargo="",
        string nombreTitular ="",string apePatTitular = "",string apeMatTitular = "",string calidadMigratoriaTitular = "",string motivo = "")
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();
        iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
        doc.SetMargins(50, 50, 50, 50);
        MemoryStream ms = new MemoryStream();
        PdfWriter pdfw = PdfWriter.GetInstance(doc, ms);
        
        //BaseFont Font = BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
        //PdfContentByte cb = pdfw.DirectContent; 
        doc.Open();
        WriteImagen(doc, "../../Imagenes/Reporte_v2.jpg",5, 50, true, 960, 750);

    


        int Fila1_Y = 613;
        int Fila2_Y = 586;
        int Fila3_Y = 559;
        int Fila4_Y = 528;
        int Fila5_Y = 501;
        int Fila6_Y = 452;
        int Fila7_Y = 425;
        int Fila8_Y = 382;
        int Fila9_Y = 340;
        int Fila9_1_Y = 312;
        int Fila10_Y = 283;
        int Fila11_Y = 250;
        int Fila12_Y = 240;
        int Fila13_Y = 230;
        int Fila14_Y = 220;
        int Fila15_Y = 115;
        
        int Columna1_X = 52;
        int Columna2_X = 225;
        int Columna3_X = 407;
        int Columna1_1_X = 63;
        short tamanoLetra = 9;

        EscribirTexto(pdfw, obeRegistroLinea.ConTipoEmision.ToUpper(), Columna1_1_X, 690, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.NumeroRegLinea.ToUpper(), Convert.ToInt32(doc.PageSize.Width) - 100, Convert.ToInt32(doc.PageSize.Height) - 80, tamanoLetra);
        EscribirTexto(pdfw, nroCarnet.ToUpper(), Columna1_1_X, 657, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.DpPrimerApellido.ToUpper(), Columna1_X, Fila1_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.DpSegundoApellido.ToUpper(), Columna2_X, Fila1_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.DpNombres.ToUpper(), Columna3_X, Fila1_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.ConFechaNacimiento.ToUpper(), Columna1_X, Fila2_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.ConGenero.ToUpper(), Columna2_X, Fila2_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.ConEstadoCivil.ToUpper(), Columna3_X, Fila2_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.ConTipoDocIdent.ToUpper(), Columna1_X, Fila3_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.DpNumeroDocIdentidad.ToUpper(), Columna2_X, Fila3_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.ConPais.ToUpper(), Columna3_X, Fila3_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.DpDomicilioPeru.ToUpper(), Columna1_X, Fila4_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.ConDepartamento.ToUpper(), Columna1_X, Fila5_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.ConProvincia.ToUpper(), Columna2_X, Fila5_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.ConDistrito.ToUpper(), Columna3_X, Fila5_Y, tamanoLetra);


        EscribirTexto(pdfw, apePatTitular.ToUpper(), Columna1_X, Fila6_Y, tamanoLetra);
        EscribirTexto(pdfw, apeMatTitular.ToUpper(), Columna2_X, Fila6_Y, tamanoLetra);
        EscribirTexto(pdfw, nombreTitular.ToUpper(), Columna3_X, Fila6_Y, tamanoLetra);

        EscribirTexto(pdfw, calidadMigratoriaTitular.ToUpper(), Columna1_X, Fila7_Y, tamanoLetra);
        if (calidadMigratoriaTitular.Length > 0)
        {
            EscribirTexto(pdfw, "DEPENDIENTE", Columna2_X, Fila7_Y, tamanoLetra);
        }


        EscribirTexto(pdfw, tipoCargo.ToUpper(), Columna1_X, Fila8_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.InCargoNombre.ToUpper(), Columna2_X, Fila8_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.InNombreInstitucion.ToUpper(), Columna1_X, Fila9_Y, tamanoLetra);


        EscribirTexto(pdfw, obeRegistroLinea.InPersonaContacto.ToUpper(), Columna1_X, Fila9_1_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.InCargoContacto.ToUpper(), Columna2_X, Fila9_1_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.InCorreoElectronico.ToUpper(), Columna1_X, Fila10_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.InNumeroTelefono.ToUpper(), Columna3_X, Fila10_Y, tamanoLetra);

        if (obeRegistroLinea.DpRutaAdjunto.Length > 0)
        {
            EscribirTexto(pdfw, "Fotografía: SI", Columna1_X, Fila11_Y, tamanoLetra);
        }
        else {
            EscribirTexto(pdfw, "Fotografía: NO", Columna1_X, Fila11_Y, tamanoLetra);
        }

        if (obeRegistroLinea.DpRutaFirma.Length > 0)
        {
            EscribirTexto(pdfw, "Firma: SI", Columna1_X, Fila12_Y, tamanoLetra);
        }
        else
        {
            EscribirTexto(pdfw, "Firma: NO", Columna1_X, Fila12_Y, tamanoLetra);
        }

        if (obeRegistroLinea.DpRutaPasaporte.Length > 0)
        {
            EscribirTexto(pdfw, "Pasaporte: SI", Columna1_X, Fila13_Y, tamanoLetra);
        }
        else
        {
            EscribirTexto(pdfw, "Pasaporte: NO", Columna1_X, Fila13_Y, tamanoLetra);
        }

        if (obeRegistroLinea.DpRutaDenunciaPolicial.Length > 0)
        {
            EscribirTexto(pdfw, "Denuncia Policial: SI", Columna1_X, Fila14_Y, tamanoLetra);
        }
        else
        {
            //EscribirTexto(pdfw, "Denuncia Policial: NO", Columna1_X, Fila14_Y, tamanoLetra);
        }

        if (motivo == "DETERIORO")
        {
            EscribirTexto(pdfw, "AL RECOGER EL DUPLICADO DE CARNÉ DE IDENTIDAD, DEBERÁ DEJAR EL DOCUMENTO DETERIORADO", Columna1_1_X, Fila15_Y, tamanoLetra);
        }

        //iTextSharp.text.Chunk NumeroCarnet = new iTextSharp.text.Chunk("------", iTextSharp.text.FontFactory.GetFont("VERDANA", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        //iTextSharp.text.Paragraph pNumeroCarnet = new iTextSharp.text.Paragraph();
        //pNumeroCarnet.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
        //pNumeroCarnet.Add(NumeroCarnet);


       //   obeRegistroLinea.DpCorreoElectronico, obeRegistroLinea.DpNumeroTelefono

        iTextSharp.text.html.simpleparser.HTMLWorker worker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc);
        worker.Parse(new StringReader(html));
        doc.Close();
        pdfw.Close();

        byte[] pdfByte = ms.ToArray();

        return pdfByte;
    }
    private void EscribirTexto(iTextSharp.text.pdf.PdfWriter pdfw,string Texto,int ubicacionX = 0, int UbicacionY = 0,short tamanoLetra = 7)
    {
        PdfContentByte cb = pdfw.DirectContent; 
        cb.BeginText();
        BaseFont f_cn = BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
        cb.SetFontAndSize(f_cn, tamanoLetra);
        cb.SetTextMatrix(ubicacionX, UbicacionY); //(xPos, yPos) 

        cb.ShowText(Texto); 
        cb.EndText();
    }
    

    //public byte[] crearPDF1(beCarneIdentidad obeCarneIdentidad, string userComp)
    public byte[] crearPDF1(beRegistroLinea obeRegistroLinea)
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();

        Document doc = new Document(PageSize.A4);
        doc.SetMargins(10, 10, 0, 0);
        MemoryStream ms = new MemoryStream();
        PdfWriter pdfw = PdfWriter.GetInstance(doc, ms);
        doc.Open();
        // CREACION ---------------------------------------------------------------------------------------
        Chunk saltoLinea = new Chunk("\n");
        Paragraph pSaltoLinea = new Paragraph();
        pSaltoLinea.Alignment = Element.ALIGN_LEFT;
        pSaltoLinea.Add(saltoLinea);
        #region Fuentes
        FontFactory.RegisterDirectories();
        iTextSharp.text.Font fuenteSmall = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 6, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteSmallBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteMedium = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 7, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteMediumBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        #endregion
        PdfPCell cellVacio = nuevaCelda("", "", fuenteMediumBold, Element.ALIGN_LEFT, 5, 0);
        PdfPCell cellCuerpo_puntos = nuevaCelda(":", "", fuenteMediumBold, Element.ALIGN_CENTER, 1, 0);
        #region Cabecera
        PdfPTable tableCab = new PdfPTable(4);
        tableCab.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        string fechaHora = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString("D2").ToString() + ":" + DateTime.Now.Minute.ToString("D2").ToString() + ":" + DateTime.Now.Second.ToString("D2").ToString();
        PdfPCell cell3 = nuevaCelda("Fecha / Hora Impresión:  ", fechaHora, fuenteSmall, Element.ALIGN_LEFT, 3, 0);
        tableCab.AddCell(cell3);
        PdfPCell cell6 = nuevaCelda("Numero de Solicitud:  ", obeRegistroLinea.NumeroRegLinea, fuenteSmall, Element.ALIGN_CENTER, 1, 0);
        tableCab.AddCell(cell6);
        PdfPCell cell7 = nuevaCelda("Tipo de Emisión:  ", obeRegistroLinea.ConTipoEmision.ToUpper(), fuenteSmall, Element.ALIGN_RIGHT, 4, 0);
        tableCab.AddCell(cell7);

        #endregion
        #region Titulo
        PdfPTable tableTitulo = new PdfPTable(1);
        tableTitulo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cellTitulo1 = nuevaCelda("REPORTE DE REGISTRO EN LINEA", "", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo1);
        PdfPCell cellTitulo2 = nuevaCelda("Solicitud en Linea - Carné de Identidad", "", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo2);
        PdfPCell cellTitulo3 = nuevaCelda("Relacion de Dependencia:   ", obeRegistroLinea.ConTitDep.ToUpper(), fuenteMediumBold, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo3);
        #endregion
        #region Cuerpo1
        PdfPTable tableCuerpo = new PdfPTable(2);
        tableCuerpo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widths = new float[] { 50f, 50f };
        tableCuerpo.SetWidths(widths);

        //PdfPCell cellTableCuerpo1 = nuevaCelda("", "", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        //tableCuerpo.AddCell(cellTableCuerpo1);
        //PdfPCell cellTableCuerpo2 = nuevaCelda("", "", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        //tableCuerpo.AddCell(cellTableCuerpo2);

        #region TablaCuerpo1A
        PdfPTable tableCuerpoA = new PdfPTable(3);
        tableCuerpoA.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsA = new float[] { 30f, 5f, 65f };
        tableCuerpoA.SetWidths(widthsA);

        PdfPCell cellTableASub1 = nuevaCelda("Datos Personales", "", fuenteMediumBold, Element.ALIGN_LEFT, 3, 0);
        tableCuerpoA.AddCell(cellTableASub1);

        string[] arrNCA = new string[] { "Primer Apellido", "Segundo Apellido" , "Nombres", "Fecha de Nacimiento" , "Estado Civil", "Sexo" , "Documento de Identidad", "Numero" , "Nacionalidad", "Domicilio en Perú" , "Departamento", "Provincia" , "Distrito", "Correo Electronico" , "Numero de Telefono" };
        string[] arrDatos1 = new string[] { obeRegistroLinea.DpPrimerApellido, obeRegistroLinea.DpSegundoApellido, obeRegistroLinea.DpNombres, obeRegistroLinea.ConFechaNacimiento, obeRegistroLinea.ConEstadoCivil, obeRegistroLinea.ConGenero, obeRegistroLinea.ConTipoDocIdent, obeRegistroLinea.DpNumeroDocIdentidad, obeRegistroLinea.ConPais, obeRegistroLinea.DpDomicilioPeru, obeRegistroLinea.ConDepartamento, obeRegistroLinea.ConProvincia, obeRegistroLinea.ConDistrito, obeRegistroLinea.DpCorreoElectronico, obeRegistroLinea.DpNumeroTelefono };

        int contador = 0;
        foreach (string str in arrNCA)
        {
            PdfPCell cellTableA1 = nuevaCelda(str, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
            tableCuerpoA.AddCell(cellTableA1);
            tableCuerpoA.AddCell(cellCuerpo_puntos);
            PdfPCell cellTableA2 = nuevaCelda("", arrDatos1[contador], fuenteSmall, Element.ALIGN_LEFT, 1, 0);
            tableCuerpoA.AddCell(cellTableA2);
            contador++;
        }
        #endregion
        #region TablaCuerpo1B
        PdfPTable tableCuerpoB = new PdfPTable(3);
        tableCuerpoB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsB = new float[] { 30f, 5f, 65f };
        tableCuerpoB.SetWidths(widthsB);

        PdfPCell cellTableASub2 = nuevaCelda("Datos de Institución, Funciones y/o Cargo", "", fuenteMediumBold, Element.ALIGN_LEFT, 3, 0);
        tableCuerpoB.AddCell(cellTableASub2);

        string[] arrNCB = new string[] { "Cargo", "Institución", "Contacto de Institucion", "Cargo de Contacto", "Correo Electronico", "Numero de Telefono", "Foto Cargada" };
        string[] arrDatos2 = new string[] { obeRegistroLinea.InCargoNombre, obeRegistroLinea.InNombreInstitucion, obeRegistroLinea.InPersonaContacto, obeRegistroLinea.InCargoContacto, obeRegistroLinea.InCorreoElectronico, obeRegistroLinea.InNumeroTelefono, (obeRegistroLinea.DpRutaAdjunto.Equals("") ? "[NO SELECCIONADO]" : "SI") };

        contador = 0;
        foreach (string str in arrNCB)
        {
            PdfPCell cellTableB1 = nuevaCelda(str, "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
            tableCuerpoB.AddCell(cellTableB1);
            tableCuerpoB.AddCell(cellCuerpo_puntos);
            PdfPCell cellTableB2 = nuevaCelda("", arrDatos2[contador], fuenteMedium, Element.ALIGN_LEFT, 1, 0);
            tableCuerpoB.AddCell(cellTableB2);
            contador++;
        }
        #endregion
        tableCuerpo.AddCell(tableCuerpoA);
        tableCuerpo.AddCell(tableCuerpoB);
        #endregion
        #region Cuerpo2
        PdfPTable tableCuerpo2 = new PdfPTable(1);
        tableCuerpo2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widths2 = new float[] { 100f };
        tableCuerpo2.SetWidths(widths2);

        PdfPCell cellTableFirma1 = nuevaCelda("Registro de Firma del Solicitante", "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo2.AddCell(cellTableFirma1);
        PdfPCell cellTableFirma2 = nuevaCelda("- Firmar en el centro del recuadro", "", fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo2.AddCell(cellTableFirma2);
        PdfPCell cellTableFirma3 = nuevaCelda("- Utilice bolígrafo tinta negra", "", fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo2.AddCell(cellTableFirma3);
        PdfPCell cellTableFirma4 = nuevaCelda("- Evite incluir sellos ni tocar los bordes", "", fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo2.AddCell(cellTableFirma4);

        iTextSharp.text.Image jpgFoto;
        string rutaOrigen = System.Threading.Thread.GetDomain().BaseDirectory;
        jpgFoto = iTextSharp.text.Image.GetInstance(rutaOrigen + "Imagenes\\cuadroFirma.jpg");

        //PdfPCell cellFoto = nuevaCeldaImagen(jpgFoto, Element.ALIGN_CENTER, 1, 0, 460f, 180f);
        PdfPCell cellFoto = nuevaCeldaImagen(jpgFoto, Element.ALIGN_CENTER, 1, 0, 460f, 160f);
        cellFoto.Rowspan = 10;
        tableCuerpo2.AddCell(cellFoto);
        #endregion
        #region Cuerpo3
        PdfPTable tableCuerpo3 = new PdfPTable(2);
        tableCuerpo3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widths3 = new float[] { 50f, 50f };
        tableCuerpo3.SetWidths(widths3);

        PdfPCell cellTableC3Sub = nuevaCelda("Para uso interno", "", fuenteSmallBold, Element.ALIGN_LEFT, 2, 0);
        tableCuerpo3.AddCell(cellTableC3Sub);
        PdfPCell cellTableC3A = nuevaCelda("", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0.1f);
        PdfPCell cellTableC3B = nuevaCelda("", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0.1f);


        PdfPTable tableCuerpo3A = new PdfPTable(1);
        float[] widths3A = new float[] { 100f };
        tableCuerpo3A.SetWidths(widths3A);
        PdfPCell cellTableC3A1 = nuevaCelda("Sello y firma de recepción", "", fuenteSmallBold, Element.ALIGN_CENTER, 1, 0);
        cellTableC3A1.FixedHeight = 100f;
        tableCuerpo3A.AddCell(cellTableC3A1);

        string tabs = new String('-', 52);

        PdfPCell cellTableC3A2 = nuevaCelda("Detalle", "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3A.AddCell(cellTableC3A2);
        PdfPCell cellTableC3A3 = nuevaCelda(tabs, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3A.AddCell(cellTableC3A3);
        PdfPCell cellTableC3A4 = nuevaCelda(tabs, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3A.AddCell(cellTableC3A4);
        PdfPCell cellTableC3A5 = nuevaCelda(tabs, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3A.AddCell(cellTableC3A5);
        cellTableC3A.AddElement(tableCuerpo3A);

        PdfPTable tableCuerpo3B = new PdfPTable(1);
        float[] widths3B = new float[] { 100f };
        tableCuerpo3B.SetWidths(widths3B);
        PdfPCell cellTableC3B1 = nuevaCelda("Resultado del proceso de evaluación", "", fuenteSmallBold, Element.ALIGN_CENTER, 1, 0);
        cellTableC3B1.FixedHeight = 100f;
        tableCuerpo3B.AddCell(cellTableC3B1);

        PdfPCell cellTableC3B2 = nuevaCelda("Detalle", "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3B.AddCell(cellTableC3B2);
        PdfPCell cellTableC3B3 = nuevaCelda(tabs, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3B.AddCell(cellTableC3B3);
        PdfPCell cellTableC3B4 = nuevaCelda(tabs, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3B.AddCell(cellTableC3B4);
        PdfPCell cellTableC3B5 = nuevaCelda(tabs, "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo3B.AddCell(cellTableC3B5);
        cellTableC3B.AddElement(tableCuerpo3B);


        tableCuerpo3.AddCell(cellTableC3A);
        tableCuerpo3.AddCell(cellTableC3B);

        #endregion

        #region Requisitos
        PdfPTable tableRequisitos = new PdfPTable(1);
        tableRequisitos.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthR = new float[] { 100f };
        tableRequisitos.SetWidths(widthR);

        PdfPCell cellTableRequisitos = nuevaCelda("Requisitos:", "", fuenteSmallBold, Element.ALIGN_LEFT, 1, 0);
        tableRequisitos.AddCell(cellTableRequisitos);
        string Requisito1 = "";
        string Requisito2 = "";
        if (obeRegistroLinea.ConTipoEmision == "DUPLICADO")
        {
            Requisito1 = "- Nota o carta, de la embajada, organización, entidad u otros según corresponda, en la que se señale el pedido de duplicado del Carné de identidad.";
            Requisito2 = "- Copia de la denuncia policial, ya sea por perdida, hurto o robo.";
        }
        if (obeRegistroLinea.ConTipoEmision == "RENOVACIÓN")
        {
            Requisito1 = "- Nota o carta, de la embajada, organización, entidad u otros según corresponda, en la que se señale el pedido de renovación, ya sea por nuevo plazo de permanencia o por actualización de otros datos del carné.";
            Requisito2 = "- Copia del pasaporte del/los solicitante(s) – página de datos biográficos.";
        }
        if (obeRegistroLinea.ConTipoEmision == "NUEVO")
        {
            Requisito1 = "- Considerar los requisitos señalados en la pagina según su calidad migratoria y adjuntarlos al resumen.";
        }
        if (Requisito1.Length > 0)
        {
            PdfPCell cellTableRequisitos1 = nuevaCelda(Requisito1, "", fuenteSmall, Element.ALIGN_LEFT, 1, 0);
            tableRequisitos.AddCell(cellTableRequisitos1);
        }
        if (Requisito2.Length > 0)
        {
            PdfPCell cellTableRequisitos2 = nuevaCelda(Requisito2, "", fuenteSmall, Element.ALIGN_LEFT, 1, 0);
            tableRequisitos.AddCell(cellTableRequisitos2);
        }        
        #endregion
        #region PiePagina
        PdfPTable tablePie = new PdfPTable(1);
        tablePie.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cellPie1 = nuevaCelda("Toda la información consignada en este documento tiene valor de Declaración Jurada", "", fuenteSmall, Element.ALIGN_CENTER, 1, 0);
        tablePie.AddCell(cellPie1);

        #endregion
        #region Armado Doc
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tableCab);
        doc.Add(pSaltoLinea);
        doc.Add(tableTitulo);
        doc.Add(pSaltoLinea);
        //doc.Add(tableCuerpo1);
        doc.Add(pSaltoLinea);
        doc.Add(tableCuerpo);
        doc.Add(pSaltoLinea);
        doc.Add(tableCuerpo2);
        doc.Add(pSaltoLinea);
        doc.Add(tableCuerpo3);
        doc.Add(pSaltoLinea);
        doc.Add(tableRequisitos);
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tablePie);
        #endregion

        HTMLWorker worker = new HTMLWorker(doc);
        worker.Parse(new StringReader(html));
        doc.Close();
        pdfw.Close();

        byte[] pdfByte = ms.ToArray();
        return (pdfByte);
    }

    public void Unir2PdfsByte(byte[] Documento1, byte[] Documento2, string RutaDocumentoResultado)
    {
        try
        {
            Stream target = new FileStream(RutaDocumentoResultado, FileMode.Create);
            // y el Documento que será el que almacene los PDF unidos
            MemoryStream ms = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            // Se utiliza un PDFCopy para unir las hojas de los PDF
            // Se instancian el MemoryStream que almacenará la información en cada PDF 
            using (PdfCopy copy = new PdfCopy(doc, target))
            {
                // Importante abrir el documento para poder modificarlo
                doc.Open();
                copy.SetLinearPageMode();
                PdfReader readerX = null;
                ms = new MemoryStream();


                // Se lee el primer archivo PDF
                PdfReader r = new PdfReader(Documento1);

                // Para poder modificar los campos se necesita trabajar con un PDFStamper
                PdfStamper stamp = new PdfStamper(r, ms);

                // Hay que cerrar el PDFStamper para que los cambios se reflejen en el PDF
                stamp.Close();

                // Se cierra el reader para que el archivo no quede amarrado a la aplicación
                r.Close();

                // ESTE ES EL TRUCO IMPORTANTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento modificado
                readerX = new PdfReader(ms.GetBuffer());

                // Se copian cada una de las páginas del nuevo PDFReader
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // NUEVAMENTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento
                readerX = new PdfReader(Documento2);

                // Se copian cada una de las páginas del nuevo PDFReader, las cuales se adicionarán al documento por debajo de las que ya se adicionaron.
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // Se ejecutan un par de acciones de limpieza
                if (readerX != null)
                {
                    readerX.Close();
                }
                ms.Dispose();

                // Se cierra el documento para que se termine correctamente
                if (doc != null)
                {
                    doc.Close();
                }

            }// Fin del using
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public byte[] Unir2PdfsByte(byte[] Documento1, byte[] Documento2)
    {
        try
        {
            byte[] mergedPdf = null; 
            //Stream target = new FileStream(RutaDocumentoResultado, FileMode.Create);
            MemoryStream msTarget = new MemoryStream();

            // y el Documento que será el que almacene los PDF unidos
            MemoryStream ms = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            // Se utiliza un PDFCopy para unir las hojas de los PDF
            // Se instancian el MemoryStream que almacenará la información en cada PDF 
            using (PdfCopy copy = new PdfCopy(doc, msTarget))
            {
                // Importante abrir el documento para poder modificarlo
                doc.Open();
                copy.SetLinearPageMode();
                PdfReader readerX = null;
                ms = new MemoryStream();


                // Se lee el primer archivo PDF
                PdfReader r = new PdfReader(Documento1);

                // Para poder modificar los campos se necesita trabajar con un PDFStamper
                PdfStamper stamp = new PdfStamper(r, ms);

                // Hay que cerrar el PDFStamper para que los cambios se reflejen en el PDF
                stamp.Close();

                // Se cierra el reader para que el archivo no quede amarrado a la aplicación
                r.Close();

                // ESTE ES EL TRUCO IMPORTANTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento modificado
                readerX = new PdfReader(ms.GetBuffer());

                // Se copian cada una de las páginas del nuevo PDFReader
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // NUEVAMENTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento
                readerX = new PdfReader(Documento2);

                // Se copian cada una de las páginas del nuevo PDFReader, las cuales se adicionarán al documento por debajo de las que ya se adicionaron.
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // Se ejecutan un par de acciones de limpieza
                if (readerX != null)
                {
                    readerX.Close();
                }
                ms.Dispose();

                // Se cierra el documento para que se termine correctamente
                if (doc != null)
                {
                    doc.Close();
                }

            }// Fin del using
            mergedPdf = msTarget.ToArray();
            return mergedPdf;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private byte[] Unir2PdfsArchivoyByte(string RutaDocumento1, byte[] Documento2)
    {
        try
        {
            byte[] mergedPdf = null;
            //Stream target = new FileStream(RutaDocumentoResultado, FileMode.Create);
            MemoryStream msTarget = new MemoryStream();

            // y el Documento que será el que almacene los PDF unidos
            MemoryStream ms = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            // Se utiliza un PDFCopy para unir las hojas de los PDF
            // Se instancian el MemoryStream que almacenará la información en cada PDF 
            using (PdfCopy copy = new PdfCopy(doc, msTarget))
            {
                // Importante abrir el documento para poder modificarlo
                doc.Open();
                copy.SetLinearPageMode();
                PdfReader readerX = null;
                ms = new MemoryStream();


                // Se lee el primer archivo PDF
                PdfReader r = new PdfReader(RutaDocumento1);

                // Para poder modificar los campos se necesita trabajar con un PDFStamper
                PdfStamper stamp = new PdfStamper(r, ms);

                // Hay que cerrar el PDFStamper para que los cambios se reflejen en el PDF
                stamp.Close();

                // Se cierra el reader para que el archivo no quede amarrado a la aplicación
                r.Close();

                // ESTE ES EL TRUCO IMPORTANTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento modificado
                readerX = new PdfReader(ms.GetBuffer());

                // Se copian cada una de las páginas del nuevo PDFReader
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // NUEVAMENTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento
                readerX = new PdfReader(Documento2);

                // Se copian cada una de las páginas del nuevo PDFReader, las cuales se adicionarán al documento por debajo de las que ya se adicionaron.
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // Se ejecutan un par de acciones de limpieza
                if (readerX != null)
                {
                    readerX.Close();
                }
                ms.Dispose();

                // Se cierra el documento para que se termine correctamente
                if (doc != null)
                {
                    doc.Close();
                }

            }// Fin del using
            mergedPdf = msTarget.ToArray();
            return mergedPdf;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Unir2PdfsArchivoyByte(string RutaDocumento1, byte[] Documento2, string RutaDocumentoResultado)
    {
        try
        {
            Stream target = new FileStream(RutaDocumentoResultado, FileMode.Create);
            // y el Documento que será el que almacene los PDF unidos
            MemoryStream ms = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            // Se utiliza un PDFCopy para unir las hojas de los PDF
            // Se instancian el MemoryStream que almacenará la información en cada PDF 
            using (PdfCopy copy = new PdfCopy(doc, target))
            {
                // Importante abrir el documento para poder modificarlo
                doc.Open();
                copy.SetLinearPageMode();
                PdfReader readerX = null;
                ms = new MemoryStream();


                // Se lee el primer archivo PDF
                PdfReader r = new PdfReader(RutaDocumento1);

                // Para poder modificar los campos se necesita trabajar con un PDFStamper
                PdfStamper stamp = new PdfStamper(r, ms);

                // Hay que cerrar el PDFStamper para que los cambios se reflejen en el PDF
                stamp.Close();

                // Se cierra el reader para que el archivo no quede amarrado a la aplicación
                r.Close();

                // ESTE ES EL TRUCO IMPORTANTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento modificado
                readerX = new PdfReader(ms.GetBuffer());

                // Se copian cada una de las páginas del nuevo PDFReader
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // NUEVAMENTE
                // Se crea un nuevo PDFReader con los byte[] del Stream que almacenaba el documento
                readerX = new PdfReader(Documento2);

                // Se copian cada una de las páginas del nuevo PDFReader, las cuales se adicionarán al documento por debajo de las que ya se adicionaron.
                for (int i = 1; i <= readerX.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(readerX, i));
                }

                // Se ejecutan un par de acciones de limpieza
                if (readerX != null)
                {
                    readerX.Close();
                }
                ms.Dispose();

                // Se cierra el documento para que se termine correctamente
                if (doc != null)
                {
                    doc.Close();
                }

            }// Fin del using
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}