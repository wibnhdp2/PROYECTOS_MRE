using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SGAC.Accesorios;
using System.Collections.Generic;

namespace SGAC.WinApp
{
    public partial class FrmCargaDatos : Form
    {
        private Funciones FuncionesHelper = new Funciones();

        public FrmCargaDatos()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                //Realiza una tarea
                System.Threading.Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
                if (backgroundWorker1.CancellationPending)
                    return;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Notificar el progreso de la tarea
            progressBar1.Value = e.ProgressPercentage;
            lblInfo.Text = e.ProgressPercentage + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            {
                //Realizamos las operaciones que haya que realizar al terminar el progreso
                lblInfo.Text = "Tarea terminada";
                MessageBox.Show("La BD del Sistema de Colas se subio con exito", "Sistema de Colas", MessageBoxButtons.OK);
                this.Close();
                //btnCancelar.Enabled = false;
                CmdInicio.Enabled = true;
                progressBar1.Value = 0;
            }
        }

        private void CmdSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmdInicio_Click(object sender, EventArgs e)
        {
            CmdInicio.Enabled = false;
            CmdInicio.BackColor = Color.Gray;
            Application.DoEvents();

            prxIntegrador.Ticket[] LisTicket = new prxIntegrador.Ticket[1];

            DataSet dsLeerFicheroXML = new DataSet();
            DataTable DtResult = new DataTable();
            string StrArchivo = Program.strArchivo;
            int A;

            Cursor.Current = Cursors.WaitCursor;
            dsLeerFicheroXML.Clear();
            dsLeerFicheroXML.ReadXml(StrArchivo);

            DtResult = dsLeerFicheroXML.Tables[1];                                    // TABLA CL_TICKET

            
            try
            {
                var dt_filtro = (from dt in DtResult.AsEnumerable()
                                 where Convert.ToInt32(dt["tick_bCargado"]) == 0
                                 select dt).CopyToDataTable();


                DtResult = dt_filtro.Copy();
                dt_filtro.Dispose();
            }
            catch
            {
                DtResult = new DataTable();
            }

            //List<Ticket> tickets=new List<Ticket>();
            // DETERMINAMOS EL NUMERO DE TICKETS Y REDIMENSIONAMOS EL ARRAY DE TICKETS
            int iNUmeroTickets = DtResult.Rows.Count;
            Array.Resize(ref LisTicket, iNUmeroTickets);


            if (DtResult.Columns.Count > 0)
            {
                if (DtResult.Rows.Count != 0)
                {
                    progressBar1.Maximum = DtResult.Rows.Count - 1;

                    for (A = 0; A <= DtResult.Rows.Count - 1; A++)
                    {
                        //BE.CL_TICKET Ticket = new BE.CL_TICKET();
                        prxIntegrador.Ticket Ticket = new prxIntegrador.Ticket();

                        progressBar1.Value = A;

                        Ticket.tick_sTipoServicioId = Convert.ToInt16(DtResult.Rows[A]["tick_sTipoServicioId"].ToString());
                        Ticket.tick_iPersonalId = Convert.ToInt32(DtResult.Rows[A]["tick_iPersonalId"].ToString());
                        Ticket.tick_iNumero = Convert.ToInt32(DtResult.Rows[A]["tick_iNumero"].ToString());


                        Ticket.tick_dFechaHoraGeneracion = "";
                        if (DtResult.Rows[A]["tick_dFechaHoraGeneracion"].ToString() != "")
                        {
                            Ticket.tick_dFechaHoraGeneracion = DtResult.Rows[A]["tick_dFechaHoraGeneracion"].ToString();
                        }

                        Ticket.tick_dAtencionInicio = "";
                        if (!String.IsNullOrEmpty(DtResult.Rows[A]["tick_dAtencionInicio"].ToString()))
                        {
                            Ticket.tick_dAtencionInicio = Fecha.ConvertirFecha(Convert.ToDateTime(DtResult.Rows[A]["tick_dAtencionInicio"]));
                        }

                        Ticket.tick_dAtencionFinal = "";
                        if (!String.IsNullOrEmpty(DtResult.Rows[A]["tick_dAtencionFinal"].ToString()))
                        {
                            Ticket.tick_dAtencionFinal = Fecha.ConvertirFecha(Convert.ToDateTime(DtResult.Rows[A]["tick_dAtencionFinal"]));
                        }

                        Ticket.tick_sPrioridadId = Convert.ToInt16(DtResult.Rows[A]["tick_sPrioridadId"].ToString());
                        Ticket.tick_sTipoCliente = Convert.ToInt16(DtResult.Rows[A]["tick_sTipoCliente"].ToString());
                        Ticket.tick_sTamanoTicket = Convert.ToInt16(DtResult.Rows[A]["tick_sTamanoTicket"].ToString());
                        Ticket.tick_sTipoEstado = Convert.ToInt16(DtResult.Rows[A]["tick_sTipoEstado"].ToString());
                        Ticket.tick_sTicketeraId = Convert.ToInt16(DtResult.Rows[A]["tick_sTicketeraId"].ToString());
                        Ticket.tick_vLLamada = DtResult.Rows[A]["tick_vLLamada"].ToString();
                        Ticket.tick_sUsuarioAtendio = Convert.ToInt16(DtResult.Rows[A]["tick_sUsuarioAtendio"].ToString());
                        Ticket.tick_cEstado = DtResult.Rows[A]["tick_cEstado"].ToString();
                        Ticket.tick_sUsuarioCreacion = Convert.ToInt16(DtResult.Rows[A]["tick_sUsuarioCreacion"].ToString());
                        Ticket.tick_vIPCreacion = DtResult.Rows[A]["tick_vIPCreacion"].ToString();

                        Ticket.tick_dFechaCreacion = "";
                        if (DtResult.Rows[A]["tick_dFechaCreacion"].ToString() != "")
                        {
                            Ticket.tick_dFechaCreacion = DtResult.Rows[A]["tick_dFechaCreacion"].ToString();
                        }

                        Ticket.tick_sUsuarioModificacion = Convert.ToInt16(DtResult.Rows[A]["tick_sUsuarioModificacion"].ToString());

                        Ticket.tick_vIPModificacion = DtResult.Rows[A]["tick_vIPModificacion"].ToString();

                        Ticket.tick_dFechaModificacion = "";
                        if (DtResult.Rows[A]["tick_dFechaModificacion"].ToString() != "")
                        {
                            Ticket.tick_dFechaModificacion = DtResult.Rows[A]["tick_dFechaModificacion"].ToString();
                        }

                        if (DtResult.Rows[A]["tick_sVentanillaId"].ToString() != "")
                        {
                            Ticket.tick_sVentanillaId = Convert.ToInt16(DtResult.Rows[A]["tick_sVentanillaId"].ToString());
                        }
                        else
                        {
                            Ticket.tick_sVentanillaId = 0;
                        }
                        LisTicket[A] = Ticket;

                        lblInfo.Text = (A + 1).ToString();
                        lblInfo.Refresh();
                    }

                    if (SubirDatos(LisTicket) == true)
                    {
                        for (A = 1; A <= DtResult.Rows.Count; A++)
                        {
                            string[,] Valores = new string[2, 3] {
                                                            { "tick_iTicketId", A.ToString(), "C" },
                                                            { "tick_bCargado","1", "" },
                                                          };

                            FuncionesHelper.XMLModificarNodo(StrArchivo, "Table1", Valores);
                        }

                        MessageBox.Show("Los tickets se subieron con éxito", "Colas de Atención", MessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron subir los Tickets, Asegurese de haber descargado la ultima version de la Base de Datos para este Consulado", "Sistema de Colas", MessageBoxButtons.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay Ticket para subir a la BD", "Sistema de Colas", MessageBoxButtons.OK);
            }
            Cursor.Current = Cursors.Arrow;
        }

        private bool SubirDatos(prxIntegrador.Ticket[] objTicketLista)
        {
            bool booOk = true;
            prxIntegrador.ColasAtencionClient SubirTicket = new prxIntegrador.ColasAtencionClient();
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                int intServOficinaConsularId = Program.intServOficinaConsularId;

                if (SubirTicket.CargaInformacion(objTicketLista, intServOficinaConsularId) == false)      // SI EL SERVICIO DEVUELVE FALSE, SIGNIFICA QUE HA OCURRIDO UN ERROR EN LA CLASE
                {
                    booOk = false;
                }
                Cursor.Current = Cursors.Arrow;
            }
            catch
            {
                // DEVOLVEMOS FALSO SI OCURRIO CUALQUIER ERROR
                MessageBox.Show("No se pudo acceder al Host, no se puede subir los Tickets", "Sistema de Colas", MessageBoxButtons.OK);
                booOk = false;
            }
            return booOk;
        }

        private void FrmSubirDatos_Load(object sender, EventArgs e)
        {
            this.Text = Program.strTituloSistema + " - " + "Subir BD al Servidor";
            lblInfo.Text = "";
        }
    }
}