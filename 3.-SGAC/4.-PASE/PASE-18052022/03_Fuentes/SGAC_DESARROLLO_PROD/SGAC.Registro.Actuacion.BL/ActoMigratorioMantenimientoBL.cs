using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE.Custom;
using System.Transactions;
using SGAC.DA.MRE.ACTOMIGRATORIO;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoMigratorioMantenimientoBL
    {
        private string s_Mensaje { get; set; }

        public string InsertarPasaporteExpedido(CBE_MIGRATORIO cbePasaporte)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbePasaporte.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbePasaporte.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = cbePasaporte.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOFORMATO_DA objActoMigratorioFormatoDA = new RE_ACTOMIGRATORIOFORMATO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbePasaporte.ACTO = objActoMigratorioDA.insertar(cbePasaporte.ACTO);
                if ((cbePasaporte.ACTO.Error == false) || (cbePasaporte.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region FORMATO
                    cbePasaporte.FORMATO.amfr_iActoMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                    cbePasaporte.FORMATO = objActoMigratorioFormatoDA.insertar(cbePasaporte.FORMATO);
                    if ((cbePasaporte.FORMATO.Error == true) || (cbePasaporte.FORMATO.amfr_iActoMigratorioFormatoId == 0))
                    {
                        lCancel = true;
                        s_Mensaje = cbePasaporte.FORMATO.Message;
                    }
                    #endregion

                    #region Histórico
                    if ((lCancel == false) && (cbePasaporte.HISTORICO.Count != 0))
                    {
                        List<RE_ACTOMIGRATORIOHISTORICO> lstHistorico = new List<RE_ACTOMIGRATORIOHISTORICO>();
                        foreach (RE_ACTOMIGRATORIOHISTORICO objHistorico in cbePasaporte.HISTORICO)
                        {
                            objHistorico.amhi_iActoMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;

                            RE_ACTOMIGRATORIOHISTORICO ins = objActoMigratorioHistoricoDA.insertar(objHistorico);
                            if ((ins.Error == true) || (ins.amhi_iActoMigratorioHistoricoId == 0)) { lCancel = true; s_Mensaje = ins.Message.ToString(); }
                            else { lstHistorico.Add(ins); }
                        }
                        cbePasaporte.HISTORICO = lstHistorico;
                    }
                    #endregion

                    #region PERSONA
                    cbePasaporte.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                    cbePasaporte.PERSONA.acmi_sMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                    cbePasaporte.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(cbePasaporte.PERSONA);
                    if ((cbePasaporte.PERSONA.Error == true) || (cbePasaporte.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = cbePasaporte.PERSONA.Message.ToString(); }
                    #endregion
                }
                else { lCancel = true; s_Mensaje = cbePasaporte.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public string ActualizarPasaporteExpedido(CBE_MIGRATORIO cbePasaporte)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbePasaporte.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbePasaporte.ACTO.OficinaConsultar;


            Int64 sTipo_Acto = cbePasaporte.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOFORMATO_DA objActoMigratorioFormatoDA = new RE_ACTOMIGRATORIOFORMATO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbePasaporte.ACTO = objActoMigratorioDA.actualizar(cbePasaporte.ACTO);
                if ((cbePasaporte.ACTO.Error == false) || (cbePasaporte.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region FORMATO
                    cbePasaporte.FORMATO.amfr_iActoMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                    cbePasaporte.FORMATO = objActoMigratorioFormatoDA.actualizar(cbePasaporte.FORMATO);
                    if ((cbePasaporte.FORMATO.Error == true) || (cbePasaporte.FORMATO.amfr_iActoMigratorioFormatoId == 0))
                    {
                        lCancel = true;
                        s_Mensaje = cbePasaporte.FORMATO.Message;
                    }
                    #endregion
                    
                    cbePasaporte.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                    cbePasaporte.PERSONA.acmi_sMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;

                    cbePasaporte.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(cbePasaporte.PERSONA);
                    if ((cbePasaporte.PERSONA.Error == true) || (cbePasaporte.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = cbePasaporte.PERSONA.Message; }

                }
                else { lCancel = true; s_Mensaje = cbePasaporte.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                //    Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public string InsertarPasaporteRevalidado(CBE_MIGRATORIO cbePasaporte)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbePasaporte.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbePasaporte.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = cbePasaporte.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOFORMATO_DA objActoMigratorioFormatoDA = new RE_ACTOMIGRATORIOFORMATO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbePasaporte.ACTO = objActoMigratorioDA.insertar(cbePasaporte.ACTO);
                if ((cbePasaporte.ACTO.Error == false) || (cbePasaporte.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region FORMATO
                    cbePasaporte.FORMATO.amfr_iActoMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                    cbePasaporte.FORMATO = objActoMigratorioFormatoDA.insertar(cbePasaporte.FORMATO);
                    if ((cbePasaporte.FORMATO.Error == true) || (cbePasaporte.FORMATO.amfr_iActoMigratorioFormatoId == 0))
                    {
                        lCancel = true;
                        s_Mensaje = cbePasaporte.FORMATO.Message;
                    }
                    #endregion

                    #region Histórico
                    if ((lCancel == false) && (cbePasaporte.HISTORICO.Count != 0))
                    {
                        List<RE_ACTOMIGRATORIOHISTORICO> lstHistorico = new List<RE_ACTOMIGRATORIOHISTORICO>();
                        foreach (RE_ACTOMIGRATORIOHISTORICO objHistorico in cbePasaporte.HISTORICO)
                        {
                            objHistorico.amhi_iActoMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;

                            RE_ACTOMIGRATORIOHISTORICO ins = objActoMigratorioHistoricoDA.insertar(objHistorico);
                            if ((ins.Error == true) || (ins.amhi_iActoMigratorioHistoricoId == 0)) { lCancel = true; s_Mensaje = ins.Message; }
                            else { lstHistorico.Add(ins); }
                        }
                        cbePasaporte.HISTORICO = lstHistorico;
                    }
                    #endregion

                    #region PERSONA
                    if (lCancel == false)
                    {
                        cbePasaporte.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                        cbePasaporte.PERSONA.acmi_sMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                        cbePasaporte.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(cbePasaporte.PERSONA);
                        if ((cbePasaporte.PERSONA.Error == true) || (cbePasaporte.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = cbePasaporte.PERSONA.Message; }
                    }
                    #endregion
                }
                else { lCancel = true; s_Mensaje = cbePasaporte.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public string ActualizarPasaporteRevalidado(CBE_MIGRATORIO cbePasaporte)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbePasaporte.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbePasaporte.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = cbePasaporte.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOFORMATO_DA objActoMigratorioFormatoDA = new RE_ACTOMIGRATORIOFORMATO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbePasaporte.ACTO = objActoMigratorioDA.actualizar(cbePasaporte.ACTO);
                if ((cbePasaporte.ACTO.Error == false) || (cbePasaporte.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region FORMATO
                    cbePasaporte.FORMATO.amfr_iActoMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                    cbePasaporte.FORMATO = objActoMigratorioFormatoDA.actualizar(cbePasaporte.FORMATO);
                    if ((cbePasaporte.FORMATO.Error == true) || (cbePasaporte.FORMATO.amfr_iActoMigratorioFormatoId == 0))
                    {
                        lCancel = true;
                        s_Mensaje = cbePasaporte.FORMATO.Message;
                    }
                    #endregion

                   

                    if (lCancel == false)
                    {
                        cbePasaporte.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                        cbePasaporte.PERSONA.acmi_sMigratorioId = cbePasaporte.ACTO.acmi_iActoMigratorioId;
                        cbePasaporte.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(cbePasaporte.PERSONA);
                        if ((cbePasaporte.PERSONA.Error == true) || (cbePasaporte.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = cbePasaporte.PERSONA.Message; }
                    }
                }
                else { lCancel = true; s_Mensaje = cbePasaporte.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public string InsertarSalvoconducto(CBE_MIGRATORIO salvoconducto)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = salvoconducto.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = salvoconducto.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = salvoconducto.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                salvoconducto.ACTO = objActoMigratorioDA.insertar(salvoconducto.ACTO);
                if ((salvoconducto.ACTO.Error == false) || (salvoconducto.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region HISTÓRICO
                    if ((lCancel == false) && (salvoconducto.HISTORICO.Count != 0))
                    {
                        List<RE_ACTOMIGRATORIOHISTORICO> lstHistorico = new List<RE_ACTOMIGRATORIOHISTORICO>();
                        foreach (RE_ACTOMIGRATORIOHISTORICO objHistorico in salvoconducto.HISTORICO)
                        {
                            objHistorico.amhi_iActoMigratorioId = salvoconducto.ACTO.acmi_iActoMigratorioId;

                            RE_ACTOMIGRATORIOHISTORICO ins = objActoMigratorioHistoricoDA.insertar(objHistorico);
                            if ((ins.Error == true) || (ins.amhi_iActoMigratorioHistoricoId == 0)) { lCancel = true; s_Mensaje = ins.Message; }
                            else { lstHistorico.Add(ins); }
                        }
                        salvoconducto.HISTORICO = lstHistorico;
                    }
                    #endregion

                    salvoconducto.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                    salvoconducto.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(salvoconducto.PERSONA);
                    if ((salvoconducto.PERSONA.Error == true) || (salvoconducto.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = salvoconducto.PERSONA.Message; }

                }
                else { lCancel = true; s_Mensaje = salvoconducto.ACTO.Message; }

                if (!lCancel) { scope.Complete();  }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;

        }

        public string ActualizarSalvoconducto(CBE_MIGRATORIO salvoconducto)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = salvoconducto.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = salvoconducto.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = salvoconducto.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                salvoconducto.ACTO = objActoMigratorioDA.actualizar(salvoconducto.ACTO);
                if ((salvoconducto.ACTO.Error == false) || (salvoconducto.ACTO.acmi_iActoMigratorioId != 0))
                {
                   
                    salvoconducto.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                    salvoconducto.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(salvoconducto.PERSONA);
                    if ((salvoconducto.PERSONA.Error == true) || (salvoconducto.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = salvoconducto.PERSONA.Message; }

                }
                else { lCancel = true; s_Mensaje = salvoconducto.ACTO.Message; }

                if (!lCancel) { scope.Complete();  }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public string InsertarVisa(CBE_MIGRATORIO cbeVisa)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbeVisa.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbeVisa.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = cbeVisa.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOFORMATO_DA objActoMigratorioFormatoDA = new RE_ACTOMIGRATORIOFORMATO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbeVisa.ACTO = objActoMigratorioDA.insertar(cbeVisa.ACTO);
                if ((cbeVisa.ACTO.Error == false) || (cbeVisa.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region FORMATO
                    cbeVisa.FORMATO.amfr_iActoMigratorioId = cbeVisa.ACTO.acmi_iActoMigratorioId;
                    cbeVisa.FORMATO = objActoMigratorioFormatoDA.insertar(cbeVisa.FORMATO);
                    if ((cbeVisa.FORMATO.Error == true) || (cbeVisa.FORMATO.amfr_iActoMigratorioFormatoId == 0))
                    {
                        lCancel = true;
                        s_Mensaje = cbeVisa.FORMATO.Message;
                    }
                    
                    #endregion

                    #region Histórico
                    if ((lCancel == false) && (cbeVisa.HISTORICO.Count != 0))
                    {
                        List<RE_ACTOMIGRATORIOHISTORICO> lstHistorico = new List<RE_ACTOMIGRATORIOHISTORICO>();
                        foreach (RE_ACTOMIGRATORIOHISTORICO objHistorico in cbeVisa.HISTORICO)
                        {
                            objHistorico.amhi_iActoMigratorioId = cbeVisa.ACTO.acmi_iActoMigratorioId;

                            RE_ACTOMIGRATORIOHISTORICO ins = objActoMigratorioHistoricoDA.insertar(objHistorico);
                            if ((ins.Error == true) || (ins.amhi_iActoMigratorioHistoricoId == 0)) { lCancel = true; s_Mensaje = ins.Message; }
                            else { lstHistorico.Add(ins);
                            }
                        }
                        cbeVisa.HISTORICO = lstHistorico;
                    }
                    #endregion
                    cbeVisa.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                    cbeVisa.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(cbeVisa.PERSONA);
                    if ((cbeVisa.PERSONA.Error == true) || (cbeVisa.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = cbeVisa.PERSONA.Message; }
                    

                }
                else { lCancel = true; s_Mensaje = cbeVisa.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public string Actualizar_Migratorio(CBE_MIGRATORIO cbeVisa)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbeVisa.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbeVisa.ACTO.OficinaConsultar;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbeVisa.ACTO = objActoMigratorioDA.Actualizar_Migratorio(cbeVisa.ACTO);
                if ((cbeVisa.ACTO.Error == false) || (cbeVisa.ACTO.acmi_iActoMigratorioId != 0))
                {
                    
                }
                else { lCancel = true; s_Mensaje = cbeVisa.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }
        public string ActualizarVisa(CBE_MIGRATORIO cbeVisa)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = cbeVisa.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = cbeVisa.ACTO.OficinaConsultar;

            Int64 sTipo_Acto = cbeVisa.ACTO.acmi_sTipoDocumentoMigratorioId;

            RE_ACTOMIGRATORIO_DA objActoMigratorioDA = new RE_ACTOMIGRATORIO_DA();
            RE_ACTOMIGRATORIOFORMATO_DA objActoMigratorioFormatoDA = new RE_ACTOMIGRATORIOFORMATO_DA();
            RE_ACTOMIGRATORIOHISTORICO_DA objActoMigratorioHistoricoDA = new RE_ACTOMIGRATORIOHISTORICO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                cbeVisa.ACTO = objActoMigratorioDA.actualizar(cbeVisa.ACTO);
                if ((cbeVisa.ACTO.Error == false) || (cbeVisa.ACTO.acmi_iActoMigratorioId != 0))
                {
                    #region FORMATO
                    cbeVisa.FORMATO.amfr_iActoMigratorioId = cbeVisa.ACTO.acmi_iActoMigratorioId;
                    cbeVisa.FORMATO = objActoMigratorioFormatoDA.actualizar(cbeVisa.FORMATO);
                    if ((cbeVisa.FORMATO.Error == true) || (cbeVisa.FORMATO.amfr_iActoMigratorioFormatoId == 0))
                    {
                        lCancel = true;
                        s_Mensaje = cbeVisa.FORMATO.Message;
                    }
                    #endregion

                    
                    cbeVisa.PERSONA.acmi_sTipoDocumentoMigratorioId = sTipo_Acto;
                    cbeVisa.PERSONA = objActoMigratorioDA.Actualizar_Datos_Persona(cbeVisa.PERSONA);
                    if ((cbeVisa.PERSONA.Error == true) || (cbeVisa.PERSONA.pers_iPersonaId == 0)) { lCancel = true; s_Mensaje = cbeVisa.PERSONA.Message; }


                }
                else { lCancel = true; s_Mensaje = cbeVisa.ACTO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(s_Mensaje))
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

                s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
            }

            return s_Mensaje;
        }

        public int Actualizar_Estados(CBE_MIGRATORIO oCBE_MIGRATORIO)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            int iCodigo = 0;

            Int32 s_Usuario = oCBE_MIGRATORIO.ACTO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = oCBE_MIGRATORIO.ACTO.OficinaConsultar;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                oCBE_MIGRATORIO.ACTO = new RE_ACTOMIGRATORIO_DA().actualizar_estados(oCBE_MIGRATORIO.ACTO);
                if ((oCBE_MIGRATORIO.ACTO.Error == true) || (oCBE_MIGRATORIO.ACTO.acmi_iActoMigratorioId == 0)) {lCancel = true;s_Mensaje=oCBE_MIGRATORIO.ACTO.Message;}
                else
                {
                    #region Histórico
                    if ((lCancel == false) && (oCBE_MIGRATORIO.HISTORICO.Count != 0))
                    {
                        List<RE_ACTOMIGRATORIOHISTORICO> lstHistorico = new List<RE_ACTOMIGRATORIOHISTORICO>();
                        foreach (RE_ACTOMIGRATORIOHISTORICO objHistorico in oCBE_MIGRATORIO.HISTORICO)
                        {
                            objHistorico.amhi_iActoMigratorioId = oCBE_MIGRATORIO.ACTO.acmi_iActoMigratorioId;

                            RE_ACTOMIGRATORIOHISTORICO ins = new RE_ACTOMIGRATORIOHISTORICO_DA().insertar(objHistorico);
                            if ((ins.Error == true) || (ins.amhi_iActoMigratorioHistoricoId == 0)) { lCancel = true; s_Mensaje = ins.Message; }
                            else
                            {
                                lstHistorico.Add(ins);
                                iCodigo = (int)ins.amhi_iActoMigratorioHistoricoId;
                            }
                        }
                        oCBE_MIGRATORIO.HISTORICO = lstHistorico;
                    }
                    #endregion
                }

                if (!lCancel) { scope.Complete(); }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(s_Mensaje))
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

            if (s_Mensaje == null) return iCodigo; else return 0;
        }

        public int Actualizar_Lamina(RE_ACTOMIGRATORIO oRE_ACTOMIGRATORIO)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = oRE_ACTOMIGRATORIO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = oRE_ACTOMIGRATORIO.OficinaConsultar;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                oRE_ACTOMIGRATORIO = new RE_ACTOMIGRATORIO_DA().actualizar_estado_lamina(oRE_ACTOMIGRATORIO);
                if ((oRE_ACTOMIGRATORIO.Error == true) || (oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId == 0)) { lCancel = true; s_Mensaje = oRE_ACTOMIGRATORIO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            if (!string.IsNullOrEmpty(s_Mensaje))
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

            if (s_Mensaje == null) return 1; else return 0;
        }

        public int Actualizar_Passaporte(RE_ACTOMIGRATORIO oRE_ACTOMIGRATORIO)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = oRE_ACTOMIGRATORIO.acmi_sUsuarioCreacion;
            Int16 s_OficinaConsular = oRE_ACTOMIGRATORIO.OficinaConsultar;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                oRE_ACTOMIGRATORIO = new RE_ACTOMIGRATORIO_DA().actualizar_estado_pasaporte(oRE_ACTOMIGRATORIO);
                if ((oRE_ACTOMIGRATORIO.Error == true) || (oRE_ACTOMIGRATORIO.acmi_iActoMigratorioId == 0)) { lCancel = true; s_Mensaje = oRE_ACTOMIGRATORIO.Message; }

                if (!lCancel) { scope.Complete(); }
                else
                {
                //    Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            if (!string.IsNullOrEmpty(s_Mensaje))
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

            if (s_Mensaje == null) return 1; else return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intInsumoLaminaId"></param>
        /// <param name="strCodLamina"></param>
        /// <param name="intOficinaConsularId"></param>
        /// <param name="intUsuarioModificacionId"></param>
        /// <param name="strLamina"></param>
        /// <param name="strMensaje"></param>
        /// <returns></returns>
        public int Validar_Insumo(int intInsumoLaminaId, string strCodLamina,
            int intOficinaConsularId, int intUsuarioModificacionId, string strLamina, ref string strMensaje)
        {
            return new RE_ACTOMIGRATORIO_DA().Validar_Lamina(intInsumoLaminaId,
                strCodLamina, intOficinaConsularId, intUsuarioModificacionId, strLamina, ref strMensaje);
        }
    }
}
