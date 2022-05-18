using System;
using SGAC.BE.MRE.Custom;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using SGAC.BE.MRE;
using System.Collections.Generic;


namespace SGAC.Registro.Actuacion.DA
{
    public class StockMinimoConsultaDA : SGAC.BE.RE_GENERAL
    {
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SI_STOCK_ALMACEN Consultar_Stock_Minimo(long stal_sInsumoId)
        {
            SI_STOCK_ALMACEN obj_Resultado = new SI_STOCK_ALMACEN();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ALMACEN_OBTENER_STOCK_MINIMO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@stal_sInsumoId", stal_sInsumoId));

                        cmd.Connection.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {  

                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = new SI_STOCK_ALMACEN();
                obj_Resultado.Message = exec.Message;
            }
            return obj_Resultado;
        }
    }
}
