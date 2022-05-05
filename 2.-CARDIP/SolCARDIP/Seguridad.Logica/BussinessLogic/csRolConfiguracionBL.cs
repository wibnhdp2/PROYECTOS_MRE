using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessADO;

namespace Seguridad.Logica.BussinessLogic
{
    public class csRolConfiguracionBL
    {
        public csTablaBE Consultar(string strRolConfiguracionID, string strAplicacionID, string strRolTipoID, string strNombre, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            csRolConfiguracionADO objADO = new csRolConfiguracionADO();
            return objADO.Consultar(strRolConfiguracionID, strAplicacionID, strRolTipoID, strNombre, strEstado, intPageSize, intPageNumber, strcontar);
        }
        public DataTable ConsultarPorUsuario(string strUsuarioID)
        {
            csRolConfiguracionADO objADO = new csRolConfiguracionADO();
            return objADO.ConsultarPorUsuario(strUsuarioID);
        }
        public string Adicionar(csRolConfiguracionBE BE, ArrayList listaRolOpcion)
        {
            csRolConfiguracionADO objADO = new csRolConfiguracionADO();
            return objADO.Adicionar(BE, listaRolOpcion);
        }

        public string Modificar(csRolConfiguracionBE BE, ArrayList listaRolOpcion)
        {
            csRolConfiguracionADO objADO = new csRolConfiguracionADO();
            return objADO.Modificar(BE, listaRolOpcion);
        }

        public string Anular(string strRolConfiguracionID, string strUsuarioModificacion, string strIPModificacion)
        {
            csRolConfiguracionADO objADO = new csRolConfiguracionADO();
            return objADO.Anular(strRolConfiguracionID, strUsuarioModificacion, strIPModificacion);
        }
    }
}
