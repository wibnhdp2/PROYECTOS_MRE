using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class BaseBussinesObject
    {
        public Int64 Identity { get; set; }
        public Boolean Error { get; set; }
        public String Message { get; set; }
        public Int16 OficinaConsultar { get; set; }
        public String HostName { get; set; }
        public SI_AUDITORIA auditoria { get; set; }
    }
}
