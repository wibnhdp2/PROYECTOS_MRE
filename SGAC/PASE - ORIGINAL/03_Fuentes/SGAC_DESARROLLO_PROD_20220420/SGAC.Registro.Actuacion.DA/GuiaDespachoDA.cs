using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SGAC.BE.MRE;
//-------------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase de acceso a datos de la guía de despacho
//-------------------------------------------------------------

namespace SGAC.Registro.Actuacion.DA
{
    public class GuiaDespachoDA
    {
        public RE_GUIADESPACHO insertar(RE_GUIADESPACHO GuiaDespachoBE)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_GUIADESPACHO_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@gude_dFechaEnvio", GuiaDespachoBE.gude_dFechaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@gude_sTipoEnvioId", GuiaDespachoBE.gude_sTipoEnvioId));
                        cmd.Parameters.Add(new SqlParameter("@gude_vNombreEmpresaEnvio", GuiaDespachoBE.gude_vNombreEmpresaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@gude_vGuiaAerea", GuiaDespachoBE.gude_vGuiaAerea));
                        cmd.Parameters.Add(new SqlParameter("@gude_vNumeroHoja", GuiaDespachoBE.gude_vNumeroHoja));
                        cmd.Parameters.Add(new SqlParameter("@gude_vNumeroGuiaDespacho", GuiaDespachoBE.gude_vNumeroGuiaDespacho));
                        cmd.Parameters.Add(new SqlParameter("@gude_sEstadoGuia", GuiaDespachoBE.gude_sEstadoGuia));
                        cmd.Parameters.Add(new SqlParameter("@gude_sUsuarioCreacion", GuiaDespachoBE.gude_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@gude_vIPCreacion", GuiaDespachoBE.gude_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", GuiaDespachoBE.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@gude_iGuiaDespachoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        GuiaDespachoBE.gude_iGuiaDespachoId = Convert.ToInt64(lReturn.Value);
                        GuiaDespachoBE.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                GuiaDespachoBE.Error = true;
                GuiaDespachoBE.Message = exec.Message.ToString();
            }
            return GuiaDespachoBE;
        }

        public RE_GUIADESPACHO actualizar(RE_GUIADESPACHO GuiaDespachoBE)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_GUIADESPACHO_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@gude_iGuiaDespachoId", GuiaDespachoBE.gude_iGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@gude_dFechaEnvio", GuiaDespachoBE.gude_dFechaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@gude_sTipoEnvioId", GuiaDespachoBE.gude_sTipoEnvioId));
                        cmd.Parameters.Add(new SqlParameter("@gude_vNombreEmpresaEnvio", GuiaDespachoBE.gude_vNombreEmpresaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@gude_vGuiaAerea", GuiaDespachoBE.gude_vGuiaAerea));
                        cmd.Parameters.Add(new SqlParameter("@gude_vNumeroHoja", GuiaDespachoBE.gude_vNumeroHoja));
                        cmd.Parameters.Add(new SqlParameter("@gude_vNumeroGuiaDespacho", GuiaDespachoBE.gude_vNumeroGuiaDespacho));
                        cmd.Parameters.Add(new SqlParameter("@gude_sEstadoGuia", GuiaDespachoBE.gude_sEstadoGuia));
                        cmd.Parameters.Add(new SqlParameter("@GUDE_SUSUARIOMODIFICACION", GuiaDespachoBE.gude_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@GUDE_VIPMODIFICACION", GuiaDespachoBE.gude_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", GuiaDespachoBE.OficinaConsultar));
                        #endregion


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        GuiaDespachoBE.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                GuiaDespachoBE.Error = true;
                GuiaDespachoBE.Message = exec.Message.ToString();
            }
            return GuiaDespachoBE;
        }

        public RE_GUIADESPACHO actualizarEnviado(RE_GUIADESPACHO GuiaDespachoBE)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_GUIADESPACHO_ACTUALIZAR_ENVIADO_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@gude_iGuiaDespachoId", GuiaDespachoBE.gude_iGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@GUDE_SUSUARIOMODIFICACION", GuiaDespachoBE.gude_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@GUDE_VIPMODIFICACION", GuiaDespachoBE.gude_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", GuiaDespachoBE.OficinaConsultar));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        GuiaDespachoBE.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                GuiaDespachoBE.Error = true;
                GuiaDespachoBE.Message = exec.Message.ToString();
            }
            return GuiaDespachoBE;
        }

        public RE_GUIADESPACHO anular(RE_GUIADESPACHO GuiaDespachoBE)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_GUIADESPACHO_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@gude_iGuiaDespachoId", GuiaDespachoBE.gude_iGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@GUDE_SUSUARIOMODIFICACION", GuiaDespachoBE.gude_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@GUDE_VIPMODIFICACION", GuiaDespachoBE.gude_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", GuiaDespachoBE.OficinaConsultar));
                        #endregion


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        GuiaDespachoBE.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                GuiaDespachoBE.Error = true;
                GuiaDespachoBE.Message = exec.Message.ToString();
            }
            return GuiaDespachoBE;
        }

        public DataTable Consultar(int intOficinaConsularId, long intGuiaDespachoId, string strFechaInicio, string strFechaFin,
                                  int intTipoEnvioId, string strNombreEmpresaEnvio, string strGuiaAerea, 
                                  string strNumeroHoja, string strNumeroGuiaDespacho, int intEstadoGuia,
                                  int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_GUIA_DESPACHO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_IGUIADESPACHOID", intGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_INICIAL", strFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_FINAL", strFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@P_STIPOENVIOID", intTipoEnvioId));
                        cmd.Parameters.Add(new SqlParameter("@P_VNOMBREEMPRESAENVIO", strNombreEmpresaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@P_VGUIAAEREA", strGuiaAerea));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROHOJA", strNumeroHoja));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROGUIADESPACHO", strNumeroGuiaDespacho));
                        cmd.Parameters.Add(new SqlParameter("@P_SESTADOGUIA", intEstadoGuia));

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public DataTable ConsultarNumeroGuiaDespacho(int intOficinaConsularId, long intGuiaDespachoId, string strAnioMes, int intEstadoGuia)                                  
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_GUIA_DESPACHO_CONSULTAR_NUMEROGUIADESPACHO_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_IGUIADESPACHOID", intGuiaDespachoId));
                        cmd.Parameters.Add(new SqlParameter("@P_VANIOMES", strAnioMes));
                        cmd.Parameters.Add(new SqlParameter("@P_SESTADOGUIA", intEstadoGuia));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }


        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
