using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.Registro.Persona.DA;

namespace SGAC.Registro.Persona.BL
{
    public class EmpresaMantenimientoBL
    {
        public string ErrMessage { get; set; }

        public Int16 Insertar(BE.RE_EMPRESA objEmpBE, DataTable dtRepresentante, DataTable dtDireccion)
        {
            DA.EmpresaMantenimientoDA objDA = new DA.EmpresaMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            Int32 s_Usuario = objEmpBE.empr_sUsuarioCreacion;
            Int16 s_OficinaConsular = objEmpBE.OficinaConsularId;


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.Insertar_Empresa(ref objEmpBE);
                    if (objDA.ErrMessage.Length != 0) this.ErrMessage = "Empresa:" + objDA.ErrMessage;
                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK) || (objEmpBE.empr_iEmpresaId > 0))
                    {
                        bool Error = false;

                        foreach (DataRow row in dtRepresentante.Rows)
                        {
                            objDA.Insertar_Representante_Legal(
                                objEmpBE.empr_iEmpresaId, row, objEmpBE.empr_sUsuarioCreacion, objEmpBE.OficinaConsularId, ref Error);
                            if (Error == true) this.ErrMessage = this.ErrMessage + " Representante:" + objDA.ErrMessage;
                        }

                        foreach (DataRow row in dtDireccion.Rows)
                        {
                            objDA.Insertar_Direcciones(
                                objEmpBE.empr_iEmpresaId, row, objEmpBE.empr_sUsuarioCreacion, objEmpBE.OficinaConsularId, ref Error);
                            if (Error == true) this.ErrMessage = this.ErrMessage + " Direccion:" + objDA.ErrMessage;
                        }

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
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
                        Transaction.Current.Rollback();
                        scope.Dispose();
                    }

                    if (lCancel == true)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                            audi_vComentario = "",
                            audi_vMensaje = this.ErrMessage,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = (Int16)s_Usuario,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    this.ErrMessage = this.ErrMessage + " Exception: " + ex.Message.ToString();
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

        public Int16 Actualizar(BE.RE_EMPRESA objEmpBE, DataTable dtRepresentante, DataTable dtDireccion)
        {
            EmpresaMantenimientoDA objDA = new EmpresaMantenimientoDA();

            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            Int32 s_Usuario = objEmpBE.empr_sUsuarioCreacion;
            Int16 s_OficinaConsular = objEmpBE.OficinaConsularId;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    intResult = objDA.Actualizar_Empresa(objEmpBE);

                    if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK) || (objEmpBE.empr_iEmpresaId > 0))
                    {
                        bool Error = false;

                        foreach (DataRow row in dtRepresentante.Rows)
                        {
                            objDA.Insertar_Representante_Legal(
                                objEmpBE.empr_iEmpresaId, row, objEmpBE.empr_sUsuarioCreacion, objEmpBE.OficinaConsularId, ref Error);
                        }

                        foreach (DataRow row in dtDireccion.Rows)
                        {
                            objDA.Insertar_Direcciones(
                                objEmpBE.empr_iEmpresaId, row, objEmpBE.empr_sUsuarioCreacion, objEmpBE.OficinaConsularId, ref Error);
                        }

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
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
                        Transaction.Current.Rollback();
                        scope.Dispose();
                    }

                    return intResult;
                }
                catch (Exception ex)
                {
                    this.ErrMessage = ex.Message;

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = this.ErrMessage,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)s_Usuario,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

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

        public Int64 Eliminar(BE.RE_EMPRESA ObjEmp)
        {
            EmpresaMantenimientoDA objDA = new EmpresaMantenimientoDA();

            Int32 s_Usuario = ObjEmp.empr_sUsuarioCreacion;
            Int16 s_OficinaConsular = ObjEmp.OficinaConsularId;

            try
            {
                return objDA.Eliminar(ObjEmp);
            }
            catch (Exception ex)
            {
                this.ErrMessage = ex.Message;

                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = this.ErrMessage,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)s_Usuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
            }
        }

        #region MDIAZ
        public Int64 Insertar(BE.MRE.RE_EMPRESA objEmpBE, List<BE.MRE.RE_REPRESENTANTELEGAL> lstRepresentantes, DataTable dtDireccion)
        {
            Int64 intEmpresaId = 0;

            bool lCancel = false;
            bool Error = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            Int32 s_Usuario = objEmpBE.empr_sUsuarioCreacion;
            Int16 s_OficinaConsular = objEmpBE.OficinaConsultar;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                EmpresaMantenimientoDA objDA = new EmpresaMantenimientoDA();
                objDA.InsertarEmpresa(ref objEmpBE);

                if (objEmpBE.Error == true || objEmpBE.empr_iEmpresaId == 0)
                {
                    lCancel = true;
                    this.ErrMessage = objEmpBE.Message.ToString();
                }
                else
                {
                    Int64 intPersonaId = 0;
                    PersonaMantenimientoBL objBL = new PersonaMantenimientoBL();
                    BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal;
                    foreach (BE.MRE.RE_REPRESENTANTELEGAL objRepresentante in lstRepresentantes)
                    {
                        objRepresentanteLegal = new BE.MRE.RE_REPRESENTANTELEGAL();
                        intPersonaId = objBL.RuneRapido(objRepresentante.PERSONA);
                        if (intPersonaId != 0)
                        {
                            objRepresentante.rele_iEmpresaId = objEmpBE.empr_iEmpresaId;
                            objRepresentante.rele_iPersonaId = intPersonaId;
                            objRepresentante.OficinaConsultar = objRepresentante.OficinaConsultar;

                            objRepresentanteLegal = objDA.InsertarRepresentanteLegal(objRepresentante);
                            if (objRepresentanteLegal.Error == true)
                            {
                                lCancel = true;
                                this.ErrMessage = objRepresentanteLegal.Message;
                                break;
                            }
                        }
                    }

                    if (lCancel != true)
                    {
                        foreach (DataRow row in dtDireccion.Rows)
                        {
                            objDA.Insertar_Direcciones(
                                objEmpBE.empr_iEmpresaId, row, objEmpBE.empr_sUsuarioCreacion, objEmpBE.OficinaConsultar, ref Error);

                            if (Error == true)
                            {
                                lCancel = true;
                                this.ErrMessage = this.ErrMessage + " Direccion:" + objDA.ErrMessage;
                                break;
                            }
                        }
                    }
                }

                if (lCancel == true)
                {
                    intEmpresaId = 0;

                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    intEmpresaId = objEmpBE.empr_iEmpresaId;
                    scope.Complete();
                }                       
            }

            if (lCancel == true)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = this.ErrMessage,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)s_Usuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
            }

            return intEmpresaId;
        }
        public Int64 Actualizar(BE.MRE.RE_EMPRESA objEmpBE, List<BE.MRE.RE_REPRESENTANTELEGAL> lstRepresentantes, DataTable dtDireccion)
        {
            PersonaMantenimientoBL objBL = new PersonaMantenimientoBL();

            Int64 intEmpresaId = 0;            

            bool lCancel = false;
            bool Error = false;
            string strMensaje = string.Empty;

            Int32 s_Usuario = objEmpBE.empr_sUsuarioCreacion;
            Int16 s_OficinaConsular = objEmpBE.OficinaConsultar;


            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {                
                EmpresaMantenimientoDA objDA = new EmpresaMantenimientoDA();
                objEmpBE = objDA.ActualizarEmpresa(objEmpBE);

                intEmpresaId = objEmpBE.empr_iEmpresaId;
                if (objEmpBE.Error == true || objEmpBE.empr_iEmpresaId == 0)
                {
                    lCancel = true;
                    this.ErrMessage = objEmpBE.Message.ToString();
                }
                else
                {
                    Int64 intPersonaId = 0;
                    BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal;
                    foreach (BE.MRE.RE_REPRESENTANTELEGAL objRepresentante in lstRepresentantes)
                    {
                        objRepresentanteLegal = new BE.MRE.RE_REPRESENTANTELEGAL();
                        intPersonaId = objBL.RuneRapido(objRepresentante.PERSONA);
                        if (intPersonaId != 0)
                        {
                            objRepresentante.rele_iEmpresaId = intEmpresaId;
                            objRepresentante.rele_iPersonaId = intPersonaId;
                            objRepresentante.OficinaConsultar = objRepresentante.OficinaConsultar;
                            objRepresentante.rele_cEstado = "A";
                            objRepresentanteLegal = objDA.ActualizarRepresentanteLegal(objRepresentante);
                            if (objRepresentanteLegal.Error == true)
                            {
                                Error = true;
                                lCancel = true;
                                this.ErrMessage = objRepresentanteLegal.Message.ToString();
                                break;
                            }
                        }
                    }

                    if (Error == true)
                    {
                        lCancel = true;
                    }
                    else
                    {
                        foreach (DataRow row in dtDireccion.Rows)
                        {
                            objDA.Insertar_Direcciones(
                                objEmpBE.empr_iEmpresaId, row, objEmpBE.empr_sUsuarioCreacion, objEmpBE.OficinaConsultar, ref Error);

                            if (Error == true)
                            {
                                lCancel = true;
                                this.ErrMessage = this.ErrMessage + " Direccion:" + objDA.ErrMessage;
                                break;
                            }
                        }
                    }
                }

                if (lCancel == true)
                {
                    intEmpresaId = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                    intEmpresaId = objEmpBE.empr_iEmpresaId;
                }
            }

            if (lCancel == true)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = this.ErrMessage,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)s_Usuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
            }

            return intEmpresaId;
        }

        /*
        public long Actualizar(BE.MRE.RE_EMPRESA objEmpBE, List<BE.MRE.RE_REPRESENTANTELEGAL> lstRepresentantes, List<BE.MRE.RE_EMPRESARESIDENCIA> lstResidencias)
        {
            PersonaMantenimientoBL objBL = new PersonaMantenimientoBL();

            Int64 intEmpresaId = 0;

            bool lCancel = false;
            bool Error = false;
            string strMensaje = string.Empty;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                EmpresaMantenimientoDA objDA = new EmpresaMantenimientoDA();
                objEmpBE = objDA.ActualizarEmpresa(objEmpBE);

                intEmpresaId = objEmpBE.empr_iEmpresaId;
                if (objEmpBE.Error == true || objEmpBE.empr_iEmpresaId == 0)
                {
                    lCancel = true;
                }
                else
                {
                    Int64 intPersonaId = 0;
                    BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal;
                    foreach (BE.MRE.RE_REPRESENTANTELEGAL objRepresentante in lstRepresentantes)
                    {
                        objRepresentanteLegal = new BE.MRE.RE_REPRESENTANTELEGAL();
                        intPersonaId = objBL.RuneRapido(objRepresentante.PERSONA);
                        if (intPersonaId != 0)
                        {
                            objRepresentante.rele_iEmpresaId = intEmpresaId;
                            objRepresentante.rele_iPersonaId = intPersonaId;
                            objRepresentante.OficinaConsultar = objRepresentante.OficinaConsultar;
                            objRepresentante.rele_cEstado = "A";
                            objRepresentanteLegal = objDA.ActualizarRepresentanteLegal(objRepresentante);
                            if (objRepresentanteLegal.Error == true)
                            {
                                Error = true;
                                lCancel = true;
                                break;
                            }
                        }
                    }

                    if (Error == true)
                    {
                        lCancel = true;
                    }
                    else
                    {
                        PersonaResidenciaMantenimientoDA objResidenciaDA = new PersonaResidenciaMantenimientoDA();

                        BE.MRE.RE_EMPRESARESIDENCIA objEmpresaResidencia;
                        foreach (BE.MRE.RE_EMPRESARESIDENCIA obj in lstResidencias)
                        {
                            if (obj.emre_iResidenciaId == 0)
                            {
                                objEmpresaResidencia = new BE.MRE.RE_EMPRESARESIDENCIA();                                
                                objEmpresaResidencia.RESIDENCIA = objResidenciaDA.Insertar(obj.RESIDENCIA);

                                obj.emre_iResidenciaId = objEmpresaResidencia.RESIDENCIA.resi_iResidenciaId;

                                //objResidenciaDA.Insert
                            }
                            else
                            {
                                // Actualizar residencia
                            }
                        }
                    }
                }

                if (lCancel == true)
                {
                    intEmpresaId = 0;
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                    intEmpresaId = objEmpBE.empr_iEmpresaId;
                }
            }
            return intEmpresaId;
        }

        */

        #endregion
    }
}