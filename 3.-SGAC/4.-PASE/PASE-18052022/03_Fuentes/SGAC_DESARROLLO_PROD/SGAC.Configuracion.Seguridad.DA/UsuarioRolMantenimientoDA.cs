using SGAC.Accesorios;
using System.Configuration;
using SGAC.BE.MRE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class UsuarioRolMantenimientoDA
    {
        ~UsuarioRolMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(SE_USUARIOROL pobjBE, ref int IntUsuarioRolId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioId", pobjBE.usro_sUsuarioId));

                        if (pobjBE.usro_sGrupoId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sGrupoId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sGrupoId", pobjBE.usro_sGrupoId));
                        }

                        if (pobjBE.usro_sRolConfiguracionId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sRolConfiguracionId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sRolConfiguracionId", pobjBE.usro_sRolConfiguracionId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usro_sOficinaConsularId", pobjBE.usro_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usro_sAcceso", pobjBE.usro_sAcceso));
                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioCreacion", pobjBE.usro_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@usro_vIPCreacion", pobjBE.usro_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@usro_vHostName", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@usro_sUsuarioRolId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        IntUsuarioRolId = Convert.ToInt32(lReturn.Value);

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

        public void Update(SE_USUARIOROL pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioRolId", pobjBE.usro_sUsuarioRolId));
                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioId", pobjBE.usro_sUsuarioId));

                        if (pobjBE.usro_sGrupoId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sGrupoId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sGrupoId", pobjBE.usro_sGrupoId));
                        }

                        if (pobjBE.usro_sRolConfiguracionId == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sRolConfiguracionId", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@usro_sRolConfiguracionId", pobjBE.usro_sRolConfiguracionId));
                        }

                        cmd.Parameters.Add(new SqlParameter("@usro_sOficinaConsularId", pobjBE.usro_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usro_sAcceso", pobjBE.usro_sAcceso));
                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioModificacion", pobjBE.usro_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@usro_vIPModificacion", pobjBE.usro_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@usro_vHostName", Util.ObtenerHostName()));                        

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

        public void Delete(SE_USUARIOROL pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioRolId", pobjBE.usro_sUsuarioRolId));                      
                        cmd.Parameters.Add(new SqlParameter("@usro_sOficinaConsularId", pobjBE.usro_sOficinaConsularId));                       
                        cmd.Parameters.Add(new SqlParameter("@usro_sUsuarioModificacion)", pobjBE.usro_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@usro_vIPModificacion", pobjBE.usro_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@usro_vHostName", Util.ObtenerHostName()));                        

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
