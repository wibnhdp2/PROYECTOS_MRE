using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using System.Data;
using SGAC.Registro.Actuacion.BL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using SGAC.WebApp.Accesorios;
using System.Configuration;
using Microsoft.Security.Application;

namespace SGAC.WebApp.Registro
{
    public partial class FrmReporteMigratorioHtml : MyBasePage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["FormatoDGC"] = Request.QueryString["FormatoDGC"];
            if (Request.QueryString["GUID"] != null)
            {
                HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
            }
            else
            {
                HFGUID.Value = "";
            }
        }
        [System.Web.Services.WebMethod]
        public static string Actualizar_Estado_Entregado()
        {
            string script = string.Empty;

            SGAC.BE.MRE.RE_ACTOMIGRATORIO oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();

            #region - Actualizando los datos de la lámina  -
            oRE_ACTOMIGRATORIO = new BE.MRE.RE_ACTOMIGRATORIO();
            oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId = Convert.ToInt64(HttpContext.Current.Session["Acto_Migratorio_ID"]); ;
            oRE_ACTOMIGRATORIO.acmi_vNumeroLamina = "NO ACTUALIZAR";
            oRE_ACTOMIGRATORIO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP(); ;
            oRE_ACTOMIGRATORIO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

            script = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Lamina(oRE_ACTOMIGRATORIO).ToString();

            #endregion

            return script;
        }

        [System.Web.Services.WebMethod]
        public static String ImpresionCorrecta(string strGUID)
        {
            string Formato = Convert.ToString(HttpContext.Current.Session["Formato"]);
            if (Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACION_ID]) == 0)
            {
                return "HUBO UN PROBLA PARA LA OBTENER DATOS DEL SISTEMA, POR FAVOR REFRESQUE LA PAGINA.";
            }
            else
            {
                Int64 iActuacionInsumoDetalleId = 0; // Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_LAMINA].ToString());
                iActuacionInsumoDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID + strGUID].ToString());

                //switch (Comun.ToNullInt32(Formato))
                //{
                //    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                        
                //        break;
                //    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                //    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                //        iActuacionInsumoDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_ACTUACION_INSUMO_DETALLE_ID_PAS].ToString());
                //        break;
                //}

                ActuacionMantenimientoBL objAct = new ActuacionMantenimientoBL();
                String Msj = String.Empty;

                Int16 sOficinaConsularId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                Int16 sUsuarioId = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);


                objAct.USP_RE_ACTUACIONINSUMODETALLE_ACTUALIZAR_IMPRESION(iActuacionInsumoDetalleId, true, sUsuarioId, sOficinaConsularId, ref Msj);

                if (Msj == String.Empty)
                {
                    Msj = "OK";
                }

                return Msj;
            }
        }

        [System.Web.Services.WebMethod]
        public static string Imprimir_Lamina(string strGUID)
        {
           
            string script = string.Empty;
            String FormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();
            Enumerador.enmMigratorioFormato enmAccion = (Enumerador.enmMigratorioFormato)HttpContext.Current.Session["iTipo_Reporte"];

            DataTable dt = new DataTable();
            ActoMigratorioConsultaBL objBL = new ActoMigratorioConsultaBL();

            Int64 iAcutacionDetalleId = Convert.ToInt64(HttpContext.Current.Session[Constantes.CONST_SESION_ACTUACIONDET_ID + strGUID]);
            Int64 iActoMigratorioId = Convert.ToInt64(HttpContext.Current.Session["Acto_Migratorio_ID"]);

            dt = objBL.FormatoMigratorio_Lamina(iAcutacionDetalleId, iActoMigratorioId);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            string imagenFotoURL = string.Empty;
            string imagenFirmaURL = string.Empty;
            if (dt.Rows.Count > 0)
            {
                imagenFotoURL = Convert.ToString(dt.Rows[0]["imagenFotoNombre"].ToString());
                imagenFirmaURL = Convert.ToString(dt.Rows[0]["imagenFirmaNombre"].ToString());
            }

            string strCadenaCodifica = dt.Rows[0]["vLinea_3"].ToString();
            string strCadenaCifrada = string.Empty;
            string strLLaveCifrado = "010419762005";
            string strVectorInicial = "1234567891234567";
            strCadenaCifrada = Util.cifrarTextoAES(strCadenaCodifica, strLLaveCifrado, strLLaveCifrado, "MD5", 22, strVectorInicial, 128);

            System.Drawing.Image objLienzo;

            objLienzo = GenerarImagenPDF417(strCadenaCodifica);

            if (File.Exists(uploadPath + @"\PDF417.bmp"))
                File.Delete(uploadPath + @"\PDF417.bmp");

            objLienzo.Save(uploadPath + @"\PDF417.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            /*Se crean los Html para mostrar en el reporte*/
            switch (enmAccion)
            {
                case Enumerador.enmMigratorioFormato.DGC_001_PASAPORTE_EXPEDIDO_LAMINA:
                    script = Plantilla_Pasaporte(string.Format("../LoadImagen.ashx?vClass={0}", imagenFotoURL), Convert.ToString(dt.Rows[0]["vNumeroDocumento"]),
                        Convert.ToString(dt.Rows[0]["vNombres"]), Convert.ToString(dt.Rows[0]["vApellidoPaterno"]) + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]),
                        Convert.ToString(dt.Rows[0]["vGenero"]), Convert.ToString(dt.Rows[0]["vNacionalidad"]),
                        Convert.ToString(dt.Rows[0]["vPerDocumentoNumero"]), Convert.ToString(dt.Rows[0]["vDepartamento"]),
                        Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Year.ToString(),
                        Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Year.ToString(), Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Year.ToString(),
                        string.Format("../LoadImagen.ashx?vClass={0}", imagenFirmaURL), string.Format("../LoadImagen.ashx?vClass={0}", "PDF417.bmp"), Convert.ToString(dt.Rows[0]["vLinea_1"]), Convert.ToString(dt.Rows[0]["vLinea_2"]),
                        string.Format("../LoadImagen.ashx?vClass={0}&img=2", "lpasaporteemi.gif"));
                    break;
                case Enumerador.enmMigratorioFormato.DGC_002_PASAPORTE_REVALIDADO_LAMINA:
                    script = Plantilla_Pasaporte(string.Format("../LoadImagen.ashx?vClass={0}", imagenFotoURL), Convert.ToString(dt.Rows[0]["vNumeroDocumento"]),
                        Convert.ToString(dt.Rows[0]["vNombres"]), Convert.ToString(dt.Rows[0]["vApellidoPaterno"]) + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]),
                        Convert.ToString(dt.Rows[0]["vGenero"]), Convert.ToString(dt.Rows[0]["vNacionalidad"]),
                        Convert.ToString(dt.Rows[0]["vPerDocumentoNumero"]), Convert.ToString(dt.Rows[0]["vDepartamento"]),
                        Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Year.ToString(),
                        Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Year.ToString(), Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Year.ToString(),
                        string.Format("../LoadImagen.ashx?vClass={0}", imagenFirmaURL), string.Format("../LoadImagen.ashx?vClass={0}", "PDF417.bmp"), Convert.ToString(dt.Rows[0]["vLinea_1"]), Convert.ToString(dt.Rows[0]["vLinea_2"]),
                        string.Format("../LoadImagen.ashx?vClass={0}&img=2", "lpasaporterev.gif"));
                    break;
                case Enumerador.enmMigratorioFormato.DGC_003_PASAPORTE_ANULADO_LAMINA:

                    break;
                case Enumerador.enmMigratorioFormato.DGC_004_PASAPORTE_BAJA_LAMINA:

                    break;

                case Enumerador.enmMigratorioFormato.DGC_005_VISA_LAMINA:
                    #region - Formato Lámina -
                    script = "          <table style=\"WIDTH: 460px; text-align: right\">";
                    script = script + "     <tr>";
                    script = script + "         <td>";
                    script = script + "             <input id=\"btn_Imprimir\" type=\"button\" value=\"Imprimir\" onclick=\"return btn_Imprimir_onclick()\" />";
                    script = script + "         </td>";
                    script = script + "     </tr>";
                    script = script + "</table>";
                    script = script + "<TABLE style=\"BORDER-COLLAPSE: collapse\" id=AutoNumber29 border=0 cellSpacing=0 borderColor=#111111 cellPadding=0 width=\"100%\" height=120>";
                    script = script + "     <TBODY>";
                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";

                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD class=texto1_borde_Celd11 width=\"65%\">";
                    script = script + "                 <FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vOficinaAbr"]) + "</FONT>";
                    script = script + "                 , &nbsp;Fec.Actuación: <FONT size=2 face=\"OCRB A\">" + Comun.FormatearFecha(dt.Rows[0]["dFechaActuacion"].ToString()).ToShortDateString() + "</FONT>";
                    script = script + "                  &nbsp;Nro.Expediente: <FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vNumeroExpediente"]) + "</FONT>";
                    script = script + "                 <BR /><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vApellidoPaterno"]) + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]) + " " + Convert.ToString(dt.Rows[0]["vNombres"]) + "</FONT>";
                    script = script + "                 <BR />&nbsp;Tipo/Type: <FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vSubTipo"]) + "</FONT> &nbsp;&nbsp;&nbsp;No.Entradas/No.Entries: <FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vEntrada"]) + "&nbsp;</FONT> &nbsp; T.Permanencia: <FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["sDiasPermanencia"]) + "</FONT>";
                    script = script + "                 <BR />&nbsp; Funcionario Responsable: <FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vNombreFuncionario"]) + "</FONT>";
                    script = script + "             </TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";

                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         <TR>";


                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";
                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";
                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";
                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";
                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;<BR /></TD>";
                    script = script + "             <TD width=\"65%\">&nbsp;</TD>";
                    script = script + "             <TD class=style3>&nbsp;</TD>";
                    script = script + "         </TR>";
                    script = script + "     </TBODY>";
                    script = script + "</TABLE>";
                    script = script + "<TABLE style=\"BORDER-COLLAPSE: collapse\" id=Table2 border=0 cellSpacing=0 borderColor=#111111 cellPadding=0 width=\"100%\" height=120>";
                    script = script + "     <TBODY>";
                    script = script + "         <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"10%\">";
                    script = script + "                 <TABLE>";
                    script = script + "                     <TBODY>";
                    script = script + "                         <TR>";
                    script = script + "                             <TD width=\"25%\" align=middle>&nbsp;<IMG src=\"" + string.Format("../LoadImagen.ashx?vClass={0}", imagenFotoURL) + "\" width=100 height=120>&nbsp;</TD>";
                    script = script + "                         </TR>";
                    script = script + "                     </TBODY>";
                    script = script + "                 </TABLE>";
                    script = script + "             </TD>";
                    script = script + "             <TD width=\"50%\">";
                    script = script + "                 <TABLE>";
                    script = script + "                     <TBODY>";
                    script = script + "                         <TR>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Lugar de expedición/Place of Issue<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vOficinaAbr"]) + "</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Válido a partir del/Valid from<BR><FONT size=2 face=\"OCRB A\">" + Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Day.ToString() + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Month) + Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Year.ToString() + "</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Válido hasta el/Valid from<BR><FONT size=2 face=\"OCRB A\">" + Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Day.ToString() + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Month) + Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Year.ToString() + "</FONT> </TD>";
                    script = script + "                         </TR>";
                    script = script + "                     </TBODY>";
                    script = script + "                 </TABLE>";
                    script = script + "                 <TABLE>";
                    script = script + "                     <TBODY>";
                    script = script + "                         <TR>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Tipo/Type<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vSubTipo"]) + "</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>No.Entradas/No.Entries<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vEntrada"]) + "&nbsp;</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Inf.Adicional/Additional Inf.<BR /><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vNumeroExpediente"]) + "</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>T.Permanencia/Permanence<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["sDiasPermanencia"]) + "</FONT> </TD>";
                    script = script + "                         </TR>";
                    script = script + "                     </TBODY>";
                    script = script + "                 </TABLE>";
                    script = script + "                 <TABLE>";
                    script = script + "                     <TBODY>";
                    script = script + "                         <TR>";

                    int intCantCaracteres = Convert.ToInt32(ConfigurationManager.AppSettings["CantCaracteresVisa"]);
                    string strDatos = string.Empty;
                    strDatos = Convert.ToString(dt.Rows[0]["vApellidoPaterno"]) + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]);

                    if (strDatos.Length <= intCantCaracteres)
                    {
                        script = script + "                             <TD class=texto1_borde_Celd11>Apellidos/Surname<BR><FONT size=2 face=\"OCRB A\">" + strDatos + "</FONT> </TD>";
                    }
                    else
                    {
                        script = script + "                             <TD class=texto1_borde_Celd11>Apellidos/Surname<BR><FONT size=1 face=\"OCRB A\">" + strDatos + "</FONT> </TD>";
                    }
                    script = script + "                         </TR>";
                    script = script + "                         <TR>";
                    if (Convert.ToString(dt.Rows[0]["vNombres"]).Length <= intCantCaracteres)
                    {
                        script = script + "                             <TD class=texto1_borde_Celd11>Nombres/Given names<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vNombres"]) + "</FONT> </TD>";
                    }
                    else
                    {
                        script = script + "                             <TD class=texto1_borde_Celd11>Nombres/Given names<BR><FONT size=1 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vNombres"]) + "</FONT> </TD>";
                    }
                    script = script + "                         </TR>";
                    script = script + "                     </TBODY>";
                    script = script + "                 </TABLE>";
                    script = script + "                 <TABLE>";
                    script = script + "                     <TBODY>";
                    script = script + "                         <TR>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Núm.Pasaporte/Passport number &nbsp;<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vNumeroPasaporte"]) + "</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Fec.Nacimiento/Date of birth &nbsp;<BR><FONT size=2 face=\"OCRB A\">" + Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Day.ToString() + MonthName(Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Month) + Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Year.ToString() + "</FONT><BR></TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Sexo/Sex<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vGenero"]) + "</FONT> </TD>";
                    script = script + "                             <TD class=texto1_borde_Celd11>Nac./Nationality<BR><FONT size=2 face=\"OCRB A\">" + Convert.ToString(dt.Rows[0]["vLinea_4"]) + "</FONT><BR></TD>";
                    script = script + "                         </TR>";
                    script = script + "                     </TBODY>";
                    script = script + "                 </TABLE>";
                    script = script + "                 <TABLE width=\"85%\">";
                    script = script + "                     <TBODY>";
                    script = script + "                         <TR>";
                    script = script + "                             <TD class=texto1_borde_Celd13 width=\"40%\">Nro.Etiqueta:" + Convert.ToString(dt.Rows[0]["vNumeroDocumento"]) + "<BR>" + Convert.ToString(dt.Rows[0]["vDatos"]) + "<BR><BR><BR></TD>";
                    script = script + "                             <TD class=texto1_borde_Celd13 width=\"38%\" align=middle><BR><BR><BR>....................................<BR>" + Convert.ToString(dt.Rows[0]["vNombreFuncionario"]) + "<BR>" + Convert.ToString(dt.Rows[0]["vCargoFuncionario"]) + "<BR><BR><BR></TD>";
                    script = script + "                         </TR>";
                    script = script + "                     </TBODY>";
                    script = script + "                 </TABLE>";
                    script = script + "             </TD>";
                    script = script + "             <TD width=\"10%\">&nbsp;</TD>";
                    script = script + "          </TR>";
                    script = script + "          <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"50%\" colSpan=2>";
                    script = script + "                 <FONT size=2 face=OCRB>" + nuevo_formato_linea1(Convert.ToString(dt.Rows[0]["vLinea_1"]).Replace("<", "&lt;"),Convert.ToString(dt.Rows[0]["vLinea_2"]).Replace("<", "&lt;")) + "</FONT><BR><FONT size=0 face=OCRB>&nbsp;<BR>";
                    script = script + "                 <FONT size=2 face=OCRB>" + Convert.ToString(dt.Rows[0]["vLinea_2"]).Replace("<", "&lt;") + "</FONT></FONT>";
                    script = script + "             </TD>";
                    script = script + "             <TD width=\"10%\">&nbsp;</TD>";
                    script = script + "          </TR>";
                    script = script + "          <TR>";
                    script = script + "             <TD class=style1>&nbsp;</TD>";
                    script = script + "             <TD width=\"50%\" colSpan=2>&nbsp;</TD>";
                    script = script + "             <TD width=\"10%\">&nbsp;</TD>";
                    script = script + "          </TR>";
                    script = script + "     </TBODY>";
                    script = script + "</TABLE>";
                    #endregion
                    break;
                case Enumerador.enmMigratorioFormato.DGC_006_SALVOCONDUCTO_LAMINA:
                    script = Plantilla_Pasaporte(string.Format("../LoadImagen.ashx?vClass={0}", imagenFotoURL), Convert.ToString(dt.Rows[0]["vNumeroDocumentoAnterior"]),
                        Convert.ToString(dt.Rows[0]["vNombres"]), Convert.ToString(dt.Rows[0]["vApellidoPaterno"]) + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]),
                        Convert.ToString(dt.Rows[0]["vGenero"]), Convert.ToString(dt.Rows[0]["vNacionalidad"]),
                        Convert.ToString(dt.Rows[0]["vPerDocumentoNumero"]), Convert.ToString(dt.Rows[0]["vDepartamento"]),
                        Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dNacimientoFecha"].ToString()).Year.ToString(),
                        Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dFechaExpedicion"].ToString()).Year.ToString(), Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Day.ToString() + " " + MonthName(Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Month) + " " + Comun.FormatearFecha(dt.Rows[0]["dFechaExpiracion"].ToString()).Year.ToString(),
                        string.Format("../LoadImagen.ashx?vClass={0}", imagenFirmaURL), string.Format("../LoadImagen.ashx?vClass={0}", "PDF417.bmp"), Convert.ToString(dt.Rows[0]["vLinea_1"]), Convert.ToString(dt.Rows[0]["vLinea_2"]),
                        string.Format("../LoadImagen.ashx?vClass={0}&img=2", "salvoconducto.png"));
                    break;
                default:
                    string s_Tipo = string.Empty;
                    string Formato = Convert.ToString(HttpContext.Current.Session["FormatoDGC"]);

                    switch (Convert.ToInt32(dt.Rows[0]["sTipoDocumentoMigratorioId"]))
                    {
                        case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                            s_Tipo = "PASAPORTE";
                            break;
                        case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                            s_Tipo = "SALVOCONDUCTO";
                            break;
                        case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                            s_Tipo = "VISAS";
                            break;
                    }
                    DateTime dt_Fecha = Comun.FormatearFecha(Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session));

                    string str_Fecha = dt_Fecha.ToString("dd") + " de " + dt_Fecha.ToString("MMMM") + " de " + dt_Fecha.ToString("yyyy");

                    script = "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\"><input id=\"btn_Imprimir\" type=\"button\" value=\"Imprimir\" onclick=\"return btn_Imprimir_onclick()\" /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"center\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\" font-size:12pt; font-weight:bold; text-decoration:underline;\">DECLARACIÓN DE CONFORMIDAD DEL USUARIO</span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"justify\" style=\"line-height: 150%; \"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">Yo, " + Convert.ToString(dt.Rows[0]["vNombres"]).ToUpper() + " " + Convert.ToString(dt.Rows[0]["vApellidoPaterno"]).ToUpper() + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]).ToUpper() +
                        ", identificado con el " + Convert.ToString(dt.Rows[0]["vPerDocumentoTipo"]) + " N° " + Convert.ToString(dt.Rows[0]["vPerDocumentoNumero"]) + ", declaro que he leído y revisado en su detalle el formato " + Formato + ", que he tenido a la vista y me ha sido entregado en la fecha, manifestando mi conformidad con su contenido </span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"center\" style=\"margin-bottom:5px;\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"80%\"><tr><td width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\">" + Convert.ToString(HttpContext.Current.Session["CiudadOficinaConsular"]).ToUpper() + ", " + str_Fecha + "</span></font></td></tr></table></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><table border=\"0\" width=\"143px\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"border-color: groove; border-width: groove; border-style: groove; height:145px;\"></td></tr><tr><td align=\"center\" width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\" font-size:10pt\">Huella Digital</span></font></td></tr></table></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><font face=\"Courier New\" color=\"#010101\" size=\"2\"><span style=\"font-size:10pt\"><br /></span></font></div>";
                    script = script + "<div align=\"right\"><table border=\"0\" width=\"35%\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\" style=\"border-top-style: dashed; border-width: 2px\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + Convert.ToString(dt.Rows[0]["vNombres"]).ToUpper() + " " + Convert.ToString(dt.Rows[0]["vApellidoPaterno"]).ToUpper() + " " + Convert.ToString(dt.Rows[0]["vApellidoMaterno"]).ToUpper() + "</span></font></td></tr><tr><td align=\"center\" width=\"50%\"><font face=\"Courier New\" color=\"#010101\" size=\"1\"><span style=\"font-size:10pt\">" + Convert.ToString(dt.Rows[0]["vPerDocumentoTipo"]) + " N° " + Convert.ToString(dt.Rows[0]["vPerDocumentoNumero"]) + "</span></font></td></tr></table></div>";
                    break;
            }
            return script;
        }

        public static string nuevo_formato_linea1(string linea1, string linea2)
        {
            //linea1 = "P<RAUL<<<<PAUCAR<<<POM<<<<<<<<<<<<<<<<<<<<<<";
            //linea2 = "43535<<<<8PER9401312M2104256<<<<<<<<<<<<<<02";
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
            {
                SizeF size1 = graphics.MeasureString(linea1, new Font("OCRB", 11, FontStyle.Regular, GraphicsUnit.Point));
                SizeF size2 = graphics.MeasureString(linea2, new Font("OCRB", 11, FontStyle.Regular, GraphicsUnit.Point));

                int lb1 = (int)size1.Width;
                int lb2 = (int)size2.Width;
                while (lb2 < lb1)
                {
                    linea1 = linea1.Substring(0, linea1.Length - 1);
                    size1 = graphics.MeasureString(linea1, new Font("OCRB", 11, FontStyle.Regular, GraphicsUnit.Point));
                    lb1 = (int)size1.Width;
                }
            }
            return linea1;
        }



        #region Métodos
        public static string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month).ToUpper().Substring(0, 3);
        }
        /// <summary>
        /// Implementación del formato de las láminas
        /// </summary>
        /// <param name="img_Foto"></param>
        /// <param name="s_numero_documento"></param>
        /// <param name="s_Nombre"></param>
        /// <param name="s_Apellidos"></param>
        /// <param name="s_Genero"></param>
        /// <param name="s_Nacionalidad"></param>
        /// <param name="s_Documento_Nacional"></param>
        /// <param name="s_Lugar_Nacimiento"></param>
        /// <param name="s_Fecha_Nacimiento"></param>
        /// <param name="s_Fecha_Emision"></param>
        /// <param name="s_Fecha_Vencimiento"></param>
        /// <param name="img_Firma"></param>
        /// <param name="img_PDF"></param>
        /// <param name="s_OCR_1"></param>
        /// <param name="s_OCR_2"></param>
        /// <param name="img_Fondo"></param>
        /// <returns></returns>
        public static string Plantilla_Pasaporte(string img_Foto, string s_numero_documento,
            string s_Nombre, string s_Apellidos, string s_Genero, string s_Nacionalidad,
            string s_Documento_Nacional, string s_Lugar_Nacimiento, string s_Fecha_Nacimiento,
            string s_Fecha_Emision, string s_Fecha_Vencimiento, string img_Firma, string img_PDF,
            string s_OCR_1, string s_OCR_2, string img_Fondo)
        {
            /*Pone la foto*/

            string script = "<table style=\"WIDTH: 460px; text-align: right\">";
            script = script + "     <tr>";
            script = script + "         <td>";
            script = script + "             <input id=\"btn_Imprimir\" type=\"button\" value=\"Imprimir\" onclick=\"return btn_Imprimir_onclick()\" />";
            script = script + "         </td>";
            script = script + "     </tr>";
            script = script + "</table>";
            script = script + "<TABLE style=\"WIDTH: 460px\">";
            script = script + "<TBODY>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 33px; WIDTH: 6439px\">&nbsp;</TD>";
            script = script + "         <TD style=\"HEIGHT: 33px; WIDTH: 90px\">&nbsp;</TD>";
            script = script + "         <TD style=\"HEIGHT: 33px; WIDTH: 1913px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 33px; WIDTH: 4224px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 33px; WIDTH: 270px\" align=right></TD>";
            script = script + "         <TD style=\"HEIGHT: 33px; WIDTH: 766px\"></TD>";
            script = script + "     </TR>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 21px; WIDTH: 6439px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 21px; WIDTH: 90px\"></TD>";
            script = script + "         <TD style=\"WIDTH: 1913px\" rowSpan=4 align=left></TD>";
            script = script + "         <TD style=\"HEIGHT: 21px; WIDTH: 4224px\"></TD>";
            script = script + "         <TD style=\"WIDTH: 270px\" rowSpan=3 align=left></TD>";
            script = script + "         <TD style=\"HEIGHT: 21px; WIDTH: 766px\"></TD>";
            script = script + "     </TR>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 24px; WIDTH: 6439px\"></TD>";
            script = script + "     </TR>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 18px; WIDTH: 6439px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 18px; WIDTH: 90px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 18px; WIDTH: 4224px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 18px; WIDTH: 766px\"></TD>";
            script = script + "     </TR>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 80px; WIDTH: 6439px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 80px; WIDTH: 90px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 80px\" colSpan=3>";
            script = script + "             <TABLE>";
            script = script + "                 <TBODY>";
            script = script + "                     <TR>";
            script = script + "                         <TD style=\"HEIGHT: 20px; WIDTH: 166px\"></TD>";
            script = script + "                         <TD style=\"WIDTH: 144px; PADDING-TOP: 5px\"></TD>";
            script = script + "                     </TR>";
            script = script + "                     <TR>";
            script = script + "                         <TD style=\"HEIGHT: 20px; WIDTH: 166px\"></TD>";
            script = script + "                         <TD style=\"WIDTH: 144px\"></TD>";
            script = script + "                     </TR>";
            script = script + "                     <TR>";
            script = script + "                         <TD style=\"HEIGHT: 20px; WIDTH: 166px\"></TD>";
            script = script + "                         <TD style=\"WIDTH: 144px\"></TD>";
            script = script + "                     </TR>";
            script = script + "                 </TBODY>";
            script = script + "             </TABLE>";
            script = script + "         </TD>";
            script = script + "     </TR>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 62px; WIDTH: 6439px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 62px; WIDTH: 90px\">&nbsp;</TD>";
            script = script + "         <TD style=\"HEIGHT: 62px; WIDTH: 1913px\" align=center></TD>";
            script = script + "         <TD style=\"HEIGHT: 62px\" colSpan=3 align=center></TD>";
            script = script + "     </TR>";
            script = script + "     <TR>";
            script = script + "         <TD style=\"HEIGHT: 60px; WIDTH: 6439px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 60px; WIDTH: 90px\"></TD>";
            script = script + "         <TD style=\"HEIGHT: 60px\" colSpan=4></TD>";
            script = script + "     </TR>";
            script = script + " </TBODY></TABLE>";
            script = script + "   <div id=\"divFoto\" style=\"POSITION: absolute; LEFT: 31px; Z-INDEX: -10; TOP: 50px\">";
            script = script + "     <img alt=\"\" src=\"" + img_Foto + "\" width=\"105px\" height=\"135px\">";
            script = script + " </div>";

            /*Pone el numero de documento*/
            script = script + "<div id=\"divDocumentNo\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 100px; POSITION: absolute; LEFT: 145px; Z-INDEX: 100; TOP: 42px\">&nbsp;";
            script = script + s_numero_documento;
            script = script + "</div>";

            /*Imagen Transparante*/
            script = script + "  <div id=\"divFoto2\" style=\"POSITION: absolute; LEFT: 365px; Z-INDEX: -10; TOP: 40px\">";
            script = script + "     <img onclick=\"return IMG1_onclick()\" id=\"IMG2\" style=\"FILTER: alpha(opacity=45); opacity: 0.4\" alt=\"\" src=\"" + img_Foto + "\" width=\"55px\" height=\"70px\">";
            script = script + "  </div>";

            double dblTamanioLetra = 10;
            int top;
            int intCantCaracteres;

            /*Nombres*/
            top = 68;
            intCantCaracteres = Convert.ToInt32(ConfigurationManager.AppSettings["CantCaracteresApeMigratorio"]);
            if (s_Apellidos.Length > intCantCaracteres)
            {
                if (s_Apellidos.Length > intCantCaracteres + 10)
                {
                    dblTamanioLetra = 6.5;
                    top = 72;
                }
                else
                {
                    top = 70;
                    dblTamanioLetra = 7.5;
                }
            }
            script = script + "<div id=\"divLastName\" style=\"FONT-SIZE: " + dblTamanioLetra + "pt; FONT-FAMILY: OCRB A; WIDTH: 320px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 145px; Z-INDEX: 100; TOP: " + top + "px\">&nbsp;";
            script = script + s_Apellidos;
            script = script + "</div>";

            /*Apellidos*/
            top = 92;
            intCantCaracteres = Convert.ToInt32(ConfigurationManager.AppSettings["CantCaracteresNomMigratorio"]);
            dblTamanioLetra = 10;

            if (s_Nombre.Length > intCantCaracteres)
            {
                if (s_Nombre.Length > intCantCaracteres + 10)
                {
                    dblTamanioLetra = 6.5;
                    top = 96;
                }
                else
                {
                    dblTamanioLetra = 7.5;
                    top = 94;
                }
            }
            script = script + "<div id=\"divFirstName\" style=\"FONT-SIZE: " + dblTamanioLetra + "pt; FONT-FAMILY: OCRB A; WIDTH: 300px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 145px; Z-INDEX: 100; TOP: " + top + "px\">&nbsp;";
            script = script + s_Nombre;
            script = script + "</div>";

            /*Genero de la persona*/
            script = script + "<div id=\"div2\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 50px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 413px; Z-INDEX: 100; TOP: 92px\">&nbsp;";
            script = script + s_Genero;
            script = script + "</div>";

            /*Nacionalidad*/
            script = script + "<div id=\"divNacionalidad\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 100px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 145px; Z-INDEX: 100; TOP: 117px\">&nbsp;";
            script = script + s_Nacionalidad;
            script = script + "</div>";

            /*Documento nacional*/
            script = script + "<div id=\"divNumdoc1\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 100px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 313px; Z-INDEX: 100; TOP: 117px\">&nbsp;";
            script = script + s_Documento_Nacional;
            script = script + "</div>";

            /*Lugar de nacimiento*/
            script = script + "<div id=\"divPlaceofBirth\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 150px; POSITION: absolute; LEFT: 145px; Z-INDEX: 100; TOP: 144px\">&nbsp;";
            script = script + s_Lugar_Nacimiento;
            script = script + "</div>";

            /*Fecha de nacimiento*/
            script = script + "<div id=\"divBirthdate\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 150px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 313px; Z-INDEX: 100; TOP: 144px\">&nbsp;";
            script = script + s_Fecha_Nacimiento;
            script = script + "</div>";

            /*Fecha Emisión*/
            script = script + "<div id=\"divIssueDate\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 150px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 145px; Z-INDEX: 100; TOP: 170px\">&nbsp;";
            script = script + s_Fecha_Emision;
            script = script + "</div>";

            /*Fecha Vencimiento*/
            script = script + "<div id=\"divExpirationDate\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB A; WIDTH: 150px; POSITION: absolute; FONT-WEIGHT: normal; LEFT: 313px; Z-INDEX: 100; TOP: 170px\">&nbsp;";
            script = script + s_Fecha_Vencimiento;
            script = script + "</div>";

            /*Pone la firma*/
            script = script + "<div id=\"divFirma\" style=\"POSITION: absolute; LEFT: 50px; Z-INDEX: 100; TOP: 210px\">";
            script = script + " <img  alt=\"\" src=\"" + img_Firma + "\" width=\"90px\" height=\"25px\"></img>";
            script = script + "</div>";

            /*Código Pdf*/
            script = script + "<div id=\"divCodigoPdf\" style=\"POSITION: absolute; LEFT: 172px; Z-INDEX: 101; TOP: 203px\">";
            script = script + " <img alt=\"\" src=\"" + img_PDF + "\"  width=\"280px\" height=\"48px\"></img>";
            script = script + "</div>";

            /*ocr1*/
            script = script + "<div id=\"divOCR1\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB; WIDTH: 425px; POSITION: absolute; LEFT: 27px; Z-INDEX: 100; TOP: 266px\">";
            script = script + nuevo_formato_linea1(s_OCR_1, s_OCR_2).Replace("<", "&lt;");
            script = script + "</div>";

            /*ocr1*/
            script = script + "<div id=\"div1\" style=\"FONT-SIZE: 10pt; FONT-FAMILY: OCRB; WIDTH: 425px; POSITION: absolute; LEFT: 27px; Z-INDEX: 100; TOP: 290px\">";
            script = script + s_OCR_2.Replace("<", "&lt;");//"43111<<<<8PER9401312M2111111<<<<<<<<<<<<<<02".Replace("<", "&lt;");
            script = script + "</div>";

            /*Imagen de fondo*/
            script = script + "<div id=\"capaImagen\" style=\"WIDTH: 476px; POSITION: absolute; LEFT: 0px; Z-INDEX: -5; TOP: 0px\">";
            script = script + " <img onclick=\"return IMG1_onclick()\" id=\"IMG1\" style=\"WIDTH: 471px; FLOAT: left; MARGIN: 0px auto\" border=\"0\" alt=\"\" src=\"" + img_Fondo + "\" height=\"320px\">";
            script = script + "</div>";

            /*Creo una tabla para poner el marfen*/
            script = script + "<TABLE id=\"Table2\" style=\"BORDER-COLLAPSE: collapse\" borderColor=\"#111111\" cellSpacing=1 cellPadding=0 width=\"100%\" border=\"0\">";
            script = script + "<TBODY>";
            script = script + "<TR>";
            script = script + "<TD width=\"50%\" align=\"center\">&nbsp;</TD>";
            script = script + "<TD width=\"50%\" align=\"center\">&nbsp;</TD></TR></TBODY></TABLE>";

            return script;
        }
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

        static void GenerarImagenTransparente(string inputFileName)
        {
            Bitmap bmpIn = (Bitmap)Bitmap.FromFile(inputFileName);
            System.Drawing.Image converted = ChangeImageOpacity((System.Drawing.Image)bmpIn, 135);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            converted.Save(uploadPath + @"\Transparente.png", ImageFormat.Png);
        }

        private const int bytesPerPixel = 4;
        public static System.Drawing.Image ChangeImageOpacity(System.Drawing.Image originalImage, double opacity)
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
    }
}