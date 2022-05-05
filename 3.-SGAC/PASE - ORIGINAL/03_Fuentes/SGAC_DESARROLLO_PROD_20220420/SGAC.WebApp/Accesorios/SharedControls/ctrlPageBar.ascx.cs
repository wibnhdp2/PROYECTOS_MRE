using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using SGAC.Accesorios;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlPageBar : System.Web.UI.UserControl
    {
        public event EventHandler Click;

        #region Properties       

        private int _PageSize;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        public int TotalResgistros
        {
            get { return int.Parse(lblNroRegistros.Text); }
            set { lblNroRegistros.Text = Convert.ToString(value); }
        }

        public int TotalPaginas
        {
            get { return int.Parse(lblTotalPaginas.Text); }
            set { lblTotalPaginas.Text = Convert.ToString(value); }
        }

        public int PaginaActual
        {
            get { return int.Parse(lblPaginaActual.Text); }
            set { lblPaginaActual.Text = Convert.ToString(value); }
        }

        public int RangoInicial
        {
            get { return int.Parse(lblInicio.Text); }
            set { lblInicio.Text = Convert.ToString(value); }
        }

        public int RangoFinal
        {
            get { return int.Parse(lblFin.Text); }
            set { lblFin.Text = Convert.ToString(value); }
        }

        //16-11-2014
        public int PaginaIr
        {
            get { return int.Parse(txtPagina.Text); }
            set { txtPagina.Text = Convert.ToString(value); }
        }

        public bool BotonSiguienteEnabled
        {
            get { return btnSiguiente.Enabled; }
            set { btnSiguiente.Enabled = value; }
        }

        public string BotonSiguienteImageUrl
        {
            get { return btnSiguiente.ImageUrl; }
            set { btnSiguiente.ImageUrl = value; }
        }

        public bool BotonAnteriorEnabled
        {
            get { return btnAnterior.Enabled; }
            set { btnAnterior.Enabled = value; }
        }

        public string BotonAnteriorImageUrl
        {
            get { return btnAnterior.ImageUrl; }
            set { btnAnterior.ImageUrl = value; }
        }

        public bool BotonPrimeroEnabled
        {
            get { return btnPrimero.Enabled; }
            set { btnPrimero.Enabled = value; }
        }

        public string BotonPrimeroImageUrl
        {
            get { return btnPrimero.ImageUrl; }
            set { btnPrimero.ImageUrl = value; }
        }

        public bool BotonUltimoEnabled
        {
            get { return btnUltimo.Enabled; }
            set { btnUltimo.Enabled = value; }
        }

        public string BotonUltimoImageUrl
        {
            get { return btnUltimo.ImageUrl; }
            set { btnUltimo.ImageUrl = value; }
        }
        #endregion

        #region Eventos

        public void SetAtributtes(String evento, String funcion)
        {
            this.txtPagina.Attributes.Add(evento, funcion);
        }      

        private void Page_Init(object sender, System.EventArgs e)
        {
            ScriptCliente();          
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPagina.Attributes.Add("onkeypress", String.Format("javascript:return SoloNumeros(event)"));
            

            if (!Page.IsPostBack)
            {
                this.BotonSiguienteEnabled = true;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
                this.BotonUltimoEnabled = true;
                this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";

                lblInicio.Text = "1";
                lblFin.Text = this.PageSize.ToString();
            }
        }

        protected void btnPrimero_Click(object sender, ImageClickEventArgs e)
        {
            this.PaginaActual = int.Parse(lblPaginaActual.Text);
            this.TotalPaginas = int.Parse(lblTotalPaginas.Text);

            this.PaginaActual = 1;

            txtPagina.Text = Convert.ToString(this.PaginaActual);
            lblInicio.Text = "1";
            lblFin.Text = this.PageSize.ToString();

            if (this.TotalPaginas > 1)
            {
                this.BotonSiguienteEnabled = true;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
                this.BotonUltimoEnabled = true;
                this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";
            }

            this.TotalResgistros = int.Parse(lblNroRegistros.Text);

            HabilitaDesabilitaBotones(this.TotalPaginas);

            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        protected void btnSiguiente_Click(object sender, ImageClickEventArgs e)
        {
            
            this.PaginaActual = int.Parse(lblPaginaActual.Text);
            this.TotalPaginas = int.Parse(lblTotalPaginas.Text);

            this.PaginaActual = this.PaginaActual + 1;

            txtPagina.Text = Convert.ToString(this.PaginaActual);

            Paginas_Paginas();

            if (this.PaginaActual < this.TotalPaginas)
            {
                if (this.TotalPaginas > 1)
                {
                    this.BotonSiguienteEnabled = true;
                    this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
                    this.BotonUltimoEnabled = true;
                    this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";
                }

                this.TotalResgistros = int.Parse(lblNroRegistros.Text);

                HabilitaDesabilitaBotones(this.TotalPaginas);
            }
            else
            {
                this.BotonAnteriorEnabled = true;
                this.BotonAnteriorImageUrl = "../../Images/NavPreviousPage.gif";
                this.BotonPrimeroEnabled = true;
                this.BotonPrimeroImageUrl = "../../Images/NavFirstPage.gif";

                this.BotonSiguienteEnabled = false;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPageDisabled.gif";
                this.BotonUltimoEnabled = false;
                this.BotonUltimoImageUrl = "../../Images/NavLastPageDisabled.gif";
            }

            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        protected void btnAnterior_Click(object sender, ImageClickEventArgs e)
        {
            this.PaginaActual = int.Parse(lblPaginaActual.Text);
            this.TotalPaginas = int.Parse(lblTotalPaginas.Text);

            this.PaginaActual = this.PaginaActual - 1;

            txtPagina.Text = Convert.ToString(this.PaginaActual);

            Paginas_Paginas();
            if (this.TotalPaginas > 1)
            {
                this.BotonSiguienteEnabled = true;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
                this.BotonUltimoEnabled = true;
                this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";
            }

            this.TotalResgistros = int.Parse(lblNroRegistros.Text);

            HabilitaDesabilitaBotones(this.TotalPaginas);

            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        protected void btnUltimo_Click(object sender, ImageClickEventArgs e)
        {
            this.PaginaActual = int.Parse(lblPaginaActual.Text);
            this.TotalPaginas = int.Parse(lblTotalPaginas.Text);

            this.PaginaActual = this.TotalPaginas;

            txtPagina.Text = Convert.ToString(this.PaginaActual);

            Paginas_Paginas();

            if (this.TotalPaginas > 1)
            {
                this.BotonSiguienteEnabled = true;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
                this.BotonUltimoEnabled = true;
                this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";
            }

            this.TotalResgistros = int.Parse(lblNroRegistros.Text);

            HabilitaDesabilitaBotones(this.TotalPaginas);

            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        private void Paginas_Paginas()
        {
            lblInicio.Text = (this.PageSize * (this.PaginaActual - 1) + 1).ToString();
            int s_Numero = this.PageSize * this.PaginaActual;
            if (s_Numero > this.TotalResgistros)
            {
                s_Numero = s_Numero - this.TotalResgistros;
            }
            else
                s_Numero = 0;
            lblFin.Text = ((this.PageSize * this.PaginaActual) - s_Numero).ToString();
        }
        protected void btnIrPagina_Click(object sender, EventArgs e)
        {
            //Comun.EjecutarScript(Page, "alert(" + btnIrPagina.ClientID + ");");

            if ((txtPagina.Text.Length > 0) && (int.Parse(txtPagina.Text) > 0) && (int.Parse(txtPagina.Text) <= this.TotalPaginas))
            {
                this.PaginaActual = int.Parse(txtPagina.Text);
                this.TotalPaginas = int.Parse(lblTotalPaginas.Text);

                Paginas_Paginas(); // 15/05/2015

                HabilitaDesabilitaBotones(this.TotalPaginas);
            }
            else
            {
                this.txtPagina.Text = "1";
                this.TotalPaginas = int.Parse(lblTotalPaginas.Text);
                InicializarPaginador();
            }

            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Metodos
        private void HabilitaDesabilitaBotones(int iTotalPaginas)
        {
            
            // Se desabilita el primer link
            if (this.PaginaActual == 1)
            {
                this.BotonPrimeroEnabled = false;
                this.BotonPrimeroImageUrl = "../../Images/NavFirstPageDisabled.gif";
            }
            else
            {
                this.BotonPrimeroEnabled = true;
                this.BotonPrimeroImageUrl = "../../Images/NavFirstPage.gif";
            }

            // Se desabilita el link previo            
            if (this.PaginaActual == 1)
            {
                this.BotonAnteriorEnabled = false;
                this.BotonAnteriorImageUrl = "../../Images/NavPreviousPageDisabled.gif";
            }
            else
            {
                this.BotonAnteriorEnabled = true;
                this.BotonAnteriorImageUrl = "../../Images/NavPreviousPage.gif";
            }

            // Se desabilita el siguiente link
            if (this.PaginaActual < iTotalPaginas)
            {
                this.BotonSiguienteEnabled = true;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
            }
            else
            {
                this.BotonSiguienteEnabled = false;
                this.BotonSiguienteImageUrl = "../../Images/NavNextPageDisabled.gif";
            }

            // Se desabilita el ultimo link
            if (this.PaginaActual < iTotalPaginas)
            {
                this.BotonUltimoEnabled = true;
                this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";
            }
            else
            {
                this.BotonUltimoEnabled = false;
                this.BotonUltimoImageUrl = "../../Images/NavLastPageDisabled.gif";
            }
        }

        private void ScriptCliente()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("__ScriptCliente__"))
            {
                StringBuilder sScript = new StringBuilder();

                sScript.AppendLine("");

                sScript.AppendLine("function SoloNumeros(e){");
                sScript.AppendLine("    var key;");
                sScript.AppendLine("    if(window.event){");
                sScript.AppendLine("        key = e.keyCode;");
                sScript.AppendLine("   }else if(e.which){");
                sScript.AppendLine("       key = e.which;");
                sScript.AppendLine("   }");
           
                sScript.AppendLine("   if ((key < 48 || key > 57) && key!=13){");
                sScript.AppendLine("    return false;");
                sScript.AppendLine("   }");

                sScript.AppendLine(" if(key==13){ ");
                /***********************************Developer by Carlos Hernández*************************************/
                
                sScript.AppendLine("   $('#MainContent_ctrlPaginadorActuacion_btnIrPagina').click(); ");
                sScript.AppendLine("     e.preventDefault();");
                /******************************************************************************************************/
                sScript.AppendLine("   ");
                sScript.AppendLine("}");
                sScript.AppendLine("    return true;");
                sScript.AppendLine("}");

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "__ScriptCliente__", sScript.ToString(), true);
            }
        }

        // 16-11-2014 IDM
 
        public void InicializarPaginador()
        {
            this.BotonPrimeroEnabled = false;
            this.BotonPrimeroImageUrl = "../../Images/NavFirstPageDisabled.gif";
            this.BotonAnteriorEnabled = false;
            this.BotonAnteriorImageUrl = "../../Images/NavPreviousPageDisabled.gif";
            this.BotonSiguienteEnabled = true;
            this.BotonSiguienteImageUrl = "../../Images/NavNextPage.gif";
            this.BotonUltimoEnabled = true;
            this.BotonUltimoImageUrl = "../../Images/NavLastPage.gif";

            Visible = false;

            TotalResgistros = 0;
            TotalPaginas = 0;
            PaginaActual = 1;
            PaginaIr = 1;

            RangoInicial = 1;
            RangoFinal = this.PageSize;
        }
        #endregion        

        protected void Button1_Click(object sender, EventArgs e)
        {
//            Comun.EjecutarScript(Page, "alert(" +btnIrPagina.ClientID+");");
        }
    }
}