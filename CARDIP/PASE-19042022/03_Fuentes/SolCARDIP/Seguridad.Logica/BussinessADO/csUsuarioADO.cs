using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csUsuarioADO
    {
        csConexionADO MiConexion = new csConexionADO();
        csUtilADO objUtilADO = new csUtilADO();

        public csTablaBE Consultar(string strUsuarioID, string strEmpresaID, string strAlias, string strApPaterno, string strApMaterno, string strNombres, string strDNI, string strOficinaID, string strSistemaID, string strEstado, int intPageSize, int intPageNumber, string strcontar)
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
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOID", strUsuarioID));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_SEMPRESAID", strEmpresaID));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_VALIAS", strAlias));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_VAPELLIDOPATERNO", strApPaterno));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_VAPELLIDOMATERNO", strApMaterno));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_VNOMBRES", strNombres));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_VDOCUMENTONUMERO", strDNI));
            cmd.Parameters.Add(new SqlParameter("@P_OFCO_SOFICINACONSULARID", strOficinaID));
            cmd.Parameters.Add(new SqlParameter("@P_SIST_SSISTEMAID", strSistemaID));
            cmd.Parameters.Add(new SqlParameter("@P_USUA_CESTADO", strEstado));
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

        public csTablaBE ConsultarLogin(string strCuenta, string strAplicacionID, int intPageSize, int intPageNumber, string strcontar)
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
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_LOGIN_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_USUA_VALIAS", strCuenta));
            cmd.Parameters.Add(new SqlParameter("@P_ROCO_SAPLICACIONID", strAplicacionID));
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

        public string Adicionar(csUsuarioBE BE, ArrayList listaRol)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);

            SqlTransaction transaction;

            cnx.Open();

            transaction = cnx.BeginTransaction("TransaccionUsuario");

            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIO_ADICIONAR_MRE";
            cmd.CommandTimeout = 0;

            string strMensaje = "";
            string strError = "N";
            string strErrorRol = "";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SEMPRESAID", BE.EmpresaId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VALIAS", BE.Alias));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VAPELLIDOPATERNO", BE.ApellidoPaterno));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VAPELLIDOMATERNO", BE.ApellidoMaterno));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VNOMBRES", BE.Nombres));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SPERSONATIPOID", BE.PersonaTipoId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SDOCUMENTOTIPOID", BE.DocumentoTipoId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VDOCUMENTONUMERO", BE.DocumentoNumero));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VCORREOELECTRONICO", BE.CorreoElectronico));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VTELEFONO", BE.Telefono));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VDIRECCION", BE.Direccion));
                
                DateTime dt = objUtilADO.obtenerDateTime(BE.FechaCaducidad);

                cmd.Parameters.Add(new SqlParameter("@P_USUA_DFECHACADUCIDAD", dt));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VDIRECCIONIP", BE.DireccionIP));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOCREACION", BE.UsuarioCreacion));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VIPCREACION", BE.IPCreacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@P_USUA_SUSUARIOID", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                strError = cmd.Parameters["@P_CERROR"].Value.ToString();
                string strusuarioID = cmd.Parameters["@P_USUA_SUSUARIOID"].Value.ToString();


                if (strError.Substring(0, 1) == "N")
                {
                    csUsuarioRolADO objUsuarioRolADO;

                    foreach (csUsuarioRolBE objUsuarioRolBE in listaRol)
                    {
                        objUsuarioRolBE.UsuarioId = strusuarioID;
                        objUsuarioRolADO = new csUsuarioRolADO();

                        strErrorRol = objUsuarioRolADO.Adicionar(objUsuarioRolBE, transaction, cnx);

                        if (strErrorRol.Substring(0, 1) == "S")
                        {
                            break;
                        }
                    }
                }
                if (strError.Substring(0, 1) == "S" || strErrorRol.Substring(0, 1) == "S")
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

        public string Modificar(csUsuarioBE BE, ArrayList listaRol)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);

            SqlTransaction transaction;

            cnx.Open();

            transaction = cnx.BeginTransaction("TransaccionUsuario");

            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIO_MODIFICAR_MRE";
            cmd.CommandTimeout = 0;

            string strMensaje = "";
            string strError = "N";
            string strErrorRol = "";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOID", BE.UsuarioId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SEMPRESAID", BE.EmpresaId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VALIAS", BE.Alias));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VAPELLIDOPATERNO", BE.ApellidoPaterno));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VAPELLIDOMATERNO", BE.ApellidoMaterno));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VNOMBRES", BE.Nombres));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SPERSONATIPOID", BE.PersonaTipoId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SDOCUMENTOTIPOID", BE.DocumentoTipoId));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VDOCUMENTONUMERO", BE.DocumentoNumero));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VCORREOELECTRONICO", BE.CorreoElectronico));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VTELEFONO", BE.Telefono));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VDIRECCION", BE.Direccion));

                DateTime dt = objUtilADO.obtenerDateTime(BE.FechaCaducidad);

                cmd.Parameters.Add(new SqlParameter("@P_USUA_DFECHACADUCIDAD", BE.FechaCaducidad));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_CESTADO", BE.Estado));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VIPMODIFICACION", BE.IPModificacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                strError = cmd.Parameters["@P_CERROR"].Value.ToString();

                if (strError.Substring(0, 1) == "N")
                {
                    csUsuarioRolADO objUsuarioRol = new csUsuarioRolADO();
                    csTablaBE objTablaBE = new csTablaBE();

                    objTablaBE = objUsuarioRol.Consultar("0", BE.UsuarioId, "0", "0", "0", "A", 10000, 1, "N");

                    bool bolExisteRol = false;

                    string strUsuarioRolID = "";

                    for (int i = 0; i < objTablaBE.dtRegistros.Rows.Count; i++)
                    {
                        strUsuarioRolID = objTablaBE.dtRegistros.Rows[i]["USRO_SUSUARIOROLID"].ToString().Trim();
                        csUsuarioRolBE objRolBE = new csUsuarioRolBE();
                        foreach (csUsuarioRolBE objUsuarioRolBE in listaRol)
                        {
                            objRolBE = objUsuarioRolBE;
                            objRolBE.UsuarioModificacion = objUsuarioRolBE.UsuarioCreacion;
                            objRolBE.Estado = "A";
                            objRolBE.IPModificacion = objUsuarioRolBE.IPCreacion;
                            if (objUsuarioRolBE.UsuarioRolId.ToString().Trim() == strUsuarioRolID)
                            {
                                bolExisteRol = true;
                                break;
                            }
                        }
                        
                        if (bolExisteRol == false)
                        {
                            strErrorRol = objUsuarioRol.Anular(strUsuarioRolID, BE.UsuarioModificacion, BE.IPModificacion, transaction, cnx);
                            if (strErrorRol.Substring(0, 1) == "S")
                            {
                                break;
                            }
                        }
                        else
                        {
                            strErrorRol = objUsuarioRol.Modificar(objRolBE, transaction, cnx);
                            if (strErrorRol.Substring(0, 1) == "S")
                            {
                                break;
                            }
                        }
                    }

                    if (strError.Substring(0, 1) == "N")
                    {
                        csUsuarioRolADO objUsuarioRolADO;

                        string strID = "";

                        foreach (csUsuarioRolBE objUsuarioRolBE in listaRol)
                        {
                            strID = objUsuarioRolBE.UsuarioRolId.ToString();

                            if (strID.Length == 0)
                            {
                                objUsuarioRolBE.UsuarioId = BE.UsuarioId;
                                objUsuarioRolADO = new csUsuarioRolADO();

                                strErrorRol = objUsuarioRolADO.Adicionar(objUsuarioRolBE, transaction, cnx);

                                if (strErrorRol.Substring(0, 1) == "S")
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (strError.Substring(0, 1) == "S" || strErrorRol.Substring(0, 1) == "S")
                {
                    strMensaje = "S...Error al actualizar!!!";
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

        public string Anular(string strUsuarioID, string strUsuarioModificacion, string strIPModificacion)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIO_ANULAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOID", strUsuarioID));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_SUSUARIOMODIFICACION", strUsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_USUA_VIPMODIFICACION", strIPModificacion));

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
