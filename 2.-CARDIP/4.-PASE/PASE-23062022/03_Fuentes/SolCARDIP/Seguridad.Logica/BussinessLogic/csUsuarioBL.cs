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
    public class csUsuarioBL
    {
        public csTablaBE Consultar(string strUsuarioID, string strEmpresaID, string strAlias, string strApPaterno, string strApMaterno, string strNombres, string strDNI,  string strOficinaID, string strSistemaID, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            csUsuarioADO objADO = new csUsuarioADO();
            return objADO.Consultar(strUsuarioID, strEmpresaID, strAlias, strApPaterno, strApMaterno, strNombres, strDNI, strOficinaID, strSistemaID, strEstado, intPageSize, intPageNumber, strcontar);
        }
        public csTablaBE ConsultarLogin(string strCuenta, string strAplicacionID, int intPageSize, int intPageNumber, string strcontar)
        {
            csUsuarioADO objADO = new csUsuarioADO();
            return objADO.ConsultarLogin(strCuenta, strAplicacionID, intPageSize, intPageNumber, strcontar);
        }
        public string Adicionar(csUsuarioBE BE, ArrayList listaRol)
        {
            csUsuarioADO objADO = new csUsuarioADO();
            return objADO.Adicionar(BE, listaRol);
        }

        public string Modificar(csUsuarioBE BE, ArrayList listaRol)
        {
            csUsuarioADO objADO = new csUsuarioADO();
            return objADO.Modificar(BE, listaRol);
        }
        
        public string Anular(string strUsuarioID, string strUsuarioModificacion, string strIPModificacion)
        {
            csUsuarioADO objADO = new csUsuarioADO();
            return objADO.Anular(strUsuarioID, strUsuarioModificacion, strIPModificacion);
        }
    }
}
