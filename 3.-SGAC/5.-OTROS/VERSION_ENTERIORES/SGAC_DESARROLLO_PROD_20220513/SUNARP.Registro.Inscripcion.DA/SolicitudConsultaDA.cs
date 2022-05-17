using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SUNARP.Registro.Inscripcion.DA
{
    public class SolicitudConsultaDA
    {
        private string strConnectionName = string.Empty;

        public SolicitudConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~SolicitudConsultaDA()
        {
            GC.Collect();
        }
        public DataTable SolicitudInscripcionConsulta(Int64 ISOLICITUDINSCRIPCIONID = 0, string CUO = "")
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_ISOLICITUDINSCRIPCIONID", ISOLICITUDINSCRIPCIONID));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCUO", CUO));

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
            catch (Exception ex)
            {
                objResultado = null;
                throw ex;
            }
            return objResultado;
        }

        public DataTable SolicitudInscripcionConsultaMultiple(Int16 iOficinaConsularId = 0, string strNumeroEscritura = "",
            DateTime? fechaInicioExtencion = null, DateTime? FechaFinExtencion = null, Int16 sTipoParticipante = 0,
            Int16 sTipDocumento = 0, string strNumeroDocumento = "",
            string strApPaterno = "", string strApeMaterno = "", string strNombres = "",bool reingreso = false)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_SOFICINACONSULAR", SqlDbType.SmallInt).Value = iOficinaConsularId;
                        cmd.Parameters.Add("@P_VNUMEROESCRITURA", SqlDbType.VarChar, 20).Value = strNumeroEscritura;
                        cmd.Parameters.Add("@P_DFECHAEXTENCIONINICIO", SqlDbType.DateTime).Value = fechaInicioExtencion;
                        cmd.Parameters.Add("@P_DFECHAEXTENCIONFIN", SqlDbType.DateTime).Value = FechaFinExtencion;
                        cmd.Parameters.Add("@P_STIPPARTICIPANTE", SqlDbType.SmallInt).Value = sTipoParticipante;
                        cmd.Parameters.Add("@P_STIPDOCUMENTO", SqlDbType.SmallInt).Value = sTipDocumento;
                        cmd.Parameters.Add("@P_VNUMERODOCUMENTO", SqlDbType.VarChar, 20).Value = strNumeroDocumento;
                        cmd.Parameters.Add("@P_VAPEPATERNO", SqlDbType.VarChar, 100).Value = strApPaterno;
                        cmd.Parameters.Add("@P_VAPEMATERNO", SqlDbType.VarChar, 100).Value = strApeMaterno;
                        cmd.Parameters.Add("@P_VNOMBRES", SqlDbType.VarChar, 100).Value = strNombres;
                        cmd.Parameters.Add("@P_REINGRESO", SqlDbType.Bit).Value = reingreso;

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
            catch (Exception ex)
            {
                objResultado = null;
                throw ex;
            }
            return objResultado;
        }

        public DataTable consultaSolicitud(short iOficinaConsularId = 0, string strNumeroEscritura = "", DateTime? fechaInicioExtencion = null, DateTime? fechaFinExtencion = null, 
            string sAnioTitulo = "0", string strNumeroTitulo="", string strNumeroCUO="", short iEstado = 0, DateTime? fechaInicioSolicitud = null, DateTime? fechaFinSolicitud = null)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_CONSULTAR_PARA_HISTORICO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_SOFICINACONSULAR", SqlDbType.SmallInt).Value = iOficinaConsularId;
                        cmd.Parameters.Add("@P_VNUMEROESCRITURA", SqlDbType.VarChar, 20).Value = strNumeroEscritura;
                        cmd.Parameters.Add("@P_DFECHAEXTENCIONINICIO", SqlDbType.DateTime).Value = fechaInicioExtencion;
                        cmd.Parameters.Add("@P_DFECHAEXTENCIONFIN", SqlDbType.DateTime).Value = fechaFinExtencion;
                        cmd.Parameters.Add("@P_SANIOTITULO", SqlDbType.VarChar,4).Value = sAnioTitulo;
                        cmd.Parameters.Add("@P_VNUMEROTITULO", SqlDbType.VarChar,20).Value = strNumeroTitulo;
                        cmd.Parameters.Add("@P_SESTADO", SqlDbType.SmallInt).Value = iEstado;
                        cmd.Parameters.Add("@P_VNUMEROCUO", SqlDbType.VarChar, 10).Value = strNumeroCUO;
                        cmd.Parameters.Add("@P_DFECHASOLICITUDINICIO", SqlDbType.DateTime).Value = fechaInicioSolicitud;
                        cmd.Parameters.Add("@P_DFECHASOLICITUDFIN", SqlDbType.DateTime).Value = fechaFinSolicitud;

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
            catch (Exception ex)
            {
                objResultado = null;
                throw ex;
            }
            return objResultado;
        }

        public DataTable SolicitudInscripcionConsultaHistorico(long iSOLICITUDINSCRIPCIONID)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[SC_SUNARP].[USP_SU_SOLICITUD_INSCRIPCION_OBTENER_HISTORICO]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_ISOLICITUDINSCRIPCIONID", iSOLICITUDINSCRIPCIONID));

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
            catch (Exception ex)
            {
                objResultado = null;
                throw ex;
            }
            return objResultado;
        }
    }
}
