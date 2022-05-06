using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGAC.BE;
using SGAC.Controlador;
using SGAC.Accesorios;
using System.Data;
using SGAC.Registro.Persona.BL;
using SGAC.Registro.Actuacion.BL;
using System.Configuration;
namespace SGAC.WebApp.Accesorios
{
    public class Persona
    {

        public static EnPersona oPersona(object[] arrParametro)
        {
            try
            {
                EnPersona objEn = new EnPersona();              
                DataTable dtPersona = new DataTable();

                object[] arrParametros = { arrParametro };

                PersonaConsultaBL objBL = new PersonaConsultaBL();
                dtPersona = objBL.Obtener_Persona(arrParametro);

                if (dtPersona.Rows.Count > 0)
                {

                    objEn.iPersonaId = Convert.ToInt64(dtPersona.Rows[0]["pers_iPersonaId"].ToString());
                    objEn.vNombres = dtPersona.Rows[0]["pers_vNombres"].ToString();
                    objEn.vApellidoPaterno = dtPersona.Rows[0]["pers_vApellidoPaterno"].ToString();
                    objEn.vApellidoMaterno = dtPersona.Rows[0]["pers_vApellidoMaterno"].ToString();
                    if (dtPersona.Rows[0]["pers_sGeneroId"].ToString() != string.Empty)
                        objEn.sGeneroId = Convert.ToInt32(dtPersona.Rows[0]["pers_sGeneroId"].ToString());
                    else
                        objEn.sGeneroId = 0;
                    objEn.vEstadoCivil = dtPersona.Rows[0]["pers_vEstadoCivil"].ToString();
                    if (dtPersona.Rows[0]["pers_sEstadoCivilId"].ToString() != string.Empty)
                        objEn.sEstadoCivilId = Convert.ToInt32(dtPersona.Rows[0]["pers_sEstadoCivilId"].ToString());
                    else
                        objEn.sEstadoCivilId = 0;
                    objEn.vColorTez = dtPersona.Rows[0]["pers_vColorTez"].ToString();
                    if (dtPersona.Rows[0]["pers_sColorTezId"].ToString() != string.Empty)
                        objEn.sColorTezId = Convert.ToInt32(dtPersona.Rows[0]["pers_sColorTezId"].ToString());
                    else
                        objEn.sColorTezId = 0;
                    objEn.vEstatura = dtPersona.Rows[0]["pers_vEstatura"].ToString();
                    objEn.vColorOjos = dtPersona.Rows[0]["pers_vColorOjos"].ToString();

                    if (dtPersona.Rows[0]["pers_sColorOjosId"].ToString() != string.Empty)
                        objEn.sColorOjosId = Convert.ToInt32(dtPersona.Rows[0]["pers_sColorOjosId"].ToString());
                    else
                        objEn.sColorOjosId = 0;
                    objEn.vGrupoSanguineo = dtPersona.Rows[0]["pers_vGrupoSanguineo"].ToString();

                    if (dtPersona.Rows[0]["pers_sGrupoSanguineoId"].ToString() != string.Empty)
                        objEn.sGrupoSanguineoId = Convert.ToInt32(dtPersona.Rows[0]["pers_sGrupoSanguineoId"].ToString());
                    else
                        objEn.sGrupoSanguineoId = 0;
                    objEn.vSenasculares = dtPersona.Rows[0]["pers_vSenasParticulares"].ToString();
                    objEn.vPersonaTipo = dtPersona.Rows[0]["pers_vPersonaTipo"].ToString();

                    if (dtPersona.Rows[0]["pers_sPersonaTipoId"].ToString() != string.Empty)
                        objEn.sPersonaTipoId = Convert.ToInt32(dtPersona.Rows[0]["pers_sPersonaTipoId"].ToString());
                    else
                        objEn.sPersonaTipoId = 0;

                    if (dtPersona.Rows[0]["peid_sDocumentoTipoId"].ToString() != string.Empty)
                        objEn.sDocumentoTipoId = Convert.ToInt32(dtPersona.Rows[0]["peid_sDocumentoTipoId"].ToString());
                    else
                        objEn.sDocumentoTipoId = 0;
                    objEn.vDocumentoTipo = dtPersona.Rows[0]["peid_vDocumentoTipo"].ToString();
                    objEn.vDocumentoNumero = dtPersona.Rows[0]["peid_vDocumentoNumero"].ToString();
                    objEn.vNacionalidad = dtPersona.Rows[0]["sNacionalidad"].ToString();

                    if (dtPersona.Rows[0]["pers_sNacionalidadId"].ToString() != string.Empty)
                        objEn.sNacionalidadId = Convert.ToInt32(dtPersona.Rows[0]["pers_sNacionalidadId"].ToString());
                    else
                        objEn.sNacionalidadId = 0;

                    objEn.vTipoParticipante = dtPersona.Rows[0]["acpa_vTipoParticipante"].ToString();

                    if (dtPersona.Rows[0]["acpa_sTipoParticipanteId"].ToString() != string.Empty)
                        objEn.sTipoParticipanteId = Convert.ToInt32(dtPersona.Rows[0]["acpa_sTipoParticipanteId"].ToString());
                    else
                        objEn.sTipoParticipanteId = 0;

                    if (dtPersona.Rows[0]["acpa_iEmpresaId"].ToString() != string.Empty)
                        objEn.iEmpresaId = Convert.ToInt32(dtPersona.Rows[0]["acpa_iEmpresaId"].ToString());
                    else
                        objEn.iEmpresaId = 0;

                    if (dtPersona.Rows[0]["acpa_sTipoDatoId"].ToString() != string.Empty)
                        objEn.sTipoDatoId = Convert.ToInt32(dtPersona.Rows[0]["acpa_sTipoDatoId"].ToString());
                    else
                        objEn.sTipoDatoId = 0;

                    objEn.vTipoVinculo = dtPersona.Rows[0]["acpa_vTipoVinculo"].ToString();

                    if (dtPersona.Rows[0]["acpa_sTipoVinculoId"].ToString() != string.Empty)
                        objEn.sTipoVinculoId = Convert.ToInt32(dtPersona.Rows[0]["acpa_sTipoVinculoId"].ToString());
                    else
                        objEn.sTipoVinculoId = 0;
                    

                    objEn.vCorreoElectronico = dtPersona.Rows[0]["pers_vCorreoElectronico"].ToString();
                    objEn.vDireccion = dtPersona.Rows[0]["resi_vResidenciaDireccion"].ToString();
                    objEn.vTelefono = dtPersona.Rows[0]["resi_vResidenciaTelefono"].ToString();

                    objEn.vDptoCont = dtPersona.Rows[0]["vDptoCont"].ToString();
                    objEn.iDptoContId = Convert.ToInt32(dtPersona.Rows[0]["iDptoContId"].ToString());
                    objEn.vProvPais = dtPersona.Rows[0]["vProvPais"].ToString();
                    objEn.iProvPaisId = Convert.ToInt32(dtPersona.Rows[0]["iProvPaisId"].ToString());
                    objEn.vDistCiu = dtPersona.Rows[0]["vDistCiu"].ToString();
                    objEn.iDistCiuId = Convert.ToInt32(dtPersona.Rows[0]["iDistCiuId"].ToString());

                    
                    objEn.cDptoContId = dtPersona.Rows[0]["iDptoContId"].ToString();
                    objEn.cProvPaisId = dtPersona.Rows[0]["iProvPaisId"].ToString();
                    objEn.cDistCiuId =  dtPersona.Rows[0]["iDistCiuId"].ToString();

                    objEn.acpa_iResidenciaId = Convert.ToInt32(dtPersona.Rows[0]["pere_iResidenciaId"].ToString());

                    if (dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString() == null || dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString().Trim()=="")
                    {
                        objEn.pers_dNacimientoFecha =  null; // MinValue?
                    }
                    else
                    {
                        objEn.pers_dNacimientoFecha = Comun.FormatearFecha(dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString());
                    }

                    if (dtPersona.Rows[0]["pers_cNacimientoLugar"].ToString() == null || dtPersona.Rows[0]["pers_cNacimientoLugar"].ToString().Trim() == "")
                    {
                        objEn.cNacimientoLugar = "0";
                    }
                    else
                    {
                        objEn.cNacimientoLugar = dtPersona.Rows[0]["pers_cNacimientoLugar"].ToString();
                    }


                    if (dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString() == null || dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString().Trim()=="")
                    {
                        objEn.dFecNacimiento = "0"; // MinValue?
                    }
                    else
                    {
                        objEn.dFecNacimiento = Convert.ToDateTime(dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechaLarga"].ToString());
                        objEn.dFecNacimientoCorta = Convert.ToDateTime(dtPersona.Rows[0]["pers_dNacimientoFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"].ToString());
                       
                       // pers_dNacimientoFechaCorta
                    }

                    //--------------------------------------------
                    //Autor: Miguel Angel Márquez Beltrán
                    //Fecha: 13/12/2016
                    //Objetivo: Asignar el Lugar de Nacimiento
                    //--------------------------------------------

                    if (dtPersona.Rows[0]["pers_vLugarNacimiento"].ToString() == null || dtPersona.Rows[0]["pers_vLugarNacimiento"].ToString().Trim() == "")
                    {
                        objEn.vLugarNacimiento = "";
                    }
                    else
                    {
                        objEn.vLugarNacimiento = dtPersona.Rows[0]["pers_vLugarNacimiento"].ToString();
                    }
                    //--------------------------------------------
                    objEn.vApellidoCasada = dtPersona.Rows[0]["pers_vApellidoCasada"].ToString();
                }

                return objEn;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
