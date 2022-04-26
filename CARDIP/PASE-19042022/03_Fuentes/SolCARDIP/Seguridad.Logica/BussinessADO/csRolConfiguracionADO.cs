using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csRolConfiguracionADO
    {
        csConexionADO MiConexion = new csConexionADO();

        public csTablaBE Consultar(string strRolConfiguracionID, string strAplicacionID, string strRolTipoID, string strNombre, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            int intPageCount = 0;
            DataTable dt = new DataTable();
            String sCadena = MiConexion.GetCadenaConexion();
            //Se inicia la Conexion
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();

            //Se configura el comando: store procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_CONSULTAR_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_ROCO_SROLCONFIGURACIONID", strRolConfiguracionID));
            cmd.Parameters.Add(new SqlParameter("@P_ROCO_SAPLICACIONID", strAplicacionID));
            cmd.Parameters.Add(new SqlParameter("@P_ROCO_SROLTIPOID", strRolTipoID));
            cmd.Parameters.Add(new SqlParameter("@P_ROCO_VNOMBRE", strNombre));
            cmd.Parameters.Add(new SqlParameter("@P_ROCO_CESTADO", strEstado));
            cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
            cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageNumber));
            cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strcontar));

            cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int, 0).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                intPageCount = int.Parse(cmd.Parameters["@P_IPAGECOUNT"].Value.ToString());
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }

            csTablaBE dtRegistros = new csTablaBE();
            dtRegistros.dtRegistros = dt;
            dtRegistros.CantidadPaginas = intPageCount;
            return dtRegistros;
        }

        public DataTable ConsultarPorUsuario(string strUsuarioID)
        {
            DataTable dt = new DataTable();
            String sCadena = MiConexion.GetCadenaConexion();
            //Se inicia la Conexion
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();

            //Se configura el comando: store procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_CONSULTAR_POR_USUARIO_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOID", strUsuarioID));

            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }

            return dt;
        }

        public string Adicionar(csRolConfiguracionBE BE, ArrayList listaRolOpcion)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);

            SqlTransaction transaction;

            cnx.Open();

            transaction = cnx.BeginTransaction("TransaccionRolConfiguracion");

            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_ADICIONAR_MRE";
            cmd.CommandTimeout = 0;

            string strMensaje = "";
            string strError = "N";
            string strErrorRolOpcion = "";

            try
            {
                csRolOpcionADO objRolOpcionADO;
                string strRolOpcionID = "";
                foreach (csRolOpcionBE objRolOpcionBE in listaRolOpcion)
                {
                    objRolOpcionADO = new csRolOpcionADO();

                    strErrorRolOpcion = objRolOpcionADO.Adicionar(objRolOpcionBE, transaction, cnx);

                    if (strErrorRolOpcion.Substring(0, 1) == "S")
                    {
                        break;
                    }
                    else
                    {
                        strRolOpcionID = strRolOpcionID + strErrorRolOpcion.Substring(1) + "|";
                    }
                }
                if (strErrorRolOpcion.Substring(0, 1) == "N")
                {
                    strRolOpcionID = strRolOpcionID.Substring(0, strRolOpcionID.Length - 1);

                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SAPLICACIONID", BE.AplicacionId));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SROLTIPOID", BE.RolTipoId));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_VROLOPCION", strRolOpcionID));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_VNOMBRE", BE.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_CHORARIO", BE.Horario));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SUSUARIOCREACION", BE.UsuarioCreacion));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_VIPCREACION", BE.IPCreacion));

                    cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    strError = cmd.Parameters["@P_CERROR"].Value.ToString();
                }
                                                                  
                if (strError.Substring(0, 1) == "S" || strErrorRolOpcion.Substring(0, 1) == "S")
                {
                    strMensaje = "S...Error al adicionar!!!";
                    transaction.Rollback();
                }
                else
                {
                    strMensaje = "N..Datos registrados correctamente.";
                    transaction.Commit();
                }
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                return "S." + ex.Message;
            }
            finally
            { cnx.Close(); }
            return strMensaje;
        }

        public string Modificar(csRolConfiguracionBE BE, ArrayList listaRolOpcion)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);

            SqlTransaction transaction;
            
            //Se configura el comando: Store Procedure
            cnx.Open();

            transaction = cnx.BeginTransaction("TransaccionRolConfiguracion");

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_MODIFICAR_MRE";
            cmd.CommandTimeout = 0;

            string strMensaje = "";
            string strError = "N";
            string strErrorRolOpcion = "";

            try
            {

                csRolOpcionADO objRolOpcionADO;
                string strRolOpcionID = "";

                string strRolOpcionOld = BE.RolOpcionOld;
                string strAplicacionID = BE.AplicacionId;
                string strFormularioID = "";                

                csFormularioADO objFormularioADO = new csFormularioADO();
                DataTable dtFormulario = new DataTable();

                dtFormulario = objFormularioADO.ConsultarMenu(strAplicacionID, strRolOpcionOld, "", "-1");

                if (dtFormulario.Rows.Count > 0)
                {
                    bool bolExiste;

                    for (int i = 0; i < dtFormulario.Rows.Count; i++)
                    {
                        strFormularioID = dtFormulario.Rows[i]["FORM_SFORMULARIOID"].ToString();
                        strRolOpcionID = dtFormulario.Rows[i]["ROOP_SROLOPCIONID"].ToString();

                        bolExiste = false;
                        foreach (csRolOpcionBE objRolOpcionBE in listaRolOpcion)
                        {
                            if (objRolOpcionBE.RolOpcionId.Equals(strRolOpcionID) && objRolOpcionBE.FormularioId.Equals(strFormularioID))
                            {
                                bolExiste = true;
                                break;
                            }
                        }
                        if (bolExiste == false)
                        {
                            csRolOpcionADO objRolOpcion = new csRolOpcionADO();

                            strErrorRolOpcion = objRolOpcion.Anular(strRolOpcionID, BE.UsuarioModificacion, BE.IPModificacion, transaction, cnx);
                            if (strErrorRolOpcion.Substring(0, 1) == "S")
                            {
                                break;
                            }
                        }
                        else
                        {
                            strErrorRolOpcion = "N";
                        }
                    }                    
                }
                if (strErrorRolOpcion.Substring(0, 1) == "N")
                {
                    strRolOpcionID = "";
                    strErrorRolOpcion = "";
                    foreach (csRolOpcionBE objRolOpcionBE in listaRolOpcion)
                    {
                        objRolOpcionADO = new csRolOpcionADO();


                        if (objRolOpcionBE.RolOpcionId.Trim() == "")
                        {
                            strErrorRolOpcion = objRolOpcionADO.Adicionar(objRolOpcionBE, transaction, cnx);
                        }
                        else
                        {
                            strErrorRolOpcion = objRolOpcionADO.Modificar(objRolOpcionBE, transaction, cnx);
                        }

                        if (strErrorRolOpcion.Substring(0, 1) == "S")
                        {
                            break;
                        }
                        else
                        {
                            if (objRolOpcionBE.RolOpcionId.Trim() == "")
                            {
                                strRolOpcionID = strRolOpcionID + strErrorRolOpcion.Substring(1) + "|";
                            }
                            else
                            {
                                strRolOpcionID = strRolOpcionID + objRolOpcionBE.RolOpcionId.ToString() + "|";
                            }

                        }
                    }
                }
                if (strErrorRolOpcion.Substring(0, 1) == "N")
                {
                    strRolOpcionID = strRolOpcionID.Substring(0, strRolOpcionID.Length - 1);

                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SROLCONFIGURACIONID", BE.RolConfiguracion));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SAPLICACIONID", BE.AplicacionId));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SROLTIPOID", BE.RolTipoId));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_VROLOPCION", strRolOpcionID));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_VNOMBRE", BE.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_CHORARIO", BE.Horario));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_CESTADO", BE.Estado));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                    cmd.Parameters.Add(new SqlParameter("@P_ROCO_VIPMODIFICACION", BE.IPModificacion));
                    
                    cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    
                    strError = cmd.Parameters["@P_CERROR"].Value.ToString();
                }
                if (strError.Substring(0, 1) == "S" || strErrorRolOpcion.Substring(0, 1) == "S")
                {
                    strMensaje = "S...Error al modificar!!!";
                    transaction.Rollback();
                }
                else
                {
                    strMensaje = "N..Datos registrados correctamente.";
                    transaction.Commit();
                }
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                return "S." + ex.Message;
            }
            finally
            { cnx.Close(); }
            return strMensaje;
        }

        public string Anular(string strRolConfiguracionID, string strUsuarioModificacion, string strIPModificacion)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_ANULAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_ROCO_SROLCONFIGURACIONID", strRolConfiguracionID));
                cmd.Parameters.Add(new SqlParameter("@P_ROCO_SUSUARIOMODIFICACION", strUsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_ROCO_VIPMODIFICACION", strIPModificacion));
                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                sError = cmd.Parameters["@P_CERROR"].Value.ToString();
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            finally
            { cnx.Close(); }
            return sError;
        }

    }
}
