using System.Data;
using SGAC.Contabilidad.CuentaCorriente.DA;
using System;

namespace SGAC.Contabilidad.CuentaCorriente.BL
{
    public class CuentaConsultasBL
    {
        private CuentaConsultasDA objDA;


        public DataTable Consultar(int intOficinaConsularId,
                           int intBancoId,
                           int intPaginaActual,
                           int intPaginaCantidad,
                           ref int intTotalRegistros,
                           ref int intTotalPaginas,
                           string strEstado = "A")
        {
            objDA = new CuentaConsultasDA();
            try
            {
                return objDA.Consultar(intOficinaConsularId,
                                       intBancoId,
                                       intPaginaActual,
                                       intPaginaCantidad,
                                       ref intTotalRegistros,
                                       ref intTotalPaginas,
                                       strEstado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }



        public DataTable ObtenerPorNroCuenta(int intOficinaConsularId, int intBancoId, string strNroCuenta, Int16 iCodCuentaCorriente = 0, string strPeriodo = "", long iTransaccionId = 0)
        {
            objDA = new CuentaConsultasDA();
            try
            {
                return objDA.ObtenerPorNroCuenta(intOficinaConsularId, intBancoId, strNroCuenta, iCodCuentaCorriente, strPeriodo, iTransaccionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }            
        }

        public DataTable ObtenerPorOficinaConsular(int intOficinaConsular)
        {
            objDA = new CuentaConsultasDA();
            try
            {
                return objDA.ObtenerPorOficinaConsular(intOficinaConsular);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public int Existe(int IntCtaId, string StrNroCuenta, int IntBancoId, int IntOperacion)
        {
            DA.CuentaConsultasDA objDA = new DA.CuentaConsultasDA();

            try
            {
                return objDA.Existe(IntCtaId, StrNroCuenta, IntBancoId, IntOperacion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public double ObtenerSaldoCuenta(Int16 sCuentaCorrienteId)
        {

            DA.CuentaConsultasDA objDA = new DA.CuentaConsultasDA();

            try
            {
                return objDA.ObtenerSaldo(sCuentaCorrienteId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }

        }
    }
}
