using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.Text;
using System.Data;

using SUNARP.BE;
using SUNARP.Registro.Inscripcion.DA;

//-----------------------------------------------------------------
//Fecha: 15/01/2021
//Autor: Miguel Márquez Beltrán
//Objetivo: Clase lógica de Seguimiento de estado de la solicitud.
//------------------------------------------------------------------

namespace SUNARP.Registro.Inscripcion.BL
{
    public class SolicitudSeguimientoBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO insertar(SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO seguimientoBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.SolicitudSeguimientoMantenimientoDA objSeguimientoDA = new SolicitudSeguimientoMantenimientoDA();
            SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO objBE = new SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objSeguimientoDA.insertar(seguimientoBE);
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

        public DataTable SeguimientoConsulta(Int64 iSolicitudId)
        {
            DA.SolicitudSeguimientoConsultaDA objDA = new SolicitudSeguimientoConsultaDA();

            try
            {
                return objDA.SeguimientoConsulta(iSolicitudId);
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
