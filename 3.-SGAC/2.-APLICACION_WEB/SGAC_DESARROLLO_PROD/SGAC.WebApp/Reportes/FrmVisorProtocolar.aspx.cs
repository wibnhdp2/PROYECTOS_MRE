using System;
using System.Data;
using SGAC.Registro.Actuacion.BL;
using SGAC.Accesorios;
using Microsoft.Reporting.WebForms;
using SGAC.WebApp.Accesorios;
 
namespace SGAC.WebApp.Reportes
{
    public partial class FrmVisorProtocolar : System.Web.UI.Page
    {
        #region Variables de Enlace
        private Int64 _lonActoNotarialId = 0;
        private Int16 _intTipoFormatoProtocolar = 0;
        private Int16 _intOficinaConsular = 0;
        private String _sFormato = string.Empty;

        private ActoNotarialConsultaBL _bl = null;
        private DataTable _dt = null;
        private ReportDataSource _rptDataSource = null;

        ReportParameter[] parameters = new ReportParameter[4];
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _lonActoNotarialId = Convert.ToInt64(Session["Acto_Notarial_ID"]); //Id del acto notarial
                _intTipoFormatoProtocolar = Convert.ToInt16(Session["Acto_Notarial_Tipo_formato"]); //8401.Minuta, 8402.Parte, 8403.Testimonio, 8404.Escritura
                _intOficinaConsular = Convert.ToInt16(Session["OficinaConsular"]);

                try
                {
                    if (_intTipoFormatoProtocolar == (int)Enumerador.enmFormatoProtocolar.MINUTA)
                    {
                        Generar_Formato_Minuta();
                    }
                    else if (_intTipoFormatoProtocolar == (int)Enumerador.enmFormatoProtocolar.PARTE)
                    {
                        Generar_Formato_Parte();
                    }
                    else if (_intTipoFormatoProtocolar == (int)Enumerador.enmFormatoProtocolar.TESTIMONIO)
                    {
                        Generar_Formato_Testimonio();
                    }
                    else if (_intTipoFormatoProtocolar == (int)Enumerador.enmFormatoProtocolar.ESCRITURA)
                    {
                        Generar_Formato_Escritura();
                    }
                    else if (_intTipoFormatoProtocolar == (int)Enumerador.enmFormatoProtocolar.LISTADO_ESCRITURAS)
                    {
                        Generar_Formato_Lista_Escrituras();
                    }
                    else if (_intTipoFormatoProtocolar == (int)Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR)
                    {
                        //-----------------------------------------------------------------------------
                        // Autor: Miguel Angel Márquez Beltrán
                        // Fecha: 19/09/2016
                        // Objetivo: Se adiciono la opción del reporte de Autorización Viaje de Menor
                        //-----------------------------------------------------------------------------

                        Generar_Formato_Lista_AutorizacionViajeMenors();
                    }

                    dsReport.LocalReport.DataSources.Clear();
                    dsReport.LocalReport.ReportEmbeddedResource = Server.MapPath(_sFormato);
                    dsReport.LocalReport.ReportPath = Server.MapPath(_sFormato);
                    if (_intTipoFormatoProtocolar == (int)Enumerador.enmFormatoProtocolar.LISTADO_ESCRITURAS ||
                        _intTipoFormatoProtocolar == (int)Enumerador.enmExtraprotocolarTipo.AUTORIZACION_VIAJE_MENOR)
                    {
                        dsReport.LocalReport.SetParameters(parameters);
                    }
                    dsReport.LocalReport.DataSources.Add(_rptDataSource);
                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion

        #region Métodos
        private void Generar_Formato_Minuta()
        {
            //---------------------------------------------------------------------------------------------------
            //Carga DataTable desde el StoredProcedure
            //---------------------------------------------------------------------------------------------------
            _bl = new ActoNotarialConsultaBL();
            _dt = new DataTable();
            _dt = _bl.ObtenerCuerpo(_lonActoNotarialId);
            //---------------------------------------------------------------------------------------------------
            _sFormato = "~/Reportes/RSNotarial/rsMinuta.rdlc";
            _rptDataSource = new ReportDataSource("Minuta", _dt);
        }

        private void Generar_Formato_Parte()
        {
            //---------------------------------------------------------------------------------------------------
            //Carga DataTable desde el StoredProcedure
            //---------------------------------------------------------------------------------------------------           
            _dt = new DataTable();

            _dt = (DataTable)Session["dtParte"];

            //---------------------------------------------------------------------------------------------------
            _sFormato = "~/Reportes/RSNotarial/rsParte.rdlc";
            _rptDataSource = new ReportDataSource("Parte", _dt);
            Session.Remove("dtParte");
        }

        private void Generar_Formato_Testimonio()
        {
            //---------------------------------------------------------------------------------------------------
            //Carga DataTable desde el StoredProcedure
            //---------------------------------------------------------------------------------------------------           
            _dt = new DataTable();

            _dt = (DataTable)Session["dtTestimonio"];

            //---------------------------------------------------------------------------------------------------
            _sFormato = "~/Reportes/RSNotarial/rsTestimonio.rdlc";
            _rptDataSource = new ReportDataSource("Testimonio", _dt);
            Session.Remove("dtTestimonio");
        }

        private void Generar_Formato_Escritura()
        {
            //---------------------------------------------------------------------------------------------------
            //Carga DataTable desde el StoredProcedure
            //---------------------------------------------------------------------------------------------------
            _bl = new ActoNotarialConsultaBL();
            _dt = new DataTable();
            _dt = _bl.ObtenerCuerpo(_lonActoNotarialId);
            //---------------------------------------------------------------------------------------------------
            _sFormato = "~/Reportes/RSNotarial/rsEscritura.rdlc";
            _rptDataSource = new ReportDataSource("Escritura", _dt);
        }

        private void Generar_Formato_Lista_Escrituras()
        {
            //---------------------------------------------------------------------------------------------------
            //Carga DataTable desde el StoredProcedure
            //---------------------------------------------------------------------------------------------------
            _bl = new ActoNotarialConsultaBL();
            _dt = new DataTable();
            _dt = (DataTable)Session["dtReporteEscrituras"];
            //---------------------------------------------------------------------------------------------------

            _sFormato = "~/Reportes/RSNotarial/rsConsultaEscrituras.rdlc";
            _rptDataSource = new ReportDataSource("ConsultaEP", _dt);

            parameters[0] = new ReportParameter("Titulo", "SERVICIO CONSULAR DEL PERÚ");
            parameters[1] = new ReportParameter("SubTitulo", "Reporte de Actos Extraprotocolares");
            parameters[2] = new ReportParameter("OficinaConsular", comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_vNombre").ToString());
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());

            Session.Remove("dtReporteEscrituras");
        }

        //-----------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 19/09/2016
        // Objetivo: Se adiciono la opción del reporte de Autorización Viaje de Menor
        //-----------------------------------------------------------------------------

        private void Generar_Formato_Lista_AutorizacionViajeMenors()
        {
            //---------------------------------------------------------------------------------------------------
            //Carga DataTable desde el StoredProcedure
            //---------------------------------------------------------------------------------------------------
            _bl = new ActoNotarialConsultaBL();
            _dt = new DataTable();
            _dt = (DataTable)Session["dtReporteEscrituras"];
            //---------------------------------------------------------------------------------------------------

            _sFormato = "~/Reportes/RSNotarial/rsConsultaAutViajeMenor.rdlc";
            _rptDataSource = new ReportDataSource("dtExtraProtocolar", _dt);

            parameters[0] = new ReportParameter("Titulo", "REGISTRO DE EXTENSIÓN DE AUTORIZACIÓN DE VIAJE DE MENOR");
            parameters[1] = new ReportParameter("SubTitulo", "x");
            parameters[2] = new ReportParameter("OficinaConsular", comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_vNombre").ToString());
            parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());

            Session.Remove("dtReporteEscrituras");
        }

        #endregion
    }
}