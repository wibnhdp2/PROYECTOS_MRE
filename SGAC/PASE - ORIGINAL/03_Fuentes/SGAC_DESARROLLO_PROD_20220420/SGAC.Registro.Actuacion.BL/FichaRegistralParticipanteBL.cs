using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.DA.MRE;
using System.Data;
using SGAC.Registro.Actuacion.DA;
//---------------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase lógica de la ficha registral de Participantes
//---------------------------------------------------------------

namespace SGAC.Registro.Actuacion.BL
{
    public class FichaRegistralParticipanteBL
    {
        private string s_Mensaje { get; set; }

        public DataTable Consultar(long intFichaRegistralId, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.FichaRegistralParticipanteDA objDA = new FichaRegistralParticipanteDA();
            try
            {
                return objDA.Consultar(intFichaRegistralId, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
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

     
        public void insertar(ref RE_FICHAREGISTRALPARTICIPANTE objFichaRegistralParticpanteBE)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            FichaRegistralParticipanteDA objFichaRegistralParticpanteDA = new FichaRegistralParticipanteDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {                
                objFichaRegistralParticpanteDA.insertar(ref objFichaRegistralParticpanteBE);
                if (objFichaRegistralParticpanteBE.Error)
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (objFichaRegistralParticpanteBE.Error) { s_Mensaje = objFichaRegistralParticpanteBE.PERSONA.Message; }


            if (!string.IsNullOrEmpty(s_Mensaje))
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)objFichaRegistralParticpanteBE.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = s_Mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)objFichaRegistralParticpanteBE.fipa_sUsuarioCreacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

        }

        public void actualizar(ref RE_FICHAREGISTRALPARTICIPANTE objFichaRegistralParticpanteBE)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            FichaRegistralParticipanteDA objFichaRegistralParticpanteDA = new FichaRegistralParticipanteDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objFichaRegistralParticpanteDA.actualizar(ref objFichaRegistralParticpanteBE);
                if (objFichaRegistralParticpanteBE.Error)
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
            if (objFichaRegistralParticpanteBE.Error) { s_Mensaje = objFichaRegistralParticpanteBE.PERSONA.Message; }

            if (!string.IsNullOrEmpty(s_Mensaje))
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)objFichaRegistralParticpanteBE.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = s_Mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)objFichaRegistralParticpanteBE.fipa_sUsuarioCreacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }
        }

        public void anular(ref RE_FICHAREGISTRALPARTICIPANTE objFichaRegistralParticpanteBE)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            FichaRegistralParticipanteDA objFichaRegistralParticpanteDA = new FichaRegistralParticipanteDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objFichaRegistralParticpanteDA.anular(ref objFichaRegistralParticpanteBE);
                if (objFichaRegistralParticpanteBE.Error)
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
            if (objFichaRegistralParticpanteBE.Error) { s_Mensaje = objFichaRegistralParticpanteBE.PERSONA.Message; }

            if (!string.IsNullOrEmpty(s_Mensaje))
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)objFichaRegistralParticpanteBE.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = s_Mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)objFichaRegistralParticpanteBE.fipa_sUsuarioCreacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

        }

        //---------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 10/03/2017
        // Objetivo: Solo agrega participante sin actualizar nada
        //---------------------------------------------------------------
        public void AgregarParticipante(ref RE_FICHAREGISTRALPARTICIPANTE objFichaRegistralParticpanteBE)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            FichaRegistralParticipanteDA objFichaRegistralParticpanteDA = new FichaRegistralParticipanteDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objFichaRegistralParticpanteDA.AgregarParticipante(ref objFichaRegistralParticpanteBE);
                if (objFichaRegistralParticpanteBE.Error)
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (objFichaRegistralParticpanteBE.Error) { s_Mensaje = objFichaRegistralParticpanteBE.PERSONA.Message; }


            if (!string.IsNullOrEmpty(s_Mensaje))
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)objFichaRegistralParticpanteBE.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = s_Mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)objFichaRegistralParticpanteBE.fipa_sUsuarioCreacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

        }


        //---------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 18/02/2019
        // Objetivo: Solo agrega documento adjunto
        //---------------------------------------------------------------
        public string AgregarDocumentoAdjuntoFicha(long fido_iFichaRegistralID, Int16 fido_sDocumentoID, string fido_vNumDocumentoID, Int16 fido_sUsuarioModificacion, string fido_vIPModificacion)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            FichaRegistralParticipanteDA objFichaRegistralParticpanteDA = new FichaRegistralParticipanteDA();
            string Resultado = "";
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Resultado = objFichaRegistralParticpanteDA.AgregarDocumentoAdjuntoFicha(fido_iFichaRegistralID, fido_sDocumentoID, fido_vNumDocumentoID, fido_sUsuarioModificacion, fido_vIPModificacion);
                if (Resultado != "OK")
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
            return Resultado;
        }

        //---------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 18/02/2019
        // Objetivo: Solo Actualiza documento adjunto
        //---------------------------------------------------------------
        public string ActualizarDocumentoAdjuntoFicha(long fido_iFichaRegistralDocumentoID, Int16 fido_sDocumentoID, string fido_vNumDocumentoID, string fido_cEstado, Int16 fido_sUsuarioModificacion, string fido_vIPModificacion)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            FichaRegistralParticipanteDA objFichaRegistralParticpanteDA = new FichaRegistralParticipanteDA();
            string Resultado = "";
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Resultado = objFichaRegistralParticpanteDA.ActualizarDocumentoAdjuntoFicha(fido_iFichaRegistralDocumentoID, fido_sDocumentoID, fido_vNumDocumentoID, fido_cEstado, fido_sUsuarioModificacion, fido_vIPModificacion);
                if (Resultado != "OK")
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
            return Resultado;
        }

        public DataTable ConsultarDocAdjuntos(long fido_iFichaRegistralID)
        {
            DA.FichaRegistralParticipanteDA objDA = new FichaRegistralParticipanteDA();
            try
            {
                return objDA.ConsultarDocAdjuntos(fido_iFichaRegistralID);
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
