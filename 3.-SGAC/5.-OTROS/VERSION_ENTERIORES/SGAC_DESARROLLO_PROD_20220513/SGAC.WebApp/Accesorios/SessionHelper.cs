using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SGAC.WebApp.Accesorios
{
    public class SessionHelper
    {
        #region Constantes de variables de sesión

        private const string constParticipantesNotificados = "ParticipantesNotificados";
        private const string constParticipantesNotificadosEliminados = "ParticipantesNotificadosEliminados";
        private const string constPagoActuaciones = "PagoActuaciones";

        #endregion

        #region Métodos Generales Lectura Escritura

        public static T Lee<T>(string variable)
        {

            object valor = HttpContext.Current.Session[variable];

            if (valor == null)

                return default(T);

            else

                return ((T)valor);

        }

        public static void Escribe(string variable, object valor)
        {

            HttpContext.Current.Session[variable] = valor;

        }

        #endregion

        #region Variables de sesión

        public static DataTable dtParticipantesNotificados
        {
            get
            {
                return Lee<DataTable>(constParticipantesNotificados);
            }
            set
            {
                Escribe(constParticipantesNotificados, value);
            }

        }

        public static DataTable dtParticipantesNotificadosEliminados
        {
            get
            {
                return Lee<DataTable>(constParticipantesNotificadosEliminados);
            }
            set
            {


                Escribe(constParticipantesNotificadosEliminados, value);
            }

        }

        public static DataTable dtPagoActuaciones
        {
            get
            {
                return Lee<DataTable>(constPagoActuaciones);
            }
            set
            {

                Escribe(constPagoActuaciones, value);
            }

        }

        #endregion
    }
}