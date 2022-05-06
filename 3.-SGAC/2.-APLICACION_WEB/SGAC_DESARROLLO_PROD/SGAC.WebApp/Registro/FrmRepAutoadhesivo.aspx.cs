using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Security.Application;
using MW6PDF417;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.BL;
using SGAC.WebApp.Accesorios;


namespace SGAC.WebApp.Registro
{
    public partial class FrmRepAutoadesivo : Page
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

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //----------------------------------------------------
            //Fecha: 15/02/2017
            //Autor: Sandra del Carmen Acosta Celis
            //Objetivo: Visibilidad del botón Imprimir del Autoadhesivo que se genera en la pantalla Consulta de Autoadhesivo.
            //----------------------------------------------------

            object dtActuacionesValorRespuesta;

            dtActuacionesValorRespuesta = Session["dtActuacionesValor"];

            if (Session["dtActuacionesValor"] == dtActuacionesValorRespuesta && Session["dtActuacionesValor"] != null)
            {
                btnImprimirAutoAdhesivo.Visible = false;
                Session.Remove("dtActuacionesValor");
            }
            else
            {
                btnImprimirAutoAdhesivo.Visible = true;
            }

            string strFontSize = "9";
            string strMarginTop = "75";
            string strMarginLeft = "60";
            
            if (System.Configuration.ConfigurationManager.AppSettings["HTML_FontSize"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_FontSize"].ToString() != string.Empty)
                    strFontSize = System.Configuration.ConfigurationManager.AppSettings["HTML_FontSize"].ToString();
            }
            if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginLeft"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginLeft"].ToString() != string.Empty)
                    strMarginLeft = System.Configuration.ConfigurationManager.AppSettings["HTML_MarginLeft"].ToString();
            }
            if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginTop"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginTop"].ToString() != string.Empty)

                    strMarginTop = System.Configuration.ConfigurationManager.AppSettings["HTML_MarginTop"].ToString();
            }
     

            hdnFontSize.Value = strFontSize;
            hdnMarginLeft.Value = strMarginLeft;
            hdnMarginTop.Value = strMarginTop;
            if (!Page.IsPostBack)
            {
                //if (Request.QueryString["GUID"] != null)
                //{
                //    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                //}
                //else
                //{
                //    HFGUID.Value = "";
                //}
                if (Request.QueryString["iDetalleActuacion"] != null)
                {
                    HIdActuacionDetalleId.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["iDetalleActuacion"].ToString());
                }
                else
                {
                    HIdActuacionDetalleId.Value = "";
                }
                if (Request.QueryString["ActuExtra"] != null)
                {
                    HIdActuacionExtra.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["ActuExtra"].ToString());
                }
                else
                {
                    HIdActuacionExtra.Value = "";
                }
                if (Request.QueryString["Actuacion"] != null)
                {
                    HIdActuacion.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["Actuacion"].ToString());
                }
                else
                {
                    HIdActuacion.Value = "";
                }
                
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
 
 
        }

        

        #endregion

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

        static Bitmap GenerarImagenPDF417(string strTexto)
        {
            Bitmap objLienzo;
            Graphics objGraficar ;
            System.Drawing.Font objFont;
            Point objCoordenadas;
            SolidBrush objPincelFondo;
            SolidBrush objPincelTexto;

            GenerarCodigo(ref strTexto, ModoProceso.Binario, ErrorCorreccion.Nivel4, 2, 6, false, false, 11, Fuente.MW6_PDF417R4);

            objLienzo = new Bitmap(600,400);
            objGraficar = Graphics.FromImage(objLienzo);
            objFont = new System.Drawing.Font("MW6 PDF417R4", 11);
            //objFont = new System.Drawing.Font("PDF417", 11);
            objCoordenadas = new Point(5, 5);
            objPincelFondo = new SolidBrush(System.Drawing.Color.White);
            objPincelTexto = new SolidBrush(System.Drawing.Color.Black);

            objGraficar.FillRectangle(objPincelFondo, 0, 0,600, 400);
            objGraficar.DrawString(strTexto, objFont, objPincelTexto, objCoordenadas);

            return objLienzo;
        }

        #endregion

        [System.Web.Services.WebMethod]
        public static void SetSession(string variable, string valor)
        {

            if (valor.ToLower() == "true")
            {
                HttpContext.Current.Session[variable] = true;
            }
            else if (valor.ToLower() == "false")
            {
                HttpContext.Current.Session[variable] = false;
            }
            else
            {
                HttpContext.Current.Session[variable] = valor;
            }
        }


        [System.Web.Services.WebMethod]
        public static String ImpresionCorrecta(string IdActuacionDetalleId, string IdActuacionExtraId, string IdActuacionId)
        {

            if (Convert.ToInt64(HttpContext.Current.Session["ID_ACTUACION_VERIFICAR"]) == 0)
            {
                HttpContext.Current.Session["ID_ACTUACION_VERIFICAR"] = null;
                return "HUBO UN PROBLEMA PARA LA OBTENER DATOS DEL SISTEMA, POR FAVOR REFRESQUE LA PAGINA.";
            }
            else {
                HttpContext.Current.Session["ID_ACTUACION_VERIFICAR"] = null;
                if (IdActuacionDetalleId == "")
                {
                    IdActuacionDetalleId = HttpContext.Current.Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID].ToString();
                }

                if (IdActuacionExtraId != "")
                {
                    HttpContext.Current.Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] = IdActuacionExtraId;
                }
                if (IdActuacionId != "")
                {
                    HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACION_ID] = IdActuacionId;
                }
                
                ActuacionMantenimientoBL objAct = new ActuacionMantenimientoBL();
                String Msj = String.Empty;

                Int16 sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                Int16 sUsuarioId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);


                objAct.USP_RE_ACTUACIONINSUMODETALLE_ACTUALIZAR_IMPRESION(Convert.ToInt64(IdActuacionDetalleId), true, sUsuarioId, sOficinaConsularId, ref Msj);

                if (Msj==String.Empty)
                {
                    Msj = "OK";
                }
                
                return Msj;
            }
        }

        [System.Web.Services.WebMethod]
        public static String Imprimir(string IdActuacionDetalleId)
        {

            try
            {
                String html = String.Empty;
             
                DataTable DtData = new DataTable("Sticker");
                DataTable DtDatosSticker = new DataTable();
                DataRow newRow = DtData.NewRow();
                ActuacionConsultaBL FunSticker = new ActuacionConsultaBL();
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                String FormatoFechas = System.Configuration.ConfigurationManager.AppSettings["FormatoFechas"].ToString();

                if (HttpContext.Current.Session["ACTO_NOTARIAL"] != null)
                {
                    if (Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACION_ID]) == 0)
                    {
                        return "";
                    }

                    long lngActuacionId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACION_ID]);
                    //long IdActuacionDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    if (IdActuacionDetalleId == "")
                    {
                        IdActuacionDetalleId = HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID].ToString();
                    }
                    Int32 iLimiteActuacion = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CantidadTarifaAutodhesivo"].ToString());
                    DtDatosSticker = FunSticker.ObtenerDatosAutoadhesivoNotarial(lngActuacionId,Convert.ToInt64(IdActuacionDetalleId), iLimiteActuacion);
                    HttpContext.Current.Session.Remove("ACTO_NOTARIAL");
                }
                else
                {
                    //long IdActuacionDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID + strGUID]);
                    if (IdActuacionDetalleId == "")
                    {
                        IdActuacionDetalleId = HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID].ToString();
                    }
                    DtDatosSticker = FunSticker.ActuacionAutuadhesivo(Convert.ToInt64(IdActuacionDetalleId));

                    if (DtDatosSticker.Rows.Count == 0)
                    {
                        return "";
                    }
                }

                //--------------------------------------------------------------------
                //Fecha: 03/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar el ID de la actuación a la session respectiva.
                //--------------------------------------------------------------------
                HttpContext.Current.Session["ID_ACTUACION_VERIFICAR"] = DtDatosSticker.Rows[0]["actu_iActuacionId"].ToString();
                //--------------------------------------------------------------------

                // Buscando indice del detalle específico
                Int64 intCorrAct = Convert.ToInt64(HttpContext.Current.Session["CORR_TARIFA"]);
                int intIndice = 0;
                if (intCorrAct != 0)
                {
                    for (int i = 0; i < DtDatosSticker.Rows.Count; i++)
                    {
                        // Busca la actuación detalle seleccionada
                        if (DtDatosSticker.Rows[i]["vRGE"].Equals("00000" + intCorrAct.ToString()))
                        {
                            intIndice = i;
                            break;
                        }
                    }
                }


                DataRow drSeleccionado = DtDatosSticker.Rows[intIndice];
                string strCadenaCodifica = string.Empty;
                bool bTextoGrande = false;
                string sConsulado = string.Empty;
                if (drSeleccionado["vNomOficConsul2"].ToString().Length > 0)
                {
                    sConsulado = drSeleccionado["vNomOficConsul"].ToString() + ' ' + drSeleccionado["vNomOficConsul2"].ToString();
                    bTextoGrande = true;
                }
                else {
                    sConsulado = drSeleccionado["vNomOficConsul"].ToString();
                }

                strCadenaCodifica = sConsulado + "|" +
                                Comun.FormatearFecha(drSeleccionado["dFecha"].ToString()).ToString(FormatoFechas) + "|" +
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
                    DateTime.Now.ToString(FormatoFechas) + "|";

                Bitmap objLienzo;
                objLienzo = GenerarImagenPDF417(strCadenaCodifica);

                ImageConverter converter = new ImageConverter();
                byte[] BarCode = (byte[])converter.ConvertTo(objLienzo,typeof(byte[]));
 

                //objLienzo.Save(uploadPath + @"\PDF420.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

                String TarifaAutoAdhesivo = string.Empty;
                if (drSeleccionado["numTarifa"].ToString().Equals("0"))
                {
                    TarifaAutoAdhesivo = drSeleccionado["letraTarifa"].ToString();                    
                }
                else
                {
                    TarifaAutoAdhesivo = drSeleccionado["numTarifa"].ToString().PadLeft(2, '0') +
                                            drSeleccionado["letraTarifa"].ToString();
                }

                Double t_1 = Math.Round((Convert.ToDouble(drSeleccionado["fMontoSolesConsulares"]) / (DtDatosSticker.Rows.Count)), 2);
                Double t_2 = Math.Round((Convert.ToDouble(drSeleccionado["fMontoMonedaLocal"]) / (DtDatosSticker.Rows.Count)), 2);

                String t1 = String.Format("{0:#,###0.00}", t_1);
                String t2 = String.Format("{0:#,###0.00}", t_2);

                string strFontSize = "9";
                string strMarginTop = "75";
                string strMarginLeft = "60";
                string strBarHeight = "40";
                string strBarWidth = "185";
                string strEspaciado = "8";
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_FontSize"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HTML_FontSize"].ToString() != string.Empty)
                        strFontSize = System.Configuration.ConfigurationManager.AppSettings["HTML_FontSize"].ToString();
                }
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginLeft"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginLeft"].ToString() != string.Empty)
                        strMarginLeft = System.Configuration.ConfigurationManager.AppSettings["HTML_MarginLeft"].ToString();
                }
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginTop"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HTML_MarginTop"].ToString() != string.Empty)
                        strMarginTop = System.Configuration.ConfigurationManager.AppSettings["HTML_MarginTop"].ToString();
                }
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_Espaciado"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HTML_Espaciado"].ToString() != string.Empty)
                        strEspaciado = System.Configuration.ConfigurationManager.AppSettings["HTML_Espaciado"].ToString();
                }
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_BarCodeHeight"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HTML_BarCodeHeight"].ToString() != string.Empty)
                        strBarHeight = System.Configuration.ConfigurationManager.AppSettings["HTML_BarCodeHeight"].ToString();
                }
                if (System.Configuration.ConfigurationManager.AppSettings["HTML_BarCodeWidth"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HTML_BarCodeWidth"].ToString() != string.Empty)
                        strBarWidth = System.Configuration.ConfigurationManager.AppSettings["HTML_BarCodeWidth"].ToString();
                }

                string vLinea1Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea1Titulo"].ToString();
                bool bLinea1TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea1TituloNegrita"].ToString());
                if (bLinea1TituloNegrita)
                    vLinea1Titulo = "<b>" + vLinea1Titulo + "</b>";

                //-------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Fecha: 14/10/2016
                // Objetivo: Incluir el apellido de casada
                //-------------------------------------------------
                int intPersonaRecurrente = Convert.ToInt32(drSeleccionado["actu_iPersonaRecurrenteId"].ToString());
                string strApellidoPaterno = "";
                string strApellidoMaterno = "";
                string strNombres = "";
                string strApellidoCasada = "";
                string strApellidosCompletos = "";
                int intLongApellidos = 0;
                int intLongNombres = 0;
                string strDatosSolicitante_1 = "";
                string strDatosSolicitante_2 = "";

                if (intPersonaRecurrente > 0)
                {
                    strApellidoPaterno = drSeleccionado["vApellidoPaterno"].ToString().Trim();
                    strApellidoMaterno = drSeleccionado["vApellidoMaterno"].ToString().Trim();
                    strNombres = drSeleccionado["vNombres"].ToString().Trim();
                    strApellidoCasada = drSeleccionado["vApellidoCasada"].ToString().Trim();
                    if (strApellidoCasada.Length > 0)
                    {
                        strApellidosCompletos = strApellidoPaterno + " " + strApellidoMaterno + " " + strApellidoCasada;
                    }
                    else
                    {
                        strApellidosCompletos = strApellidoPaterno + " " + strApellidoMaterno;
                    }
                    intLongApellidos = strApellidosCompletos.Length;
                    intLongNombres = strNombres.Length;
                    if ((intLongApellidos + intLongNombres) <= 44)
                    {
                        strDatosSolicitante_1 = drSeleccionado["vSolicitante"].ToString();
                    }
                    else
                    {
                        if (strApellidosCompletos.Length > 44)
                        {
                            strDatosSolicitante_1 = strApellidosCompletos.Substring(0, 44);
                        }
                        else
                        {
                            strDatosSolicitante_1 = strApellidosCompletos.Trim();
                        }
                        if (strNombres.Length > 44)
                        {
                            strDatosSolicitante_2 = strNombres.Substring(0, 44);
                        }
                        else
                        {
                            strDatosSolicitante_2 = strNombres;
                        }
                    }
                }
                else
                {
                    strDatosSolicitante_1 = drSeleccionado["vSolicitante"].ToString();
                }
                                                                 
                //-------------------------------------------------
                string vLinea2Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea2Titulo"].ToString();
                bool bLinea2TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea2TituloNegrita"].ToString());
                if (bLinea2TituloNegrita)
                    vLinea2Titulo = "<b>" + vLinea2Titulo + "</b>";
                bool bLinea2TextoNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea2TextoNegrita"].ToString());
                if (bLinea2TextoNegrita)
                {
                    strDatosSolicitante_1 = "<b>" + strDatosSolicitante_1 + "</b>";
                    if (strDatosSolicitante_2.Length > 0)
                    { strDatosSolicitante_2 = "<b>" + strDatosSolicitante_2 + "</b>"; }
                }
                string vLinea3Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea3Titulo"].ToString();
                bool bLinea3TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea3TituloNegrita"].ToString());
                if (bLinea3TituloNegrita)
                    vLinea3Titulo = "<b>" + vLinea3Titulo + "</b>";

                string vLinea4Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea4Titulo"].ToString();
                bool bLinea4TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea4TituloNegrita"].ToString());
                if (bLinea4TituloNegrita)
                    vLinea4Titulo = "<b>" + vLinea4Titulo + "</b>";

                string vLinea5Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea5Titulo"].ToString();
                bool bLinea5TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea5TituloNegrita"].ToString());
                if (bLinea5TituloNegrita)
                    vLinea5Titulo = "<b>" + vLinea5Titulo + "</b>";

                string vLinea6Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea6Titulo"].ToString();
                bool bLinea6TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea6TituloNegrita"].ToString());
                if (bLinea6TituloNegrita)
                    vLinea6Titulo = "<b>" + vLinea6Titulo + "</b>";

                string vLinea7Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea7Titulo"].ToString();
                bool bLinea7TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea7TituloNegrita"].ToString());

                string vLinea8Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea8Titulo"].ToString();
                bool bLinea8TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea8TituloNegrita"].ToString());

                string vLinea9Titulo = System.Configuration.ConfigurationManager.AppSettings["Linea9Titulo"].ToString();
                bool bLinea9TituloNegrita = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Linea9TituloNegrita"].ToString());



                    
                if (bLinea7TituloNegrita)
                    vLinea7Titulo = "<b>" + vLinea7Titulo + "</b>";

      
                if (bLinea8TituloNegrita && vLinea8Titulo.Length > 0)
                {
                    vLinea8Titulo = "<b>" + vLinea8Titulo + "</b>";
                }
                else if (vLinea8Titulo.Length == 0)
                {

                    vLinea8Titulo = "<b style=\"color:transparent; visibility: hidden; \">" + vLinea7Titulo + "</b>";

                }

                if (bLinea9TituloNegrita)
                    vLinea9Titulo = "<b>" + vLinea9Titulo + "</b>";



                html += " <div class='container' style='font-size:" + strFontSize + "px;font-family:Arial Narrow;margin-top: " + strMarginTop + "px;margin-left: " + strMarginLeft + "px;'>";
               

                html += "<div class='row'>";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-12 text-left'>";
                if (bTextoGrande)
                {
                    html += "                          <b>" + drSeleccionado["vNomOficConsul"].ToString() + "</b><br/>";
                    html += "                          <b>" + drSeleccionado["vNomOficConsul2"].ToString() + "</b>";
                }
                else {
                    html += "                          <b>" + drSeleccionado["vNomOficConsul"].ToString() + "</b>";
                }
                
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                //------------------------------------------------
                //Fecha: 17/10/2016
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Adicionar un margin-top de 5
                //------------------------------------------------

                if (strDatosSolicitante_2.Length == 0)
                {
                    html += "         <div class='Formato_Salto' style='margin-top: 5px'></div>";
                }
                else
                {
                    html += "         <div class='Formato_Salto' style='margin-top: 1px'></div>";
                }
                
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-12 text-left'>";
                html += "                            " + vLinea1Titulo + Comun.FormatearFecha(drSeleccionado["dFecha"].ToString()).ToString(FormatoFechas).ToUpper();
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                       " + vLinea2Titulo + strDatosSolicitante_1;
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                //-------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Fecha: 14/10/2016
                // Objetivo: Incluir nombres en la segunda fila
                //-------------------------------------------------
                if (strDatosSolicitante_2.Length > 0)
                {
                    html += "         <div class='col-sm-12'>";
                    html += "                 <div class='row'>";
                    html += "                     <div class='col-sm-4 text-left'>";
                    html += "                       " + vLinea2Titulo + strDatosSolicitante_2;
                    html += "                     </div>";
                    html += "                 </div>";
                    html += "         </div>";
                }
                //-------------------------------------------------
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-12 text-left'>";
                html += "                            " + vLinea3Titulo + drSeleccionado["vNombreActuacion"].ToString();
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";                
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea4Titulo + drSeleccionado["vRGE"].ToString(); 
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea5Titulo + drSeleccionado["iCorrTarifa"].ToString();
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "          <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea6Titulo + TarifaAutoAdhesivo;
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "          <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea7Titulo + t1 + " &nbsp; SOLES CONSULARES";
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";

                if (drSeleccionado["VTIPOPAGO"].ToString().Trim() != "PAGADO EN LIMA")
                {
                    html += "          <div class='col-sm-12'>";
                    html += "                 <div class='row'>";
                    html += "                     <div class='col-sm-4 text-left'>";
                    html += "                            " + vLinea8Titulo + t2 + " &nbsp; " + drSeleccionado["vMoneda"].ToString();
                    html += "                     </div>";
                    html += "                 </div>";
                    html += "         </div>";
                }
                else
                {
                    html += "          <div class='col-sm-12'>";
                    html += "                 <div class='row'>";
                    html += "                     <div class='col-sm-4 text-left'>";
                    html += "                            " + vLinea9Titulo + drSeleccionado["VTIPOPAGO"].ToString();
                    html += "                     </div>";
                    html += "                 </div>";
                    html += "         </div>";
                }
                html += " </div>";
                html += " </div>";



                //--------------------------------
                //Autor: Miguel Márquez Beltrán
                //--------------------------------
                //Fecha: 14/10/2016
                //Objetivo: Se asigno a: strEspaciado valor de 12 a 1
                //-----------------------------------------------------
                int intEspaciadoTop = Convert.ToInt16(strEspaciado);
                if (strDatosSolicitante_2.Length == 0)
                {
                    strEspaciado = Convert.ToString(intEspaciadoTop + 5);
                }

                html += " <div class='container' style='margin-top: " + strEspaciado + "px;margin-left: " + strMarginLeft  + "px;'>";
                html += "       <div class='row'>";
                html += "          <div class='col-sm-12 text-left'>";
                //html += "               <img class='img-cod-bar'  style='width:" + strBarWidth + "px;height:" + strBarHeight + "px;'  src=\"" + string.Format("../LoadImagen.ashx?vClass={0}", "PDF420.bmp") + "\" alt='Picture'/>";
//                BarCode
                html +=                     "<img class='img-cod-bar'  style='width:" + strBarWidth + "px;height:" + strBarHeight + "px;'  src='data:image/jpg;base64,"+Convert.ToBase64String(BarCode)+"' alt='Picture'/>";

                html += "          </div>";
                html += "       </div>";
                html += " </div>";



                //Se habilitará cuando se defina el uso de marca de agua
                #region Marca de Agua de Consulta  

                bool bConsulta = false;// 

                if (bConsulta)
                {
                    html += "  <div style=\"font-size:30px; font-family:Arial Narrow; display:block; left:-20px; top: 170px; color:GrayText; position:absolute; -webkit-transform: rotate(-45deg); transform: rotate(-45deg);";
                    html += "   \">";

                    html += "   SOLO PARA CONSULTAS";
                    html += "   </div>";
                }

                #endregion

                int intMarginTop = Convert.ToInt16(strMarginTop);
                if (strDatosSolicitante_2.Length == 0)
                {
                    strMarginTop = Convert.ToString(intMarginTop + 15);
                }
                else
                {
                    strMarginTop = Convert.ToString(intMarginTop + 10);
                }

                html += " <div class='container' style='font-size:" + strFontSize + "px; font-family:Arial Narrow; margin-top: " + strMarginTop + "px;margin-left: " + strMarginLeft + "px;'>";
                html += "<div class='row'>";
                html += "         <br />";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                //html += "                     <div class='col-sm-12 text-center>";
                //html += "                          <b>RECIBO</b>";
                //html += "                     </div>";
                //html += "                 </div>";
                html += "         </div>";             
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-12 text-left'>";
                if (bTextoGrande)
                {
                    html += "                          <b>" + drSeleccionado["vNomOficConsul"] + "</b><br/>";
                    html += "                          <b>" + drSeleccionado["vNomOficConsul2"] + "</b>";
                }
                else
                {
                    html += "                          <b>" + drSeleccionado["vNomOficConsul"] + "</b><br/>";
                }
                
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                //------------------------------------------------
                //Fecha: 17/10/2016
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Adicionar un margin-top de 5
                //------------------------------------------------
                html += "         <div class='Formato_Salto' style='margin-top: 5px'></div>";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-12 text-left'>";
                html += "                            " + vLinea1Titulo + Comun.FormatearFecha(drSeleccionado["dFecha"].ToString()).ToString(FormatoFechas).ToUpper();
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                       " + vLinea2Titulo + strDatosSolicitante_1;
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                //-------------------------------------------------
                // Autor: Miguel Márquez Beltrán
                // Fecha: 14/10/2016
                // Objetivo: Incluir el apellido de casada
                //-------------------------------------------------
                if (strDatosSolicitante_2.Length > 0)
                {
                    html += "         <div class='col-sm-12'>";
                    html += "                 <div class='row'>";
                    html += "                     <div class='col-sm-4 text-left'>";
                    html += "                       " + vLinea2Titulo + strDatosSolicitante_2;
                    html += "                     </div>";
                    html += "                 </div>";
                    html += "         </div>";
                }
                //-------------------------------------------------

                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-12 text-left'>";
                html += "                            " + vLinea3Titulo + drSeleccionado["vNombreActuacion"].ToString();
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";                
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea4Titulo + drSeleccionado["vRGE"].ToString(); 
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "         <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea5Titulo + drSeleccionado["iCorrTarifa"].ToString();
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "          <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left'>";
                html += "                            " + vLinea6Titulo + TarifaAutoAdhesivo;
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";
                html += "          <div class='col-sm-12'>";
                html += "                 <div class='row'>";
                html += "                     <div class='col-sm-4 text-left' >";
                html += "                            " + vLinea7Titulo + t1 + " &nbsp; SOLES CONSULARES";
                html += "                     </div>";
                html += "                 </div>";
                html += "         </div>";

                if (drSeleccionado["VTIPOPAGO"].ToString().Trim() != "PAGADO EN LIMA")
                {
                    html += "          <div class='col-sm-12'>";
                    html += "                 <div class='row'>";
                    html += "                     <div class='col-sm-4 text-left'>";
                    html += "                            " + vLinea8Titulo + t2 + " &nbsp; " + drSeleccionado["vMoneda"].ToString();
                    html += "                     </div>";
                    html += "                 </div>";
                    html += "           </div>";
                }
                else
                {
                    html += "          <div class='col-sm-12'>";
                    html += "                 <div class='row'>";
                    html += "                     <div class='col-sm-4 text-left'>";
                    html += "                            " + vLinea9Titulo + drSeleccionado["VTIPOPAGO"].ToString();
                    html += "                     </div>";
                    html += "                 </div>";
                    html += "         </div>";
                }


                html += "  </div>";
                html += "  </div>"; 
                
                //html += "</table>";



                   
                return html;
            }
            catch (Exception ex)
            {
                throw ex;    
            }
        }
    }
}