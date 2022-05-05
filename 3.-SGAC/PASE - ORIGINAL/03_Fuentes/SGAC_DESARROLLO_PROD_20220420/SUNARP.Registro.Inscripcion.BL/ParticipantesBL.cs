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
//Objetivo: Clase lógica de Participantes.
//---------------------------------------------------

namespace SUNARP.Registro.Inscripcion.BL
{
    public class ParticipantesBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public SU_PARTICIPANTES insertar(SU_PARTICIPANTES participanteBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.ParticipantesMantenimientoDA objParticipanteDA = new ParticipantesMantenimientoDA();
            SU_PARTICIPANTES objBE = new SU_PARTICIPANTES();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objParticipanteDA.insertar(participanteBE);
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

        public SU_PARTICIPANTES actualizar(SU_PARTICIPANTES participanteBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.ParticipantesMantenimientoDA objParticipanteDA = new ParticipantesMantenimientoDA();
            SU_PARTICIPANTES objBE = new SU_PARTICIPANTES();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objParticipanteDA.actualizar(participanteBE);
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

        public DataTable consultar(Int64 intSolicitudId, Int64 intParticipanteId=0)
        {
            DA.ParticipantesConsultaDA objDA = new ParticipantesConsultaDA();

            try
            {
                return objDA.ParticipantesConsulta(intSolicitudId, intParticipanteId);
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
        //----------------------------------------------
    }
}
