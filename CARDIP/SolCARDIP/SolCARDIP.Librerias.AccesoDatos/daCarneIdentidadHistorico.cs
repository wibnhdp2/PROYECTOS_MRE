using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daCarneIdentidadHistorico
    {
        public int adicionar(SqlConnection con, SqlTransaction trx, beCarneIdentidadHistorico parametrosCarneIdentidadHistorico)
        {
                int CarneIdentidadIdHis = -1;
                string Fecha = "01/01/0001";
                DateTime FechaNull = DateTime.Parse(Fecha);
                SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_HISTORICO_ADICIONAR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = trx;

                SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDIDAD_ID", SqlDbType.SmallInt);
                par1.Direction = ParameterDirection.Input;
                par1.Value = parametrosCarneIdentidadHistorico.CarneIdentidadid;

                SqlParameter par2 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", SqlDbType.SmallInt);
                par2.Direction = ParameterDirection.Input;
                par2.Value = parametrosCarneIdentidadHistorico.CalidadMigratoriaid;

                SqlParameter par3 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA_SEC", SqlDbType.SmallInt);
                par3.Direction = ParameterDirection.Input;
                par3.Value = parametrosCarneIdentidadHistorico.CalidadMigratoriaSecId;

                if (parametrosCarneIdentidadHistorico.FechaEmision == FechaNull) { SqlParameter par4 = cmd.Parameters.Add("@P_FECHA_EMISION", DBNull.Value); }
                else
                {
                    SqlParameter par4 = cmd.Parameters.Add("@P_FECHA_EMISION", SqlDbType.DateTime);
                    par4.Direction = ParameterDirection.Input;
                    par4.Value = parametrosCarneIdentidadHistorico.FechaEmision;
                }

                if (parametrosCarneIdentidadHistorico.FechaVencimiento == FechaNull) { SqlParameter par5 = cmd.Parameters.Add("@P_FECHA_VENCIMIENTO", DBNull.Value); }
                else
                {
                    SqlParameter par5 = cmd.Parameters.Add("@P_FECHA_VENCIMIENTO", SqlDbType.DateTime);
                    par5.Direction = ParameterDirection.Input;
                    par5.Value = parametrosCarneIdentidadHistorico.FechaVencimiento;
                }

                SqlParameter par6 = cmd.Parameters.Add("@P_RUTA_ARCHIVO_FOTO", SqlDbType.VarChar, 250);
                par6.Direction = ParameterDirection.Input;
                par6.Value = parametrosCarneIdentidadHistorico.RutaArchivoFoto;

                SqlParameter par7 = cmd.Parameters.Add("@P_OFICINA_CONSULAR_EX", SqlDbType.SmallInt);
                par7.Direction = ParameterDirection.Input;
                par7.Value = parametrosCarneIdentidadHistorico.OficinaConsularExid;

                SqlParameter par8 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
                par8.Direction = ParameterDirection.Input;
                par8.Value = parametrosCarneIdentidadHistorico.EstadoId;

                SqlParameter par9 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
                par9.Direction = ParameterDirection.Input;
                par9.Value = parametrosCarneIdentidadHistorico.Usuariocreacion;

                SqlParameter par10 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
                par10.Direction = ParameterDirection.Input;
                par10.Value = parametrosCarneIdentidadHistorico.Ipcreacion;

                SqlParameter par12 = cmd.Parameters.Add("@P_RUTA_ARCHIVO_FIRMA", SqlDbType.VarChar, 250);
                par12.Direction = ParameterDirection.Input;
                par12.Value = parametrosCarneIdentidadHistorico.RutaArchivoFirma;

                SqlParameter par11 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.Int);
                par11.Direction = ParameterDirection.ReturnValue;

                int n = cmd.ExecuteNonQuery();
                if (n > 0) CarneIdentidadIdHis = Convert.ToInt32(par11.Value);
                return (CarneIdentidadIdHis);            
        }
    }
}
