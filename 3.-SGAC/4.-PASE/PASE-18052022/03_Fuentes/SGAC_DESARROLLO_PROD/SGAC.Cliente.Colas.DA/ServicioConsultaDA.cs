using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DL.DAC;

namespace SGAC.Cliente.Colas.DA
{
    public class ServicioConsultaDA
    {
        ~ServicioConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable GetAll(int iOficinaconsularId, string servsServicioId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_CONSULTAR_LISTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_sServicioId", servsServicioId));
                        cmd.Parameters.Add(new SqlParameter("@serv_sOficinaConsularId", iOficinaconsularId));
                        
                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable Consultar(int iOficinaconsularId,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_sOficinaConsularId", iOficinaconsularId));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ConsultarDetalle(int iOficinaconsularId,
                                          int intPaginaActual,
                                          int intPaginaCantidad,
                                          int intservIServicioId,
                                          ref int intTotalRegistros,
                                          ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_CONSULTAR_DETALLE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_sOficinaConsularId", iOficinaconsularId));
                        cmd.Parameters.Add(new SqlParameter("@IservIServicioId", intservIServicioId));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ConsultarElegir(int iOficinaconsularId,
                                         int intPaginaActual,
                                         int intPaginaCantidad,
                                         ref int intTotalRegistros,
                                         ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_SERVICIO_CONSULTAR_ELEGIR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_IOficinaConsularId", iOficinaconsularId));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable LlenarCabeceraTreeView (int iOficinaconsularId, int vedeIVentanillaId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_SERVICIO_LLENARSERVICIOS_PADRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_IOficinaConsularId", iOficinaconsularId));
                        cmd.Parameters.Add(new SqlParameter("@vede_IVentanillaId", vedeIVentanillaId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }


        public DataTable LlenarDetalleTreeView (int iOficinaconsularId, int servIServicioId, int vedeIVentanillaId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_SERVICIO_LLENARTREESERVICIOS_HIJOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_IOficinaConsularId", iOficinaconsularId));
                        cmd.Parameters.Add(new SqlParameter("@serv_IServicioId", servIServicioId));
                        cmd.Parameters.Add(new SqlParameter("@vede_IVentanillaId", vedeIVentanillaId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ConsultarElegirServicios(int iOficinaconsularId,
                                                  int intPaginaActual,
                                                  int intPaginaCantidad,
                                                  ref int intTotalRegistros,
                                                  ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_SERVICIO_CONSULTAR_ELEGIR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_IOficinaConsularId", iOficinaconsularId));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public int ObtenerCodigoServicio(int ventIVentanillaId)
        {
            DataTable objResultado = new DataTable();

            int Valor;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_SERVICIO_GENERARCODIGO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_IOficinaConsularId", ventIVentanillaId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                            Valor = Convert.ToInt32(objResultado.Rows[0][0].ToString());
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                Valor = -1;
                throw exec;
            }

            return Valor;
        }

        public int ConsultarCodigoServicio(int ventIVentanillaId)
        {
            DataTable objResultado = new DataTable();

            int Valor;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_SERVICIO_CONSULTAR_CODIGOVENTANILLA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_IServicioId", ventIVentanillaId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                            Valor = Convert.ToInt32(objResultado.Rows[0][0].ToString());
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                Valor = -1;
                throw exec;
            }

            return Valor;
        }

        public List<SGAC.BE.CL_SERVICIO> ListarServiciosConsulado(int iOficinaconsularId)
        {
            int A;
            DataSet dsResult;
            DataTable dtResult;

            string StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];

            SqlParameter[] prmParameter = new SqlParameter[2];

            prmParameter[0] = new SqlParameter("@serv_iConsuladoId", SqlDbType.Int);
            prmParameter[0].Value = iOficinaconsularId;

            dsResult = SqlHelper.ExecuteDataset(StrConnectionName, CommandType.StoredProcedure, "PN_CLIENTE.USP_MA_SERVICIO_CONSULTAR", prmParameter);
            dtResult = dsResult.Tables[0];

            List<SGAC.BE.CL_SERVICIO> LstServicio = new List<SGAC.BE.CL_SERVICIO>();

            for (A = 0; A <= dtResult.Rows.Count - 1; A++)
            {
                SGAC.BE.CL_SERVICIO Servicio = new SGAC.BE.CL_SERVICIO();

                Servicio.serv_cEstado = dtResult.Rows[A]["serv_cEstado"].ToString();
                Servicio.serv_sOficinaConsularId = Convert.ToInt16(dtResult.Rows[A]["serv_sOficinaConsularId"].ToString());
                Servicio.serv_vDescripcion = dtResult.Rows[A]["serv_vDescripcion"].ToString();
                Servicio.serv_sServicioId = Convert.ToInt16(dtResult.Rows[A]["serv_iServicioId"].ToString());

                LstServicio.Add(Servicio);
            }

            return LstServicio;
        }
    }
}
