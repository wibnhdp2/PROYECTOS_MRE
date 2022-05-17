using System;
using System.Collections.Generic;
using System.Text;

namespace SGAC.Accesorios
{
    public class Tarifario
    {
        public static double Calculo (BE.MRE.SI_TARIFARIO pobjBE, int intCantidad)
        {
            double douMonto = 0;

            switch (pobjBE.tari_sCalculoTipoId)
            {
                case (int)Enumerador.enmTipoCalculoTarifario.MONTO_FIJO:
                    {
                        douMonto = Convert.ToDouble(pobjBE.tari_FCosto) * intCantidad;
                        break;
                    }
                case (int)Enumerador.enmTipoCalculoTarifario.PORCENTAJE:
                    {
                        douMonto = (Convert.ToDouble(pobjBE.tari_FCosto) / 100 ) * intCantidad;
                        if (pobjBE.tari_ITopeCantidad != null)
                        {
                            if (pobjBE.tari_ITopeCantidad != 0)
                            {
                                if (intCantidad <= pobjBE.tari_ITopeCantidad)
                                {
                                    douMonto = (Convert.ToDouble(pobjBE.tari_FCosto) / 100) * intCantidad;
                                }
                                else
                                {
                                    douMonto = (Convert.ToDouble(pobjBE.tari_FCosto) / 100) * ((double)pobjBE.tari_ITopeCantidad);
                                }
                            }
                        }
                        break;
                    }
                case (int)Enumerador.enmTipoCalculoTarifario.FORMULA:
                    {                        
                        switch (pobjBE.tari_sTarifarioId)
                        {
                            case 16:
                                {
                                    // TARIFA 7C
                                    if (intCantidad > 1)
                                        douMonto = 150*2 + (intCantidad - 2) * 8;
                                    else
                                        douMonto = 150;
                                    break;
                                }
                            case 50:
                                {
                                    // TARIFA 17A
                                    douMonto = 16 + (intCantidad - 1) * 8;
                                    break;
                                }
                            case 52:
                                {
                                    // TARIFA 17C
                                    douMonto = 16 + (intCantidad - 1) * 8;
                                    break;
                                }
                            case 76:
                                {
                                    // TARIFA 30
                                    if (intCantidad > 1)
                                        douMonto = 80 + (intCantidad - 2) * 20;
                                    else            
                                        douMonto = 80;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                    break;
                    }
            }
            return douMonto;
        }


    }
}
