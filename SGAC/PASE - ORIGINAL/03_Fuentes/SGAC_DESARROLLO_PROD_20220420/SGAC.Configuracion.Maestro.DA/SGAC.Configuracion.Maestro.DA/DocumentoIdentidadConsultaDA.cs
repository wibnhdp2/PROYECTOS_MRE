using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.Accesorios;
//--------------------------------------------
//Fecha: 04/02/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar los registros de Documentos.
//--------------------------------------------
namespace SGAC.Configuracion.Maestro.DA
{
    public class DocumentoIdentidadConsultaDA
    {
        ~DocumentoIdentidadConsultaDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar_DocumentosIdentidad(short intDocumentoIdentidadId, string strDescripcionCorta, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_DOCUMENTO_IDENTIDAD_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_DOID_STIPODOCUMENTOIDENTIDADID", intDocumentoIdentidadId));
                        cmd.Parameters.Add(new SqlParameter("@P_DOID_VDESCRIPCIONCORTA", strDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@P_DOID_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
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
