using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Data;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlOficinaConsular : System.Web.UI.UserControl
    {
        private string _strOficinaConsular = string.Empty;

        public string OficinasConsulares
        {
            get { return _strOficinaConsular; }
            set { _strOficinaConsular = value; }
        }

        public Unit Width
        {
            get { return ddlOficinaConsular.Width; }
            set { ddlOficinaConsular.Width = value; }
        }
        public int SelectedIndex
        {
            get { return ddlOficinaConsular.SelectedIndex; }
            set { ddlOficinaConsular.SelectedIndex = value; }
        }
        public string SelectedValue
        {
            get { return ddlOficinaConsular.SelectedValue; }
            set { ddlOficinaConsular.SelectedValue = value; }
        }
        public ListItem SelectedItem
        {
            get { return ddlOficinaConsular.SelectedItem; }
        }
        public bool Enabled
        {
            get { return ddlOficinaConsular.Enabled; }
            set { ddlOficinaConsular.Enabled = value; }
        }
        //public bool Visible
        //{
        //    get { return ddlOficinaConsular.Visible; }
        //    set { ddlOficinaConsular.Visible = value; }
        //}
        public bool AutoPostBack
        {
            get { return ddlOficinaConsular.AutoPostBack; }
            set { ddlOficinaConsular.AutoPostBack = value; }
        }
        //

        public void CargarContinentePais(bool bolTodas = false, bool bolAgregarItem = true, string strTexto = "- SELECCIONAR -", string strContinente = "", string strPais = "")
        {
            if (bolTodas)
            {
                // Funciona si solo se usa un CONTROL en el formulario
                DataTable dt = new DataTable();

                dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();
                
                //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
                
                //DataView dv = dtOficinasConsulares.DefaultView;
                //dv.Sort = " ofco_iReferenciaPadreId ASC";

                if (strContinente == "")
                {
                    Util.CargarDropDownList(ddlOficinaConsular, dt, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                }
                else
                {
                    DataView dv = dt.DefaultView;

                    if (strPais == "")
                    {
                        dv.RowFilter = " ofco_cUbigeoCodigo in (" + strContinente + ")";
                    }
                    else
                    {
                        //dv.RowFilter = " ofco_cUbigeoCodigo in (" + strContinente + ")";
                        dv.RowFilter = "(ofco_cUbigeoCodigo = " + strContinente + ") AND (ofco_cUbigeoCodigoPais  = " + strPais + ") ";
                    }

                    DataTable dt_1 = dv.ToTable();
                    Util.CargarDropDownList(ddlOficinaConsular, dt_1, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                }
            }
        }

        //jonatan silva cachay
        // 30/11/2017
        // se agrega busqueda por ciudad
        public void CargarContinentePaisCuidad(bool bolTodas = false, bool bolAgregarItem = true, string strTexto = "- SELECCIONAR -", string strContinente = "", string strPais = "", string strCiudad = "")
        {
            if (bolTodas)
            {
                // Funciona si solo se usa un CONTROL en el formulario
                DataTable dt = new DataTable();

                dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();

                //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
                //DataView dv = dtOficinasConsulares.DefaultView;
                //dv.Sort = " ofco_iReferenciaPadreId ASC";

                if (strContinente == "")
                {
                    Util.CargarDropDownList(ddlOficinaConsular, dt, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                }
                else
                {
                    DataView dv = dt.DefaultView;

                    if (strPais == "")
                    {
                        dv.RowFilter = " ofco_cUbigeoCodigo in (" + strContinente + ")";
                    }
                    else
                    {
                        if (strCiudad == "")
                        {
                            dv.RowFilter = "(ofco_cUbigeoCodigo = " + strContinente + ") AND (ofco_cUbigeoCodigoPais  = " + strPais + ") "; 
                        }
                        else 
                        {
                            dv.RowFilter = "(ofco_cUbigeoCodigo = " + strContinente + ") AND (ofco_cUbigeoCodigoPais  = " + strPais + ") AND (ofco_cUbigeoCodigoCiudad  = " + strCiudad + ") "; 
                        }
                        //dv.RowFilter = " ofco_cUbigeoCodigo in (" + strContinente + ")";
                        
                    }

                    DataTable dt_1 = dv.ToTable();
                    Util.CargarDropDownList(ddlOficinaConsular, dt_1, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                }
            }
        }

        public void CargarTodoSin(bool bolAgregarItem = true, string strTexto = "- SELECCIONAR -", string strSinColumna = "", string strSinValor = "")
        {
            // Funciona si solo se usa un CONTROL en el formulario
            DataTable dt = new DataTable();
            dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();
            //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
            //DataView dv = dtOficinasConsulares.DefaultView;
            //dv.Sort = " ofco_iReferenciaPadreId ASC";

            if (strSinColumna == "")
            {
                Util.CargarDropDownList(ddlOficinaConsular, dt, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
            }
            else
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = " " +strSinColumna+ " not in (" + "'" + strSinValor + "'" + ")";

                DataTable dt_1 = dv.ToTable();
                Util.CargarDropDownList(ddlOficinaConsular, dt_1, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
            }
        }

        public void Cargar(bool bolTodas = false, bool bolAgregarItem = true, string strTexto = "- SELECCIONAR -", string strContinente = "")
        {
            if (bolTodas)
            {
                // Funciona si solo se usa un CONTROL en el formulario
                DataTable dt = new DataTable();
                dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();
                //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
                //DataView dv = dtOficinasConsulares.DefaultView;
                //dv.Sort = " ofco_iReferenciaPadreId ASC";

                if (strContinente == "")
                {
                    Util.CargarDropDownList(ddlOficinaConsular, dt, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                }
                else
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = " ofco_cUbigeoCodigo in (" + strContinente + ")";

                    DataTable dt_1 = dv.ToTable();
                    Util.CargarDropDownList(ddlOficinaConsular, dt_1, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                }
            }
            else
            {
                if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != null)
                {
                    int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                    DataTable dtOficinaConsular = new DataTable();

                    dtOficinaConsular = Comun.ObtenerOficinasConsularesCargaInicial();
                    
                    //DataTable dtOficinaConsular = (DataTable)Session[Constantes.CONST_SESION_OFICINACONSULTA_DT];
                    int intUsuarioAccesoId = (int)Session[Constantes.CONST_SESION_ACCESO_ID];

                    //---------------------------------------------
                    //Fecha: 06/01/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Validar cuando el objeto es nulo.
                    //---------------------------------------------
                    string strAccesos = "";
                    object objAcceso = comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_vAccesoOficina");

                    if (objAcceso != null)
                    {
                        strAccesos = objAcceso.ToString();
                    }
                    

                    //string strAccesos = dtOficinaConsular.Rows[0]["ofco_vAccesoOficina"].ToString();
                    string[] arrAcceso = strAccesos.Split(',');

                    //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
                    DataTable dtOficinasConsulares = new DataTable();

                    dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial().Copy();

                    
                    DataView dv = dtOficinasConsulares.DefaultView;

                    // Listar Todas las oficinas Consulares
                    if (intUsuarioAccesoId == (int)Enumerador.enmAccesoUsuario.TOTAL)
                    {
                        if (intOficinaConsularId == (int)Constantes.CONST_OFICINACONSULAR_LIMA)
                        {
                            DataTable dt = new DataTable();
                            dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();

                            //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
                            Util.CargarDropDownList(ddlOficinaConsular, dt, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                        }
                        else
                        {
                            // Acceso a más de una OC
                            if (arrAcceso.Length > 1)
                            {
                                dv.RowFilter = " ofco_sOficinaConsularId in (" + strAccesos + ")";
                                dv.Sort = " ofco_iReferenciaPadreId ASC";
                                DataTable dtOficinaAcceso = dv.ToTable();

                                foreach (DataRow dr in dtOficinaAcceso.Rows)
                                {
                                    if (intOficinaConsularId == Convert.ToInt32(dr["ofco_iReferenciaPadreId"]))
                                    {
                                        dr["ofco_vNombre"] = " - " + dr["ofco_vNombre"].ToString();
                                    }
                                }

                                Util.CargarDropDownList(ddlOficinaConsular, dtOficinaAcceso, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                            }
                            else
                            {   // Acceso solo a su Misión Consular
                                //------------------------------------------------
                                //Fecha: 06/01/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Se valida si accesos esta en blanco.
                                //-------------------------------------------------
                                if (strAccesos.Length > 0)
                                {
                                    dv.RowFilter = " ofco_sOficinaConsularId in (" + strAccesos + ")";
                                }
                                DataTable dtOficinaAcceso = dv.ToTable();
                                Util.CargarDropDownList(ddlOficinaConsular, dtOficinaAcceso, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                            }
                        }
                    }
                    else if (intUsuarioAccesoId == (int)Enumerador.enmAccesoUsuario.PARCIAL)
                    {
                        // Acceso a más de una OC
                        if (arrAcceso.Length > 1)
                        {
                            dv.RowFilter = " ofco_sOficinaConsularId in (" + strAccesos + ")";
                            dv.Sort = " ofco_iReferenciaPadreId ASC";
                            DataTable dtOficinaAcceso = dv.ToTable();

                            foreach (DataRow dr in dtOficinaAcceso.Rows)
                            {
                                if (intOficinaConsularId == Convert.ToInt32(dr["ofco_iReferenciaPadreId"]))
                                {
                                    dr["ofco_vNombre"] = " - " + dr["ofco_vNombre"].ToString();
                                }
                            }

                            Util.CargarDropDownList(ddlOficinaConsular, dtOficinaAcceso, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                        }
                        else
                        {   // Acceso solo a su Misión Consular
                            dv.RowFilter = " ofco_sOficinaConsularId in (" + strAccesos + ")";
                            DataTable dtOficinaAcceso = dv.ToTable();
                            Util.CargarDropDownList(ddlOficinaConsular, dtOficinaAcceso, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                        }
                    }
                    else
                    {
                        strAccesos = arrAcceso[0];
                        // Acceso solo a su Misión Consular
                        dv.RowFilter = " ofco_sOficinaConsularId in (" + strAccesos + ")";
                        DataTable dtOficinaAcceso = dv.ToTable();
                        Util.CargarDropDownList(ddlOficinaConsular, dtOficinaAcceso, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);
                    }
                    ddlOficinaConsular.SelectedValue = intOficinaConsularId.ToString();
                    //
                    OficinasConsulares = strAccesos;
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlOficinaConsular, new DataTable(), bolAgregarItem, strTexto);
                }
            }
        }

        public void CargarPorJefatura(string sOficinaConsularJefaturaId, bool bolAgregarItem = true, string strTexto = "- SELECCIONAR -")
        {

            DataTable dt = new DataTable();

            dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();

            //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();


            int ofco_iReferenciaPadreId = 0;
            var auxReferenciaPadre = dt.Select("ofco_sOficinaConsularId=" + sOficinaConsularJefaturaId);

            if (auxReferenciaPadre.Count() > 0)
            {
                ofco_iReferenciaPadreId = int.Parse(auxReferenciaPadre.ElementAt(0)["ofco_iReferenciaPadreId"].ToString());
            }
            
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ofco_sOficinaConsularId =" + sOficinaConsularJefaturaId + " or ofco_iReferenciaPadreId=" + sOficinaConsularJefaturaId;

            if (dv.ToTable().Rows.Count > 1)//Cuando posee más de un hijo es una jefatura y se incluye lima
            {
                dv.RowFilter += " or ofco_sOficinaConsularId=" + Constantes.CONST_OFICINACONSULAR_LIMA.ToString();
            }
            else
            {
                dv.RowFilter += " or ofco_sOficinaConsularId=" + ofco_iReferenciaPadreId.ToString();
            }

            DataTable dt_1 = dv.ToTable();
            Util.CargarDropDownList(ddlOficinaConsular, dt_1, "ofco_vNombre", "ofco_sOficinaConsularId", bolAgregarItem, strTexto);



        }

        public void CargarPorOficinasConsulares(string strOficinasConsularesId, bool bolSeleccion = false, string strItemTexto = " - SELECCIONAR - ")
        {
            DataTable dtOficinasConsulares = new DataTable();
            dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();
            //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
            DataView dv = dtOficinasConsulares.DefaultView;
            dv.RowFilter = " ofco_sOficinaConsularId in (" + strOficinasConsularesId + ")";
            DataTable dtFiltrado = dv.ToTable();
            Util.CargarDropDownList(ddlOficinaConsular, dtFiltrado, "ofco_vNombre", "ofco_sOficinaConsularId", bolSeleccion, strItemTexto);
        }

        public bool EsPermitidoDestinoLima()
        {
            DataTable dtOficinasConsulares = new DataTable();
            dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();
            
            //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
            DataView dv = dtOficinasConsulares.DefaultView;

            dv.RowFilter = "ofco_sOficinaConsularId in (" + SelectedValue + ") and ofco_iReferenciaPadreId=" + Constantes.CONST_OFICINACONSULAR_LIMA.ToString();

            if (dv.ToTable().Rows.Count > 0) //Entra cuando su padre es Lima
                return true;


            return false;
        }

        public bool EsOficinaJefatura(string strOficinaConsularId)
        {
            DataTable dtOficinasConsulares = new DataTable();
            dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();

            //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
            DataView dv = dtOficinasConsulares.DefaultView;

            dv.RowFilter = "ofco_sOficinaConsularId in (" + strOficinaConsularId + ") and ofco_iReferenciaPadreId=" + Constantes.CONST_OFICINACONSULAR_LIMA.ToString();

            if (dv.ToTable().Rows.Count > 0) //Entra cuando su padre es Lima
                return true;


            return false;
        }

        public bool EsDestinoMiJefatura(string strOficinaConsularDestinoId)
        {
            DataTable dtOficinasConsulares = new DataTable();
            dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();

            //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
            DataView dv = dtOficinasConsulares.DefaultView;

            dv.RowFilter = "ofco_sOficinaConsularId in (" + SelectedValue + ") and ofco_iReferenciaPadreId=" + strOficinaConsularDestinoId;

            if (dv.ToTable().Rows.Count > 0) //Entra cuando destino es su jefatura
                return true;


            return false;
        }

        public bool OficinaPoseeHijos(string strOficinaConsularId)
        {
            DataTable dtOficinasConsulares = new DataTable();
            dtOficinasConsulares = Comun.ObtenerOficinasConsularesCargaInicial();

            //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
            DataView dv = dtOficinasConsulares.DefaultView;

            dv.RowFilter = "ofco_iReferenciaPadreId=" + strOficinaConsularId;

            if (dv.ToTable().Rows.Count > 0) //Entra cuando posee hijos
                return true;


            return false;
        }

    }
}