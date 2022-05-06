using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Configuration;
using System.Reflection;

using SGAC.Accesorios;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
    public class RE_ACTONOTARIALDOCUMENTO_DA
    {
        public BE.MRE.RE_ACTONOTARIALDOCUMENTO insertar(BE.MRE.RE_ACTONOTARIALDOCUMENTO documento)
        {
            try 
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion())) 
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDOCUMENTO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros                        
                        cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialId", documento.ando_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@ando_sTipoDocumentoId", documento.ando_sTipoDocumentoId));
                        cmd.Parameters.Add(new SqlParameter("@ando_sTipoInformacionId", documento.ando_sTipoInformacionId));

                        if (documento.ando_sSubTipoInformacionId !=0) cmd.Parameters.Add(new SqlParameter("@ando_sSubTipoInformacionId", documento.ando_sSubTipoInformacionId));
                        if (documento.ando_vDescripcion != null) cmd.Parameters.Add(new SqlParameter("@ando_vDescripcion", documento.ando_vDescripcion));
                        if (documento.ando_vDetalleDocumento != null) cmd.Parameters.Add(new SqlParameter("@ando_vDetalleDocumento", documento.ando_vDetalleDocumento));
                        if (documento.ando_vRutaArchivo != null) cmd.Parameters.Add(new SqlParameter("@ando_vRutaArchivo", documento.ando_vRutaArchivo));

                        cmd.Parameters.Add(new SqlParameter("@ando_sUsuarioCreacion", documento.ando_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ando_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@ando_sOficinaConsularId", documento.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ando_iActoNotarialDocumentoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        documento.ando_iActoNotarialDocumentoId = Convert.ToInt64(lReturn.Value);
                        documento.Error = false;
                    }
                }
            }
            catch (SqlException exec) {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
            }
            return documento;
        }

        public BE.MRE.RE_ACTONOTARIALDOCUMENTO actualizar(BE.MRE.RE_ACTONOTARIALDOCUMENTO documento)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDOCUMENTO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialDocumentoId", documento.ando_iActoNotarialDocumentoId));
                        cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialId", documento.ando_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@ando_sTipoDocumentoId", documento.ando_sTipoDocumentoId));
                        cmd.Parameters.Add(new SqlParameter("@ando_sTipoInformacionId", documento.ando_sTipoInformacionId));

                        if (documento.ando_sSubTipoInformacionId != 0) cmd.Parameters.Add(new SqlParameter("@ando_sSubTipoInformacionId", documento.ando_sSubTipoInformacionId));
                        if (documento.ando_vDescripcion != null) cmd.Parameters.Add(new SqlParameter("@ando_vDescripcion", documento.ando_vDescripcion));
                        if (documento.ando_vDetalleDocumento != null) cmd.Parameters.Add(new SqlParameter("@ando_vDetalleDocumento", documento.ando_vDetalleDocumento));
                        if (documento.ando_vRutaArchivo != null) cmd.Parameters.Add(new SqlParameter("@ando_vRutaArchivo", documento.ando_vRutaArchivo));

                        cmd.Parameters.Add(new SqlParameter("@ando_sUsuarioModificacion", documento.ando_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ando_vIPModificacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@ando_sOficinaConsularId", documento.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        documento.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
            }

            return documento;
        }
        
        public int EliminarDocumento(RE_ACTONOTARIALDOCUMENTO documento)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDOCUMENTO_ELIMINAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialDocumentoId", documento.ando_iActoNotarialDocumentoId));

                        cmd.Parameters.Add(new SqlParameter("@ando_sUsuarioModificacion", documento.ando_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ando_vIPModificacion", documento.ando_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", documento.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        #endregion

                        cmd.ExecuteNonQuery();
                    }
                }
            }
      
            catch (SqlException exec)
            {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
                return -1;

            }
            catch (Exception exec)
            {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
                return -1;

            }
            return 1;
        }

        public List<RE_ACTONOTARIALDOCUMENTO> listado(RE_ACTONOTARIALDOCUMENTO documento){
            List<RE_ACTONOTARIALDOCUMENTO> Container = new List<RE_ACTONOTARIALDOCUMENTO>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDOCUMENTO_CONSULTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (documento.ando_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialId", documento.ando_iActoNotarialId));
                        if (documento.ando_sTipoDocumentoId != 0) cmd.Parameters.Add(new SqlParameter("@ando_sTipoDocumentoId", documento.ando_sTipoDocumentoId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader()){
                            while (loReader.Read()){
                                BE.MRE.RE_ACTONOTARIALDOCUMENTO item = new BE.MRE.RE_ACTONOTARIALDOCUMENTO();
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull"){
                                        PropertyInfo pInfo = (PropertyInfo)item.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null){ pInfo.SetValue(item, loReader[col], null); }
                                    }
                                }
                                Container.Add(item);
                            }
                        }

                        documento.Error = false;

                    }
                }
            }
            catch (SqlException exec){
                documento.Error = true;
                documento.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
            }
            return Container;
        }

        public DataTable ObtenerDocumentos(RE_ACTONOTARIALDOCUMENTO documento)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDOCUMENTO_CONSULTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (documento.ando_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialId", documento.ando_iActoNotarialId));
                        #endregion

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];                        
                    }
                }
            }
            catch (SqlException exec)
            {
                documento.Error = true;
                documento.Message = exec.Message.ToString();
            }

            return dt;
        }

        public RE_ACTONOTARIALDOCUMENTO obtener(RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDOCUMENTO_OBTENER", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (ActoNotarialDocumento.ando_iActoNotarialDocumentoId != 0) cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialDocumentoId", ActoNotarialDocumento.ando_iActoNotarialDocumentoId));
                        if (ActoNotarialDocumento.ando_iActoNotarialId != 0) cmd.Parameters.Add(new SqlParameter("@ando_iActoNotarialId", ActoNotarialDocumento.ando_iActoNotarialId));
                        if (ActoNotarialDocumento.ando_sTipoDocumentoId != 0) cmd.Parameters.Add(new SqlParameter("@ando_sTipoDocumentoId", ActoNotarialDocumento.ando_sTipoDocumentoId));
                        if (ActoNotarialDocumento.ando_sTipoInformacionId != 0) cmd.Parameters.Add(new SqlParameter("@ando_sTipoInformacionId", ActoNotarialDocumento.ando_sTipoInformacionId));
                        if (ActoNotarialDocumento.ando_sSubTipoInformacionId != 0) cmd.Parameters.Add(new SqlParameter("@ando_sSubTipoInformacionId", ActoNotarialDocumento.ando_sSubTipoInformacionId));
                        if (ActoNotarialDocumento.ando_vDescripcion != null) cmd.Parameters.Add(new SqlParameter("@ando_vDescripcion", ActoNotarialDocumento.ando_vDescripcion));                        
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)ActoNotarialDocumento.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(ActoNotarialDocumento, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }

                        ActoNotarialDocumento.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoNotarialDocumento.Error = true;
                ActoNotarialDocumento.Message = exec.Message.ToString();
            }

            return ActoNotarialDocumento;
        }

        string conexion(){
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
