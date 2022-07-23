using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.ReglasNegocio;
using Seguridad.Logica.BussinessEntity;
using SAE.UInterfaces;
using Microsoft.Security.Application;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;

namespace SolCARDIP.Paginas.Recepcion
{
    public partial class RecepcionEntradaRegistro : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region Session Principal
                csUsuarioBE objUsuarioBE = new csUsuarioBE();
                objUsuarioBE = (csUsuarioBE)Session["usuario"];
                ViewState["Usuarioid"] = objUsuarioBE.UsuarioId;
                ViewState["Oficinaid"] = objUsuarioBE.codOficina;
                ViewState["IP"] = oCodigoUsuario.obtenerIP();
                #endregion
                #region Generales
                //GENERALES
                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    // CATEGORIA OFICINA EXTRANJERA
                    obeGenerales.CategoriaOficinaExtranjera.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                    ddlCategoriaOfcoEx.DataSource = obeGenerales.CategoriaOficinaExtranjera;
                    ddlCategoriaOfcoEx.DataValueField = "Parametroid";
                    ddlCategoriaOfcoEx.DataTextField = "Descripcion";
                    ddlCategoriaOfcoEx.DataBind();
                    cargarComboNull(ddlMision);
                    // TIPO ENTRADA
                    obeGenerales.TipoEntrada.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                    ddlTipoEntrada.DataSource = obeGenerales.TipoEntrada;
                    ddlTipoEntrada.DataValueField = "Parametroid";
                    ddlTipoEntrada.DataTextField = "Descripcion";
                    ddlTipoEntrada.DataBind();
                }
                #endregion
            }
            else
            {
            }
        }

        protected void seleccionarCategoriaOficina(object sender, EventArgs e)
        {
            try
            {
                if (!ddlCategoriaOfcoEx.SelectedValue.Equals("0"))
                {
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();
                        lbeOficinaconsularExtranjera = oCodigoUsuario.obtenerOficinasConsularesExtranjeras(short.Parse(ddlCategoriaOfcoEx.SelectedValue), obeGenerales.ListaOficinasConsularesExtranjeras);
                        if (lbeOficinaconsularExtranjera.Count > 0)
                        {
                            lbeOficinaconsularExtranjera.Insert(0, new beOficinaconsularExtranjera { OficinaconsularExtranjeraid = 0, Nombre = "<Seleccione>" });
                            ddlMision.DataSource = lbeOficinaconsularExtranjera;
                            ddlMision.DataValueField = "OficinaconsularExtranjeraid";
                            ddlMision.DataTextField = "Nombre";
                            ddlMision.DataBind();
                            ddlMision.Enabled = true;
                        }
                        else
                        {
                            cargarComboNull(ddlMision);
                            ddlMision.Enabled = false;
                        }
                    }
                }
                else
                {
                    cargarComboNull(ddlMision);
                    ddlMision.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
                obrGeneral.grabarLog(ex);
            }
        }

        protected void cargarComboNull(DropDownList controlDropDown)
        {
            try
            {
                List<beNull> lbeNull = new List<beNull>();
                beNull obeNull = new beNull();
                obeNull.Id = "0";
                obeNull.Seleccione = "<Seleccione>";
                lbeNull.Add(obeNull);
                controlDropDown.DataSource = lbeNull;
                controlDropDown.DataValueField = "Id";
                controlDropDown.DataTextField = "Seleccione";
                controlDropDown.DataBind();
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
    }
}