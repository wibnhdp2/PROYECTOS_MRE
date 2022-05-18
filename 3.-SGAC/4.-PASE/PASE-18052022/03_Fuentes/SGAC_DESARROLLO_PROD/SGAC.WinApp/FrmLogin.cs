using System;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Windows.Forms;

namespace SGAC.WinApp
{    

    public partial class FrmLogin : Form
    {
        private int intNumeroOportunidades = 0;                 // ALAMCENA EL NUMERO DE OPORTUNIDADES PARA EVALUAR LAS CLAVE DEL USUARIO
        public string strUsuario = "";


        public FrmLogin()
        {
            InitializeComponent();
        }

        private void CmdAceptar_Click(object sender, EventArgs e)
        {
            # region VALIDAMOS QUE LOS DATOS NECESARIOS SE INGRESARON
            if (TxtUsuario.Text == "")
            {
                MessageBox.Show("No ha ingresado el login del usuario", Program.strTituloSistema, MessageBoxButtons.OK);
                TxtUsuario.Focus();
                return;
            }
            
            #endregion

            if (BuscarUsuario(TxtUsuario.Text) == true)
            {
                Program.intUsuarioId = HallarIdusuario(TxtUsuario.Text);
                Program.CargarConstante();
                Program.strUsuario = TxtUsuario.Text;
                
                this.Close();
            }
            else
            {
                intNumeroOportunidades = intNumeroOportunidades + 1;
                MessageBox.Show("Datos del usuario no validos, vuelva a ingresar correctamente los datos ", Program.strTituloSistema, MessageBoxButtons.OK);
                TxtUsuario.Text = "";
                TxtUsuario.Focus();

                if (intNumeroOportunidades == 3)
                {
                    MessageBox.Show("Se agotaron el número de oportunidades, se abandonara el inicio de la aplicación", Program.strTituloSistema, MessageBoxButtons.OK);
                    this.Close();
                    Application.Exit();
                }
            }
        }

        private bool BuscarUsuario(string strUsuario)
        {
            bool boolOk = false;

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            int intFila = 0;

            dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\data.xml");

            if (dsResult.Tables.Count == 0)
            {
                do
                {
                    dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\data.xml");

                } while (dsResult.Tables.Count == 0);
            }


            dtResult = dsResult.Tables[11];

            for (intFila = 0; intFila <= dtResult.Rows.Count - 1; intFila++)
            {
                if (dtResult.Rows[intFila]["usua_vAlias"].ToString().ToUpper() == strUsuario.ToUpper())
                {
                    boolOk = true;
                    break;
                }
            }

            return boolOk;
        }

        private int HallarIdusuario(string strUsuario)
        {
            int intUsuarioId = 0;
            int intFila = 0;
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\data.xml");

            if (dsResult.Tables.Count == 0)
            {
                do
                {
                    dsResult = Program.AbrirArchivoDatos(Program.vRutaBDLocal + @"\data.xml");

                } while (dsResult.Tables.Count == 0);
            }


            dtResult = dsResult.Tables[11];

            for (intFila = 0; intFila <= dtResult.Rows.Count - 1; intFila++)
            {
                if (dtResult.Rows[intFila]["usua_vAlias"].ToString().ToUpper() == strUsuario.ToUpper())
                {
                    intUsuarioId = Convert.ToInt32(dtResult.Rows[intFila]["usua_sUsuarioId"].ToString());
                }
            }

            return intUsuarioId;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            Funciones MiFun = new Funciones();

            TxtUsuario.Text = "";
            this.Refresh();
            groupBox1.Refresh();

            string strVersion = MiFun.ObtenerVersionAplicacion();
            this.Text = "SGAC - Colas de Atencion       " + strVersion;

            Program.vRutaBDLocal = MiFun.IniLeer("PREFERENCIAS", "RUTABD", Environment.CurrentDirectory + "\\" + Program.strIniLocal);
            TxtUsuario.Focus();
        }

        private void CmdCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.intUsuarioId == 0) { Application.Exit(); }
        }


        private void TxtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Aceptar automático al presionar la tecla enter
            if (e.KeyChar == 13)
            {
                string strUsuario;
                strUsuario = TxtUsuario.Text;
                TxtUsuario.Text = strUsuario.ToUpper();
                CmdAceptar_Click(sender, e);
            }
        }

        

        private void TxtUsuario_Validating(object sender, CancelEventArgs e)
        {
            if (TxtUsuario.Text == "")
            {
                return;
            }            
            CmdAceptar.Focus();            
        }

        private void FrmLogin_Activated(object sender, EventArgs e)
        {
            TxtUsuario.Focus();
        }

        //  FUNCIONA PARA VALIDAR LOS USUARIOS DEL ACTIVE DIRECTORY
        private bool ValidarUsuariosActiveDirectory(string strUsuario, string strPassword)
        {
            try
            {
                string username = strUsuario;
                string pwd = strPassword;

                string strPath = Program.strDominioRuta;
                string strDomain = Program.strDominioNombre;

                string domainAndUsername = strDomain + @"\" + username;
                DirectoryEntry entry = new DirectoryEntry(strPath, domainAndUsername, pwd);
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "name=" + username;
                SearchResultCollection results = search.FindAll();
                foreach (SearchResult resultados in results)
                {
                    ResultPropertyCollection colProperties = resultados.Properties;
                    foreach (string key in colProperties.PropertyNames)
                    {
                        foreach (object value in colProperties[key])
                        {
                            //Response.Write("" + key.ToString() + ": " + value + "");
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

// Colores del texto usuario
        private void TxtUsuario_Enter(object sender, EventArgs e)
        {
            TxtUsuario.BackColor = Color.FromArgb(255, 255, 225);
        }

        private void TxtUsuario_Leave(object sender, EventArgs e)
        {
            TxtUsuario.BackColor = Color.White;
        }

       
    }
}