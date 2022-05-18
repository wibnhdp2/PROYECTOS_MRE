using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class StockMinimoMantenimientoDA
    {
        ~StockMinimoMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SI_STOCK_ALMACEN Insertar(SI_STOCK_ALMACEN objStock)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_STOCK_ALMACEN_ADICIONAR", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@stck_sOficinaConsularId", objStock.stck_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@stck_sInsumoId", objStock.stck_sInsumoId));
                        cmd.Parameters.Add(new SqlParameter("@stck_INumeroStockMinimo", objStock.stck_INumeroStockMinimo));
                        cmd.Parameters.Add(new SqlParameter("@stck_sUsuarioCreacion", objStock.stck_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@stck_vIPCreacion", objStock.stck_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@stck_vHostName", objStock.HostName));

                        SqlParameter lReturn = cmd.Parameters.Add("@stck_sStockId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objStock.stck_sStockId = Convert.ToInt16(lReturn.Value);
                        objStock.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objStock.Error = true;
                objStock.Message = ex.Message.ToString();
            }
            return objStock;
        }

        public SI_STOCK_ALMACEN Actualizar(SI_STOCK_ALMACEN objStock)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_STOCK_ALMACEN_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@stck_sStockId", objStock.stck_sStockId));
                        cmd.Parameters.Add(new SqlParameter("@stck_sOficinaConsularId", objStock.stck_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@stck_sInsumoId", objStock.stck_sInsumoId));
                        cmd.Parameters.Add(new SqlParameter("@stck_INumeroStockMinimo", objStock.stck_INumeroStockMinimo));
                        cmd.Parameters.Add(new SqlParameter("@stck_sUsuarioModificacion", objStock.stck_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@stck_vIPModificacion", objStock.stck_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@stck_vHostName", objStock.HostName));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objStock.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objStock.Error = true;
                objStock.Message = ex.Message;
            }
            return objStock;
        }

        public SI_STOCK_ALMACEN Eliminar(SI_STOCK_ALMACEN objStock)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_STOCK_ALMACEN_ELIMINAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@stck_sStockId", objStock.stck_sStockId));
                        cmd.Parameters.Add(new SqlParameter("@stck_sUsuarioModificacion", objStock.stck_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@stck_vIPModificacion", objStock.stck_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@stck_sOficinaConsularId", objStock.stck_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@stck_vHostName", objStock.HostName));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objStock.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objStock.Error = true;
                objStock.Message = ex.Message;
            }
            return objStock;
        }
    }
}
