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
using System.Reflection;
using System.Net;
using System.Globalization;
using SolCARDIP.Librerias.ReglasNegocio;
using System.Net.Mail;
using System.Net.Mime;
using SAE.UInterfaces;
using SolCARDIP.Librerias.EntidadesNegocio;
using Microsoft.Security.Application;
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
    private static UIEncriptador UIEncripto = new UIEncriptador();
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

    public bool obtenerTipos(object obj1, object obj2)
    {
        bool iguales = true;
        object objeto1;
        object objeto2;
        objeto1 = obj1;
        objeto2 = obj2;

        Type tipo1 = objeto1.GetType();
        Type tipo2 = objeto2.GetType();
        PropertyInfo[] prop1 = tipo1.GetProperties();
        PropertyInfo[] prop2 = tipo2.GetProperties();
        int n = 1;

        string[] valores1 = new string[prop1.Length];
        //string[] valores1 = new string[prop1.Count()];
        foreach (PropertyInfo m_propiedad1 in prop1)
        {
            if (prop1[n - 1].GetValue(objeto1, null) != null)
            {
                valores1[n - 1] = prop1[n - 1].GetValue(objeto1, null).ToString();
            }
            else
            {
                valores1[n - 1] = "null";
            }
            n = ++n;
        }
        n = 1;

        string[] valores2 = new string[prop2.Length];
        //string[] valores2 = new string[prop2.Count()];
        foreach (PropertyInfo m_propiedad2 in prop2)
        {
            if (prop2[n - 1].GetValue(objeto2, null) != null)
            {
                valores2[n - 1] = prop2[n - 1].GetValue(objeto2, null).ToString();
            }
            else
            {
                valores2[n - 1] = "null";
            }
            n = ++n;
        }

        //for (int i = 0; i < valores1.Count(); i++)
        for (int i = 0; i < valores1.Length; i++)
        {
            if (!valores1[i].Equals(valores2[i]))
            {
                iguales = false;
                break;
            }
        }
        return (iguales);
    }

    public string obtenerIP()
    {
        string strHostName = Dns.GetHostName();
        IPHostEntry IPEntry = Dns.GetHostEntry(strHostName);

        string strDireccionIP = IPEntry.AddressList.GetValue(1).ToString();
        return strDireccionIP;
    }

    public string obtenerNombreHost()
    {
        string strHostName = string.Empty;
        strHostName = Dns.GetHostName();
        return (strHostName);
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
            if (c is ImageButton && c.ID != "ibtSalir")
            {
                ((ImageButton)(c)).Enabled = State;
            }
            if (c is FileUpload)
            {
                ((FileUpload)(c)).Enabled = State;
            }
            if (c is GridView)
            {
                ((GridView)(c)).Enabled = State;
            }
            if (c is System.Web.UI.WebControls.Image && c.ID != "ibtSalir")
            {
                ((System.Web.UI.WebControls.Image)(c)).Enabled = State;
            }
            DisableControls(c, State);
        }
    }

    public string LetraCapital(string CadenaTexto)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CadenaTexto);
    }

    public TimeSpan fechaCaducidad(DateTime fechaInicio, DateTime fechaFin)
    {
        TimeSpan fechaCaducidad = fechaInicio - fechaFin;
        return TimeSpan.Parse(fechaCaducidad.Days.ToString());
    }

    public string Encriptar(string _cadenaAencriptar)
    {
        string result = string.Empty;
        byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
        result = Convert.ToBase64String(encryted);
        return result;
    }

    /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
    public string DesEncriptar(string _cadenaAdesencriptar)
    {
        try
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
            return "Error";
        }
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

    public void pruebaEnvioCorreo(string destinatario)
    {
        MailMessage oMail = new MailMessage();
        oMail.From = new MailAddress("lnunja@rree.gob.pe");
        oMail.To.Add(destinatario);
        oMail.Subject = "Sistema de Envio Virtual de Documentos";
        oMail.Body = "Administrador: Usted tiene una solicitud que atender";
        SmtpClient oSMTP = new SmtpClient("vicus.rree.gob.pe");
        oSMTP.Credentials = new System.Net.NetworkCredential("lnunja@rree.gob.pe", "nunja1584");
        oSMTP.Send(oMail);
    }

    public string envioCorreoImagen(List<string> destinatario, string identificadorRegistro, string Proceso, string TipoDocTramiteDes, int ubicacion, int observacion, int rechazo, int reiteracion)
    {
        try
        {
            MailMessage mail = new MailMessage();
            string correoAdminEncriptado = ConfigurationManager.AppSettings["correoAdmin"];
            string correoAdmin = UIEncripto.DesEncriptarCadena(correoAdminEncriptado);
            //string passAdminEncriptado = ConfigurationManager.AppSettings["passwordAdmin"];
            //string passAdmin = UIEncripto.DesEncriptarCadena(passAdminEncriptado);
            string rutaImagenEncriptada = ConfigurationManager.AppSettings["pathImagenCorreo"];
            string rutaImagen = UIEncripto.DesEncriptarCadena(rutaImagenEncriptada);
            string protocoloEncriptado = ConfigurationManager.AppSettings["protocoloSMTP"];
            string protocolo = UIEncripto.DesEncriptarCadena(protocoloEncriptado);
            string puertoEncriptado = ConfigurationManager.AppSettings["puerto"];
            string puerto = UIEncripto.DesEncriptarCadena(puertoEncriptado);
            string urlSistemaEncriptado = ConfigurationManager.AppSettings["urlSistema"];
            string urlSistema = UIEncripto.DesEncriptarCadena(urlSistemaEncriptado);
            mail.From = new MailAddress(correoAdmin);
            for (int n = 0; n <= destinatario.Count - 1; n++)
            {
                mail.To.Add(destinatario[n]);
            }
            mail.Body = "Administrador: Usted tiene una solicitud que atender";
            string text = "Administrador: Usted tiene una solicitud que atender";
            string textoCUerpo = "";
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(text, Encoding.UTF8, MediaTypeNames.Text.Plain);
            #region Texto de Asunto y Cuerpo
            if (ubicacion == 1)
            {
                if (Proceso == "SOLICITUD")
                {
                    if (reiteracion == 1)
                    {
                        textoCUerpo = "<table style='width:35%;height:15%;font-family:Trebuchet MS;font-style:normal;font-size:14px;font-weight:bold;text-align:left;border-style:solid;border-width:1px;border-color:Black;'><tr><td rowspan='5' style='border-style:solid;border-width:1px;border-color:Black;'><img src='cid:imagen' width='65px' height='70px' alt=''/></td><td>Estimado Usuario(a):</td></tr><tr><td>Se REITERA la solicitud del registro.</td></tr><tr><td>N° de Solicitud: " + identificadorRegistro + "</td></tr><tr><td>Tipo de Documento: " + TipoDocTramiteDes + "</td></tr><tr><td>Sírvase revisar la aplicacion: <a href='" + urlSistema + "'>Envio Virtual de Documentos</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table><table><tr><td style='font-family:Trebuchet MS;font-style:normal;font-size:10px;font-weight:bold;text-align:left;'>Este correo ha sido enviado de manera automatica y no debe ser respondido.</td></tr></table>";
                        mail.Subject = "Sistema de Envio Virtual de Documentos - REITERACION DE SOLICITUD";
                    }
                    else
                    {
                        if (observacion == 1)
                        {
                            textoCUerpo = "<table style='width:35%;height:15%;font-family:Trebuchet MS;font-style:normal;font-size:14px;font-weight:bold;text-align:left;border-style:solid;border-width:1px;border-color:Black;'><tr><td rowspan='5' style='border-style:solid;border-width:1px;border-color:Black;'><img src='cid:imagen' width='65px' height='70px' alt=''/></td><td>Estimado Usuario(a):</td></tr><tr><td>Se ha Observado un registro de Solicitud.</td></tr><tr><td>N° de Solicitud: " + identificadorRegistro + "</td></tr><tr><td>Tipo de Documento: " + TipoDocTramiteDes + "</td></tr><tr><td>Sírvase revisar la aplicacion: <a href='" + urlSistema + "'>Envio Virtual de Documentos</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table><table><tr><td style='font-family:Trebuchet MS;font-style:normal;font-size:10px;font-weight:bold;text-align:left;'>Este correo ha sido enviado de manera automatica y no debe ser respondido.</td></tr></table>";
                            mail.Subject = "Sistema de Envio Virtual de Documentos - Solicitud Observada";
                        }
                        else
                        {
                            textoCUerpo = "<table style='width:35%;height:15%;font-family:Trebuchet MS;font-style:normal;font-size:14px;font-weight:bold;text-align:left;border-style:solid;border-width:1px;border-color:Black;'><tr><td rowspan='5' style='border-style:solid;border-width:1px;border-color:Black;'><img src='cid:imagen' width='65px' height='70px' alt=''/></td><td>Estimado Usuario(a):</td></tr><tr><td>Usted ha recibido una Solicitud que debe atender.</td></tr><tr><td>N° de Solicitud: " + identificadorRegistro + "</td></tr><tr><td>Tipo de Documento: " + TipoDocTramiteDes + "</td></tr><tr><td>Sírvase revisar la aplicacion: <a href='" + urlSistema + "'>Envio Virtual de Documentos</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table><table><tr><td style='font-family:Trebuchet MS;font-style:normal;font-size:10px;font-weight:bold;text-align:left;'>Este correo ha sido enviado de manera automatica y no debe ser respondido.</td></tr></table>";
                            mail.Subject = "Sistema de Envio Virtual de Documentos - Solicitud Nueva";
                        }
                    }
                }
            }
            else
            {
                if (Proceso == "SOLICITUD")
                {
                    if (rechazo == 1)
                    {
                        textoCUerpo = "<table style='width:35%;height:15%;font-family:Trebuchet MS;font-style:normal;font-size:14px;font-weight:bold;text-align:left;border-style:solid;border-width:1px;border-color:Black;'><tr><td rowspan='5' style='border-style:solid;border-width:1px;border-color:Black;'><img src='cid:imagen' width='65px' height='70px' alt=''/></td><td>Estimado Usuario(a):</td></tr><tr><td>Se ha rechazado un registro de Solicitud.</td></tr><tr><td>N° de Solicitud: " + identificadorRegistro + "</td></tr><tr><td>Tipo de Documento: " + TipoDocTramiteDes + "</td></tr><tr><td>Sírvase revisar la aplicacion: <a href='" + urlSistema + "'>Envio Virtual de Documentos</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table><table><tr><td style='font-family:Trebuchet MS;font-style:normal;font-size:10px;font-weight:bold;text-align:left;'>Este correo ha sido enviado de manera automatica y no debe ser respondido.</td></tr></table>";
                        mail.Subject = "Sistema de Envio Virtual de Documentos - Solicitud Rechaza";
                    }
                    else
                    {
                        textoCUerpo = "<table style='width:35%;height:15%;font-family:Trebuchet MS;font-style:normal;font-size:14px;font-weight:bold;text-align:left;border-style:solid;border-width:1px;border-color:Black;'><tr><td rowspan='5' style='border-style:solid;border-width:1px;border-color:Black;'><img src='cid:imagen' width='65px' height='70px' alt=''/></td><td>Estimado Usuario(a):</td></tr><tr><td>Se ha respondido un registro de Solicitud.</td></tr><tr><td>N° de Solicitud: " + identificadorRegistro + "</td></tr><tr><td>Tipo de Documento: " + TipoDocTramiteDes + "</td></tr><tr><td>Sírvase revisar la aplicacion: <a href='" + urlSistema + "'>Envio Virtual de Documentos</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table><table><tr><td style='font-family:Trebuchet MS;font-style:normal;font-size:10px;font-weight:bold;text-align:left;'>Este correo ha sido enviado de manera automatica y no debe ser respondido.</td></tr></table>";
                        mail.Subject = "Sistema de Envio Virtual de Documentos - Solicitud Respondida";
                    }
                }
                else
                {
                    textoCUerpo = "<table style='width:35%;height:15%;font-family:Trebuchet MS;font-style:normal;font-size:14px;font-weight:bold;text-align:left;border-style:solid;border-width:1px;border-color:Black;'><tr><td rowspan='5' style='border-style:solid;border-width:1px;border-color:Black;'><img src='cid:imagen' width='65px' height='70px' alt=''/></td><td>Estimado Usuario(a):</td></tr><tr><td>Usted tiene un Envio nuevo que atender</td></tr><tr><td>N° de Envio: " + identificadorRegistro + "</td></tr><tr><td>Tipo de Documento: " + TipoDocTramiteDes + "</td></tr><tr><td>Sírvase revisar la aplicacion: <a href='" + urlSistema + "'>Envio Virtual de Documentos</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table><table><tr><td style='font-family:Trebuchet MS;font-style:normal;font-size:10px;font-weight:bold;text-align:left;'>Este correo ha sido enviado de manera automatica y no debe ser respondido.</td></tr></table>";
                    mail.Subject = "Sistema de Envio Virtual de Documentos - Envio Nuevo";
                }
            }
            #endregion
            string html = textoCUerpo; //+ "<img src='cid:imagen' />";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);
            string rutaOrigen = System.Threading.Thread.GetDomain().BaseDirectory; //HttpContext.Current.Server.MapPath(@"/");
            LinkedResource img = new LinkedResource(rutaOrigen + rutaImagen, MediaTypeNames.Image.Jpeg);
            img.ContentId = "imagen";
            htmlView.LinkedResources.Add(img);
            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);
            SmtpClient smtp = new SmtpClient(protocolo, int.Parse(puerto.ToString()));
            smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new System.Net.NetworkCredential(correoAdmin, passAdmin);
            smtp.Send(mail);
            string mensaje = "ALERTA ENVIADA CON EXITO.";
            return mensaje;
        }
        catch (Exception ex)
        {
            string mensaje = "NO SE PUDO ENVIAR LA ALERTA.";
            obrGeneral.grabarLog(ex);
            return mensaje;
        }
    }
    public bool EnviarCorreoPlantillaHTML(
                                  string pstrRUTACorreo
                                , DataTable pDtParametros
                                , string pstrSMTPServer
                                , string pintSMTPPort
                                , string pstrEmailFrom
                                , string pstrEmailPassword
                                , string pstrEmailTo
                                , string pstrSubject
                                , MailPriority pstrPriority
                                , List<string> plstAttachments)
    {
        bool enviado = false;
        MailMessage oMail = new MailMessage();
        SmtpClient oSmtp = new SmtpClient();
        try
        {
            string htmlTexto = string.Empty;
            string filePath = pstrRUTACorreo;
            StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            htmlTexto = text;
            for (int index = 0; index <= pDtParametros.Rows.Count - 1; index++)
            {
                htmlTexto = htmlTexto.Replace(Convert.ToString(pDtParametros.Rows[index][0]), Convert.ToString(pDtParametros.Rows[index][1]));
            }

            // Remitente
            if (pstrEmailFrom == null)
                throw new Exception("No hay Remitente");
            if (pstrEmailFrom == string.Empty)
                throw new Exception("No hay Remitente");
            oMail.From = new MailAddress(pstrEmailFrom);

            // Destinatario
            string[] ArrayPara = pstrEmailTo.Split(';');
            foreach (string strPara in ArrayPara)
                oMail.To.Add(strPara.Trim());

            if (oMail.To == null)
                throw new Exception("No hay Destinatario");
            if (oMail.To.Count < 1)
                throw new Exception("No hay Destinatario");

            oMail.Subject = pstrSubject;
            oMail.IsBodyHtml = true;
            oMail.Body = htmlTexto;

            oMail.Priority = pstrPriority;

            if (plstAttachments != null)
            {
                foreach (var item in plstAttachments)
                {
                    oMail.Attachments.Add(new Attachment(item));
                }
            }

            oSmtp.Host = pstrSMTPServer;
            oSmtp.Port = Convert.ToInt32(pintSMTPPort);
            oSmtp.UseDefaultCredentials = true;
            try
            {
                oSmtp.Send(oMail);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }

            enviado = true;
        }
        catch (Exception ex)
        {
            string strMensaje = string.Empty;
            if (ex.Message.Contains("The specified string is not in the form required for an e-mail address"))
                strMensaje = "Dirección de correo incorrecta.";
            else if (ex.Message.Contains("Failure sending mail"))
                strMensaje = "Error en el envío de correo, verifique la conexión de Internet.";
            else if (ex.Message.Contains("SMTP server requires a secure connection or the client was not authenticated"))
            {
                //SMTP server requires a secure connection or the client was not authenticated
                strMensaje = "Servidor SMTP requiere una conexión segura o cliente no ha sido autenticado";
            }
            else
                strMensaje = ex.Message;

            throw new Exception(strMensaje);
        }
        finally
        {
            if (oMail != null)
            {
                oMail.Dispose();
                oMail = null;
            }
            if (oSmtp != null)
            {
                oSmtp.Dispose();
                oSmtp = null;
            }
        }
        return enviado;
    }
    public bool EnviarCorreo(
                                  string pstrSMTPServer
                                , string pintSMTPPort
                                , string pstrEmailFrom
                                , string pstrEmailPassword
                                , string pstrEmailTo
                                , string pstrSubject
                                , MailPriority pstrPriority , 
                                  string htmlTexto)
    {
        bool enviado = false;
        MailMessage oMail = new MailMessage();
        SmtpClient oSmtp = new SmtpClient();
        try
        {
            // Remitente
            if (pstrEmailFrom == null)
                throw new Exception("No hay Remitente");
            if (pstrEmailFrom == string.Empty)
                throw new Exception("No hay Remitente");
            oMail.From = new MailAddress(pstrEmailFrom);

            // Destinatario
            string[] ArrayPara = pstrEmailTo.Split(';');
            foreach (string strPara in ArrayPara)
                oMail.To.Add(strPara.Trim());

            if (oMail.To == null)
                throw new Exception("No hay Destinatario");
            if (oMail.To.Count < 1)
                throw new Exception("No hay Destinatario");

            oMail.Subject = pstrSubject;
            oMail.IsBodyHtml = true;
            oMail.Body = htmlTexto;

            oMail.Priority = pstrPriority;
            //oMail.Attachments.Add(new Attachment(item));
                

            oSmtp.Host = pstrSMTPServer;
            oSmtp.Port = Convert.ToInt32(pintSMTPPort);
            oSmtp.UseDefaultCredentials = true;
            try
            {
                oSmtp.Send(oMail);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }

            enviado = true;
        }
        catch (Exception ex)
        {
            string strMensaje = string.Empty;
            if (ex.Message.Contains("The specified string is not in the form required for an e-mail address"))
                strMensaje = "Dirección de correo incorrecta.";
            else if (ex.Message.Contains("Failure sending mail"))
                strMensaje = "Error en el envío de correo, verifique la conexión de Internet.";
            else if (ex.Message.Contains("SMTP server requires a secure connection or the client was not authenticated"))
            {
                //SMTP server requires a secure connection or the client was not authenticated
                strMensaje = "Servidor SMTP requiere una conexión segura o cliente no ha sido autenticado";
            }
            else
                strMensaje = ex.Message;

            throw new Exception(strMensaje);
        }
        finally
        {
            if (oMail != null)
            {
                oMail.Dispose();
                oMail = null;
            }
            if (oSmtp != null)
            {
                oSmtp.Dispose();
                oSmtp = null;
            }
        }
        return enviado;
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
        catch(Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (lbeCalidadMigratoria);
    }

    public List<beOficinaconsularExtranjera> obtenerOficinasConsularesExtranjeras(short CategoriaId, List<beOficinaconsularExtranjera> ListaOficinas)
    {
        List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();
        try
        {
            lbeOficinaconsularExtranjera = ListaOficinas.FindAll(x => x.Categoriaid == CategoriaId);
        }
        catch(Exception ex)
        {
            lbeOficinaconsularExtranjera = null;
            obrGeneral.grabarLog(ex);
        }
        return(lbeOficinaconsularExtranjera);
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

    public bool compareValTK(string valorControlEnc, string valorSaveEnc)
    {
        bool exitoTKSEG = false;
        if (!valorControlEnc.Equals("") & !valorSaveEnc.Equals(""))
        {
            string valorControl = UIEncripto.DesEncriptarCadena(valorControlEnc);
            string valorSave = UIEncripto.DesEncriptarCadena(valorSaveEnc);
            if (valorControl.Equals("") | valorSave.Equals(""))
            {
                exitoTKSEG = false;
                obrGeneral.grabarError("TK vacio | Valor Control: " + valorControl + " | ValorSave: " + valorSave);
            }
            else
            {
                if (!valorControl.Equals(valorSave))
                {
                    obrGeneral.grabarError("Valor TK distinto | Valor Control: " + valorControl + " | ValorSave: " + valorSave);
                    exitoTKSEG = false;
                }
                else
                {
                    exitoTKSEG = true;
                }
            }
        }
        else
        {
            obrGeneral.grabarError("TK vacio | Valor ControlEnc: " + valorControlEnc + " | ValorSaveEnc: " + valorSaveEnc);
        }
        return (exitoTKSEG);
    }

    public string generateValTK(string formName)
    {
        string TKENC = UIEncripto.EncriptarCadena("TKSEG" + formName);// + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString());
        return (TKENC);
    }

    public string evaluateAntiXSS(string strEvaluate)
    {
        string strEnc = Microsoft.Security.Application.Encoder.HtmlEncode(strEvaluate);
        return (strEnc);
    }

    public bool evaluarLetras(string strEvaluar)
    {
        bool exito = false;
        try
        {
            string[] arrABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " " };
            if (strEvaluar.Length > 0)
            {
                for (int n = 0; n <= strEvaluar.Length - 1; n++)
                {
                    string caracter = strEvaluar[n].ToString();
                    int result = Array.FindIndex(arrABC, x => x.Equals(caracter));
                    if (result == -1) { exito = false; break; }
                    else { exito = true; }
                }
            }
            else { exito = true; }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (exito);
    }

    public bool evaluarNumeros(string strEvaluar)
    {
        bool exito = false;
        try
        {
            string[] arrABC = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " " };
            if(strEvaluar.Length > 0)
            {
                for (int n = 0; n <= strEvaluar.Length - 1; n++)
            {
                string caracter = strEvaluar[n].ToString();
                int result = Array.FindIndex(arrABC, x => x.Equals(caracter));
                if (result == -1) { exito = false; break; }
                else { exito = true; }
            }
            }
            else { exito = true; }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (exito);
    }

    public bool evaluarAlfaNum(string strEvaluar)
    {
        bool exito = false;
        try
        {
            string[] arrABC = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " "};
            if (strEvaluar.Length > 0)
            {
                for (int n = 0; n <= strEvaluar.Length - 1; n++)
                {
                    string caracter = strEvaluar[n].ToString();
                    int result = Array.FindIndex(arrABC, x => x.Equals(caracter));
                    if (result == -1) { exito = false; break; }
                    else { exito = true; }
                }
            }
            else { exito = true; }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (exito);
    }

    public bool evaluarAlfaNumSim(string strEvaluar)
    {
        bool exito = false;
        try
        {
            string[] arrABC = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "-", "/", ".", " " };
            if (strEvaluar.Length > 0)
            {
                for (int n = 0; n <= strEvaluar.Length - 1; n++)
                {
                    string caracter = strEvaluar[n].ToString();
                    int result = Array.FindIndex(arrABC, x => x.Equals(caracter));
                    if (result == -1) { exito = false; break; }
                    else { exito = true; }
                }
            }
            else { exito = true; }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (exito);
    }

    public bool evaluarFecha(string strEvaluar)
    {
        bool exito = false;
        try
        {
            string[] arrABC = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/" };
            if (strEvaluar.Length > 0)
            {
                for (int n = 0; n <= strEvaluar.Length - 1; n++)
                {
                    string caracter = strEvaluar[n].ToString();
                    int result = Array.FindIndex(arrABC, x => x.Equals(caracter));
                    if (result == -1) { exito = false; break; }
                    else { exito = true; }
                }
            }
            else { exito = true; }
        }
        catch (Exception ex)
        {
            obrGeneral.grabarLog(ex);
        }
        return (exito);
    }

    public string encriptarDatos(string datos)
    {
        string cadenaEncr = "error";
        byte[] bDatos = System.Text.Encoding.UTF8.GetBytes(datos);
        cadenaEncr = Convert.ToBase64String(bDatos);
        return (cadenaEncr);
    }

    public String crearSTR(string url, String currurl, string userAlias, string srtAl, int identChar)
    {
        Uri ur = new Uri(url);
        string host = ur.Host;
        string aliasUser = userAlias;//objUsuarioBE.Alias;
        string strComp = encriptarDatos("host=" + host + ",userAlias=" + aliasUser + ",key=" + srtAl);
        char concatChar = (identChar == 0 ? '?' : '&');
        String redirecturl = currurl + concatChar + "valS=" + strComp;
        return (redirecturl);
    }

    public bool validarSTR(string str, string sessionAlias, string sessionHost, string sessionKey)
    {
        bool exito = false;
        if (str != null)
        {
            if (!str.Equals("error"))
            {
                byte[] byteCookie = Convert.FromBase64String(str);
                string strDesEnc = System.Text.Encoding.UTF8.GetString(byteCookie);
                string strClear = strDesEnc.Replace("\0", string.Empty);

                int posHost = strClear.IndexOf("host");
                int posUseAlias = strClear.IndexOf("userAlias");
                int posKey = strClear.IndexOf("key");

                if (posHost != -1 & posUseAlias != -1 & posKey != -1)
                {
                    string cadenaHost = strClear.Substring(posHost, strClear.IndexOf(",", posHost));
                    string cadenaUser = strClear.Substring(posUseAlias, posKey - (cadenaHost.Length + 2));
                    string cadenaKey = strClear.Substring(posKey, strClear.Length - ((cadenaHost.Length + 1) + (cadenaUser.Length + 2) - 1));

                    string host = cadenaHost.Substring(cadenaHost.IndexOf("=") + 1, cadenaHost.Length - (cadenaHost.IndexOf("=") + 1));
                    string userAlias = cadenaUser.Substring(cadenaUser.IndexOf("=") + 1, cadenaUser.Length - (cadenaUser.IndexOf("=") + 1));
                    string key = cadenaKey.Substring(cadenaKey.IndexOf("=") + 1, cadenaKey.Length - (cadenaKey.IndexOf("=") + 1));

                    if (userAlias.Equals(sessionAlias)) { exito = true; } else { exito = false; obrGeneral.grabarError("Alias es distinto | strClear:" + strClear); }
                    if (exito)
                    {
                        if (host.Equals(sessionHost)) { exito = true; } else { exito = false; obrGeneral.grabarError("Host es distinto | strClear:" + strClear); }
                    }
                    if (exito)
                    {
                        if (key.Equals(sessionKey)) { exito = true; } else { exito = false; obrGeneral.grabarError("key es distinto | strClear:" + strClear); }
                    }
                }
                else
                {
                    exito = false;
                    obrGeneral.grabarError("valor no encontrado | posHost: " + posHost + " | posUseAlias: " + posUseAlias + " | posKey: " + posKey + " | strClear:" + strClear);
                }
            }
            else
            {
                obrGeneral.grabarError("str es error");
            }
        }
        else
        {
            obrGeneral.grabarError("str es null");
        }
        return (exito);
    }

    public string generarStrAleatorio()
    {
        string cadenaAleatoria;
        int[] arrInt = new int[] { 15, 20, 25, 30, 35 };
        Random rndCantCar = new Random();
        int n = rndCantCar.Next(0, arrInt.Length - 1);
        int cantCar = arrInt[n];

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < cantCar; i++)
        {
            sb.Append(obtenerCaracter());
        }
        cadenaAleatoria = sb.ToString();
        return (cadenaAleatoria);
    }

    private char obtenerCaracter()
    {
        Random oAzar = new Random();
        int n = oAzar.Next(26) + 65;
        System.Threading.Thread.Sleep(15);
        return ((char)n);
    }

    // CREAR PDF

    //public byte[] crearPDF1(beCarneIdentidad obeCarneIdentidad, string userComp)
    //{
    //    StringWriter sw = new StringWriter();
    //    string html = sw.ToString();

    //    Document doc = new Document(PageSize.A4);
    //    doc.SetMargins(10, 10, 0, 10);
    //    MemoryStream ms = new MemoryStream();
    //    PdfWriter pdfw = PdfWriter.GetInstance(doc, ms);
    //    doc.Open();
    //    // CREACION ---------------------------------------------------------------------------------------
    //    Chunk saltoLinea = new Chunk("\n");
    //    Paragraph pSaltoLinea = new Paragraph();
    //    pSaltoLinea.Alignment = Element.ALIGN_LEFT;
    //    pSaltoLinea.Add(saltoLinea);
    //    #region Fuentes
    //    FontFactory.RegisterDirectories();
    //    iTextSharp.text.Font fuenteSmall = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 6, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteSmallBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteMedium = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 7, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteMediumBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    #endregion
    //    #region Cabecera
    //    PdfPTable tableCab = new PdfPTable(2);
    //    tableCab.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    PdfPCell cell1 = nuevaCelda("Impreso por:  ", userComp, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
    //    tableCab.AddCell(cell1);
    //    PdfPCell cell2 = nuevaCelda("Solicitud:  ", obeCarneIdentidad.ConIdent, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
    //    tableCab.AddCell(cell2);
    //    string fechaHora = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString("D2").ToString() + ":" + DateTime.Now.Minute.ToString("D2").ToString() + ":" + DateTime.Now.Second.ToString("D2").ToString();
    //    PdfPCell cell3 = nuevaCelda("Fecha / Hora Impresión:  ", fechaHora, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
    //    tableCab.AddCell(cell3);
    //    PdfPCell cell4 = nuevaCelda("Mesa de Partes:  ", obeCarneIdentidad.IdentMesaPartes, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
    //    tableCab.AddCell(cell4);
    //    #endregion
    //    #region Titulo
    //    PdfPTable tableTitulo = new PdfPTable(1);
    //    tableTitulo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    PdfPCell cellTitulo1 = nuevaCelda("", "INFORMACIÓN DE REGISTRO", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
    //    tableTitulo.AddCell(cellTitulo1);
    //    PdfPCell cellTitulo2 = nuevaCelda("NRO CARNÉ:  ", obeCarneIdentidad.CarneNumero, fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
    //    tableTitulo.AddCell(cellTitulo2);
    //    #endregion
    //    #region Cuerpo1
    //    PdfPTable tableCuerpo1 = new PdfPTable(7);
    //    tableCuerpo1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    float[] widths1 = new float[] { 15f, 5f, 35f, 5f, 15f, 5f, 20f };
    //    tableCuerpo1.SetWidths(widths1);
    //    PdfPCell cellCuerpo1_puntos = nuevaCelda(":", "", fuenteMediumBold, Element.ALIGN_CENTER, 1, 0);

    //    PdfPCell cellCuerpo1 = nuevaCelda("PRIMER APELLIDO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo1);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo2 = nuevaCelda("", obeCarneIdentidad.ApePatPersona, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo2);
    //    tableCuerpo1.AddCell("");
    //    PdfPCell cellCuerpo4 = nuevaCelda("FECHA INS.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo4);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo5 = nuevaCelda("", obeCarneIdentidad.ConFechaInscripcion, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo5);
        
    //    PdfPCell cellCuerpo6 = nuevaCelda("SEGUNDO APELLIDO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo6);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo7 = nuevaCelda("", obeCarneIdentidad.ApeMatPersona, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo7);
    //    tableCuerpo1.AddCell("");
    //    PdfPCell cellCuerpo9 = nuevaCelda("FECHA EMI.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo9);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo10 = nuevaCelda("", obeCarneIdentidad.ConFechaEmision, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo10);
        
    //    PdfPCell cellCuerpo11 = nuevaCelda("NOMBRES", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo11);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo12 = nuevaCelda("", obeCarneIdentidad.NombresPersona, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo12);
    //    tableCuerpo1.AddCell("");
    //    PdfPCell cellCuerpo13 = nuevaCelda("FECHA VENC.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo13);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo14 = nuevaCelda("", obeCarneIdentidad.ConFechaVen, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo14);

    //    PdfPCell cellCuerpo16 = nuevaCelda("FECHA NACIMIENTO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo16);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo17 = nuevaCelda("", obeCarneIdentidad.ConFechaNac, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo17);
    //    tableCuerpo1.AddCell("");
    //    PdfPCell cellCuerpo15 = nuevaCelda("ESTADO CARNÉ", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo15);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo18 = nuevaCelda("", obeCarneIdentidad.ConEstado, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo18);

    //    PdfPCell cellCuerpo21 = nuevaCelda("SEXO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo21);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo22 = nuevaCelda("", obeCarneIdentidad.ConGenero, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo22);
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");

    //    PdfPCell cellCuerpo23 = nuevaCelda("ESTADO CIVIL", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo23);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo24 = nuevaCelda("", obeCarneIdentidad.ConEstCivil, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo24);
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");

    //    PdfPCell cellCuerpo26 = nuevaCelda("PAIS (NACIONALIDAD)", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo26);
    //    tableCuerpo1.AddCell(cellCuerpo1_puntos);
    //    PdfPCell cellCuerpo27 = nuevaCelda("", obeCarneIdentidad.ConPaisNacionalidad, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo1.AddCell(cellCuerpo27);
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    tableCuerpo1.AddCell("");
    //    #endregion
    //    #region Cuerpo2
    //    PdfPTable tableCuerpo2 = new PdfPTable(3);
    //    tableCuerpo2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    float[] widths = new float[] { 15f, 5f, 80f, };
    //    tableCuerpo2.SetWidths(widths);
    //    PdfPCell cellCuerpo2_puntos = nuevaCelda(":", "", fuenteMediumBold, Element.ALIGN_CENTER, 1, 0);

    //    PdfPCell cellCuerpo2_1 = nuevaCelda("CAL. MIGRATORIA", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_1);
    //    tableCuerpo2.AddCell(cellCuerpo2_puntos);
    //    PdfPCell cellCuerpo2_2 = nuevaCelda("", obeCarneIdentidad.ConCalidadMigratoria, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_2);
    //    PdfPCell cellCuerpo2_16 = nuevaCelda("CARGO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_16);
    //    tableCuerpo2.AddCell(cellCuerpo2_puntos);
    //    PdfPCell cellCuerpo2_17 = nuevaCelda("", obeCarneIdentidad.ConCargo, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_17);
    //    PdfPCell cellCuerpo2_6 = nuevaCelda("CAT. INSTITUCIÓN", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_6);
    //    tableCuerpo2.AddCell(cellCuerpo2_puntos);
    //    PdfPCell cellCuerpo2_7 = nuevaCelda("", obeCarneIdentidad.ConCatMision, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_7);
    //    PdfPCell cellCuerpo2_11 = nuevaCelda("INTITUCIÓN", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_11);
    //    tableCuerpo2.AddCell(cellCuerpo2_puntos);
    //    PdfPCell cellCuerpo2_12 = nuevaCelda("", obeCarneIdentidad.ConOficinaConsularEx, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
    //    tableCuerpo2.AddCell(cellCuerpo2_12);
    //    #endregion
    //    #region Armado Doc
    //    doc.Add(pSaltoLinea);
    //    doc.Add(pSaltoLinea);
    //    doc.Add(tableCab);
    //    doc.Add(pSaltoLinea);
    //    doc.Add(tableTitulo);
    //    doc.Add(pSaltoLinea);
    //    doc.Add(tableCuerpo1);
    //    doc.Add(pSaltoLinea);
    //    doc.Add(tableCuerpo2);
    //    #endregion
    //    HTMLWorker worker = new HTMLWorker(doc);
    //    worker.Parse(new StringReader(html));
    //    doc.Close();
    //    pdfw.Close();

    //    byte[] pdfByte = ms.ToArray();
    //    return (pdfByte);
    //}

    public byte[] crearPDF1(beCarneIdentidad obeCarneIdentidad, string userComp)
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();

        Document doc = new Document(PageSize.A4);
        doc.SetMargins(10, 10, 0, 10);
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
        #region Cabecera
        PdfPTable tableCab = new PdfPTable(2);
        tableCab.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cell1 = nuevaCelda("Impreso por:  ", userComp, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCab.AddCell(cell1);
        PdfPCell cell2 = nuevaCelda("Solicitud:  ", obeCarneIdentidad.ConIdent, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
        tableCab.AddCell(cell2);
        string fechaHora = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString("D2").ToString() + ":" + DateTime.Now.Minute.ToString("D2").ToString() + ":" + DateTime.Now.Second.ToString("D2").ToString();
        PdfPCell cell3 = nuevaCelda("Fecha / Hora Impresión:  ", fechaHora, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCab.AddCell(cell3);
        PdfPCell cell4 = nuevaCelda("Mesa de Partes:  ", obeCarneIdentidad.IdentMesaPartes, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
        tableCab.AddCell(cell4);
        #endregion
        #region Titulo
        PdfPTable tableTitulo = new PdfPTable(1);
        tableTitulo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cellTitulo1 = nuevaCelda("", "INFORMACIÓN DE REGISTRO", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo1);
        PdfPCell cellTitulo2 = nuevaCelda("NRO CARNÉ:  ", obeCarneIdentidad.CarneNumero, fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo2);
        #endregion
        #region Cuerpo1
        PdfPTable tableCuerpo1 = new PdfPTable(7);
        tableCuerpo1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widths1 = new float[] { 15f, 5f, 35f, 5f, 15f, 5f, 20f };
        tableCuerpo1.SetWidths(widths1);
        PdfPCell cellCuerpo1_puntos = nuevaCelda(":", "", fuenteMediumBold, Element.ALIGN_CENTER, 1, 0);
        PdfPCell cellVacio = nuevaCelda("", "", fuenteMediumBold, Element.ALIGN_LEFT, 5, 0);
        PdfPCell cellVacio1 = nuevaCelda("", "", fuenteMediumBold, Element.ALIGN_LEFT, 7, 0);

        PdfPCell cellSub1 = nuevaCelda("DATOS PERSONALES", "", fuenteMediumBold, Element.ALIGN_LEFT, 5, 0);
        tableCuerpo1.AddCell(cellSub1);

        iTextSharp.text.Image jpgFoto;
        string rutaOrigen = System.Threading.Thread.GetDomain().BaseDirectory;
        if (File.Exists(obrGeneral.rutaAdjuntos + obeCarneIdentidad.RutaArchivoFoto))
        {
            jpgFoto = iTextSharp.text.Image.GetInstance(obrGeneral.rutaAdjuntos + obeCarneIdentidad.RutaArchivoFoto);
        }
        else
        {
            //jpgFoto = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("/Imagenes/notFound.jpg"));
            jpgFoto = iTextSharp.text.Image.GetInstance(rutaOrigen + "Imagenes\\notFound.jpg");
        }
        
        PdfPCell cellFoto = nuevaCeldaImagen(jpgFoto, Element.ALIGN_LEFT, 2, 1);
        cellFoto.Rowspan = 20;
        tableCuerpo1.AddCell(cellFoto);
        tableCuerpo1.AddCell(cellVacio);

        PdfPCell cellCuerpo1 = nuevaCelda("PRIMER APELLIDO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo1);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo2 = nuevaCelda("", obeCarneIdentidad.ApePatPersona, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo6 = nuevaCelda("SEGUNDO APELLIDO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo6);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo7 = nuevaCelda("", obeCarneIdentidad.ApeMatPersona, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo7);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo11 = nuevaCelda("NOMBRES", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo11);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo12 = nuevaCelda("", obeCarneIdentidad.NombresPersona, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo12);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo16 = nuevaCelda("FECHA NACIMIENTO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo16);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo17 = nuevaCelda("", obeCarneIdentidad.ConFechaNac, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo17);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo21 = nuevaCelda("SEXO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo21);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo22 = nuevaCelda("", obeCarneIdentidad.ConGenero, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo22);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo23 = nuevaCelda("ESTADO CIVIL", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo23);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo24 = nuevaCelda("", obeCarneIdentidad.ConEstCivil, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo24);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo26 = nuevaCelda("PAIS (NACIONALIDAD)", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo26);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo27 = nuevaCelda("", obeCarneIdentidad.ConPaisNacionalidad, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo27);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        /*--------------adicion de nro pasaporte---*/
        PdfPCell cellCuerpo26_1 = nuevaCelda("Tipo Doc.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo26_1);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo27_1 = nuevaCelda("", ""+obeCarneIdentidad.TipodocDesc, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo27_1);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo26_2 = nuevaCelda("Nro. Documento", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo26_2);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo27_2 = nuevaCelda("",  obeCarneIdentidad.DocumentoNumero, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo27_2);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");
        /*-------------------------*/

        PdfPCell cellCuerpo28 = nuevaCelda("DIRECCION", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo28);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo29 = nuevaCelda("", obeCarneIdentidad.ConDireccion, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo29);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo30 = nuevaCelda("UBIGEO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo30);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo31 = nuevaCelda("", obeCarneIdentidad.ConUbigeo, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo31);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        tableCuerpo1.AddCell(cellVacio);
        PdfPCell cellSub2 = nuevaCelda("DATOS DE CALIDAD MIGRATORIA", "", fuenteMediumBold, Element.ALIGN_LEFT, 5, 0);
        tableCuerpo1.AddCell(cellSub2);
        tableCuerpo1.AddCell(cellVacio);

        PdfPCell cellCuerpo2_1 = nuevaCelda("CAL. MIGRATORIA", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_1);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo2_2 = nuevaCelda("", obeCarneIdentidad.ConCalidadMigratoria, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_2);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo2_18 = nuevaCelda("VINCULO FAMILIAR", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_18);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo2_19 = nuevaCelda("", obeCarneIdentidad.ConTitDep, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_19);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");
        
        PdfPCell cellCuerpo2_16 = nuevaCelda("CARGO", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_16);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo2_17 = nuevaCelda("", obeCarneIdentidad.ConCargo, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_17);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");
        
        PdfPCell cellCuerpo2_6 = nuevaCelda("CAT. INSTITUCIÓN", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_6);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo2_7 = nuevaCelda("", obeCarneIdentidad.ConCatMision, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_7);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");
        
        PdfPCell cellCuerpo2_11 = nuevaCelda("INTITUCIÓN", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_11);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo2_12 = nuevaCelda("", obeCarneIdentidad.ConOficinaConsularEx, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo2_12);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        tableCuerpo1.AddCell(cellVacio1);

        PdfPCell cellSub3 = nuevaCelda("DATOS DE CARNÉ", "", fuenteMediumBold, Element.ALIGN_LEFT, 5, 0);
        tableCuerpo1.AddCell(cellSub3);
        tableCuerpo1.AddCell(cellVacio);

        PdfPCell cellCuerpo4 = nuevaCelda("FECHA INS.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo4);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo5 = nuevaCelda("", obeCarneIdentidad.ConFechaInscripcion, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo5);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        iTextSharp.text.Image jpgFirma;
        if (File.Exists(obrGeneral.rutaAdjuntos + obeCarneIdentidad.RutaArchivoFirma))
        {
            jpgFirma = iTextSharp.text.Image.GetInstance(obrGeneral.rutaAdjuntos + obeCarneIdentidad.RutaArchivoFirma);
        }
        else
        {
            //jpgFirma = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("/Imagenes/notFound.jpg"));
            jpgFirma = iTextSharp.text.Image.GetInstance(rutaOrigen + "Imagenes\\notFound.jpg");
        }

        PdfPCell cellFirma = nuevaCeldaImagenFirma(jpgFirma, Element.ALIGN_LEFT, 2, 1);
        cellFirma.Rowspan = 20;
        tableCuerpo1.AddCell(cellFirma);

        //PdfPCell cellFirma = nuevaCelda("", "", fuenteMedium, Element.ALIGN_LEFT, 2, 1);
        //cellFirma.Rowspan = 20;
        //tableCuerpo1.AddCell(cellFirma);

        PdfPCell cellCuerpo9 = nuevaCelda("FECHA EMI.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo9);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo10 = nuevaCelda("", obeCarneIdentidad.ConFechaEmision, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo10);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo13 = nuevaCelda("FECHA VENC.", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo13);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo14 = nuevaCelda("", obeCarneIdentidad.ConFechaVen, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo14);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        PdfPCell cellCuerpo15 = nuevaCelda("ESTADO CARNÉ", "", fuenteMediumBold, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo15);
        tableCuerpo1.AddCell(cellCuerpo1_puntos);
        PdfPCell cellCuerpo18 = nuevaCelda("", obeCarneIdentidad.ConEstado, fuenteMedium, Element.ALIGN_LEFT, 1, 0);
        tableCuerpo1.AddCell(cellCuerpo18);
        tableCuerpo1.AddCell("");
        tableCuerpo1.AddCell("");

        tableCuerpo1.AddCell(cellVacio);
        tableCuerpo1.AddCell(cellVacio);
        tableCuerpo1.AddCell(cellVacio);
        tableCuerpo1.AddCell(cellVacio);

        #endregion
        #region Cuerpo2
        PdfPTable tableCuerpo2 = new PdfPTable(3);
        tableCuerpo2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widths = new float[] { 15f, 5f, 80f, };
        tableCuerpo2.SetWidths(widths);
        PdfPCell cellCuerpo2_puntos = nuevaCelda(":", "", fuenteMediumBold, Element.ALIGN_CENTER, 1, 0);

        #endregion
        #region Armado Doc
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tableCab);
        doc.Add(pSaltoLinea);
        doc.Add(tableTitulo);
        doc.Add(pSaltoLinea);
        doc.Add(tableCuerpo1);
        doc.Add(pSaltoLinea);
        doc.Add(tableCuerpo2);
        #endregion
        HTMLWorker worker = new HTMLWorker(doc);
        worker.Parse(new StringReader(html));
        doc.Close();
        pdfw.Close();

        byte[] pdfByte = ms.ToArray();
        return (pdfByte);
    }

    public byte[] crearActaConformidad(beActaConformidadPrincipal datos, string userComp)
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();
        Document doc = new Document(PageSize.A4);
        doc.SetMargins(10, 10, 30, 30);
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
        iTextSharp.text.Font fuenteLarge = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteLargeBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        #endregion
        #region Cabecera
        PdfPTable tableCab = new PdfPTable(2);
        tableCab.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cell1 = nuevaCelda("Impreso por:  ", userComp, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCab.AddCell(cell1);
        string fechaHora = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString("D2").ToString() + ":" + DateTime.Now.Minute.ToString("D2").ToString() + ":" + DateTime.Now.Second.ToString("D2").ToString();
        PdfPCell cell3 = nuevaCelda("Fecha / Hora Impresión:  ", fechaHora, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
        tableCab.AddCell(cell3);
        #endregion
        #region Titulo
        PdfPTable tableTitulo = new PdfPTable(1);
        tableTitulo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cellTitulo1 = nuevaCelda("", "CARGO DE ENTREGA", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo1);
        #endregion
        #region Cuerpo1
        //PdfPTable tableCuerpo1 = new PdfPTable(3);
        //tableCuerpo1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //float[] widths1 = new float[] { 1f, 20f, 25f};
        //tableCuerpo1.SetWidths(widths1);

        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tableCab);
        doc.Add(pSaltoLinea);
        doc.Add(tableTitulo);
        doc.Add(pSaltoLinea);

        for (int i = 0; i <= datos.Instituciones.Count - 1; i++)
        {
            PdfPTable tableInstitcion = new PdfPTable(3);
            tableInstitcion.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float[] widthsInstitucion = new float[] { 1f, 20f, 25f };
            tableInstitcion.SetWidths(widthsInstitucion);

            List<beActaConformidadDetalle> listaFiltro = new List<beActaConformidadDetalle>();
            listaFiltro = datos.ActaDetalle.FindAll(x => x.ConOficinaId == datos.Instituciones[i].OficinaconsularExtranjeraid);
            PdfPCell Institucion1 = nuevaCelda1("INSTITUCIÓN: " + datos.Instituciones[i].Nombre + " / (" + listaFiltro.Count.ToString() + " Carné(s) a Entregar)", "", fuenteLargeBold, Element.ALIGN_LEFT, 1, 0, 3);
            tableInstitcion.AddCell(Institucion1);

            doc.Add(tableInstitcion);
            doc.Add(pSaltoLinea);

            PdfPTable tableCuerpo1 = new PdfPTable(3);
            tableCuerpo1.HeaderRows = 1;
            tableCuerpo1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float[] widths1 = new float[] { 2.5f, 35f, 15f };
            tableCuerpo1.SetWidths(widths1);

            PdfPCell Cabecera2 = nuevaCelda1("", "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0.5f, 1);
            tableCuerpo1.AddCell(Cabecera2);
            PdfPCell Cabecera3 = nuevaCelda1("TITULAR DEL CARNÉ", "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0.5f, 1);
            tableCuerpo1.AddCell(Cabecera3);
            PdfPCell Cabecera4 = nuevaCelda1("NÚMERO DE CARNÉ", "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0.5f, 1);
            tableCuerpo1.AddCell(Cabecera4);
            
            for (int x = 0; x <= listaFiltro.Count - 1; x++)
            {
                PdfPCell Contenido1 = nuevaCelda1((x + 1).ToString(), "", fuenteLarge, Element.ALIGN_CENTER, 1, 0.5f, 1);
                tableCuerpo1.AddCell(Contenido1);
                PdfPCell Contenido2 = nuevaCelda1(listaFiltro[x].ConTitular.ToUpper(), "", fuenteLarge, Element.ALIGN_LEFT, 1, 0.5f, 1);
                tableCuerpo1.AddCell(Contenido2);
                PdfPCell Contenido3 = nuevaCelda1(listaFiltro[x].ConNumeroCarne, "", fuenteLarge, Element.ALIGN_LEFT, 1, 0.5f, 1);
                tableCuerpo1.AddCell(Contenido3);
            }
            doc.Add(tableCuerpo1);
            doc.Add(pSaltoLinea);
        }

        PdfPTable tableTotalCarnes = new PdfPTable(3);
        tableTotalCarnes.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsTotalCarnes = new float[] { 1f, 20f, 25f };
        tableTotalCarnes.SetWidths(widthsTotalCarnes);

        PdfPCell cellTotalCarnes1 = nuevaCelda1("Total de Carnés a Entregar: " + datos.ActaDetalle.Count.ToString(), "", fuenteLargeBold, Element.ALIGN_LEFT, 1, 0, 3);
        tableTotalCarnes.AddCell(cellTotalCarnes1);

        PdfPCell cellTotalCarnes2 = nuevaCelda1("Total de Instituciones: " + datos.Instituciones.Count.ToString(), "", fuenteLargeBold, Element.ALIGN_LEFT, 1, 0, 3);
        tableTotalCarnes.AddCell(cellTotalCarnes2);

        doc.Add(tableTotalCarnes);
        doc.Add(pSaltoLinea);

        PdfPTable tableObservaciones = new PdfPTable(1);
        tableObservaciones.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsObservaciones = new float[] { 100f };
        tableObservaciones.SetWidths(widthsObservaciones);

        PdfPCell cellObservaciones1 = nuevaCelda1("OBSERVACIONES:", "", fuenteLargeBold, Element.ALIGN_LEFT, 1, 0.5f, 1);
        tableObservaciones.AddCell(cellObservaciones1);
        PdfPCell cellObservaciones2 = nuevaCelda1(datos.ActaCabecera.Observacion, "", fuenteLarge, Element.ALIGN_LEFT, 1, 0.5f, 1);
        tableObservaciones.AddCell(cellObservaciones2);

        PdfPTable tableSolicitante = new PdfPTable(2);
        tableSolicitante.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsSolicitante = new float[] { 50f, 25f };
        tableSolicitante.SetWidths(widthsSolicitante);

        tableSolicitante.AddCell("");
        Paragraph linea = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_BOTTOM, 1)));
        tableSolicitante.AddCell(linea);

        tableSolicitante.AddCell("");
        PdfPCell cellSolicitante1 = nuevaCelda1(datos.ActaCabecera.ConSolicitante, "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0, 1);
        tableSolicitante.AddCell(cellSolicitante1);
        tableSolicitante.AddCell("");
        PdfPCell cellSolicitante2 = nuevaCelda1(datos.ActaCabecera.ConTipoDocIdent + ": " + datos.ActaCabecera.ConNumeroDocIdent, "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0, 1);
        tableSolicitante.AddCell(cellSolicitante2);

        doc.Add(tableObservaciones);
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tableSolicitante);

        #endregion
        // ------------------------------------------------------------------------------------------------
        HTMLWorker worker = new HTMLWorker(doc);
        worker.Parse(new StringReader(html));
        doc.Close();
        pdfw.Close();
        byte[] pdfByte = ms.ToArray();
        return (pdfByte);
    }

    public byte[] crearActaRecepcion(beActaRecepcionPrincipal datos, string userComp)
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();
        Document doc = new Document(PageSize.A4);
        doc.SetMargins(10, 10, 30, 30);
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
        iTextSharp.text.Font fuenteLarge = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteLargeBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
        #endregion
        #region Cabecera
        PdfPTable tableCab = new PdfPTable(2);
        tableCab.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cell1 = nuevaCelda("Impreso por:  ", userComp, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
        tableCab.AddCell(cell1);
        string fechaHora = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString("D2").ToString() + ":" + DateTime.Now.Minute.ToString("D2").ToString() + ":" + DateTime.Now.Second.ToString("D2").ToString();
        PdfPCell cell3 = nuevaCelda("Fecha / Hora Impresión:  ", fechaHora, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
        tableCab.AddCell(cell3);
        #endregion
        #region Titulo
        PdfPTable tableTitulo = new PdfPTable(1);
        tableTitulo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        PdfPCell cellTitulo1 = nuevaCelda("", "CARGO DE RECEPCIÓN", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
        tableTitulo.AddCell(cellTitulo1);
        #endregion
        #region Cuerpo1
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tableCab);
        doc.Add(pSaltoLinea);
        doc.Add(tableTitulo);
        doc.Add(pSaltoLinea);

        PdfPTable tableCuerpo1 = new PdfPTable(3);
        tableCuerpo1.HeaderRows = 1;
        tableCuerpo1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widths1 = new float[] { 2.5f, 35f, 15f };
        tableCuerpo1.SetWidths(widths1);

        PdfPCell Cabecera2 = nuevaCelda1("", "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0.5f, 1);
        tableCuerpo1.AddCell(Cabecera2);
        PdfPCell Cabecera3 = nuevaCelda1("TITULAR DEL CARNÉ", "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0.5f, 1);
        tableCuerpo1.AddCell(Cabecera3);
        PdfPCell Cabecera4 = nuevaCelda1("NÚMERO DE EXPEDIENTE", "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0.5f, 1);
        tableCuerpo1.AddCell(Cabecera4);

        for (int x = 0; x <= datos.ActaDetalle.Count - 1; x++)
        {
            PdfPCell Contenido1 = nuevaCelda1((x + 1).ToString(), "", fuenteLarge, Element.ALIGN_CENTER, 1, 0.5f, 1);
            tableCuerpo1.AddCell(Contenido1);
            PdfPCell Contenido2 = nuevaCelda1(datos.ActaDetalle[x].ConTitular.ToUpper(), "", fuenteLarge, Element.ALIGN_LEFT, 1, 0.5f, 1);
            tableCuerpo1.AddCell(Contenido2);
            PdfPCell Contenido3 = nuevaCelda1(datos.ActaDetalle[x].ConNumeroIdent, "", fuenteLarge, Element.ALIGN_LEFT, 1, 0.5f, 1);
            tableCuerpo1.AddCell(Contenido3);
        }

        doc.Add(tableCuerpo1);
        doc.Add(pSaltoLinea);

        PdfPTable tableTotalCarnes = new PdfPTable(3);
        tableTotalCarnes.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsTotalCarnes = new float[] { 1f, 20f, 25f };
        tableTotalCarnes.SetWidths(widthsTotalCarnes);

        PdfPCell cellTotalCarnes1 = nuevaCelda1("Total de Solicitudes: " + datos.ActaDetalle.Count.ToString(), "", fuenteLargeBold, Element.ALIGN_LEFT, 1, 0, 3);
        tableTotalCarnes.AddCell(cellTotalCarnes1);

        doc.Add(tableTotalCarnes);
        doc.Add(pSaltoLinea);

        PdfPTable tableObservaciones = new PdfPTable(1);
        tableObservaciones.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsObservaciones = new float[] { 100f };
        tableObservaciones.SetWidths(widthsObservaciones);

        PdfPCell cellObservaciones1 = nuevaCelda1("OBSERVACIONES:", "", fuenteLargeBold, Element.ALIGN_LEFT, 1, 0.5f, 1);
        tableObservaciones.AddCell(cellObservaciones1);
        PdfPCell cellObservaciones2 = nuevaCelda1(datos.ActaCabecera.Observacion, "", fuenteLarge, Element.ALIGN_LEFT, 1, 0.5f, 1);
        tableObservaciones.AddCell(cellObservaciones2);

        PdfPTable tableSolicitante = new PdfPTable(2);
        tableSolicitante.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        float[] widthsSolicitante = new float[] { 50f, 25f };
        tableSolicitante.SetWidths(widthsSolicitante);

        tableSolicitante.AddCell("");
        Paragraph linea = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_BOTTOM, 1)));
        tableSolicitante.AddCell(linea);

        tableSolicitante.AddCell("");
        PdfPCell cellSolicitante1 = nuevaCelda1(datos.ActaCabecera.ConSolicitante, "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0, 1);
        tableSolicitante.AddCell(cellSolicitante1);
        tableSolicitante.AddCell("");
        PdfPCell cellSolicitante2 = nuevaCelda1(datos.ActaCabecera.ConTipoDocIdent + ": " + datos.ActaCabecera.ConNumeroDocIdent, "", fuenteLargeBold, Element.ALIGN_CENTER, 1, 0, 1);
        tableSolicitante.AddCell(cellSolicitante2);

        doc.Add(tableObservaciones);
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(pSaltoLinea);
        doc.Add(tableSolicitante);

        #endregion
        // ------------------------------------------------------------------------------------------------
        HTMLWorker worker = new HTMLWorker(doc);
        worker.Parse(new StringReader(html));
        doc.Close();
        pdfw.Close();
        byte[] pdfByte = ms.ToArray();
        return (pdfByte);
    }

    //public byte[] crearReporte(string userComp)
    //{
    //    StringWriter sw = new StringWriter();
    //    string html = sw.ToString();
    //    Document doc = new Document(PageSize.A4.Rotate());
    //    doc.SetMargins(5, 5, 5, 5);
    //    MemoryStream ms = new MemoryStream();
    //    PdfWriter pdfw = PdfWriter.GetInstance(doc, ms);
    //    doc.Open();
    //    // CREACION ---------------------------------------------------------------------------------------
    //    Chunk saltoLinea = new Chunk("\n");
    //    Paragraph pSaltoLinea = new Paragraph();
    //    pSaltoLinea.Alignment = Element.ALIGN_LEFT;
    //    pSaltoLinea.Add(saltoLinea);
    //    #region Fuentes
    //    FontFactory.RegisterDirectories();
    //    iTextSharp.text.Font fuenteSmall = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 6, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteSmallBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteMedium = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 7, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteMediumBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteLarge = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteLargeBold = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font(FontFactory.GetFont("Consolas", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //    #endregion
    //    #region Cabecera
    //    PdfPTable tableCab = new PdfPTable(2);
    //    tableCab.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    PdfPCell cell1 = nuevaCelda("Impreso por:  ", userComp, fuenteSmall, Element.ALIGN_LEFT, 1, 0);
    //    tableCab.AddCell(cell1);
    //    string fechaHora = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString("D2").ToString() + ":" + DateTime.Now.Minute.ToString("D2").ToString() + ":" + DateTime.Now.Second.ToString("D2").ToString();
    //    PdfPCell cell3 = nuevaCelda("Fecha / Hora Impresión:  ", fechaHora, fuenteSmall, Element.ALIGN_RIGHT, 1, 0);
    //    tableCab.AddCell(cell3);
    //    #endregion
    //    #region Titulo
    //    PdfPTable tableTitulo = new PdfPTable(1);
    //    tableTitulo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

    //    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(@"D:\NUNJA\05_CARDIP\SolCARDIP\SolCARDIP\Imagenes\Logos\rree_membrete.gif");
    //    img.ScaleAbsolute(90f, 50f);

    //    //iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(@"D:\NUNJA\05_CARDIP\SolCARDIP\SolCARDIP\Imagenes\Logos\rree_membrete.gif");
    //    PdfPCell cell = new PdfPCell(img);
    //    PdfPCell cell = nuevaCelda("", "CARGO DE RECEPCIÓN", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
    //    tableTitulo.AddCell(cell);

        
    //    //PdfPCell cellTitulo1 = nuevaCelda("", "CARGO DE RECEPCIÓN", fuenteTitulo, Element.ALIGN_CENTER, 1, 0);
    //    //tableTitulo.AddCell(cellTitulo1);
        
        
    //    #endregion
        

    //    doc.Add(tableTitulo);
    //    // ------------------------------------------------------------------------------------------------
    //    HTMLWorker worker = new HTMLWorker(doc);
    //    worker.Parse(new StringReader(html));
    //    doc.Close();
    //    pdfw.Close();
    //    byte[] pdfByte = ms.ToArray();
    //    return (pdfByte);
    //}

    protected PdfPCell nuevaCeldaImagen(iTextSharp.text.Image imagenJpg, int alineacion, int colspan, float border)
    {
        PdfPCell cell = new PdfPCell(imagenJpg);
        imagenJpg.ScaleAbsolute(113, 183);
        //imagenJpg.WidthPercentage = 100f;
        cell.BorderWidth = border;
        cell.Colspan = colspan;
        cell.HorizontalAlignment = alineacion;
        return (cell);
    }

    protected PdfPCell nuevaCeldaImagenFirma(iTextSharp.text.Image imagenJpg, int alineacion, int colspan, float border)
    {
        PdfPCell cell = new PdfPCell(imagenJpg);
        imagenJpg.ScaleAbsolute(113, 50);
        //imagenJpg.WidthPercentage = 100f;
        cell.BorderWidth = border;
        cell.Colspan = colspan;
        cell.HorizontalAlignment = alineacion;
        return (cell);
    }

    protected PdfPCell nuevaCelda(string definicion, string dato, iTextSharp.text.Font fuente, int alineacion, int colspan, float border)
    {
        PdfPCell cell = new PdfPCell(new Phrase(definicion + dato, fuente));
        cell.BorderWidth = border;
        cell.Colspan = colspan;
        cell.HorizontalAlignment = alineacion;
        return (cell);
    }

    protected PdfPCell nuevaCelda1(string definicion, string dato, iTextSharp.text.Font fuente, int alineacion, int colspan, float border, int vColspan)
    {
        PdfPCell cell = new PdfPCell(new Phrase(definicion + dato, fuente));
        cell.BorderWidth = border;
        cell.Colspan = colspan;
        cell.HorizontalAlignment = alineacion;
        cell.Colspan = vColspan;
        return (cell);
    }

    // CONTROL CALIDAD
    public string[] obtenerControlCalidad(string controlCalidad)
    {
        string[] listaControl = new string[15];
        try
        {
            int contador = 0;
            foreach (char valor in controlCalidad)
            {
                string valorConv = valor.ToString();
                if (!valorConv.Equals("|")) { listaControl[contador] = valorConv; contador++; }
            }
            return (listaControl);
        }
        catch(Exception ex)
        {
            listaControl = null;
            obrGeneral.grabarLog(ex);
        }
        return (listaControl);
    }

    public string[] obtenerControlCalidadDetalle(string controlCalidadDetalle)
    {
        string[] listaControlDetalle = new string[16];
        try
        {
            int contador = 0;
            string cadena = "";
            foreach (char valor in controlCalidadDetalle)
            {
                string valorConv = valor.ToString();
                if (!valorConv.Equals("|")) { cadena = cadena + valorConv; }
                else { listaControlDetalle[contador] = cadena; cadena = ""; contador++; }
            }
            listaControlDetalle[contador] = cadena;
            return (listaControlDetalle);
        }
        catch (Exception ex)
        {
            listaControlDetalle = null;
            obrGeneral.grabarLog(ex);
        }
        return (listaControlDetalle);
    }

    // REFLECTION

    public DataTable obtenerValoresAtributos(List<object> listaObjetos)
    {
        int _contador = 0;
        object[] arrSStr;
        DataTable dt = new DataTable();
        foreach (object objeto1 in listaObjetos)
        {
            arrSStr = new object[dt.Columns.Count];
            _contador = 0;
            foreach (var prop in objeto1.GetType().GetProperties())
            {
                //string nombre = prop.Name;
                object valorCampo = prop.GetValue(objeto1, null);
                arrSStr[_contador] = valorCampo;
                _contador = _contador + 1;
            }
            dt.Rows.Add(arrSStr);
        }
        return (dt);
    }

    public byte[] GenerarPDF(beRegistroLinea obeRegistroLinea, string nroCarnet = "--------", string tipoCargo = "",
        string nombreTitular = "", string apePatTitular = "", string apeMatTitular = "", string calidadMigratoriaTitular = "")
    {
        StringWriter sw = new StringWriter();
        string html = sw.ToString();
        iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
        doc.SetMargins(50, 50, 50, 50);
        MemoryStream ms = new MemoryStream();
        PdfWriter pdfw = PdfWriter.GetInstance(doc, ms);

        doc.Open();
        WriteImagen(doc, "../../Imagenes/Reporte.jpg", 5, 50, true, 960, 750);

        int Fila1_Y = 602;
        int Fila2_Y = 565;
        int Fila3_Y = 530;
        int Fila4_Y = 495;
        int Fila5_Y = 463;
        int Fila6_Y = 407;
        int Fila7_Y = 373;
        int Fila8_Y = 320;
        int Fila9_Y = 264;
        int Fila10_Y = 230;
        int Fila11_Y = 185;
        int Fila12_Y = 175;
        int Fila13_Y = 165;
        int Fila14_Y = 155;

        int Columna1_X = 46;
        int Columna2_X = 225;
        int Columna3_X = 407;

        short tamanoLetra = 9;

        EscribirTexto(pdfw, obeRegistroLinea.ConTipoEmision.ToUpper(), 53, 680, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.NumeroRegLinea.ToUpper(), Convert.ToInt32(doc.PageSize.Width) - 100, Convert.ToInt32(doc.PageSize.Height) - 73, tamanoLetra);
        EscribirTexto(pdfw, nroCarnet.ToUpper(), 175, 660, tamanoLetra);

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
        EscribirTexto(pdfw, obeRegistroLinea.InPersonaContacto.ToUpper(), Columna2_X, Fila9_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.InCargoContacto.ToUpper(), Columna3_X, Fila9_Y, tamanoLetra);

        EscribirTexto(pdfw, obeRegistroLinea.InCorreoElectronico.ToUpper(), Columna1_X, Fila10_Y, tamanoLetra);
        EscribirTexto(pdfw, obeRegistroLinea.InNumeroTelefono.ToUpper(), Columna3_X, Fila10_Y, tamanoLetra);

        if (obeRegistroLinea.DpRutaAdjunto.Length > 0)
        {
            EscribirTexto(pdfw, "Fotografía: SI", Columna1_X, Fila11_Y, tamanoLetra);
        }
        else
        {
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
            EscribirTexto(pdfw, "Denuncia Policial: NO", Columna1_X, Fila14_Y, tamanoLetra);
        }


        iTextSharp.text.html.simpleparser.HTMLWorker worker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc);
        worker.Parse(new StringReader(html));
        doc.Close();
        pdfw.Close();

        byte[] pdfByte = ms.ToArray();

        return pdfByte;
    }
    private void EscribirTexto(iTextSharp.text.pdf.PdfWriter pdfw, string Texto, int ubicacionX = 0, int UbicacionY = 0, short tamanoLetra = 7)
    {
        PdfContentByte cb = pdfw.DirectContent;
        cb.BeginText();
        BaseFont f_cn = BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, false);
        cb.SetFontAndSize(f_cn, tamanoLetra);
        cb.SetTextMatrix(ubicacionX, UbicacionY); //(xPos, yPos) 

        cb.ShowText(Texto);
        cb.EndText();
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
}

