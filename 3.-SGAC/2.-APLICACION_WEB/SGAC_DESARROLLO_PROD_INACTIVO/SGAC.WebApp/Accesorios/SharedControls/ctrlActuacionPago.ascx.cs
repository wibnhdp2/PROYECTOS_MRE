using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using System.Data;
using SGAC.Configuracion.Sistema.BL;


namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlActuacionPago : System.Web.UI.UserControl
    {

        public delegate void ddlTipoPagoCommandEventHandler(ddlTipoPagoCommandEventArgs e);
        public event ddlTipoPagoCommandEventHandler ddlTipoPagoSelectorChanged;

        public class ddlTipoPagoCommandEventArgs
        {
            public int Id { get; protected set; }

            public ddlTipoPagoCommandEventArgs(int id)
            {
                this.Id = id;
            }
        }

        #region Propiedades

        

        protected string uniqueKey;

        private bool _bValidado = false;
      
        private int _iTipoPago=0;

        public int iTipoPago
        {
            get
            {
                if (ddlTipoPago.SelectedIndex > 0)
                {
                    return Convert.ToInt32(ddlTipoPago.SelectedValue);
                }
                else
                    return 0;
            }
            set 
            { 
                _iTipoPago = value;
                ddlTipoPago.SelectedValue = _iTipoPago.ToString();
            }
        }

        string _vNroVoucher = string.Empty;

        public string vNroVoucher
        {
            get { return txtNroVoucher.Text; }
            set 
            { 
                _vNroVoucher = value;
                txtNroVoucher.Text = _vNroVoucher.ToUpper();
            }
        }


        string _vNroOperacion = string.Empty;

        public string vNroOperacion
        {
            get { return txtNroOperacion.Text; }
            set 
            { 
                _vNroOperacion = value;
                txtNroOperacion.Text = _vNroOperacion.ToUpper();
            }
        }

        int _iBank;

        public int iBank
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlBanco.SelectedValue))
                {
                    return Convert.ToInt32(ddlBanco.SelectedValue);
                }
                else
                    return 0;
            }
            set
            {
                _iBank = value;
                ddlBanco.SelectedValue = _iBank.ToString();
            }
        }

        DateTime _dFechaPago;

        public DateTime DFechaPago
        {
            get { return _dFechaPago; }
            set { _dFechaPago = value; }
        }

        public DateTime dFechaPago
        {
            get {
                return Comun.FormatearFecha(ctrFecPago.Text); 
            }
            set 
            { 
                _dFechaPago = value;
                ctrFecPago.Text = _dFechaPago.Date.ToString("MMM-dd-yyyy");
            }
        }

        string _vMonto="0";

        public string vMonto
        {
            get { return _vMonto; }
            set
            {
                txtMonto.Text = double.Parse(value).ToString(System.Configuration.ConfigurationManager.AppSettings["FormatoMonto"].ToString());
                _vMonto = txtMonto.Text;
            }
        }

        private float _fLabelWidth;

        public float fLabelsWidth
        {
            get { return _fLabelWidth; }
            set
            {
                _fLabelWidth = value;

                lblTipoPago.Width = (Unit)fLabelsWidth;
                lblNroVoucher.Width = (Unit)fLabelsWidth;
                lblNroOperacion.Width = (Unit)fLabelsWidth;
                lblBanco.Width = (Unit)fLabelsWidth;
                lblFechaPago.Width = (Unit)fLabelsWidth;
                lblMonto.Width = (Unit)fLabelsWidth;



            }
        }

        private float _fControlsWidth;

        public float fControlsWidth
        {
            get { return _fControlsWidth; }
            set {

                _fControlsWidth = value;

                ddlTipoPago.Width = (Unit)_fControlsWidth;
                txtNroVoucher.Width = (Unit)_fControlsWidth;
                txtNroOperacion.Width = (Unit)_fControlsWidth;
                ddlBanco.Width = (Unit)_fControlsWidth;
                //FechaPago.Width = (Unit)fLabelsWidth;
                //lblMonto.Width = (Unit)fLabelsWidth;
            }
        }

        private bool _bEnabledControl;

        public bool bEnabledControl
        {
            get { return _bEnabledControl; }
            set
            {
                _bEnabledControl = value;

                ddlTipoPago.Enabled = _bEnabledControl;
                txtNroVoucher.Enabled = _bEnabledControl;
                txtNroOperacion.Enabled = _bEnabledControl;
                ddlBanco.Enabled = _bEnabledControl;
                //txtMonto.Enabled = _bEnabledControl;
                (ctrFecPago.FindControl("txtFecha") as TextBox).Enabled = _bEnabledControl;
                (ctrFecPago.FindControl("btnFecha") as ImageButton).Enabled = _bEnabledControl;
            }
        }


        string _vTarifaLetra = string.Empty;

        public string vTarifaLetra
        {
            get { return _vTarifaLetra; }
            set {
                _vTarifaLetra = value;
                lbltarifa.Text = _vTarifaLetra;
                CargarTipoPagoNormaTarifario();
            }
        }

        long _iExoneracionId = 0;

        public long iExoneracionId
        {
            get {
                if (ddlExoneracion.SelectedIndex > 0)
                {
                    return Convert.ToInt64(ddlExoneracion.SelectedValue);
                }
                
                return 0; 
            }
            set
            {
                _iExoneracionId = value;
                ddlExoneracion.SelectedValue = _iExoneracionId.ToString();
            }
        }

        string _vTipoPagoId = string.Empty;

        public string vTipoPagoId
        {
            get { return HF_TipoPagoId.Value; }
            set
            {
                _vTipoPagoId = value;
                HF_TipoPagoId.Value = _vTipoPagoId;
            }
        }


        string _vSustentoTipoPago = string.Empty;

        public string vSustentoTipoPago
        {
            get { return txtSustentoTipoPago.Text; }
            set
            {
                _vSustentoTipoPago = value;
                txtSustentoTipoPago.Text = _vSustentoTipoPago;
            }
        }

        bool _isVisible_lblExoneracion = false;

        public bool isVisible_lblExoneracion
        {
            get { return lblExoneracion.Visible; }
            set
            {
                _isVisible_lblExoneracion = value;
                lblExoneracion.Visible = _isVisible_lblExoneracion;
            }
        }

        bool _isVisible_ddlExoneracion = false;

        public bool isVisible_ddlExoneracion
        {
            get { return ddlExoneracion.Visible; }
            set
            {
                _isVisible_ddlExoneracion = value;
                ddlExoneracion.Visible = _isVisible_ddlExoneracion;
            }
        }

        bool _isVisible_lblSustentoTipoPago = false;

        public bool isVisible_lblSustentoTipoPago
        {
            get { return lblSustentoTipoPago.Visible; }
            set
            {
                _isVisible_lblSustentoTipoPago = value;
                lblSustentoTipoPago.Visible = _isVisible_lblSustentoTipoPago;
            }
        }

        bool _isVisible_txtSustentoTipoPago = false;

        public bool isVisible_txtSustentoTipoPago
        {
            get { return txtSustentoTipoPago.Visible; }
            set
            {
                _isVisible_txtSustentoTipoPago = value;
                txtSustentoTipoPago.Visible = _isVisible_txtSustentoTipoPago;
            }
        }

        bool _isVisible_lblValSustentoTipoPago = false;

        public bool isVisible_lblValSustentoTipoPago
        {
            get { return lblValSustentoTipoPago.Visible; }
            set
            {
                _isVisible_lblValSustentoTipoPago = value;
                lblValSustentoTipoPago.Visible = _isVisible_lblValSustentoTipoPago;
            }
        }

        bool _isVisible_RBNormativa = false;

        public bool isVisible_RBNormativa
        {
            get { return RBNormativa.Visible; }
            set
            {
                _isVisible_RBNormativa = value;
                RBNormativa.Visible = _isVisible_RBNormativa;
            }
        }

        bool _isVisible_RBSustentoTipoPago = false;

        public bool isVisible_RBSustentoTipoPago
        {
            get { return RBSustentoTipoPago.Visible; }
            set
            {
                _isVisible_RBSustentoTipoPago = value;
                RBSustentoTipoPago.Visible = _isVisible_RBSustentoTipoPago;
            }
        }

        bool _isVisible_lblValExoneracion = false;

        public bool isVisible_lblValExoneracion
        {
            get { return lblValExoneracion.Visible; }
            set
            {
                _isVisible_lblValExoneracion = value;
                lblValExoneracion.Visible = _isVisible_lblValExoneracion;
            }
        }

        bool _isChecked_rbNormativa = false;

        public bool isChecked_rbNormativa
        {
            get { return RBNormativa.Checked; }
            set
            {
                _isChecked_rbNormativa = value;
                RBNormativa.Checked = _isChecked_rbNormativa;
            }
        }

        bool _isChecked_rbSustentoTipoPago = false;

        public bool isChecked_rbSustentoTipoPago
        {
            get { return RBSustentoTipoPago.Checked; }
            set
            {
                _isChecked_rbSustentoTipoPago = value;
                RBSustentoTipoPago.Checked = _isChecked_rbSustentoTipoPago;
            }
        }

        bool _isEnabled_ddlExoneracion = false;

        public bool isEnabled_ddlExoneracion
        {
            get { return ddlExoneracion.Enabled; }
            set
            {
                _isEnabled_ddlExoneracion = value;
                ddlExoneracion.Enabled = _isEnabled_ddlExoneracion;
            }
        }

        bool _isEnabled_txtSustentoTipoPago = false;

        public bool isEnabled_txtSustentoTipoPago
        {
            get { return txtSustentoTipoPago.Enabled; }
            set
            {
                _isEnabled_txtSustentoTipoPago = value;
                txtSustentoTipoPago.Enabled = _isEnabled_txtSustentoTipoPago;
            }
        }

        #endregion

        #region Constructores
       


        protected void Page_Load(object sender, EventArgs e)
        {

            if (ViewState[(sender as Control).UniqueID] == null)
            {
                ViewState[(sender as Control).UniqueID] = (sender as Control).UniqueID;
            }

            this.uniqueKey = ViewState[(sender as Control).UniqueID].ToString();

            if (!IsPostBack)
            {


                if (string.IsNullOrEmpty(_vMonto))
                    _vMonto = "0";
                
                CargarCombos();
                
               // this.ddlTipoPago.Attributes["onchange"] = "return ucActuacionPagoHabilitarPorTipoPago_" + uniqueKey + "();";
                this.ddlBanco.Attributes["onchange"] = "return ucActuacionPagoValidarControles_" + uniqueKey + "();";
                this.txtNroVoucher.Attributes["onblur"] = "return ucActuacionPagoValidarControles_" + uniqueKey + "();";
                this.txtNroOperacion.Attributes["onblur"] = "return ucActuacionPagoValidarControles_" + uniqueKey + "();";
                this.btnValidar.OnClientClick = "return ucActuacionPagoMostrarValidacion_" + uniqueKey + "(); return false;";

            }


            Comun.EjecutarScriptUniqueIdDinamico(this.Page, "ucActuacionPagoHabilitarPorTipoPago_" + uniqueKey + "();",uniqueKey);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            
        }

        private void CargarCombos()
        {
            Util.CargarParametroDropDownList(ddlTipoPago, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO), true);

            Util.CargarParametroDropDownList(ddlBanco, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.BANCO), true);
            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
            }
            if (_iTipoPago > 0)
            {
                ddlTipoPago.SelectedValue = _iTipoPago.ToString();
            }
        }

        #endregion

      
        #region Métodos

        public void CargarTipoPagoNormaTarifario()
        {

            int IntTotalCount = 0;
            int IntTotalPages = 0;

            if (vTarifaLetra.Trim().Length == 0)
            { return; }

            string strTarifaLetra = vTarifaLetra.Trim().ToUpper();

            //---------------------------------------------------------------------
            DataTable dtNormaTarifario = new DataTable();
            NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();

            dtNormaTarifario = objNormaTarifarioBL.Consultar(0, -1, strTarifaLetra, "", false, 20000, 1, "N", ref IntTotalCount, ref IntTotalPages);
            DataTable dtTipoPagoSel = dtNormaTarifario.DefaultView.ToTable(true, "nota_sPagoTipoId");
            //---------------------------------------------------------------------
            DataTable dtConsuladoTipoPagoTarifa = new DataTable();
            OficinaConsularTarifarioTipoPagoDL objConsuladoTarifarioTipoPagoBL = new OficinaConsularTarifarioTipoPagoDL();

            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            dtConsuladoTipoPagoTarifa = objConsuladoTarifarioTipoPagoBL.Consultar(intOficinaConsularId, 0, strTarifaLetra, false, 20000, 1, "N", ref IntTotalCount, ref IntTotalPages);
            DataTable dtConsuladoTipoPagoSel = dtConsuladoTipoPagoTarifa.DefaultView.ToTable(true, "ofpa_sPagoTipoId");
            //----------------------------------------------------
            DataTable dtTipoPago = new DataTable();
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);
            Util.CargarParametroDropDownList(ddlTipoPago, dtTipoPago, true);

            if (dtTipoPagoSel.Rows.Count > 0)
            {
                //-------------------------------------
                Int16 intTipoPagoId = 0;
                bool bExisteTipoPago = false;
                bool bExisteConsuladoTipoPago = false;

                for (int i = 0; i < dtTipoPago.Rows.Count; i++)
                {
                    intTipoPagoId = Convert.ToInt16(dtTipoPago.Rows[i]["id"].ToString());
                    bExisteTipoPago = false;

                    for (int x = 0; x < dtTipoPagoSel.Rows.Count; x++)
                    {
                        if (intTipoPagoId == Convert.ToInt16(dtTipoPagoSel.Rows[x]["nota_sPagoTipoId"].ToString()))
                        {
                            bExisteTipoPago = true;
                            break;
                        }
                    }
                    bExisteConsuladoTipoPago = false;

                    for (int z = 0; z < dtConsuladoTipoPagoSel.Rows.Count; z++)
                    {
                        if (intTipoPagoId == Convert.ToInt16(dtConsuladoTipoPagoSel.Rows[z]["ofpa_sPagoTipoId"].ToString()))
                        {
                            bExisteConsuladoTipoPago = true;
                            break;
                        }
                    }
                    if (bExisteTipoPago == false && bExisteConsuladoTipoPago == false)
                    {
                        ddlTipoPago.Items.Remove(ddlTipoPago.Items.FindByValue(dtTipoPago.Rows[i]["id"].ToString()));
                    }
                }
                //------------------------------------
            }

            if (Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Convert.ToInt16(Constantes.CONST_ID_CONSULADO_CARACAS))
            {
                if (ddlTipoPago.Items.FindByText("PAGO ARUBA") != null)
                {
                    ddlTipoPago.Items.FindByText("PAGO ARUBA").Enabled = false;
                }
                if (ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS") != null)
                {
                    ddlTipoPago.Items.FindByText("PAGO OTRAS ISLAS CARIBEÑAS").Enabled = false;
                }
            }
        }

        public void LlenarListaExoneracion()
        {
            if (ddlTipoPago.SelectedIndex == 0)
            {
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                lblValExoneracion.Visible = false;
                return;
            }
            
            string strTarifaLetra = lbltarifa.Text.Trim().ToUpper();
            string strTipoPago = ddlTipoPago.SelectedItem.Text.Trim();

            DataTable dtExoneracion = new DataTable();

            NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            string strFecha = Comun.syyyymmdd(DateTime.Now.ToShortDateString());
            Int16 intTipoPagoId = Convert.ToInt16(ddlTipoPago.SelectedValue);

            dtExoneracion = objNormaTarifarioBL.Consultar(intTipoPagoId, -1, strTarifaLetra, strFecha, false, 1000, 1, "N", ref IntTotalCount, ref IntTotalPages);

            Util.CargarDropDownList(ddlExoneracion, dtExoneracion, "norm_vDescripcionCorta", "nota_iNormaTarifarioId", true);
            if (dtExoneracion.Rows.Count > 0)
            {
                #region Si_ExisteRegistros

                if (dtExoneracion.Rows.Count == 1)
                {
                    ddlExoneracion.SelectedIndex = 1;
                }
                else
                {
                    ddlExoneracion.SelectedIndex = 0;
                }
                ddlExoneracion.Enabled = true;
                RBNormativa.Checked = true;
                txtSustentoTipoPago.Enabled = false;
                txtSustentoTipoPago.Text = "";
                lblExoneracion.Visible = true;
                ddlExoneracion.Visible = true;
                lblValExoneracion.Visible = true;

                if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
                {
                    lblSustentoTipoPago.Visible = true;
                    txtSustentoTipoPago.Visible = true;
                    lblValSustentoTipoPago.Visible = true;
                    RBNormativa.Visible = true;
                    RBSustentoTipoPago.Visible = true;
                }
                else
                {
                    lblSustentoTipoPago.Visible = false;
                    txtSustentoTipoPago.Visible = false;
                    lblValSustentoTipoPago.Visible = false;
                    RBNormativa.Visible = false;
                    RBSustentoTipoPago.Visible = false;
                }

                #endregion
            }
            else
            {
                #region Cuando_No_Existan_registros

                lblExoneracion.Visible = false;
                ddlExoneracion.Visible = false;
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
                lblValExoneracion.Visible = false;

                #endregion
            } 
            
        }

        public void MostrarDL173_DS076_2005RE()
        {
            //---------------------------------------------------------------------------
            //Fecha: 21/01/2019
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Habilitar la etiqueta: D.L. 173 del D.S. 0076-2005-RE
            //          cuando el tipo de pago sea: Gratuito por Ley 
            //          no tomar en cuenta la tarifa 2 ni la Sección III del Tarifario
            //---------------------------------------------------------------------------
            bool bisSeccionIII = Comun.isSeccionIII(Session, lbltarifa.Text.Trim().ToUpper());
            string strTarifa = lbltarifa.Text.Trim().ToUpper();


            if (Comun.ToNullInt32(ddlTipoPago.SelectedValue) == Comun.ToNullInt32(Enumerador.enmTipoCobroActuacion.GRATIS))
            {
                if (bisSeccionIII == false && strTarifa != "2")
                {
                    lblExoneracion.Visible = true;
                    //lblNorma173.Visible = true;
                    lblValExoneracion.Visible = false;
                    lblSustentoTipoPago.Visible = true;
                    txtSustentoTipoPago.Visible = true;
                    txtSustentoTipoPago.Enabled = true;
                    lblValSustentoTipoPago.Visible = true;
                    RBNormativa.Visible = false;
                    RBSustentoTipoPago.Visible = false;
                }
                else
                {
                    //lblNorma173.Visible = false;
                }
            }
            else
            {
                //lblNorma173.Visible = false;
                lblSustentoTipoPago.Visible = false;
                txtSustentoTipoPago.Visible = false;
                lblValSustentoTipoPago.Visible = false;
                RBNormativa.Visible = false;
                RBSustentoTipoPago.Visible = false;
            }
           
            //-----------------------------------------------------------------
        }

   

        #endregion


        protected void RBNormativa_CheckedChanged(object sender, EventArgs e)
        {
            ddlExoneracion.Enabled = RBNormativa.Checked;
            txtSustentoTipoPago.Enabled = !(RBNormativa.Checked);
            if (ddlExoneracion.Enabled == false)
            {
                ddlExoneracion.SelectedIndex = 0;
            }
            else
            {
                txtSustentoTipoPago.Text = "";
            }
            
        }

        protected void ddlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            LlenarListaExoneracion();
            MostrarDL173_DS076_2005RE();
            RBNormativa.Checked = true;
            RBSustentoTipoPago.Checked = false;

            int intId = Convert.ToInt32(ddlTipoPago.SelectedValue);

            ddlTipoPagoSelectorChanged(new ddlTipoPagoCommandEventArgs(intId));
        }

   
    }
}