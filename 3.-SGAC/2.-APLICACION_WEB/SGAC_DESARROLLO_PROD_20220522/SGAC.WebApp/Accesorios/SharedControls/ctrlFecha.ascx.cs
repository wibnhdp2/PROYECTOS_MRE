using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

namespace SGAC.WebApp
{
    public partial class Fecha : System.Web.UI.UserControl
    {
        public delegate void TextChangedDelegate(object sender, EventArgs e);

        private DateTime _dFechaIni = Convert.ToDateTime("01/01/1900");
        public DateTime dFechaIni
        {
            get { return _dFechaIni; }
            set { _dFechaIni = value; }
        }

        private DateTime _dFechaFin = DateTime.Today;
        public DateTime dFechaFin
        {
            get { return _dFechaFin; }
            set { _dFechaFin = value; }
        }

        public String Text
        {
            set { this.TxtFecha.Text = value; }
            get { return this.TxtFecha.Text; }
        }

        public DateTime Value
        {
            set { this.TxtFecha.Text = value.ToString("dd/MM/yyyyy"); }
            get { return (this.TxtFecha.Text != "") ? Convert.ToDateTime(this.TxtFecha.Text) : DateTime.Today; }
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
                this.TxtFecha.Enabled = value;                
            }
            get { return this.TxtFecha.Enabled; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ceFecha.StartDate = _dFechaIni;

            if (AllowFutureDate)
                this.ceFecha.EndDate = _dFechaFin.AddYears(1);                
            else
                this.ceFecha.EndDate = _dFechaFin;

            this.ceFecha.EnableViewState = true;

            if (!Page.IsPostBack)
            {
                bool bStatus = this.TxtFecha.Enabled;
                this.btnFecha.Style["cursor"] = bStatus ? "pointer" : "";
                this.TxtFecha.Enabled = bStatus;
                this.ceFecha.Enabled = bStatus;
            }
        }

        public bool IsValid()
        {
            DateTime ddate = this.Value;
            return !(ddate < _dFechaIni || ddate > _dFechaFin);
        }

        public CalendarExtender Calendario
        {
            get { return ceFecha; }
            set { ceFecha = value; }
        }

        public MaskedEditExtender EdicionCalendario
        {
            get { return meFecha; }
            set { meFecha = value; }
        }
    }
}