using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SGAC.Accesorios
{
    public class Correo
    {
        #region Datos
        private static string pstrHost = "";
        private static int pintPort = 0;
        //private static bool pbEnabledSSL = true;
        #endregion

        public static bool EnviarCorreo(
                              string pstrEmailFrom
                            , string pstrEmailPassword
                            , string pstrEmailTo
                            , string pstrEmailCC
                            , string pstrEmailCCo
                            , string pstrSubject
                            , string pstrMessageBody
                            , bool pbEnabledSSL
                            , bool pbIsBodyHtml
                            , MailPriority pstrPriority
                            , List<string> plstAttachments)
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
                    if (strPara != "") oMail.To.Add(strPara.Trim());

                if (oMail.To == null)
                    throw new Exception("No hay Destinatario");
                if (oMail.To.Count < 1)
                    throw new Exception("No hay Destinatario");

                //
                if (pstrEmailCC != null)
                {
                    string[] ArrayCopiaA = pstrEmailCC.Split(';');
                    foreach (string strCopiarA in ArrayCopiaA)
                        if (strCopiarA != "") oMail.CC.Add(strCopiarA.Trim());
                }

                if (pstrEmailCCo != null)
                {
                    string[] ArrayCopiaOculta = pstrEmailCCo.Split(';');
                    foreach (string strCopiarOculta in ArrayCopiaOculta)
                        if (strCopiarOculta != "") oMail.Bcc.Add(strCopiarOculta.Trim());
                }
                //

                oMail.Subject = pstrSubject;
                oMail.IsBodyHtml = pbIsBodyHtml;
                oMail.Body = pstrMessageBody;
                oMail.Priority = pstrPriority;

                if (plstAttachments != null)
                {
                    foreach (var item in plstAttachments)
                    {
                        oMail.Attachments.Add(new Attachment(item));
                    }
                }

                oSmtp.Host = pstrHost; // Global
                oSmtp.Port = pintPort; // Global
                oSmtp.EnableSsl = pbEnabledSSL; // Global
                oSmtp.Credentials = new System.Net.NetworkCredential(pstrEmailFrom, pstrEmailPassword);
                oSmtp.Send(oMail);

                enviado = true;
            }
            catch (SGACExcepcion ex)
            {
                if (ex.Message.Contains("e-mail address"))
                {

                }
                throw ex;
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

        public static bool EnviarCorreo(
                              string pstrEmailFrom
                            , string pstrEmailPassword
                            , List<string> pstrEmailTo
                            , List<string> pstrEmailCC
                            , List<string> pstrEmailCCo
                            , string pstrSubject
                            , string pstrMessageBody
                            , bool pbEnabledSSL
                            , bool pbIsBodyHtml
                            , MailPriority pstrPriority
                            , List<string> plstAttachments)
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

                foreach (string strPara in pstrEmailTo)
                    if (strPara != string.Empty) oMail.To.Add(strPara.Trim());

                if (oMail.To == null)
                    throw new Exception("No hay Destinatario");
                if (oMail.To.Count < 1)
                    throw new Exception("No hay Destinatario");

                //
                if (pstrEmailCC != null)
                {
                    foreach (string strCopiarA in pstrEmailCC)
                        if (strCopiarA != "") oMail.CC.Add(strCopiarA.Trim());
                }

                if (pstrEmailCCo != null)
                {
                    foreach (string strCopiarOculta in pstrEmailCCo)
                        if (strCopiarOculta != "") oMail.Bcc.Add(strCopiarOculta.Trim());
                }
                //

                oMail.Subject = pstrSubject;
                oMail.IsBodyHtml = pbIsBodyHtml;
                oMail.Body = pstrMessageBody;

                oMail.Priority = pstrPriority;

                if (plstAttachments != null)
                {
                    foreach (var item in plstAttachments)
                    {
                        oMail.Attachments.Add(new Attachment(item));
                    }
                }

                oSmtp.Host = pstrHost; // Global
                oSmtp.Port = pintPort; // Global
                oSmtp.EnableSsl = pbEnabledSSL; // Global
                oSmtp.Credentials = new System.Net.NetworkCredential(pstrEmailFrom, pstrEmailPassword);
                oSmtp.Send(oMail);

                enviado = true;
            }
            catch (SGACExcepcion ex)
            {
                throw ex;
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



        public static bool EnviarCorreo(
                              string pstrSMTPServer
                            , string pintSMTPPort
                            , string pstrEmailFrom
                            , string pstrEmailPassword
                            , string pstrEmailTo
                            , string pstrEmailCC
                            , string pstrEmailCCo
                            , string pstrSubject
                            , string pstrMessageBody
                            , bool pbEnabledSSL
                            , bool pbIsBodyHtml
                            , MailPriority pstrPriority
                            , List<string> plstAttachments)
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
                 
                if (pstrEmailCC != null)
                {
                    string[] ArrayCopiaA = pstrEmailCC.Split(';');
                    foreach (string strCopiarA in ArrayCopiaA)
                        if (strCopiarA != "") oMail.CC.Add(strCopiarA.Trim());
                }

                if (pstrEmailCCo != null)
                {
                    string[] ArrayCopiaOculta = pstrEmailCCo.Split(';');
                    foreach (string strCopiarOculta in ArrayCopiaOculta)
                        if (strCopiarOculta != "") oMail.Bcc.Add(strCopiarOculta.Trim());
                }

                oMail.Subject = pstrSubject;
                oMail.IsBodyHtml = pbIsBodyHtml;
                oMail.Body = pstrMessageBody;

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
                //oSmtp.EnableSsl = pbEnabledSSL; // Global
                oSmtp.UseDefaultCredentials = true;
                //oSmtp.Credentials = new System.Net.NetworkCredential(pstrEmailFrom, pstrEmailPassword);
                try
                {
                    oSmtp.Send(oMail);
                }
                catch (SmtpException ex)
                {
                    throw new SGACExcepcion(ex.Message);
                }

                enviado = true;
            }
            catch (SGACExcepcion ex)
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

                throw new SGACExcepcion(strMensaje);
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

        public static bool EnviarCorreo(
                              string pstrSMTPServer
                            , int pintSMTPPort
                            , string pstrEmailFrom
                            , string pstrEmailPassword
                            , List<string> pstrEmailTo
                            , List<string> pstrEmailCC
                            , List<string> pstrEmailCCo
                            , string pstrSubject
                            , string pstrMessageBody
                            , bool pbEnabledSSL
                            , bool pbIsBodyHtml
                            , MailPriority pstrPriority
                            , List<string> plstAttachments)
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

                foreach (string strPara in pstrEmailTo)
                    if (strPara != string.Empty) oMail.To.Add(strPara.Trim());

                if (oMail.To == null)
                    throw new Exception("No hay Destinatario");
                if (oMail.To.Count < 1)
                    throw new Exception("No hay Destinatario");

                // 
                if (pstrEmailCC != null)
                {
                    foreach (string strCopiarA in pstrEmailCC)
                        if (strCopiarA != "") oMail.CC.Add(strCopiarA.Trim());
                }

                if (pstrEmailCCo != null)
                {
                    foreach (string strCopiarOculta in pstrEmailCCo)
                        if (strCopiarOculta != "") oMail.Bcc.Add(strCopiarOculta.Trim());
                }
                //

                oMail.Subject = pstrSubject;
                oMail.IsBodyHtml = pbIsBodyHtml;
                oMail.Body = pstrMessageBody;
                oMail.Priority = pstrPriority;

                if (plstAttachments != null)
                {
                    foreach (var item in plstAttachments)
                    {
                        oMail.Attachments.Add(new Attachment(item));
                    }
                }

                oSmtp.Host = pstrHost; // Global
                oSmtp.Port = pintPort; // Global
                oSmtp.EnableSsl = pbEnabledSSL; // Global
                oSmtp.Credentials = new System.Net.NetworkCredential(pstrEmailFrom, pstrEmailPassword);
                oSmtp.Send(oMail);
                enviado = true;
            }
            catch (SGACExcepcion ex)
            {
                throw ex;
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

        public static bool EnviarCorreoPlantillaHTML(
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
                    throw new SGACExcepcion(ex.Message);
                }

                enviado = true;
            }
            catch (SGACExcepcion ex)
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

                throw new SGACExcepcion(strMensaje);
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


    }
}