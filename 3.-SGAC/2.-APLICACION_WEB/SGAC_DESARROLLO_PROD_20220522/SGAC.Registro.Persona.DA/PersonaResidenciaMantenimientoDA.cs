using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaResidenciaMantenimientoDA
    {
        ~PersonaResidenciaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public BE.MRE.RE_RESIDENCIA Insertar(BE.MRE.RE_RESIDENCIA residencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@resi_sResidenciaTipoId", residencia.resi_sResidenciaTipoId));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaDireccion", residencia.resi_vResidenciaDireccion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vCodigoPostal", residencia.resi_vCodigoPostal));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaTelefono", residencia.resi_vResidenciaTelefono));
                        cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo", residencia.resi_cResidenciaUbigeo));
                        if (residencia.resi_ICentroPobladoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@resi_ICentroPobladoId", residencia.resi_ICentroPobladoId));
                        cmd.Parameters.Add(new SqlParameter("@resi_sUsuarioCreacion", residencia.resi_sUsuarioCreacion));
                        //cmd.Parameters.Add(new SqlParameter("@resi_vIPCreacion", residencia.resi_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@resi_sOficinaConsularId", residencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@resi_vHostName", Util.ObtenerHostName()));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@resi_iResidenciaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        residencia.resi_iResidenciaId = Convert.ToInt64(lReturn.Value);
                        residencia.Error = false;    
                    }
                }
            }
            catch (SqlException exec)
            {
                residencia.Error = true;
                residencia.Message = exec.Message.ToString();
            }

            return residencia;
        }

        public BE.MRE.RE_PERSONARESIDENCIA InsertarDR(BE.MRE.RE_PERSONARESIDENCIA personaResidencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pere_iPersonaId", personaResidencia.pere_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@pere_iResidenciaId", personaResidencia.pere_iResidenciaId));
                        cmd.Parameters.Add(new SqlParameter("@pere_sUsuarioCreacion", personaResidencia.pere_sUsuarioCreacion));
                        //cmd.Parameters.Add(new SqlParameter("@pere_vIPCreacion", personaResidencia.pere_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pere_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@pere_sOficinaConsularId", personaResidencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pere_vHostName", Util.ObtenerHostName()));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        personaResidencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                personaResidencia.Error = true;
                personaResidencia.Message = exec.Message.ToString();
            }

            return personaResidencia;
        }

        // RUNE RÁPIDO - EMPRESA - NO HA SIDO PROBADO... LOS SPS PENDIENTE DE MODIFICACIÓN
        public BE.MRE.RE_EMPRESARESIDENCIA InsertarEmpresaResidencia(BE.MRE.RE_EMPRESARESIDENCIA empresaResidencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESARESIDENCIA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@emre_iEmpresaId", empresaResidencia.emre_iEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@emre_iResidenciaId", empresaResidencia.emre_iResidenciaId));
                        cmd.Parameters.Add(new SqlParameter("@emre_cEstado", empresaResidencia.emre_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@emre_sUsuarioCreacion", empresaResidencia.emre_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@emre_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@emre_sOficinaConsularId", empresaResidencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@emre_vHostName", Util.ObtenerHostName()));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        empresaResidencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                empresaResidencia.Error = true;
                empresaResidencia.Message = exec.Message.ToString();
            }

            return empresaResidencia;
        }

        public BE.MRE.RE_EMPRESARESIDENCIA ActualizarEmpresaResidencia(BE.MRE.RE_EMPRESARESIDENCIA empresaResidencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESARESIDENCIA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@emre_iEmpresaId", empresaResidencia.emre_iEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@emre_iResidenciaId", empresaResidencia.emre_iResidenciaId));
                        cmd.Parameters.Add(new SqlParameter("@emre_cEstado", empresaResidencia.emre_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@emre_sUsuarioModificacion", empresaResidencia.emre_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@emre_vIPModificacion", empresaResidencia.emre_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@emre_sOficinaConsularId", empresaResidencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@emre_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        empresaResidencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                empresaResidencia.Error = true;
                empresaResidencia.Message = exec.Message.ToString();
            }

            return empresaResidencia;
        }

        public BE.MRE.RE_EMPRESARESIDENCIA EliminarEmpresaResidencia(BE.MRE.RE_EMPRESARESIDENCIA empresaResidencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESARESIDENCIA_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@emre_iEmpresaId", empresaResidencia.emre_iEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@emre_iResidenciaId", empresaResidencia.emre_iResidenciaId));
                        cmd.Parameters.Add(new SqlParameter("@emre_vIPModificacion", empresaResidencia.emre_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@emre_sOficinaConsularId", empresaResidencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@emre_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        empresaResidencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                empresaResidencia.Error = true;
                empresaResidencia.Message = exec.Message.ToString();
            }

            return empresaResidencia;
        }
        //-------------------------------------------------------------------------------------------------------------------------        

        /**************************************************************************************************/
        /************MANEJO DE LAS TABLAS RESIDENCIA Y PERSONA RESIDENCIA X REGISTRO***********************/
        /**************************************************************************************************/

        private string StrConnectionName = string.Empty;

        public PersonaResidenciaMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(BE.RE_RESIDENCIA ObjResBE,
                            BE.RE_PERSONARESIDENCIA ObjPersResBE,
                            int IntOficinaConsularId)
        {
            int IntResultQueryRes, IntResultQueryPerRes;

            try
            {
                SqlParameter[] prmParameterRes = new SqlParameter[10];

                prmParameterRes[0] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                prmParameterRes[0].Direction = ParameterDirection.Output;

                prmParameterRes[1] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                prmParameterRes[1].Value = ObjResBE.resi_sResidenciaTipoId;

                prmParameterRes[2] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                prmParameterRes[2].Value = ObjResBE.resi_vResidenciaDireccion;

                prmParameterRes[3] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                prmParameterRes[3].Value = ObjResBE.resi_vCodigoPostal;

                prmParameterRes[4] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                prmParameterRes[4].Value = ObjResBE.resi_vResidenciaTelefono;

                prmParameterRes[5] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);

                if (ObjResBE.resi_cResidenciaUbigeo.Length == 0)
                {
                    prmParameterRes[5].Value = null;
                }
                else
                {
                    prmParameterRes[5].Value = ObjResBE.resi_cResidenciaUbigeo;
                }

                prmParameterRes[6] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterRes[6].Value = IntOficinaConsularId;

                prmParameterRes[7] = new SqlParameter("@resi_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterRes[7].Value = ObjResBE.resi_sUsuarioCreacion;

                prmParameterRes[8] = new SqlParameter("@resi_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterRes[8].Value = ObjResBE.resi_vIPCreacion;

                prmParameterRes[9] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                prmParameterRes[9].Value = Util.ObtenerHostName();

                IntResultQueryRes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                              CommandType.StoredProcedure,
                                                              "PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR",
                                                              prmParameterRes);
                //if (IntResultQueryRes > 0)
                //{
                long LonResidenciaId = (long)prmParameterRes[0].Value;

                if (LonResidenciaId > 0)
                {
                    SqlParameter[] prmParameterPersRes = new SqlParameter[6];

                    prmParameterPersRes[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                    prmParameterPersRes[0].Value = ObjPersResBE.pere_iPersonaId;

                    prmParameterPersRes[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                    prmParameterPersRes[1].Value = LonResidenciaId;

                    prmParameterPersRes[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterPersRes[2].Value = IntOficinaConsularId;

                    prmParameterPersRes[3] = new SqlParameter("@pere_sUsuarioCreacion", SqlDbType.SmallInt);
                    prmParameterPersRes[3].Value = ObjPersResBE.pere_sUsuarioCreacion;

                    prmParameterPersRes[4] = new SqlParameter("@pere_vIPCreacion", SqlDbType.VarChar, 50);
                    prmParameterPersRes[4].Value = ObjPersResBE.pere_vIPCreacion;

                    prmParameterPersRes[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                    prmParameterPersRes[5].Value = Util.ObtenerHostName();

                    IntResultQueryPerRes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR",
                                                                    prmParameterPersRes);
                }
                //}

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Actualizar(BE.RE_RESIDENCIA ObjResBE,
                              BE.RE_PERSONARESIDENCIA ObjPersResBE,
                              int IntOficinaConsularId)
        {
            int IntResultQueryRes, IntResultQueryPerRes;

            try
            {
                SqlParameter[] prmParameterRes = new SqlParameter[10];

                prmParameterRes[0] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                prmParameterRes[0].Value = ObjResBE.resi_iResidenciaId;

                prmParameterRes[1] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                prmParameterRes[1].Value = ObjResBE.resi_sResidenciaTipoId;

                prmParameterRes[2] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                prmParameterRes[2].Value = ObjResBE.resi_vResidenciaDireccion;

                prmParameterRes[3] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                prmParameterRes[3].Value = ObjResBE.resi_vCodigoPostal;

                prmParameterRes[4] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                prmParameterRes[4].Value = ObjResBE.resi_vResidenciaTelefono;

                prmParameterRes[5] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                
                if (ObjResBE.resi_cResidenciaUbigeo.Length == 0)
                {
                    prmParameterRes[5].Value = null;
                }
                else
                {
                    prmParameterRes[5].Value = ObjResBE.resi_cResidenciaUbigeo;
                }

                prmParameterRes[6] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterRes[6].Value = IntOficinaConsularId;

                prmParameterRes[7] = new SqlParameter("@resi_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterRes[7].Value = ObjResBE.resi_sUsuarioModificacion;

                prmParameterRes[8] = new SqlParameter("@resi_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterRes[8].Value = ObjResBE.resi_vIPModificacion;

                prmParameterRes[9] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                prmParameterRes[9].Value = Util.ObtenerHostName();

                IntResultQueryRes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                              CommandType.StoredProcedure,
                                                              "PN_REGISTRO.USP_RE_RESIDENCIA_ACTUALIZAR",
                                                              prmParameterRes);
                //if (IntResultQueryRes > 0)
                //{
                SqlParameter[] prmParameterPersRes = new SqlParameter[6];

                prmParameterPersRes[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                prmParameterPersRes[0].Value = ObjPersResBE.pere_iPersonaId;

                prmParameterPersRes[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                prmParameterPersRes[1].Value = ObjPersResBE.pere_iResidenciaId;

                prmParameterPersRes[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterPersRes[2].Value = IntOficinaConsularId;

                prmParameterPersRes[3] = new SqlParameter("@pere_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterPersRes[3].Value = ObjPersResBE.pere_sUsuarioModificacion;

                prmParameterPersRes[4] = new SqlParameter("@pere_vIPModificacion", SqlDbType.VarChar, 20);
                prmParameterPersRes[4].Value = ObjPersResBE.pere_vIPModificacion;

                prmParameterPersRes[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                prmParameterPersRes[5].Value = Util.ObtenerHostName();

                IntResultQueryPerRes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                 CommandType.StoredProcedure,
                                                                 "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ACTUALIZAR",
                                                                 prmParameterPersRes);
                //}

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Eliminar(BE.RE_RESIDENCIA ObjResBE,
                            BE.RE_PERSONARESIDENCIA ObjPersResBE,
                            int IntOficinaConsularId)
        {
            int IntResultQueryRes, IntResultQueryPerRes;

            try
            {
                SqlParameter[] prmParameterRes = new SqlParameter[5];

                prmParameterRes[0] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                prmParameterRes[0].Value = ObjResBE.resi_iResidenciaId;

                prmParameterRes[1] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterRes[1].Value = IntOficinaConsularId;

                prmParameterRes[2] = new SqlParameter("@resi_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterRes[2].Value = ObjResBE.resi_sUsuarioModificacion;

                prmParameterRes[3] = new SqlParameter("@resi_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterRes[3].Value = ObjResBE.resi_vIPModificacion;

                prmParameterRes[4] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                prmParameterRes[4].Value = Util.ObtenerHostName();

                IntResultQueryRes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                              CommandType.StoredProcedure,
                                                              "PN_REGISTRO.USP_RE_RESIDENCIA_ELIMINAR",
                                                              prmParameterRes);
                //if (IntResultQueryRes > 0)
                //{
                SqlParameter[] prmParameterPersRes = new SqlParameter[6];

                prmParameterPersRes[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                prmParameterPersRes[0].Value = ObjPersResBE.pere_iPersonaId;

                prmParameterPersRes[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                prmParameterPersRes[1].Value = ObjResBE.resi_iResidenciaId;

                prmParameterPersRes[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterPersRes[2].Value = IntOficinaConsularId;

                prmParameterPersRes[3] = new SqlParameter("@pere_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterPersRes[3].Value = ObjPersResBE.pere_sUsuarioModificacion;

                prmParameterPersRes[4] = new SqlParameter("@pere_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterPersRes[4].Value = ObjResBE.resi_vIPModificacion;

                prmParameterPersRes[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                prmParameterPersRes[5].Value = Util.ObtenerHostName();

                IntResultQueryPerRes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                 CommandType.StoredProcedure,
                                                                 "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ELIMINAR",
                                                                 prmParameterPersRes);
                //}

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
