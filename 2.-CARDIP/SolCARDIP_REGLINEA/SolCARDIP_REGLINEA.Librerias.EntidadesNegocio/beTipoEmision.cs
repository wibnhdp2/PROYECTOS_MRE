using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    public class beTipoEmision
    {
        //string TIPO_EMSION_NUEVO = "NUEVO";
        //string TIPO_EMSION_EDITAR = "EDITAR";
        public string TIPO_EMSION(int valor)
        {
            if(valor == 0)
            {
                return "NUEVO";
            }
            else
            {
                return "EDITAR";
            }
            //get { return "NUEVO"; }
        }
        
        //public string TIPO_EMSION_NUEVO
        //{
        //    get { return "NUEVO";}
        //}
        //public string TIPO_EMSION_EDITAR
        //{
        //    get { return "EDITAR"; }
        //}
    }
}
