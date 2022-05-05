using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.DA.MRE;
using System.Data;
using SGAC.Registro.Actuacion.DA;
//------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase lógica de la Guia de Despacho
//------------------------------------------------------
namespace SGAC.Registro.Actuacion.BL
{ 
    public class GuiaDespachoBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public RE_GUIADESPACHO Insertar(RE_GUIADESPACHO objGuiaDespachoBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.GuiaDespachoDA objGuiaDespachoDA = new GuiaDespachoDA();
            RE_GUIADESPACHO objBE = new RE_GUIADESPACHO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objGuiaDespachoDA.insertar(objGuiaDespachoBE);
                if (objBE.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objBE.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
            return objBE;
        }

        public void Actualizar(RE_GUIADESPACHO objGuiaDespachoBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.GuiaDespachoDA objGuiaDespachoDA = new GuiaDespachoDA();
            RE_GUIADESPACHO objBE = new RE_GUIADESPACHO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objGuiaDespachoDA.actualizar(objGuiaDespachoBE);
                if (objBE.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objBE.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
        }

        public void actualizarEnviado(RE_GUIADESPACHO objGuiaDespachoBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.GuiaDespachoDA objGuiaDespachoDA = new GuiaDespachoDA();
            RE_GUIADESPACHO objBE = new RE_GUIADESPACHO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objGuiaDespachoDA.actualizarEnviado(objGuiaDespachoBE);
                if (objBE.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objBE.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
        }

        public void Anular(RE_GUIADESPACHO objGuiaDespachoBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.GuiaDespachoDA objGuiaDespachoDA = new GuiaDespachoDA();
            RE_GUIADESPACHO objBE = new RE_GUIADESPACHO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objGuiaDespachoDA.anular(objGuiaDespachoBE);
                if (objBE.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objBE.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
        }

        public DataTable Consultar(int intOficinaConsularId, long intGuiaDespachoId, string strFechaInicio, string strFechaFin,
                                 int intTipoEnvioId, string strNombreEmpresaEnvio, string strGuiaAerea,
                                 string strNumeroHoja, string strNumeroGuiaDespacho, int intEstadoGuia,
                                 int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.GuiaDespachoDA objDA = new DA.GuiaDespachoDA();

            try
            {
                return objDA.Consultar(intOficinaConsularId, intGuiaDespachoId, strFechaInicio, strFechaFin, intTipoEnvioId, strNombreEmpresaEnvio, strGuiaAerea,
                                        strNumeroHoja, strNumeroGuiaDespacho, intEstadoGuia,
                                        ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
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

        public DataTable ConsultarNumeroGuiaDespacho(int intOficinaConsularId, long intGuiaDespachoId, string strAnioMes, int intEstadoGuia)
        {
            DA.GuiaDespachoDA objDA = new DA.GuiaDespachoDA();

            try
            {
                return objDA.ConsultarNumeroGuiaDespacho(intOficinaConsularId, intGuiaDespachoId, strAnioMes, intEstadoGuia);                                        
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
