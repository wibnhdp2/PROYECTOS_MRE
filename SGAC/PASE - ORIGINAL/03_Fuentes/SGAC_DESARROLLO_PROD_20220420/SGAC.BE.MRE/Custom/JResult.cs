using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class JResult : BaseBussinesObject {
        public JResult(){}

        public JResult(Int64 identity, String message) {
            this.Identity = identity;
            this.Message = message;
        }
    }
}
