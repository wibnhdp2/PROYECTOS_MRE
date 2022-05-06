using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.Accesorios
{
    [Serializable]
    public class beUbigeoListas
    {
        public List<beUbicaciongeografica> Ubigeo01 { get; set; }
        public List<beUbicaciongeografica> Ubigeo02 { get; set; }
        public List<beUbicaciongeografica> Ubigeo03 { get; set; }
    }
}
