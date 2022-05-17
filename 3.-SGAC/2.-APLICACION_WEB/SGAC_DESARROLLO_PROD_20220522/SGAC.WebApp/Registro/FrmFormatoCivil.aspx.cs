using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using SGAC.Accesorios;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using SGAC.WebApp.Accesorios;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using com.itextpdf.text;

namespace SGAC.WebApp.Registro
{
    public partial class FrmFormatoCivil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HF_REFISTROCIVIL.Value = Request.QueryString[0].ToString();
            HF_ACTUDETALLE.Value = Request.QueryString[1].ToString();
            HF_TIPOACTA.Value = Request.QueryString[2].ToString();
        }


        [System.Web.Services.WebMethod]
        public static Boolean Imprimir(Int64 HF_REFISTROCIVIL, Int64 HF_ACTUDETALLE, Int32 intTipoActa)
        {

            try
            {
                StringBuilder sScript = new StringBuilder();
                Boolean Resultado = false;
                String vTipoActa = String.Empty;
                switch (intTipoActa)
                {
                    case (int)Enumerador.enmTipoActa.NACIMIENTO:
                        vTipoActa = Enumerador.enmTipoActa.NACIMIENTO.ToString();
                        break;
                    case (int)Enumerador.enmTipoActa.MATRIMONIO:
                        vTipoActa = Enumerador.enmTipoActa.MATRIMONIO.ToString();
                        break;
                    case (int)Enumerador.enmTipoActa.DEFUNCION:
                        vTipoActa = Enumerador.enmTipoActa.DEFUNCION.ToString();
                        break;
                }

                DataTable dt = new Reportes.dsActoCivil().Tables[vTipoActa];
                String NroCui = String.Empty;
                DateTime dFecNac;
                String vDiaNac = String.Empty;
                String vMesNac = String.Empty;
                String vAnio = String.Empty;
                String vHoraNac = String.Empty;
                String vMinNac = String.Empty;
                String vAMPM = String.Empty;

                String nUbigeoTipoLugarOcurrencia = String.Empty;
                String vUbigeoLugarOcurrencia = String.Empty;

                String nUbigeoOcurrenciaDep = String.Empty;
                String vUbigeoOcurrenciaDep = String.Empty;

                String nUbigeoOcurrenciaProv = String.Empty;
                String vUbigeoOcurrenciaProv = String.Empty;

                String nUbigeoOcurrenciaDist = String.Empty;
                String vUbigeoOcurrenciaDist = String.Empty;

                String nSexo = String.Empty;
                String vSexo = String.Empty;

                String vPrimerApellidoTitular = String.Empty;
                String vSegundoApellidoTitular = String.Empty;
                String vNombreTitular = String.Empty;

                String vPrimerApellidoPaterno = String.Empty;
                String vSegundoApellidoPaterno = String.Empty;
                String vNombrePaterno = String.Empty;
                String nTipoDocumentoPaterno = String.Empty;
                String vNumeroDocumentoPaterno = String.Empty;
                String nNacionalidadPaterno = String.Empty;

                String vPrimerApellidoMadre = String.Empty;
                String vSegundoApellidoMadre = String.Empty;
                String vNombreMadre = String.Empty;
                String vTipoDocumentoMadre = String.Empty;
                String vNumeroDocumentoMadre = String.Empty;
                String vNacionalidadMadre = String.Empty;
                String nUbigeoDepMadre = String.Empty;
                String vUbigeoDepMadre = String.Empty;
                String nUbigeoProvMadre = String.Empty;
                String vUbigeoProvMadre = String.Empty;
                String nUbigeodistMadre = String.Empty;
                String vUbigeodistMadre = String.Empty;


                dt = new SGAC.Registro.Actuacion.BL.ActoCivilConsultaBL().Consultar_Formato(HF_ACTUDETALLE, HF_REFISTROCIVIL);


                //DataTable dtTMPReemplazar = new DataTable();
                //dtTMPReemplazar = CrearTmpTabla();

                string strRutaHtml = string.Empty;
                string strArchivoPDF = string.Empty;

                String localfilepath = String.Empty;
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                string strRutaPDF = uploadPath + @"\" + "ActaNacimiento" + DateTime.Now.Ticks.ToString() + ".pdf";
                strRutaHtml = uploadPath + @"\" + "ActaNacimiento" + DateTime.Now.Ticks.ToString() + ".html";

                StreamWriter str = new StreamWriter(strRutaHtml, true, Encoding.Default);

                switch (intTipoActa)
                {
                    case (int)Enumerador.enmTipoActa.NACIMIENTO:

                        str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
                        str.Dispose();


                        CreateFilePDFNacimiento2(dt, strRutaHtml, strRutaPDF);
                        
                        if (System.IO.File.Exists(strRutaPDF))
                        {
                            WebClient User = new WebClient();
                            Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                            if (FileBuffer != null)
                            {
                                HttpContext.Current.Session["binaryData"] = FileBuffer;
                                Resultado = true;
                        
                            }
                        }

                        if (File.Exists(strRutaHtml))
                        {
                            File.Delete(strRutaHtml);
                        }


                        break;
                    case (int)Enumerador.enmTipoActa.MATRIMONIO:


                        str.Write("<p align=\"justify\" style=\"background-color: transparent;\">Hola</p>");
                        str.Dispose();


                        CreateFilePDFMatrimonio(dt, strRutaHtml, strRutaPDF);

                        if (System.IO.File.Exists(strRutaPDF))
                        {
                            WebClient User = new WebClient();
                            Byte[] FileBuffer = User.DownloadData(strRutaPDF);
                            if (FileBuffer != null)
                            {
                                HttpContext.Current.Session["binaryData"] = FileBuffer;
                                Resultado = true;                        
                            }
                        }

                        if (File.Exists(strRutaHtml))
                        {
                            File.Delete(strRutaHtml);
                        }

                        break;
                    case (int)Enumerador.enmTipoActa.DEFUNCION:

                        break;
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static  void CreateFilePDFMatrimonio(DataTable dt, string HtmlPath, string PdfPath)
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
                iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, false);




                cb.SetFontAndSize(bfTimes, 14);

                float ejex = (float)11.6;
                float fOrigenX = 67;
                float fOrigenY = document.PageSize.Height - 70 - 18;

                #region PosicionesY

                float fechaPosY = fOrigenY;
                float ubigeoLugarDepProvPosY = fOrigenY - 23;
                float ubigeoLugarDistCpoPosY = fOrigenY - 38;

                float celebranteIdentidad = fOrigenY - 54;
                float celebranteNombrePosY = fOrigenY - 70;

                float celebrantePrimerApellidoPosY = fOrigenY - 86;
                float celebranteSegundoApellidoPosY = fOrigenY - 102;
                float celebranteCargoPosY = fOrigenY - 105;
                float celebranteExpedientePosY = fOrigenY - 23;

                float conyuge1NombrePosY = fOrigenY - 178;
                float conyuge1PrimerApellidoPosY = fOrigenY - 198;
                float conyuge1SegundoApellidoPosY = fOrigenY - 214;
                float conyuge1IdentiNacionalPosY = fOrigenY - 230;
                float conyuge1EdadEdoCivilPosY = fOrigenY - 248;
                float conyuge1DepProvPosY = fOrigenY - 270;
                float conyuge1DistCpoPosY = fOrigenY - 286;

                float conyuge2NombrePosY = fOrigenY - 316;
                float conyuge2PrimerApellidoPosY = fOrigenY - 336;
                float conyuge2SegundoApellidoPosY = fOrigenY - 352;
                float conyuge2IdentiNacionalPosY = fOrigenY - 368;
                float conyuge2EdadEdoCivilPosY = fOrigenY - 386;
                float conyuge2DepProvPosY = fOrigenY - 404;
                float conyuge2DistCpoPosY = fOrigenY - 420;

                float fechaRegistroPosY = fOrigenY - 444;

                float registradorDepProvPosY = fOrigenY - 480;
                float registradorDistCpoPosY = fOrigenY - 496;

                float registradorNombrePosY = fOrigenY - 512;

                float registradorIdentidadPosY = fOrigenY - 528;

                #endregion

                cb.BeginText();



                //Fecha de Celebracion
                DateTime fecha = Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString());
                string vFecha = fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.Month.ToString().PadLeft(2, '0') + " " + fecha.Year.ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 9), fechaPosY, ejex, vFecha, cb, document);

                //Lugar Ubigeo Departamento
                string ubigeoDepCod = dt.Rows[0]["LugarUbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDesc = dt.Rows[0]["LugarUbigeoDepDetalle"].ToString();
                string ubigeoDep = ubigeoDepCod + " " + ubigeoDepDesc;

                EscribirLetraxLetra(fOrigenX, ubigeoLugarDepProvPosY, ejex, ubigeoDep, cb, document);


                //Lugar Ubigeo Provincia
                string ubigeoProvCod = dt.Rows[0]["LugarUbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDesc = dt.Rows[0]["LugarUbigeoPrvDetalle"].ToString();
                string ubigeoProv = ubigeoProvCod + " " + ubigeoProvDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), ubigeoLugarDepProvPosY, ejex, ubigeoProv, cb, document);


                //Lugar Ubigeo Distrito
                string ubigeoDistCod = dt.Rows[0]["LugarUbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDesc = dt.Rows[0]["LugarUbigeoDstDetalle"].ToString();
                string ubigeoDist = ubigeoDistCod + " " + ubigeoDistDesc;

                EscribirLetraxLetra(fOrigenX, ubigeoLugarDistCpoPosY, ejex, ubigeoDist, cb, document);


                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCod = dt.Rows[0]["LugarUbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDesc = dt.Rows[0]["LugarUbigeoCpoDetalle"].ToString();
                string ubigeoCpo = ubigeoCpoCod + " " + ubigeoCpoDesc;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), ubigeoLugarDistCpoPosY, ejex, ubigeoCpo, cb, document);


                //Documento Identidad

                string celTipoDocumento = dt.Rows[0]["CelebranteTipoDocumento"].ToString();
                string celNroDocumento = dt.Rows[0]["CelebranteNumeroDocumento"].ToString();
                string celDocumento = celTipoDocumento + "  " + celNroDocumento;

                EscribirLetraxLetra(fOrigenX + (ejex * 26), celebranteIdentidad, ejex, celDocumento, cb, document);


                //Nombre

                string vNombre = dt.Rows[0]["CelebrantePreNombres"].ToString();

                EscribirLetraxLetra(fOrigenX, celebranteNombrePosY, ejex, vNombre, cb, document);


                //Primer Apellido

                string vPrimerApellido = dt.Rows[0]["CelebrantePrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, celebrantePrimerApellidoPosY, ejex, vPrimerApellido, cb, document);


                //Segundo Apellido

                string vSegundoApellido = dt.Rows[0]["CelebranteSegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX, celebranteSegundoApellidoPosY, ejex, vSegundoApellido, cb, document);


                //Cargo

                string vCargo = dt.Rows[0]["CelebranteCargo"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 5), fOrigenY - 140, ejex, vCargo, cb, document);


                //Cargo

                string vExpediente = dt.Rows[0]["CelebranteExpediente"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 7), fOrigenY - 160, ejex, vExpediente, cb, document);


                //Conyuge1 Don PreNombre

                string vConyuge1Nombre = dt.Rows[0]["Conyuge1Prenombres"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 0), conyuge1NombrePosY, ejex, vConyuge1Nombre, cb, document);


                //Conyuge1 Don PrimerApellido

                string vConyuge1PrimerApellido = dt.Rows[0]["Conyuge1PrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 0), conyuge1PrimerApellidoPosY, ejex, vConyuge1PrimerApellido, cb, document);


                //Conyuge1 Don SegundoApellido

                string vConyuge1SegundoApellido = dt.Rows[0]["Conyuge1SegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 0), conyuge1SegundoApellidoPosY, ejex, vConyuge1SegundoApellido, cb, document);


                //Conyuge1 Don Documento Identidad

                string vConyuge1TipoDocumento = dt.Rows[0]["Conyuge1TipoDocumento"].ToString();
                string vConyuge1NumeroDocumento = dt.Rows[0]["Conyuge1NumeroDocumento"].ToString();

                string vConyuge1Documento = vConyuge1TipoDocumento + "    " + vConyuge1NumeroDocumento;

                EscribirLetraxLetra(fOrigenX + (ejex * 8), conyuge1IdentiNacionalPosY, ejex, vConyuge1Documento, cb, document);


                //Conyuge1 Nacionalidad

                string vConyuge1Nacionalidad = dt.Rows[0]["Conyuge1Nacionalidad"].ToString();
                string vConyuge1NacionalidadTexto = dt.Rows[0]["Conyuge1NacionalidadTexto"].ToString();

                string vConyuge1NacionalidadCompleta = vConyuge1Nacionalidad + "     " + vConyuge1NacionalidadTexto;

                EscribirLetraxLetra(fOrigenX + (ejex * 29), conyuge1IdentiNacionalPosY, ejex, vConyuge1NacionalidadCompleta, cb, document);


                //Conyuge1 Edad

                string vConyuge1Edad = dt.Rows[0]["Conyuge1Edad"].ToString().PadLeft(3, '0');

                EscribirLetraxLetra(fOrigenX + (ejex * 2), conyuge1EdadEdoCivilPosY, ejex, vConyuge1Edad, cb, document);


                //Conyuge1 Estado Civil

                string vConyuge1EstadoCivil = dt.Rows[0]["Conyuge1EstadoCivil"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 29), conyuge1EdadEdoCivilPosY, ejex, vConyuge1EstadoCivil, cb, document);


                //Lugar Ubigeo Departamento Conyuge1
                string ubigeoDepCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoDepDetalle"].ToString();
                string ubigeoDepConyuge1 = ubigeoDepCodConyuge1 + " " + ubigeoDepDescConyuge1;

                EscribirLetraxLetra(fOrigenX, conyuge1DepProvPosY, ejex, ubigeoDepConyuge1, cb, document);


                //Lugar Ubigeo Provincia
                string ubigeoProvCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoPrvDetalle"].ToString();
                string ubigeoProvConyuge1 = ubigeoProvCodConyuge1 + " " + ubigeoProvDescConyuge1;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), conyuge1DepProvPosY, ejex, ubigeoProvConyuge1, cb, document);


                //Lugar Ubigeo Distrito
                string ubigeoDistCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoDstDetalle"].ToString();
                string ubigeoDistConyuge1 = ubigeoDistCodConyuge1 + " " + ubigeoDistDescConyuge1;

                EscribirLetraxLetra(fOrigenX, conyuge1DistCpoPosY, ejex, ubigeoDistConyuge1, cb, document);


                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCodConyuge1 = dt.Rows[0]["Conyuge1UbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDescConyuge1 = dt.Rows[0]["Conyuge1UbigeoCpoDetalle"].ToString();
                string ubigeoCpoConyuge1 = ubigeoCpoCodConyuge1 + " " + ubigeoCpoDescConyuge1;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), conyuge1DistCpoPosY, ejex, ubigeoCpoConyuge1, cb, document);


                //Conyuge2 Don PreNombre

                string vConyuge2Nombre = dt.Rows[0]["Conyuge2Prenombres"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 0), conyuge2NombrePosY, ejex, vConyuge2Nombre, cb, document);


                //Conyuge2 Doña PrimerApellido

                string vConyuge2PrimerApellido = dt.Rows[0]["Conyuge2PrimerApellido"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 0), conyuge2PrimerApellidoPosY, ejex, vConyuge2PrimerApellido, cb, document);


                //Conyuge2 Don SegundoApellido

                string vConyuge2SegundoApellido = dt.Rows[0]["Conyuge2SegundoApellido"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 0), conyuge2SegundoApellidoPosY, ejex, vConyuge2SegundoApellido, cb, document);


                //Conyuge2 Don Documento Identidad

                string vConyuge2TipoDocumento = dt.Rows[0]["Conyuge2TipoDocumento"].ToString();
                string vConyuge2NumeroDocumento = dt.Rows[0]["Conyuge2NumeroDocumento"].ToString();

                string vConyuge2Documento = vConyuge2TipoDocumento + "     " + vConyuge2NumeroDocumento;

                EscribirLetraxLetra(fOrigenX + (ejex * 8), conyuge2IdentiNacionalPosY, ejex, vConyuge2Documento, cb, document);


                //Conyuge2 Nacionalidad

                string vConyuge2Nacionalidad = dt.Rows[0]["Conyuge2Nacionalidad"].ToString();
                string vConyuge2NacionalidadTexto = dt.Rows[0]["Conyuge2NacionalidadTexto"].ToString();

                string vConyuge2NacionalidadCompleta = vConyuge2Nacionalidad + "    " + vConyuge2NacionalidadTexto;

                EscribirLetraxLetra(fOrigenX + (ejex * 29), conyuge2IdentiNacionalPosY, ejex, vConyuge2NacionalidadCompleta, cb, document);


                //Conyuge2 Edad

                string vConyuge2Edad = dt.Rows[0]["Conyuge2Edad"].ToString().PadLeft(3, '0');

                EscribirLetraxLetra(fOrigenX + (ejex * 2), conyuge2EdadEdoCivilPosY, ejex, vConyuge2Edad, cb, document);


                //Conyuge2 Estado Civil

                string vConyuge2EstadoCivil = dt.Rows[0]["Conyuge2EstadoCivil"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 29), conyuge2EdadEdoCivilPosY, ejex, vConyuge2EstadoCivil, cb, document);


                //Lugar Ubigeo Departamento Conyuge2
                string ubigeoDepCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoDepDetalle"].ToString();
                string ubigeoDepConyuge2 = ubigeoDepCodConyuge2 + " " + ubigeoDepDescConyuge2;

                EscribirLetraxLetra(fOrigenX, conyuge2DepProvPosY, ejex, ubigeoDepConyuge2, cb, document);


                //Lugar Ubigeo Provincia
                string ubigeoProvCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoPrvDetalle"].ToString();
                string ubigeoProvConyuge2 = ubigeoProvCodConyuge2 + " " + ubigeoProvDescConyuge2;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), conyuge2DepProvPosY, ejex, ubigeoProvConyuge2, cb, document);


                //Lugar Ubigeo Distrito
                string ubigeoDistCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoDstDetalle"].ToString();
                string ubigeoDistConyuge2 = ubigeoDistCodConyuge2 + " " + ubigeoDistDescConyuge2;

                EscribirLetraxLetra(fOrigenX, conyuge2DistCpoPosY, ejex, ubigeoDistConyuge2, cb, document);


                //Lugar Ubigeo Centros Poblados
                string ubigeoCpoCodConyuge2 = dt.Rows[0]["Conyuge2UbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDescConyuge2 = dt.Rows[0]["Conyuge2UbigeoCpoDetalle"].ToString();
                string ubigeoCpoConyuge2 = ubigeoCpoCodConyuge2 + " " + ubigeoCpoDescConyuge2;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), conyuge2DistCpoPosY, ejex, ubigeoCpoConyuge2, cb, document);


                //Fecha de Celebracion
                DateTime fechaRegistro = Comun.FormatearFecha(dt.Rows[0]["RegistroFecha"].ToString());
                string vFechaRegistro = fechaRegistro.Day.ToString().PadLeft(2, '0') + " " + fechaRegistro.Month.ToString().PadLeft(2, '0') + " " + fechaRegistro.Year.ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 9), fechaRegistroPosY, ejex, vFechaRegistro, cb, document);


                //Registro Ubigeo Departamento
                string ubigeoDepCodRegistro = dt.Rows[0]["RegistroUbigeoDep"].ToString().PadLeft(2, '0');
                string ubigeoDepDescRegistro = dt.Rows[0]["RegistroUbigeoDepDetalle"].ToString();
                string ubigeoDepRegistro = ubigeoDepCodRegistro + " " + ubigeoDepDescRegistro;

                EscribirLetraxLetra(fOrigenX, registradorDepProvPosY, ejex, ubigeoDepRegistro, cb, document);


                //Registro Ubigeo Provincia
                string ubigeoProvCodRegistro = dt.Rows[0]["RegistroUbigeoPrv"].ToString().PadLeft(2, '0');
                string ubigeoProvDescRegistro = dt.Rows[0]["RegistroUbigeoPrvDetalle"].ToString();
                string ubigeoProvRegistro = ubigeoProvCodRegistro + " " + ubigeoProvDescRegistro;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), registradorDepProvPosY, ejex, ubigeoProvRegistro, cb, document);


                //Registro Ubigeo Distrito
                string ubigeoDistCodRegistro = dt.Rows[0]["RegistroUbigeoDst"].ToString().PadLeft(2, '0');
                string ubigeoDistDescRegistro = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();
                string ubigeoDistRegistro = ubigeoDistCodRegistro + " " + ubigeoDistDescRegistro;

                EscribirLetraxLetra(fOrigenX, registradorDistCpoPosY, ejex, ubigeoDistRegistro, cb, document);


                //Registro Ubigeo Centros Poblados
                string ubigeoCpoCodRegistro = dt.Rows[0]["RegistroUbigeoCpo"].ToString().PadLeft(2, '0');
                string ubigeoCpoDescRegistro = dt.Rows[0]["RegistroUbigeoCpoDetalle"].ToString();
                string ubigeoCpoRegistro = ubigeoCpoCodRegistro + " " + ubigeoCpoDescRegistro;

                EscribirLetraxLetra(fOrigenX + (ejex * 25), registradorDistCpoPosY, ejex, ubigeoCpoRegistro, cb, document);


                //Registrador Civil Nombre Completo
                string vRegistradorPrenombres = dt.Rows[0]["RegistradorPrenombres"].ToString();
                string vRegistradorPrimerApellido = dt.Rows[0]["RegistradorPrimerApellido"].ToString();
                string vRegistradorSegundoApellido = dt.Rows[0]["RegistradorSegundoApellido"].ToString();

                string vRegistradorNombreCompleto = vRegistradorPrenombres + " " + vRegistradorPrimerApellido + " " + vRegistradorSegundoApellido;

                EscribirLetraxLetra(fOrigenX + (ejex * 8), registradorNombrePosY, ejex, vRegistradorNombreCompleto, cb, document);


                //Registrador Don Documento Identidad

                string vRegistradorNumeroDocumento = dt.Rows[0]["RegistradorNumeroDocumento"].ToString();

                EscribirLetraxLetra(fOrigenX + (ejex * 3), registradorIdentidadPosY, ejex, vRegistradorNumeroDocumento, cb, document);

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
            float cont = ejeXInicio;
            foreach (char letra in palabra)
            {
                cb.SetTextMatrix(cont, document.PageSize.Height - ejeYInicio);
                cb.ShowText(letra.ToString());
                cont += ejeXDistancia;
            }
        }

       private static void CreateFilePDFNacimiento2(DataTable dt, string HtmlPath, string PdfPath)
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
               iTextSharp.text.pdf.BaseFont bfTimes = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.COURIER, iTextSharp.text.pdf.BaseFont.CP1252, false);

               cb.SetFontAndSize(bfTimes, 14);

               float ejex = (float)11.6;
               float fOrigenX = -36;
               float fOrigenY_ = document.PageSize.Height - 70 - 18;
               float fOrigenX_ = document.PageSize.Width;
               float fOrigenY = 18;
               #region PosicionesY

               float CuiPosY = fOrigenY +  40;
               float CuiPosX = fOrigenX + 220;

               float fechaPosY = fOrigenY + 62;
               float fechaPosX = fOrigenX + 133;

               float HoraPosY = fOrigenY + 62;
               float HoraPosX = fOrigenX + 336;

               float TipoLugarPosY = fOrigenY + 82;
               float TipoLugarPosX = fOrigenX + 131;

               float LugarPosY = fOrigenY + 82;
               float LugarPosX = fOrigenX + 152;

               /*                                     */
               float LugarUbioDepPosX = fOrigenX;
               float LugarUbioDepPosY = fOrigenY + 100;

               float LugarUbigeoDepDetallePosX = fOrigenX + 37;
               float LugarUbigeoDepDetallePosY = fOrigenY + 100;
               
               float LugarUbigeoPrvPosX = fOrigenX + 290;
               float LugarUbigeoPrvPosY = fOrigenY + 100;

               float LugarUbigeoPrvdetallePosX = fOrigenX + 330;
               float LugarUbigeoPrvdetallePosY = fOrigenY + 100;

               float LugarUbigeoDstPosX = fOrigenX;
               float LugarUbigeoDstPosY = fOrigenY + 120;

               float LugarUbigeoDstDetallePosX = fOrigenX + 37;
               float LugarUbigeoDstDetallePosY = fOrigenY + 120;

               float SexoPosX = fOrigenX + 36;
               float SexoPosY = fOrigenY + 138;

               float SexoDetallePosX = fOrigenX + 87;
               float SexoDetallePosY = fOrigenY + 138;


               float PreNombresPosX = fOrigenX;
               float PreNombresPosY = fOrigenY + 161;

               float PrimerApellidoPosX = fOrigenX;
               float PrimerApellidoPosY = fOrigenY + 181;
               
               float SegundoApellidoPosX = fOrigenX;
               float SegundoApellidoPosY = fOrigenY + 201;





               float PadrePreNombresPosX = fOrigenX;
               float PadrePreNombresPosY = fOrigenY +232;

               float PadrePrimerApellidoPosX = fOrigenX;
               float PadrePrimerApellidoPosY = fOrigenY + 250;

               float PadreSegundoApellidoPosX = fOrigenX;
               float PadreSegundoApellidoPosY = fOrigenY + 268;

               float PadreTipoDocumentoPosX = fOrigenX + 89;
               float PadreTipoDocumentoPosY = fOrigenY + 285;

               float PadreNumeroDocumentoPosX = fOrigenX + 223;
               float PadreNumeroDocumentoPosY = fOrigenY + 285;

               float PadreNacionalidadPosX = fOrigenX + 430;
               float PadreNacionalidadPosY = fOrigenY + 285;


               float MadrePreNombresPosX = fOrigenX;
               float MadrePreNombresPosY = fOrigenY + 307;

               float MadrePrimerApellidoPosX = fOrigenX;
               float MadrePrimerApellidoPosY = fOrigenY + 326;

               float MadreSegundoApellidoPosX = fOrigenX;
               float MadreSegundoApellidoPosY = fOrigenY + 343;

               float MadreTipoDocumentoPosX = fOrigenX + 89;
               float MadreTipoDocumentoPosY = fOrigenY + 360;

               float MadreNumeroDocumentoPosX = fOrigenX + 223;
               float MadreNumeroDocumentoPosY = fOrigenY + 360;

               float MadreNacionalidadPosX = fOrigenX + 430;
               float MadreNacionalidadPosY = fOrigenY + 360;

               float DireccionPosX = fOrigenX + 62;
               float DireccionPosY = fOrigenY + 375;

               float DireccionUbigeoDepPosX = fOrigenX;
               float DireccionUbigeoDepPosY = fOrigenY + 386;

               float DireccionUbigeoDepDetallePosX = fOrigenX + 37;
               float DireccionUbigeoDepDetallePosY = fOrigenY + 386;

               float DireccionUbigeoPrvPosX = fOrigenX + 290;
               float DireccionUbigeoPrvPosY = fOrigenY + 386;

               float DireccionUbigeoPrvDetallePosX = fOrigenX + 330;
               float DireccionUbigeoPrvDetallePosY = fOrigenY + 386;

               float DireccionUbigeoDstPosX = fOrigenX;
               float DireccionUbigeoDstPosY = fOrigenY + 409;

               float DireccionUbigeoDstDetallePosX = fOrigenX + 37;
               float DireccionUbigeoDstDetallePosY = fOrigenY + 409;

               //---
               float RegistroFechaPosX = fOrigenX + 100;
               float RegistroFechaPosY = fOrigenY + 430;
               
               float RegisTroUbigeoDepPosX = fOrigenX;
               float RegisTroUbigeoDepPosY = fOrigenY + 452;

               float RegistroUbigeoDetallePosX = fOrigenX + 35;
               float RegistroUbigeoDetallePosY = fOrigenY + 452;

               float RegistroUbigeoPrvPosX = fOrigenX + 292;
               float RegistroUbigeoPrvPosY = fOrigenY + 452;
              
               float RegistroUbigeoPrvDetallePosX = fOrigenX + 330;
               float RegistroUbigeoPrvDetallePosY = fOrigenY + 452;
              
               float RegistroUbigeoDstPosX = fOrigenX;
               float RegistroUbigeoDstPosY = fOrigenY + 470;
 
               float RegistroUbigeoDstDetallePosX = fOrigenX + 35;
               float RegistroUbigeoDstDetallePosY = fOrigenY + 470;

               float Declarante1VinculoPosX = fOrigenX + 90;
               float Declarante1VinculoPosY = fOrigenY + 488;

               float Declarante1PrenombresPosX = fOrigenX;
               float Declarante1PrenombresPosY = fOrigenY + 495;

               float Declarante1PrimerApellidoPosX = fOrigenX;
               float Declarante1PrimerApellidoPosY = fOrigenY + 515;
              
               float Declarante1SegundoApellidoPosX = fOrigenX;
               float Declarante1SegundoApellidoPosY = fOrigenY + 530;
              
              float Declarante1TipoDocumentoPosX = fOrigenX + 293;
              float Declarante1TipoDocumentoPosY = fOrigenY + 488;

              float Declarante1NumeroDocumentoPosX = fOrigenX + 400;
              float Declarante1NumeroDocumentoPosY = fOrigenY + 488;

              float Declarante2VinculoPosX = fOrigenX + 92;
              float Declarante2VinculoPosY = fOrigenY + 550;

              float Declarante2PrenombresPosX = fOrigenX;
              float Declarante2PrenombresPosY = fOrigenY + 565;

              float Declarante2PrimerApellidoPosX = fOrigenX;
              float Declarante2PrimerApellidoPosY = fOrigenY + 580;

              float Declarante2SegundoApellidoPosX = fOrigenX;
              float Declarante2SegundoApellidoPosY = fOrigenY + 595;

              float Declarante2TipoDocumentoPosX = fOrigenX + 293;
              float Declarante2TipoDocumentoPosY = fOrigenY + 550;

              float Declarante2NumeroDocumentoPosX = fOrigenX + 398;
              float Declarante2NumeroDocumentoPosY = fOrigenY + 550;

              float RegistradorNombresCompletosPosX = fOrigenX + 90;
              float RegistradorNombresCompletosPosY = fOrigenY + 612;


              float RegistradorNumeroDocumentoPosX = fOrigenX+22;
              float RegistradorNumeroDocumentoPosY = fOrigenY + 630;
              
              float ObservacionesPosX = fOrigenX + 105;
              float ObservacionesPosY = fOrigenY + 655;

               #endregion

               cb.BeginText();

               String Cui = String.Empty;
               String Lugar = String.Empty;
               String TipoLugar = String.Empty;
               String LugarUbioDep = String.Empty;
               String LugarUbigeoDepDetalle = String.Empty;
               String LugarUbigeoPrv = String.Empty;
               String LugarUbigeoPrvdetalle = String.Empty;
               String LugarUbigeoDst = String.Empty;
               String LugarUbigeoDstDetalle = String.Empty;

               String Sexo = String.Empty;
               String SexoDetalle = String.Empty;

               
              String  PreNombres= String.Empty;
              String  PrimerApellido= String.Empty;
              String  SegundoApellido= String.Empty;

              String  PadrePreNombres= String.Empty;
              String  PadrePrimerApellido= String.Empty;
              String  PadreSegundoApellido= String.Empty;
              String  PadreTipoDocumento= String.Empty;
              String  PadreNumeroDocumento= String.Empty;
              String  PadreNacionalidad = String.Empty;


              String MadrePreNombres = String.Empty;
              String MadrePrimerApellido = String.Empty;
              String MadreSegundoApellido = String.Empty;
              String MadreTipoDocumento = String.Empty;
              String MadreNumeroDocumento = String.Empty;
              String MadreNacionalidad = String.Empty;
              String DireccionUbigeoDep = String.Empty;
              String DireccionUbigeoDepDetalle = String.Empty;
              String DireccionUbigeoPrv = String.Empty;
              String DireccionUbigeoPrvDetalle = String.Empty;
              String DireccionUbigeoDst = String.Empty;
              String DireccionUbigeoDstDetalle = String.Empty;
              String Direccion = String.Empty;

              String RegistroFecha= String.Empty;
              String RegisTroUbigeoDep= String.Empty;
              String RegistroUbigeoDetalle= String.Empty;
              String RegistroUbigeoPrv= String.Empty;
              String RegistroUbigeoPrvDetalle= String.Empty;
              String RegistroUbigeoDst= String.Empty;
              String RegistroUbigeoDstDetalle= String.Empty;

              String Declarante1Vinculo= String.Empty;
              String Declarante1Prenombres= String.Empty;
              String Declarante1PrimerApellido= String.Empty;
              String Declarante1SegundoApellido= String.Empty;
              String Declarante1TipoDocumento= String.Empty;
              String Declarante1NumeroDocumento= String.Empty;

              String Declarante2Vinculo= String.Empty;
              String Declarante2Prenombres= String.Empty;
              String Declarante2PrimerApellido= String.Empty;
              String Declarante2SegundoApellido= String.Empty;
              String Declarante2TipoDocumento= String.Empty;
              String Declarante2NumeroDocumento= String.Empty;

              String RegistradorPrenombres= String.Empty;
              String RegistradorPrimerApellido = String.Empty;
              String RegistradorSegundoApellido = String.Empty;

              String RegistradorNombresCompletos = String.Empty;
              String RegistradorNumeroDocumento= String.Empty;

              String Observaciones = String.Empty;

               //CUI 
               if(dt.Rows[0]["CUI"]!=null) Cui = dt.Rows[0]["CUI"].ToString();
               EscribirLetraxLetra(CuiPosX + (ejex * 9), CuiPosY, ejex, Cui, cb, document); 

               //Fecha de Nacimiento
               DateTime fecha = Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString());
               string vFecha = fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.Month.ToString().PadLeft(2, '0') + " " + fecha.Year.ToString();
               string vHora = fecha.Hour.ToString().PadLeft(2, '0') + " " + fecha.Minute.ToString().PadLeft(2, '0');// +"  " + fecha.Ticks.ToString();

               EscribirLetraxLetra(fechaPosX + (ejex * 9), fechaPosY, ejex, vFecha, cb, document);
               EscribirLetraxLetra(HoraPosX + (ejex * 9), HoraPosY, ejex, vHora, cb, document);

               //Lugar u Tipo Lugar
               if (dt.Rows[0]["Lugar"] != null) Lugar = dt.Rows[0]["Lugar"].ToString();
               if (dt.Rows[0]["LugarTipo"] != null) TipoLugar = dt.Rows[0]["LugarTipo"].ToString();
               EscribirLetraxLetra(LugarPosX + (ejex * 9), LugarPosY, ejex, Lugar, cb, document);
               EscribirLetraxLetra(TipoLugarPosX + (ejex * 9), TipoLugarPosY, ejex, TipoLugar, cb, document); 


               //Lugar de Ocurrencia
               if (dt.Rows[0]["LugarUbigeoDep"] != null) LugarUbioDep = dt.Rows[0]["LugarUbigeoDep"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["LugarUbigeoDepDetalle"] != null) LugarUbigeoDepDetalle = dt.Rows[0]["LugarUbigeoDepDetalle"].ToString();
               if (dt.Rows[0]["LugarUbigeoPrv"] != null) LugarUbigeoPrv = dt.Rows[0]["LugarUbigeoPrv"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["LugarUbigeoPrvdetalle"] != null) LugarUbigeoPrvdetalle = dt.Rows[0]["LugarUbigeoPrvdetalle"].ToString();
               if (dt.Rows[0]["LugarUbigeoDst"] != null) LugarUbigeoDst = dt.Rows[0]["LugarUbigeoDst"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["LugarUbigeoDstDetalle"] != null) LugarUbigeoDstDetalle = dt.Rows[0]["LugarUbigeoDstDetalle"].ToString();


               EscribirLetraxLetra(LugarUbioDepPosX + (ejex * 9), LugarUbioDepPosY, ejex, LugarUbioDep, cb, document);
               EscribirLetraxLetra(LugarUbigeoDepDetallePosX + (ejex * 9), LugarUbigeoDepDetallePosY, ejex, LugarUbigeoDepDetalle, cb, document);
               EscribirLetraxLetra(LugarUbigeoPrvPosX + (ejex * 9), LugarUbigeoPrvPosY, ejex, LugarUbigeoPrv, cb, document);
               EscribirLetraxLetra(LugarUbigeoPrvdetallePosX + (ejex * 9), LugarUbigeoPrvdetallePosY, ejex, LugarUbigeoPrvdetalle, cb, document);
               EscribirLetraxLetra(LugarUbigeoDstPosX + (ejex * 9), LugarUbigeoDstPosY, ejex, LugarUbigeoDst, cb, document);
               EscribirLetraxLetra(LugarUbigeoDstDetallePosX + (ejex * 9), LugarUbigeoDstDetallePosY, ejex, LugarUbigeoDstDetalle, cb, document);


               //Sexo
               if (dt.Rows[0]["Sexo"] != null) Sexo = dt.Rows[0]["Sexo"].ToString();
               if (dt.Rows[0]["SexoDetalle"] != null) SexoDetalle = dt.Rows[0]["SexoDetalle"].ToString();

               EscribirLetraxLetra(SexoPosX + (ejex * 9), SexoPosY, ejex, Sexo, cb, document);
               EscribirLetraxLetra(SexoDetallePosX + (ejex * 9), SexoDetallePosY, ejex, SexoDetalle, cb, document);

               //Titular
               if (dt.Rows[0]["PreNombres"] != null) PreNombres = dt.Rows[0]["PreNombres"].ToString();
               if (dt.Rows[0]["PrimerApellido"] != null) PrimerApellido = dt.Rows[0]["PrimerApellido"].ToString();
               if (dt.Rows[0]["SegundoApellido"] != null) SegundoApellido = dt.Rows[0]["SegundoApellido"].ToString();
               EscribirLetraxLetra(PreNombresPosX + (ejex * 9), PreNombresPosY, ejex, PreNombres, cb, document);
               EscribirLetraxLetra(PrimerApellidoPosX + (ejex * 9), PrimerApellidoPosY, ejex, PrimerApellido, cb, document);
               EscribirLetraxLetra(SegundoApellidoPosX + (ejex * 9), SegundoApellidoPosY, ejex, SegundoApellido, cb, document);


               //Padre
               if (dt.Rows[0]["PadrePreNombres"] != null) PadrePreNombres = dt.Rows[0]["PadrePreNombres"].ToString();
               if (dt.Rows[0]["PadrePrimerApellido"] != null) PadrePrimerApellido = dt.Rows[0]["PadrePrimerApellido"].ToString();
               if (dt.Rows[0]["PadreSegundoApellido"] != null) PadreSegundoApellido = dt.Rows[0]["PadreSegundoApellido"].ToString();
               if (dt.Rows[0]["PadreTipoDocumento"] != null) PadreTipoDocumento = dt.Rows[0]["PadreTipoDocumento"].ToString();
               if (dt.Rows[0]["PadreNumeroDocumento"] != null) PadreNumeroDocumento = dt.Rows[0]["PadreNumeroDocumento"].ToString();
               if (dt.Rows[0]["PadreNacionalidad"] != null) PadreNacionalidad = dt.Rows[0]["PadreNacionalidad"].ToString();
               EscribirLetraxLetra(PadrePreNombresPosX + (ejex * 9), PadrePreNombresPosY, ejex, PadrePreNombres, cb, document);
               EscribirLetraxLetra(PadrePrimerApellidoPosX + (ejex * 9), PadrePrimerApellidoPosY, ejex, PadrePrimerApellido, cb, document);
               EscribirLetraxLetra(PadreSegundoApellidoPosX + (ejex * 9), PadreSegundoApellidoPosY, ejex, PadreSegundoApellido, cb, document);
               EscribirLetraxLetra(PadreTipoDocumentoPosX + (ejex * 9), PadreTipoDocumentoPosY, ejex, PadreTipoDocumento, cb, document);
               EscribirLetraxLetra(PadreNumeroDocumentoPosX + (ejex * 9), PadreNumeroDocumentoPosY, ejex, PadreNumeroDocumento, cb, document);
               EscribirLetraxLetra(PadreNacionalidadPosX + (ejex * 9), PadreNacionalidadPosY, ejex, PadreNacionalidad, cb, document);

               //MADRE
               if (dt.Rows[0]["MadrePreNombres"] != null) MadrePreNombres = dt.Rows[0]["MadrePreNombres"].ToString();
               if (dt.Rows[0]["MadrePrimerApellido"] != null) MadrePrimerApellido = dt.Rows[0]["MadrePrimerApellido"].ToString();
               if (dt.Rows[0]["MadreSegundoApellido"] != null) MadreSegundoApellido = dt.Rows[0]["MadreSegundoApellido"].ToString();
               if (dt.Rows[0]["MadreTipoDocumento"] != null) MadreTipoDocumento = dt.Rows[0]["MadreTipoDocumento"].ToString();
               if (dt.Rows[0]["MadreNumeroDocumento"] != null) MadreNumeroDocumento = dt.Rows[0]["MadreNumeroDocumento"].ToString();
               if (dt.Rows[0]["MadreNacionalidad"] != null) MadreNacionalidad = dt.Rows[0]["MadreNacionalidad"].ToString();
               if (dt.Rows[0]["Direccion"] != null) Direccion = dt.Rows[0]["Direccion"].ToString();

               if (dt.Rows[0]["DireccionUbigeoDep"] != null) DireccionUbigeoDep = dt.Rows[0]["DireccionUbigeoDep"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["DireccionUbigeoDepDetalle"] != null) DireccionUbigeoDepDetalle = dt.Rows[0]["DireccionUbigeoDepDetalle"].ToString();
               if (dt.Rows[0]["DireccionUbigeoPrv"] != null) DireccionUbigeoPrv = dt.Rows[0]["DireccionUbigeoPrv"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["DireccionUbigeoPrvDetalle"] != null) DireccionUbigeoPrvDetalle = dt.Rows[0]["DireccionUbigeoPrvDetalle"].ToString();
               if (dt.Rows[0]["DireccionUbigeoDst"] != null) DireccionUbigeoDst = dt.Rows[0]["DireccionUbigeoDst"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["DireccionUbigeoDstDetalle"] != null) DireccionUbigeoDstDetalle = dt.Rows[0]["DireccionUbigeoDstDetalle"].ToString();
               EscribirLetraxLetra(MadrePreNombresPosX + (ejex * 9), MadrePreNombresPosY, ejex, MadrePreNombres, cb, document);
               EscribirLetraxLetra(MadrePrimerApellidoPosX + (ejex * 9), MadrePrimerApellidoPosY, ejex, MadrePrimerApellido, cb, document);
               EscribirLetraxLetra(MadreSegundoApellidoPosX + (ejex * 9), MadreSegundoApellidoPosY, ejex, MadreSegundoApellido, cb, document);
               EscribirLetraxLetra(MadreTipoDocumentoPosX + (ejex * 9), MadreTipoDocumentoPosY, ejex, MadreTipoDocumento, cb, document);
               EscribirLetraxLetra(MadreNumeroDocumentoPosX + (ejex * 9), MadreNumeroDocumentoPosY, ejex, MadreNumeroDocumento, cb, document);
               EscribirLetraxLetra(MadreNacionalidadPosX + (ejex * 9), MadreNacionalidadPosY, ejex, MadreNacionalidad, cb, document);
               EscribirLetraxLetra(DireccionPosX + (ejex * 9), DireccionPosY, ejex, Direccion, cb, document);

               EscribirLetraxLetra(DireccionUbigeoDepPosX + (ejex * 9), DireccionUbigeoDepPosY, ejex, DireccionUbigeoDep, cb, document);
               EscribirLetraxLetra(DireccionUbigeoDepDetallePosX + (ejex * 9), DireccionUbigeoDepDetallePosY, ejex, DireccionUbigeoDepDetalle, cb, document);
               EscribirLetraxLetra(DireccionUbigeoPrvPosX + (ejex * 9), DireccionUbigeoPrvPosY, ejex, DireccionUbigeoPrv, cb, document);
               EscribirLetraxLetra(DireccionUbigeoPrvDetallePosX + (ejex * 9), DireccionUbigeoPrvDetallePosY, ejex, DireccionUbigeoPrvDetalle, cb, document);
               EscribirLetraxLetra(DireccionUbigeoDstPosX + (ejex * 9), DireccionUbigeoDstPosY, ejex, DireccionUbigeoDst, cb, document);
               EscribirLetraxLetra(DireccionUbigeoDstDetallePosX + (ejex * 9), DireccionUbigeoDstDetallePosY, ejex, DireccionUbigeoDstDetalle, cb, document);


               //REGISTRADOR
               DateTime fechaRegistrador = Comun.FormatearFecha(dt.Rows[0]["Fecha"].ToString());
               RegistroFecha = fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.Month.ToString().PadLeft(2, '0') + " " + fecha.Year.ToString();

               if (dt.Rows[0]["RegistroUbigeoDep"] != null) RegisTroUbigeoDep = dt.Rows[0]["RegistroUbigeoDep"].ToString();
               if (dt.Rows[0]["RegistroUbigeoDepDetalle"] != null) RegistroUbigeoDetalle = dt.Rows[0]["RegistroUbigeoDepDetalle"].ToString();
               if (dt.Rows[0]["RegistroUbigeoPrv"] != null) RegistroUbigeoPrv = dt.Rows[0]["RegistroUbigeoPrv"].ToString();
               if (dt.Rows[0]["RegistroUbigeoPrvDetalle"] != null) RegistroUbigeoPrvDetalle = dt.Rows[0]["RegistroUbigeoPrvDetalle"].ToString();
               if (dt.Rows[0]["RegistroUbigeoDst"] != null) RegistroUbigeoDst = dt.Rows[0]["RegistroUbigeoDst"].ToString();
               if (dt.Rows[0]["RegistroUbigeoDstDetalle"] != null) RegistroUbigeoDstDetalle = dt.Rows[0]["RegistroUbigeoDstDetalle"].ToString();

               if (dt.Rows[0]["RegistradorPrenombres"] != null) RegistradorPrenombres = dt.Rows[0]["RegistradorPrenombres"].ToString();
               if (dt.Rows[0]["RegistradorPrimerApellido"] != null) RegistradorPrimerApellido = dt.Rows[0]["RegistradorPrimerApellido"].ToString();
               if (dt.Rows[0]["RegistradorSegundoApellido"] != null) RegistradorSegundoApellido = dt.Rows[0]["RegistradorSegundoApellido"].ToString();

               if (dt.Rows[0]["RegistradorNumeroDocumento"] != null) RegistradorNumeroDocumento = dt.Rows[0]["RegistradorNumeroDocumento"].ToString();
               
               RegistradorNombresCompletos = RegistradorPrimerApellido + " " + RegistradorSegundoApellido + " " + RegistradorPrenombres;
               
               EscribirLetraxLetra(RegistroFechaPosX + (ejex * 9), RegistroFechaPosY, ejex, RegistroFecha, cb, document);
               EscribirLetraxLetra(RegisTroUbigeoDepPosX + (ejex * 9), RegisTroUbigeoDepPosY, ejex, RegisTroUbigeoDep, cb, document);
               EscribirLetraxLetra(RegistroUbigeoDetallePosX + (ejex * 9), RegistroUbigeoDetallePosY, ejex, RegistroUbigeoDetalle, cb, document);
               EscribirLetraxLetra(RegistroUbigeoPrvPosX + (ejex * 9), RegistroUbigeoPrvPosY, ejex, RegistroUbigeoPrv, cb, document);
               EscribirLetraxLetra(RegistroUbigeoPrvDetallePosX + (ejex * 9), RegistroUbigeoPrvDetallePosY, ejex, RegistroUbigeoPrvDetalle, cb, document);
               EscribirLetraxLetra(RegistroUbigeoDstPosX + (ejex * 9), RegistroUbigeoDstPosY, ejex, RegistroUbigeoDst, cb, document);
               EscribirLetraxLetra(RegistroUbigeoDstDetallePosX + (ejex * 9), RegistroUbigeoDstDetallePosY, ejex, RegistroUbigeoDstDetalle, cb, document);

               EscribirLetraxLetra(RegistradorNombresCompletosPosX + (ejex * 9), RegistradorNombresCompletosPosY, ejex, RegistradorNombresCompletos, cb, document);
               EscribirLetraxLetra(RegistradorNumeroDocumentoPosX + (ejex * 9), RegistradorNumeroDocumentoPosY, ejex, RegistradorNumeroDocumento, cb, document);
               

               //DECLARANTE 1

               if (dt.Rows[0]["Declarante1Vinculo"] != null) Declarante1Vinculo = dt.Rows[0]["Declarante1Vinculo"].ToString();
               if (dt.Rows[0]["Declarante1Prenombres"] != null) Declarante1Prenombres = dt.Rows[0]["Declarante1Prenombres"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["Declarante1PrimerApellido"] != null) Declarante1PrimerApellido = dt.Rows[0]["Declarante1PrimerApellido"].ToString();
               if (dt.Rows[0]["Declarante1SegundoApellido"] != null) Declarante1SegundoApellido = dt.Rows[0]["Declarante1SegundoApellido"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["Declarante1TipoDocumento"] != null) Declarante1TipoDocumento = dt.Rows[0]["Declarante1TipoDocumento"].ToString();
               if (dt.Rows[0]["Declarante1NumeroDocumento"] != null) Declarante1NumeroDocumento = dt.Rows[0]["Declarante1NumeroDocumento"].ToString().PadLeft(2, '0');

               EscribirLetraxLetra(Declarante1VinculoPosX + (ejex * 9), Declarante1VinculoPosY, ejex, Declarante1Vinculo, cb, document);
               EscribirLetraxLetra(Declarante1PrenombresPosX + (ejex * 9), Declarante1PrenombresPosY, ejex, Declarante1Prenombres, cb, document);
               EscribirLetraxLetra(Declarante1PrimerApellidoPosX + (ejex * 9), Declarante1PrimerApellidoPosY, ejex, Declarante1PrimerApellido, cb, document);
               EscribirLetraxLetra(Declarante1SegundoApellidoPosX + (ejex * 9), Declarante1SegundoApellidoPosY, ejex, Declarante1SegundoApellido, cb, document);
               EscribirLetraxLetra(Declarante1TipoDocumentoPosX + (ejex * 9), Declarante1TipoDocumentoPosY, ejex, Declarante1TipoDocumento, cb, document);
               EscribirLetraxLetra(Declarante1NumeroDocumentoPosX + (ejex * 9), Declarante1NumeroDocumentoPosY, ejex, Declarante1NumeroDocumento, cb, document);

               //DECLARANTE 2

               if (dt.Rows[0]["Declarante2Vinculo"] != null) Declarante2Vinculo = dt.Rows[0]["Declarante2Vinculo"].ToString();
               if (dt.Rows[0]["Declarante2Prenombres"] != null) Declarante2Prenombres = dt.Rows[0]["Declarante2Prenombres"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["Declarante2PrimerApellido"] != null) Declarante2PrimerApellido = dt.Rows[0]["Declarante2PrimerApellido"].ToString();
               if (dt.Rows[0]["Declarante2SegundoApellido"] != null) Declarante2SegundoApellido = dt.Rows[0]["Declarante2SegundoApellido"].ToString().PadLeft(2, '0');
               if (dt.Rows[0]["Declarante2TipoDocumento"] != null) Declarante2TipoDocumento = dt.Rows[0]["Declarante2TipoDocumento"].ToString();
               if (dt.Rows[0]["Declarante2NumeroDocumento"] != null) Declarante2NumeroDocumento = dt.Rows[0]["Declarante2NumeroDocumento"].ToString().PadLeft(2, '0');
               EscribirLetraxLetra(Declarante2VinculoPosX + (ejex * 9), Declarante2VinculoPosY, ejex, Declarante2Vinculo, cb, document);
               EscribirLetraxLetra(Declarante2PrenombresPosX + (ejex * 9), Declarante2PrenombresPosY, ejex, Declarante2Prenombres, cb, document);
               EscribirLetraxLetra(Declarante2PrimerApellidoPosX + (ejex * 9), Declarante2PrimerApellidoPosY, ejex, Declarante2PrimerApellido, cb, document);
               EscribirLetraxLetra(Declarante2SegundoApellidoPosX + (ejex * 9), Declarante2SegundoApellidoPosY, ejex, Declarante2SegundoApellido, cb, document);
               EscribirLetraxLetra(Declarante2TipoDocumentoPosX + (ejex * 9), Declarante2TipoDocumentoPosY, ejex, Declarante2TipoDocumento, cb, document);
               EscribirLetraxLetra(Declarante2NumeroDocumentoPosX + (ejex * 9), Declarante2NumeroDocumentoPosY, ejex, Declarante2NumeroDocumento, cb, document);

               //DECLARANTE 1
               if (dt.Rows[0]["Observaciones"] != null) Observaciones = dt.Rows[0]["Observaciones"].ToString();

               cb.SetTextMatrix(ObservacionesPosX, document.PageSize.Height - ObservacionesPosY);
               cb.ShowText(Observaciones.ToString());

               cb.EndText();


               document.Close();
               oStreamReader.Close();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

    }
 
}