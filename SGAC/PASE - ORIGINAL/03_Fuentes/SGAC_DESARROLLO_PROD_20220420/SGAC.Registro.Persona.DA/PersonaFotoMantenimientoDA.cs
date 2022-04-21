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
    public class PersonaFotoMantenimientoDA
    {
        ~PersonaFotoMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }


        private string StrConnectionName = string.Empty;

        public PersonaFotoMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(BE.RE_PERSONAFOTO ObjPersFotoBE,
                            int IntOficinaConsularId)
        {
            long LonResultQuery;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[8];

                prmParameter[0] = new SqlParameter("@pefo_iPersonaFotoId", SqlDbType.BigInt);
                prmParameter[0].Direction = ParameterDirection.Output;

                prmParameter[1] = new SqlParameter("@pefo_iPersonaId", SqlDbType.BigInt);
                prmParameter[1].Value = ObjPersFotoBE.pefo_iPersonaId;

                prmParameter[2] = new SqlParameter("@pefo_sFotoTipoId", SqlDbType.SmallInt);
                prmParameter[2].Value = ObjPersFotoBE.pefo_sFotoTipoId;

                prmParameter[3] = new SqlParameter("@pefo_GFoto", SqlDbType.Image);
                prmParameter[3].Value = ObjPersFotoBE.pefo_GFoto;

                prmParameter[4] = new SqlParameter("@pefo_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[4].Value = IntOficinaConsularId;

                prmParameter[5] = new SqlParameter("@pefo_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameter[5].Value = ObjPersFotoBE.pefo_sUsuarioCreacion;

                prmParameter[6] = new SqlParameter("@pefo_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameter[6].Value = ObjPersFotoBE.pefo_vIPCreacion;

                prmParameter[7] = new SqlParameter("@pefo_vHostName", SqlDbType.VarChar, 20);
                prmParameter[7].Value = Util.ObtenerHostName();

                LonResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAFOTO_ADICIONAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int Actualizar(BE.RE_PERSONAFOTO ObjPersFotoBE,
                              int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[8];

                prmParameter[0] = new SqlParameter("@pefo_iPersonaFotoId", SqlDbType.BigInt);
                prmParameter[0].Value = ObjPersFotoBE.pefo_iPersonaFotoId;

                prmParameter[1] = new SqlParameter("@pefo_iPersonaId", SqlDbType.BigInt);
                prmParameter[1].Value = ObjPersFotoBE.pefo_iPersonaId;

                prmParameter[2] = new SqlParameter("@pefo_sFotoTipoId", SqlDbType.SmallInt);
                prmParameter[2].Value = ObjPersFotoBE.pefo_sFotoTipoId;

                prmParameter[3] = new SqlParameter("@pefo_GFoto", SqlDbType.Image);
                prmParameter[3].Value = ObjPersFotoBE.pefo_GFoto;

                prmParameter[4] = new SqlParameter("@pefo_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[4].Value = IntOficinaConsularId;

                prmParameter[5] = new SqlParameter("@pefo_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameter[5].Value = ObjPersFotoBE.pefo_sUsuarioModificacion;

                prmParameter[6] = new SqlParameter("@pefo_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameter[6].Value = ObjPersFotoBE.pefo_vIPModificacion;

                prmParameter[7] = new SqlParameter("@pefo_vHostName", SqlDbType.VarChar, 20);
                prmParameter[7].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAFOTO_ACTUALIZAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int Eliminar(BE.RE_PERSONAFOTO ObjPersFotoBE,
                            int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[5];

                prmParameter[0] = new SqlParameter("@pefo_iPersonaFotoId", SqlDbType.BigInt);
                prmParameter[0].Value = ObjPersFotoBE.pefo_iPersonaFotoId;

                prmParameter[1] = new SqlParameter("@pefo_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[1].Value = IntOficinaConsularId;

                prmParameter[2] = new SqlParameter("@pefo_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameter[2].Value = ObjPersFotoBE.pefo_sUsuarioModificacion;

                prmParameter[3] = new SqlParameter("@pefo_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameter[3].Value = ObjPersFotoBE.pefo_vIPModificacion;

                prmParameter[4] = new SqlParameter("@pefo_vHostName", SqlDbType.VarChar, 20);
                prmParameter[4].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAFOTO_ELIMINAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}