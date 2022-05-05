using System;
using System.Collections.Generic;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;
using SGAC.Registro.Persona.BL;
using System.Transactions;
using System.Data;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoMilitarMantenimientoBL
    {
        private ActoMilitarMantenimientoDA objDA;

        public Int64 Insertar(BE.RE_REGISTROMILITAR objBE, BE.RE_PERSONA objBEP, BE.RE_RESIDENCIA objREResidencia,
            List<BE.RE_PARTICIPANTE> LstPARTICIPANTE)
        {
            
            objDA = new ActoMilitarMantenimientoDA();

            bool lCancel = false;
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.Insertar(objBE, objBEP);
                    

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                    {
                        #region Residencia

                        if (objREResidencia.resi_cResidenciaUbigeo != null)
                        {
                            PersonaResidenciaMantenimientoBL objResidenciaBL = new PersonaResidenciaMantenimientoBL();
                            BE.RE_PERSONARESIDENCIA objREPersonaResidencia = new RE_PERSONARESIDENCIA();
                            objREPersonaResidencia.pere_iResidenciaId = objREResidencia.resi_iResidenciaId;
                            objREPersonaResidencia.pere_iPersonaId = objBEP.pers_iPersonaId;
                            objREPersonaResidencia.pere_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                            objREPersonaResidencia.pere_sUsuarioCreacion = objBEP.pers_sUsuarioCreacion;
                            objREPersonaResidencia.pere_sUsuarioModificacion = objBEP.pers_sUsuarioModificacion;
                            objREPersonaResidencia.pere_vIPCreacion = objBEP.pers_vIPCreacion;
                            objREPersonaResidencia.pere_vIPModificacion = objBEP.pers_vIPModificacion;

                            if (objREResidencia.resi_iResidenciaId == -1)
                            {
                                objResidenciaBL.Insertar(objREResidencia, objREPersonaResidencia, Convert.ToInt32(objBE.OficinaConsularId));                                
                            }
                            else
                            {
                                objResidenciaBL.Actualizar(objREResidencia, objREPersonaResidencia, Convert.ToInt32(objREResidencia.OficinaConsularId));
                            }

                            ValidacionError(objResidenciaBL.strError, objREResidencia.OficinaConsularId, objREResidencia.resi_sUsuarioCreacion);
                        }

                        #endregion Residencia

                        #region Participante

                        ParticipanteMantenimientoBL objBL = new ParticipanteMantenimientoBL();
                        long lngPersonaId = 0;

                        foreach (BE.RE_PARTICIPANTE objPARTICIPANTE in LstPARTICIPANTE)
                        {
                            lngPersonaId = objPARTICIPANTE.iPersonaId;
                            var nro = objBL.Insertar(Enumerador.enmTipoActuacionParticipante.MILITAR, objPARTICIPANTE, ref lngPersonaId);
    
                            if (nro == 0)
                            {
                                lCancel = true;
                                break;
                            }
                        }


                        #endregion

                    }
                    else
                    {
                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }


                    return objBE.remi_iRegistroMilitarId;

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    return 0;
                }
                finally
                {
                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.remi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }

            }
            
        }

        public int Actualizar(BE.RE_REGISTROMILITAR objBE, BE.RE_PERSONA objBEP, BE.RE_RESIDENCIA objREResidencia, List<BE.RE_PARTICIPANTE> LstPARTICIPANTE)
        {
            objDA = new ActoMilitarMantenimientoDA();

            bool lCancel = false;
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.Actualizar(objBE, objBEP);
                    

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                    {

                        #region Residencia

                        if (objREResidencia.resi_cResidenciaUbigeo != null)
                        {
                            PersonaResidenciaMantenimientoBL objResidenciaBL = new PersonaResidenciaMantenimientoBL();
                            BE.RE_PERSONARESIDENCIA objREPersonaResidencia = new RE_PERSONARESIDENCIA();
                            objREPersonaResidencia.pere_iResidenciaId = objREResidencia.resi_iResidenciaId;
                            objREPersonaResidencia.pere_iPersonaId = objBEP.pers_iPersonaId;
                            objREPersonaResidencia.pere_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                            objREPersonaResidencia.pere_sUsuarioCreacion = objBEP.pers_sUsuarioCreacion;
                            objREPersonaResidencia.pere_sUsuarioModificacion = objBEP.pers_sUsuarioModificacion;
                            objREPersonaResidencia.pere_vIPCreacion = objBEP.pers_vIPCreacion;
                            objREPersonaResidencia.pere_vIPModificacion = objBEP.pers_vIPModificacion;
                            if (objREResidencia.resi_iResidenciaId == -1)
                            {
                                objResidenciaBL.Insertar(objREResidencia, objREPersonaResidencia, Convert.ToInt32(objBE.OficinaConsularId));
                            }
                            else
                            {
                                objResidenciaBL.Actualizar(objREResidencia, objREPersonaResidencia, Convert.ToInt32(objREResidencia.OficinaConsularId));
                            }

                            ValidacionError(objResidenciaBL.strError, objREResidencia.OficinaConsularId, objREResidencia.resi_sUsuarioCreacion);
                        }

                        #endregion Residencia

                        #region Participante

                        ParticipanteMantenimientoBL objBL = new ParticipanteMantenimientoBL();
                        long lngPersonaId = 0;

                        foreach (BE.RE_PARTICIPANTE objPARTICIPANTE in LstPARTICIPANTE)
                        {
                            lngPersonaId = objPARTICIPANTE.iPersonaId;
                            var nro = objBL.Insertar(Enumerador.enmTipoActuacionParticipante.CIVIL, objPARTICIPANTE, ref lngPersonaId);                            

                            if (nro == 0)
                            {
                                lCancel = true;
                                break;
                            }
                        }


                        #endregion

                    }
                    else
                    {
                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }


                    return 1;//>0
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    return 0;
                }
                finally
                {
                    ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.remi_sUsuarioCreacion);

                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }

        public int Eliminar(BE.RE_REGISTROMILITAR ObjRegMilBE, int IntOficinaConsularId)
        {
            DA.ActoMilitarMantenimientoDA objDA = new DA.ActoMilitarMantenimientoDA();

            bool lCancel = false;
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);


            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.Eliminar(ObjRegMilBE, IntOficinaConsularId);

                    

                    if (intResult != Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                    {
                        lCancel = true;
                    }

                    //Finalizando transacción
                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new DataException();
                    }


                    return 1;//>0
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    return 0;
                }
                finally
                {
                    ValidacionError(objDA.strError, ObjRegMilBE.OficinaConsularId, ObjRegMilBE.remi_sUsuarioCreacion);
                    
                    if (objDA != null)
                    {
                        objDA = null;
                    }
                }
            }
        }

        public void ValidacionError(string mensaje, Int16 sOficinaConsular, Int16 sUsuario)
        {

            if (!string.IsNullOrEmpty(mensaje))
            {


                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = sOficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = sUsuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

            }
        }
    }
}