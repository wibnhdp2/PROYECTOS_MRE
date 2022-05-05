using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using AjaxControlToolkit;
using System.Configuration;
using System.Globalization;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlDate : System.Web.UI.UserControl
    {
        public delegate void TextChangedDelegate(object sender, EventArgs e);

        protected string uniqueKey;

        private short TabIndex
        {
            get { return TxtFecha.TabIndex; }
            set { TxtFecha.TabIndex = value; }
        }

        public bool EnabledText
        {
            set
            {
                this.TxtFecha.Enabled = value;
                this.ceFecha.EnableViewState = value;
                this.ceFecha.Enabled = value;
                this.btnFecha.Enabled = value;
                if (value)
                {
                    this.ceFecha.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    
                }
                else
                {
                    this.ceFecha.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
                }
            }
            get { return this.TxtFecha.Enabled; }
        }
        
        public bool EnabledIcon
        {
            set { this.btnFecha.Enabled = value; }
            get { return this.btnFecha.Enabled; }
        }

        public string Text
        {
            set
            {
                this.TxtFecha.Text = value;
            }
            get
            {
                return this.TxtFecha.Text;
            }
        }
        public DateTime set_Value
        {
            set
            {
                var s_fecha = IsFecha(value);

                this.TxtFecha.Text = s_fecha.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
            }
        }

        public DateTime Value()
        {
            DateTime d_fecha;
            try
            {
                d_fecha = Comun.FormatearFecha(TxtFecha.Text.Replace(".",""));
            }
            catch
            {
                d_fecha = DateTime.MinValue;
            }
            return d_fecha;
        }
        private DateTime IsFecha(object s_fecha)
        {
            DateTime d_fecha;
            try
            {
                d_fecha = Convert.ToDateTime(s_fecha, CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            }
            catch
            {
                d_fecha = DateTime.MinValue;
                Accesorios.Comun.EjecutarScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, "alert('Formato de fecha inválido');");
                this.TxtFecha.Text = "";
                this.TxtFecha.Focus();
            }
            return d_fecha;
        }

        public bool ToDateTime()
        {
            bool d_fecha;
            try
            {
                Convert.ToDateTime(Comun.FormatearFecha(this.TxtFecha.Text));
                d_fecha = true;
                lblErrorDate.Text = "";
            }
            catch
            {
                lblErrorDate.Text = "Error.!";
                this.TxtFecha.Focus();
                d_fecha = false;
            }
            return d_fecha;
        }

        public String CssClass
        {
            set { this.TxtFecha.CssClass = value; }
            get { return this.TxtFecha.CssClass; }
        }

        public void SetAtributtes(String evento, String funcion)
        {
            this.TxtFecha.Attributes.Add(evento, funcion);
        }

        public Boolean Enabled
        {
            set
            {
                this.btnFecha.Style["cursor"] = value ? "pointer" : "";
                this.ceFecha.PopupButtonID = (value ? "btnFecha" : "");
                this.btnFecha.Enabled = value; 
                this.TxtFecha.Enabled = value;

                if (value)
                {
                    this.ceFecha.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

                }
                else
                {
                    this.ceFecha.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
                }

            }
            get { return this.TxtFecha.Enabled; }
        }

        private DateTime _dFechaIni = Convert.ToDateTime("01/01/1900");
        public DateTime StartDate
        {
            get { return _dFechaIni; }
            set { _dFechaIni = value; }
        }

        private DateTime _dFechaFin = DateTime.Today;
        public DateTime EndDate
        {
            get { return _dFechaFin; }
            set { _dFechaFin = value; }
        }

        public bool AutoPostBack
        {
            set { this.TxtFecha.AutoPostBack = value; }
            get { return this.TxtFecha.AutoPostBack; }
        }

        private bool _bAllowFutureDate = false;
        public bool AllowFutureDate
        {
            set { _bAllowFutureDate = value; }
            get { return _bAllowFutureDate; }
        }

        private DateTime _Now = DateTime.UtcNow;
        public DateTime Now
        {
            set { _Now = ObtenerFechaActualConsulado(); }
            get { return _Now; }
        }

        private DateTime ObtenerFechaActualConsulado()
        {
            DateTime datFecha = DateTime.UtcNow;
            double dblDiferenciaHoraria = 0;
            double dblHorasConsiderar = 0;
            double dblHorarioVerano = 0;

            //--------------------------------------------------------
            //Fecha: 09/12/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar la sesion cuando es nula.
            //--------------------------------------------------------
            if (Session[SGAC.Accesorios.Constantes.CONST_SESION_DIFERENCIA_HORARIA] != null)
            {
                dblDiferenciaHoraria = Convert.ToDouble(Session[SGAC.Accesorios.Constantes.CONST_SESION_DIFERENCIA_HORARIA].ToString());
            }
            if (Session[SGAC.Accesorios.Constantes.CONST_SESION_HORARIO_VERANO] != null)
            {
                dblHorarioVerano = Convert.ToDouble(Session[SGAC.Accesorios.Constantes.CONST_SESION_HORARIO_VERANO].ToString());
            }
            //---------------------------------------------------
            // Fecha: 03/12/2019
            // Autor: Miguel Márquez Beltrán
            // Motivo: Se asignan los valores desde la sessión.
            //---------------------------------------------------

            //DataTable dtOficinaConsular = new DataTable();

            //dtOficinaConsular = Comun.ObtenerOficinaConsularPorId(Session);
            //if (dtOficinaConsular != null)
            //{
            //    if (dtOficinaConsular.Rows.Count > 0)
            //    {
            //        dblDiferenciaHoraria = Convert.ToDouble(dtOficinaConsular.Rows[0]["ofco_sDiferenciaHoraria"].ToString());
            //        dblHorarioVerano = Convert.ToDouble(dtOficinaConsular.Rows[0]["ofco_sHorarioVerano"].ToString());

            //    }
            //}
                        
            dblHorasConsiderar = Convert.ToDouble(dblDiferenciaHoraria + dblHorarioVerano);

            datFecha = datFecha.AddHours(dblHorasConsiderar);
            return datFecha;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState[(sender as Control).UniqueID] == null)
            {
                ViewState[(sender as Control).UniqueID] = (sender as Control).UniqueID;
            }

            this.uniqueKey = ViewState[(sender as Control).UniqueID].ToString();

            TxtFecha.Attributes["onBlur"] = "return validaFechaMMMddyyyy_" + uniqueKey + "(this);";
            TxtFecha.Attributes["onkeypress"] = "return Validar_Fecha_" + uniqueKey + "(event,this);"; 

            this.ceFecha.StartDate = _dFechaIni;
             
            if (AllowFutureDate)
            {
                this.ceFecha.EndDate = ObtenerFechaActualConsulado().AddYears(20);
            }
            else
            {
                this.ceFecha.EndDate = ObtenerFechaActualConsulado();
            }

            this.ceFecha.EnableViewState = true;

            if (!this.TxtFecha.Text.Trim().Equals(""))
            {
                ToDateTime();
            }

            if (!Page.IsPostBack)
            {
                bool bStatus = this.TxtFecha.Enabled;
                this.btnFecha.Style["cursor"] = bStatus ? "pointer" : "";
                this.TxtFecha.Enabled = bStatus;
                this.ceFecha.Enabled = bStatus;
            }

            if (Ejecutar_Scrip)
            {

                ceFecha.OnClientDateSelectionChanged = "Ejecuar_Script";
                EnabledText = true;
            }

            if (Ejecutar_Scrip_Pago)
            {
                ceFecha.OnClientDateSelectionChanged = "Ejecuar_Script_Pago";
                EnabledText = true;
            }
        }


        public bool Ejecutar_Scrip { get; set; }

        public bool Ejecutar_Scrip_Pago { get; set; }

        public CalendarExtender Calendario
        {
            get { return ceFecha; }
            set { ceFecha = value; }
        }
    }
}