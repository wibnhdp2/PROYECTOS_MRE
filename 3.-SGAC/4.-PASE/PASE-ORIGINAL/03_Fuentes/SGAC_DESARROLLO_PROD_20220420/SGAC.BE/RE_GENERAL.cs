using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    public class RE_GENERAL
    {
        public Int16 ToNullInt16(object s_Valor)
        {
            Int16 i_Resultado = 0;
            try
            {
                i_Resultado = Convert.ToInt16(s_Valor);
            }
            catch
            {
                i_Resultado = 0;
            }
            return i_Resultado;
        }

        public Boolean ToNullBoolean(object s_Valor)
        {
            Boolean i_Resultado = false;
            try
            {
                i_Resultado = Convert.ToBoolean(s_Valor);
            }
            catch
            {
                i_Resultado = false;
            }
            return i_Resultado;
        }

        public Int64 ToNullInt64(object s_Valor)
        {
            Int64 i_Resultado = 0;
            try
            {
                i_Resultado = Convert.ToInt64(s_Valor);
            }
            catch
            {
                i_Resultado = 0;
            }
            return i_Resultado;
        }

        public DateTime ToNullDateTime(object strFecha)
        {
            DateTime datFecha = DateTime.MinValue;
            if (!strFecha.ToString().Equals(""))
                if (!DateTime.TryParse(strFecha.ToString(), out datFecha))
                    datFecha = Convert.ToDateTime(strFecha.ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
            return datFecha;

        }
    }
}
