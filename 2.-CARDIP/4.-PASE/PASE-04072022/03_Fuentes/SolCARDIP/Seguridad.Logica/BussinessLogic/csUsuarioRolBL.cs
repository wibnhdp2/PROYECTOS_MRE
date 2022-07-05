using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessADO;

namespace Seguridad.Logica.BussinessLogic
{
    public class csUsuarioRolBL
    {
        public csTablaBE Consultar(string strUsuarioRolID, string strUsuarioID, string strGrupoID, string strRolConfiguracionID, string strOficinaConsularID, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            csUsuarioRolADO objADO = new csUsuarioRolADO();
            return objADO.Consultar(strUsuarioRolID, strUsuarioID, strGrupoID, strRolConfiguracionID, strOficinaConsularID, strEstado, intPageSize, intPageNumber, strcontar);
        }
        public string Adicionar(csUsuarioRolBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            csUsuarioRolADO objADO = new csUsuarioRolADO();
            return objADO.Adicionar(BE, transaction, cnx);
        }
        public string Anular(string strUsuarioRolID, string strUsuarioModificacion, string strIPModificacion, SqlTransaction transaction, SqlConnection cnx)
        {
            csUsuarioRolADO objADO = new csUsuarioRolADO();
            return objADO.Anular(strUsuarioRolID, strUsuarioModificacion, strIPModificacion, transaction, cnx);
        }

    }
}
