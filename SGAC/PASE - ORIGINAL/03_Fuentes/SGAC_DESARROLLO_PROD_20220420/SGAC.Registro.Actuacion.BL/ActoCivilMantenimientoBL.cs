using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Persona.BL;
using SGAC.BE.MRE;
using System.Linq;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoCivilMantenimientoBL
    {

        private string s_Mensaje { get; set; }

        public int InsertarParticipantes(BE.RE_PARTICIPANTE Participantes)
        {
            try
            {
                int intRespuesta = 1;
                long lngPersonaId = 0;
                lngPersonaId = Participantes.iPersonaId;
                ParticipanteMantenimientoBL objBL = new ParticipanteMantenimientoBL();
                var nro = objBL.Insertar(Enumerador.enmTipoActuacionParticipante.CIVIL, Participantes, ref lngPersonaId);
                if (nro == 0)
                {
                    intRespuesta = -1;
                }
                return intRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public int Insertar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE,
                            int IntOficinaConsularId,
                            List<BE.RE_PARTICIPANTE> lstParticipantes,
                            ref long LonRegCivId)
        {
            DA.ActoCivilMantenimientoDA objDA = new DA.ActoCivilMantenimientoDA();

            int intRespuesta;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 3, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
            
            Int32 s_Usuario = ObjRegCivBE.reci_sUsuarioCreacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);
            try
            {
                intRespuesta = objDA.Insertar(ObjRegCivBE, IntOficinaConsularId, ref LonRegCivId);

                ParticipanteMantenimientoBL objBL = new ParticipanteMantenimientoBL();
                long lngPersonaId = 0;
                if (intRespuesta > 0)
                {
                    foreach (BE.RE_PARTICIPANTE objPARTICIPANTE in lstParticipantes.OrderBy(x=> x.sTipoParticipanteId ))
                    {
                        lngPersonaId = objPARTICIPANTE.iPersonaId;
                        objPARTICIPANTE.iActuacionDetId = ObjRegCivBE.reci_iActuacionDetalleId;

                        if (ObjRegCivBE.acde_iReferenciaId != null)
                        {
                            objPARTICIPANTE.iParticipanteId = 0;
                        }

                        var nro = objBL.Insertar(Enumerador.enmTipoActuacionParticipante.CIVIL, objPARTICIPANTE, ref lngPersonaId);
                        if (nro == 0)
                        {
                            intRespuesta = -1;
                            s_Mensaje = ObjRegCivBE.Message;                           
                            break;
                        }

                    }
                }
                else {
                    s_Mensaje = ObjRegCivBE.Message; 
                }


                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                scope.Complete();
                scope.Dispose();

                if (objDA != null)
                {
                    objDA = null;
                }
            }

            return intRespuesta;
        }

        public int Actualizar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE,
                              int IntOficinaConsularId,
                              List<SGAC.BE.RE_PARTICIPANTE> lstParticipantes,//DataTable DtParticipantes,
                              DataTable DtRegDirecciones)
        {
            int intRespuesta = 0;
            DA.ActoCivilMantenimientoDA objDA = new DA.ActoCivilMantenimientoDA();

            Int32 s_Usuario = ObjRegCivBE.reci_sUsuarioModificacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);

            try
            {
                intRespuesta = objDA.Actualizar(ObjRegCivBE,
                                        IntOficinaConsularId,
                                        DtRegDirecciones);

                ParticipanteMantenimientoBL objBL = new ParticipanteMantenimientoBL();
                long lngPersonaId = 0;
                if (intRespuesta > 0)
                {
                    foreach (BE.RE_PARTICIPANTE objPARTICIPANTE in lstParticipantes)
                    {


                        objPARTICIPANTE.iActuacionDetId = ObjRegCivBE.reci_iActuacionDetalleId;
                        objPARTICIPANTE.sUsuarioCreacion = (short)ObjRegCivBE.reci_sUsuarioModificacion;
                        objPARTICIPANTE.sUsuarioModificacion = (short)ObjRegCivBE.reci_sUsuarioModificacion;
                        objPARTICIPANTE.vIPCreacion = ObjRegCivBE.reci_vIPModificacion;
                        objPARTICIPANTE.vIPModificacion = ObjRegCivBE.reci_vIPModificacion;
                        objPARTICIPANTE.vHostname = Util.ObtenerHostName();
                        objPARTICIPANTE.sOficinaConsularId = Convert.ToInt16(IntOficinaConsularId);//ObjRegCivBE.OficinaConsularId;

                        lngPersonaId = objPARTICIPANTE.iPersonaId;
                        var nro = objBL.Insertar(Enumerador.enmTipoActuacionParticipante.CIVIL, objPARTICIPANTE, ref lngPersonaId);
                        if (nro == 0)
                        {
                            intRespuesta = -1;
                            s_Mensaje = ObjRegCivBE.Message; 
                            break;
                        }

                    }
                }
                else { s_Mensaje = ObjRegCivBE.Message; }


                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
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
            return intRespuesta;
        }

        public int Eliminar(BE.MRE.RE_REGISTROCIVIL ObjRegCivBE, int IntOficinaConsularId)
        {
            DA.ActoCivilMantenimientoDA objDA = new DA.ActoCivilMantenimientoDA();
            int intRespuesta = 0;

            Int32 s_Usuario = ObjRegCivBE.reci_sUsuarioModificacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);

            try
            {
                intRespuesta =  objDA.Eliminar(ObjRegCivBE, IntOficinaConsularId);

                if (intRespuesta > 0)
                {
                  
                }
                else { s_Mensaje = ObjRegCivBE.Message; }

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
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


                return intRespuesta;
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