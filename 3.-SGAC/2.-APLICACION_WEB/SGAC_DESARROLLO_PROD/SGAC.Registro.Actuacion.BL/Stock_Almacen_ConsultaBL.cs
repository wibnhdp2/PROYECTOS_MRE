using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE.Custom;
using System.Data;
using SGAC.Registro.Actuacion.DA;
using SGAC.BE.MRE;

namespace SGAC.Registro.Actuacion.BL
{
    public class Stock_Almacen_ConsultaBL
    {
        public SI_STOCK_ALMACEN Consultar_Stock_Minimo(long stal_sInsumoId)
        {
            SI_STOCK_ALMACEN obj_Resultado = new SI_STOCK_ALMACEN();
            try
            {
                obj_Resultado = new SGAC.Registro.Actuacion.DA.StockMinimoConsultaDA().Consultar_Stock_Minimo(stal_sInsumoId);
            }
            catch (Exception ex)
            {
                obj_Resultado = null;
                obj_Resultado.Message = ex.Message;
            }
            return obj_Resultado;
        }
    }
}
