using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlToolBarImage : System.Web.UI.UserControl
    {
        #region Declare
        // Declaracion del Delegado  
        public delegate void OnButtonNuevoClick();
        public delegate void OnButtonEditarClick();
        public delegate void OnButtonEliminarClick();
        public delegate void OnButtonGrabarClick();
        public delegate void OnButtonCancelarClick();
        public delegate void OnButtonBuscarClick();
        public delegate void OnButtonExportarExcelClick();
        public delegate void OnButtonExportarPDFClick();
        public delegate void OnButtonPrintClick();
        public delegate void OnButtonSalirClick();

        // Declaraciones de los Eventos
        public event OnButtonNuevoClick imgNuevoHandler;
        public event OnButtonEditarClick imgEditarHandler;
        public event OnButtonEliminarClick imgEliminarHandler;
        public event OnButtonGrabarClick imgGrabarHandler;
        public event OnButtonCancelarClick imgCancelarHandler;
        public event OnButtonBuscarClick imgBuscarHandler;
        public event OnButtonExportarExcelClick imgExportarExcelHandler;
        public event OnButtonExportarPDFClick imgExportarPDFHandler;
        public event OnButtonPrintClick imgPrintHandler;
        public event OnButtonSalirClick imgSalirHandler;

        #endregion

        #region Properties

        private bool m_VisibleIButtonImgNuevo = false;
        public bool VisibleIButtonImgNuevo
        {
            get { return m_VisibleIButtonImgNuevo; }
            set { m_VisibleIButtonImgNuevo = value; }
        }

        private bool m_VisibleIButtonImgEditar = false;
        public bool VisibleIButtonImgEditar
        {
            get { return m_VisibleIButtonImgEditar; }
            set { m_VisibleIButtonImgEditar = value; }
        }

        private bool m_VisibleIButtonImgEliminar = false;
        public bool VisibleIButtonImgEliminar
        {
            get { return m_VisibleIButtonImgEliminar; }
            set { m_VisibleIButtonImgEliminar = value; }
        }

        private bool m_VisibleIButtonImgGrabar = false;
        public bool VisibleIButtonImgGrabar
        {
            get { return m_VisibleIButtonImgGrabar; }
            set { m_VisibleIButtonImgGrabar = value; }
        }

        private bool m_VisibleIButtonImgCancelar = false;
        public bool VisibleIButtonImgCancelar
        {
            get { return m_VisibleIButtonImgCancelar; }
            set { m_VisibleIButtonImgCancelar = value; }
        }

        private bool m_VisibleIButtonImgBuscar = false;
        public bool VisibleIButtonImgBuscar
        {
            get { return m_VisibleIButtonImgBuscar; }
            set { m_VisibleIButtonImgBuscar = value; }
        }

        private bool m_VisibleIButtonImgExportarExcel = false;
        public bool VisibleIButtonImgExportarExcel
        {
            get { return m_VisibleIButtonImgExportarExcel; }
            set { m_VisibleIButtonImgExportarExcel = value; }
        }

        private bool m_VisibleIButtonImgExportarPDF = false;
        public bool VisibleIButtonImgExportarPDF
        {
            get { return m_VisibleIButtonImgExportarPDF; }
            set { m_VisibleIButtonImgExportarPDF = value; }
        }

        private bool m_VisibleIButtonImgPrint = false;
        public bool VisibleIButtonImgPrint
        {
            get { return m_VisibleIButtonImgPrint; }
            set { m_VisibleIButtonImgPrint = value; }
        }

        private bool m_VisibleIButtonImgSalir = false;
        public bool VisibleIButtonImgSalir
        {
            get { return m_VisibleIButtonImgSalir; }
            set { m_VisibleIButtonImgSalir = value; }
        }       
          
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            commandNew.Visible = m_VisibleIButtonImgNuevo;

            if (commandNew.Visible == false)
            {
                separator1.Visible = false;
            }

            commandEdit.Visible = m_VisibleIButtonImgEditar;

            if (commandEdit.Visible == false)
            {
                separator2.Visible = false;
                separator3.Visible = false;
            }

            commandDelete.Visible = m_VisibleIButtonImgEliminar;

            if (commandDelete.Visible == false)
            {
                separator3.Visible = false;
                separator4.Visible = false;
            }

            commandSave.Visible = m_VisibleIButtonImgGrabar;

            if (commandSave.Visible == false)
            {
                separator4.Visible = false;
                separator5.Visible = false;
            }

            commandUndo.Visible = m_VisibleIButtonImgCancelar;

            if (commandUndo.Visible == false)
            {
                separator5.Visible = false;
                separator6.Visible = false;
            }

            commandSearch.Visible = m_VisibleIButtonImgBuscar;

            if (commandSearch.Visible == false)
            {
                separator6.Visible = false;
                separator7.Visible = false;
            }

            commandExportToExcel.Visible = m_VisibleIButtonImgExportarExcel;

            if (commandExportToExcel.Visible == false)
            {
                separator7.Visible = false;
                separator8.Visible = false;
            }

            commandExportToPDF.Visible = m_VisibleIButtonImgExportarPDF;

            if (commandExportToPDF.Visible == false)
            {
                separator8.Visible = false;
                separator9.Visible = false;
            }

            commandPrint.Visible = m_VisibleIButtonImgPrint;

            if (commandPrint.Visible == false)
            {
                separator9.Visible = false;
            }

            commandExit.Visible = m_VisibleIButtonImgSalir;

            if (commandExit.Visible == false)
            {
                separator9.Visible = false;
            }
        }

        protected void imgNuevo_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgNuevoHandler != null)
            {
                imgNuevoHandler();
            }           
        }

        protected void imgEditar_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgEditarHandler != null)
            {
                imgEditarHandler();
            }    
        }

        protected void imgEliminar_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgEliminarHandler != null)
            {
                imgEliminarHandler();
            }    
        }

        protected void imgGrabar_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgGrabarHandler != null)
            {
                imgGrabarHandler();
            }    
        }

        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgCancelarHandler != null)
            {
                imgCancelarHandler();
            }    
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {           
            if (imgBuscarHandler != null)
            {
                imgBuscarHandler();
            }    
        }

        protected void imgExportarExcel_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgExportarExcelHandler != null)
            {
                imgExportarExcelHandler();
            }    
        }

        protected void imgExportarPDF_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgExportarPDFHandler != null)
            {
                imgExportarPDFHandler();
            }    
        }

        protected void imgPrint_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgPrintHandler != null)
            {
                imgPrintHandler();
            }    
        }

        protected void imgSalir_Click(object sender, ImageClickEventArgs e)
        {            
            if (imgSalirHandler != null)
            {
                imgSalirHandler();
            }    
        }
    }

    #endregion

    #region Methods
        //<button id="btnNuevo"><img src="../Images/img_menu_add.png" alt="" height="16px" /> Nueva</button>
    #endregion

}