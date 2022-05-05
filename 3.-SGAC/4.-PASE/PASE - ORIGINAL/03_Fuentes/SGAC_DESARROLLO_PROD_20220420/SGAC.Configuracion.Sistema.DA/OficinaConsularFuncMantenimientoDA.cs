using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.DA
{
    public class OficinaConsularFuncMantenimientoDA
    {
        public string strError { get; set; }

        ~OficinaConsularFuncMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(RE_OFICINACONSULARFUNCIONARIO pobjBe, ref int intFuncionarioOficConsulId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_OFICINACONSULARFUNCIONARIO_REGISTRAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ocfu_sOficinaConsularId", pobjBe.ocfu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vDocumentoNumero", pobjBe.ocfu_vDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vNombreFuncionario", pobjBe.ocfu_vNombreFuncionario));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vApellidoPaternoFuncionario", pobjBe.ocfu_vApellidoPaternoFuncionario));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vApellidoMaternoFuncionario", pobjBe.ocfu_vApellidoMaternoFuncionario));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vCargo", pobjBe.ocfu_vCargo));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_IFuncionarioId", pobjBe.ocfu_IFuncionarioId));   
                        if (pobjBe.ocfu_sCargoFuncionarioId > 0)
                            cmd.Parameters.Add(new SqlParameter("@ocfu_sCargoFuncionarioId", pobjBe.ocfu_sCargoFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_sUsuarioCreacion", pobjBe.ocfu_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vIPCreacion", pobjBe.ocfu_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_sGeneroId", pobjBe.sGeneroId));                  
                        
                        SqlParameter lReturn = cmd.Parameters.Add("@ocfu_sOfiConFuncionarioId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intFuncionarioOficConsulId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch(SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }

        public void Update(RE_OFICINACONSULARFUNCIONARIO pobjBe, ref int intFuncionarioOficConsulId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_OFICINACONSULARFUNCIONARIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ocfu_sOficinaConsularId", pobjBe.ocfu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_IFuncionarioId", pobjBe.ocfu_IFuncionarioId));                 
                        cmd.Parameters.Add(new SqlParameter("@ocfu_sUsuarioModificacion", pobjBe.ocfu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vIPModificacion", pobjBe.ocfu_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vHostName", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@ocfu_sOfiConFuncionarioId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intFuncionarioOficConsulId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }

        public void Delete(RE_OFICINACONSULARFUNCIONARIO pobjBe, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_OFICINACONSULARFUNCIONARIO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ocfu_sOfiConFuncionarioId", pobjBe.ocfu_sOfiConFuncionarioId));                        
                        cmd.Parameters.Add(new SqlParameter("@ocfu_sUsuarioModificacion", pobjBe.ocfu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vIPModificacion", pobjBe.ocfu_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", pobjBe.OficinaConsultar));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }
        public void ActualizarCargo(Int16 ocfu_sOficinaConsularId, Int16 ocfu_sUsuarioModificacion, string ocfu_vIPModificacion, Int16 ocfu_sCargoFuncionarioId, Int16 ocfu_sGenero, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_OFICINACONSULARFUNCIONARIO_AGREGAR_CARGO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ocfu_sOfiConFuncionarioId", ocfu_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_sUsuarioModificacion", ocfu_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vIPModificacion", ocfu_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@catf_sCategoriaFuncionarioId", ocfu_sCargoFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@ocfu_sgenero", ocfu_sGenero));
                        

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Error = true;
                strError = ex.StackTrace.ToString();
            }
        }
    }
}
