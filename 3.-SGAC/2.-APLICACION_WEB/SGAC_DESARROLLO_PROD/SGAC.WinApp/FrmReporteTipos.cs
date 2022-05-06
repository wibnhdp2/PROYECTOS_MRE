using System;
using System.Data;
using System.Windows.Forms;

namespace SGAC.WinApp
{
    public partial class FrmReporteTipos : Form
    {
        private Funciones FunFunciones = new Funciones();

        public FrmReporteTipos()
        {
            InitializeComponent();
        }

        //DataSet CargarDatos()
        //{
        //    DataSet dsLeerFicheroXML = new DataSet();
        //    DataTable DtResult = new DataTable();
        //    dsLeerFicheroXML.Clear();
        //    dsLeerFicheroXML.ReadXml(Program.strArchivo);
        //    //DtResult = dsLeerFicheroXML.Tables[5];

        //    return dsLeerFicheroXML;
        //}

        private void CmdImprimir_Click(object sender, EventArgs e)
        {
            string StrNombreOficinaConsular = "";
            string StrTituloReporte = "";
            string StrTitulo1 = "";
            DataSet DsDatos = new DataSet();

            //DsDatos = CargarDatos();

            StrNombreOficinaConsular = Program.TraeOficinaConsular(Program.strServOficinaConsularCodigo.ToString(), Program.strArchivo).ToUpper();
            if (DtpFchEmision.Text == "")
            {
                MessageBox.Show("No ha indicado la fecha de emision de los ticket", "Sistema de Colas", MessageBoxButtons.OK);
            }

            string VRutaArchivo = "";

            if (RdoOpcion1.Checked == true)
            {
                VRutaArchivo = Program.vRutaReportes + "\\rsTicketEmitidosResumen.rdlc";
                StrTituloReporte = "Sistema de Colas - Detalle de Ticket Emitidos x Dia";
                StrTitulo1 = "REPORTE DE TICKET RESUMIDO";
            }

            if (RdoOpcion2.Checked == true)
            {
                VRutaArchivo = Program.vRutaReportes + "\\rsTicketListaEmitidos.rdlc";
                StrTituloReporte = "Sistema de Colas - Resumen de Ticket Emitidos x Dia";
                StrTitulo1 = "REPORTE DE TICKET DETALLADO";
            }

            string strFecha = DtpFchEmision.Text;
            DateTime dFecha = Convert.ToDateTime(DtpFchEmision.Value);
            strFecha = dFecha.ToString().Substring(0, 10);

            string[,] Parametros = new string[5, 2] {{"OficinaConsular",StrNombreOficinaConsular},
                                                     {"Titulo1",StrTitulo1},
                                                     {"Titulo2","(TICKET DEL DIA)"},
                                                     {"FechaEmision",strFecha},
                                                     {"FechaFormato",DtpFchEmision.Text.ToUpper()},
                                                    };

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            dsResult.Clear();

            dsResult.ReadXml(Program.strArchivo);

            FunFunciones.CrystalVisor(Parametros, StrTituloReporte, VRutaArchivo, dsResult, strFecha);
        }

        private void FrmReportesTicket_Load(object sender, EventArgs e)
        {
            this.Text = "Sistema de Colas - Reporte de Ticket Emitidos x Dia";

            // PREGUTAMOS SI LA OFICINA CONSULAR DEL SERVIDOR COINCIDE CON LA OFICINA DE LA BASE DE DATOS
            string StrBDOficinaConsularId = Program.HallarOficinaConsularBD(Program.strArchivo);
            string StrBDOficinaConsularNombre = "";

            if (Program.strServOficinaConsularCodigo != StrBDOficinaConsularId)
            {
                StrBDOficinaConsularNombre = Program.TraeOficinaConsular(StrBDOficinaConsularId, Program.strArchivo);
                MessageBox.Show("La Oficina Consular de la BD es " + StrBDOficinaConsularNombre.ToUpper() + " no coincide con la Oficina Consular Actual " + Program.strServOficinaConsularNombre + ", Inicialice la BD en la opcion [Iniciar BD] del menu [Procesos]", "Sistema de Colas", MessageBoxButtons.OK);
                this.Close();
            }

            DtpFchEmision.Format = DateTimePickerFormat.Custom;
            DtpFchEmision.CustomFormat = "MMM/dd/yyyy";

            RdoOpcion1.Checked = true;
            RdoOpcion1.Focus();
        }

        private void CmdCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmReportesTicket_Activated(object sender, EventArgs e)
        {
        }

        private void DtpFchEmision_ValueChanged(object sender, EventArgs e)
        {
        }

        private void DtpFchEmision_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void DtpFchEmision_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void DtpFchEmision_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}