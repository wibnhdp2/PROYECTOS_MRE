using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using System.Transactions;

namespace SGAC.Accesorios
{
    public class Tabla
    {
        #region Funciones
        public DataTable ObtenerFuncion(string strAccion, string strEntidad)
        {
            string strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];

            DataSet dsResult = null;
            DataTable dtResult = null;

            SqlParameter[] prmParameter = new SqlParameter[2];
            prmParameter[0] = new SqlParameter("@func_vAccion", SqlDbType.VarChar);
            prmParameter[0].Value = strAccion;
            prmParameter[1] = new SqlParameter("@func_vEntidad", SqlDbType.VarChar);
            prmParameter[1].Value = strEntidad;


            //var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            //{
                try
                {
                    dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                        CommandType.StoredProcedure,
                                                        "PS_ACCESORIOS.USP_AC_FUNCION_CONSULTAR",
                                                        prmParameter);
                    dtResult = dsResult.Tables[0];

                 //   scope.Complete();
                }
                catch (Exception ex)
                {
                    //Transaction.Current.Rollback();
                    
                    throw ex;
                }
           // }
            return dtResult;
        }


        #endregion

        #region Notificacion

        #endregion

        #region CargaInicial

        //--------------------------------------------
        //Modificado por: Miguel Márquez Beltrán
        //Fecha: 11/11/2019
        //Motivo_ Método sin uso.
        //--------------------------------------------
        #region Comentada
        //public DataSet ObtenerCargaInicial(int intOficinaConsularId)
        //{
        //    string strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        //    DataSet ds = new DataSet();
        //    SqlParameter[] prmParameter;

        //    DataTable dtOficinaConsular = new DataTable();
        //    DataTable dtOficinasConsulares = new DataTable();
        //    DataTable dtParametros = new DataTable();
        //    DataTable dtTarifario = new DataTable();
        //    DataTable dtTarifarioRequisito = new DataTable();
        //    DataTable dtEstados = new DataTable();
        //    DataTable dtMaestra = new DataTable();
        //    DataTable dtSistema = new DataTable();
        //    DataTable dtBoveda = new DataTable();

        //    DataTable dtDocumentoIdentidad = new DataTable();
        //    DataTable dtTarifarioConsultas = new DataTable();

        //    DataTable dtOficinasConsularesActivas = new DataTable();

        //    #region OficinaConsular
        //    prmParameter = new SqlParameter[1];
        //    prmParameter[0] = new SqlParameter("@ofco_iOficinaConsularId", SqlDbType.SmallInt);
        //    prmParameter[0].Value = intOficinaConsularId;

        //    dtOficinaConsular = SqlHelper.ExecuteDataset(strConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PS_SISTEMA.USP_SI_OFICINACONSULAR_CONSULTAR_POR_ID",
        //                                            prmParameter).Tables[0];

        //    dtOficinaConsular.TableName = "dtOficinaConsular";
        //    ds.Tables.Add(dtOficinaConsular.Copy());
        //    #endregion

        //    #region Parametros
        //    DataSet dsGenerales = SqlHelper.ExecuteDataset(strConnectionName,
        //                                        CommandType.StoredProcedure,
        //                                        "PS_SISTEMA.USP_SI_PARAMETRO_CARGA_INICIAL", prmParameter);

        //    dtOficinasConsulares = dsGenerales.Tables[0];
        //    dtOficinasConsulares.TableName = "dtOficinasConsulares";
        //    ds.Tables.Add(dtOficinasConsulares.Copy());

        //    dtParametros = dsGenerales.Tables[1];
        //    dtParametros.TableName = "dtParametros";
        //    ds.Tables.Add(dtParametros.Copy());

        //    dtTarifario = dsGenerales.Tables[2];
        //    dtTarifario.TableName = "dtTarifario";
        //    ds.Tables.Add(dtTarifario.Copy());            

        //    //dtUbigeo = dsGenerales.Tables[3];
        //    //dtUbigeo.TableName = "dtUbigeo";
        //    //ds.Tables.Add(dtUbigeo.Copy());
            
        //    // Estados
        //    dtEstados = dsGenerales.Tables[3];
        //    dtEstados.TableName = "dtEstados";
        //    ds.Tables.Add(dtEstados.Copy());

        //    // Maestra
        //    dtMaestra = dsGenerales.Tables[4];
        //    dtMaestra.TableName = "dtMaestra";
        //    ds.Tables.Add(dtMaestra.Copy());
            
        //    // Sistema
        //    dtSistema = dsGenerales.Tables[5];
        //    dtSistema.TableName = "dtSistema";
        //    ds.Tables.Add(dtSistema.Copy());        
   


        //    // Centro Poblado
        //    //dtSistema = dsGenerales.Tables[6];
        //    //dtSistema.TableName = "dtCentroPoblado";
        //    //ds.Tables.Add(dtSistema.Copy());            


        //    //Documento Identidad
        //    dtDocumentoIdentidad = dsGenerales.Tables[6];
        //    dtDocumentoIdentidad.TableName = "dtDocumentoIdentidad";
        //    ds.Tables.Add(dtDocumentoIdentidad.Copy());

        //    //Tarifario consultas
        //    dtTarifarioConsultas = dsGenerales.Tables[7];
        //    dtTarifarioConsultas.TableName = "dtTarifarioConsultas";
        //    ds.Tables.Add(dtTarifarioConsultas.Copy());

        //    dtOficinasConsularesActivas = dsGenerales.Tables[8];
        //    dtOficinasConsularesActivas.TableName = "dtOficinasActivas";
        //    ds.Tables.Add(dtOficinasConsularesActivas.Copy());
        //    #endregion


        //    #region Boveda
        //    dtBoveda = SqlHelper.ExecuteDataset(strConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_ALMACEN.USP_AL_BOVEDA_CONSULTAR").Tables[0];
        //    dtBoveda.TableName = "dtBoveda";
        //    ds.Tables.Add(dtBoveda.Copy());           
        //    #endregion

        //    return ds;
        //}
        #endregion

        #endregion
    }
}
