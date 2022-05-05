using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SGAC.Controlador;
using SGAC.Registro.Persona.BL;

namespace SGAC.WebApp.Accesorios
{
    public class funcionario
    {
        public static DataTable dtFuncionario(int sOficinaConsularId,int IFuncionarioId)
        {
            try
            {
                DataTable dt = new DataTable();

                object[] arrParametros = { sOficinaConsularId, IFuncionarioId };

                FuncionarioConsultaBL objBL = new FuncionarioConsultaBL();
                dt = objBL.Funcionario_MRE(sOficinaConsularId.ToString());

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}