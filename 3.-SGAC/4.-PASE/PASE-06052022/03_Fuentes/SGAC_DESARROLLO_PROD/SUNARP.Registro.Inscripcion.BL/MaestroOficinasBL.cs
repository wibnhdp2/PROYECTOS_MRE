using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.Text;
using System.Data;

using SUNARP.BE;
using SUNARP.Registro.Inscripcion.DA;

//---------------------------------------------------
//Fecha: 06/10/2020
//Autor: Miguel Márquez Beltrán
//Objetivo: Clase lógica del Maestro de Oficinas.
//---------------------------------------------------

namespace SUNARP.Registro.Inscripcion.BL
{
    public class MaestroOficinasBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public SU_MAESTRO_OFICINAS insertar(SU_MAESTRO_OFICINAS oficinaBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.MaestroOficinasMantenimientoDA objOficinaDA = new MaestroOficinasMantenimientoDA();
            SU_MAESTRO_OFICINAS objBE = new SU_MAESTRO_OFICINAS();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objOficinaDA.insertar(oficinaBE);
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

        public SU_MAESTRO_OFICINAS actualizar(SU_MAESTRO_OFICINAS oficinaBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.MaestroOficinasMantenimientoDA objOficinaDA = new MaestroOficinasMantenimientoDA();
            SU_MAESTRO_OFICINAS objBE = new SU_MAESTRO_OFICINAS();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objOficinaDA.actualizar(oficinaBE);
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

        public DataTable consultar(short intOficinaId,
            string strCodigoZona, string strCodigoOficina, string strEstado, string strFiltrarOficinas,
            int intPageSize, string StrCurrentPage, string strContar, ref int intTotalPages)
        {
            DA.MaestroOficinasConsultaDA objDA = new MaestroOficinasConsultaDA();

            try
            {
                return objDA.MaestroOficinasConsulta(intOficinaId, strCodigoZona, strCodigoOficina, strEstado, strFiltrarOficinas,
                                                    intPageSize, StrCurrentPage, strContar, ref intTotalPages);
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
 //-----------------------------------------------------
    }
}
