using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessADO;

namespace Seguridad.Logica.BussinessLogic
{
    public class csRolOpcionBL
    {
        public csTablaBE Consultar(string strRolOpcionID, string strFormularioID, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            csRolOpcionADO objADO = new csRolOpcionADO();
            return objADO.Consultar(strRolOpcionID, strFormularioID, strEstado, intPageSize, intPageNumber, strcontar);
        }

        public string Adicionar(csRolOpcionBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            csRolOpcionADO objADO = new csRolOpcionADO();
            return objADO.Adicionar(BE, transaction, cnx);
        }
        
        public string Modificar(csRolOpcionBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            csRolOpcionADO objADO = new csRolOpcionADO();
            return objADO.Modificar(BE,transaction, cnx);
        }

        public string Anular(string strRolOpcionID, string strUsuarioModificacion, string strIPModificacion, SqlTransaction transaction, SqlConnection cnx)
        {
            csRolOpcionADO objADO = new csRolOpcionADO();
            return objADO.Anular(strRolOpcionID, strUsuarioModificacion, strIPModificacion, transaction, cnx);
        }
    }
}
