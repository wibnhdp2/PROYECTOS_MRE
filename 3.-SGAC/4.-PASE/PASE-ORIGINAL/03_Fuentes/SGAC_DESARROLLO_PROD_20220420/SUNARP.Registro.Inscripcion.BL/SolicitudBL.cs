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
//Objetivo: Clase lógica de Solicitud de Inscripción.
//---------------------------------------------------

namespace SUNARP.Registro.Inscripcion.BL
{
    public class SolicitudBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }


        public SU_SOLICITUD_INSCRIPCION insertar(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.SolicitudMantenimientoDA objSolicitudMantenimiento = new SolicitudMantenimientoDA();
            SU_SOLICITUD_INSCRIPCION objBE = new SU_SOLICITUD_INSCRIPCION();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objSolicitudMantenimiento.insertar(solicitud);
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
        public SU_SOLICITUD_INSCRIPCION reingreso(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.SolicitudMantenimientoDA objSolicitudMantenimiento = new SolicitudMantenimientoDA();
            SU_SOLICITUD_INSCRIPCION objBE = new SU_SOLICITUD_INSCRIPCION();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objSolicitudMantenimiento.reingreso(solicitud);
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
        public SU_SOLICITUD_INSCRIPCION eliminar(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.SolicitudMantenimientoDA objSolicitudMantenimiento = new SolicitudMantenimientoDA();
            SU_SOLICITUD_INSCRIPCION objBE = new SU_SOLICITUD_INSCRIPCION();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objSolicitudMantenimiento.eliminar(solicitud);
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
        public SU_SOLICITUD_INSCRIPCION enviar(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.SolicitudMantenimientoDA objSolicitudMantenimiento = new SolicitudMantenimientoDA();
            SU_SOLICITUD_INSCRIPCION objBE = new SU_SOLICITUD_INSCRIPCION();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objSolicitudMantenimiento.enviar(solicitud);
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
        public SU_SOLICITUD_INSCRIPCION ActualizarDocumentoFirmado(SU_SOLICITUD_INSCRIPCION solicitud)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.SolicitudMantenimientoDA objSolicitudMantenimiento = new SolicitudMantenimientoDA();
            SU_SOLICITUD_INSCRIPCION objBE = new SU_SOLICITUD_INSCRIPCION();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objSolicitudMantenimiento.ActualizarDocumentoFirmado(solicitud);
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
        public DataTable SolicitudInscripcionConsulta(Int64 ISOLICITUDINSCRIPCIONID = 0, string CUO = "")
        {
            DA.SolicitudConsultaDA objDA = new SolicitudConsultaDA();

            try
            {
                return objDA.SolicitudInscripcionConsulta(ISOLICITUDINSCRIPCIONID, CUO);
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
        public DataTable SolicitudInscripcionConsultaHistorico(Int64 ISOLICITUDINSCRIPCIONID = 0)
        {
            DA.SolicitudConsultaDA objDA = new SolicitudConsultaDA();

            try
            {
                return objDA.SolicitudInscripcionConsultaHistorico(ISOLICITUDINSCRIPCIONID);
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
        public DataTable consultaMultiple(Int16 iOficinaConsularId = 0, string strNumeroEscritura = "",
            DateTime? fechaInicioExtencion = null, DateTime? FechaFinExtencion = null, Int16 sTipoParticipante = 0,
            Int16 sTipDocumento = 0, string strNumeroDocumento = "",
            string strApPaterno = "", string strApeMaterno = "", string strNombres = "", bool reingreso = false)
        {
            DA.SolicitudConsultaDA objDA = new SolicitudConsultaDA();

            try
            {
                return objDA.SolicitudInscripcionConsultaMultiple(iOficinaConsularId, strNumeroEscritura, fechaInicioExtencion, FechaFinExtencion, sTipoParticipante, sTipDocumento, strNumeroDocumento, strApPaterno, strApeMaterno, strNombres, reingreso);
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

        //---------------------------------------------------


        public DataTable consultaSolicitud(Int16 iOficinaConsularId = 0, string strNumeroEscritura = "",
           DateTime? fechaInicioExtencion = null, DateTime? FechaFinExtencion = null, string sAnioTitulo = "0",
           string strNumeroTitulo = "",
           string strNumeroCUO = "", Int16 iEstado = 0, DateTime? fechaInicioSolicitud = null, DateTime? FechaFinSolicitud = null)
        {
            DA.SolicitudConsultaDA objDA = new SolicitudConsultaDA();

            try
            {
                return objDA.consultaSolicitud(iOficinaConsularId, strNumeroEscritura, fechaInicioExtencion, FechaFinExtencion,
                    sAnioTitulo, strNumeroTitulo, strNumeroCUO, iEstado, fechaInicioSolicitud, FechaFinSolicitud);
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
