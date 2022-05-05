using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

//--------------------------------------------------------
//Fecha: 09/09/2019
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar la tabla: RE_PLANTILLA_TRADUCCION
//--------------------------------------------------------

namespace SGAC.Configuracion.Sistema.DA
{
    public class PlantillaTraduccionConsultasDA
    {
        ~PlantillaTraduccionConsultasDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        public DataTable Consultar(Int64 intPlantillaTraduccionId, Int16 intPlantillaId, Int16 intIdiomaId, Int64 intEtiquetaId, 
            string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PLANTILLA_TRADUCCION_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_IPLANTILLATRADUCCIONID", intPlantillaTraduccionId));
                        cmd.Parameters.Add(new SqlParameter("@P_SPLANTILLAID", intPlantillaId));
                        cmd.Parameters.Add(new SqlParameter("@P_SIDIOMAID", intIdiomaId));
                        cmd.Parameters.Add(new SqlParameter("@P_IETIQUETA", intEtiquetaId));
                        cmd.Parameters.Add(new SqlParameter("@P_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn2.Value);
                        IntTotalPages = Convert.ToInt32(lReturn1.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

    }
}
