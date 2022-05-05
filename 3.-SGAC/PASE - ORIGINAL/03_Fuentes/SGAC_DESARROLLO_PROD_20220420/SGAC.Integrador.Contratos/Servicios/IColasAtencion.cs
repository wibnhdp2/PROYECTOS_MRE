using System.Collections.Generic;
using System.Net.Security;
using SGAC.Integrador.Contratos.Datos;
using WCF = System.ServiceModel;

namespace SGAC.Integrador.Contratos
{
    [WCF.ServiceContractAttribute(Namespace = "http://IntegradorSGAC", Name = "IColasAtencion",
        SessionMode = WCF.SessionMode.Allowed, ProtectionLevel = ProtectionLevel.None)]
    [WCF.XmlSerializerFormatAttribute]
    public interface IColasAtencion
    {
        [WCF.OperationContractAttribute(Action = "DescargaConfiguracion", ReplyAction = "", IsTerminating = false,
            IsInitiating = true, IsOneWay = false, AsyncPattern = false, ProtectionLevel = ProtectionLevel.None)]
        string DescargaConfiguracion(int intOficinaConsularId);

        [WCF.OperationContractAttribute(Action = "CargaInformacion", ReplyAction = "", IsTerminating = false,
            IsInitiating = true, IsOneWay = false, AsyncPattern = false, ProtectionLevel = ProtectionLevel.None)]
        bool CargaInformacion(List<Ticket> lstTicket, int intOficinaConsularId);

        [WCF.OperationContractAttribute(Action = "BuscarPersonaRune", ReplyAction = "", IsTerminating = false,
            IsInitiating = true, IsOneWay = false, AsyncPattern = false, ProtectionLevel = ProtectionLevel.None)]
        string BuscarPersonaRune(int intDocumentoTipo, string strDocumentoNumero);
    }
}