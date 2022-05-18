using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SUNARP.BE;

namespace SUNARP.Registro.Inscripcion.DA
{
    public class SolicitudMantenimientoDA
    {
        public SU_SOLICITUD_INSCRIPCION insertar(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_REGISTRAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pacno_iActoNotarialId", solicitud.acno_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCODZONAREGISTRAL", solicitud.SOIN_VCODZONAREGISTRAL));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCODOFICINAREGISTRAL", solicitud.SOIN_VCODOFICINAREGISTRA));
                        cmd.Parameters.Add(new SqlParameter("@pSOFICINACONSULARID", solicitud.OFICINACONSULARID));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_IACTUACIONID", solicitud.SOIN_IACTUACIONID));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCODACTO", solicitud.SOIN_VCODACTO));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VDESACTO", solicitud.SOIN_VDESACTO));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VNOINSCRIBIR", solicitud.SOIN_VNOINSCRIBIR));
                        cmd.Parameters.Add(new SqlParameter("@pIFuncionarioId", solicitud.IFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCORREOPRESENTANTE", solicitud.SOIN_VCORREOPRESENTANTE));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCORREOSOLICITANTE", solicitud.SOIN_VCORREOSOLICITANTE));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_SUSUARIOCREACION", solicitud.SOIN_SUSUARIOCREACION));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VIPCREACION", solicitud.SOIN_VIPCREACION));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@pSOIN_VCUO", SqlDbType.VarChar,20);
                        lReturn.Direction = ParameterDirection.Output;
                        SqlParameter lReturID = cmd.Parameters.Add("@pSOIN_ISOLICITUDINSCRIPCIONID", SqlDbType.BigInt);
                        lReturID.Direction = ParameterDirection.Output;
                        

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        solicitud.SOIN_VCUO = lReturn.Value.ToString();
                        solicitud.SOIN_ISOLICITUDINSCRIPCIONID = Convert.ToInt64(lReturID.Value);


                    }
                }

            }
            catch (SqlException exec)
            {
                solicitud.Error = true;
                throw new Exception(exec.Message, exec.InnerException);
            }
            return solicitud;
        }

        public SU_SOLICITUD_INSCRIPCION reingreso(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_REGISTRAR_REINGRESO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCUO", solicitud.SOIN_VCUO));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCODZONAREGISTRAL", solicitud.SOIN_VCODZONAREGISTRAL));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VCODOFICINAREGISTRAL", solicitud.SOIN_VCODOFICINAREGISTRA));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VANIOTITULOSUNARP", solicitud.SOIN_VANIOTITULOSUNARP));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VNUMTITULOSUNARP", solicitud.SOIN_VNUMTITULOSUNARP));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VTIPREINGRESO", solicitud.SOIN_VTIPREINGRESO));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_IACTUACIONID", solicitud.SOIN_IACTUACIONID));

                        cmd.Parameters.Add(new SqlParameter("@pSOIN_SUSUARIOCREACION", solicitud.SOIN_SUSUARIOCREACION));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VIPCREACION", solicitud.SOIN_VIPCREACION));

                        #endregion

                        #region Output
                        SqlParameter lReturID = cmd.Parameters.Add("@pSOIN_ISOLICITUDINSCRIPCIONID", SqlDbType.BigInt);
                        lReturID.Direction = ParameterDirection.Output;


                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        solicitud.SOIN_ISOLICITUDINSCRIPCIONID = Convert.ToInt64(lReturID.Value);

                    }
                }

            }
            catch (SqlException exec)
            {
                solicitud.Error = true;
                throw new Exception(exec.Message, exec.InnerException);
            }
            return solicitud;
        }

        public SU_SOLICITUD_INSCRIPCION enviar(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_ENVIAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_ISOLICITUDINSCRIPCIONID", solicitud.SOIN_ISOLICITUDINSCRIPCIONID));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_SUSUARIOCREACION", solicitud.SOIN_SUSUARIOCREACION));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VIPCREACION", solicitud.SOIN_VIPCREACION));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (SqlException exec)
            {
                solicitud.Error = true;
                throw new Exception(exec.Message, exec.InnerException);
            }
            return solicitud;
        }

        public SU_SOLICITUD_INSCRIPCION ActualizarDocumentoFirmado(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_ACTUALIZAR_PARTEFIRMADO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_ISOLICITUDINSCRIPCIONID", solicitud.SOIN_ISOLICITUDINSCRIPCIONID));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VPARTEFIRMADO", solicitud.SOIN_VPARTEFIRMADO));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_SUSUARIOCREACION", solicitud.SOIN_SUSUARIOCREACION));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VIPCREACION", solicitud.SOIN_VIPCREACION));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException exec)
            {
                solicitud.Error = true;
                throw new Exception(exec.Message, exec.InnerException);
            }
            return solicitud;
        }
        public SU_SOLICITUD_INSCRIPCION eliminar(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_ELIMINAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_ISOLICITUDINSCRIPCIONID", solicitud.SOIN_ISOLICITUDINSCRIPCIONID));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_SUSUARIOCREACION", solicitud.SOIN_SUSUARIOCREACION));
                        cmd.Parameters.Add(new SqlParameter("@pSOIN_VIPCREACION", solicitud.SOIN_VIPCREACION));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (SqlException exec)
            {
                solicitud.Error = true;
                throw new Exception(exec.Message, exec.InnerException);
            }
            return solicitud;
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
