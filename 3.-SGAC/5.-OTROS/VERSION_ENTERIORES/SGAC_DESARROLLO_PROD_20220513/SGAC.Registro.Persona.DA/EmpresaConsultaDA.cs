using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;

namespace SGAC.Registro.Persona.DA
{
    public class EmpresaConsultaDA
    {
        private string strConnectionName = string.Empty;

        public EmpresaConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~EmpresaConsultaDA()
        {
            GC.Collect();
        }

        #region Método CONSULTAR POR ID

        public DataSet ConsultarId(Int64 empr_iEmpresaId)
        {
            try
            {
                DataSet dsResult;

                SqlParameter[] prmParametros = new SqlParameter[1];

                prmParametros[0] = new SqlParameter("@empr_iEmpresaId", SqlDbType.BigInt);
                prmParametros[0].Value = empr_iEmpresaId;

                dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_REGISTRO.USP_RE_EMPRESA_CONSULTAR_POR_ID", prmParametros);
                return dsResult;
            }
            catch (SGACExcepcion ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion Método CONSULTAR POR ID

        public RE_EmpresaFiltro Empresa_Existe(object[] arrParametro)
        {
            using (SqlConnection cn = new SqlConnection(strConnectionName))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                RE_EmpresaFiltro objEn = new RE_EmpresaFiltro();
                objEn = (RE_EmpresaFiltro)arrParametro[0];

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_EMPRESA_EXISTE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@empr_vNroDocumento", SqlDbType.VarChar, 20).Value = objEn.empr_vNumeroDocumento;
                    da.SelectCommand.Parameters.Add("@empr_vRazonSocial", SqlDbType.VarChar, 200).Value = objEn.empr_vRazonSocial;
                    da.SelectCommand.Parameters.Add("@empr_sTipoDocumentoId", SqlDbType.Int).Value = objEn.empr_sTipoDocumentoId;
                    da.SelectCommand.Parameters.Add("@empr_iEmpresaId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                    da.Fill(ds, "Empresa");
                    dt = ds.Tables["Empresa"];

                    objEn.empr_iEmpresaId = Convert.ToInt64(da.SelectCommand.Parameters["@empr_iEmpresaId"].Value.ToString());

                    return objEn;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}