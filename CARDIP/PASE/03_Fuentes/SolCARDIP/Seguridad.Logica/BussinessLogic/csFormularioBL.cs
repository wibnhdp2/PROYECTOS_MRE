using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessADO;

namespace Seguridad.Logica.BussinessLogic
{
    public class csFormularioBL
    {
        public csTablaBE Consultar(string strFormularioID, string strAplicacionID, string strNombre, string strVisible, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            csFormularioADO objADO = new csFormularioADO();
            return objADO.Consultar(strFormularioID, strAplicacionID, strNombre, strVisible, strEstado, intPageSize, intPageNumber, strcontar);
        }
        public DataTable ConsultarMenu(string strAplicacionID, string strRolOpcion, string strRutaPagina, string strVisible)
        {
            csFormularioADO objADO = new csFormularioADO();
            return objADO.ConsultarMenu(strAplicacionID, strRolOpcion, strRutaPagina, strVisible);
        }
        public string Adicionar(csFormularioBE BE)
        {
            csFormularioADO objADO = new csFormularioADO();
            return objADO.Adicionar(BE);
        }

        public string Modificar(csFormularioBE BE)
        {
            csFormularioADO objADO = new csFormularioADO();
            return objADO.Modificar(BE);
        }

        public string Anular(string strFormularioID, string strUsuarioModificacion, string strIPModificacion)
        {
            csFormularioADO objADO = new csFormularioADO();
            return objADO.Anular(strFormularioID, strUsuarioModificacion, strIPModificacion);
        }
    }
}
