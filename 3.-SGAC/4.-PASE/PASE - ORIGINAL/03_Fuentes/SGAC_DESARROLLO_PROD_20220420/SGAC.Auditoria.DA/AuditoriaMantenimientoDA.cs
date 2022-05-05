using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.Auditoria.DA
{
    public class AuditoriaMantenimientoDA
    {
        ~AuditoriaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public Int64 Insertar_Error(SI_AUDITORIA pobjBe)
        {
            Int64 i_Resultado = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR_ERROR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@NombreRuta", pobjBe.audi_vNombreRuta));
                        cmd.Parameters.Add(new SqlParameter("@idTipoOperacion", pobjBe.audi_sOperacionTipoId));
                        cmd.Parameters.Add(new SqlParameter("@idTabla", pobjBe.audi_sTablaId));   
                        cmd.Parameters.Add(new SqlParameter("@sClavePrimaria", pobjBe.audi_sClavePrimaria));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", pobjBe.audi_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@audi_vComentario", pobjBe.audi_vComentario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vMensaje", pobjBe.audi_vMensaje));
                        cmd.Parameters.Add(new SqlParameter("@audi_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", pobjBe.audi_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", Util.ObtenerDireccionIP()));
                        
                        SqlParameter lReturn = cmd.Parameters.Add("@iResultado", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        i_Resultado = Convert.ToInt64(lReturn.Value);
                    }
                }
            }
            catch
            {
                i_Resultado = -1;
            }

            return i_Resultado;
        }
    }
}
