using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessADO;

namespace Seguridad.Logica.BussinessLogic
{
    public class csSistemaBL
    {
        public csTablaBE Consultar(string strSistemaID, string strNombre, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            csSistemaADO objADO = new csSistemaADO();
            return objADO.Consultar(strSistemaID, strNombre, strEstado, intPageSize, intPageNumber, strcontar);
        }
        public byte[] LeerImagen(string strSistemaID)
        {
            csSistemaADO objADO = new csSistemaADO();
            return objADO.LeerImagen(strSistemaID);
        }
        public string Adicionar(csSistemaBE BE)
        {
            csSistemaADO objADO = new csSistemaADO();
            return objADO.Adicionar(BE);
        }
        public string Modificar(csSistemaBE BE)
        {
            csSistemaADO objADO = new csSistemaADO();
            return objADO.Modificar(BE);
        }
        public string ModificarImagen(csSistemaBE BE)
        {
            csSistemaADO objADO = new csSistemaADO();
            return objADO.ModificarImagen(BE);
        }
        public string Anular(string strSistemaID, string strUsuarioModificacion, string strIPModificacion)
        {
            csSistemaADO objADO = new csSistemaADO();
            return objADO.Anular(strSistemaID, strUsuarioModificacion, strIPModificacion);
        }
    }
}
