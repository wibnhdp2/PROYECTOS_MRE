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
//SEGURIDAD PDF
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;
using iTextSharp.text.xml;
using SAE.UInterfaces;
using SolCARDIP.Librerias.ReglasNegocio;

[Serializable]

public class Utilitarios
{

    brGeneral obrGeneral = new brGeneral();
    private static UIEncriptador UIEncripto = new UIEncriptador();

    public Utilitarios()
    {
    }

    public void validateActiveSession(Page pagPage)
    {
        if (pagPage.Session["usuario"] == null)
        {
            String strAppPath = pagPage.Request.ApplicationPath;
            if (strAppPath == "/")
            { strAppPath = ""; }
            //String ruta = strAppPath + "/mensajes.aspx?mensaje=Su sesion ha expirado";
            //pagPage.Response.Write("<script>window.open('" + ruta + "','_parent');</script>");

            String ruta = strAppPath + "/Login.aspx";
            pagPage.Response.Write("<script>window.open('" + ruta + "','_parent');</script>");

        }
    }
    public bool validateActiveSession2(Page pagPage)
    {
        if (pagPage.Session["usuario"] == null)
        {
            string strAppPath = pagPage.Request.ApplicationPath;
            if (strAppPath == "/")
            { strAppPath = ""; }
            pagPage.Session["mensaje"] = "Su sesion ha expirado";
            string ruta = strAppPath + "/mensaje.aspx";
            pagPage.Response.Write("<script>window.open('" + ruta + "','_parent');</script>");
            return true;
        }
        return false;
    }

    public void validatePageAccess(Page pagPage)
    {
        if (!validateAccess(pagPage))
        {
            string strAppPath = pagPage.Request.ApplicationPath;
            if (strAppPath == "/")
            { strAppPath = ""; }
            pagPage.Session["mensaje"] = "Ud. no tiene acceso a esta pagina";
            string ruta = strAppPath + "/mensajes.aspx";
            pagPage.Response.Write("<script>window.open('" + ruta + "','_parent');</script>");
        }
    }
    public void validatePageAccessLogin(Page pagPage)
    {
        string strAppPath = pagPage.Request.ApplicationPath;
        if (strAppPath == "/")
        { strAppPath = ""; }
        pagPage.Session["mensaje"] = "Ud. no tiene acceso a esta pagina";
        string ruta = strAppPath + "/mensajes.aspx";
        pagPage.Response.Write("<script>window.open('" + ruta + "','_parent');</script>");
    }
    public void validatePageAccessLogin(Page pagPage, string strmensaje)
    {
        string strAppPath = pagPage.Request.ApplicationPath;
        if (strAppPath == "/")
        { strAppPath = ""; }
        pagPage.Session["mensaje"] = strmensaje;
        string ruta = strAppPath + "/mensajes.aspx";
        pagPage.Response.Write("<script>window.open('" + ruta + "','_parent');</script>");
    }
    private bool validateAccess(Page pagPage)
    {
        csUsuarioBE objUsuarioBE = new csUsuarioBE();

        objUsuarioBE = (csUsuarioBE)pagPage.Session["usuario"];

        DataTable dt = new DataTable();

        csFormularioBL objFormularioBL = new csFormularioBL();
        try
        {
            string strIdSistemaEnc = ConfigurationManager.AppSettings["IdSistema"].Trim();
            string strIdSistema = UIEncripto.DesEncriptarCadena(strIdSistemaEnc);

            string strRolOpcion = objUsuarioBE.RolOpcion;

            string strRuta = pagPage.AppRelativeVirtualPath.ToString();

            dt = objFormularioBL.ConsultarMenu(strIdSistema, strRolOpcion, strRuta, "-1");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() != "0")
                { return true; }
                else
                { return false; }
            }
            else
            { return false; }
        }
        catch (Exception ex)
        {
            controlError(pagPage, ex);
            throw ex;
        }
    }

    public void cargaPaginas(Menu mnuEntrada, Page papPage)
    {
        csUsuarioBE objUsuarioBE = new csUsuarioBE();

        csFormularioBL objFormularioBL = new csFormularioBL();

        DataTable dt = new DataTable();

        objUsuarioBE = (csUsuarioBE)papPage.Session["usuario"];

        string strIdSistemaEnc = ConfigurationManager.AppSettings["IdSistema"].Trim();
        string strIdSistema = UIEncripto.DesEncriptarCadena(strIdSistemaEnc);

        string strRolOpcion = objUsuarioBE.RolOpcion;

        dt = objFormularioBL.ConsultarMenu(strIdSistema, strRolOpcion, "", "1");

        mnuEntrada.Items.Clear();
        if (dt.Rows.Count > 0)
        { cargaPaginasHijas(dt, "0", null, mnuEntrada); }
    }


    private void cargaPaginasHijas(DataTable dt, String sPagina, MenuItem mnuItem, Menu mnuEntrada)
    {
        DataRow[] drwPaginas;

        drwPaginas = dt.Select("FORM_SREFERENCIAID = " + sPagina);

        for (Int16 i = 0; i < drwPaginas.Length; i++)
        {
            MenuItem ArbolNodo = new MenuItem();

            ArbolNodo.Value = drwPaginas[i]["FORM_SFORMULARIOID"].ToString();
            ArbolNodo.Text = drwPaginas[i]["FORM_VNOMBRE"].ToString().Trim();
            ArbolNodo.NavigateUrl = drwPaginas[i]["FORM_VRUTA"].ToString().Trim();

            if (mnuItem == null)
            { mnuEntrada.Items.Add(ArbolNodo); }
            else
            { mnuItem.ChildItems.Add(ArbolNodo); }
            cargaPaginasHijas(dt, ArbolNodo.Value, ArbolNodo, mnuEntrada);
        }
    }

    public void controlError(Page pagPage, Exception ex)
    {

        string strAppPath = pagPage.Request.ApplicationPath;
        if (strAppPath == "/")
        { strAppPath = ""; }
        pagPage.Session["mensaje"] = "Ocurrio un error inesperado";
        writeLog(pagPage, ex);
        pagPage.Response.Redirect(strAppPath + "/mensajes.aspx");

    }

    public void controlError(Page pagPage, string strmensaje)
    {

        string strAppPath = pagPage.Request.ApplicationPath;
        if (strAppPath == "/")
        { strAppPath = ""; }
        pagPage.Session["mensaje"] = strmensaje;
        writeLog(pagPage, strmensaje);
        pagPage.Response.Redirect(strAppPath + "/mensajes.aspx");

    }

    private void writeLog(Page pagPage, string strMensaje)
    {
        string strLogPath = System.Web.Configuration.WebConfigurationManager.AppSettings["logPath"];
        string strPathEnc = System.Web.Configuration.WebConfigurationManager.AppSettings[strLogPath];

        string strPath = UIEncripto.DesEncriptarCadena(strPathEnc);
        try
        {
            FileStream fs = new FileStream(strPath, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Fecha y Hora: " + DateTime.Now.ToString());
            sw.WriteLine("Error: " + strMensaje);
            sw.WriteLine("Página: " + pagPage.AppRelativeVirtualPath.Substring(2).ToUpper());
            sw.WriteLine("******************************************************");
            sw.WriteLine();
            sw.Close();
        }
        catch (Exception e)
        {
            obrGeneral.grabarLog(e);
            Console.WriteLine("Acceso restringido");
        }
    }
    private void writeLog(Page pagPage, Exception ex)
    {
        string strLogPath = System.Web.Configuration.WebConfigurationManager.AppSettings["logPath"];
        string strPathEnc = System.Web.Configuration.WebConfigurationManager.AppSettings[strLogPath];
        string strPath = UIEncripto.DesEncriptarCadena(strPathEnc);

        try
        {
            FileStream fs = new FileStream(strPath, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Fecha y Hora: " + DateTime.Now.ToString());
            sw.WriteLine("Error: " + ex.ToString());
            sw.WriteLine("Página: " + pagPage.AppRelativeVirtualPath.Substring(2).ToUpper());
            sw.WriteLine("******************************************************");
            sw.WriteLine();
            sw.Close();
        }
        catch (Exception e)
        {
            obrGeneral.grabarLog(e);
            Console.WriteLine("Acceso restringido");
        }
    }

    public void loadCombo(DropDownList pobjDDL, DataTable pdtbSource, String pstrTextColumn, String pstrValueColumn)
    {
        if (pdtbSource.Rows.Count > 0)
        {
            pobjDDL.DataValueField = pstrValueColumn;
            pobjDDL.DataTextField = pstrTextColumn;
            pobjDDL.DataSource = pdtbSource;
            pobjDDL.DataBind();
        }
        else
        { pobjDDL.Items.Clear(); }

    }
    public void loadCheckBoxList(CheckBoxList pobjDDL, DataTable pdtbSource, String pstrTextColumn, String pstrValueColumn)
    {
        pobjDDL.DataValueField = pstrValueColumn;
        pobjDDL.DataTextField = pstrTextColumn;
        pobjDDL.DataSource = pdtbSource;
        pobjDDL.DataBind();
    }
    public void loadComboAndItem(DropDownList pobjDDL, DataTable pdtbSource, String pstrTextColumn, String pstrValueColumn, String pstrItemText)
    {
        loadCombo(pobjDDL, pdtbSource, pstrTextColumn, pstrValueColumn);
        pobjDDL.Items.Insert(0, new System.Web.UI.WebControls.ListItem(pstrItemText, "0"));
        pobjDDL.SelectedIndex = 0;
    }

    public void loadComboAndItem(DropDownList pobjDDL, DataTable pdtbSource, String pstrTextColumn, String pstrValueColumn, String pstrItemText, String pstrItemValue)
    {
        loadCombo(pobjDDL, pdtbSource, pstrTextColumn, pstrValueColumn);
        pobjDDL.Items.Insert(0, new System.Web.UI.WebControls.ListItem(pstrItemText, pstrItemValue));
        pobjDDL.SelectedIndex = 0;
    }

    public void limpiaCombo(String pstrItemText, params object[] comboParams)
    {
        DropDownList combo;
        for (int i = 0; i <= comboParams.GetUpperBound(0); i++)
        {
            combo = new DropDownList();
            combo = (DropDownList)comboParams[i];
            combo.Items.Clear();
            combo.Items.Insert(0, new System.Web.UI.WebControls.ListItem(pstrItemText, "0"));
            combo.SelectedIndex = 0;
        }
    }
    public void Script(Page pPage, String sScript)
    {
        pPage.ClientScript.RegisterStartupScript(this.GetType(), "SCRIPT", sScript);
    }

    public void muestraMensaje(Page pPage, String smensaje)
    {
        smensaje = smensaje.Replace("\r\n", "");
        String popup = "";
        popup = "<script language='javascript'>alert('" + smensaje + "');</script>";
        pPage.ClientScript.RegisterStartupScript(this.GetType(), "Mensaje", popup);
    }

    public void mensajeAJAX(Page pPage, String smensaje)
    {
        smensaje = smensaje.Replace("\r\n", "");

        ScriptManager.RegisterStartupScript(pPage, pPage.GetType(), "SCRIPTAJAX", "alert('" + smensaje + "');", true);
    }

    public void ScriptAJAX(Page pPage, String sScript)
    {
        ScriptManager.RegisterStartupScript(pPage, pPage.GetType(), "SCRIPTAJAX", sScript, true);
    }

    public String FormatDecimal(Double objNumber)
    {
        return objNumber.ToString("#,##0.00");
    }

    public String FormatNumber(Double objNumber)
    {
        return objNumber.ToString("#,##0");
    }

    public String FormatShortDate(DateTime dtmDate)
    {
        return dtmDate.ToShortDateString();
    }

    public String FormatLongDate(DateTime objDate)
    {
        return objDate.ToLongDateString();
    }

    public void onMouseOverGridView(GridViewRowEventArgs e)
    {
        e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
        e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");
        // Le indico que utilice el cursor de selección cuando se pase por encima.
        e.Row.Style.Add("cursor", "hand");
    }

    public String syyyymmdd(String sddmmyyyy)
    {
        if (sddmmyyyy.Length == 10)
        { return sddmmyyyy.Substring(6) + sddmmyyyy.Substring(3, 2) + sddmmyyyy.Substring(0, 2); }
        else
        {
            return "";
        }
    }

    public String sddmmyyyy(String syyyymmdd)
    {
        if (syyyymmdd.Length == 8)
        { return syyyymmdd.Substring(6, 2) + "/" + syyyymmdd.Substring(4, 2) + "/" + syyyymmdd.Substring(0, 4); }
        else
        {
            return "";
        }
    }

    public bool isdate(String sddmmyyyy)
    {
        Regex re = new Regex("(^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$)");
        bool esfecha = re.IsMatch(sddmmyyyy.Trim());
        return esfecha;
    }

    public void buscarIndiceCombo(string skey, DropDownList pobjDDL)
    {
        int nfilas = pobjDDL.Items.Count;

        for (int i = 0; i < nfilas; i++)
        {
            if (pobjDDL.Items[i].Value.Equals(skey))
            {
                pobjDDL.SelectedIndex = i;
                break;
            }
        }
    }

    public void cargaPaginacion(DropDownList pobjDDL, int nTotalPagina)
    {
        pobjDDL.Items.Clear();
        for (int i = 1; i <= nTotalPagina; i++)
        {
            pobjDDL.Items.Add(i.ToString());
        }
        if (nTotalPagina == 0)
        { pobjDDL.SelectedIndex = -1; }
        else
        { pobjDDL.SelectedIndex = 0; }
    }


    public void cargaSistema(DropDownList pobjDDL)
    {
        csSistemaBL objSistemaBL = new csSistemaBL();
        Seguridad.Logica.BussinessEntity.csTablaBE objBE = new Seguridad.Logica.BussinessEntity.csTablaBE();

        objBE = objSistemaBL.Consultar("0", "", "A", 100000, 1, "N");
        loadCombo(pobjDDL, objBE.dtRegistros, "SIST_VNOMBRE", "SIST_SSISTEMAID");
    }
    public void cargaSistema(DropDownList pobjDDL, string pstrItemText)
    {
        csSistemaBL objSistemaBL = new csSistemaBL();
        Seguridad.Logica.BussinessEntity.csTablaBE objBE = new Seguridad.Logica.BussinessEntity.csTablaBE();

        objBE = objSistemaBL.Consultar("0", "", "A", 100000, 1, "N");
        loadComboAndItem(pobjDDL, objBE.dtRegistros, "SIST_VNOMBRE", "SIST_SSISTEMAID", pstrItemText);
    }

    public void cargaRol(DropDownList pobjDDL, string strSistema)
    {
        csRolConfiguracionBL objRol = new csRolConfiguracionBL();

        Seguridad.Logica.BussinessEntity.csTablaBE objBE = new Seguridad.Logica.BussinessEntity.csTablaBE();

        objBE = objRol.Consultar("0", strSistema, "0", "", "A", 1000000, 1, "N");
        loadCombo(pobjDDL, objBE.dtRegistros, "ROCO_VNOMBRE", "ROCO_SROLCONFIGURACIONID");
    }
    public void cargaRol(DropDownList pobjDDL, string strSistema, string pstrItemText)
    {
        csRolConfiguracionBL objRol = new csRolConfiguracionBL();

        Seguridad.Logica.BussinessEntity.csTablaBE objBE = new Seguridad.Logica.BussinessEntity.csTablaBE();

        objBE = objRol.Consultar("0", strSistema, "0", "", "A", 1000000, 1, "N");
        loadComboAndItem(pobjDDL, objBE.dtRegistros, "ROCO_VNOMBRE", "ROCO_SROLCONFIGURACIONID", pstrItemText);
    }

    public bool IsPDFHeader(string fileName)
    {
        byte[] buffer = null;
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        //long numBytes = new FileInfo(fileName).Length;
        //buffer = br.ReadBytes((int)numBytes);
        buffer = br.ReadBytes(5);

        var enc = new ASCIIEncoding();
        var header = enc.GetString(buffer);

        bool ispdf = false;

        //%PDF−1.0
        // If you are loading it into a long, this is (0x04034b50).
        if (buffer[0] == 0x25 && buffer[1] == 0x50
            && buffer[2] == 0x44 && buffer[3] == 0x46)
        {
            ispdf = header.StartsWith("%PDF-");
        }
        br.Close();
        fs.Close();

        return ispdf;
    }

    //19.- Grabar a un arreglo byte desde FileUpload
    public byte[] obtenerArregloByteFromFileUpload(FileUpload Upload)
    {
        Byte[] byteimagen = null;
        byteimagen = new Byte[Upload.PostedFile.ContentLength];
        HttpPostedFile ImgFile = Upload.PostedFile;
        ImgFile.InputStream.Read(byteimagen, 0, Upload.PostedFile.ContentLength);
        return byteimagen;
    }
    //32.- Obtener PDF sin permisos
    public byte[] ObtenerPDFSinPermisos(byte[] data)
    {
        PdfReader pdfReader = new PdfReader(data);
        MemoryStream ms = new MemoryStream();
        PdfStamper stamper = new PdfStamper(pdfReader, ms);
        stamper.ViewerPreferences = PdfWriter.HideMenubar | PdfWriter.HideToolbar | PdfWriter.HideWindowUI;
        stamper.SetEncryption(null, null, 0, PdfWriter.STRENGTH128BITS);

        stamper.Close();

        pdfReader.Close();

        pdfReader = null;
        return ms.ToArray();
    }

    public MemoryStream InsertarMarcaAguaTextoToPDF(byte[] dataPDF, string strTexto)
    {
        MemoryStream ms = null;
        PdfReader pdfReader = null;
        PdfStamper pdfStamper = null;

        try
        {
            pdfReader = new iTextSharp.text.pdf.PdfReader(dataPDF);
            ms = new MemoryStream();
            pdfStamper = new PdfStamper(pdfReader, ms);



            BaseFont bf = BaseFont.CreateFont(@"c:\windows\fonts\arial.ttf", BaseFont.CP1252, true);
            PdfGState gs = new PdfGState();
            gs.FillOpacity = 0.35F;
            gs.StrokeOpacity = 0.35F;

            for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
            {

                iTextSharp.text.Rectangle tamPagina = pdfReader.GetPageSizeWithRotation(pageIndex);
                PdfContentByte over = pdfStamper.GetOverContent(pageIndex);

                over.BeginText();
                WriteTextDocument(bf, tamPagina, over, gs, strTexto);
                over.EndText();
            }
        }

        catch (Exception ex)
        {
            throw new ApplicationException("Exception occured. " + ex.Message);
        }
        finally
        {
            pdfReader.Close();
            if (pdfStamper != null) { pdfStamper.Close(); }
            if (ms != null) { ms.Close(); }


        }
        return ms;
    }

    private static void WriteTextDocument(BaseFont bf, iTextSharp.text.Rectangle tamPagina,
                                           PdfContentByte over, PdfGState gs, string texto)
    {
        over.SetGState(gs);
        over.SetRGBColorFill(220, 220, 220);
        over.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_STROKE);
        over.SetFontAndSize(bf, 46);

        Single anchoDiag =
            (Single)Math.Sqrt(Math.Pow((tamPagina.Height - 120), 2)
            + Math.Pow((tamPagina.Width - 60), 2));

        Single porc = (Single)100 * (anchoDiag / bf.GetWidthPoint(texto, 46));

        over.SetHorizontalScaling(porc);

        double angPage = (-1) * Math.Atan((tamPagina.Height - 60) / (tamPagina.Width - 60));

        over.SetTextMatrix((float)Math.Cos(angPage),
               (float)Math.Sin(angPage),
               (float)((-1F) * Math.Sin(angPage)),
               (float)Math.Cos(angPage),
               30F,
               (float)tamPagina.Height - 60);

        over.ShowText(texto);
    }
}