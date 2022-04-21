using System;
using System.Collections.Generic;
using SGAC.BE;
using SGAC.Cliente.Colas.DA;
using SGAC.Integrador.Contratos.Datos;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.BL
{
    public class IntegradorBL
    {
        public string DescargaConfiguracion(int iOficinaConsularId)
        {
            var xFun = new IntegradorDA();
            var dsResult = xFun.DescargaConfiguracion(iOficinaConsularId);
            var strResult = dsResult.GetXml();
            return strResult;
        }

        public bool CargaInformacion(List<Ticket> LisTicket, int iOficinaConsularId)
        {
            CL_TICKET objBE;
            var ListBE = new List<CL_TICKET>();

            foreach (var item in LisTicket)
            {
                objBE = new CL_TICKET();

                if (item != null)
                {
                    objBE.tick_sTipoServicioId = Convert.ToInt16(item.tick_sTipoServicioId);
                    objBE.tick_iPersonalId = item.tick_iPersonalId;
                    objBE.tick_iNumero = item.tick_iNumero;
                    objBE.tick_dFechaHoraGeneracion = Fecha.ConvertirCadena(item.tick_dFechaHoraGeneracion);
                    objBE.tick_dAtencionInicio = Fecha.ConvertirCadena(item.tick_dAtencionInicio);
                    objBE.tick_dAtencionFinal = Fecha.ConvertirCadena(item.tick_dAtencionFinal);
                    objBE.tick_sPrioridadId = Convert.ToInt16(item.tick_sPrioridadId);
                    objBE.tick_sTipoCliente = Convert.ToInt16(item.tick_sTipoCliente);
                    objBE.tick_sTamanoTicket = Convert.ToInt16(item.tick_sTamanoTicket);
                    objBE.tick_sTipoEstado = Convert.ToInt16(item.tick_sTipoEstado);
                    objBE.tick_sTicketeraId = Convert.ToInt16(item.tick_sTicketeraId);
                    objBE.tick_vLLamada = item.tick_vLLamada;
                    objBE.tick_sUsuarioAtendio = Convert.ToInt16(item.tick_sUsuarioAtendio);
                    objBE.tick_cEstado = item.tick_cEstado;
                    objBE.tick_sUsuarioCreacion = Convert.ToInt16(item.tick_sUsuarioCreacion);
                    objBE.tick_vIPCreacion = item.tick_vIPCreacion;
                    objBE.tick_dFechaCreacion = (DateTime)Fecha.ConvertirCadena(item.tick_dFechaCreacion);
                    objBE.tick_sUsuarioModificacion = Convert.ToInt16(item.tick_sUsuarioModificacion);
                    objBE.tick_vIPModificacion = item.tick_vIPModificacion;
                    objBE.tick_dFechaModificacion = Fecha.ConvertirCadena(item.tick_dFechaModificacion);
                    objBE.tick_sVentanillaId = Convert.ToInt16(item.tick_sVentanillaId);

                    ListBE.Add(objBE);
                }
            }

            var xFun = new IntegradorDA();
            return xFun.CargaInformacion(ListBE, iOficinaConsularId);
        }

        public string BuscarPersonaRune(int intDocumentoTipo, string strDocumentoNumero)
        {
            var xFun = new IntegradorDA();
            var strCadena = xFun.BuscarPersonaRune(intDocumentoTipo, strDocumentoNumero);
            return strCadena;
        }
    }
}