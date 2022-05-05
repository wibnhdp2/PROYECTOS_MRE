using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SGAC.Configuracion.Sistema.DA
{
    public class LibroConsultasDA
    {
        private string StrConnectionName = string.Empty;

        public LibroConsultasDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable obtener(int intOficinaConsularId, int intPeriodo,
            int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas, 
            ref string strError, ref string strErrorCompleto)
        {
            //List<BE.MRE.SI_LIBRO> lstLibros = new List<BE.MRE.SI_LIBRO>();
            //BE.MRE.SI_LIBRO libro = new BE.MRE.SI_LIBRO();
            DataTable dtLibros = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_LIBRO_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                            cmd.Parameters.Add(new SqlParameter("@libr_sOficinaConsularId", intOficinaConsularId));
                        if (intPeriodo != 0)
                            cmd.Parameters.Add(new SqlParameter("@libr_sPeriodo", intPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));
                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        parTotalPagina.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtLibros = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(parTotalRegistros.Value);
                        intTotalPaginas = Convert.ToInt32(parTotalPagina.Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                strError = ex.Message;
                strErrorCompleto = ex.StackTrace;
            }
            return dtLibros;
        }


        public BE.MRE.SI_LIBRO consultar(BE.MRE.SI_LIBRO libro)
        {
            BE.MRE.SI_LIBRO objLibro = new BE.MRE.SI_LIBRO();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_LIBRO_CONSULTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@libr_sLibroId", libro.libr_sLibroId));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", 1));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", 10));
                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        parTotalPagina.Direction = ParameterDirection.Output;

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)objLibro.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(objLibro, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objLibro.Error = true;
                objLibro.Message = exec.Message.ToString();
            }
            return objLibro;
        }



        public Int32 ObtenerFojaActual(BE.MRE.SI_LIBRO libro)
        {
            BE.MRE.SI_LIBRO objLibro = new BE.MRE.SI_LIBRO();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OBTENER_FOJA_ACTUAL", cnx))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@libr_sTipoLibroId", libro.libr_sTipoLibroId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", libro.libr_sOficinaConsularId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        DataTable dtLibro = new DataTable();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtLibro = dsObjeto.Tables[0];
                        }


                        if (dtLibro.Rows.Count > 0)
                            return Convert.ToInt32(dtLibro.Rows[0][0]);

                      


                        
                       
                    }
                }
            }
            catch (SqlException exec)
            {
                objLibro.Error = true;
                objLibro.Message = exec.Message.ToString();
            }

            return 0;

        }
    }
}
