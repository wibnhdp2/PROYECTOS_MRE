using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Controlador;
using System.Data;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using Microsoft.Reporting.WebForms;
using SGAC.Almacen.BL;

namespace SGAC.WebApp.Almacen
{
    public partial class FrmReporte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string StrNombreArchReporte = string.Empty;
                StrNombreArchReporte = "rsOrdenServicio.rdlc";

                DataTable dt = new DataTable();
                MovimientoDetalleConsultaBL objBL = new MovimientoDetalleConsultaBL();
                Int32 mode_iMovimientoId = Convert.ToInt32(Session["Movimiento_Id"]);
                dt = objBL.MovimientoDetalleConsultar(mode_iMovimientoId);

                SGAC.BE.AL_MOVIMIENTO objMovimiento = new BE.AL_MOVIMIENTO(); 
                objMovimiento = (BE.AL_MOVIMIENTO)Session["OBJ_MOVIMIENTO"];

                ReportParameter[] parameters = parameters = new ReportParameter[14];
                parameters[0] = new ReportParameter("MovOficinaConsular", Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString());

                parameters[1] = new ReportParameter("MovUsuario", Session[Constantes.CONST_SESION_USUARIO].ToString());
                parameters[2] = new ReportParameter("MovCodigoMovimiento", objMovimiento.movi_cMovimientoCodigo.ToString());
                parameters[3] = new ReportParameter("MovOficinaConsularOrigen", comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(objMovimiento.movi_sOficinaConsularIdOrigen)));

                parameters[4] = new ReportParameter("MovTipoBovedaOrigen", comun_Part1.ObtenerParametroPorId(Session, objMovimiento.movi_sBovedaTipoIdOrigen, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA).Rows[0]["descripcion"].ToString());
                parameters[5] = new ReportParameter("MovNombreBovedaOrigen", comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(objMovimiento.movi_sBodegaOrigenId)));

                parameters[6] = new ReportParameter("MovOficinaConsularDestino", comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(objMovimiento.movi_sOficinaConsularIdDestino)));
                parameters[7] = new ReportParameter("MovTipoBovedaDestino", comun_Part1.ObtenerParametroPorId(Session, objMovimiento.movi_sBovedaTipoIdDestino, Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA).Rows[0]["descripcion"].ToString());
                parameters[8] = new ReportParameter("MovNombreBovedaDestino", comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(objMovimiento.movi_sBodegaDestinoId)));

                parameters[9] = new ReportParameter("mode_iMovimientoId", Session["Movimiento_Id"].ToString());
                parameters[10] = new ReportParameter("MovCodigoPedido", objMovimiento.movi_cPedidoCodigo.ToString());
                parameters[11] = new ReportParameter("MovActaRemision", objMovimiento.movi_vActaRemision.ToString());
                parameters[12] = new ReportParameter("MovMotivo", comun_Part1.ObtenerParametroPorId(Session, Convert.ToInt32(objMovimiento.movi_sMovimientoMotivoId), Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO).Rows[0]["descripcion"].ToString());
                parameters[13] = new ReportParameter("MovInsumo", comun_Part1.ObtenerParametroPorId(Session, Convert.ToInt32(objMovimiento.movi_sInsumoTipoId), Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO).Rows[0]["descripcion"].ToString());
 
                Session.Remove("Movimiento_Id");
                
                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);

                dsReport.LocalReport.DataSources.Clear();
                dsReport.LocalReport.Refresh();


                dsReport.LocalReport.ReportEmbeddedResource = Server.MapPath("~/Almacen/rsOrdenServicio.rdlc");
                dsReport.LocalReport.ReportPath = Server.MapPath("~/Almacen/rsOrdenServicio.rdlc");
                dsReport.LocalReport.SetParameters(parameters);
                dsReport.LocalReport.DataSources.Add(datasource);
            }
        }
      
    }
}