using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Registro.Persona.BL;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmBuscarFuncionario : Page
    {
        private static readonly int PAGE_SIZE_FUNCIONARIOS = 10;
        private string strNombreFuncionario = "FUNCIONARIO";
        private string strIdFuncionario = "Funcionario_Indice";
        private string strVariableDtFunc = "Funcionario_Tabla";

        #region Eventos

        private void Page_Init(object sender, System.EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    UpdGrvPaginada.Update();
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }


        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            try
            {
                buscar();
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }

        private void buscar()
        {
            try
            {
                if (txtNroDocumento.Text.Trim() == string.Empty &&
                    txtPriApellido.Text.Trim() == string.Empty &&
                    txtSegApellido.Text.Trim() == string.Empty &&
                    txtNombres.Text.Trim() == string.Empty)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreFuncionario, "No ha ingresado información para la búsqueda."));
                    return;
                }

                if ((txtNroDocumento.Text.Length > 0) || (txtPriApellido.Text.Length >= 3) || (txtSegApellido.Text.Length >= 3) || (txtNombres.Text.Length >= 3))
                {
                    ctrlPaginador.InicializarPaginador();
                    CargarGrillaSolicitante(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim(), txtNombres.Text.Trim());
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_MIN_3_CARACTERES, true, Enumerador.enmTipoMensaje.WARNING);
                    ctrlPaginador.Visible = false;
                    ctrlPaginador.PaginaActual = 1;
                    ctrlPaginador.InicializarPaginador();

                    Grd_Solicitante.DataSource = null;
                    Grd_Solicitante.DataBind();
                }

                UpdGrvPaginada.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void MyUserControlPagingEvent_Click(object sender, EventArgs e)
        {
            CargarGrillaSolicitante(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim(), txtNombres.Text.Trim());
        }


        protected void Grd_Solicitante_RowCommand(object sender, GridViewCommandEventArgs e)
        {           
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Select")
                {
                    DataTable dt = (DataTable)Session[strVariableDtFunc];
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["IFuncionarioId"].ToString().Equals(Grd_Solicitante.Rows[index].Cells[0].Text))
                            {
                                Comun.EjecutarScript(Page, "alert('El funcionario ya está registrado');");
                                return;
                            }
                        }
                    }

                    Session[strIdFuncionario] = Grd_Solicitante.Rows[index].Cells[0].Text;
                    Session["Funcionario_Documento"] = Grd_Solicitante.Rows[index].Cells[1].Text;
                    Session["ocfu_vApellidoPaternoFuncionario"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[2].Text);
                    Session["ocfu_vApellidoMaternoFuncionario"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[3].Text);
                    Session["ocfu_vNombreFuncionario"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[4].Text);
                    Session["Funcionario_Cargo"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[5].Text);
                    Session["Funcionario_sGenero"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[6].Text);

                    Comun.EjecutarScript(this, "window.parent.close_ModalPopup('MainContent_btnEjecutarFuncionario');");                    
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }
        #endregion

        #region Métodos

        private void CargarGrillaSolicitante(string StrNroDoc, string StrApePat, string StrApeMat, string strNombre)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            ctrlValidacion.MostrarValidacion("", false);
            FuncionarioConsultaBL BL = new FuncionarioConsultaBL();
            DataTable DtFuncionario = new DataTable();

            DtFuncionario = BL.Funcionario_Listar(StrNroDoc, StrApePat, StrApeMat, strNombre,
                                                  ctrlPaginador.PaginaActual.ToString(), PAGE_SIZE_FUNCIONARIOS,
                                                  ref IntTotalCount, ref IntTotalPages);

            if (DtFuncionario.Rows.Count > 0)
            {
                Grd_Solicitante.DataSource = DtFuncionario;
                Grd_Solicitante.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlPaginador.Visible = false;
                if (IntTotalPages > 1)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(IntTotalCount), true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.WARNING);
                ctrlPaginador.Visible = false;
                ctrlPaginador.PaginaActual = 1;
                ctrlPaginador.InicializarPaginador();

                Grd_Solicitante.DataSource = null;
                Grd_Solicitante.DataBind();
            }

            UpdGrvPaginada.Update();
        }

        #endregion

    }
}