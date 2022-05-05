using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using SGAC.Accesorios;
using SGAC.BE.MRE.Custom;
using SGAC.Registro.Persona.DA;
using SGAC.BE.MRE;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaAsistenciaMantenimientoBL
    {
        #region Prueba RUNE RAPIDO - MDIAZ
       
        public Int64 Insertar(BE.MRE.RE_ASISTENCIA ObjAsisBE, List<BE.MRE.RE_ASISTENCIABENEFICIARIO> lstBeneficiarios)
        {
            PersonaMantenimientoBL objBL = new PersonaMantenimientoBL();

            Int64 intAsistencia = 0;
            bool lCancel = false;
            bool Error = false;

            string s_Mensaje = string.Empty;

            Int32 s_Usuario = ObjAsisBE.asis_sUsuarioCreacion;
            Int16 s_OficinaConsular = ObjAsisBE.OficinaConsultar;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                PersonaAsistenciaMantenimientoDA objDA = new PersonaAsistenciaMantenimientoDA();
                objDA.InsertarAsistencia(ref ObjAsisBE);

                if (ObjAsisBE.Error == true) { lCancel = true; s_Mensaje = ObjAsisBE.Message.ToString(); }
                else
                {
                    Int64 intPersonaId = 0;
                    BE.MRE.RE_ASISTENCIABENEFICIARIO objAsistenciaBeneficiario;
                    foreach (BE.MRE.RE_ASISTENCIABENEFICIARIO objBeneficiario in lstBeneficiarios)
                    {
                        objAsistenciaBeneficiario = new BE.MRE.RE_ASISTENCIABENEFICIARIO();
                        intPersonaId = objBL.RuneRapido(objBeneficiario.PERSONA);
                        if (intPersonaId != 0)
                        {
                            objAsistenciaBeneficiario.asbe_iAsistenciaId = ObjAsisBE.asis_iAsistenciaId;
                            objAsistenciaBeneficiario.asbe_iPersonaId = intPersonaId;
                            objAsistenciaBeneficiario.asbe_FMonto = objBeneficiario.asbe_FMonto;
                            objAsistenciaBeneficiario.asbe_sUsuarioCreacion = objBeneficiario.asbe_sUsuarioCreacion;
                            objAsistenciaBeneficiario.OficinaConsultar = objBeneficiario.OficinaConsultar;

                            objAsistenciaBeneficiario = objDA.InsertarBeneficiario(objAsistenciaBeneficiario);
                            if (objAsistenciaBeneficiario.Error == true)
                            {
                                lCancel = true;
                                s_Mensaje = objAsistenciaBeneficiario.Message.ToString();
                                break;
                            }
                        }
                    }
                    if (Error == true) { lCancel = true; }
                }

                if (lCancel == true)
                {
                    intAsistencia = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else {
                    intAsistencia = ObjAsisBE.asis_iAsistenciaId;
                    scope.Complete();
                }
            }

            if (intAsistencia == 0)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = s_Mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)s_Usuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
            }

            return intAsistencia;
        }
        public Int64 Actualizar(BE.MRE.RE_ASISTENCIA ObjAsisBE, List<BE.MRE.RE_ASISTENCIABENEFICIARIO> lstBeneficiarios)
        {
            PersonaMantenimientoBL objBL = new PersonaMantenimientoBL();

            Int64 intAsistencia = 0;
            bool lCancel = false;
            bool Error = false;
            string strMensaje = string.Empty;

            Int32 s_Usuario = ObjAsisBE.asis_sUsuarioCreacion;
            Int16 s_OficinaConsular = ObjAsisBE.OficinaConsultar;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                PersonaAsistenciaMantenimientoDA objDA = new PersonaAsistenciaMantenimientoDA();
                objDA.Actualizar(ObjAsisBE, ref strMensaje);

                //if (ObjAsisBE.Error == true) { lCancel = true; }
                if (strMensaje != string.Empty) { lCancel = true; }
                else
                {
                    Int64 intPersonaId = 0;
                    BE.MRE.RE_ASISTENCIABENEFICIARIO objAsistenciaBeneficiario;
                    foreach (BE.MRE.RE_ASISTENCIABENEFICIARIO objBeneficiario in lstBeneficiarios)
                    {
                        objAsistenciaBeneficiario = new BE.MRE.RE_ASISTENCIABENEFICIARIO();
                        intPersonaId = objBL.RuneRapido(objBeneficiario.PERSONA);
                        if (intPersonaId != 0)
                        {
                            objAsistenciaBeneficiario.asbe_iAsistenciaBeneficiarioId = objBeneficiario.asbe_iAsistenciaBeneficiarioId;
                            objAsistenciaBeneficiario.asbe_iAsistenciaId = ObjAsisBE.asis_iAsistenciaId;
                            objAsistenciaBeneficiario.asbe_iPersonaId = intPersonaId;
                            objAsistenciaBeneficiario.asbe_FMonto = objBeneficiario.asbe_FMonto;
                            objAsistenciaBeneficiario.asbe_sUsuarioModificacion = objBeneficiario.asbe_sUsuarioModificacion;
                            objAsistenciaBeneficiario.OficinaConsultar = objBeneficiario.OficinaConsultar;
                            objAsistenciaBeneficiario = objDA.ActualizarBeneficiario(objAsistenciaBeneficiario);
                            if (objAsistenciaBeneficiario.Error == true)
                            {
                                Error = true;
                                lCancel = true;
                                strMensaje = objAsistenciaBeneficiario.Message.ToString();
                                break;
                            }
                        }
                    }
                    if (Error == true) { lCancel = true; }
                }                

                if (lCancel == true)
                {
                    intAsistencia = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else { 
                    scope.Complete();
                    intAsistencia = (int)Enumerador.enmResultadoQuery.OK;
                }

                if (intAsistencia == 0)
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = strMensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)s_Usuario,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                }

            }
            return intAsistencia;
        }
         
        #endregion
        
        public int Eliminar(BE.MRE.RE_ASISTENCIA ObjAsisBE)
        {
            int rpta = 0;
            bool lCancel = false;
            bool Error = false;

            Int32 s_Usuario = ObjAsisBE.asis_sUsuarioCreacion;
            Int16 s_OficinaConsular = ObjAsisBE.OficinaConsultar;

            string MensajeError = "";

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                DA.PersonaAsistenciaMantenimientoDA objDA = new DA.PersonaAsistenciaMantenimientoDA();
                

                Error = objDA.Eliminar(ObjAsisBE, ref MensajeError);
                if (Error == true) { lCancel = true; }

                rpta = (int)Enumerador.enmResultadoQuery.OK;

                if (lCancel == true)
                {
                    rpta = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else { scope.Complete(); }


            }

            if (rpta == 0)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = MensajeError,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)s_Usuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
            }

            return rpta;

        }

        // Insertar
        /*
        public int Insertar(BE.MRE.RE_ASISTENCIA ObjAsisBE, DataTable dtBeneficiario)
        {
            int rpta = 0;
            bool lCancel = false;
            bool Error = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                DA.PersonaAsistenciaMantenimientoDA objDA = new DA.PersonaAsistenciaMantenimientoDA();
                string MensajeError = "";

                Error = objDA.Insertar(ObjAsisBE, ref MensajeError);
                if (Error == true) { lCancel = true; }
                else
                {                    
                    Error = objDA.InsertarBeneficiario(ObjAsisBE, dtBeneficiario, ref MensajeError);
                    if (Error == true) { lCancel = true; }
                }

                rpta = (int)Enumerador.enmResultadoQuery.OK;

                if (lCancel == true)
                {
                    rpta = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else { scope.Complete(); }
            }
            return rpta;

        }
        */
        // Actualizar
        /*
        public int Actualizar(BE.MRE.RE_ASISTENCIA ObjAsisBE, DataTable dtBeneficiario)
        {
            int rpta = 0;
            bool lCancel = false;
            bool Error = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                DA.PersonaAsistenciaMantenimientoDA objDA = new DA.PersonaAsistenciaMantenimientoDA();
                string MensajeError = "";

                Error = objDA.Actualizar(ObjAsisBE, ref MensajeError);
                if (Error == true) { lCancel = true; }
                else
                {
                    Error = objDA.ActualizarBeneficiario(ObjAsisBE, dtBeneficiario, ref MensajeError);
                    if (Error == true) { lCancel = true; }
                }

                rpta = (int)Enumerador.enmResultadoQuery.OK;

                if (lCancel == true)
                {
                    rpta = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else { scope.Complete(); }

            }
            return rpta;


        }
        */
    }
}