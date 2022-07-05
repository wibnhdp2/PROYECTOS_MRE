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
// PDF --------------------------------------
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
// ------------------------------------------
using System.Web.Routing;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SolCARDIP.Paginas.Reportes
{
    public partial class FormularioReportes : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Session["Reporte"] = null;
                    #region Generales
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        // CALIDAD MIGRATORIA
                        obeGenerales.ListaCalidadMigratoriaNivelPrincipal.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                        ddlCalidadMigratoriaPri.DataSource = obeGenerales.ListaCalidadMigratoriaNivelPrincipal;
                        ddlCalidadMigratoriaPri.DataValueField = "CalidadMigratoriaid";
                        ddlCalidadMigratoriaPri.DataTextField = "Nombre";
                        ddlCalidadMigratoriaPri.DataBind();
                        // CATEGORIA OFICINA EXTRANJERA
                        obeGenerales.CategoriaOficinaExtranjera.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlCategoriaOfcoEx.DataSource = obeGenerales.CategoriaOficinaExtranjera;
                        ddlCategoriaOfcoEx.DataValueField = "Parametroid";
                        ddlCategoriaOfcoEx.DataTextField = "Descripcion";
                        ddlCategoriaOfcoEx.DataBind();
                        cargarComboNull(ddlMision);
                        // ESTADOS
                        obeGenerales.ListaEstados.Insert(0, new beEstado { Estadoid = 0, DescripcionCorta = "<Seleccione>" });
                        ddlEstado.DataSource = obeGenerales.ListaEstados;
                        ddlEstado.DataValueField = "Estadoid";
                        ddlEstado.DataTextField = "DescripcionCorta";
                        ddlEstado.DataBind();

                        //estado de pestaña consulta de carnet
                        ddlEstadoRep.DataSource = obeGenerales.ListaEstados;
                        ddlEstadoRep.DataValueField = "Estadoid";
                        ddlEstadoRep.DataTextField = "DescripcionCorta";
                        ddlEstadoRep.DataBind();


                        obeGenerales.ListaPaises.Insert(0, new bePais { Paisid = 0, Nombre = "<Seleccione>" });
                        ddlPaisRep.DataSource = obeGenerales.ListaPaises;
                        ddlPaisRep.DataValueField = "Paisid";
                        ddlPaisRep.DataTextField = "Nombre";
                        ddlPaisRep.DataBind();


                        obeGenerales.TitularDependienteParametros.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlTitulaFamiliarRep.DataSource = obeGenerales.TitularDependienteParametros;
                        ddlTitulaFamiliarRep.DataValueField = "Parametroid";
                        ddlTitulaFamiliarRep.DataTextField = "Descripcion";
                        ddlTitulaFamiliarRep.DataBind();

                        obeGenerales.ListaOficinasConsularesExtranjeras.Insert(0, new beOficinaconsularExtranjera { OficinaconsularExtranjeraid = 0, Nombre = "<Seleccione>" });
                        ddlOficinaRep.DataSource = obeGenerales.ListaOficinasConsularesExtranjeras;
                        ddlOficinaRep.DataValueField = "OficinaconsularExtranjeraid";
                        ddlOficinaRep.DataTextField = "Nombre";
                        ddlOficinaRep.DataBind();

                        obeGenerales.ListaUsuarios.Insert(0, new beUsuario { Usuarioid = 0, NombreCompleto = "<Seleccione>" });
                        ddlUsuarioRep.DataSource = obeGenerales.ListaUsuarios;
                        ddlUsuarioRep.DataValueField = "Usuarioid";
                        ddlUsuarioRep.DataTextField = "NombreCompleto";
                        ddlUsuarioRep.DataBind();

                        obeGenerales.ListaDocumentoIdentidadRegLinea.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionCorta = "<Seleccione>" });
                        ddlTipoDocRep.DataSource = obeGenerales.ListaDocumentoIdentidadRegLinea;
                        ddlTipoDocRep.DataValueField = "Tipodocumentoidentidadid";
                        ddlTipoDocRep.DataTextField = "DescripcionCorta";
                        ddlTipoDocRep.DataBind();


                        obeGenerales.TipoEmision.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });


                        List<beParametro> listTipoEmision = new List<beParametro>();
                        listTipoEmision.Add(obeGenerales.TipoEmision.Find(x => x.Descripcion.Contains("NUEVO")));
                        listTipoEmision.Add(obeGenerales.TipoEmision.Find(x => x.Descripcion.Contains("DUPLICADO")));
                        listTipoEmision.Add(obeGenerales.TipoEmision.Find(x => x.Descripcion.Contains("RENOVACIÓN")));
                        listTipoEmision.Add(obeGenerales.TipoEmision.Find(x => x.Descripcion.Contains("<Seleccione>")));
                        foreach (var item in listTipoEmision)
                        {
                            if (item.Valor==null)
                            {
                                item.Descripcion = "TODOS";
                                item.Valor = "0";
                            }
                        }

                        ddlStipoEmisionRep.DataSource = listTipoEmision;
                        ddlStipoEmisionRep.DataValueField = "Parametroid";
                        ddlStipoEmisionRep.DataTextField = "Descripcion";
                        ddlStipoEmisionRep.DataBind();
                        ddlStipoEmisionRep.SelectedValue = "0";

                    }
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "preloader", "preloader();", true);
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void traerDatos(object sender, EventArgs e)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            beReporteResumenxCalidad parametros = new beReporteResumenxCalidad();
            parametros.FechaInicio = DateTime.Parse(txtFechaInicio.Text);
            parametros.FechaFin = DateTime.Parse(txtFechaFin.Text);
            brReportes obrReportes = new brReportes();
            obeReportesListas = obrReportes.traerDatos(parametros);
            obeReportesListas.ParametrosResumen = parametros;
            Session["ReporteResumen"] = obeReportesListas;
            gvReportexCalidad.DataSource = obeReportesListas.ListaResultado;
            gvReportexCalidad.DataBind();
            if (obeReportesListas.Totales != null)
            {
                lblCantCalMig.Text = obeReportesListas.ListaResultado.Count.ToString();
                lblCantReg.Text = obeReportesListas.Totales.Registrados.ToString();
                lblCantEmi.Text = obeReportesListas.Totales.Emitidos.ToString();
                lblCantVig.Text = obeReportesListas.Totales.Vigentes.ToString();
                lblCantVen.Text = obeReportesListas.Totales.Vencidos.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
        }

        protected void traerDatosDetalle(object sender, EventArgs e)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            beReporteDetallexCalidad parametros = new beReporteDetallexCalidad();
            parametros.FechaInicio = DateTime.Parse(txtFechaInicioDet.Text);
            parametros.FechaFin = DateTime.Parse(txtFechaFinDet.Text);
            parametros.CalMigId = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
            parametros.EstadoId = short.Parse(ddlEstado.SelectedValue);
            parametros.OfcoExId = short.Parse(ddlMision.SelectedValue);
            if (rbFechaEmision.Checked)
            {
                parametros.TipoBusqueda = "EMISION";
            }
            else
            {
                parametros.TipoBusqueda = "REGISTRO";
            }
            
            brReportes obrReportes = new brReportes();
            obeReportesListas = obrReportes.traerDatosDetalle(parametros);
            obeReportesListas.ParametrosDetalle = parametros;
            Session["ReporteDetalle"] = obeReportesListas;
            gvReporteDetalle.DataSource = obeReportesListas.ListaResultadoDetalle;
            gvReporteDetalle.DataBind();
            if (obeReportesListas.ListaResultadoDetalle != null)
            {
                lblCantRegDetalle.Text = obeReportesListas.ListaResultadoDetalle.Count.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
        }
        protected void consultarCarnet(object sender, EventArgs e)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            beReporteDetallexCalidad parametros = new beReporteDetallexCalidad();


            parametros.StipoEmision = short.Parse(ddlStipoEmisionRep.SelectedValue); 
            parametros.FechaInicio = txtFechaInicioRep.Text.Equals("") ? new DateTime(1900, 1, 1) : DateTime.Parse(txtFechaInicioRep.Text); 
            parametros.FechaFin = txtFechaFinRep.Text.Equals("") ? new DateTime(1900, 1, 1) : DateTime.Parse(txtFechaFinRep.Text);
            parametros.EstadoId = short.Parse(ddlEstadoRep.SelectedValue); 
            parametros.TipoDoc = ddlTipoDocRep.SelectedItem.Text.Equals("<Seleccione>")?"": ddlTipoDocRep.SelectedItem.Text;
            parametros.Num_documento = txtNum_documentoRep.Text;
            parametros.Num_solicitud = txtNum_solicitudRep.Text;
            parametros.Numero_carne = txtNumero_carneRep.Text;
            parametros.Apellidos = txtApellidosRep.Text.ToUpper();
            parametros.FechaNacimiento = txtFechaNacimientoRep.Text.Equals("")? new DateTime(1900, 1, 1): DateTime.Parse(txtFechaNacimientoRep.Text); //------------------
            parametros.Paisid= short.Parse(ddlPaisRep.SelectedValue); 
            parametros.TitulaFamiliar = short.Parse(ddlTitulaFamiliarRep.SelectedValue);
            parametros.OfCoId = short.Parse(ddlOficinaRep.SelectedValue);
            parametros.Usuario = short.Parse(ddlUsuarioRep.SelectedValue);

            brReportes obrReportes = new brReportes();
            obeReportesListas = obrReportes.consularCarnet(parametros);
            obeReportesListas.ParametrosDetalle = parametros;
            Session["ReporteConsultaCarnet"] = obeReportesListas;
            gvConsultaCarnet.DataSource = obeReportesListas.ListaResultadoDetalle;
            gvConsultaCarnet.DataBind();
            if (obeReportesListas.ListaResultadoDetalle != null)
            {
                cantRegistroCarnet.Text = obeReportesListas.ListaResultadoDetalle.Count.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
        }

        protected void imprimirReporteResumen(object sender, EventArgs e)
        {
            try
            {
                beReportesListas obeReportesListas = new beReportesListas();
                obeReportesListas = (beReportesListas)Session["ReporteResumen"];
                object[] arrParametros = new object[2];
                arrParametros[0] = obeReportesListas.ParametrosResumen.FechaInicio.ToShortDateString();
                arrParametros[1] = obeReportesListas.ParametrosResumen.FechaFin.ToShortDateString();
                Session["ParametrosReporte"] = arrParametros;

                DataTable dtResumen = new DataTable();
                DataTable dtTotales = new DataTable();

                beReporteResumenxCalidad objetoResumen = new beReporteResumenxCalidad();
                Type tipo1 = objetoResumen.GetType();
                PropertyInfo[] prop1 = tipo1.GetProperties();

                string name = prop1[0].Name;

                for (int n = 0; n <= prop1.Length - 1; n++)
                {
                    dtResumen.Columns.Add(prop1[n].Name);
                    dtTotales.Columns.Add(prop1[n].Name);
                }


                object[] arrSStr;
                int _contador = 0;
                // RESUMEN -----------------------------------------------------
                foreach (beReporteResumenxCalidad objetoResumen1 in obeReportesListas.ListaResultado)
                {
                    arrSStr = new object[dtResumen.Columns.Count];
                    _contador = 0;
                    foreach (var prop in objetoResumen1.GetType().GetProperties())
                    {
                        //string nombre = prop.Name;
                        object valorCampo = prop.GetValue(objetoResumen1, null);
                        arrSStr[_contador] = valorCampo;
                        _contador = _contador + 1;
                    }
                    dtResumen.Rows.Add(arrSStr);
                }

                // TOTALES -----------------------------------------------------
                beReporteResumenxCalidad objetoResumen2 = obeReportesListas.Totales;
                arrSStr = new object[dtTotales.Columns.Count];
                _contador = 0;
                foreach (var prop in objetoResumen2.GetType().GetProperties())
                {
                    //string nombre = prop.Name;
                    object valorCampo = prop.GetValue(objetoResumen2, null);
                    arrSStr[_contador] = valorCampo;
                    _contador = _contador + 1;
                }
                dtTotales.Rows.Add(arrSStr);

                List<DataTable> listaTablas = new List<DataTable>();
                listaTablas.Add(dtResumen);
                listaTablas.Add(dtTotales);
                string rutaTipoReporte = "RDLC/ResumenxCalidad.rdlc";

                Session["reporteRuta"] = rutaTipoReporte;
                Session["DataSetListaTablas"] = listaTablas;
                string identRep = "001";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script>");
                sb.Append("window.open('VistaImpresion.aspx?identRep=" + identRep + "');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "", sb.ToString(), false);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR EN LA GENERACION DEL REPORTE');", true);
                obrGeneral.grabarLog(ex);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
        }

        protected void imprimirReporteDetalle(object sender, EventArgs e)
        {
            try
            {
                beReportesListas obeReportesListas = new beReportesListas();
                obeReportesListas = (beReportesListas)Session["ReporteDetalle"];
                object[] arrParametros = new object[6];
                arrParametros[0] = obeReportesListas.ParametrosDetalle.FechaInicio.ToShortDateString();
                arrParametros[1] = obeReportesListas.ParametrosDetalle.FechaFin.ToShortDateString();
                arrParametros[2] = (obeReportesListas.ParametrosDetalle.CalMigId == 0 ? "[Todos]" : ddlCalidadMigratoriaPri.SelectedItem.Text);
                arrParametros[3] = (obeReportesListas.ParametrosDetalle.EstadoId == 0 ? "[Todos]" : ddlEstado.SelectedItem.Text);
                arrParametros[4] = (obeReportesListas.ParametrosDetalle.OfcoExId == 0 ? "[Todos]" : ddlMision.SelectedItem.Text);
                arrParametros[5] = obeReportesListas.ListaResultadoDetalle.Count.ToString();
                Session["ParametrosReporte"] = arrParametros;

                DataTable dtDetalle = new DataTable();

                beReporteDetallexCalidad objetoDetalle = new beReporteDetallexCalidad();
                Type tipo1 = objetoDetalle.GetType();
                PropertyInfo[] prop1 = tipo1.GetProperties();

                string name = prop1[0].Name;

                for (int n = 0; n <= prop1.Length - 1; n++)
                {
                    dtDetalle.Columns.Add(prop1[n].Name);
                }

                object[] arrSStr;
                int _contador = 0;
                // RESUMEN -----------------------------------------------------
                foreach (beReporteDetallexCalidad objetoResumen1 in obeReportesListas.ListaResultadoDetalle)
                {
                    arrSStr = new object[dtDetalle.Columns.Count];
                    _contador = 0;
                    foreach (var prop in objetoResumen1.GetType().GetProperties())
                    {
                        //string nombre = prop.Name;
                        object valorCampo = prop.GetValue(objetoResumen1, null);
                        arrSStr[_contador] = valorCampo;
                        _contador = _contador + 1;
                    }
                    dtDetalle.Rows.Add(arrSStr);
                }

                List<DataTable> listaTablas = new List<DataTable>();
                listaTablas.Add(dtDetalle);
                string rutaTipoReporte = "RDLC/DetallexCalidad.rdlc";

                Session["reporteRuta"] = rutaTipoReporte;
                Session["DataSetListaTablas"] = listaTablas;
                string identRep = "002";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script>");
                sb.Append("window.open('VistaImpresion.aspx?identRep=" + identRep + "');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "", sb.ToString(), false);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR EN LA GENERACION DEL REPORTE');", true);
                obrGeneral.grabarLog(ex);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
        }
        protected void imprimirReporteConsultaCarnet(object sender, EventArgs e)
        {
            try
            {
                beReportesListas obeReportesListas = new beReportesListas();
                obeReportesListas = (beReportesListas)Session["ReporteConsultaCarnet"];
                if (obeReportesListas == null){
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
                    return;                    
                }
                object[] arrParametros = new object[6];
                arrParametros[0] = obeReportesListas.ParametrosDetalle.FechaInicio.ToShortDateString();
                arrParametros[1] = obeReportesListas.ParametrosDetalle.FechaFin.ToShortDateString();
                arrParametros[2] = (obeReportesListas.ParametrosDetalle.CalMigId == 0 ? "[Todos]" : ddlCalidadMigratoriaPri.SelectedItem.Text);
                arrParametros[3] = (obeReportesListas.ParametrosDetalle.EstadoId == 0 ? "[Todos]" : ddlEstado.SelectedItem.Text);
                arrParametros[4] = (obeReportesListas.ParametrosDetalle.OfcoExId == 0 ? "[Todos]" : ddlMision.SelectedItem.Text);
                arrParametros[5] = obeReportesListas.ListaResultadoDetalle.Count.ToString();
                Session["ParametrosReporte"] = arrParametros;

                DataTable dtDetalle = new DataTable();

                beReporteDetallexCalidad objetoDetalle = new beReporteDetallexCalidad();
                Type tipo1 = objetoDetalle.GetType();
                PropertyInfo[] prop1 = tipo1.GetProperties();

                string name = prop1[0].Name;

                for (int n = 0; n <= prop1.Length - 1; n++)
                {
                    dtDetalle.Columns.Add(prop1[n].Name);
                }

                object[] arrSStr;
                int _contador = 0;
                // RESUMEN -----------------------------------------------------
                foreach (beReporteDetallexCalidad objetoResumen1 in obeReportesListas.ListaResultadoDetalle)
                {
                    arrSStr = new object[dtDetalle.Columns.Count];
                    _contador = 0;
                    foreach (var prop in objetoResumen1.GetType().GetProperties())
                    {
                        //string nombre = prop.Name;
                        object valorCampo = prop.GetValue(objetoResumen1, null);
                        arrSStr[_contador] = valorCampo;
                        _contador = _contador + 1;
                    }
                    dtDetalle.Rows.Add(arrSStr);
                }

                List<DataTable> listaTablas = new List<DataTable>();
                listaTablas.Add(dtDetalle);
                string rutaTipoReporte = "RDLC/RptConsultaCarnet.rdlc";

                Session["reporteRuta"] = rutaTipoReporte;
                Session["DataSetListaTablas"] = listaTablas;
                string identRep = "003";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script>");
                sb.Append("window.open('VistaImpresion.aspx?identRep=" + identRep + "');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "", sb.ToString(), false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR EN LA GENERACION DEL REPORTE');", true);
                obrGeneral.grabarLog(ex);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
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