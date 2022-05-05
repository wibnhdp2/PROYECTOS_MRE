using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Sistema.DA
{
    public class TipoActoProtocolarTarifarioConsultasDA
    {
        ~TipoActoProtocolarTarifarioConsultasDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar_TipoActoProtocolarTarifario(short sTipoActoProtocolarTarifarioId, short sTipoActoProtocolarId, short sTarifarioId, 
            int IntPageSize, string StrCurrentPage, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_ACTA_ITIPOACTOPROTOCOLARTARIFARIOID", sTipoActoProtocolarTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@P_ACTA_STIPOACTOPROTOCOLARID", sTipoActoProtocolarId));
                        cmd.Parameters.Add(new SqlParameter("@P_ACTA_STARIFARIOID", sTarifarioId));
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

        public bool ConsultarExisteTipoActoProtocolarTarifario(short intTipoActoProtocolarId, short intTarifarioId)
        {
            bool bExiste = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTO_NOTARIAL_TIPO_ACTO_PROTOCOLAR_TARIFARIO_CONSULTAR_EXISTE_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_STIPOACTOPROTOCOLARID", intTipoActoProtocolarId));
                        cmd.Parameters.Add(new SqlParameter("@P_STARIFARIOID", intTarifarioId));


                        cmd.Connection.Open();
                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                short intExiste = 0;

                                intExiste = Convert.ToInt16(loReader["EXISTE"].ToString());
                                if (intExiste > 0)
                                {
                                    bExiste = true;
                                    break;
                                }
                            }
                        }

                    }
                }
            }

            catch (SqlException exec)
            {
                throw exec;
            }

            return bExiste;
        }

        public DataTable Consultar_TipoActoProtocolar()
        {
            DataTable dtResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO_CONSULTAR_TIPO_ACTO_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
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

//------------------------------
    }
}
