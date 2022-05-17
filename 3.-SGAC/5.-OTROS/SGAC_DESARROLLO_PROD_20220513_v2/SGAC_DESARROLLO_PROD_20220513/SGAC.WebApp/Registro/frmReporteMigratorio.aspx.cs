using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using SGAC.Registro.Actuacion.BL;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.IO;
using System.Drawing;
using MW6PDF417;
using System.Drawing.Imaging;

namespace SGAC.WebApp.Registro
{
    public partial class frmReporteMigratorio : MyBasePage
    {
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

        public string SSRS = ConfigurationManager.AppSettings["SSRS"];
        protected void Page_Load(object sender, EventArgs e)
        {
            hdn_Tipo.Value = Request.QueryString["vClass"];

            if (!IsPostBack)
            {
                string StrNombreArchReporte = string.Empty;

                Enumerador.enmMigratorioFormato enmAccion = (Enumerador.enmMigratorioFormato)Session["iTipo_Reporte"];

                #region - Formato DGC -

                if (Convert.ToString(Request.QueryString["vClass"]).Equals("1"))
                {
                    switch (enmAccion)
                    {
                        case Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO:
                            StrNombreArchReporte = "rsMigratorioDGC001.rdlc";
                            break;
                        case Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO:
                            StrNombreArchReporte = "rsMigratorioDGC002.rdlc";
                            break;

                        case Enumerador.enmMigratorioFormato.DGC_003_PASAPORTE_ANULADO:
                            StrNombreArchReporte = "rsMigratorioDGC003.rdlc";
                            break;
                        case Enumerador.enmMigratorioFormato.DGC_004_PASAPORTE_BAJA:
                            StrNombreArchReporte = "rsMigratorioDGC004.rdlc";
                            break;

                        case Enumerador.enmMigratorioFormato.DGC_005_VISA:
                            string s_Tipo = Convert.ToString(Request.QueryString["VisaTipo"]);
                            if (s_Tipo != null)
                            {
                                switch (s_Tipo)
                                {
                                    case "DIPLOMATICA":
                                        StrNombreArchReporte = "rsMigratorioDGC005.rdlc";
                                        break;
                                    case "PRENSA":
                                        StrNombreArchReporte = "rsMigratorioDGC005_3.rdlc";
                                        break;
                                    default:
                                        StrNombreArchReporte = "rsMigratorioDGC005_2.rdlc";
                                        break;
                                }
                            }
                            else
                                StrNombreArchReporte = "rsMigratorioDGC005_2.rdlc";

                            break;
                        case Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO:
                            StrNombreArchReporte = "rsMigratorioDGC006.rdlc";
                            break;
                    }

                    Imprimir_Migratorio(StrNombreArchReporte);
                }

                #endregion

                #region - Formato Lámina -

                if (Convert.ToString(Request.QueryString["vClass"]).Equals("2"))
                {
                    switch (enmAccion)
                    {
                        case Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO_LAMINA:
                            StrNombreArchReporte = "rsLaminaDGC001.rdlc";
                            break;
                        case Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO_LAMINA:
                            StrNombreArchReporte = "rsLaminaDGC001.rdlc";
                            break;

                        case Enumerador.enmMigratorioFormato.DGC_003_PASAPORTE_ANULADO_LAMINA:
                            StrNombreArchReporte = "rsLaminaDGC005.rdlc";
                            break;
                        case Enumerador.enmMigratorioFormato.DGC_004_PASAPORTE_BAJA_LAMINA:
                            StrNombreArchReporte = "rsLaminaDGC005.rdlc";
                            break;

                        case Enumerador.enmMigratorioFormato.DGC_005_VISA_LAMINA:
                            StrNombreArchReporte = "rsLaminaDGC005.rdlc";
                            break;
                        case Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO_LAMINA:
                            StrNombreArchReporte = "rsLaminaDGC006.rdlc";
                            break;
                    }

                    Imprimir_Lamina(StrNombreArchReporte);
                }

                #endregion

                #region - Formato Baja / Anulación -

                if (Convert.ToString(Request.QueryString["vClass"]).Equals("3"))
                {
                    switch (enmAccion)
                    {
                        case Enumerador.enmMigratorioFormato.DGC_004_PASAPORTE_BAJA:
                            StrNombreArchReporte = "rsMigratorioDGC004.rdlc";
                            Imprimir_Baja(StrNombreArchReporte, Comun.ToNullInt64(Request.QueryString["vActo"]));
                            break;
                        case Enumerador.enmMigratorioFormato.DGC_003_PASAPORTE_ANULADO:
                            StrNombreArchReporte = "rsMigratorioDGC003.rdlc";
                            Imprimir_Anulacion(StrNombreArchReporte, Comun.ToNullInt64(Request.QueryString["vActo"]));
                            break;
                    }
                }

                #endregion

                #region - Formato de Entrega -
                if (Convert.ToString(Request.QueryString["vClass"]).Equals("4"))
                {
                    switch (enmAccion)
                    {
                        case Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO:
                        case Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO:
                        case Enumerador.enmMigratorioFormato.DGC_003_PASAPORTE_ANULADO:
                        case Enumerador.enmMigratorioFormato.DGC_004_PASAPORTE_BAJA:
                        case Enumerador.enmMigratorioFormato.DGC_005_VISA:
                        case Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO:
                            StrNombreArchReporte = "rsActaConformidad.rdlc";
                            break;
                    }

                    Imprimir_Migratorio(StrNombreArchReporte);
                }
                #endregion

            }
        }

        private void Imprimir_Lamina(string StrNombreArchReporte)
        {
            DataTable dt = new DataTable();
            ActoMigratorioConsultaBL objBL = new ActoMigratorioConsultaBL();

            Int64 iAcutacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            Int64 iActoMigratorioId = Convert.ToInt64(Session["Acto_Migratorio_ID"]);


            dt = objBL.FormatoMigratorio_Lamina(iAcutacionDetalleId, iActoMigratorioId);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            string imagenFotoURL = string.Empty;
            string imagenFirmaURL = string.Empty;
            if (dt.Rows.Count > 0)
            {
                imagenFotoURL = uploadPath + @"\" + Convert.ToString(dt.Rows[0]["imagenFotoNombre"].ToString());
                imagenFirmaURL = uploadPath + @"\" + Convert.ToString(dt.Rows[0]["imagenFirmaNombre"].ToString());
            }

            string strCadenaCodifica = dt.Rows[0]["vLinea_3"].ToString();
            string strCadenaCifrada = string.Empty;
            string strLLaveCifrado = "010419762005";
            string strVectorInicial = "1234567891234567";
            strCadenaCifrada = Util.cifrarTextoAES(strCadenaCodifica, strLLaveCifrado, strLLaveCifrado, "MD5", 22, strVectorInicial, 128);

            System.Drawing.Image objLienzo;
            objLienzo = GenerarImagenPDF417(strCadenaCodifica);
            objLienzo.Save(uploadPath + @"\PDF417.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            GenerarImagenTransparente(imagenFotoURL);

            ReportParameter[] parameters = parameters = new ReportParameter[4];//Transparente
            parameters[0] = new ReportParameter("foto", "file:" + imagenFotoURL);
            parameters[1] = new ReportParameter("firma", "file:" + imagenFirmaURL);
            parameters[2] = new ReportParameter("PDF417", "file:" + uploadPath + @"\PDF417.bmp");
            parameters[3] = new ReportParameter("Transparente", "file:" + uploadPath + @"\Transparente.png");

            rsReporte.LocalReport.ReportEmbeddedResource = "Reportes\\RSMigratorio\\" + StrNombreArchReporte;
            rsReporte.LocalReport.ReportPath = @"Reportes\\RSMigratorio\\" + StrNombreArchReporte;

            Session.Remove("Acto_Migratorio_ID");
            rsReporte.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource("dsMigratorio", dt);

            rsReporte.LocalReport.EnableExternalImages = true;
            rsReporte.LocalReport.SetParameters(parameters);
            rsReporte.LocalReport.DataSources.Add(datasource);
        }

        private void Imprimir_Baja(string StrNombreArchReporte, Int64 iActoMigratorioHistorico)
        {
            DataTable dt = new DataTable();
            ActoMigratorioConsultaBL objBL = new ActoMigratorioConsultaBL();

            Int64 iAcutacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            Int64 iActoMigratorioId = Convert.ToInt64(Session["Acto_Migratorio_ID"]);

            dt = objBL.FormatoMigratorio_Baja(iAcutacionDetalleId, iActoMigratorioId, iActoMigratorioHistorico);


            rsReporte.LocalReport.ReportEmbeddedResource = "Reportes\\RSMigratorio\\" + StrNombreArchReporte;
            rsReporte.LocalReport.ReportPath = @"Reportes\\RSMigratorio\\" + StrNombreArchReporte;

            Session.Remove("Acto_Migratorio_ID");
            rsReporte.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource("dsMigratorio", dt);

            rsReporte.LocalReport.EnableExternalImages = true;

            rsReporte.LocalReport.DataSources.Add(datasource);
        }

        private void Imprimir_Anulacion(string StrNombreArchReporte, Int64 iActoMigratorioHistorico)
        {
            DataTable dt = new DataTable();
            ActoMigratorioConsultaBL objBL = new ActoMigratorioConsultaBL();

            Int64 iAcutacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            Int64 iActoMigratorioId = Convert.ToInt64(Session["Acto_Migratorio_ID"]);

            dt = objBL.FormatoMigratorio_Anulado(iAcutacionDetalleId, iActoMigratorioId, iActoMigratorioHistorico);


            rsReporte.LocalReport.ReportEmbeddedResource = "Reportes\\RSMigratorio\\" + StrNombreArchReporte;
            rsReporte.LocalReport.ReportPath = @"Reportes\\RSMigratorio\\" + StrNombreArchReporte;

            Session.Remove("Acto_Migratorio_ID");
            rsReporte.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource("dsMigratorio", dt);

            rsReporte.LocalReport.EnableExternalImages = true;

            rsReporte.LocalReport.DataSources.Add(datasource);
        }

        private void Imprimir_Migratorio(string StrNombreArchReporte)
        {
            DataTable dt = new DataTable();
            ActoMigratorioConsultaBL objBL = new ActoMigratorioConsultaBL();

            Int64 iAcutacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
            Int64 iActoMigratorioId = Convert.ToInt64(Session["Acto_Migratorio_ID"]);

            dt = objBL.FormatoMigratorio(iAcutacionDetalleId, iActoMigratorioId);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            string imagenFotoURL = string.Empty;
            string imagenHuellaURL = string.Empty;
            string imagenFirmaURL = string.Empty;

            int autoriza = 0;
            if (dt.Rows.Count > 0)
            {
                imagenFotoURL = uploadPath + @"\" + Convert.ToString(dt.Rows[0]["imagenFotoNombre"]);
                imagenHuellaURL = "file:" + uploadPath + @"\" + Convert.ToString(dt.Rows[0]["imagenHuellaNombre"]);
                imagenFirmaURL = "file:" + uploadPath + @"\" + Convert.ToString(dt.Rows[0]["imagenFirmaNombre"]);

                if (dt.Rows[0]["sTipoAutorizacionId"].ToString() == "")
                {
                    autoriza = 0;
                }
                else
                {
                    autoriza = Convert.ToInt32(dt.Rows[0]["sTipoAutorizacionId"].ToString());
                }

                switch (autoriza)
                {
                    case (int)Enumerador.enmVisaAutorizacion.MISION_CONSULAR:
                        dt.Rows[0]["vAutoMisionSecionConsular"] = "X";
                        break;
                    case (int)Enumerador.enmVisaAutorizacion.MRE_TRC:
                        dt.Rows[0]["vAutoTRC"] = "X";
                        break;
                    case (int)Enumerador.enmVisaAutorizacion.MRE_CON:
                        dt.Rows[0]["vAutoCON"] = "X";
                        break;
                    case (int)Enumerador.enmVisaAutorizacion.MRE_DGC:
                        dt.Rows[0]["vAutoDGC"] = "X";
                        break;
                    case (int)Enumerador.enmVisaAutorizacion.MRE_PRI:
                        dt.Rows[0]["vAutoPRI"] = "X";
                        break;
                    case (int)Enumerador.enmVisaAutorizacion.MRE_OGC:
                        dt.Rows[0]["vAutoOGC"] = "X";
                        break;
                }
                dt.AcceptChanges();
            }


            ReportParameter[] parameters = parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("foto", "file:" + imagenFotoURL);
            parameters[1] = new ReportParameter("huella", imagenHuellaURL);
            parameters[2] = new ReportParameter("firma", imagenFirmaURL);


            rsReporte.LocalReport.ReportEmbeddedResource = "Reportes\\RSMigratorio\\" + StrNombreArchReporte;
            rsReporte.LocalReport.ReportPath = @"Reportes\\RSMigratorio\\" + StrNombreArchReporte;

            Session.Remove("Acto_Migratorio_ID");
            rsReporte.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource("dsMigratorio", dt);

            rsReporte.LocalReport.EnableExternalImages = true;
            rsReporte.LocalReport.SetParameters(parameters);
            rsReporte.LocalReport.DataSources.Add(datasource);


        }

        #region Métodos

        public static Byte[] IMG_CargarImagen(string rutaArchivo)
        {
            if (rutaArchivo != "")
            {
                try
                {
                    FileStream Archivo = new FileStream(rutaArchivo, FileMode.Open);//Creo el archivo
                    BinaryReader binRead = new BinaryReader(Archivo);       //Cargo el Archivo en modo binario
                    Byte[] imagenEnBytes = new Byte[(Int64)Archivo.Length]; //Creo un Array de Bytes donde guardare la imagen
                    binRead.Read(imagenEnBytes, 0, (int)Archivo.Length);    //Cargo la imagen en el array de Bytes
                    binRead.Close();
                    Archivo.Close();
                    return imagenEnBytes;                                   //Devuelvo la imagen convertida en un array de bytes
                }
                catch
                {
                    return new Byte[0];
                }
            }
            return new byte[0];
        }



        void GenerarCodigo(ref string strMensaje, ModoProceso objModoProceso, ErrorCorreccion objErroCorreccion,
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

        Bitmap GenerarImagenPDF417(string strTexto)
        {
            Bitmap objLienzo;
            Graphics objGraficar;
            System.Drawing.Font objFont;
            Point objCoordenadas;
            SolidBrush objPincelFondo;
            SolidBrush objPincelTexto;

            GenerarCodigo(ref strTexto, ModoProceso.Binario, ErrorCorreccion.Nivel4, 2, 6, false, false, 11, Fuente.MW6_PDF417R4);

            objLienzo = new Bitmap(600, 400);
            objGraficar = Graphics.FromImage(objLienzo);
            objFont = new System.Drawing.Font("MW6 PDF417R4", 11);
            objCoordenadas = new Point(5, 5);
            objPincelFondo = new SolidBrush(System.Drawing.Color.White);
            objPincelTexto = new SolidBrush(System.Drawing.Color.Black);

            objGraficar.FillRectangle(objPincelFondo, 0, 0, 600, 400);
            objGraficar.DrawString(strTexto, objFont, objPincelTexto, objCoordenadas);

            return objLienzo;
        }

        void GenerarImagenTransparente(string inputFileName)
        {
            Bitmap bmpIn = (Bitmap)Bitmap.FromFile(inputFileName);
            System.Drawing.Image converted = ChangeImageOpacity((System.Drawing.Image)bmpIn, 135);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            converted.Save(uploadPath + @"\Transparente.png", ImageFormat.Png);
        }

        private const int bytesPerPixel = 4;
        public System.Drawing.Image ChangeImageOpacity(System.Drawing.Image originalImage, double opacity)
        {
            if ((originalImage.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
            {
                // Cannot modify an image with indexed colors
                return originalImage;
            }

            Bitmap bmp = (Bitmap)originalImage.Clone();

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 32 bits per pixels 
            // (32 bits = 4 bytes, 3 for RGB and 1 byte for alpha).
            int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];

            // Copy the ARGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the
            // RGB values for all pixels in the the bitmap.
            for (int counter = 0; counter < argbValues.Length; counter += bytesPerPixel)
            {
                // argbValues is in format BGRA (Blue, Green, Red, Alpha)

                // If 100% transparent, skip pixel
                if (argbValues[counter + bytesPerPixel - 1] == 0)
                    continue;

                int pos = 0;
                pos++; // B value
                pos++; // G value
                pos++; // R value

                argbValues[counter + pos] = (byte)(argbValues[counter + pos] * opacity);
            }

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        #endregion

        protected void btn_Imprimir_Click(object sender, EventArgs e)
        {
            string script = string.Empty;
            try
            {
                script = "$(function){{";
                script = script + "var myWindow = window.open(\"\", \"myWindow\", \"width=200px\", \"height=100px\");";
                script = script + "myWindow.document.write(\"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">\");";
                script = script + "myWindow.document.write(<HTML>);";
                script = script + "myWindow.document.write(     myWindow.document.write(\"<HEAD>\");";
                script = script + "myWindow.document.write(     myWindow.document.write(\"<META content=\"IE=5.0000\" http-equiv=\"X-UA-Compatible\">\");";
                script = script + "myWindow.document.write(     myWindow.document.write(\"</HEAD>\");";
                script = script + "myWindow.document.write(</HTML>);";
                script = script + "}});";


                script = string.Format(script);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Cargar_Popup", script, true);
            }
            catch
            {
            }
        }

    }
}