using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using SGAC.Cliente.Colas.BL;
using SGAC.Integrador.Contratos;
using SGAC.Integrador.Contratos.Datos;
using WCF = System.ServiceModel;

namespace SGAC.Integrador.Implementacion
{
    [WCF.ServiceBehaviorAttribute(Name = "ColasAtencionServicio", Namespace = "http://IntegradorSGAC",
        InstanceContextMode = WCF.InstanceContextMode.Single, ConcurrencyMode = WCF.ConcurrencyMode.Multiple)]
    public abstract class ColasAtencionServicioBase : IColasAtencion
    {
        public virtual string DescargaConfiguracion(int intOficinaConsularId)
        {
            return null;
        }

        public virtual bool CargaInformacion(List<Ticket> lstTicket, int intOficinaConsularId)
        {
            return false;
        }

        public virtual string BuscarPersonaRune(int intDocumentoTipo, string strDocumentoNumero)
        {
            return null;
        }
    }

    public class ColasAtencionServicio : ColasAtencionServicioBase
    {
        public override string DescargaConfiguracion(int intOficinaConsularId)
        {
            string strResult = null;

            try
            {
                var objBL = new IntegradorBL();
                strResult = objBL.DescargaConfiguracion(intOficinaConsularId);
            }
            catch (DataException ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        public override bool CargaInformacion(List<Ticket> lstTicket, int intOficinaConsularId)
        {
            try
            {
                var objBL = new IntegradorBL();
                return objBL.CargaInformacion(lstTicket, intOficinaConsularId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public override string BuscarPersonaRune(int intDocumentoTipo, string strDocumentoNumero)
        {
            string strResult = null;

            try
            {
                var objBL = new IntegradorBL();
                strResult = objBL.BuscarPersonaRune(intDocumentoTipo, strDocumentoNumero);
            }
            catch (DataException ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }
    }
}