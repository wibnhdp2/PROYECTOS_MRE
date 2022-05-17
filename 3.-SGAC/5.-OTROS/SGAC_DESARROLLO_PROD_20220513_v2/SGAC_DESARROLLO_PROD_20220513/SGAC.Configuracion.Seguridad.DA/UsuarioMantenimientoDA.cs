using SGAC.BE.MRE;
using System;
using SGAC.Accesorios;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class UsuarioMantenimientoDA
    {
        ~UsuarioMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(SE_USUARIO pobjBE, ref int IntUsuarioId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (pobjBE.usua_sEmpresaId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sEmpresaId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sEmpresaId", pobjBE.usua_sEmpresaId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usua_vAlias", pobjBE.usua_vAlias));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidoPaterno", pobjBE.usua_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidoMaterno", pobjBE.usua_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@usua_vNombres", pobjBE.usua_vNombres));

                        if (pobjBE.usua_sPersonaTipoId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sPersonaTipoId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sPersonaTipoId", pobjBE.usua_sPersonaTipoId));
                        }

                        if (pobjBE.usua_sDocumentoTipoId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sDocumentoTipoId", DBNull.Value));                            
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sDocumentoTipoId", pobjBE.usua_sDocumentoTipoId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usua_vDocumentoNumero", pobjBE.usua_vDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@usua_vCorreoElectronico", pobjBE.usua_vCorreoElectronico));

                        if (pobjBE.usua_vTelefono == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vTelefono", DBNull.Value));                            
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vTelefono", pobjBE.usua_vTelefono));
                        }

                        if (pobjBE.usua_vDireccion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccion", pobjBE.usua_vDireccion));
                        }

                        if (pobjBE.usua_sReferenciaId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sReferenciaId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sReferenciaId", pobjBE.usua_sReferenciaId));
                        }

                        if (pobjBE.usua_dFechaCaducidad == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_dFechaCaducidad", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_dFechaCaducidad", pobjBE.usua_dFechaCaducidad));
                        }

                        if (pobjBE.usua_vDireccionIP == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccionIP", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccionIP", pobjBE.usua_vDireccionIP));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usua_bNotificaRemesa", pobjBE.usua_bNotificaRemesa));
                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioCreacion", pobjBE.usua_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@usua_vIPCreacion", pobjBE.usua_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@usua_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vHostName", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@usua_sUsuarioId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        IntUsuarioId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        public void Update(SE_USUARIO pobjBE, ref int IntUsuarioId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioId", pobjBE.usua_sUsuarioId));

                        if (pobjBE.usua_sEmpresaId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sEmpresaId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sEmpresaId", pobjBE.usua_sEmpresaId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usua_vAlias", pobjBE.usua_vAlias));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidoPaterno", pobjBE.usua_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidoMaterno", pobjBE.usua_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@usua_vNombres", pobjBE.usua_vNombres));

                        if (pobjBE.usua_sPersonaTipoId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sPersonaTipoId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sPersonaTipoId", pobjBE.usua_sPersonaTipoId));
                        }

                        if (pobjBE.usua_sDocumentoTipoId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sDocumentoTipoId", DBNull.Value));                            
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sDocumentoTipoId", pobjBE.usua_sDocumentoTipoId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usua_vDocumentoNumero", pobjBE.usua_vDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@usua_vCorreoElectronico", pobjBE.usua_vCorreoElectronico));

                        if (pobjBE.usua_vTelefono == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vTelefono", DBNull.Value));                            
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vTelefono", pobjBE.usua_vTelefono));
                        }

                        if (pobjBE.usua_vDireccion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccion", pobjBE.usua_vDireccion));
                        }

                        if (pobjBE.usua_sReferenciaId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sReferenciaId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_sReferenciaId", pobjBE.usua_sReferenciaId));
                        }

                        if (pobjBE.usua_dFechaCaducidad == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_dFechaCaducidad", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_dFechaCaducidad", pobjBE.usua_dFechaCaducidad));
                        }

                        if (pobjBE.usua_vDireccionIP == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccionIP", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usua_vDireccionIP", pobjBE.usua_vDireccionIP));
                        }
                        cmd.Parameters.Add(new SqlParameter("@usua_bBloqueoActiva", pobjBE.usua_bBloqueoActiva));
                        cmd.Parameters.Add(new SqlParameter("@usua_cEstado", pobjBE.usua_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@usua_bNotificaRemesa", pobjBE.usua_bNotificaRemesa));
                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioModificacion", pobjBE.usua_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@usua_vIPModificacion", pobjBE.usua_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@usua_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        public void Actualizar_Sesion_Activa(int IntUsuarioId, bool bActivo, string IPAddress)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ACTUALIZAR_SESION_ACTIVA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usua_bSesionActiva", bActivo));
                        cmd.Parameters.Add(new SqlParameter("@usua_vDireccionIP", IPAddress));
                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioId", IntUsuarioId));

                        cmd.Parameters.Add(new SqlParameter("@usua_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Actualizar_Sesion_Bloqueada(int IntUsuarioId, bool bActivo, string vIP)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ACTUALIZAR_SESION_BLOQUEADA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usua_bBloqueoActiva", bActivo));
                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioId", IntUsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vDireccionIP", vIP));
                        cmd.Parameters.Add(new SqlParameter("@usua_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(SE_USUARIO pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioId", pobjBE.usua_sUsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioModificacion", pobjBE.usua_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@usua_vIPModificacion", pobjBE.usua_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@usua_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }        
    }
}
