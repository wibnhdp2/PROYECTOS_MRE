using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Media;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace SGAC.Accesorios
{
    public class Util
    {
        private static string[] UNIDADES = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        private static string[] DECENAS = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve", "veinti", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        private static string[] CENTENAS = { "", "ciento", "doscientos", "trecientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

        #region Controles
        public static void CargarDropDownList(DropDownList ddlList, DataTable dtTable, string strDescription, string strValue, bool bItemAdicional = false, string strItemTexto = "- SELECCIONAR -")
        {
            ddlList.Items.Clear();

            ddlList.DataSource = dtTable;
            ddlList.DataTextField = strDescription;
            ddlList.DataValueField = strValue;
            
            if (bItemAdicional)
            {
                ddlList.AppendDataBoundItems = bItemAdicional;
                ddlList.Items.Insert(0, new ListItem(strItemTexto, "0"));
            }

            ddlList.DataBind();
        }
        //--------------------------------------------------------
        // Fecha: 04/01/2017
        // Autor: Miguel Márquez Beltrán
        // Objetivo: Cargar datos en el control RadioButtonList
        //--------------------------------------------------------
        public static void CargarParametroRadioButtonList(RadioButtonList rblList, DataTable dtTable)
        {
            rblList.Items.Clear();
            
            rblList.DataTextField = "descripcion"; //"para_vDescripcion";
            rblList.DataValueField = "id"; // "para_sParametroId";
            rblList.DataSource = dtTable;
            rblList.DataBind();
        }

        //----------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 21/11/2019
        // Objetivo: Filtrar el grupo de una lista de grupos
        //----------------------------------------------------

        public static DataTable ObtenerParametrosFiltrandoGrupo(string strGrupo, DataTable dtGrupoParametros)
        {
            if (dtGrupoParametros != null)
            {
                DataView dv = dtGrupoParametros.DefaultView;
                dv.RowFilter = " grupo = '" + strGrupo + "'";
                DataTable dtParametrosFiltrados = dv.ToTable();
                return dtParametrosFiltrados;
            }
            return dtGrupoParametros;
        }
        //-----------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 21/11/2019
        // Objetivo: Cargar cada DropDownList de una lista de grupos.
        //-----------------------------------------------------------------

        public static void CargarParametroDropDownListDesdeListaGrupos(DropDownList[] arrDDL, Enumerador.enmGrupo[] arrENMGrupo, DataTable dtGrupoParametros, bool bItemAdicional = false, string strItemTexto = "- SELECCIONAR -")
        {
            string strGrupo = "";

            for (int i = 0; i < arrDDL.Length; i++)
            {
                arrDDL[i].Items.Clear();
                arrDDL[i].DataTextField = "descripcion"; //"para_vDescripcion";
                arrDDL[i].DataValueField = "id"; // "para_sParametroId";

                DataTable dt = new DataTable();

                strGrupo = Util.ObtenerNombreGrupoParametro(arrENMGrupo[i]);
                dt = ObtenerParametrosFiltrandoGrupo(strGrupo, dtGrupoParametros);

                arrDDL[i].DataSource = dt;
                if (bItemAdicional)
                {
                    arrDDL[i].AppendDataBoundItems = bItemAdicional;
                    arrDDL[i].Items.Insert(0, new ListItem(strItemTexto, "0"));
                }
                arrDDL[i].DataBind();
            }
        }
        
        public static void CargarParametroDropDownList(DropDownList ddlList, DataTable dtTable, bool bItemAdicional = false, string strItemTexto = "- SELECCIONAR -")
        {
            ddlList.Items.Clear();


            ddlList.DataTextField = "descripcion"; //"para_vDescripcion";
            ddlList.DataValueField = "id"; // "para_sParametroId";
            ddlList.DataSource = dtTable;
            
            if (bItemAdicional)
            {
                ddlList.AppendDataBoundItems = bItemAdicional;
                ddlList.Items.Insert(0, new ListItem(strItemTexto, "0"));
            }
            ddlList.DataBind();
        }

        public static void CargarParametroDropDownListDATATABLE(DropDownList ddlList, DataTable dtTable, string DataTextField, string DataValueField)
        {
            ddlList.Items.Clear();


            ddlList.DataTextField = DataTextField; //"para_vDescripcion";
            ddlList.DataValueField = DataValueField; // "para_sParametroId";
            ddlList.DataSource = dtTable;

            ddlList.DataBind();
        }

        public static GridView BorrarGrillaColumnas(GridView grdGrilla)
        {
            while (grdGrilla.Columns.Count > 1)
            {
                grdGrilla.Columns.RemoveAt(1);
            }

            if (grdGrilla.Columns.Count == 1)
                grdGrilla.Columns.RemoveAt(0);

            grdGrilla.DataBind();
            return grdGrilla;
        }
        #endregion

        #region Seguridad
        public static string ObtenerHostName()
        {
            string domainName = string.Empty;
            try
            {
                domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            catch
            {
            }
            string fqdn = "localhost";
            try
            {
                fqdn = System.Net.Dns.GetHostName();
                if (!string.IsNullOrEmpty(domainName))
                {
                    if (!fqdn.ToLowerInvariant().EndsWith("." + domainName.ToLowerInvariant()))
                    {
                        //fqdn += "." + domainName;                        
                    }
                }
            }
            catch
            {
            }
            return fqdn;
        }

        public static string ObtenerDireccionIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        #endregion

        #region DataTableCrear
        /// <summary>
        /// Permite crear un DataTable
        /// </summary>
        /// <param name="DataTableColumnas">Nombre del objeto DataTable que se a clonar DataSet. Este array debe de contener 2 Columnas donde se designara la siguiente información. Columna 0 = Indica el nombre del campo que se agregar, Columna 1 = Indica el tipo de dato del campo</param>
        /// <param name="DataTableNombre">Array de parametros para generar las columnas del datatable</param>
        /// Ejemplo: string[,] Columnas = new string[5, 2] { {"tick_ITicketId", "System.Int16"}, {"tick_IOficinaConsularId", "System.Int32"},{"tick_ITipoServicioId", "System.String"},{"tick_IPersonalId", "System.Decimal"}, {"tick_IPersonalId2", "System.DateTime"}};
        /// <returns>DataTable</returns>
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
                vNombreCampo = DataTableColumnas[intFila, 0].ToString();
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

            }

            dtResult.TableName = DataTableNombre;
            return dtResult;
        }
        #endregion DataTableCrear

        #region DataTableFiltrar
        /// <summary>
        /// Permite agregar las columnas de un DataTable a otro DataTable
        /// </summary>
        /// <param name="DtTablaOrigen">Nombre del objeto DataTable del cual se extraeran las columnas</param>
        /// <param name="StrFiltro"></param>
        /// <param name="StrOrden"></param>
        /// Ejemplo: DtResult = MiFun.DatatTableFiltrar(DtResult, "tick_ITipoEstado = 0", "tick_INumero ASC");
        /// <returns>DataTable</returns>
        public static DataTable DataTableFiltrar(DataTable DtTablaOrigen, string StrFiltro, string StrOrden)
        {
            DataRow[] DrwRows;
            DataTable dtNew;

            try
            {
                dtNew = DtTablaOrigen.Clone();
                DrwRows = DtTablaOrigen.Select(StrFiltro, StrOrden);

                foreach (DataRow dr in DrwRows)
                {
                    dtNew.ImportRow(dr);
                }
                return dtNew;
            }

            catch (Exception ex)
            {
                throw new Exception("FiltrarDataTable" + " – " + ex.Source + " – " + ex.Message);
            }
        }
        #endregion DatatTableFiltrar

        #region DropDownList
        /// <summary>
        /// Carga un DropDownList con datos provenientes de un datatable
        /// </summary>
        /// <param name="DropDownControl">Referencia a un Control DropDownControl</param>
        /// <param name="CampoIndice">Nombre del campo indice del datatable</param>
        /// <param name="CampoDescripcion">Nombre del campo descricpion del datatable</param>
        /// <param name="DtDatos">Datatable de donde se cargaran los datos</param>
        public void DropDownListLLenar(ref DropDownList DropDownControl, string CampoIndice, string CampoDescripcion, DataTable DtDatos)
        {
            DropDownControl.DataValueField = CampoIndice;
            DropDownControl.DataTextField = CampoDescripcion;
            DropDownControl.DataSource = DtDatos;
            DropDownControl.DataBind();

            // INSERTAMOS UNA FILA VACIA PARA PODER MOSTRAR EL COMBO EN BLANCO
            DropDownControl.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            //DropDownControl.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }


        public static void DropDownListEliminarRegistro(DropDownList ddl, string Value)
        {
            for (int i = ddl.Items.Count - 1; i >= 0; i--)
            {
                if (ddl.Items[i].Value == Value)
                {
                    ddl.Items.RemoveAt(i);
                }
            }
        }
        #endregion DropDownList

        #region cifrarTextoAES
        /// <summary>
        /// Encripta una cadena de texto
        /// </summary>
        /// <param name="textoCifrar">Texto a cifrar</param>
        /// <param name="palabraPaso">Palabra clave para decifrar</param>
        /// <param name="valorRGBSalt"></param>
        /// <param name="algoritmoEncriptacionHASH">Algoritmo de encriptacion</param>
        /// <param name="iteraciones"></param>
        /// <param name="vectorInicial"></param>
        /// <param name="tamanoClave"></param>
        /// <returns>Cadena Cifrada</returns>
        public static string cifrarTextoAES(string textoCifrar, string palabraPaso,
                  string valorRGBSalt, string algoritmoEncriptacionHASH,
                  int iteraciones, string vectorInicial, int tamanoClave)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(vectorInicial);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(valorRGBSalt);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoCifrar);

                PasswordDeriveBytes password =
                    new PasswordDeriveBytes(palabraPaso, saltValueBytes,
                        algoritmoEncriptacionHASH, iteraciones);

                byte[] keyBytes = password.GetBytes(tamanoClave / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor =
                    symmetricKey.CreateEncryptor(keyBytes, InitialVectorBytes);

                MemoryStream memoryStream = new MemoryStream();

                CryptoStream cryptoStream =
                    new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] cipherTextBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                string textoCifradoFinal = Convert.ToBase64String(cipherTextBytes);

                return textoCifradoFinal;
            }
            catch
            {
                return null;
            }
        }
        #endregion cifrarTextoAES

        #region descifrarTextoAES
        /// <summary>
        /// Desencripta una cadena de texto
        /// </summary>
        /// <param name="textoCifrado">Texto a cifrar</param>
        /// <param name="palabraPaso">Palabra clave para decifrar</param>
        /// <param name="valorRGBSalt"></param>
        /// <param name="algoritmoEncriptacionHASH">Algoritmo de encriptacion</param>
        /// <param name="iteraciones"></param>
        /// <param name="vectorInicial"></param>
        /// <param name="tamanoClave"></param>
        /// <returns></returns>
        public static string descifrarTextoAES(string textoCifrado, string palabraPaso,
            string valorRGBSalt, string algoritmoEncriptacionHASH,
            int iteraciones, string vectorInicial, int tamanoClave)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(vectorInicial);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(valorRGBSalt);

                byte[] cipherTextBytes = Convert.FromBase64String(textoCifrado);

                PasswordDeriveBytes password =
                    new PasswordDeriveBytes(palabraPaso, saltValueBytes,
                        algoritmoEncriptacionHASH, iteraciones);

                byte[] keyBytes = password.GetBytes(tamanoClave / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, InitialVectorBytes);

                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();

                string textoDescifradoFinal = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                return textoDescifradoFinal;
            }
            catch
            {
                return null;
            }
        }
        #endregion descifrarTextoAES

        #region IMG_ConvImagenToByte
        /// <summary>
        /// Convierte a bytes una imagen
        /// </summary>
        /// <param name="imageToConvert">Imagen a convertir a bytes</param>
        /// <param name="formatOfImage">Formato de la imagen que se convertira
        /// System.Drawing.Imaging.ImageFormat.Bmp
        /// System.Drawing.Imaging.ImageFormat.Jpeg
        /// System.Drawing.Imaging.ImageFormat.Png
        /// System.Drawing.Imaging.ImageFormat.MemoryBmp
        /// </param>
        /// <returns></returns>
        public byte[] IMG_ConvImagenToByte(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
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
        /// <summary>
        /// Convierte en un array e bytes la imagen almacenada en un archivo
        /// </summary>
        /// <param name="rutaArchivo">Ruta Fisica del archivo de imagen que se va a cargar</param>
        /// <returns>Array Byte</returns>
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
        /// <summary>
        /// Convierte en imagen un array de bytes
        /// </summary>
        /// <param name="ArregloImagen">Arreglo de bytes a convertir a imagen</param>
        /// <returns>Image</returns>
        public System.Drawing.Image IMG_ConvByteToImagen(byte[] ArregloImagen)
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
            catch
            {
                return null;
            }
        }
        #endregion IMG_ConvByteToImagen

        #region Configuracion TABS

        public static string NombrarTab(int intTabIndice, string strTabNombre, string strTabId = "tabs")
        {
            string strScript = @"$(function(){{";
            strScript += "$('#" + strTabId + " ul:first li:eq(" + intTabIndice + ") a').text('" + strTabNombre + "');";
            strScript += "}});";
            return strScript;
        }
        public static string ActivarTab(int intTabIndice, string strTabNombre = "Registro", string strTabId = "tabs")
        {
            string strScript = @"$(function(){{
                                $('#" + strTabId + "').enableTab(" + intTabIndice + ");";
            strScript += "$('#" + strTabId + " ul:first li:eq(" + intTabIndice + ") a').text('" + strTabNombre + "');";
            strScript += "}});";
            return strScript;
        }
        public static string HabilitarTab(int intTabIndice, string strTabId = "tabs")
        {
            string script = @"$(function(){{
                                $('#" + strTabId + "').enableTab(" + intTabIndice + ");";
            script += "}});";
            return script;
        }
        public static string HabilitarSoloTab(int intTabIndice, string strTabId = "tabs")
        {
            string script = @"$(function(){{
                                $('#" + strTabId + "').justEnableTab(" + intTabIndice + ");";
            script += "}});";
            return script;
        }
        public static string DeshabilitarSoloTab(int intTabIndice, string strTabId = "tabs")
        {
            string script = @"$(function(){{
                                $('#" + strTabId + "').disableTab(" + intTabIndice + ");";
            script += "}});";
            return script;
        }


        public static string DeshabilitarTab(int intTabIndice, string strTabId = "tabs", bool bolEsconder = false)
        {
            string script = @"$(function(){{
                                $('#" + strTabId + "').disableTab(" + intTabIndice + "," + bolEsconder.ToString().ToLower() + ");";
            script += "}});";
            return script;
        }
        public static string OcultarTab(int intTabIndice, string strTabId = "tabs")
        {
            string script = @"$(function(){{";
            script += "$('#" + strTabId + "').disableTab(" + intTabIndice + ",true);";
            script += "}});";
            return script;
        }

        public static string MoverTab(int intTabIndice, string strTabId = "tabs", string strOption = "option", string strActive = "active")
        {
            string script = @"$(function(){{";
            script += "$('#" + strTabId + "').tabs('" + strOption + "',  + '" + strActive + "', " + intTabIndice + ");";
            script += "}});";
            return script;
        }

        #endregion

        #region Parametros
        public static string ObtenerNombreGrupoParametro(Enumerador.enmGrupo enmGrupo)
        {
            string strNombreGrupo = string.Empty;
            switch (enmGrupo)
            {
                case Enumerador.enmGrupo.GRUPO:
                    strNombreGrupo = "GRUPO";
                    break;

                #region Configuracion
                case Enumerador.enmGrupo.CONFIG_PROCESOS_SISTEMA:
                    strNombreGrupo = "CONFIGURACIÓN-PROCESOS SISTEMA";
                    break;                
                case Enumerador.enmGrupo.CONFIG_MES:
                    strNombreGrupo = "CONFIGURACIÓN-MES";
                    break;
                case Enumerador.enmGrupo.CONFIG_EMISION_DOC_MRE:
                    strNombreGrupo = "CONFIGURACIÓN-EMISIÓN DOCUMENTOS MRE";
                    break;
                case Enumerador.enmGrupo.CONFIG_TIPO_LOCACION:
                    strNombreGrupo = "CONFIGURACIÓN-TIPO LOCACIÓN";
                    break;
                case Enumerador.enmGrupo.CONFIG_ROL_LOCACION:
                    strNombreGrupo = "CONFIGURACIÓN-ROL LOCACIÓN";
                    break;
                case Enumerador.enmGrupo.CONFIG_CATEGORIA_OFICINA_CONSULAR:
                    strNombreGrupo = "CONFIGURACIÓN-CATEGORÍA OFICINA CONSULAR";
                    break;
                case Enumerador.enmGrupo.CONFIG_ATRIBUTOS_OFICINA_CONSULAR:
                    strNombreGrupo = "CONFIGURACIÓN-ATRIBUTOS OFICINA CONSULAR";
                    break;
                case Enumerador.enmGrupo.CONFIG_TABLAS:
                    strNombreGrupo = "CONFIGURACIÓN-TABLAS";
                    break;
                case Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO:
                    strNombreGrupo = "CONFIGURACIÓN-CORREO";
                    break;
                case Enumerador.enmGrupo.CONFIG_TIPO_ROL:
                    strNombreGrupo = "CONFIGURACIÓN-TIPO ROL";
                    break;
                case Enumerador.enmGrupo.CONFIG_IDIOMA:
                    strNombreGrupo = "CONFIGURACIÓN-IDIOMA";
                    break;
                case Enumerador.enmGrupo.CONFIG_RUTA_COMPARTIDA:
                    strNombreGrupo = "CONFIGURACIÓN-RUTA COMPARTIDA";
                    break;
                case Enumerador.enmGrupo.CONFIG_DIFERENCIA_HORARIA:
                    strNombreGrupo = "CONFIGURACIÓN-DIFERENCIA HORARIA";
                    break;

                #endregion

                #region Seguridad
                    
                case Enumerador.enmGrupo.SEGURIDAD_TIPO_OPERADOR:
                    strNombreGrupo = "SEGURIDAD-TIPO OPERADOR";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_TIPO_OPERACION:
                    strNombreGrupo = "SEGURIDAD-OPERACIÓN TIPO";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_RESULTADO_OPERACION:
                    strNombreGrupo = "SEGURIDAD-OPERACIÓN RESULTADO";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_RESULTADO_INCIDENCIA:
                    strNombreGrupo = "SEGURIDAD-OPERACIÓN INCIDENCIA";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_SERVICIOS:
                    strNombreGrupo = "SEGURIDAD-SERVICIOS";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_GRUPOS:
                    strNombreGrupo = "SEGURIDAD-GRUPOS";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_ENTIDAD:
                    strNombreGrupo = "SEGURIDAD-ENTIDAD";
                    break;
                case Enumerador.enmGrupo.SEGURIDAD_TIPO_AUTENTICACION:
                    strNombreGrupo = "SEGURIDAD-TIPO AUTENTICACIÓN";
                    break;
                #endregion

                #region Sistema

                case Enumerador.enmGrupo.SISTEMA_ACCESOS:
                    strNombreGrupo = "SISTEMA-ACCESOS";
                    break;

                #endregion

                #region Persona

                case Enumerador.enmGrupo.PERSONA_GENERO:
                    strNombreGrupo = "PERSONA-GÉNERO";
                    break;
                case Enumerador.enmGrupo.PERSONA_NACIONALIDAD:
                    strNombreGrupo = "PERSONA-NACIONALIDAD";
                    break;
                case Enumerador.enmGrupo.PERSONA_TIPO:
                    strNombreGrupo = "PERSONA-TIPO";
                    break;
                case Enumerador.enmGrupo.PERSONA_GRADO_INSTRUCCION:
                    strNombreGrupo = "PERSONA-GRADO INSTRUCCIÓN";
                    break;
                case Enumerador.enmGrupo.PERSONA_TIPO_VINCULO:
                    strNombreGrupo = "PERSONA-TIPO VÍNCULO";
                    break;
                case Enumerador.enmGrupo.PERSONA_TIPO_RESIDENCIA:
                    strNombreGrupo = "PERSONA-TIPO RESIDENCIA";
                    break;
                case Enumerador.enmGrupo.PERSONA_GRUPO_SANGUINEO:
                    strNombreGrupo = "PERSONA-GRUPO SANGUÍNEO";
                    break;
                case Enumerador.enmGrupo.PERSONA_COLOR_OJOS:
                    strNombreGrupo = "PERSONA-COLOR OJOS";
                    break;
                case Enumerador.enmGrupo.PERSONA_COLOR_TEZ:
                    strNombreGrupo = "PERSONA-COLOR TEZ";
                    break;
                case Enumerador.enmGrupo.PERSONA_COLOR_CABELLO:
                    strNombreGrupo = "PERSONA-COLOR CABELLO";
                    break;
                case Enumerador.enmGrupo.PERSONA_ORIGEN_DATOS:
                    strNombreGrupo = "PERSONA-ORIGEN DATOS";
                    break;
                case Enumerador.enmGrupo.PERSONA_RELACION_CONTACTO:
                    strNombreGrupo = "PERSONA-RELACIÓN CONTACTO";
                    break;
                case Enumerador.enmGrupo.PERSONA_TIPO_IMAGEN:
                    strNombreGrupo = "PERSONA-TIPO IMAGEN";
                    break;
                case Enumerador.enmGrupo.EMPRESA_TIPO:
                    strNombreGrupo = "EMPRESA - TIPO";
                    break;
                case Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO:
                    strNombreGrupo = "EMPRESA - TIPO DOCUMENTO";
                    break;
                case Enumerador.enmGrupo.PERSONA_TIPO_INCAPACIDAD:
                    strNombreGrupo = "PERSONA-TIPO INCAPACIDAD";
                    break;
                #endregion

                #region Tarifario

                case Enumerador.enmGrupo.TARIFARIO_TIPO_CALCULO:
                    strNombreGrupo = "TARIFARIO-TIPO CALCULO";
                    break;
                case Enumerador.enmGrupo.TARIFARIO_TIPO_FONDO:
                    strNombreGrupo = "TARIFARIO-TIPO FONDO";
                    break;
                case Enumerador.enmGrupo.TARIFARIO_TOPE_UNIDAD:
                    strNombreGrupo = "TARIFARIO-TOPE UNIDAD";
                    break;

                #endregion

                #region Acreditacion

                case Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO:
                    strNombreGrupo = "ACREDITACIÓN-TIPO COBRO";
                    break;

                #endregion

                #region Actuación

                //case Enumerador.enmGrupo.ACTUACION_UNIDAD_MONETARIA:
                //    strNombreGrupo = "ACTUACIÓN-UNIDAD MONETARIA";
                //    break;
                case Enumerador.enmGrupo.ACTUACION_TIPO_REGISTRO:
                    strNombreGrupo = "ACTUACIÓN-TIPO REGISTRO";
                    break;
                case Enumerador.enmGrupo.ACTUACION_TIPO_ANOTACION:
                    strNombreGrupo = "ACTUACIÓN-TIPO ANOTACIÓN";
                    break;
                case Enumerador.enmGrupo.ACTUACION_TIPO_VISA:
                    strNombreGrupo = "ACTUACIÓN-TIPO VISA";
                    break;
                case Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO:
                    strNombreGrupo = "ACTUACIÓN-TIPO ADJUNTO";
                    break;
                case Enumerador.enmGrupo.ACTUACION_REQUISITO_CONDICION:
                    strNombreGrupo = "ACTUACIÓN-REQUISITO CONDICIÓN";
                    break;
                //case Enumerador.enmGrupo.ACTUACION_MOTIVO_ANOTACION:
                //    strNombreGrupo = "ACTUACION-MOTIVO ANOTACION";
                //    break;
                #endregion

                #region Registro Migratorio
                case Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA TIPO";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_RESIDENTE:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA TIPO RESIDENTE";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_TEMPORAL:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA TIPO TEMPORAL";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_TITULAR_FAMILIA:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA TITULAR/FAMILIA";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_DOCUMENTO_RREE:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA TIPO DOCUMENTO RREE";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_DOCUMENTO_DIGEMIN:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA TIPO DOCUMENTO DIGEMIN";
                    break;  
                case Enumerador.enmGrupo.ACTO_MIGRATORIO_TIPO_NRO_PASAPORTE:
                    strNombreGrupo = "ACTO MIGRATORIO - TIPO NRO PASAPORTE";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_PASAPORTE_TIPO:
                    // Pendiente
                    strNombreGrupo = "ACTO MIGRATORIO - PASAPORTE TIPO";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_AUTORIZACION:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA AUTORIZACIÓN";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_CARGO_PRENSA:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA CARGO-PRENSA";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_CARGO_FUNCIONARIO:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA CARGO-FUNCIONARIO";
                    break;
                case Enumerador.enmGrupo.REG_MIGRA_VISA_CARGO_DIPLOMATIVO:
                    strNombreGrupo = "ACTO MIGRATORIO - VISA CARGO-DIPLOMATICO";
                    break;
                case Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVOS:
                    strNombreGrupo = "ACTO MIGRATORIO - MOTIVOS HISTORICO";
                    break;
                case Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVOS_ANULAR:
                    strNombreGrupo = "ACTO MIGRATORIO - MOTIVOS ANULACIÓN HISTORICO";
                    break;
                case Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVO_CANCELADO:
                    strNombreGrupo = "ACTO MIGRATORIO - MOTIVOS CANCELACION";
                    break;
                case Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVO_RECHAZO :
                    strNombreGrupo = "ACTO MIGRATORIO - MOTIVOS RECHAZO";
                    break;
                case Enumerador.enmGrupo.ACTO_MIGRATORIO_DOCUMENTOS:
                    strNombreGrupo = "ACTO MIGRATORIO - DOCUMENTOS";
                    break;
                #endregion

                #region Registro Civil
                case Enumerador.enmGrupo.REG_CIVIL_TIPO_RECONOCIMIENTO:
                    strNombreGrupo = "REGISTRO CIVIL-TIPO RECONOCIMIENTO";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_NACIMIENTO_LUGAR:
                    strNombreGrupo = "REGISTRO CIVIL-NACIMIENTO LUGAR";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_NACIMIENTO_DECLARANTE:
                    strNombreGrupo = "REGISTRO CIVIL-NACIMIENTO DECLARANTE";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_NACIMIENTO_MODIFICACION:
                    strNombreGrupo = "REGISTRO CIVIL-NACIMIENTO MODIFICACIÓN";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_MATRIMONIO_REGISTRO:
                    strNombreGrupo = "REGISTRO CIVIL-MATRIMONIO REGISTRO";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_TRAMITE_DNI_REGISTRO:
                    strNombreGrupo = "REGISTRO CIVIL-TRÁMITE DNI REGISTRO";
                    break;
                case Enumerador.enmGrupo.ACTO_CIVIL_PARTICIPANTE_TIPO_DATO:
                    strNombreGrupo = "REGISTRO CIVIL - PARTICIPANTE TIPO DATO";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO:
                    strNombreGrupo = "REGISTRO CIVIL - PARTICIPANTE TIPO NACIMIENTO";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO:
                    strNombreGrupo = "REGISTRO CIVIL - PARTICIPANTE TIPO MATRIMONIO";
                    break;
                case Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION:
                    strNombreGrupo = "REGISTRO CIVIL - PARTICIPANTE TIPO DEFUNCION";
                    break;
                #endregion

                #region Registro Militar

                case Enumerador.enmGrupo.REG_MILITAR_FUERZA_ARMADA:
                    strNombreGrupo = "REGISTRO MILITAR-FUERZA ARMADA";
                    break;
                case Enumerador.enmGrupo.REG_MILITAR_ENTREGA_DOC:
                    strNombreGrupo = "REGISTRO MILITAR-ENTREGA DOCUMENTO";
                    break;
                case Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_CALIFICACION:
                    strNombreGrupo = "REGISTRO MILITAR-TIPO CALIFICACIÓN";
                    break;
                case Enumerador.enmGrupo.REGISTRO_MILITAR_SERVICIO_RESERVA_MILITAR:
                    strNombreGrupo = "REGISTRO MILITAR-SERVICIO RESERVA MILITAR";
                    break;
                case Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_PARTICIPANTE:
                    strNombreGrupo = "REGISTRO MILITAR-TIPO PARTICIPANTE";
                    break;
                case Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_INSCRIPCION:
                    strNombreGrupo = "REGISTRO MILITAR - TIPO INSCRIPCION";
                    break;
                #endregion

                #region Registro Notarial

                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO:
                    strNombreGrupo = "REGISTRO NOTARIAL-TIPO ACTO";
                    break;
                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTOR:
                    strNombreGrupo = "REGISTRO NOTARIAL - TIPO ACTOR";
                    break;
                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_EXTRAPROTOCOLAR:
                    strNombreGrupo = "REGISTRO NOTARIAL- EXTRAPROTOCOLAR TIPO";
                    break;
                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR:
                    strNombreGrupo = "REGISTRO NOTARIAL- PROTOCOLAR TIPO";
                    break;

                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACCION_ACTO_PROTOCOLAR:
                    strNombreGrupo = "REGISTRO NOTARIAL- PROTOCOLAR ACCION MODIFICATORIA";
                    break;

                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_INFORMACION_INSERTO:
                    strNombreGrupo = "ACTO NOTARIAL - TIPO INFORMACION";
                    break;
                case Enumerador.enmGrupo.REG_NOTARIAL_SUB_TIPO_INFORMACION_INSERTO:
                    strNombreGrupo = "ACTO NOTARIAL - SUB TIPO INFORMACION";
                    break;
                case Enumerador.enmGrupo.REG_NOTARIAL_IDIOMA:
                    strNombreGrupo = "ACTO NOTARIAL - IDIOMA";
                    break;
                case Enumerador.enmGrupo.REG_NOTARIAL_TIPO_LIBRO:
                    strNombreGrupo = "REGISTRO NOTARIAL- LIBRO TIPO";
                    break;
                case Enumerador.enmGrupo.REGISTRO_NOTARIAL_PROTOCOLAR_TIPO_PARTICIPANTE:
                    strNombreGrupo = "REGISTRO NOTARIAL- PROTOCOLAR TIPO PARTICIPANTE";
                    break;
                case Enumerador.enmGrupo.AVISOS:
                    strNombreGrupo = "AVISOS";
                    break;
                
                #endregion

                #region Contabilidad

                case Enumerador.enmGrupo.CONTA_TIPO_LIBRO:
                    strNombreGrupo = "CONTABILIDAD-TIPO LIBRO";
                    break;
                case Enumerador.enmGrupo.CONTA_TIPO_ABONO:
                    strNombreGrupo = "CONTABILIDAD-TIPO MOVIMIENTO TRANSACCIÓN";
                    break;
                case Enumerador.enmGrupo.CONTA_TIPO_TRANSACCION:
                    strNombreGrupo = "CONTABILIDAD-TIPO TRANSACCIÓN";
                    break;
                case Enumerador.enmGrupo.CONTA_TIPO_CUENTA:
                    strNombreGrupo = "CONTABILIDAD-TIPO CUENTA";
                    break;
                case Enumerador.enmGrupo.CONTA_SITUACION_CUENTA:
                    strNombreGrupo = "CONTABILIDAD-SITUACIÓN CUENTA";
                    break;
                case Enumerador.enmGrupo.CONTA_TIPO_REMESA_DETALLE:
                    strNombreGrupo = "CONTABILIDAD-TIPO REMESA DETALLE";
                    break;
                case Enumerador.enmGrupo.CONTA_TIPO_INGRESO:
                    strNombreGrupo = "CONTABILIDAD-TIPO INGRESO";
                    break;
                case Enumerador.enmGrupo.CONTA_TIPO_EGRESO:
                    strNombreGrupo = "CONTABILIDAD-TIPO EGRESO";
                    break;
                case Enumerador.enmGrupo.CONTA_PLAZO_ENVIO_REMESA:
                    strNombreGrupo = "CONTABILIDAD-PLAZO ENVÍO REMESA";
                    break;
                #endregion

                #region Almacen

                case Enumerador.enmGrupo.ALMACEN_TIPO_BOVEDA:
                    strNombreGrupo = "ALMACÉN-TIPO BÓVEDA";
                    break;
                case Enumerador.enmGrupo.ALMACEN_TIPO_MOVIMIENTO:
                    strNombreGrupo = "ALMACÉN-MOVIMIENTO TIPO";
                    break;
                case Enumerador.enmGrupo.ALMACEN_MOTIVO_MOVIMIENTO:
                    strNombreGrupo = "ALMACÉN-MOVIMIENTO MOTIVO";
                    break;
                case Enumerador.enmGrupo.ALMACEN_TIPO_PEDIDO:
                    strNombreGrupo = "ALMACÉN-PEDIDO TIPO";
                    break;
                case Enumerador.enmGrupo.ALMACEN_MOTIVO_PEDIDO:
                    strNombreGrupo = "ALMACÉN-PEDIDO MOTIVO";
                    break;
                case Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO:
                    strNombreGrupo = "ALMACÉN-INSUMO TIPO";
                    break;
                case Enumerador.enmGrupo.ALMACEN_TIPO_REPORTE:
                    strNombreGrupo = "ALMACÉN-TIPO REPORTE";
                    break;


                #endregion

                #region Colas

                case Enumerador.enmGrupo.COLAS_TIPO_REPORTE:
                    strNombreGrupo = "COLAS-TIPO REPORTE";
                    break;
                case Enumerador.enmGrupo.COLAS_TIPO_SERVICIO:
                    strNombreGrupo = "COLAS-TIPO SERVICIO";
                    break;
                case Enumerador.enmGrupo.COLAS_PROCESOS:
                    strNombreGrupo = "COLAS-PROCESOS";
                    break;
                case Enumerador.enmGrupo.COLAS_ACCION_VENTANILLA:
                    strNombreGrupo = "COLAS-ACCIÓN VENTANILLA";
                    break;
                case Enumerador.enmGrupo.COLAS_SECCION_CONFIG:
                    strNombreGrupo = "COLAS-SECCIÓN CONFIGURACIÓN";
                    break;
                case Enumerador.enmGrupo.COLAS_PANTALLAS_SISTEMA:
                    strNombreGrupo = "COLAS-PANTALLAS DEL SISTEMA";
                    break;
                case Enumerador.enmGrupo.COLAS_REPORTE_TICKETS:
                    strNombreGrupo = "COLAS-REPORTE DE TICKETS";
                    break;
//                case Enumerador.enmGrupo.COLAS_TIPO_ATENCION:
//                    strNombreGrupo = "COLAS-TIPO ATENCIÓN";
                //-------------------------------------------------------
                // Fecha: 27/02/2017
                // Autor: Miguel Márquez Beltrán
                // Objetivo: Mostrar la descripción: COLAS- SEGEMENTACIÓN CLIENTES
                //-------------------------------------------------------
                case Enumerador.enmGrupo.COLAS_SEGMENTACION_CLIENTE:
                    strNombreGrupo = "COLAS-SEGMENTACIÓN CLIENTES";
                    break;
                //-------------------------------------------------------

                case Enumerador.enmGrupo.COLAS_TIPO_CLIENTE:
                    strNombreGrupo = "COLAS-TIPO CLIENTE";
                    break;
                case Enumerador.enmGrupo.COLAS_IMPRESION_TICKET:
                    strNombreGrupo = "COLAS-IMPRESIÓN TICKET";
                    break;
                case Enumerador.enmGrupo.COLAS_TAMANIO_TICKET:
                    strNombreGrupo = "COLAS-TAMAÑO TICKET";
                    break;
                case Enumerador.enmGrupo.COLAS_OPCION_SERVICIO:
                    strNombreGrupo = "COLAS-OPCION SERVICIO";
                    break;

                case Enumerador.enmGrupo.COLAS_SECTOR_CONSULAR:
                    strNombreGrupo = "COLAS - SECTOR CONSULAR";
                    break;
                case Enumerador.enmGrupo.COLAS_REPORTES:
                    strNombreGrupo = "COLAS-REPORTES";
                    break;

                #endregion
           
                #region Acto Judicial
                case Enumerador.enmGrupo.ACTO_JUDICIAL_EXPEDIENTE_TIPO_NOTIFICACIÓN:
                    strNombreGrupo = "ACTO JUDICIAL - EXPEDIENTE TIPO NOTIFICACIÓN";
                    break;

                case Enumerador.enmGrupo.ACTO_JUDICIAL_EXPEDIENTE_ENTIDAD_SOLICITANTE:
                    strNombreGrupo = "ACTO JUDICIAL - EXPEDIENTE ENTIDAD SOLICITANTE";
                    break;

                case Enumerador.enmGrupo.ACTO_JUDICIAL_NOTIFICACION_TIPO_RECEPCIÓN:
                    strNombreGrupo = "ACTO JUDICIAL - NOTIFICACION TIPO RECEPCIÓN";
                    break;

                case Enumerador.enmGrupo.ACTO_JUDICIAL_NOTIFICACION_VIA_ENVÍO:
                    strNombreGrupo = "ACTO JUDICIAL - NOTIFICACION VIA ENVÍO";
                    break;

                case Enumerador.enmGrupo.ACTO_JUDICIAL_TIPO_PARTICIPANTE:
                    strNombreGrupo = "ACTO JUDICIAL- TIPO PARTICIPANTE";
                    break;

                case Enumerador.enmGrupo.ACTO_JUDICIAL_ACTA_TIPO:
                    strNombreGrupo = "ACTA JUDICIAL - ACTA TIPO";
                    break;

                #endregion

                #region Reportes Gerenciales
                case Enumerador.enmGrupo.REG_REPORTE_GERENCIAL:
                    strNombreGrupo = "REPORTES - GERENCIALES";
                    break;

                #endregion

                #region Reportes Actos Migratorios
                case Enumerador.enmGrupo.REG_REPORTE_MIGRATORIO:
                    strNombreGrupo = "REPORTES - ACTOS MIGRATORIOS";
                    break;

                #endregion

                #region Nuevos Grupos - MRE
                case Enumerador.enmGrupo.TRADUCCION_IDIOMA:
                    strNombreGrupo = "TRADUCCION-IDIOMA";
                    break;
                case Enumerador.enmGrupo.FICHA_REGISTRAL_PARTICIPANTE_MAYOR:
                    strNombreGrupo = "FICHA REGISTRAL - PARTICIPANTE TIPO MAYOR";
                    break;
                case Enumerador.enmGrupo.FICHA_REGISTRAL_PARTICIPANTE_MENOR:
                    strNombreGrupo = "FICHA REGISTRAL - PARTICIPANTE TIPO MENOR";
                    break;
                #endregion

                default:
                    break;
            }

            return strNombreGrupo;
        }
        public static string ObtenerNombreGrupoParametro(Enumerador.enmMaestro enmMaestro)
        {
            string strNombreGrupo = string.Empty;
            switch (enmMaestro)
            {
                case Enumerador.enmMaestro.PROCESO:
                    strNombreGrupo = "PROCESO";
                    break;
                case Enumerador.enmMaestro.CONTINENTE:
                    strNombreGrupo = "CONTINENTE";
                    break;
                case Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD:
                    strNombreGrupo = "DOCUMENTO_IDENTIDAD";
                    break;
                case Enumerador.enmMaestro.OCUPACION:
                    strNombreGrupo = "OCUPACION";
                    break;
                case Enumerador.enmMaestro.PROFESION:
                    strNombreGrupo = "PROFESION";
                    break;
                case Enumerador.enmMaestro.BANCO:
                    strNombreGrupo = "BANCO";
                    break;               
                case Enumerador.enmMaestro.CARGO_FUNCIONARIO:
                    strNombreGrupo = "CARGO_FUNCIONARIO";
                    break;               
                case Enumerador.enmMaestro.SECCION:
                    strNombreGrupo = "CARGO_FUNCIONARIO";
                    break;
                case Enumerador.enmMaestro.BASE_PERCEPCION:
                    strNombreGrupo = "BASE_PERCEPCION";
                    break;
                case Enumerador.enmMaestro.REQUISITO_ACTUACION:
                    strNombreGrupo = "REQUISITO_ACTUACION";
                    break;
                case Enumerador.enmMaestro.PLANTILLA:
                    strNombreGrupo = "PLANTILLA";
                    break;
                case Enumerador.enmMaestro.MONEDA:
                    strNombreGrupo = "MONEDA";
                    break;
                case Enumerador.enmMaestro.CATEGORIA_FUNCIONARIO:
                    strNombreGrupo = "CATEGORIA_FUNCIONARIO";
                    break;
                case Enumerador.enmMaestro.ESTADO_CIVIL:
                    strNombreGrupo = "ESTADO_CIVIL";
                    break;
                case Enumerador.enmMaestro.REG_NOTARIAL_ESTADO_ESCRITURA:
                    strNombreGrupo = "ACTOS-PROTOCOLARES ESTADO";
                    break;

                default:
                    break;
            }
            return strNombreGrupo;
        }
        public static string ObtenerNombreGrupoParametro(Enumerador.enmEstadoGrupo enmEstado)
        {
            string strNombreGrupo = string.Empty;
            switch (enmEstado)
            {
                case Enumerador.enmEstadoGrupo.ACTUACION:
                    strNombreGrupo = "ACTUACIÓN-ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.REMESA:
                    strNombreGrupo = "CONTABILIDAD-ESTADO REMESA";
                    break;
                case Enumerador.enmEstadoGrupo.MOVIMIENTO:
                    strNombreGrupo = "ALMACÉN-MOVIMIENTO ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.PEDIDO:
                    strNombreGrupo = "ALMACÉN-PEDIDO ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.INSUMO:
                    strNombreGrupo = "ALMACÉN-INSUMO ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.TICKET:
                    strNombreGrupo = "COLAS-ESTADO DE TICKETS";
                    break;
                case Enumerador.enmEstadoGrupo.ASISTENCIA:
                    strNombreGrupo = "PERSONA-ASISTENCIA";
                    break;
                case Enumerador.enmEstadoGrupo.PASAPORTE:
                    strNombreGrupo = "ACTO MIGRATORIO - PASAPORTE";
                    break;
                case Enumerador.enmEstadoGrupo.SALVOCONDUCTO:
                    strNombreGrupo = "SALVOCONDUCTO-ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.VISA:
                    strNombreGrupo = "VISA-ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.VISA_TRAMITE:
                    strNombreGrupo = "VISA-ESTADO TRAMITE";
                    break;
                case Enumerador.enmEstadoGrupo.TARIFARIO_ESTADO:
                    strNombreGrupo = "TARIFARIO-ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.JUDICIAL_ESTADO_EXPEDIENTE:
                    strNombreGrupo = "JUDICIAL EXPEDIENTE ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.JUDICIAL_ESTADO_ACTA:
                    strNombreGrupo = "JUDICIAL ACTA ESTADO";
                    break;
                case Enumerador.enmEstadoGrupo.PROTOCOLARES_ESTADO_ESCRITURA:
                    strNombreGrupo = "ACTOS-PROTOCOLARES ESTADO";
                    break; 


                default:
                    break;
            }
            return strNombreGrupo;
        }
        public static string ObtenerNombreGrupoParametro(Enumerador.enmServicio enmServicio)
        {
            string strNombre = string.Empty;
            switch (enmServicio)
            {
                case Enumerador.enmServicio.PAHL:
                    strNombre = "";
                    break;
                case Enumerador.enmServicio.PAH:
                    strNombre = "";
                    break;
                case Enumerador.enmServicio.OTROS:
                    strNombre = "";
                    break;
                default:
                    break;
            }
            return strNombre;
        }
        #endregion

        #region Fecha

        public static void CargarComboDiasPorMes(DropDownList ddlDia, int intMes, int intAnio)
        {
            // Obtener ultimo dia del mes del año
            DateTime datFecha = new DateTime(intAnio, intMes + 1, 1).AddDays(-1);
            int intUltimoDia = datFecha.Day;

            for (int i = 1; i <= intUltimoDia; i++)
            {
                ddlDia.Items.Add(i.ToString().PadLeft(2, '0'));
            }
        }
        public static void CargarComboAnios(DropDownList ddlAnioo, int intAnioInicial, int intAnioFinal)
        {
            ddlAnioo.Items.Clear();
            for (int i = intAnioInicial; i <= intAnioFinal; i++)
            {
                ddlAnioo.Items.Add(i.ToString());
            }
        }

        public static DateTime ObtenerFechaActual(double dblDiferenciaHoraria, double dblHorarioVerano)
        {
            return ObtenerFechaActual(Convert.ToDouble(dblDiferenciaHoraria + dblHorarioVerano));
        }

        public static DateTime ObtenerFechaActual(double dblHorasConsiderar)
        {
            DateTime datFecha = DateTime.UtcNow;
            datFecha = datFecha.AddHours(dblHorasConsiderar);
            return datFecha;
        }

  

        public static string ObtenerFechaParaDocumentoLegalProtocolar(DateTime dfecha, bool bolMayusculas=false)
        {
            string vfechaReturn = string.Empty;
            string strAnioLetras = string.Empty;
            string strDiasLetras = string.Empty;
            string vmes = string.Empty;
                    
            switch (dfecha.Month)
            {
                case 1:
                    vmes = "Enero";
                    break;

                case 2:
                    vmes = "Febrero";
                    break;

                case 3:
                    vmes = "Marzo";
                    break;

                case 4:
                    vmes = "Abril";
                    break;

                case 5:
                    vmes = "Mayo";
                    break;

                case 6:
                    vmes = "Junio";
                    break;

                case 7:
                    vmes = "Julio";
                    break;

                case 8:
                    vmes = "Agosto";
                    break;

                case 9:
                    vmes = "Septiembre";
                    break;

                case 10:
                    vmes = "Octubre";
                    break;

                case 11:
                    vmes = "Noviembre";
                    break;

                case 12:
                    vmes = "Diciembre";
                    break;
            }

//            vfechaReturn = string.Format("a los {0} días del mes de {1} del {2}", odoc.ConvertirNumeroLetras(dfecha.Day.ToString().PadLeft(2, '0'), false), vmes, odoc.ConvertirNumeroLetras(dfecha.Year.ToString(), false));
            strAnioLetras = ConvertirNumeroLetras(dfecha.Year.ToString(), false);
            strDiasLetras = ConvertirNumeroLetras(dfecha.Day.ToString().PadLeft(2, '0'), false);

            if (dfecha.Day == 1)
            {
                vfechaReturn = string.Format("al primer {0} día del mes de {1} del año {2}", "(" + dfecha.Day.ToString().PadLeft(2, '0') + ")", vmes, strAnioLetras);
            }
            else
            {                
                
                vfechaReturn = string.Format("a los {0} días del mes de {1} del año {2}", strDiasLetras + " (" + dfecha.Day.ToString().PadLeft(2, '0') + ")", vmes, strAnioLetras);
            }
            if (bolMayusculas)
                return vfechaReturn.ToUpper();
            else
                return vfechaReturn;

        }

        //---------------------------------------------------
        //Fecha: 21/04/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener [dia] de [Mes largo] del [Año]
        //---------------------------------------------------
        public static string ObtenerDiaMesLargoAnio(DateTime dfecha, bool bolMayusculas = false)
        {
            string vfechaReturn = string.Empty;
            string vmes = string.Empty;

            switch (dfecha.Month)
            {
                case 1:
                    vmes = "Enero";
                    break;

                case 2:
                    vmes = "Febrero";
                    break;

                case 3:
                    vmes = "Marzo";
                    break;

                case 4:
                    vmes = "Abril";
                    break;

                case 5:
                    vmes = "Mayo";
                    break;

                case 6:
                    vmes = "Junio";
                    break;

                case 7:
                    vmes = "Julio";
                    break;

                case 8:
                    vmes = "Agosto";
                    break;

                case 9:
                    vmes = "Septiembre";
                    break;

                case 10:
                    vmes = "Octubre";
                    break;

                case 11:
                    vmes = "Noviembre";
                    break;

                case 12:
                    vmes = "Diciembre";
                    break;
            }

            vfechaReturn = string.Format("{0} de {1} del año {2}", dfecha.Day.ToString().PadLeft(2, '0'), vmes, dfecha.Year.ToString());

            if (bolMayusculas)
                return vfechaReturn.ToUpper();
            else
                return vfechaReturn;

        }

        //---------------------------------------------------
        //Fecha: 09/05/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener [Número del día en letras] (día en numeros) de [Mes largo] del [Año]
        //---------------------------------------------------

        public static string ObtenerNumeroDiaLetrasDiaNumerosMesLargoAnio(DateTime dfecha, bool bolMayusculas = false)
        {
            string vfechaReturn = string.Empty;
            string vmes = string.Empty;
            Documento odoc = new Documento();

            switch (dfecha.Month)
            {
                case 1:
                    vmes = "Enero";
                    break;

                case 2:
                    vmes = "Febrero";
                    break;

                case 3:
                    vmes = "Marzo";
                    break;

                case 4:
                    vmes = "Abril";
                    break;

                case 5:
                    vmes = "Mayo";
                    break;

                case 6:
                    vmes = "Junio";
                    break;

                case 7:
                    vmes = "Julio";
                    break;

                case 8:
                    vmes = "Agosto";
                    break;

                case 9:
                    vmes = "Septiembre";
                    break;

                case 10:
                    vmes = "Octubre";
                    break;

                case 11:
                    vmes = "Noviembre";
                    break;

                case 12:
                    vmes = "Diciembre";
                    break;
            }

            vfechaReturn = string.Format("{0} de {1} del año {2}", ConvertirNumeroLetras(dfecha.Day.ToString(),false) +  " ("+ dfecha.Day.ToString().PadLeft(2, '0') + ")", vmes, dfecha.Year.ToString());

            if (bolMayusculas)
                return vfechaReturn.ToUpper();
            else
                return vfechaReturn;

        }
        
        
        #endregion

        #region Complementario

        public static string ObtenerNameForm()
        {
            string str_Ruta = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            string str_cantidad = str_Ruta.Substring(1, str_Ruta.Length - 1);
            string str_cadena = str_cantidad.Substring(str_cantidad.IndexOf("/"), str_cantidad.Length - str_cantidad.IndexOf("/"));

            return "~" + str_cadena;
        }

        public static int ObtenerIndiceColumnaGrilla(GridView grid, string col)
        {
            string field = string.Empty;
            for (int i = 0; i < grid.Columns.Count; i++)
            {

                if (grid.Columns[i].GetType() == typeof(BoundField))
                {
                    field = ((BoundField)grid.Columns[i]).DataField.ToLower();
                }
                else if (grid.Columns[i].GetType() == typeof(TemplateField))
                {
                    field = ((TemplateField)grid.Columns[i]).HeaderText.ToLower();
                }

                if (field == col.ToLower())
                {
                    return i;
                }

                field = string.Empty;
            }

            return -1;
        }

        public static int ObtenerIndiceCombo(DropDownList ddl, string value)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Value == value)
                    return i;
               
            }

            return -1;
        }

        public static int ObtenerIndiceComboPorText(DropDownList ddl, string value)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Text == value)
                    return i;

            }

            return -1;
        }
        public static string GetSessionVariableValue(System.Web.SessionState.HttpSessionState Session, string strName)
        {
            string strValue = string.Empty;
            try
            {
                if (Session[strName] != null)
                    strValue = Session[strName].ToString();
            }
            catch(FormatException fex)
            {
            }
            return strValue;
        }
        public static string ReemplazarCaracter(string strCadena, string strCaracter = "'")
        {
            string strCadenaActualizada = string.Empty;
            if (strCadena.Trim().Length == 0)
                return strCadena;
            else
            {
                char cCaracter = strCaracter[0];
                char cActual = strCadena[0];
                int intCantidad = 0;
                for (int i = 0; i < strCadena.Length; i++)
                {
                    cActual = strCadena[i];
                    if (cActual == cCaracter)
                    {
                        intCantidad++;
                    }
                    else
                    {
                        if (intCantidad % 2 == 1)
                        {
                            strCadenaActualizada += cCaracter;
                        }
                        intCantidad = 0;
                    }
                    strCadenaActualizada += cActual;
                }
            }
            return strCadenaActualizada;
        }
        public static string GenerarCodigoRandom()
        {
            Random r = new Random();
            string s = "";

            for (int j = 0; j < 5; j++)
            {
                int i = r.Next(3);
                int ch;

                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        s = s + ch.ToString();
                        break;

                    case 2:
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;

                    case 3:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;

                    default:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }

                r.NextDouble();
                r.Next(100, 1999);
            }

            return s;
        }

        /// <summary>
        /// Convierte todos los valores de tipo varchar en mayúsculas
        /// </summary>
        /// <param name="dt"></param>
        public static void DataTableVarcharMayusculas(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].DataType == typeof(string))
                    {
                        dr[i] = dr[i].ToString().ToUpper();
                    }
                }
            }
        }

        /// <summary>
        /// Convierte la imgaen a arreglo de byts
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public static Byte[] ConvertToByteImage(String ruta)
        {
            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Byte[] arreglo = new Byte[foto.Length];
            BinaryReader reader = new BinaryReader(foto);
            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));
            return arreglo;
        }

        /// <summary>
        /// Encripta una cadena
        /// </summary>
        /// <param name="_cadenaAencriptar"></param>
        /// <returns></returns>
        public static string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// <summary>
        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        /// </summary>
        /// <param name="_cadenaAdesencriptar"></param>
        /// <returns></returns>
        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        public static string GenerarLineasubigeoderecha(string des)
        {

            int limite = Convert.ToInt32(ConfigurationManager.AppSettings["LimiteCaracteresderecha"]);
            int cantidad = des.Trim().Length;
            if (cantidad >= limite)
            {
                return des;
            }
            if (cantidad < (limite-4))
            {
                des = des + "----";
            }
            else
            {
                for (int i = cantidad; i < limite; i++)
                {
                    des += "-";
                }
            }

            return des;
        }
        public static string GenerarLineasubigeoizquierda(string des)
        {

            int limite = Convert.ToInt32(ConfigurationManager.AppSettings["LimiteCaracteresizquierda"]);
            int cantidad = des.Trim().Length;
            if (cantidad >= limite)
            {
                return des;
            }
            if (cantidad < (limite - 4))
            {
                des = des + "----";
            }
            else
            {
                for (int i = cantidad; i < limite; i++)
                {
                    des += "-";
                }
            }

            return des;
        }

        public static string GenerarLineasDocumento(string des)
        {

            int limite = Convert.ToInt32(ConfigurationManager.AppSettings["LimiteCaracteresDocumento"]);
            int cantidad = des.Trim().Length;
            if (cantidad >= limite)
            {
                return des;
            }
            //if (cantidad < (limite - 4))
            //{
            //    des = des + "----";
            //}
            else
            {
                for (int i = cantidad; i < limite; i++)
                {
                    des += "-";
                }
            }

            return des;
        }

        public static string GenerarLineasVinculo(string des)
        {

            int limite = Convert.ToInt32(ConfigurationManager.AppSettings["LimiteCaracteresVinculo"]);
            int cantidad = des.Trim().Length;
            if (cantidad >= limite)
            {
                return des;
            }
            if (cantidad < (limite - 4))
            {
                des = des + "----";
            }
            else
            {
                for (int i = cantidad; i < limite; i++)
                {
                    des += "-";
                }
            }
            return des;
        }

        public static bool ContieneItemArreglo(string[] arreglo, string strItem)
        {
            bool bValor = false;

            for (int i = 0; i < arreglo.Length; i++)
            {
                if (arreglo[i].ToString() == strItem)
                {
                    bValor = true;
                    break;
                }
                //bValor = arreglo[i].Contains(strItem);
                //if (bValor)
                //    break;
            }
            return bValor;
        }


        public static string ObtenerMesLargo(string strNumeroMes)
        {
            string strMesLargo="";
            switch (strNumeroMes)
            {
                case "01":
                    strMesLargo = "Enero";
                    break;
                case "02":
                    strMesLargo = "Febrero";
                    break;
                case "03":
                    strMesLargo = "Marzo";
                    break;
                case "04":
                    strMesLargo = "Abril";
                    break;
                case "05":
                    strMesLargo = "Mayo";
                    break;
                case "06":
                    strMesLargo = "Junio";
                    break;
                case "07":
                    strMesLargo = "Julio";
                    break;
                case "08":
                    strMesLargo = "Agosto";
                    break;
                case "09":
                    strMesLargo = "Septiembre";
                    break;
                case "10":
                    strMesLargo = "Octubre";
                    break;
                case "11":
                    strMesLargo = "Noviembre";
                    break;
                case "12":
                    strMesLargo = "Diciembre";
                    break;
            }
            return strMesLargo;
        }

        #endregion

        //-----------------------------------------------------------
        //Fecha: 27/01/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Convertir un número en romanos (Limite: 3999)
        //-----------------------------------------------------------

        public static string ObtenerRomanosMax3999(int intNumero)
        {
            int Miles, Resto, Cen, Dec, Uni, N;

            Miles = intNumero / 1000;
            Resto = intNumero % 1000;
            Cen = Resto / 100;
            Resto = Resto % 100;
            Dec = Resto / 10;
            Resto = Resto % 10;
            Uni = Resto;

            string strMiles = "";

            switch (Miles)
            {
                case 1: strMiles = "M"; break;
                case 2: strMiles = "MM"; break;
                case 3: strMiles = "MMM"; break;
            }

            string strCen = "";

            switch (Cen)
            {
                case 1: strCen = "C"; break;
                case 2: strCen = "CC"; break;
                case 3: strCen = "CCC"; break;
                case 4: strCen = "CD"; break;
                case 5: strCen = "D"; break;
                case 6: strCen = "DC"; break;
                case 7: strCen = "DCC"; break;
                case 8: strCen = "DCCC"; break;
                case 9: strCen = "CM"; break;
            }

            string strDec = "";
            switch (Dec)
            {
                case 1: strDec = "X";
                    break;
                case 2: strDec = "XX";
                    break;
                case 3: strDec = "XXX";
                    break;
                case 4: strDec = "XL";
                    break;
                case 5: strDec = "L";
                    break;
                case 6: strDec = "LX";
                    break;
                case 7: strDec = "LXX";
                    break;
                case 8: strDec = "LXXX";
                    break;
                case 9: strDec = "XC";
                    break;
            }
            string strUni = "";
            switch (Uni)
            {
                case 1: strUni = "I"; break;
                case 2: strUni = "II"; break;
                case 3: strUni = "III"; break;
                case 4: strUni = "IV"; break;
                case 5: strUni = "V"; break;
                case 6: strUni = "VI"; break;
                case 7: strUni = "VII"; break;
                case 8: strUni = "VIII"; break;
                case 9: strUni = "IX"; break;
            }
            return strMiles + strCen + strDec + strUni;
        }

        //-----------------------------------------------------------
        //Fecha: 04/04/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Separar las palabras de las unidades por coma.
        //-----------------------------------------------------------

        public static string ObtenerUnidadesPalabrasComas(string strNumero, bool bMayusculas = false)
        {
            string strUnidades = strNumero.Trim();

            if (strNumero.Length == 0)
            { return strUnidades; }

            strUnidades = "(";
            string strPalabra = "";
            string[] UNIDADES = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };

            for (int i = 0; i < strNumero.Length; i++)
            {
                try
                {
                    strPalabra = UNIDADES[int.Parse(strNumero.Substring(i, 1))];
                }
                catch (Exception)
                {
                    if (strNumero.Substring(i, 1).Equals("-"))
                    {
                        strPalabra = "GUION";
                    }
                    else
                    {
                        strPalabra = strNumero.Substring(i, 1);
                    }
                }

                strUnidades = strUnidades + strPalabra + ", ";
            }

            strUnidades = strUnidades.Substring(0, strUnidades.Length - 2);
            strUnidades = strUnidades + ")";
            if (bMayusculas)
            { strUnidades = strUnidades.ToUpper(); }
            return strUnidades;
        }

        public static String ConvertirNumeroLetras(String numero, bool mayusculas)
        {
            Regex r;
            String literal = "";
            String parte_decimal;
            //si el numero utiliza (.) en lugar de (,) -> se reemplaza
            numero = numero.Replace(".", ",");
            //si el numero no tiene parte decimal, se le agrega ,00
            if (numero.IndexOf(",") == -1)
            {
                numero = numero + ",00";
            }
            //se valida formato de entrada -> 0,00 y 999 999 999,00
            r = new Regex(@"\d{1,9},\d{1,2}");
            MatchCollection mc = r.Matches(numero);
            if (mc.Count > 0)
            {
                //se divide el numero 0000000,00 -> entero y decimal
                String[] Num = numero.Split(',');

                //de da formato al numero decimal
                parte_decimal = Num[1];
                //se convierte el numero a literal
                if (int.Parse(Num[0]) == 0)
                {//si el valor es cero                
                    literal = "cero ";
                }
                else if (int.Parse(Num[0]) > 999999)
                {//si es millon
                    literal = getMillones(Num[0]);
                }
                else if (int.Parse(Num[0]) > 999)
                {//si es miles
                    literal = getMiles(Num[0]);
                }
                else if (int.Parse(Num[0]) > 99)
                {//si es centena
                    literal = getCentenas(Num[0]);
                }
                else if (int.Parse(Num[0]) > 9)
                {//si es decena
                    literal = getDecenas(Num[0]);
                }
                else
                {//sino unidades -> 9
                    literal = getUnidades(Num[0]);
                }
                //devuelve el resultado en mayusculas o minusculas
                if (mayusculas)
                {
                    return (literal).ToUpper();
                }
                else
                {
                    return (literal);
                }
            }
            else
            {//error, no se puede convertir
                return literal = null;
            }
        }

        /* funciones para convertir los numeros a literales */
        private static string getUnidades(String numero)
        {   // 1 - 9            
            //si tuviera algun 0 antes se lo quita -> 09 = 9 o 009=9
            String num = numero.Substring(numero.Length - 1);
            return UNIDADES[int.Parse(num)];
        }

        private static string getDecenas(String num)
        {// 99                        
            int n = int.Parse(num);
            if (n < 10)
            {//para casos como -> 01 - 09
                return getUnidades(num);
            }
            else if (n > 19)
            {//para 20...99
                String u = getUnidades(num);
                if (u.Equals(""))
                { //para 20,30,40,50,60,70,80,90
                    if (n == 20)
                    {
                        return "veinte";
                    }
                    else
                    {
                        return DECENAS[int.Parse(num.Substring(0, 1)) + 8];
                    }
                }
                else
                {
                    if (n >= 31)
                    {
                        return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + " y " + u;
                    }
                    else
                    {
                        if (n == 22)
                        {
                            return "veintidós";
                        }
                        else
                        {
                            return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + "" + u;
                        }
                    }
                }
            }
            else
            {//numeros entre 11 y 19
                return DECENAS[n - 10];
            }
        }

        private static string getCentenas(String num)
        {// 999 o 099
            if (int.Parse(num) > 99)
            {//es centena
                if (int.Parse(num) == 100)
                {//caso especial
                    return "cien";
                }
                else
                {
                    return CENTENAS[int.Parse(num.Substring(0, 1))] + " " + getDecenas(num.Substring(1));
                }
            }
            else
            {//por Ej. 099 
                //se quita el 0 antes de convertir a decenas
                return getDecenas(int.Parse(num) + "");
            }
        }

        private static string getMiles(String numero)
        {// 999 999
            //obtiene las centenas
            String c = numero.Substring(numero.Length - 3);
            //obtiene los miles
            String m = numero.Substring(0, numero.Length - 3);
            String n = "";
            //se comprueba que miles tenga valor entero
            if (int.Parse(m) > 0)
            {
                if (int.Parse(m) == 1)
                {
                    return "mil " + getCentenas(c);
                }
                else
                {
                    n = getCentenas(m);
                    return n + " mil " + getCentenas(c);
                }
            }
            else
            {
                return "" + getCentenas(c);
            }

        }

        private static string getMillones(String numero)
        { //000 000 000        
            //se obtiene los miles
            String miles = numero.Substring(numero.Length - 6);
            //se obtiene los millones
            String millon = numero.Substring(0, numero.Length - 6);
            String n = "";
            if (millon.Length > 1)
            {
                n = getCentenas(millon) + " millones";
            }
            else
            {
                n = getUnidades(millon) + " millon";
            }
            return n + getMiles(miles);
        }
        public static DateTime FormatearFecha(string strFecha)
        {
            DateTime datFecha = new DateTime();

            if (strFecha != null)
            {
                strFecha = strFecha.Replace(".", "");
                strFecha = strFecha.Replace("Ene", "Jan");
                strFecha = strFecha.Replace("Abr", "Apr");
                strFecha = strFecha.Replace("Ago", "Aug");
                strFecha = strFecha.Replace("Set", "Sep");
                strFecha = strFecha.Replace("Dic", "Dec");

                strFecha = strFecha.Replace("ene", "Jan");
                strFecha = strFecha.Replace("abr", "Apr");
                strFecha = strFecha.Replace("ago", "Aug");
                strFecha = strFecha.Replace("set", "Sep");
                strFecha = strFecha.Replace("dic", "Dec");

                strFecha = strFecha.Replace("ENE", "Jan");
                strFecha = strFecha.Replace("ABR", "Apr");
                strFecha = strFecha.Replace("AGO", "Aug");
                strFecha = strFecha.Replace("SET", "Sep");
                strFecha = strFecha.Replace("DIC", "Dec");
            }

            if (!DateTime.TryParse(strFecha, out datFecha))
            {
                datFecha = Convert.ToDateTime(strFecha, System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            }
            return datFecha;
        }    
    }
}
