using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using SGAC.Accesorios;
using System.Net;
using System.Text;

namespace SGAC.WebApp.Registro.Preview
{
    public partial class FrmPreviewActaDefuncion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new SGAC.Registro.Actuacion.BL.ActoCivilConsultaBL().Consultar_Formato(10251, 10019);

            if (dt.Rows.Count > 0)
            {
                lt_dia_1.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("dd").Substring(0,1);
                lt_dia_2.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("dd").Substring(1,1);

                lt_mes_1.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("MM").Substring(0, 1);
                lt_mes_2.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("MM").Substring(1, 1);

                lt_anio_1.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("yyyy").Substring(0, 1);
                lt_anio_2.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("yyyy").Substring(1, 1);
                lt_anio_3.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("yyyy").Substring(2, 1);
                lt_anio_4.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("yyyy").Substring(3, 1);

                lt_hora_1.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("HH").Substring(0, 1);
                lt_hora_2.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("HH").Substring(1, 1);
                lt_minuto_1.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("MM").Substring(0, 1);
                lt_minuto_2.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("MM").Substring(1, 1);

                lt_am_1.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("tt").Substring(0, 1);
                lt_am_2.Text = Convert.ToDateTime(dt.Rows[0]["Fecha"]).ToString("tt").Substring(2, 1);

                lt_lo_1.Text = Convert.ToString(dt.Rows[0]["LugarTipo"]).PadLeft(2,'0').Substring(0, 1);
                lt_lo_2.Text = Convert.ToString(dt.Rows[0]["LugarTipo"]).PadLeft(2, '0').Substring(1, 1);
                lt_lo_3.Text = Convert.ToString(dt.Rows[0]["LugarOcurrencia"]);
                
            }
        }

        DataTable CrearTmpTabla()
        {
            DataTable dtTablaTemporal = new DataTable();

            dtTablaTemporal.Columns.Add("strCadenaBuscar", typeof(string));
            dtTablaTemporal.Columns.Add("strCadenaReemplazar", typeof(string));

            return dtTablaTemporal;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new SGAC.Registro.Actuacion.BL.ActoCivilConsultaBL().Consultar_Formato(10251, 10019);


            string strRutaHtml = string.Empty;
            string strArchivoPDF = string.Empty;

            String localfilepath = String.Empty;
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            //Boolean Resultado = false;

            string strRutaPDF = uploadPath + @"\" + "DefuncionHtml" + DateTime.Now.Ticks.ToString() + ".pdf";
            strRutaHtml = uploadPath + @"\" + "DefuncionHtml" + DateTime.Now.Ticks.ToString() + ".html";

            StreamWriter str = new StreamWriter(strRutaHtml, true, Encoding.Default);
            str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
            str.Dispose();

            CreateFilePDFDefuncion(dt, strRutaHtml, strRutaPDF);

            if (System.IO.File.Exists(strRutaPDF))
            {
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                if (FileBuffer != null)
                {
                    HttpContext.Current.Session["binaryData"] = FileBuffer;
                    //Resultado = true;
                    //string strUrl = "../Accesorios/VisorPDF.aspx";
                    //string strScript = "window.open('" + strUrl + "', 'Visor', 'scrollbars=1,resizable=1,window.innerWidth = screen.width, window.innerHeight = screen.height');";
                    //Comun.EjecutarScript(Page,strScript);
                }
            }

            if (File.Exists(strRutaHtml))
            {
                File.Delete(strRutaHtml);
            }
        }

        private static void CreateFilePDFDefuncion(DataTable dt, string HtmlPath, string PdfPath)
        {
            int iValorY = 5;
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

                //iTextSharp.text.IElement oIElement;
                //iTextSharp.text.Paragraph oParagraph = null;
                //iTextSharp.text.pdf.PdfPTable oPdfPTable;
                //iTextSharp.text.pdf.PdfPRow oPdfPRow;
                //iTextSharp.text.pdf.PdfPCell oPdfPCell = null;
                //iTextSharp.text.Chunk oChunk;
                List<iTextSharp.text.IElement> objects;
                string strContent = string.Empty;

                iTextSharp.text.FontFactory.RegisterDirectories();
                document.Open();

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(PdfPath, FileMode.Create));

                document.Open();

                document.NewPage();

                objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(oStreamReader, styles);


                float fAnchoAreaTexto = document.PageSize.Width - fMargenDerechaDoc - fMargenIzquierdaDoc;

                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);

                cb.SetFontAndSize(bfTimes, 11);
                float ejex = (float)11.6;
                float fOrigenX = 68;
                float fOrigenY = 80;
                cb.BeginText();

                //if (vNavegador.IndexOf("firefox") == -1)
                //{
                //    fOrigenX -= 1;
                //    fOrigenY += 5;
                //}

                //Fecha de ocurrencia
                DateTime fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"]);
                string vFecha = fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.Month.ToString().PadLeft(2, '0') + " " + fecha.Year.ToString();
                string vHora = fecha.ToString("hh") + " " + fecha.ToString("mm") + " " + fecha.ToString("tt").ToUpper().Replace(".", "").Trim();


                _EscribirLetraxLetra(fOrigenX + (ejex * (float)11.5), fOrigenY + 2, ejex, vFecha, cb, document);

                _EscribirLetraxLetra(fOrigenX + (ejex * (float)29.5), fOrigenY + 2, ejex, vHora, cb, document);

                //Hora de Ocurrencia


                //Lugar de ocurrencuia

                string OcurreciaubigeoDepCod = dt.Rows[0]["LugarTipo"].ToString().PadLeft(2, '0');
                string OcurrenciaubigeoDepDesc = dt.Rows[0]["LugarOcurrencia"].ToString();
                string OcurreciaubigeoDep = OcurreciaubigeoDepCod + " " + OcurrenciaubigeoDepDesc;

                EscribirLetraxLetra(fOrigenX - 5, fOrigenY + 32, ejex, OcurreciaubigeoDep, cb, document);

                //Lugar Ubigeo Departamento
                string ubigeoDepCod = dt.Rows[0]["LugarUbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDesc = dt.Rows[0]["LugarUbigeoDepDetalle"].ToString();
                string ubigeoDep = ubigeoDepCod + "   " + ubigeoDepDesc;

                EscribirLetraxLetra(fOrigenX - 5, fOrigenY + 49 + iValorY, ejex, ubigeoDep, cb, document);


                //Lugar Ubigeo Provincia
                string ubigeoProvCod = dt.Rows[0]["LugarUbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDesc = dt.Rows[0]["LugarUbigeoPrvDetalle"].ToString();
                string ubigeoProv = ubigeoProvCod + "   " + ubigeoProvDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), fOrigenY + 49 + iValorY, ejex, ubigeoProv, cb, document);


                //Lugar Ubigeo Distrito
                string ubigeoDistCod = dt.Rows[0]["LugarUbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDesc = dt.Rows[0]["LugarUbigeoDstDetalle"].ToString();
                string ubigeoDist = ubigeoDistCod + "   " + ubigeoDistDesc;

                EscribirLetraxLetra(fOrigenX - 5, fOrigenY + 68 + iValorY, ejex, ubigeoDist, cb, document);


                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCod = dt.Rows[0]["LugarUbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDesc = dt.Rows[0]["LugarUbigeoCpoDetalle"].ToString();
                string ubigeoCpo = ubigeoCpoCod + "   " + ubigeoCpoDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), fOrigenY + 68 + iValorY, ejex, ubigeoCpo.Replace("00", "").Trim(), cb, document);

                //Nombre

                string vNombre = dt.Rows[0]["Prenombres"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 121 + iValorY, ejex, vNombre, cb, document);


                //Primer Apellido

                string vPrimerApellido = dt.Rows[0]["PrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 139 + iValorY, ejex, vPrimerApellido, cb, document);


                //Segundo Apellido

                string vSegundoApellido = dt.Rows[0]["SegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 156 + iValorY, ejex, vSegundoApellido, cb, document);



                //Documento Identidad

                string celTipoDocumento = dt.Rows[0]["TipoDocumento"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 8), fOrigenY + 169 + iValorY, ejex, celTipoDocumento, cb, document);

                string celNroDocumento = dt.Rows[0]["NumeroDocumento"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 20), fOrigenY + 170 + iValorY, ejex, celNroDocumento, cb, document);

                //Edad 

                string celEdad = dt.Rows[0]["Edad"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 36), fOrigenY + 170 + iValorY, ejex, celEdad, cb, document);

                //Nacionalidad
                string celNacionalidad = dt.Rows[0]["Nacionalidad"].ToString();
                EscribirLetraxLetra(fOrigenX + (ejex * 5), fOrigenY + 188 + iValorY, ejex, celNacionalidad, cb, document);

                string celNacionalidadTexto = dt.Rows[0]["NacionalidadTexto"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 13), fOrigenY + 188 + iValorY, ejex, celNacionalidadTexto, cb, document);

                //Lugar de nacimiento
                //Lugar Ubigeo Departamento
                string ubigeoNacDepCod = dt.Rows[0]["NacUbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoNacDepDesc = dt.Rows[0]["NacUbigeoDepDetalle"].ToString();
                string ubigeoNacDep = ubigeoNacDepCod + " " + ubigeoNacDepDesc;

                EscribirLetraxLetra(fOrigenX, fOrigenY + 222 + iValorY, ejex, ubigeoNacDep, cb, document);


                //Lugar Ubigeo Provincia
                string ubigeoNacProvCod = dt.Rows[0]["NacUbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoNacProvDesc = dt.Rows[0]["NacUbigeoPrvDetalle"].ToString();
                string ubigeoNacProv = ubigeoNacProvCod + " " + ubigeoNacProvDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), fOrigenY + 222 + iValorY, ejex, ubigeoNacProv, cb, document);


                //Lugar Ubigeo Distrito
                string ubigeoNacDistCod = dt.Rows[0]["NacUbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoNacDistDesc = dt.Rows[0]["NacUbigeoDstDetalle"].ToString();
                string ubigeoNacDist = ubigeoNacDistCod + " " + ubigeoNacDistDesc;

                EscribirLetraxLetra(fOrigenX, fOrigenY + 243 + iValorY, ejex, ubigeoNacDist, cb, document);


                //Lugar Ubigeo Centros Poblados
                string ubigeoNacCpoCod = dt.Rows[0]["NacUbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoNacCpoDesc = dt.Rows[0]["NacUbigeoCpoDetalle"].ToString();
                string ubigeoNacCpo = ubigeoNacCpoCod + " " + ubigeoNacCpoDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), fOrigenY + 243 + iValorY, ejex, ubigeoNacCpo.Replace("00", "").Trim(), cb, document);

                //Datos del padre

                //Nombre

                string vNombrePadre = dt.Rows[0]["PadrePrenombres"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 272 + iValorY, ejex, vNombrePadre, cb, document);


                //Primer Apellido

                string vPrimerApellidoPadre = dt.Rows[0]["PadrePrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 290 + iValorY, ejex, vPrimerApellidoPadre, cb, document);


                //Segundo Apellido

                string vSegundoApellidoPadre = dt.Rows[0]["PadreSegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 308 + iValorY, ejex, vSegundoApellidoPadre, cb, document);

                //Datos de la madre

                //Nombre

                string vNombreMadre = dt.Rows[0]["MadrePrenombres"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 336 + iValorY, ejex, vNombreMadre, cb, document);


                //Primer Apellido

                string vPrimerApellidoMadre = dt.Rows[0]["MadrePrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 353 + iValorY, ejex, vPrimerApellidoMadre, cb, document);


                //Segundo Apellido

                string vSegundoApellidoMadre = dt.Rows[0]["MadreSegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 372 + iValorY, ejex, vSegundoApellidoMadre, cb, document);

                //Fecha de registro - Oficina registral
                DateTime fechaRegistro = Convert.ToDateTime(dt.Rows[0]["RegistroFecha"]);
                string vFechaRegistro = fechaRegistro.Day.ToString().PadLeft(2, '0') + " " + fechaRegistro.Month.ToString().PadLeft(2, '0') + " " + fechaRegistro.Year.ToString();

                _EscribirLetraxLetra(fOrigenX + (ejex * (float)(8.5)), fOrigenY + 401 + iValorY, ejex, vFechaRegistro, cb, document);

                //Lugar Ubigeo Departamento
                string RegistroubigeoDepCod = dt.Rows[0]["RegistroUbigeoDep"].ToString().PadLeft(2, '0');
                string RegistroubigeoDepDesc = dt.Rows[0]["RegistroUbigeoDepDetalle"].ToString();
                string RegistroubigeoDep = RegistroubigeoDepCod + "   " + RegistroubigeoDepDesc;

                EscribirLetraxLetra(fOrigenX, fOrigenY + 420 + iValorY, ejex, RegistroubigeoDep, cb, document);


                //Lugar Ubigeo Provincia
                string RegistroubigeoProvCod = dt.Rows[0]["RegistroUbigeoPrv"].ToString().PadLeft(2, '0');
                string RegistroubigeoProvDesc = dt.Rows[0]["RegistroUbigeoPrvDetalle"].ToString();
                string RegistroubigeoProv = RegistroubigeoProvCod + "   " + RegistroubigeoProvDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), fOrigenY + 420 + iValorY, ejex, RegistroubigeoProv, cb, document);


                //Lugar Ubigeo Distrito
                string RegistroubigeoDistCod = dt.Rows[0]["RegistroUbigeoDst"].ToString().PadLeft(2, '0');
                string RegistroubigeoDistDesc = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();
                string RegistroubigeoDist = RegistroubigeoDistCod + "   " + RegistroubigeoDistDesc;

                EscribirLetraxLetra(fOrigenX, fOrigenY + 442 + iValorY, ejex, RegistroubigeoDist, cb, document);


                //Lugar Ubigeo Centros Poblados
                string RegistroubigeoCpoCod = dt.Rows[0]["RegistroUbigeoCpo"].ToString().PadLeft(2, '0');
                string RegistroubigeoCpoDesc = dt.Rows[0]["RegistroUbigeoCpoDetalle"].ToString();
                string RegistroubigeoCpo = RegistroubigeoCpoCod + "   " + RegistroubigeoCpoDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), fOrigenY + 442 + iValorY, ejex, RegistroubigeoCpo.Replace("00", "").Trim(), cb, document);


                //Documento registrador
                string TipoDocumentoDeclarante = dt.Rows[0]["DeclaranteTipoDocumento"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * (float)(23.5)), fOrigenY + 459 + iValorY, ejex, TipoDocumentoDeclarante, cb, document);


                string NumeroDocumentoDeclarante = dt.Rows[0]["DeclaranteNumeroDocumento"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * (float)(36.5)), fOrigenY + 464 + iValorY, ejex, NumeroDocumentoDeclarante, cb, document);


                //Datos del declarante

                //Nombre

                string vNombreDeclarante = dt.Rows[0]["DeclarantePrenombres"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 475 + iValorY, ejex, vNombreDeclarante, cb, document);


                //Primer Apellido

                string vPrimerApellidoDeclarante = dt.Rows[0]["DeclarantePrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 490 + iValorY, ejex, vPrimerApellidoDeclarante, cb, document);


                //Segundo Apellido

                string vSegundoApellidoDeclarante = dt.Rows[0]["DeclaranteSegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, fOrigenY + 508 + iValorY, ejex, vSegundoApellidoDeclarante, cb, document);


                //Datos del servidor

                string NomnbresRegistrador = dt.Rows[0]["RegistradorPrimerApellido"].ToString() + " " + dt.Rows[0]["RegistradorSegundoApellido"].ToString() + " " + dt.Rows[0]["RegistradorPrenombres"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * (float)(8)), fOrigenY + 527 + iValorY, ejex, NomnbresRegistrador, cb, document);


                string DocumentoRegistrador = dt.Rows[0]["RegistradorNumeroDocumento"].ToString();

                EscribirLetraxLetra(fOrigenX + 20, fOrigenY + 548 + iValorY, ejex, DocumentoRegistrador, cb, document);


                string Observaciones = dt.Rows[0]["Observaciones"].ToString();

                EscribirLetraxLetra(fOrigenX + 43, fOrigenY + 573 + iValorY, ejex, Observaciones, cb, document);



                cb.EndText();



                document.Close();
                oStreamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void EscribirLetraxLetra(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {

            cb.SetTextMatrix(ejeXInicio, document.PageSize.Height - ejeYInicio);
            cb.ShowText(palabra.ToString());

            //float cont = ejeXInicio;
            //foreach (char letra in palabra)
            //{
            //    cb.SetTextMatrix(cont, document.PageSize.Height - ejeYInicio);
            //    cb.ShowText(letra.ToString());
            //    cont += ejeXDistancia;
            //}
        }

        private static void _EscribirLetraxLetra(float ejeXInicio, float ejeYInicio, float ejeXDistancia, string palabra, iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Document document, int limiteLetra = 0)
        {
            float cont = ejeXInicio;
            foreach (char letra in palabra)
            {
                cb.SetTextMatrix(cont, document.PageSize.Height - ejeYInicio);
                cb.ShowText(letra.ToString());
                cont += ejeXDistancia;
            }
        }
    }
}