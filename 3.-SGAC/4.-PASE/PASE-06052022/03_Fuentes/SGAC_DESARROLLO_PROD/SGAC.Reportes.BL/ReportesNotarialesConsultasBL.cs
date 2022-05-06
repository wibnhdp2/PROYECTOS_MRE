using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SGAC.Reportes.DA;

namespace SGAC.Reportes.BL
{
   public class ReportesNotarialesConsultasBL
    {
       ReportesNotarialesConsultasDA oReportesNotarialesConsultasDA = new ReportesNotarialesConsultasDA();

       public String USP_RP_NOTARIAL_PROTOCOLAR_TESTIMONIO(Int64 acno_iActoNotarialId, Int16 sOficinaConsularID, int intCorrelativo, Int64 intiActoNotarialDetalleId)
       {
           return oReportesNotarialesConsultasDA.USP_RP_NOTARIAL_PROTOCOLAR_TESTIMONIO(acno_iActoNotarialId, sOficinaConsularID, intCorrelativo, intiActoNotarialDetalleId);
       }

       public DataTable USP_RP_NOTARIAL_PROTOCOLAR_PARTE(Int64 acno_iActoNotarialId, Int16 sOficinaConsularID,Int16 sNroParte, string strNumeroOficio)
       {
           return oReportesNotarialesConsultasDA.USP_RP_NOTARIAL_PROTOCOLAR_PARTE(acno_iActoNotarialId, sOficinaConsularID, sNroParte, strNumeroOficio);
       }
    }
}
