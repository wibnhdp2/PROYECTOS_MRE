using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;

namespace SolCARDIP_REGLINEA.Librerias.AccesoDatos
{
    public class daUbicaciongeografica
    {
        public beUbigeoListas obtenerUbiGeo(SqlConnection con, beUbicaciongeografica parametros)
        {
            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
            SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_UBIGEO_MRE", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_PAIS_SIGLAS", SqlDbType.VarChar, 3);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Siglapais;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                List<beUbicaciongeografica> Lista01 = new List<beUbicaciongeografica>();
                int posUbi0101 = drd.GetOrdinal("COD01");
                int posSiglapais01 = drd.GetOrdinal("UBIGEO_PAIS");
                int posDepartamento = drd.GetOrdinal("UBIGEO_DEPARTAMENTO");
                beUbicaciongeografica obeUbicaciongeografica01;
                while (drd.Read())
                {
                    obeUbicaciongeografica01 = new beUbicaciongeografica();
                    obeUbicaciongeografica01.Ubi01 = drd.GetString(posUbi0101);
                    obeUbicaciongeografica01.Siglapais = drd.GetString(posSiglapais01);
                    obeUbicaciongeografica01.Departamento = drd.GetString(posDepartamento);
                    Lista01.Add(obeUbicaciongeografica01);
                }
                obeUbigeoListas.Ubigeo01 = Lista01;
                if (drd.NextResult())
                {
                    List<beUbicaciongeografica> Lista02 = new List<beUbicaciongeografica>();
                    int posUbi0202 = drd.GetOrdinal("COD02");
                    int posUbi0102 = drd.GetOrdinal("COD01");
                    int posSiglapais02 = drd.GetOrdinal("UBIGEO_PAIS");
                    int posProvincia = drd.GetOrdinal("UBIGEO_PROVINCIA");
                    beUbicaciongeografica obeUbicaciongeografica02;
                    while (drd.Read())
                    {
                        obeUbicaciongeografica02 = new beUbicaciongeografica();
                        obeUbicaciongeografica02.Ubi02 = drd.GetString(posUbi0202);
                        obeUbicaciongeografica02.Ubi01 = drd.GetString(posUbi0102);
                        obeUbicaciongeografica02.Siglapais = drd.GetString(posSiglapais02);
                        obeUbicaciongeografica02.Provincia = drd.GetString(posProvincia);
                        Lista02.Add(obeUbicaciongeografica02);
                    }
                    obeUbigeoListas.Ubigeo02 = Lista02;
                    if (drd.NextResult())
                    {
                        List<beUbicaciongeografica> Lista03 = new List<beUbicaciongeografica>();
                        int posUbi0303 = drd.GetOrdinal("COD03");
                        int posUbi0103 = drd.GetOrdinal("COD01");
                        int posUbi0203 = drd.GetOrdinal("COD02");
                        int posSiglapais03 = drd.GetOrdinal("UBIGEO_PAIS");
                        int posDistrito = drd.GetOrdinal("UBIGEO_DISTRITO");
                        beUbicaciongeografica obeUbicaciongeografica03;
                        while (drd.Read())
                        {
                            obeUbicaciongeografica03 = new beUbicaciongeografica();
                            obeUbicaciongeografica03.Ubi03 = drd.GetString(posUbi0303);
                            obeUbicaciongeografica03.Ubi01 = drd.GetString(posUbi0103);
                            obeUbicaciongeografica03.Ubi02 = drd.GetString(posUbi0203);
                            obeUbicaciongeografica03.Siglapais = drd.GetString(posSiglapais03);
                            obeUbicaciongeografica03.Distrito = drd.GetString(posDistrito);
                            Lista03.Add(obeUbicaciongeografica03);
                        }
                        obeUbigeoListas.Ubigeo03 = Lista03;
                    }
                }
                drd.Close();
            }
            return obeUbigeoListas;
        }
    }
}
