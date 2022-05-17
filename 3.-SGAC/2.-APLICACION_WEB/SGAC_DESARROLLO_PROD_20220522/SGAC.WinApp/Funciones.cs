using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Reporting.WinForms;

namespace SGAC.WinApp
{
    public class Funciones
    {
        public int intErrorNumero = 0;
        public string strErrorMensaje = "";

        public int IntCrystalVisorAnchoFormulario = 0;
        public int IntCrystalVisorAltoFormulario = 0;
        public static string strMensajeError = "";

        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(string ApplicationName, string KeyName, string StrValue, string FileName);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string ApplicationName, string KeyName, string DefaultValue, StringBuilder ReturnString, int nSize, string FileName);

        private InstalledFontCollection installedFonts = new InstalledFontCollection();

        #region IniEscribir

        //****************************************************************************************************************
        //** NOMBRE      : IniEscribir
        //** TIPO        : Funcion
        //** DESCRIPCION : Escribe una fila en un archivo INI
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrSectionName     | Indica el nombre de la seccion que se va a escribir
        //**               string       | StrKeyName         | Indica el nombre del Key que se va escribir
        //**               string       | StrKeyValue        | Indica el valor del Key que se va escribir
        //**               string       | StrFileName        | Indica el nombre del archivo INI que se va a escribir
        //** DEVUELVE    : string
        //****************************************************************************************************************
        public void IniEscribir(string StrSectionName, string StrKeyName, string StrKeyValue, string StrFileName)
        {
            WritePrivateProfileString(StrSectionName, StrKeyName, StrKeyValue, StrFileName);
        }

        #endregion IniEscribir

        #region IniLeer

        //****************************************************************************************************************
        //** NOMBRE      : IniLeer
        //** TIPO        : Funcion
        //** DESCRIPCION : Lee una fila del archivo ini
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrSectionName     | Indica el nombre de la seccion que se va a leer
        //**               string       | StrKeyName         | Indica el nombre del Key que se va leer
        //**               string       | StrFileName        | Indica el nombre del archivo INI que se va a leer
        //** DEVUELVE    : string
        //****************************************************************************************************************
        public string IniLeer(string StrSectionName, string StrKeyName, string StrFileName)
        {
            StringBuilder szStr = new StringBuilder(255);
            GetPrivateProfileString(StrSectionName, StrKeyName, "", szStr, 255, StrFileName);
            return szStr.ToString().Trim();
        }

        #endregion IniLeer

        #region XMLContarRegistros

        //****************************************************************************************************************
        //** NOMBRE      : XMLContarRegistros
        //** TIPO        : Funcion
        //** DESCRIPCION : Cuentra los registros de una tabla XML
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrArxhivoXML      | Indica la ruta del archivo XML
        //**               string       | StrTabla           | Indica el nombre de la tabla a contar
        //**               string       | StrCampo           | Indica el nombre del campo a contar
        //** DEVUELVE    : int
        //****************************************************************************************************************
        public int XMLContarRegistros(string StrArxhivoXML, string StrTabla, string StrCampo)
        {
            //where prod.Element("tick_dFechaHoraGeneracion").Value.Substring(0, 10) == "18/09/2014"
            XDocument xmlDoc = XDocument.Load(StrArxhivoXML);
            var xmlValue = (from prod in xmlDoc.Descendants(StrTabla)
                            select
                            (
                                        int.Parse(prod.Element(StrCampo).Value)
                            )).Count();

            return Convert.ToInt32(xmlValue);
        }

        #endregion XMLContarRegistros

        #region XMLMaximoRegistro

        //****************************************************************************************************************
        //** NOMBRE      : XMLMaximoRegistro
        //** TIPO        : Funcion
        //** DESCRIPCION : Halla el valor maximo de un campo en una tabla XML
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrArxhivoXML      | Indica la ruta del archivo XML
        //**               string       | StrTabla           | Indica el nombre de la tabla a contar
        //**               string       | StrCampo           | Indica el nombre del campo a contar
        //** DEVUELVE    : int
        //****************************************************************************************************************
        public int XMLMaximoRegistro(string StrArxhivoXML, string StrTabla, string StrCampo)
        {
            //where prod.Element("tick_dFechaHoraGeneracion").Value.Substring(0, 10) == "18/09/2014"
            XDocument xmlDoc = XDocument.Load(StrArxhivoXML);
            var xmlValue = (from prod in xmlDoc.Descendants(StrTabla)
                            select
                            (
                                        int.Parse(prod.Element(StrCampo).Value)
                            )).Max();

            return Convert.ToInt32(xmlValue);
        }

        #endregion XMLMaximoRegistro

        #region XMLMaximoRegistroTicket

        //****************************************************************************************************************
        //** NOMBRE      : XMLMaximoRegistro
        //** TIPO        : Funcion
        //** DESCRIPCION : Halla el valor maximo de un campo en una tabla XML
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrArxhivoXML      | Indica la ruta del archivo XML
        //**               string       | StrTabla           | Indica el nombre de la tabla a contar
        //**               string       | StrCampo           | Indica el nombre del campo a contar
        //**               string       | StrFechaEmision    | Indica el nombre del campo a contar
        //** DEVUELVE    : int
        //****************************************************************************************************************
        public int XMLMaximoRegistroTicket(string StrArxhivoXML, string StrTabla, string StrCampo, string StrFechaEmision)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(StrArxhivoXML);
                var xmlValue = (from prod in xmlDoc.Descendants(StrTabla)
                                where prod.Element("tick_dFechaHoraGeneracion").Value.Substring(0, 8) == StrFechaEmision.Substring(0, 8)
                                select
                                (
                                            int.Parse(prod.Element(StrCampo).Value)
                                )).Max();
                return Convert.ToInt32(xmlValue);
            }
            catch
            {
                return 0;
            }
            //return Convert.ToInt32(xmlValue);
        }

        #endregion XMLMaximoRegistroTicket

        #region XMLMinimoRegistro

        //****************************************************************************************************************
        //** NOMBRE      : XMLMinimoRegistro
        //** TIPO        : Funcion
        //** DESCRIPCION : Halla el valor minimo de un campo en una tabla XML
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrArxhivoXML      | Indica la ruta del archivo XML
        //**               string       | StrTabla           | Indica el nombre de la tabla a contar
        //**               string       | StrCampo           | Indica el nombre del campo a contar
        //** DEVUELVE    : int
        //****************************************************************************************************************
        public int XMLMinimoRegistro(string StrArxhivoXML, string StrTabla, string StrCampo)
        {
            //where prod.Element("tick_dFechaHoraGeneracion").Value.Substring(0, 10) == "18/09/2014"
            XDocument xmlDoc = XDocument.Load(StrArxhivoXML);
            var xmlValue = (from prod in xmlDoc.Descendants(StrTabla)
                            select
                            (
                                        int.Parse(prod.Element(StrCampo).Value)
                            )).Min();

            return Convert.ToInt32(xmlValue);
        }

        #endregion XMLMinimoRegistro

        #region ArchivoBorrar

        /// <summary>
        /// Borra fisicamente un archivo del disco duro
        /// </summary>
        /// <param name="StrRutaArchivo">Indica la ruta del archivo a borrar</param>
        /// <returns>bool</returns>
        public bool ArchivoBorrar(string StrRutaArchivo)
        {
            bool BolExiste = false;

            try
            {
                File.Delete(StrRutaArchivo);
                BolExiste = true;
                return BolExiste;
            }
            catch
            {
                return BolExiste;
            }
        }

        #endregion ArchivoBorrar

        #region ArchivoExiste

        //****************************************************************************************************************
        //** NOMBRE      : ArchivoExiste
        //** TIPO        : Funcion
        //** DESCRIPCION : Busca un archivo en el disco duro
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrRutaArchivo     | Indica la ruta el archivo que se buscara
        //** DEVUELVE    : bool
        //****************************************************************************************************************
        public bool ArchivoExiste(string StrRutaArchivo)
        {
            bool BolExiste;
            BolExiste = File.Exists(StrRutaArchivo);

            return BolExiste;
        }

        #endregion ArchivoExiste

        #region XMLAgregarNodo

        //****************************************************************************************************************
        //** NOMBRE      : XMLAgregarNodo
        //** TIPO        : Funcion
        //** DESCRIPCION : Agrega un nodo al archivo XML indicado
        //** PARAMETROS  :
        //**               TIPO        | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string      | StrArchivoXML      | Archivo XML a procesar
        //**               string      | StrNombreTabla     | Nombre de la tabla dentro del archivo XML
        //**               object[]    | ObjValores         | Array de objetos con los campos que se ingresaran, los objetos
        //**                                                  deben de ser del tipo xElement
        //** EJEMPLO     :
        //**               object[] MisCampos = new object[4] { new XElement("tick_ITicketId", "0"),
        //**                                                    new XElement("tick_IOficinaConsularId","1"),
        //**                                                    new XElement("tick_ITipoServicioId", "1"),
        //**                                                    new XElement("tick_IPersonalId", "1"),
        //**                                                  };
        //**               bOk = MiFun.XMLAgregarNodo(Program.sArchivo, "Table1", MisCampos);
        //**               if (bOk == true) {  MessageBox.Show("Se Agrego correctamente"); }
        //**
        //** DEVUELVE    : bool
        //****************************************************************************************************************
        public bool XMLAgregarNodo(string StrArchivoXML, string StrNombreTabla, object[] ObjValores)
        {
            bool BolbOk = false;

            try
            {
                if (ArchivoBloqueado(StrArchivoXML) == true)
                {
                    do
                    {
                        //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
                    } while (ArchivoBloqueado(StrArchivoXML) == true);
                    //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
                }

                MemoryStream memStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memStream, Encoding.UTF8);

                XDocument doc = XDocument.Load(StrArchivoXML);  //load the xml file.

                IEnumerable<XElement> oMemberList = doc.Element("NewDataSet").Elements(StrNombreTabla);

                var oMember = new XElement(StrNombreTabla, ObjValores);

                oMemberList.Last().AddAfterSelf(oMember);  //add node to the last element.
                doc.Save(StrArchivoXML);
                BolbOk = true;
            }
            catch
            {
                return false;
            }

            return BolbOk;
        }

        #endregion XMLAgregarNodo

        #region XMLEliminarNodo

        //****************************************************************************************************************
        //** NOMBRE      : XMLEliminarNodo
        //** TIPO        : Funcion
        //** DESCRIPCION : Elimina un nodo del archivo XML indicado
        //** PARAMETROS  :
        //**               TIPO        | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string      | StrArchivoXML      | Archivo XML a procesar
        //**               string      | StrNombreTabla     | Nombre de la tabla dentro del archivo XML
        //**               string      | StrCampoCondicion  | Nombre del campo de la tabla para evaluar condicion
        //**               string      | StrValorCondicion  | Valor para evaluar condicion
        //** DEVUELVE    : bool
        //****************************************************************************************************************
        public bool XMLEliminarNodo(string StrArchivoXML, string StrNombreTabla, string StrCampoCondicion, string StrValorCondicion)
        {
            bool BolbOk = false;

            try
            {
                if (ArchivoBloqueado(StrArchivoXML) == true)
                {
                    do
                    {
                        //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
                    } while (ArchivoBloqueado(StrArchivoXML) == true);
                    //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
                }

                XDocument document = XDocument.Load(StrArchivoXML);

                var deleteQuery = from r in document.Descendants(StrNombreTabla) where r.Element(StrCampoCondicion).Value == StrValorCondicion select r;

                foreach (var query in deleteQuery)
                {
                    deleteQuery.First().Remove();
                    break;
                }

                document.Save(StrArchivoXML);

                BolbOk = true;
            }
            catch
            {
                return false;
            }
            return BolbOk;
        }

        #endregion XMLEliminarNodo

        public bool ArchivoBloqueado(string filepath)
        {
            try
            {
                FileStream oStream = new FileStream(filepath, FileMode.Open, FileAccess.Write, FileShare.None);
                oStream.Close();
            }
            catch
            {
                return true;
            }
            return false;
        }

        #region XMLModificarNodo

        //********************************************************************************************************************************************
        //** NOMBRE      : XMLModificarNodo
        //** TIPO        : Funcion
        //** DESCRIPCION : Permite modificar un nodo de un archivo XML
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrArchivoXML      | Nombre del archivo XML que se modificara
        //**               string       | StrNombreTabla     | Nombre de la tabla que se modificara
        //**               string[,]    | StrValores         | Array de campos a modificar, el campo condicionante se indicara
        //**                                                   agregando la letra "C" en el 3 indice del array
        //**                                                   Columna 0 = Indica el nombre del campo
        //**                                                   Columna 1 = Indica el valor a modificar o a encontrar (esto depende de la columna 2)
        //**                                                   Columna 2 = Indica si el campo sera de busqueda o de actualizacion
        //**                                                               "C" = indica que el campo sera de Busqueda        "" = Indica que el campo se actualizara
        //** EJEMPLO     :
        //**               string[,] Valores = new string[2, 3] {
        //**                                                        { "tick_ITicketId", "001", "C" },
        //**                                                        { "tick_ITipoEstado", "99", "" },
        //""                                                    };
        //** DEVUELVE    : bool
        //********************************************************************************************************************************************
        public bool XMLModificarNodo(string StrArchivoXML, string StrNombreTabla, string[,] StrValores)
        {
            bool bOk = false;
            int A;
            int IntNumeroElementos = 0;
            string StrCampoWhere = "";
            string StrValorWhere = "";

            try
            {

                if (ArchivoBloqueado(StrArchivoXML) == true)
                {
                    do
                    {
                        //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
                    } while (ArchivoBloqueado(StrArchivoXML) == true);
                    //MessageBox.Show("El tiempo de espera para registrar este proceso ha culminado, haga clic en aceptar para continuar");
                }
                XElement xmlContactos = XElement.Load(StrArchivoXML);

                IntNumeroElementos = Convert.ToInt16(StrValores.GetLongLength(0));
                // OBTENEMOS LOS DATOS PARA HACER EL WHERE EN EL REGISTRO
                for (A = 0; A <= IntNumeroElementos - 1; A++)
                {
                    if (StrValores[A, 2].ToString() == "C")
                    {
                        StrCampoWhere = StrValores[A, 0].ToString();
                        StrValorWhere = StrValores[A, 1].ToString();
                    }
                }

                // APLICAMOS LA CONDICION DE BUSQUEDA
                var Condicion = (from c in xmlContactos.Descendants(StrNombreTabla)
                                 where c.Element(StrCampoWhere).Value.ToUpper() == StrValorWhere
                                 select c).FirstOrDefault();

                if (Condicion != null)
                {
                    //ACTUALIZAMOS LOS DATOS QUE HAYA QUE ACTUALIZAR
                    for (A = 0; A <= IntNumeroElementos - 1; A++)
                    {
                        if (StrValores[A, 2].ToString() != "C")
                        {
                            StrCampoWhere = StrValores[A, 0].ToString();
                            StrValorWhere = StrValores[A, 1].ToString();

                            Condicion.SetElementValue(StrCampoWhere, StrValorWhere);
                        }
                    }
                }

                xmlContactos.Save(StrArchivoXML);
                bOk = true;
            }

            catch
            {
                return false;
            }
            return bOk;
        }

        #endregion XMLModificarNodo

        #region ConvertirHorasDecimal

        //****************************************************************************************************************
        //** NOMBRE      : ConvertirHorasDecimal
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte una hora a decimal
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrHoraCadena      | Hora a convertir en formato cadena
        //** DEVUELVE    : Double
        //****************************************************************************************************************
        //public Double ConvertirHorasDecimal(string StrHoraCadena)
        //{
        //    decimal DecHoraDecimal;

        //    DecHoraDecimal = Convert.ToDecimal(TimeSpan.Parse(StrHoraCadena).TotalHours);
        //    return Convert.ToDouble(DecHoraDecimal);
        //}

        public double ConvertirHorasDecimal(string StrHoraCadena)
        {
            double Resultado;

            if ((StrHoraCadena == "") || (StrHoraCadena == "00:00"))
            {
                Resultado = 0;
            }
            else
            {
                string[] StrHoras = StrHoraCadena.Split(':');
                double dblHoras = Convert.ToInt32(StrHoras[0]);
                double dblMinutos = Convert.ToInt32(StrHoras[1]);

                double dblSegundos = Convert.ToInt32(StrHoras[2].Substring(0, 2));
                Resultado = dblHoras + (((dblMinutos * 100) / 60) / 100);

                Resultado = Resultado + (((dblSegundos * 100) / 3600) / 100);
            }

            return Resultado;
            //        Dim D As Date, TB, Resultado As Single
            //'Para el ejemplo, pero puede ser en string
            //D = "12:15"
            //TB = Split(D, ":")
            //Resultado = TB(0) + ((TB(1) * 100) / 60) / 100
        }

        #endregion ConvertirHorasDecimal

        #region ConvertirDecimalHoras

        //****************************************************************************************************************
        //** NOMBRE      : ConvertirDecimalHoras
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte un numero decimal a horas
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               double       | DecHoraDecimal     | Hora en formato decimal a convertir
        //** DEVUELVE    : string
        //****************************************************************************************************************
        public string ConvertirDecimalHoras(double DecHoraDecimal)
        {
            string StrOutput = "";

            if (DecHoraDecimal == 0)
            {
                StrOutput = "";
                return StrOutput;
            }

            //if (double.IsNaN(DecHoraDecimal) != true)
            //{
            //    TimeSpan timespan = TimeSpan.FromHours(DecHoraDecimal);
            //    //TimeSpan timespan = TimeSpan.fro(DecHoraDecimal);
            //    StrOutput = timespan.ToString("hh\\:mm"); // HH: 24h or hh: 12h
            //}

            //var timeSpan = TimeSpan.FromMinutes(DecHoraDecimal);
            //int hh = timeSpan.Hours;
            //int mm = timeSpan.Minutes;
            //int ss = timeSpan.Seconds;
            //StrOutput = hh.ToString("00") + ":" + mm.ToString("00") + ":" + ss.ToString("00");

            var timeSpan = TimeSpan.FromHours(DecHoraDecimal);
            int Hora = timeSpan.Hours;
            int Minuto = timeSpan.Minutes;
            int Segundo = timeSpan.Seconds;

            //double Hora = 0;
            //double MinutosSaldo = 0;
            //if (Math.Truncate(DecHoraDecimal)!=0){
            //    MinutosSaldo = (DecHoraDecimal - Math.Truncate(DecHoraDecimal));
            //    Hora = Math.Truncate(DecHoraDecimal);
            //}
            //else{
            //    Hora = 0;
            //    MinutosSaldo = DecHoraDecimal;
            //}

            //double Minuto = (MinutosSaldo * 60);
            //Minuto = Math.Truncate(Minuto);

            //double SegundosSaldo = (MinutosSaldo - Math.Truncate(MinutosSaldo));

            //double Segundo = (SegundosSaldo * 60);
            //Segundo = Math.Truncate(Segundo);

            StrOutput = Hora.ToString("00") + ":" + Minuto.ToString("00") + ":" + Segundo.ToString("00");

            //StrOutput = Math.Truncate(Hora).ToString("00") + ":" + Math.Truncate(Minuto).ToString("00") + ":" + Math.Truncate(Segundo).ToString("00");
            return StrOutput;
        }

        #endregion ConvertirDecimalHoras

        #region RestarHoras

        //****************************************************************************************************************
        //** NOMBRE      : RestarHoras
        //** TIPO        : Funcion
        //** DESCRIPCION : Resta el numero de horas
        //** PARAMETROS  :
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               string       | StrHoraInicio      | Hora de inicio, a esta hora se le restara la hora final
        //**               string       | StrHoraFinal       | Hora final, esta hora restara a la hora de inicio
        //** DEVUELVE    : string
        //****************************************************************************************************************
        public string RestarHoras(string StrHoraInicio, string StrHoraFinal)
        {
            string StrResultado;
            try
            {
                //DateTime StartTime = DateTime.Parse(StrHoraInicio);
                //DateTime EndTime = DateTime.Parse(StrHoraFinal);

                //TimeSpan Span = EndTime.Subtract(StartTime);

                //DateTime TotalTime = DateTime.Parse(Span.Hours + ":" + Span.Minutes);

                //StrResultado = Span.ToString(); //TotalTime.ToString("HH:mm");
                //return StrResultado;

                double Hora1 = ConvertirHorasDecimal(StrHoraInicio);
                double Hora2 = ConvertirHorasDecimal(StrHoraFinal);
                double TotalHoras = (Hora2 - Hora1);

                StrResultado = ConvertirDecimalHoras(TotalHoras);
                return StrResultado;
            }
            catch
            {
                return "";
                //Yo muestro mensaje de error, cada cual su forma.
            }
        }

        #endregion RestarHoras

        #region DataSetCombinarTablas

        //****************************************************************************************************************
        //** NOMBRE      : DataSetCombinarTablas
        //** TIPO        : Funcion
        //** DESCRIPCION : Combina la tablas de un DataSet a travez de un campo en comun, todas las tablas que se indique
        //**               en el array de parametros se fusionaran en una unica tabla la cual sera especificada dejando la
        //**               columna 3 del array en blanco
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               DataSet    |  ObjDataSet      | Nombre del Objeto DataSet donde se fusionaran las tablas
        //**               string[,]  |  StrTablasDato   | Array de parametros para generar la fucion de las tablas del
        //**                                               DataSet.
        //**                                               Este array debe de contener 4 Columnas donde se designara la siguiente
        //**                                               informacion
        //**                                               Columna 0 = Indica el nombre de la tabla que se va a fusionar
        //**                                               Columna 1 = Indica el nombre del campo llave de la tabla,
        //**                                               Columna 2 = Indica el Tipo de datos del campo clave
        //**                                               Columna 3 = Indica el campo a relacionar en la tabla principal
        //** EJEMPLO     :
        //**               string[,] ObjDataSet = new string[2, 4] {
        //**                                                           { "Table1", "tick_ITicketId",   "N", "" },
        //**                                                           { "Table2", "serv_IServicioId", "N", "tick_ITipoServicioId" }
        //**                                                       };
        //** DEVUELVE    : DataTable
        //****************************************************************************************************************
        public DataTable DataSetCombinarTablas(DataSet ObjDataSet, string[,] StrTablasDato)
        {
            int A;
            int IntNumeroElementos;
            int IntNumTablaPrincipal;
            string StrTablaPrincipal;
            string StrTablaAgregar;
            DataTable dtResult = new DataTable();
            StrTablaPrincipal = "";
            StrTablaAgregar = "";

            //BUSCAMOS LA TABLA PRINCIPAL DE LA COMBINACION
            IntNumeroElementos = Convert.ToInt16(StrTablasDato.GetLongLength(0));
            IntNumTablaPrincipal = 0;
            for (A = 0; A <= IntNumeroElementos - 1; A++)
            {
                StrTablasDato[A, 1].ToString();

                if (StrTablasDato[A, 3].ToString() == "")
                {
                    StrTablaPrincipal = StrTablasDato[A, 0].ToString();
                    IntNumTablaPrincipal = IntNumTablaPrincipal + 1;
                }
            }

            if (IntNumTablaPrincipal > 1)
            {
                //ERROR NO PUEDE HABER MAS DE UNA TABLA PRINCIPAL PARA REALIZAR LA COMBINACION DE DATOS
                // 1 = Se ha detectado que mas de una tabla principal, verifique que la columna Relacion no tenga mas de un registro sin datos
                return dtResult;
            }

            // CLONAMOS EL DATATABLE PRINCIPAL
            dtResult = DataTableClonar(ObjDataSet.Tables[StrTablaPrincipal]);

            for (A = 0; A <= IntNumeroElementos - 1; A++)
            {
                if (StrTablasDato[A, 3].ToString() != "")
                {
                    DataTable dtTablaOrigen = new DataTable();
                    Funciones xMiFun = new Funciones();

                    StrTablaAgregar = StrTablasDato[A, 0].ToString();

                    if (StrTablasDato[A, 4].ToString() != "")
                    {
                        dtTablaOrigen = ObjDataSet.Tables[StrTablaAgregar];
                        //dtTablaOrigen = xMiFun.DataTableFiltrar(dtTablaOrigen, StrTablasDato[A, 4].ToString(), "");
                        dtTablaOrigen = xMiFun.DataViewFiltrar(dtTablaOrigen, StrTablasDato[A, 4].ToString(), "").ToTable();
                    }
                    else
                    {
                        dtTablaOrigen = ObjDataSet.Tables[StrTablaAgregar];
                    }

                    // SI NO ES LA TABLA PRINCIPAL LE FUSIONAMOS LAS COLUMAS DE LAS DEMAS TABLAS
                    //dtResult = DataTableFusionar(ObjDataSet.Tables[VTablaAgregar], dtResult, TablasDato[A, 1].ToString(), TablasDato[A, 3].ToString());
                    dtResult = DataTableFusionar(dtTablaOrigen, dtResult, StrTablasDato[A, 1].ToString(), StrTablasDato[A, 3].ToString());
                }
            }

            return dtResult;
        }

        #endregion DataSetCombinarTablas

        #region DataTableFusionar

        //****************************************************************************************************************
        //** NOMBRE      : DataTableFusionar
        //** TIPO        : Funcion
        //** DESCRIPCION : Permite agregar las columnas de un DataTable a otro DataTable
        //** PARAMETROS  : DataTable = Indica el
        //**               TIPO         | NOMBRE               | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               DataTable    | DtTablaOrigen        | Nombre del objeto DataTable del cual se extraeran las columnas
        //**               DataTable    | DtTablaDestino       | Nombre del objeto DataTable al cual se insertaran las columnas
        //**               string       | StrCampoComunOrigen  |
        //**               string       | StrCampoComunDestino |
        //** DEVUELVE    : DataTable
        //****************************************************************************************************************
        public DataTable DataTableFusionar(DataTable DtTablaOrigen, DataTable DtTablaDestino, string StrCampoComunOrigen, string StrCampoComunDestino)
        {
            int IntColumna;
            int IntFila;
            int IntFilaD;
            string StrNombreCampo = "";

            IntColumna = 0;
            // AGREGAMOS LAS COLUMNAS DEL DataTable ORIGEN
            for (IntColumna = 0; IntColumna <= DtTablaOrigen.Columns.Count - 1; IntColumna++)
            {
                StrNombreCampo = DtTablaOrigen.Columns[IntColumna].ToString();
                DtTablaDestino.Columns.Add(StrNombreCampo, DtTablaOrigen.Columns[IntColumna].DataType);
            }

            // AGREGAMOS LOS REGISTROS DEL DataTable ORIGEN AL DataTable DESTINO
            // RECORREMOS LAS TABLA ORIGEN
            for (IntFila = 0; IntFila <= DtTablaOrigen.Rows.Count - 1; IntFila++)
            {
                for (IntFilaD = 0; IntFilaD <= DtTablaDestino.Rows.Count - 1; IntFilaD++)
                {
                    if (DtTablaDestino.Rows[IntFilaD][StrCampoComunDestino].ToString() == DtTablaOrigen.Rows[IntFila][StrCampoComunOrigen].ToString())
                    {
                        //AGREGAMOS LOS REGISTROS DEL ORIGEN AL DESTINO
                        for (IntColumna = 0; IntColumna <= DtTablaOrigen.Columns.Count - 1; IntColumna++)
                        {
                            StrNombreCampo = DtTablaOrigen.Columns[IntColumna].ToString();
                            if (DtTablaOrigen.Rows[IntFila][StrNombreCampo].ToString() != "")
                            {
                                DtTablaDestino.Rows[IntFilaD][StrNombreCampo] = DtTablaOrigen.Rows[IntFila][StrNombreCampo].ToString();
                            }
                        }
                    }
                }
            }

            return DtTablaDestino;
        }

        #endregion DataTableFusionar

        #region DataTableFiltrar

        //****************************************************************************************************************
        //** NOMBRE      : DataTableFiltrar
        //** TIPO        : Funcion
        //** DESCRIPCION : Permite Filtrar un DataTabkle
        //** PARAMETROS  : DataTable = Indica el
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               DataTable    | DtTablaOrigen      | Nombre del objeto DataTable del cual se extraeran las columnas
        //**               string       | StrFiltro             |
        //**               string       | StrOrden              |
        //** EJEMPLO     :
        //**               DtResult = MiFun.DatatTableFiltrar(DtResult, "tick_ITipoEstado = 0", "tick_INumero ASC");
        //** DEVUELVE    : DataTable
        //****************************************************************************************************************
        public DataTable DataTableFiltrar(DataTable DtTablaOrigen, string StrFiltro, string StrOrden)
        {
            DataRow[] DrwRows;
            DataTable dtNew;

            try
            {
                dtNew = DtTablaOrigen.Clone();
                //dtNew.Columns[0].DataType = GetType(int);

                DrwRows = DtTablaOrigen.Select(StrFiltro, StrOrden);
                if (DrwRows != null)
                {
                    foreach (DataRow dr in DrwRows)
                    {
                        dtNew.ImportRow(dr);
                    }
                }
                return dtNew;
            }
            catch (Exception ex)
            {
                throw new Exception("FiltrarDataTable" + " – " + ex.Source + " – " + ex.Message);
            }
        }

        #endregion DataTableFiltrar

        #region DataViewFiltrar

        //****************************************************************************************************************
        //** NOMBRE      : DataViewFiltrar
        //** TIPO        : Funcion
        //** DESCRIPCION : Crea una DataView Filtrado a Partir de un DataTable
        //** PARAMETROS  : DataTable = Indica el
        //**               TIPO         | NOMBRE             | DESCRIPCION
        //**               -----------------------------------------------------------------------------------------------
        //**               DataTable    | DtTablaOrigen      | Nombre del objeto DataTable del cual se extraeran las columnas
        //**               string       | StrFiltro          | Condicion del filtro a aplicar
        //**               string       | StrOrden           |
        //** EJEMPLO     :
        //**               DataView = MiFun.DataViewFiltrar(DtResult, "tick_ITipoEstado = 0", "tick_INumero ASC");
        //** DEVUELVE    : DataView
        //****************************************************************************************************************
        public DataView DataViewFiltrar(DataTable DtTablaOrigen, string StrFiltro, string StrOrden)
        {
            DataView DvResultado = new DataView(DtTablaOrigen);
            DvResultado.RowFilter = StrFiltro;
            DvResultado.Sort = StrOrden;
            return DvResultado;
        }

        #endregion DataViewFiltrar

        #region DataTableClonar

        //*********************************************************************************************
        //** NOMBRE      : DataTableClonar
        //** TIPO        : Funcion
        //** DESCRIPCION : Permite clonar un DataTable
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               DataTable  |  DtTablaOrigen   | Nombre del objeto DataTable que se a clonar
        //**
        //** DEVUELVE    : DataTable
        //*********************************************************************************************
        public DataTable DataTableClonar(DataTable DtTablaOrigen)
        {
            DataTable dtResult = new DataTable();
            int A;
            string vNombreCampo = "";

            A = 0;
            for (A = 0; A <= DtTablaOrigen.Columns.Count - 1; A++)
            {
                vNombreCampo = DtTablaOrigen.Columns[A].ToString();
                dtResult.Columns.Add(vNombreCampo, DtTablaOrigen.Columns[A].DataType);
            }

            dtResult = DtTablaOrigen.Copy();
            return dtResult;
        }

        #endregion DataTableClonar

        #region DataTableCrear

        //*********************************************************************************************
        //** NOMBRE      : DataTableClonar
        //** TIPO        : Funcion
        //** DESCRIPCION : Permite crear un DataTable
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE             | DESCRIPCION
        //**               ----------------------------------------------
        //**               string[]   |  DataTableColumnas  | Nombre del objeto DataTable que se a clonar
        //**               string     |  DataTableNombre    | Array de parametros para generar las columnas del datatable                                                  DataSet.
        //**                                                  Este array debe de contener 2 Columnas donde se designara la siguiente
        //**                                                  informacion
        //**                                                  Columna 0 = Indica el nombre del campo que se agregar
        //**                                                  Columna 1 = Indica el tipo de dato del campo
        //**  EJEMPLO    :
        //**               string[,] Columnas = new string[5, 2] { {"tick_ITicketId", "System.Int16"},
        //**                                                       {"tick_IOficinaConsularId", "System.Int32"},
        //**                                                       {"tick_ITipoServicioId", "System.String"},
        //**                                                       {"tick_IPersonalId", "System.Decimal"},
        //**                                                       {"tick_IPersonalId2", "System.DateTime"}
        //**                                                     };
        //** DEVUELVE    : DataTable
        //*********************************************************************************************
        public DataTable DataTableCrear(string[,] DataTableColumnas, string DataTableNombre)
        {
            DataTable dtResult = new DataTable();
            int intNumeroElementos;
            int intFila = 0;
            string vNombreCampo = "";
            string vTipoCampo = "";

            intNumeroElementos = Convert.ToInt16(DataTableColumnas.GetLongLength(0));

            for (intFila = 0; intFila <= intNumeroElementos - 1; intFila++)
            {
                vNombreCampo = DataTableColumnas[intFila, 0].ToString(); //DtTablaOrigen.Columns[A].ToString();
                vTipoCampo = DataTableColumnas[intFila, 1].ToString();
                if (vTipoCampo == "System.Boolean") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Boolean")); }
                if (vTipoCampo == "System.Byte") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Byte")); }
                if (vTipoCampo == "System.Byte[]") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Byte[]")); }
                if (vTipoCampo == "System.Char") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Char")); }
                if (vTipoCampo == "System.DateTime") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.DateTime")); }
                if (vTipoCampo == "System.Decimal") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Decimal")); }
                if (vTipoCampo == "System.Double") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Double")); }
                if (vTipoCampo == "System.Int16") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Int16")); }
                if (vTipoCampo == "System.Int32") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Int32")); }
                if (vTipoCampo == "System.int64") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.Int64")); }
                if (vTipoCampo == "System.String") { dtResult.Columns.Add(vNombreCampo, Type.GetType("System.String")); }

                //Boolean
                //Byte
                //Char
                //DateTime
                //Decimal
                //Double
                //Guid
                //Int16
                //Int32
                //Int64
                //SByte
                //Single
                //String
                //TimeSpan
                //UInt16
                //UInt32
                //UInt64
            }
            dtResult.TableName = DataTableNombre;
            return dtResult;
        }

        #endregion DataTableCrear

        #region DataTableDistinc

        //*********************************************************************************************
        //** NOMBRE      : DataTableDistinc
        //** TIPO        : Funcion
        //** DESCRIPCION : Permite hacer distinc a un campo de un datatable
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               string     |  TableName       | Nombre de la tabla
        //**               DataTable  |  SourceTable     | Datatable a la cual se le aplicara el distinc
        //**               string     |  FieldName       | Campo al que se realizara el distinc
        //**
        //** DEVUELVE    : DataTable
        //*********************************************************************************************
        public DataTable DataTableDistinc(string TableName, DataTable SourceTable, string FieldId, string FieldDescripcion)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldId, SourceTable.Columns[FieldId].DataType);
            dt.Columns.Add(FieldDescripcion, SourceTable.Columns[FieldDescripcion].DataType);

            object LastValue = null;
            object LastValue2 = null;
            foreach (DataRow dr in SourceTable.Select("", FieldId))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldId])))
                {
                    LastValue = dr[FieldId];
                    LastValue2 = dr[FieldDescripcion];
                    dt.Rows.Add(new object[] { LastValue, LastValue2 });
                }
            }
            return dt;
        }

        #endregion DataTableDistinc

        private bool ColumnEqual(object A, object B)
        {
            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }

        #region ListBoxLLenarConArray

        //*********************************************************************************************
        //** NOMBRE      : ListBoxLLenarConArray
        //** TIPO        : Procedimiento
        //** DESCRIPCION : Llena un ListBox con datos procedentes de un Array
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               ListBox    |  CmbControl      | Nombre del Control ListBox
        //**               string[]   |  StrValor        | Valores que se llenaran en en el ListBox
        //**
        //** DEVUELVE    :
        //*********************************************************************************************
        public void ListBoxLLenarConArray(ref ListBox CmbControl, string[] StrValor)
        {
            CmbControl.Items.Clear();
            CmbControl.Items.AddRange(StrValor);
        }

        #endregion ListBoxLLenarConArray

        #region ListBoxLimpiarLista

        //*********************************************************************************************
        //** NOMBRE      : ListBoxLimpiarLista
        //** TIPO        : Procedimiento
        //** DESCRIPCION : Limpia un ListBox
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               ListBox    |  CmbControl      | Nombre del Control ListBox a limpiar
        //**
        //** DEVUELVE    :
        //*********************************************************************************************
        public void ListBoxLimpiarLista(ref ListBox CmbControl)
        {
            CmbControl.Items.Clear();
        }

        #endregion ListBoxLimpiarLista

        #region ComboBoxCargarDataTable

        //*********************************************************************************************
        //** NOMBRE      : ComboBoxCargarDataTable
        //** TIPO        : Procedimiento
        //** DESCRIPCION : Carga un ComboBox con los datos provenientes de un Datatable
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               ComboBox   |  CmbControl      | Nombre del Control ComboBox a cargar
        //**               DataTable  |  DtMyTabla       | DataTable de donde se cargaran los datos
        //**               string     |  StrCampoValue   | Nombre del campo identificacion del DataTable
        //**               string     |  StrCampoDisplay | Nombre del Campo Descripcion del DataTable
        //**
        //** DEVUELVE    :
        //*********************************************************************************************
        public void ComboBoxCargarDataTable(ComboBox CmbControl, DataTable DtMyTabla, string StrCampoValue, string StrCampoDisplay)
        {
            CmbControl.DataSource = DtMyTabla;
            CmbControl.DisplayMember = StrCampoDisplay;   // INDICAMOS EL NOMBRE DEL CAMPO A MOSTRAR
            CmbControl.ValueMember = StrCampoValue;       // IDICAMOS EL NOMBRE DEL CAMPO VALUE
            CmbControl.SelectedValue = 0;              // LE INDICAMOS QUE POR DEFECTO NO SELECCIONE NINGUNO ELEMENTO DEL COMBO BOX
        }

        #endregion ComboBoxCargarDataTable

        #region CrystalVisor

        //*********************************************************************************************
        //** NOMBRE      : CrystalVisor
        //** TIPO        : Procedimiento
        //** DESCRIPCION : Carga un reporte de crystal report
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               string[,]  |  StrParametros   | Array de parametros que se le pasaran al reporte de crystal report
        //**                                               Este array debe de contener 2 Columnas donde se designara la siguiente
        //**                                               informacion
        //**                                               Columna 0 = Indica el nombre del parametro del reporte
        //**                                               Columna 1 = Indica el valor del parametro del reporte,
        //**               string     |  StrTituloReporte| Titulo del reporte
        //**               string     |  StrRutaReporte  | Ruta del archivo de reporte
        //** EJEMPLO     :
        //**               string[,] StrParametros = new string[2, 2] {
        //**                                                             { "Parametro1", "9999" },
        //**                                                             { "Parametro1", "Otro Parametro" }
        //**                                                          };
        //** DEVUELVE    :
        //*********************************************************************************************

        //public void CrystalVisor(string[,] StrParametros, string StrTituloReporte, string StrRutaReporte, DataTable DtDatos)
        public void CrystalVisor(string[,] StrParametros, string StrTituloReporte, string StrRutaReporte, DataSet DsDatos, string FechaFiltro)
        {
            int A;
            int IntNumeroElementos;
            ParameterFields ParParamFiels = new ParameterFields();

            IntNumeroElementos = Convert.ToInt16(StrParametros.GetLongLength(0));

            ReportParameter[] parameters = new ReportParameter[IntNumeroElementos];
            // CARGAMOS LOS PARAMETROS DEL REPORTE
            for (A = 0; A <= IntNumeroElementos - 1; A++)
            {
                ParameterField ParParam = new ParameterField();
                ParameterDiscreteValue discreteValue = new ParameterDiscreteValue();
                parameters[A] = new ReportParameter(StrParametros[A, 0], StrParametros[A, 1]);

                //ParParam.ParameterFieldName = StrParametros[A, 0].ToString();
                //discreteValue.Value = StrParametros[A, 1].ToString();
                //ParParam.CurrentValues.Add(discreteValue);

                //ParParamFiels.Add(ParParam);
            }

            //ReportTicket xFrm = new ReportTicket();
            FrmReporteVistaPrevia xFrm = new FrmReporteVistaPrevia();
            //if (DtDatos.TableName != "")
            //{
            //    xFrm.DtDatos = DtDatos;
            //}
            xFrm.DsDatos = DsDatos;
            xFrm.IntAltoFormulario = IntCrystalVisorAltoFormulario;
            xFrm.IntAnchoFormulario = IntCrystalVisorAnchoFormulario;
            //xFrm.ParParametroReportes = ParParamFiels;
            xFrm.parameters = parameters;
            xFrm.StrTituloReporte = StrTituloReporte;
            xFrm.StrRutaReporte = StrRutaReporte;
            xFrm.FechaFiltro = FechaFiltro; 
            xFrm.ShowDialog();
        }

        public void CrystalVisor(string[,] StrParametros, string StrTituloReporte, string StrRutaReporte, DataSet DsDatos)
        {
            int A;
            int IntNumeroElementos;
            ParameterFields ParParamFiels = new ParameterFields();

            IntNumeroElementos = Convert.ToInt16(StrParametros.GetLongLength(0));

            ReportParameter[] parameters = new ReportParameter[IntNumeroElementos];
            // CARGAMOS LOS PARAMETROS DEL REPORTE
            for (A = 0; A <= IntNumeroElementos - 1; A++)
            {
                ParameterField ParParam = new ParameterField();
                ParameterDiscreteValue discreteValue = new ParameterDiscreteValue();
                parameters[A] = new ReportParameter(StrParametros[A, 0], StrParametros[A, 1]);

                //ParParam.ParameterFieldName = StrParametros[A, 0].ToString();
                //discreteValue.Value = StrParametros[A, 1].ToString();
                //ParParam.CurrentValues.Add(discreteValue);

                //ParParamFiels.Add(ParParam);
            }

            //ReportTicket xFrm = new ReportTicket();
            FrmReporteVistaPrevia xFrm = new FrmReporteVistaPrevia();
            //if (DtDatos.TableName != "")
            //{
            //    xFrm.DtDatos = DtDatos;
            //}
            xFrm.DsDatos = DsDatos;
            xFrm.IntAltoFormulario = IntCrystalVisorAltoFormulario;
            xFrm.IntAnchoFormulario = IntCrystalVisorAnchoFormulario;
            //xFrm.ParParametroReportes = ParParamFiels;
            xFrm.parameters = parameters;
            xFrm.StrTituloReporte = StrTituloReporte;
            xFrm.StrRutaReporte = StrRutaReporte;
            xFrm.ShowDialog();
            
        }

        #endregion CrystalVisor

        #region CrystalImprimir

        //*********************************************************************************************
        //** NOMBRE      : CrystalImprimir
        //** TIPO        : Procedimiento
        //** DESCRIPCION : Imprime directamente un reporte
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               string[,]  |  StrParametros   | Array de parametros que se le pasaran al reporte de crystal report
        //**                                               Este array debe de contener 2 Columnas donde se designara la siguiente
        //**                                               informacion
        //**                                               Columna 0 = Indica el nombre del parametro del reporte
        //**                                               Columna 1 = Indica el valor del parametro del reporte,
        //**               string     |  StrTituloReporte| Titulo del reporte
        //**               string     |  StrRutaReporte  | Ruta del archivo de reporte
        //** EJEMPLO     :
        //**               string[,] StrParametros = new string[2, 2] {
        //**                                                             { "Parametro1", "9999" },
        //**                                                             { "Parametro1", "Otro Parametro" }
        //**                                                          };
        //** DEVUELVE    :
        //*********************************************************************************************
        public void CrystalImprimir(string[,] StrParametros, string StrTituloReporte, string StrRutaReporte, DataSet DsDatos, string strImpresora)
        {
            intErrorNumero = 0;
            try
            {
                int A;
                int IntNumeroElementos;
                ParameterFields ParParamFiels = new ParameterFields();

                IntNumeroElementos = Convert.ToInt16(StrParametros.GetLongLength(0));

                // CARGAMOS LOS PARAMETROS DEL REPORTE
                for (A = 0; A <= IntNumeroElementos - 1; A++)
                {
                    ParameterField ParParam = new ParameterField();
                    ParameterDiscreteValue discreteValue = new ParameterDiscreteValue();

                    ParParam.ParameterFieldName = StrParametros[A, 0].ToString();
                    discreteValue.Value = StrParametros[A, 1].ToString();
                    ParParam.CurrentValues.Add(discreteValue);

                    ParParamFiels.Add(ParParam);
                }

                ReportDocument MiRpt = new ReportDocument();

                //lInforme.PrintOptions.PrinterName
                MiRpt.PrintOptions.PrinterName = strImpresora;
                
                

                MiRpt.Load(StrRutaReporte);                       // CARGAMOS EL REPORTE
                MiRpt.SetDataSource(DsDatos.Copy());              // CARGAMOS LOS DATOS

                int intNumFilas = Convert.ToInt32(StrParametros.GetLongLength(0));

                for (A = 0; A <= intNumFilas - 1; A++)
                {
                    MiRpt.SetParameterValue(StrParametros[A, 0], StrParametros[A, 1]);
                }

                MiRpt.PrintToPrinter(0, true, 1, 1);    // ENVIAMOS A IMPRIMIR EL TICKET
            }
            catch (Exception ex)
            {
                intErrorNumero = -1;
                strErrorMensaje = ex.Message;
            }
        }

        #endregion CrystalImprimir

        #region NulosN

        //*********************************************************************************************
        //** NOMBRE      : NulosN
        //** TIPO        : Funcion
        //** DESCRIPCION : Devuelve sin un objeto es Nulo
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               object     |  ObjObjeto       | Objeto a eveluar
        //**
        //** DEVUELVE    : object
        //*********************************************************************************************
        public object NulosN(object ObjObjeto)
        {
            if (ObjObjeto == null)
            {
                return 0;
            }
            else
            {
                return ObjObjeto;
            }
        }

        #endregion NulosN

        #region NulosC

        //*********************************************************************************************
        //** NOMBRE      : NulosC
        //** TIPO        : Funcion
        //** DESCRIPCION : Devuelve sin un objeto es Nulo
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               object     |  ObjObjeto       | Objeto a eveluar
        //**
        //** DEVUELVE    : object
        //*********************************************************************************************
        public string NulosC(object ObjObjeto)
        {
            if (ObjObjeto == null)
            {
                return "";
            }
            else
            {
                return ObjObjeto.ToString();
            }
        }

        #endregion NulosC

        #region FechaCadena

        //*********************************************************************************************
        //** NOMBRE      : FechaCadena
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte una fecha en formato de hora local a cadenatexto en formato de hora universal
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               DateTime   |  DatFecha        | Fecha y actual a tranformar
        //**
        //** DEVUELVE    : string
        //*********************************************************************************************
        public string FechaCadena(DateTime DatFecha)
        {
            string Strfecha;
            DatFecha = DatFecha.ToUniversalTime();
            Strfecha = DatFecha.Year.ToString() + DatFecha.Month.ToString() + DatFecha.Day.ToString() + "T" + DatFecha.Hour.ToString() + DatFecha.Minute.ToString();

            return Strfecha;
        }

        #endregion FechaCadena

        #region CadenaFecha

        //*********************************************************************************************
        //** NOMBRE      : CadenaFecha
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte una cadenatexto en formato de fecha y hora universal a datetime en fomato de fecha y hora local
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               string     |  StrCadenaFecha  | Fecha y actual a tranformar
        //**
        //** DEVUELVE    : DateTime
        //*********************************************************************************************
        public DateTime CadenaFecha(string StrCadenaFecha)
        {
            string StrCadena;
            DateTime DatFecha;

            StrCadena = StrCadenaFecha.Substring(6, 2) + "/" + StrCadenaFecha.Substring(4, 2) + "/" + StrCadenaFecha.Substring(0, 4) + " " + StrCadenaFecha.Substring(9, 2) + ":" + StrCadenaFecha.Substring(11, 2);
            DatFecha = Convert.ToDateTime(StrCadena);
            return DatFecha.ToLocalTime();
        }

        #endregion CadenaFecha

        #region ImpresoraEnLinea

        //*********************************************************************************************
        //** NOMBRE      : ImpresoraEnLinea
        //** TIPO        : Funcion
        //** DESCRIPCION : Indica si una impresora esta disponible para iniciar la impresion
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               string     |  printerName     | Nombre de la impresora a verificar
        //**
        //** DEVUELVE    : bool
        //*********************************************************************************************
        public bool ImpresoraEnLinea(string printerName)
        {
            string str = "";
            bool online = false;

            //set the scope of this search to the local machine
            ManagementScope scope = new ManagementScope(ManagementPath.DefaultPath);
            //connect to the machine
            scope.Connect();

            //query for the ManagementObjectSearcher
            SelectQuery query = new SelectQuery("select * from Win32_Printer");

            ManagementClass m = new ManagementClass("Win32_Printer");

            ManagementObjectSearcher obj = new ManagementObjectSearcher(scope, query);

            //get each instance from the ManagementObjectSearcher object
            using (ManagementObjectCollection printers = m.GetInstances())
                //now loop through each printer instance returned
                foreach (ManagementObject printer in printers)
                {
                    //first make sure we got something back
                    if (printer != null)
                    {
                        //get the current printer name in the loop
                        str = printer["Name"].ToString().ToLower();

                        //check if it matches the name provided
                        if (str.Equals(printerName.ToLower()))
                        {
                            //since we found a match check it's status
                            if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                                online = true;
                            else
                                online = false;
                            //if (printer["WorkOffline"].ToString().ToLower().Equals("true") || printer["PrinterStatus"].Equals(7))
                            //    //it's offline
                            //    online = false;
                            //else
                            //    //it's online
                            //    online = true;
                        }
                    }
                    else
                        throw new Exception("No printers were found");
                }
            return online;
        }

        #endregion ImpresoraEnLinea

        #region ImpresoraPredeterminada

        //*********************************************************************************************
        //** NOMBRE      : ImpresoraPredeterminada
        //** TIPO        : Funcion
        //** DESCRIPCION : Devuelve el nombre de la impresora predeterminada
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**
        //**
        //** DEVUELVE    : string
        //*********************************************************************************************
        public string ImpresoraPredeterminada()
        {
            PrintDocument pd = new PrintDocument();
            string s_Default_Printer = pd.PrinterSettings.PrinterName;
            return s_Default_Printer;
        }

        #endregion ImpresoraPredeterminada

        #region VerArchivoTexto

        //*********************************************************************************************
        //** NOMBRE      : VerArchivoTexto
        //** TIPO        : Procedimiento
        //** DESCRIPCION : Visualiza el contenido de un archivo de texto
        //** PARAMETROS  :
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               string     | StrRutaArchivo   | Ruta del archivo que se desea visualizar
        //**               string     | StrTitulo        | Titulo que se mostrara en el visor de archivos
        //**
        //** DEVUELVE    : No devuelve nada
        //*********************************************************************************************
        public void VerArchivoTexto(string StrRutaArchivo, string StrTitulo)
        {
            FrmVerArchivoTexto FrmVisor = new FrmVerArchivoTexto();

            FrmVisor.StrNombreArchivo = StrRutaArchivo;
            FrmVisor.StrTitulo = StrTitulo;
            FrmVisor.ShowDialog();

            //// Initialize the filter to look for text files.
            //openFile1.Filter = "Text Files|*.txt";

            //// If the user selected a file, load its contents into the RichTextBox.
            //if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    richTextBox1.LoadFile(openFile1.FileName,
            //    RichTextBoxStreamType.PlainText);
        }

        #endregion VerArchivoTexto

        #region ShowDialogoVerArchivo

        //*********************************************************************************************
        //** NOMBRE      : ShowDialogVerArchivo
        //** TIPO        : Funcion
        //** DESCRIPCION : Funcion que devuuelve la ruta del archivo seleccionado
        //** PARAMETROS  :
        //**               TIPO           |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               OpenFileDialog | Control          | Nombre del objeto OpenFileDialog
        //**               string         | StrFiltro        | Filtro que se le aplicara al control
        //**               int            | IntFiltroXDefecto| Indica el indice del filtro que se aplicara
        //**               string         | StrTexto         | Indica el titulo que se mostra en el cuadro
        //**                                                   de busqueda del control OpenFileDialog
        //**               string         | StrTitulo        | Titulo del control OpenFileDialog
        //**
        //** EJEMPLI     :
        //**               string StrFiltro = "Bitmap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif|JGP files (*.jpg)|*.jpg|All (*.*)|*.* |PNG (*.patito)|*.png ";
        //** DEVUELVE    : Devuelve string con el nombre y ubicacion del archivo seleccionado
        //*********************************************************************************************
        public string ShowDialogVerArchivo(OpenFileDialog Control, string StrFiltro, int IntFiltroXDefecto, string StrTexto, string StrTitulo)
        {
            string StrArchivo = "";
            //Definimos los filtros de archivos a permitir, en este caso imagenes
            Control.Filter = StrFiltro;              // "Bitmap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif|JGP files (*.jpg)|*.jpg|All (*.*)|*.* |PNG (*.patito)|*.png ";
            ///Establece que filtro se mostrará por deceto en este caso, 3=jpg
            Control.FilterIndex = IntFiltroXDefecto; // 1;
            ///Esto aparece en el Nombre del archivo a seleccionar, se puede quitar no es fundamental
            Control.FileName = StrTexto;             // "Seleccione una imagen";

            //El titulo de la Ventana....
            Control.Title = StrTitulo;               // "Bachillerato en Computación";

            //El directorio que por deceto habrirá, para cada contrapleca del Path colocar \\ C:\\Fotitos\\Wizard y así sucesivamente
            Control.InitialDirectory = "c:\\";

            /// Evalúa que si al aparecer el cuadro de dialogo la persona presionó Ok
            if (Control.ShowDialog() == DialogResult.OK)
            {
                /// Si esto se cumple, capturamos la propiedad File Name y la guardamos en la variable Garrobito
                StrArchivo = Control.FileName;
            }
            return StrArchivo;
        }

        #endregion ShowDialogoVerArchivo

        #region IMG_ControlToImagen

        ////*********************************************************************************************
        ////** NOMBRE      : IMG_ControlToImagen
        ////** TIPO        : Funcion
        ////** DESCRIPCION : Convierte en Imagen el contenido de un Control
        ////** PARAMETROS  :
        ////**               TIPO         |  NOMBRE      | DESCRIPCION
        ////**               ----------------------------------------------
        ////**               Control      |  Control     | Nombre del control del que se obtendra la imagen
        ////**
        ////** DEVUELVE    :
        ////*********************************************************************************************
        public byte[] IMG_ControlToImagen(Control Control)
        {
            byte[] ByteImagen;
            Bitmap bm = new Bitmap(Control.Width, Control.Height);

            Control.DrawToBitmap(bm, new Rectangle(0, 0, Control.Width, Control.Height));

            ByteImagen = IMG_ConvImagenToByte(bm, System.Drawing.Imaging.ImageFormat.Bmp);
            return ByteImagen;
            //e.Graphics.DrawImage(bm, 0, 0);
        }

        #endregion IMG_ControlToImagen

        #region IMG_ConvImagenToByte

        //*********************************************************************************************
        //** NOMBRE      : IMG_ConvImagenToByte
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte a bytes una imagen
        //** PARAMETROS  :
        //**               TIPO        |  NOMBRE         | DESCRIPCION
        //**               ----------------------------------------------
        //**               Image       |  imageToConvert | Imagen a convertir a bytes
        //**               ImageFormat |  formatOfImage  | Formato de la imagen que se convertira
        //**                                               System.Drawing.Imaging.ImageFormat.Bmp
        //**                                               System.Drawing.Imaging.ImageFormat.Jpeg
        //**                                               System.Drawing.Imaging.ImageFormat.Png
        //**                                               System.Drawing.Imaging.ImageFormat.MemoryBmp
        //**
        //** DEVUELVE    :
        //*********************************************************************************************
        public static byte[] IMG_ConvImagenToByte(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        #endregion IMG_ConvImagenToByte

        #region IMG_CargarImagen

        //*********************************************************************************************
        //** NOMBRE      : IMG_CargarImagen
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte en un array e bytes la imagen almacenada en un archivo
        //** PARAMETROS  :
        //**               TIPO        |  NOMBRE        | DESCRIPCION
        //**               ----------------------------------------------------------------------------
        //**               string      |  rutaArchivo   | Ruta Fisica del archivo de imagen que se va a
        //**                                              cargar
        //**
        //** DEVUELVE    : Array Byte
        //*********************************************************************************************
        public static Byte[] IMG_CargarImagen(string rutaArchivo)
        {
            if (rutaArchivo != "")
            {
                try
                {
                    FileStream Archivo = new FileStream(rutaArchivo, FileMode.Open);//Creo el archivo
                    BinaryReader binRead = new BinaryReader(Archivo);       //Cargo el Archivo en modo binario
                    Byte[] imagenEnBytes = new Byte[(Int64)Archivo.Length]; //Creo un Array de Bytes donde guardare la imagen
                    binRead.Read(imagenEnBytes, 0, (int)Archivo.Length);    //Cargo la imagen en el array de Bytes
                    binRead.Close();
                    Archivo.Close();
                    return imagenEnBytes;                                   //Devuelvo la imagen convertida en un array de bytes
                }
                catch
                {
                    return new Byte[0];
                }
            }
            return new byte[0];
        }

        #endregion IMG_CargarImagen

        #region IMG_ConvByteToImagen

        //*********************************************************************************************
        //** NOMBRE      : IMG_ConvByteToImagen
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte en imagen un array de bytes
        //** PARAMETROS  :
        //**               TIPO        |  NOMBRE        | DESCRIPCION
        //**               ----------------------------------------------------------------------------
        //**               byte[]      |  ArregloImagen | Arreglo de bytes a convertir a imagen
        //**
        //** DEVUELVE    : Image
        //*********************************************************************************************
        public static System.Drawing.Image IMG_ConvByteToImagen(byte[] ArregloImagen)
        {
            if (ArregloImagen == null)
                return null;

            System.IO.MemoryStream ms = new System.IO.MemoryStream(ArregloImagen);
            System.Drawing.Bitmap bm = null;
            try
            {
                bm = new System.Drawing.Bitmap(ms);
                return (System.Drawing.Image)bm;
            }
            catch (Exception ex)
            {
                strMensajeError = ex.Message;
                return null;
                throw;
            }
        }

        #endregion IMG_ConvByteToImagen

        #region FuentesCargarCombo

        //*********************************************************************************************
        //** NOMBRE      : FuentesCargarCombo
        //** TIPO        : Funcion
        //** DESCRIPCION : carga un ComboBox con todas las fuentes dispoibles
        //** PARAMETROS  :
        //**               TIPO        |  NOMBRE        | DESCRIPCION
        //**               ----------------------------------------------------------------------------
        //**               ComboBox    |  CmbControl    | contro combo Box que contendra la informacion
        //**                                              devuelta
        //**
        //** DEVUELVE    :
        //*********************************************************************************************
        public void FuentesCargarCombo(ComboBox CmbControl)
        {
            System.Drawing.FontFamily[] ArrayFuentes = System.Drawing.FontFamily.Families;
            foreach (System.Drawing.FontFamily fuente in ArrayFuentes)
            {
                CmbControl.Items.Add(fuente.Name);
            }
        }

        #endregion FuentesCargarCombo

        #region CombocargarNumero

        //*********************************************************************************************
        //** NOMBRE      : CombocargarNumero
        //** TIPO        : Funcion
        //** DESCRIPCION : carga un ComboBox numeros hasta un rango definido
        //** PARAMETROS  :
        //**               TIPO        |  NOMBRE        | DESCRIPCION
        //**               ----------------------------------------------------------------------------
        //**               ComboBox    |  CmbControl    | contro combo Box que contendra la informacion
        //**                                              devuelta
        //**
        //** DEVUELVE    :
        //*********************************************************************************************
        public void CombocargarNumero(ComboBox CmbControl, int NumeroMaximo)
        {
            int intFila = 0;

            for (intFila = 1; intFila <= NumeroMaximo; intFila++)
            {
                CmbControl.Items.Add(intFila.ToString());
            }
        }

        #endregion CombocargarNumero

        public string ObtenerVersionAplicacion()
        {
            string strVersion = "";
            string mLocation = "";

            mLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var objVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(mLocation);
            strVersion = objVersion.ProductVersion.ToString();

            return strVersion;
        }
    }
}