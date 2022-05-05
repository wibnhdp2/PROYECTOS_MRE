using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace SGAC.Accesorios
{
    public class Globalizacion
    {
        public static void CrearIdiomaXML(string Idioma,string NombreArchivoXML)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand("PS_ACCESORIOS.USP_AC_GLOBALIZACION_CONSULTAR", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            XmlTextWriter writer = new XmlTextWriter(NombreArchivoXML, System.Text.Encoding.UTF8);

            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            writer.WriteStartElement("COMMANDS_APP");

            writer.WriteStartElement("Comandos");

            while (reader.Read())
            {
                string Cadena = reader[Idioma].ToString();
                string NomComando = reader["glob_vNomComando"].ToString();
                writer.WriteStartElement(NomComando);
                writer.WriteString(Cadena);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            reader.Close();
            conn.Close();
        }

        public static void ObtenerIdiomaMenu(string NombreArchivoXML, Page Page, Menu NavigationMenu)
        {
            string cadena = "";

            XmlDocument documento = new XmlDocument();
            documento.Load(NombreArchivoXML);

            XmlNodeList xnList = documento.SelectNodes("COMMANDS_APP/Comandos");


            foreach (MenuItem mit in NavigationMenu.Items)
            {
                cadena = mit.Value;
                mit.Text = RecorreMenuXML(cadena, documento, xnList, Page);

                foreach (MenuItem mit1 in mit.ChildItems)
                {
                    cadena = mit1.Value;
                    mit1.Text = RecorreMenuXML(cadena, documento, xnList, Page); 

                    foreach (MenuItem mit2 in mit1.ChildItems)
                    {
                        cadena = mit2.Value;
                        mit2.Text = RecorreMenuXML(cadena, documento, xnList, Page); 

                        foreach (MenuItem mit3 in mit2.ChildItems)
                        {
                            cadena = mit3.Value;
                            mit3.Text = RecorreMenuXML(cadena, documento, xnList, Page); 
                        }

                    }
                }

            }
        }

        public static void ObtenerIdiomaFormulario(string NombreArchivoXML,MasterPage Master)
        {
            XmlDocument documento = new XmlDocument();
            documento.Load(NombreArchivoXML);
            {
                XmlNodeList xnList = documento.SelectNodes("COMMANDS_APP/Comandos");
                foreach (XmlNode xn in xnList)
                {
                    string Todo = xn.InnerXml;
                    string FF = xn.Attributes.ToString();
                    string TodoXML = documento.LastChild.InnerText;
                    string NomEti = "";
                    string NomCom = "";
                    bool encontroBtnCtrl = false;


                    ContentPlaceHolder contentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                    foreach (Control control in contentPlaceHolder.Controls)
                    {
                        if (!control.GetType().ToString().EndsWith("LiteralControl"))
                        {
                            string ver = control.ClientID;
                            XmlNode child = documento.SelectSingleNode("/COMMANDS_APP/Comandos");

                            if (child != null)
                            {
                                XmlNodeReader nr = new XmlNodeReader(child);
                                string controlID = control.ClientID.Substring(12, control.ClientID.Length - 12);
                                while (nr.Read())

                                    switch (nr.NodeType)
                                    {
                                        case XmlNodeType.Element:
                                            while (nr.MoveToNextAttribute())
                                                if (nr.Name == controlID)
                                                {
                                                    NomCom = (nr.Name);
                                                }
                                            break;

                                        case XmlNodeType.Text:
                                            NomEti = (nr.Value);
                                            if (nr.ToString() == controlID)
                                            {
                                            }
                                            break;

                                        case XmlNodeType.EndElement:

                                            if (ver.Contains("ToolBar"))
                                            {
                                                UserControl uc = (UserControl)control;

                                                foreach (Control c in uc.Controls)
                                                {
                                                    encontroBtnCtrl = false;

                                                    if (c is System.Web.UI.HtmlControls.HtmlTableCell)
                                                    {

                                                        foreach (Control c3 in c.Controls)
                                                        {

                                                            if (c3 is Button)
                                                            {
                                                                controlID = c3.ClientID.Substring(25, c3.ClientID.Length - 25);//

                                                                if (nr.Name == controlID)
                                                                {
                                                                    Button myLabel1 = (Button)c3;
                                                                    myLabel1.Text = NomEti;
                                                                    encontroBtnCtrl = true;
                                                                    break;
                                                                }

                                                            }
                                                        }
                                                    }

                                                    if (encontroBtnCtrl)
                                                        break;
                                                }
                                            }

                                            else if (nr.Name == controlID)
                                            {
                                                string valores = nr.Name;
                                                string comando = controlID/*control.ClientID*/;
                                                Control myControl1 = contentPlaceHolder.FindControl(comando);

                                                if (myControl1 is Label)
                                                {
                                                    Label myLabel1 = (Label)myControl1;
                                                    myLabel1.Text = NomEti;
                                                }

                                                else if (myControl1 is TextBox)
                                                {
                                                    TextBox myLabel1 = (TextBox)myControl1;
                                                    myLabel1.Text = NomEti;
                                                }

                                                else if (myControl1 is Button)
                                                {
                                                    Button myLabel1 = (Button)myControl1;
                                                    myLabel1.Text = NomEti;
                                                }

                                                else if (myControl1 is GridView)
                                                {
                                                    GridView myHeader = (GridView)myControl1;
                                                    string text = NomEti;
                                                    string[] Arreglo = text.Split('|');
                                                    int Cabeceras = Arreglo.Count();
                                                    int Colo = myHeader.Columns.Count;
                                                    int cont = 0;

                                                    foreach (string item in Arreglo)
                                                    {
                                                        myHeader.Columns[cont].HeaderText = item;
                                                        cont = cont + 1;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                            }
                        }
                    }
                }

            }

        }

        private static string RecorreMenuXML(string cadena, XmlDocument documento, XmlNodeList xnList,Page Page)
        {

            foreach (XmlNode xn in xnList)
            {
                string Todo = xn.InnerXml;
                string FF = xn.Attributes.ToString();
                string TodoXML = documento.LastChild.InnerText;
                string NomEti = "";
                string NomCom = "";

                foreach (Control control in Page.Form.Controls)
                {
                    if (!control.GetType().ToString().EndsWith("LiteralControl"))
                    {
                        XmlNode child = documento.SelectSingleNode("/COMMANDS_APP/Comandos");


                        if (child != null)
                        {
                            XmlNodeReader nr = new XmlNodeReader(child);
                            while (nr.Read())
                                switch (nr.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        while (nr.MoveToNextAttribute())
                                            if (nr.Name == cadena)
                                            {
                                                NomCom = (nr.Name);
                                            }
                                        break;

                                    case XmlNodeType.Text:
                                        NomEti = (nr.Value);
                                        if (nr.ToString() == cadena)
                                        {
                                        }
                                        break;

                                    case XmlNodeType.EndElement:
                                        if (nr.Name == cadena)
                                        {
                                            string valores = nr.Name;
                                            //mit.Text = NomEti ;
                                            return NomEti;
                                        }
                                        break;
                                }
                        }
                    }
                }

            }
            return "";

        }

    }
}

