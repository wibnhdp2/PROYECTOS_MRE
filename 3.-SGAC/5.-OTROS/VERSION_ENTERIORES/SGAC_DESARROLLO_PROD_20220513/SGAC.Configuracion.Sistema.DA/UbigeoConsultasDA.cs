using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.DA
{
    public class UbigeoConsultasDA
    {
        ~UbigeoConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
            
        }

        public DataTable Consultar(string StrDepartamento,
                                   string StrProvincia,
                                   string StrDistrito,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   string StrEstado)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vDepartamento", StrDepartamento));
                        cmd.Parameters.Add(new SqlParameter("@vProvincia", StrProvincia));
                        cmd.Parameters.Add(new SqlParameter("@vDistrito", StrDistrito));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cEstado", StrEstado));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
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

        public int Existe(string StrCodigo, int IntOperacion)
        {
            int Rspta = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_EXISTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@cCodigo", StrCodigo));
                        cmd.Parameters.Add(new SqlParameter("@IOperacion", IntOperacion));

                        SqlParameter lReturn = cmd.Parameters.Add("@Rspta", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Rspta = Convert.ToInt32(lReturn.Value);                  
                    }
                }
            }
            catch (SqlException exec)
            {
                Rspta = -1;
                throw exec;
            }

            return Rspta;
        }

        public DataTable ObtenerUbigeo(string cUbigeo)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_OBTENER_UBIGEO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ubge_cCodigo", cUbigeo));

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


        public DataTable Consultar(string strCodigoUbigeo01 = null, string strCodigoUbigeo02 = null, string strCodigoUbigeo03 = null)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vDepartamento", string.Empty));
                        cmd.Parameters.Add(new SqlParameter("@vProvincia", string.Empty));
                        cmd.Parameters.Add(new SqlParameter("@vDistrito", string.Empty));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", 1));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", 100000));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi01", strCodigoUbigeo01));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi02", strCodigoUbigeo02));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi03", strCodigoUbigeo03));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

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

        public DataTable Consultar_Pais()
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PAIS_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

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

//--------------------------------------------------------------
//Fecha: 31/03/2017
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar La Tabla Ubigeo y asignar a un objeto List
//--------------------------------------------------------------
        public List<listaUbigeo> ObtenerUbigeo(string strCodigoUbigeo01, string strCodigoUbigeo02, string strCodigoUbigeo03,
           string strValor, string strDescripcion, bool bolAgregarItemAdicional, string pstrItemText, string pstrItemValue)
        {
            SqlDataReader dr;

            List<listaUbigeo> lista = new List<listaUbigeo>();


            bool isPrimero = true;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBICACIONGEOGRAFICA_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi01", strCodigoUbigeo01));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi02", strCodigoUbigeo02));
                        cmd.Parameters.Add(new SqlParameter("@ubge_cUbi03", strCodigoUbigeo03));

                        cmd.Connection.Open();

                        dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (isPrimero)
                            {
                                if (bolAgregarItemAdicional)
                                {
                                    lista.Add(new listaUbigeo() { PartName = pstrItemText, PartId = pstrItemValue });
                                }
                            }
                            lista.Add(new listaUbigeo() { PartName = dr[strDescripcion].ToString(), PartId = dr[strValor].ToString() });

                            isPrimero = false;

                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                dr = null;
                throw exec;
            }
            return lista;
        }





        public beUbigeoListas obtenerUbiGeo()
        {

            try
            {
                beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBIGEO_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Connection.Open();
                        SqlDataReader drd = cmd.ExecuteReader();
                        if (drd != null)
                        {
                            List<beUbicaciongeografica> Lista01 = new List<beUbicaciongeografica>();
                            int posUbi0101 = drd.GetOrdinal("COD01");
                            //int posSiglapais01 = drd.GetOrdinal("UBIGEO_PAIS");
                            int posDepartamento = drd.GetOrdinal("UBIGEO_DEPARTAMENTO");
                            beUbicaciongeografica obeUbicaciongeografica01;
                            while (drd.Read())
                            {
                                obeUbicaciongeografica01 = new beUbicaciongeografica();
                                obeUbicaciongeografica01.Ubi01 = drd.GetString(posUbi0101);
                                //obeUbicaciongeografica01.Siglapais = drd.GetString(posSiglapais01);
                                obeUbicaciongeografica01.Departamento = drd.GetString(posDepartamento);
                                Lista01.Add(obeUbicaciongeografica01);
                            }
                            obeUbigeoListas.Ubigeo01 = Lista01;
                            if (drd.NextResult())
                            {
                                List<beUbicaciongeografica> Lista02 = new List<beUbicaciongeografica>();
                                int posUbi0202 = drd.GetOrdinal("COD02");
                                int posUbi0102 = drd.GetOrdinal("COD01");
                                //int posSiglapais02 = drd.GetOrdinal("UBIGEO_PAIS");
                                int posProvincia = drd.GetOrdinal("UBIGEO_PROVINCIA");
                                beUbicaciongeografica obeUbicaciongeografica02;
                                while (drd.Read())
                                {
                                    obeUbicaciongeografica02 = new beUbicaciongeografica();
                                    obeUbicaciongeografica02.Ubi02 = drd.GetString(posUbi0202);
                                    obeUbicaciongeografica02.Ubi01 = drd.GetString(posUbi0102);
                                    //obeUbicaciongeografica02.Siglapais = drd.GetString(posSiglapais02);
                                    obeUbicaciongeografica02.Provincia = drd.GetString(posProvincia);
                                    Lista02.Add(obeUbicaciongeografica02);
                                }
                                obeUbigeoListas.Ubigeo02 = Lista02;
                                if (drd.NextResult())
                                {
                                    List<beUbicaciongeografica> Lista03 = new List<beUbicaciongeografica>();
                                    int posUbi0303 = drd.GetOrdinal("COD03");
                                    int posUbi0103 = drd.GetOrdinal("COD01");
                                    int posUbi0203 = drd.GetOrdinal("COD02");
                                    //int posSiglapais03 = drd.GetOrdinal("UBIGEO_PAIS");
                                    int posDistrito = drd.GetOrdinal("UBIGEO_DISTRITO");
                                    beUbicaciongeografica obeUbicaciongeografica03;
                                    while (drd.Read())
                                    {
                                        obeUbicaciongeografica03 = new beUbicaciongeografica();
                                        obeUbicaciongeografica03.Ubi03 = drd.GetString(posUbi0303);
                                        obeUbicaciongeografica03.Ubi01 = drd.GetString(posUbi0103);
                                        obeUbicaciongeografica03.Ubi02 = drd.GetString(posUbi0203);
                                        //obeUbicaciongeografica03.Siglapais = drd.GetString(posSiglapais03);
                                        obeUbicaciongeografica03.Distrito = drd.GetString(posDistrito);
                                        Lista03.Add(obeUbicaciongeografica03);
                                    }
                                    obeUbigeoListas.Ubigeo03 = Lista03;
                                }
                            }
                            drd.Close();
                        }
                    }
                }
                return obeUbigeoListas;
            }
            catch (SqlException exec)
            {
                throw exec;
            }           
        }

    }    
}