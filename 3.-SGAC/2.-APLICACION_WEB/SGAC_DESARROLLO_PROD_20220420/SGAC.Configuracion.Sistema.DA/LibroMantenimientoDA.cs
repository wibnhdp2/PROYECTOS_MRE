using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class LibroMantenimientoDA
    {
        ~LibroMantenimientoDA()
        {
            GC.Collect(); 
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SI_LIBRO Insertar(SI_LIBRO objLibro)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_LIBRO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@libr_sOficinaConsularId", objLibro.libr_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@libr_sPeriodo", objLibro.libr_sPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@libr_sTipoLibroId", objLibro.libr_sTipoLibroId));
                        cmd.Parameters.Add(new SqlParameter("@libr_iNumeroEscritura", objLibro.libr_INumeroEscritura));
                        cmd.Parameters.Add(new SqlParameter("@libr_iNumeroLibro", objLibro.libr_INumeroLibro));
                        cmd.Parameters.Add(new SqlParameter("@libr_iNumeroFolioInicial", objLibro.libr_INumeroFolioInicial));
                        cmd.Parameters.Add(new SqlParameter("@libr_iNumeroFolioActual", objLibro.libr_INumeroFolioActual));
                        cmd.Parameters.Add(new SqlParameter("@libr_iNumeroFolioTotal", objLibro.libr_INumeroFolioTotal));
                        cmd.Parameters.Add(new SqlParameter("@libr_sUsuarioCreacion", objLibro.libr_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@libr_vIPCreacion", objLibro.libr_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@libr_vHostName", objLibro.HostName));
                        //-------------------------------------------------------
                        //Fecha: 23/06/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: libr_vRangoFoliosNC
                        //-------------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@libr_vRangoFoliosNC", objLibro.libr_vRangoFoliosNC));
                        //-------------------------------------------------------
                        
                        SqlParameter lReturn = cmd.Parameters.Add("@libr_sLibroId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objLibro.libr_sLibroId = Convert.ToInt16(lReturn.Value);
                        objLibro.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objLibro.Error = true;
                objLibro.Message = ex.Message.ToString();
            }
            return objLibro;
        }

        public SI_LIBRO Actualizar(SI_LIBRO objLibro)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_LIBRO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@libr_sLibroId", objLibro.libr_sLibroId));
                        cmd.Parameters.Add(new SqlParameter("@libr_sOficinaConsularId", objLibro.libr_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@libr_sPeriodo", objLibro.libr_sPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@libr_sTipoLibroId", objLibro.libr_sTipoLibroId));
                        cmd.Parameters.Add(new SqlParameter("@libr_INumeroEscritura", objLibro.libr_INumeroEscritura));
                        cmd.Parameters.Add(new SqlParameter("@libr_INumeroLibro", objLibro.libr_INumeroLibro));
                        cmd.Parameters.Add(new SqlParameter("@libr_INumeroFolioInicial", objLibro.libr_INumeroFolioInicial));
                        cmd.Parameters.Add(new SqlParameter("@libr_INumeroFolioActual", objLibro.libr_INumeroFolioActual));
                        cmd.Parameters.Add(new SqlParameter("@libr_INumeroFolioTotal", objLibro.libr_INumeroFolioTotal));
                        cmd.Parameters.Add(new SqlParameter("@libr_sUsuarioModificacion", objLibro.libr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@libr_vIPModificacion", objLibro.libr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@libr_vHostName", objLibro.HostName));
                        //-------------------------------------------------------
                        //Fecha: 23/06/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: libr_vRangoFoliosNC
                        //-------------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@libr_vRangoFoliosNC", objLibro.libr_vRangoFoliosNC));
                        //-------------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objLibro.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objLibro.Error = true;
                objLibro.Message = ex.Message;
            }
            return objLibro;
        }

        public SI_LIBRO Eliminar(SI_LIBRO objLibro)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_LIBRO_ELIMINAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@libr_sLibroId", objLibro.libr_sLibroId));                        
                        cmd.Parameters.Add(new SqlParameter("@libr_sUsuarioModificacion", objLibro.libr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@libr_vIPModificacion", objLibro.libr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@libr_sOficinaConsularId", objLibro.libr_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@libr_vHostName", objLibro.HostName));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objLibro.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objLibro.Error = true;
                objLibro.Message = ex.Message;
            }
            return objLibro;
        }

    }
}
