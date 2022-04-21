using System;
using System.Data;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace SGAC.WinApp
{
    internal static class Program
    {
        public static string strIniLocal = "SGAC.ini";
        public static string strIniServidor = "Colas.ini";
        public static string strTituloSistema = "SISTEMA DE COLAS";
        public static string strUsuario = "";
        public static string strArchivo = "";
        public static string strDominioRuta = "";
        public static string strDominioNombre = "";
        public static string vIP = "";
        public static string vPuerto = "";
        public static string vRutaBDLocal = "";
        public static string vRutaReportes = "";
        public static string vRutaVideos = "";
        public static string strFormatoFecha = "dd/MM/yyyy HH:mm:ss";

        public static string strServOficinaConsularCodigo = "";        // cODIGO DE LA OFICINA CONSULAR DEL SERVIDOR (ticket.ini)
        public static string strServOficinaConsularNombre = "";        // NOMBRE DE LA OFICINA CONSULAR DEL SERVIDOR (ticket.ini)
        public static string strImpresora = "";                        // NOMBRE DE LA IMPRESORA
        public static string strImpresoraTicket = "";                  // NOMBRE DE LA IMPRESORA
        public static string strFuenteNombre = "";                     // NOMBRE DE LA FUENTE PARA EL DISPLAY
        public static int intFuenteTamaño = 0;                         // TAMAÑO DE LA FUENTE PARA EL DISPLAY
        public static int intServOficinaConsularId = 0;                // ID DE LA OFICINA CONSULAR DEL SERVIDOR (ticket.ini)
        public static int intVentanillaId = 0;                         // ID DE LA VENTANILLA ASIGNADA AL CLIENTE
        public static int intVentanillaNumero = 0;                     // NUMERO DE LA VENTANILLA ASIGNADA AL CLIENTE
        public static int intImprimirTicket = 0;
        public static int intTamañoTicketId = 0;                       // ALMACENA EL TAMAÑO DEL TICKET QUE SE ESTA UTILIZANDO
        public static int intTiketeraId = 0;                           // ALMACENA EL ID DE LA TICKETERA
        public static int intUsuarioId = 0;                            // ALMACENA EL ID DE LA TICKETERA

        public static int intEstadoEmitido = 30;                       // ESTADO EMITIDO
        public static int intEstadoAbandonado = 32;                    // ESTADO ABANDONADO
        public static int intEstadoAtendido = 29;                      // ESTADO ATENDIDO
        public static int intEstadoLlamado = 2;                        // ESTADO LLAMADO 
        public static int intEstadoEnAtencion = 3;                     // ESTADO EN ATENCIÓN

        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-PE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-PE");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MDIMenu());
            
            MDIMenu frmMenu = new MDIMenu();
            frmMenu.ShowDialog();
            
            //Application.Run(new FrmLogin());
        }
       
        public static bool DatosBajadosSonCorrectos(DataSet DsDatos)
        {
            bool BolOk = true;
            DataTable DtTabla = new DataTable();
            int IntNumTablas = 0;
            Funciones MisFunciones = new Funciones();
            string StrCadena = "";
            string StrNombreTabla = "";
            string StrNombreArchivo = Environment.CurrentDirectory + "\\testdatos.txt";

            StreamWriter ObjArchivo = new StreamWriter(StrNombreArchivo);

            StrCadena = "                                 REPORTE SISTEMA DE COLAS";
            ObjArchivo.WriteLine(StrCadena);
            StrCadena = "                                 ========================";
            ObjArchivo.WriteLine(StrCadena);
            StrCadena = "";
            ObjArchivo.WriteLine(StrCadena);

            ObjArchivo.WriteLine(StrCadena);

            for (IntNumTablas = 1; IntNumTablas <= 11; IntNumTablas++)
            {
                if (DsDatos.Tables.Contains("Table" + IntNumTablas.ToString()) == false)
                {
                    //if (IntNumTablas == 1) { StrNombreTabla = "Tickets"; }
                    if (IntNumTablas == 2) { StrNombreTabla = "Servicios"; }
                    if (IntNumTablas == 3) { StrNombreTabla = "Perfil de Atencion"; }
                    if (IntNumTablas == 4) { StrNombreTabla = "Televisores"; }
                    if (IntNumTablas == 5) { StrNombreTabla = "Ticketeras"; }
                    if (IntNumTablas == 6) { StrNombreTabla = "Ventanillas"; }
                    if (IntNumTablas == 7) { StrNombreTabla = "Ventanillas Detalle"; }
                    if (IntNumTablas == 8) { StrNombreTabla = "Videos"; }
                    if (IntNumTablas == 9) { StrNombreTabla = "Oficinas Cosulares"; }
                    if (IntNumTablas == 10) { StrNombreTabla = "Parametros"; }
                    if (IntNumTablas == 11) { StrNombreTabla = "Usuarios"; }

                    if (IntNumTablas != 1)
                    {
                        StrCadena = "La tabla " + StrNombreTabla + " no tiene datos";
                        ObjArchivo.WriteLine(StrCadena);
                        BolOk = false;
                    }
                }
            }

            if (BolOk == false)
            {
                StrCadena = "";
                ObjArchivo.WriteLine(StrCadena);
                StrCadena = "Se han encontrado que las tablas mencionadas no contienen registros.";
                ObjArchivo.WriteLine(StrCadena);
                StrCadena = "Para agregar datos a las tablas vaya al menu Configuracion opcion Colas de Atencion";
                ObjArchivo.WriteLine(StrCadena);
                StrCadena = "del sistema WEB SGAC.";
                ObjArchivo.WriteLine(StrCadena);
            }

            ObjArchivo.Close();
            return BolOk;
        }

        public static string TraeOficinaConsular(string StrOficinaConsularCodigo, string StrArchivo)
        {
            DataSet DsLeerFicheroXML = new DataSet();
            DataTable DtResult = new DataTable();
            int A;
            string StrNombreOficina = "";

            DsLeerFicheroXML.Clear();
            DsLeerFicheroXML.ReadXml(StrArchivo);

            DtResult = DsLeerFicheroXML.Tables[9];

            for (A = 0; A <= DtResult.Rows.Count - 1; A++)
            {
                if (DtResult.Rows[A]["ofco_sOficinaConsularId"].ToString() == StrOficinaConsularCodigo)
                {
                    StrNombreOficina = DtResult.Rows[A]["ofco_vNombreNormal"].ToString();
                }
            }
            return StrNombreOficina;
        }

        public static DataSet AbrirArchivoDatos(string StrArchivoDatos)
        {
            DataSet DsLeerFicheroXML = new DataSet();

            DsLeerFicheroXML.ReadXml(StrArchivoDatos);
            return DsLeerFicheroXML;
        }

        public static string HallarOficinaConsularBD(string StrArchivoDatos)
        {
            DataSet DsData = new DataSet();
            DataTable DtTabla = new DataTable();

            DsData = Program.AbrirArchivoDatos(StrArchivoDatos);
            DtTabla = DsData.Tables["table1"];

            return DtTabla.Rows[0]["tick_IOficinaConsularId"].ToString();
        }

        public static int HallarVentanillaId(string StrArchivoDatos, int IntVentanillaNumero)
        {
            DataSet DsData = new DataSet();
            DataTable DtTabla = new DataTable();
            int IntFil = 0;
            int IntOficinaId = 0;

            DsData = Program.AbrirArchivoDatos(StrArchivoDatos);
            DtTabla = DsData.Tables["Table6"];

            for (IntFil = 0; IntFil <= DtTabla.Rows.Count - 1; IntFil++)
            {
                if (Convert.ToInt32(DtTabla.Rows[0]["vent_INumeroOrden"].ToString()) == IntVentanillaNumero)
                {
                    IntOficinaId = Convert.ToInt32(DtTabla.Rows[0]["vent_IVentanillaId"].ToString());
                    break;
                }
            }

            return IntOficinaId;
        }

        public static string UsuarioNombre(string StrArchivoDatos, int intUsuarioId)
        {
            string strNombre = "";
            DataSet DsData = new DataSet();
            DataTable DtTabla = new DataTable();
            int IntFil = 0;
            //int IntOficinaId = 0;

            DsData = Program.AbrirArchivoDatos(StrArchivoDatos);
            DtTabla = DsData.Tables["Table11"];

            for (IntFil = 0; IntFil <= DtTabla.Rows.Count - 1; IntFil++)
            {
                if (Convert.ToInt32(DtTabla.Rows[0]["usua_sUsuarioId"].ToString()) == intUsuarioId)
                {
                    strNombre = DtTabla.Rows[0]["usua_vAlias"].ToString();
                    break;
                }
            }

            return strNombre;
        }

        //-----------------------------------------
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener URL del video YOUTUBE
        //Fecha: 31/01/2018
        //-----------------------------------------
        public static string ObtenerURLYoutube(string StrArchivoDatos, int intYoutubeId)
        {
            string strURLYoutube = "";
            DataSet DsData = new DataSet();
            DataTable DtTabla = new DataTable();
            int IntFil = 0;
            //int IntOficinaId = 0;

            DsData = Program.AbrirArchivoDatos(StrArchivoDatos);
            DtTabla = DsData.Tables["Table8"];

            for (IntFil = 0; IntFil <= DtTabla.Rows.Count - 1; IntFil++)
            {
                if (Convert.ToInt32(DtTabla.Rows[IntFil]["vide_sVideoId"].ToString()) == intYoutubeId)
                {
                    strURLYoutube = DtTabla.Rows[IntFil]["vide_vUrl"].ToString();
                    break;
                }
            }

            return strURLYoutube;
        }

        //-----------------------------------------

        public static int UsuarioVentanilla(string StrArchivoDatos, int intUsuarioId)
        {
            int intVentanillaId2 = 0;
            DataSet DsData = new DataSet();
            DataTable DtTabla = new DataTable();
            int IntFil = 0;

            DsData = Program.AbrirArchivoDatos(StrArchivoDatos);
            DtTabla = DsData.Tables["Table"];

            for (IntFil = 0; IntFil <= DtTabla.Rows.Count - 1; IntFil++)
            {
                if (Convert.ToInt32(DtTabla.Rows[IntFil]["veus_iUsuarioId"].ToString()) == intUsuarioId)
                {
                    intVentanillaId2 = Convert.ToInt32(DtTabla.Rows[IntFil]["veus_iVentanillaId"].ToString());
                    break;
                }
            }

            return intVentanillaId2;
        }

        public static bool CrearDataLocal(string StrRutaArchivo, int IntOficinaConsularId, string SFechaProceso)
        {
            bool BolOk = false;
            DataSet custDS = new DataSet();
            Funciones MiFun = new Funciones();

            try
            {
                //prxIntegrador.ColasAtencionServicio FunDescargaConfiguracion = new prxIntegrador.ColasAtencionServicio();
                prxIntegrador.ColasAtencionClient FunDescargaConfiguracion = new prxIntegrador.ColasAtencionClient();

                string StrNuevOArchivo = "";
                StrNuevOArchivo = FunDescargaConfiguracion.DescargaConfiguracion(IntOficinaConsularId);

                string xmlData = StrNuevOArchivo;
                DataSet DsTablas = new DataSet();
                byte[] buffer = Encoding.UTF8.GetBytes(xmlData);

                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    XmlReader reader = XmlReader.Create(stream);
                    DsTablas.ReadXml(reader);
                }

                if (Program.DatosBajadosSonCorrectos(DsTablas) == true)
                {
                    if (MiFun.ArchivoExiste(StrRutaArchivo) == true)
                    {
                        MiFun.ArchivoBorrar(StrRutaArchivo);
                    }

                    System.IO.StreamWriter xmlSW = new System.IO.StreamWriter(StrRutaArchivo);
                    DsTablas.WriteXml(xmlSW, XmlWriteMode.WriteSchema);
                    xmlSW.Close();
                    BolOk = true;
                }
                else
                {
                    BolOk = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("No se pudo acceder al Host, no se puede inicializar la BD del sistema", "Sistema de Colas", MessageBoxButtons.OK);
                MessageBox.Show("No se pudo acceder al Host, por el siguiente motivo " + ex.Message + "no se puede inicializar la BD del sistema", "Sistema de Colas", MessageBoxButtons.OK);
                BolOk = false;
            }

            return BolOk;
        }

        public static void CargarConstante()
        {
            string StrRutaBD = "";
            string StrRutReportes = "";
            string StrRutVideos = "";
            Funciones MiFun = new Funciones();

            StrRutReportes = MiFun.IniLeer("PREFERENCIAS", "RUTAREPORTE", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            Program.vRutaReportes = StrRutReportes;

            StrRutVideos = MiFun.IniLeer("PREFERENCIAS", "RUTAVIDEO", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            Program.vRutaVideos = StrRutVideos;

            Program.strDominioRuta = MiFun.IniLeer("PREFERENCIAS", "DOMINIORUTA", Environment.CurrentDirectory + "\\" + Program.strIniLocal); ;
            Program.strDominioNombre = MiFun.IniLeer("PREFERENCIAS", "DOMINIONOMBRE", Environment.CurrentDirectory + "\\" + Program.strIniLocal);

            StrRutaBD = MiFun.IniLeer("PREFERENCIAS", "RUTABD", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            Program.vRutaBDLocal = StrRutaBD;

            if (Program.vRutaBDLocal != "")
            {
                Program.strArchivo = StrRutaBD + "\\data.xml";

                Program.intServOficinaConsularId = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "OFICINACONSULAR", Program.vRutaBDLocal + "\\" + Program.strIniServidor));

                //CARGAMOS LO DATOS DE LA OFICINA DEL SERVIDOR ticket.ini
                Program.strServOficinaConsularCodigo = MiFun.IniLeer("PARAMETROS", "OFICINACONSULAR", StrRutaBD + "\\" + Program.strIniServidor);
                Program.strServOficinaConsularNombre = Program.TraeOficinaConsular(Program.strServOficinaConsularCodigo, Program.strArchivo).ToUpper();

                // CARGAMOS LA VARIABLE QUE INDICA SI SE IMPRIMIRAN LOS TICKETS
                if (MiFun.IniLeer("PARAMETROS", "IMPRIMIR", StrRutaBD + "\\" + Program.strIniServidor) == "")
                {
                    Program.intImprimirTicket = 0;
                }
                else
                {
                    Program.intImprimirTicket = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "IMPRIMIR", StrRutaBD + "\\" + Program.strIniServidor));
                }

                // CARGAMOS EL TAMAÑO DEL TICKET
                if (MiFun.IniLeer("PARAMETROS", "TICKETTAMANO", StrRutaBD + "\\" + Program.strIniServidor) == "")
                {
                    Program.intTamañoTicketId = 0;
                }
                else
                {
                    Program.intTamañoTicketId = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "TICKETTAMANO", StrRutaBD + "\\" + Program.strIniServidor));
                }

                // CARGAMOS LA TICKETERA
                if (MiFun.IniLeer("PARAMETROS", "TICKETERA", StrRutaBD + "\\" + Program.strIniServidor) == "")
                {
                    Program.intTiketeraId = 0;
                }
                else
                {
                    Program.intTiketeraId = Convert.ToInt32(MiFun.IniLeer("PARAMETROS", "TICKETERA", StrRutaBD + "\\" + Program.strIniServidor));

                    Funciones ObjFunciones = new Funciones();
                    DataTable dtTicketera = new DataTable();
                    DataView dwTicket = new DataView();
                    DataSet dsResult = new DataSet();

                    dsResult.Clear();

                    dsResult.ReadXml(Program.strArchivo);
                    dtTicketera = dsResult.Tables[5];     // TABLA CL_TICKETERA

                    dwTicket = ObjFunciones.DataViewFiltrar(dtTicketera, "tira_sTicketeraId = " + Program.intTiketeraId + "", "");

                    if (dwTicket.Count != 0)
                    {
                        Program.strImpresoraTicket = dwTicket[0]["tira_vNombre"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No ha indicado el nombre de la ticketera, vaya a la opción [Parametros del Sistema] del menú [Procesos] y asigne una ticketera", "Sistema de Colas", MessageBoxButtons.OK);
                        Program.strImpresoraTicket = "";
                    }
                }

                Program.intVentanillaId = Program.UsuarioVentanilla(Program.vRutaBDLocal + "\\usuarios.xml", Program.intUsuarioId);
            }
            // ***********************
            // ** ARCHIVO INI LOCAL **
            // ***********************

            //// CARGAMOS LA IMPRESORA POR SELECCIONADA
            //if (MiFun.IniLeer("PREFERENCIAS", "IMPRESORA", Environment.CurrentDirectory + "\\" + Program.strIniLocal) == "")
            //{
            //    Program.strImpresora = "";
            //    //IMPRESORA=HP LaserJet M1530 MFP Series PCL 6
            //}
            //else
            //{
            //    Program.strImpresora = MiFun.IniLeer("PREFERENCIAS", "IMPRESORA", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            //}

            //// CARGAMOS EL ID DEL USUARIO ASIGNADO A LA VENTANILLAS
            //if (MiFun.IniLeer("PREFERENCIAS", "USUARIOVENTANILLA", Environment.CurrentDirectory + "\\" + Program.strIniLocal) == "")
            //{
            //    Program.intUsuarioId = 0;
            //}
            //else
            //{
            //    Program.intUsuarioId = Convert.ToInt32(MiFun.IniLeer("PREFERENCIAS", "USUARIOVENTANILLA", Environment.CurrentDirectory + "\\" + Program.strIniLocal));
            //}

            //// CARGAMOS EL NUMERO DE VENTANILLA ASIGNADA AL CLIENTE
            //if (MiFun.IniLeer("PREFERENCIAS", "VENTANILLA", Environment.CurrentDirectory + "\\" + Program.strIniLocal) == "")
            //{
            //    Program.intVentanillaNumero = 0;
            //    Program.intVentanillaId = 0;
            //}
            //else
            //{
            //    Program.intVentanillaId = Convert.ToInt32(MiFun.IniLeer("PREFERENCIAS", "VENTANILLA", Environment.CurrentDirectory + "\\" + Program.strIniLocal)); //Program.HallarVentanillaId(Program.sArchivo, Program.IntVentanillaNumero);
            //}
        }

        //public static bool ValidarUsuario(string strDominio, string strUsuario, string strPassWord)
        //{
        //    string path = string.Format("LDAP://{0}", strDominio);
        //    string domUser = strDominio + @"\" + strUsuario;
        //    DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}",
        //    path), domUser, strPassWord);
        //    entry.AuthenticationType = AuthenticationTypes.ServerBind;
        //    try
        //    {
        //        object nativeObj = entry.NativeObject;
        //        Console.WriteLine(string.Format("Username '{0}' Authenticated",
        //        strUsuario));
        //        return true;
        //    }
        //    catch (COMException e)
        //    {
        //        MessageBox.Show(string.Format("Username '{0}' Not Authenticated:\n{1}",
        //        strUsuario, e.Message));
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(string.Format("Username '{0}' Not Authenticated:\n{1}",
        //        strUsuario, e.Message));
        //    }
        //    return false;
        //}
    }
}