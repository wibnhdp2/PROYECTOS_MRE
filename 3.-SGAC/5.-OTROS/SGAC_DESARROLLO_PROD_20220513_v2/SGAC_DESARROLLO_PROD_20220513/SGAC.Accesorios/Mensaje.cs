using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.Accesorios
{
    public class Mensaje
    {
        #region Mensajes

        public static string MostrarMensaje(Enumerador.enmTipoMensaje iType, string strTitle, string strMessage)
        {
            char tipo = 'i';
            switch (iType)
            {
                case Enumerador.enmTipoMensaje.NONE:
                    break;
                case Enumerador.enmTipoMensaje.INFORMATION:
                    tipo = 'i';
                    break;
                case Enumerador.enmTipoMensaje.QUESTION:
                    tipo = 'c';
                    break;
                case Enumerador.enmTipoMensaje.WARNING:
                    tipo = 'a';
                    break;
                case Enumerador.enmTipoMensaje.ERROR:
                    tipo = 'e';
                    break;
                default:
                    break;
            }

            string strScript = "showpopupother(" +
                "'" + tipo + "'," +
                "'" + strTitle + "'," +
                "'" + strMessage + "'";
            strScript += ", false";
            strScript += ", 200";
            strScript += ", 300";
            strScript += ");";

            return strScript;
        }
        public static string MostrarMensaje(Enumerador.enmTipoMensaje iType, string strTitle, string strMessage, bool bResizable, int height, int width)
        {
            char tipo = 'i';
            switch (iType)
            {
                case Enumerador.enmTipoMensaje.NONE:
                    break;
                case Enumerador.enmTipoMensaje.INFORMATION:
                    tipo = 'i';
                    break;
                case Enumerador.enmTipoMensaje.QUESTION:
                    tipo = 'c';
                    break;
                case Enumerador.enmTipoMensaje.WARNING:
                    tipo = 'a';
                    break;
                case Enumerador.enmTipoMensaje.ERROR:
                    tipo = 'e';
                    break;
                default:
                    break;
            }

            // El mensaje... no puede tener comilla simple... si hubiera sera reemplazo. En caso tuviera no mostrará el mensae
            strMessage = strMessage.Replace("'", "|");

            string strScript = "showpopupother(" +
                "'" + tipo + "'," +
                "'" + strTitle + "'," +
                "'" + strMessage + "'";
            strScript += ", " + bResizable.ToString().ToLower();
            strScript += ", " + height;
            strScript += ", " + width;
            strScript += ");";
            return strScript;
        }

        #endregion

        #region Notificaciones
        public static string MostrarNotificacion(Enumerador.enmTipoNotificacion iType, string strTitle, string strMessage, bool bCloseButton)
        {
            string strScript = @"$(function(){{";
            if (bCloseButton)
            {
                strScript += "toastr.options.closeButton = true;";
            }
            // Por defecto: toast-
            //script += "toastr.options.positionClass = 'toast-center-center';";
            switch (iType)
            {
                case Enumerador.enmTipoNotificacion.INFO:
                    strScript += "toastr.info('";
                    break;
                case Enumerador.enmTipoNotificacion.WARNING:
                    strScript += "toastr.warning('";
                    break;
                case Enumerador.enmTipoNotificacion.ERROR:
                    strScript += "toastr.error('";
                    break;
                case Enumerador.enmTipoNotificacion.SUCCESS:
                    strScript += "toastr.success('";
                    break;
                default:
                    break;
            }
            strScript += strMessage + "','" + strTitle + "');";
            strScript += "}});";
            return strScript;
        }
        public static string EnviarNotificacion(string strMessage, string strUserReceive, string strUserSend)
        {
            string strScript = "";
            strScript += "notificar('" + strMessage + "','" + strUserReceive + "','" + strUserSend + "');";
            return strScript;
        }
        #endregion

    }
}
