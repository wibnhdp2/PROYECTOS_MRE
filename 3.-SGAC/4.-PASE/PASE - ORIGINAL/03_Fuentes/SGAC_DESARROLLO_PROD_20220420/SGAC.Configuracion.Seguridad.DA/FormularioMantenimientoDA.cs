using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class FormularioMantenimientoDA
    {
        ~FormularioMantenimientoDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insert(ref SE_FORMULARIO pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_FORMULARIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@form_sAplicacionId", pobjBE.form_sAplicacionId));

                        if (pobjBE.form_sComponenteId == null)
                            cmd.Parameters.Add(new SqlParameter("@form_sComponenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@form_sComponenteId", pobjBE.form_sComponenteId));  
                     
                        cmd.Parameters.Add(new SqlParameter("@form_vNombre", pobjBE.form_vNombre));
                       
                        if(pobjBE.form_sReferenciaId==null)
                            cmd.Parameters.Add(new SqlParameter("@form_sReferenciaId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@form_sReferenciaId", pobjBE.form_sReferenciaId));

                        cmd.Parameters.Add(new SqlParameter("@form_vRuta", pobjBE.form_vRuta));

                        if(pobjBE.form_vImagen==null)
                            cmd.Parameters.Add(new SqlParameter("@form_vImagen", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@form_vImagen", pobjBE.form_vImagen));

                        cmd.Parameters.Add(new SqlParameter("@form_sOrden", pobjBE.form_sOrden));
                        cmd.Parameters.Add(new SqlParameter("@form_bVisible", pobjBE.form_bVisible));
                        cmd.Parameters.Add(new SqlParameter("@form_cEstado", pobjBE.form_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@form_sUsuarioCreacion", pobjBE.form_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@form_vIPCreacion", pobjBE.form_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@OficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@form_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@form_sFormularioId", pobjBE.form_sFormularioId)).Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                pobjBE.Message = ex.StackTrace.ToString();
            }
            return intResultado;
        }


        public int Update(SE_FORMULARIO pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_FORMULARIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@form_sFormularioId", pobjBE.form_sFormularioId));
                        cmd.Parameters.Add(new SqlParameter("@form_sAplicacionId", pobjBE.form_sAplicacionId));
                        if (pobjBE.form_sComponenteId == null)
                            cmd.Parameters.Add(new SqlParameter("@form_sComponenteId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@form_sComponenteId", pobjBE.form_sComponenteId));

                        cmd.Parameters.Add(new SqlParameter("@form_vNombre", pobjBE.form_vNombre));

                        if (pobjBE.form_sReferenciaId == null)
                            cmd.Parameters.Add(new SqlParameter("@form_sReferenciaId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@form_sReferenciaId", pobjBE.form_sReferenciaId));

                        cmd.Parameters.Add(new SqlParameter("@form_vRuta", pobjBE.form_vRuta));

                        if (pobjBE.form_vImagen == null)
                            cmd.Parameters.Add(new SqlParameter("@form_vImagen", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@form_vImagen", pobjBE.form_vImagen));
                        cmd.Parameters.Add(new SqlParameter("@form_sOrden", pobjBE.form_sOrden));
                        cmd.Parameters.Add(new SqlParameter("@form_bVisible", pobjBE.form_bVisible));
                        cmd.Parameters.Add(new SqlParameter("@form_cEstado", pobjBE.form_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@form_sUsuarioModificacion", pobjBE.form_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@form_vIPModificacion", pobjBE.form_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@OficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@form_vHostName", Util.ObtenerHostName()));
                       

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                pobjBE.Message = ex.StackTrace.ToString();
            }
            return intResultado;
        }

        public int Delete(SE_FORMULARIO pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_FORMULARIO_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@form_sFormularioId", pobjBE.form_sFormularioId));
                        cmd.Parameters.Add(new SqlParameter("@form_sUsuarioModificacion", pobjBE.form_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@form_vIPModificacion", pobjBE.form_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@OficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@form_vHostName", Util.ObtenerHostName()));
                       
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                pobjBE.Message = ex.StackTrace.ToString();
            }
            return intResultado;
        }
    }
}