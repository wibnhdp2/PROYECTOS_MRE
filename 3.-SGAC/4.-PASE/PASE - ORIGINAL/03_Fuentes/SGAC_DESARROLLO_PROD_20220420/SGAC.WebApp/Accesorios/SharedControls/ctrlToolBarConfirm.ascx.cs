using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using System.Data;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlToolBarConfirm : UserControl
    {
        #region Declare
        // Declaracion del Delegado  
        public delegate void OnButtonNuevoClick();
        public delegate void OnButtonEditarClick();
        public delegate void OnButtonEliminarClick();
        public delegate void OnButtonGrabarClick();
        public delegate void OnButtonCancelarClick();
        public delegate void OnButtonBuscarClick();
        //public delegate void OnButtonExportarExcelClick();
        //public delegate void OnButtonExportarPDFClick();
        public delegate void OnButtonPrintClick();
        public delegate void OnButtonConfigurationClick();
        public delegate void OnButtonSalirClick();

        // Declaraciones de los Eventos
        public event OnButtonNuevoClick btnNuevoHandler;
        public event OnButtonEditarClick btnEditarHandler;
        public event OnButtonEliminarClick btnEliminarHandler;
        public event OnButtonGrabarClick btnGrabarHandler;
        public event OnButtonCancelarClick btnCancelarHandler;
        public event OnButtonBuscarClick btnBuscarHandler;
        //public event OnButtonExportarExcelClick btnExportarExcelHandler;
        //public event OnButtonExportarPDFClick btnExportarPDFHandler;
        public event OnButtonPrintClick btnPrintHandler;
        public event OnButtonConfigurationClick btnConfigurationHandler;
        public event OnButtonSalirClick btnSalirHandler;

        #endregion

        #region Properties

        //public string ValidationGroup
        //{
        //    get { return btnGrabar.ValidationGroup; }
        //    set { 
        //        btnGrabar.ValidationGroup = value;
        //        btnEliminar.ValidationGroup = value;
        //    }
        //}

        //private bool m_Enabled = false;
        //public bool Enabled
        //{
        //    get { return m_Enabled; }
        //    set { m_Enabled = value; }
        //}

        private bool m_VisibleIButtonNuevo = false;
        public bool VisibleIButtonNuevo
        {
            get { return m_VisibleIButtonNuevo; }
            set { m_VisibleIButtonNuevo = value; }
        }

        private bool m_VisibleIButtonEditar = false;
        public bool VisibleIButtonEditar
        {
            get { return m_VisibleIButtonEditar; }
            set { m_VisibleIButtonEditar = value; }
        }

        private bool m_VisibleIButtonEliminar = false;
        public bool VisibleIButtonEliminar
        {
            get { return m_VisibleIButtonEliminar; }
            set { m_VisibleIButtonEliminar = value; }
        }

        private bool m_VisibleIButtonGrabar = false;
        public bool VisibleIButtonGrabar
        {
            get { return m_VisibleIButtonGrabar; }
            set { m_VisibleIButtonGrabar = value; }
        }

        private bool m_VisibleIButtonCancelar = false;
        public bool VisibleIButtonCancelar
        {
            get { return m_VisibleIButtonCancelar; }
            set { m_VisibleIButtonCancelar = value; }
        }

        private bool m_VisibleIButtonBuscar = false;
        public bool VisibleIButtonBuscar
        {
            get { return m_VisibleIButtonBuscar; }
            set { m_VisibleIButtonBuscar = value; }
        }



        //private bool m_VisibleIButtonExportarExcel = false;
        //public bool VisibleIButtonExportarExcel
        //{
        //    get { return m_VisibleIButtonExportarExcel; }
        //    set { m_VisibleIButtonExportarExcel = value; }
        //}

        //private bool m_VisibleIButtonExportarPDF = false;
        //public bool VisibleIButtonExportarPDF
        //{
        //    get { return m_VisibleIButtonExportarPDF; }
        //    set { m_VisibleIButtonExportarPDF = value; }
        //}

        public string ValidationGrupo
        {
            get { return btnGrabar.ValidationGroup; }
            set {
                btnGrabar.ValidationGroup = value;
                btnEliminar.ValidationGroup = value;
            }
        }

        private bool m_VisibleIButtonPrint = false;
        public bool VisibleIButtonPrint
        {
            get { return m_VisibleIButtonPrint; }
            set { m_VisibleIButtonPrint = value; }
        }

        private bool m_VisibleIButtonConfiguration = false;
        public bool VisibleIButtonConfiguration
        {
            get { return m_VisibleIButtonConfiguration; }
            set { m_VisibleIButtonConfiguration = value; }
        }

        private bool m_VisibleIButtonSalir = false;
        public bool VisibleIButtonSalir
        {
            get { return m_VisibleIButtonSalir; }
            set { m_VisibleIButtonSalir = value; }
        }

        #endregion

        #region Methods
        // Tab: mantenmiento
        public void HabilitarBotonesPorOpcion(Enumerador.enmBoton enmBoton)
        {
            btnNuevo.Enabled = false;
            btnEditar.Enabled = false;
            btnGrabar.Enabled = false;
            btnEliminar.Enabled = false;
            btnConfiguration.Enabled = false;
            btnCancelar.Enabled = true;
            switch (enmBoton)
            {
                case Enumerador.enmBoton.NINGUNO:
                    btnNuevo.Enabled = true;
                    btnGrabar.Enabled = true;
                    break;
                case Enumerador.enmBoton.VER:
                    break;
                case Enumerador.enmBoton.EDITAR:
                    btnNuevo.Enabled = true;
                    btnGrabar.Enabled = true;
                    btnEliminar.Enabled = true;
                    break;
                case Enumerador.enmBoton.ELIMINAR:
                    btnGrabar.Enabled = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            commandNew.Visible = m_VisibleIButtonNuevo;
            commandEdit.Visible = m_VisibleIButtonEditar;
            commandDelete.Visible = m_VisibleIButtonEliminar;
            commandSave.Visible = m_VisibleIButtonGrabar;
            commandUndo.Visible = m_VisibleIButtonCancelar;
            commandSearch.Visible = m_VisibleIButtonBuscar;
            commandPrint.Visible = m_VisibleIButtonPrint;
            commandConfiguration.Visible = m_VisibleIButtonConfiguration;
            commandExit.Visible = m_VisibleIButtonSalir;

            //btnNuevo.Enabled = m_Enabled;
            //btnEditar.Enabled = m_Enabled;
            //btnEliminar.Enabled = m_Enabled;
            //btnGrabar.Enabled = m_Enabled;
            //btnCancelar.Enabled = m_Enabled;
            //btnBuscar.Enabled = m_Enabled;
            //btnExportarExcel.Enabled = m_Enabled;
            //btnExportarPDF.Enabled = m_Enabled;
            //btnImprimir.Enabled = m_Enabled;
            //btnConfiguration.Enabled = m_Enabled;
            //btnSalir.Enabled = m_Enabled;        
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if (btnNuevoHandler != null)
            {
                btnNuevoHandler();
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            if (btnEditarHandler != null)
            {
                btnEditarHandler();
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (btnEliminarHandler != null)
            {
                btnEliminarHandler();
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if (btnGrabarHandler != null)
            {
                btnGrabarHandler();
            }
            else
            {
                //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (btnCancelarHandler != null)
            {
                btnCancelarHandler();
            }
            else
            {
                //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (btnBuscarHandler != null)
            {
                btnBuscarHandler();
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }        

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            if (btnPrintHandler != null)
            {
                btnPrintHandler();
            }
            else
            {
                //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnConfiguration_Click(object sender, EventArgs e)
        {
            if (btnConfigurationHandler != null)
            {
                btnConfigurationHandler();
            }
            else
            {
                //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            if (btnSalirHandler != null)
            {
                btnSalirHandler();
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "OPCIONES", Constantes.CONST_MENSAJE_NO_IMPLEMENTACION));
            }
        }

        #endregion
    }
}