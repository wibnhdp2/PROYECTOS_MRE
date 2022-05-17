using System;
using System.Data;
using SGAC.BE.MRE;
using SGAC.Configuracion.Sistema.DA;
using System.Transactions;
using SGAC.Accesorios;
using System.Collections.Generic;

namespace SGAC.Configuracion.Sistema.BL
{

    public class NormaTarifarioDL
    {

        #region Metodos_Norma_Tarifario

        public DataTable Consultar(Int16 intPagoId, Int16 intNormaId, string strTarifaLetra, string strFecha, bool bExcepcion,
            int intPageSize, int intPageNumber, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DA.NormaTarifarioDA objDA = new NormaTarifarioDA();

            try
            {
                return objDA.Consultar(intPagoId, intNormaId, strTarifaLetra, strFecha, bExcepcion, intPageSize, intPageNumber, strContar, ref IntTotalCount, ref IntTotalPages);
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
        public bool ConsultarCantidadPagoNormaTarifario(Int16 intNormaId, Int16 intTarifarioId, Int16 intPagoId)
        {
            DA.NormaTarifarioDA objDA = new NormaTarifarioDA();

            try
            {
                return objDA.ConsultarCantidadPagoNormaTarifario(intNormaId, intTarifarioId, intPagoId);
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

        public SI_NORMA_TARIFARIO InsertarNormaTarifario(List<SGAC.BE.MRE.SI_NORMA_TARIFARIO> listaNormaTarifario)
        {
            NormaTarifarioDA objDA = new NormaTarifarioDA();

            SI_NORMA_TARIFARIO objNormaTarifarioBE = new SI_NORMA_TARIFARIO();

            Int16 intOficinaConsularId = listaNormaTarifario[0].OficinaConsultar;
            Int16 intUsuarioId = (Int16)listaNormaTarifario[0].nota_sUsuarioCreacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    for (int i = 0; i < listaNormaTarifario.Count; i++)
                    {
                        objNormaTarifarioBE = objDA.InsertarNormaTarifario(listaNormaTarifario[i]);
                        if (objNormaTarifarioBE.Error)
                        {
                            break;
                        }
                    }
                    
                    if (objNormaTarifarioBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objNormaTarifarioBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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
            return objNormaTarifarioBE;
        }

        public SI_NORMA_TARIFARIO ActualizarNormaTarifario(List<SGAC.BE.MRE.SI_NORMA_TARIFARIO> listaNormaTarifario)
        {
            NormaTarifarioDA objDA = new NormaTarifarioDA();

            SI_NORMA_TARIFARIO objNormaTarifarioBE = new SI_NORMA_TARIFARIO();

            Int16 intOficinaConsularId = listaNormaTarifario[0].OficinaConsultar;
            Int16 intUsuarioId = (Int16)listaNormaTarifario[0].nota_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    for (int i = 0; i < listaNormaTarifario.Count; i++)
                    {
                        objNormaTarifarioBE = objDA.ActualizarNormaTarifario(listaNormaTarifario[i]);
                        if (objNormaTarifarioBE.Error)
                        {
                            break;
                        }
                    }                    

                    if (objNormaTarifarioBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objNormaTarifarioBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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
            return objNormaTarifarioBE;
        }

        public SI_NORMA_TARIFARIO AnularNormaTarifario(SI_NORMA_TARIFARIO objNormaTarifario)
        {
            NormaTarifarioDA objDA = new NormaTarifarioDA();

            SI_NORMA_TARIFARIO objNormaTarifarioBE = new SI_NORMA_TARIFARIO();

            Int16 intOficinaConsularId = objNormaTarifario.OficinaConsultar;
            Int16 intUsuarioId = (Int16)objNormaTarifario.nota_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objNormaTarifarioBE = objDA.AnularNormaTarifario(objNormaTarifario);

                    if (objNormaTarifarioBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objNormaTarifarioBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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
            return objNormaTarifarioBE;
        }

        #endregion

        #region Metodos_Norma

        public DataTable ConsultarNorma(short intTipoNormaId, short intObjetoNormaId, string strDescripcionCortaNorma,
            string strFechaInicial, string strFechaFinal, short intEstadoId, short intGrupoNormaId, 
            int intPageSize, int intPageNumber, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DA.NormaTarifarioDA objDA = new NormaTarifarioDA();

            try
            {
                return objDA.ConsultarNorma(intTipoNormaId, intObjetoNormaId, strDescripcionCortaNorma,
                    strFechaInicial, strFechaFinal, intEstadoId, intGrupoNormaId, 
                    intPageSize, intPageNumber, strContar, ref IntTotalCount, ref IntTotalPages);
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

        public SI_NORMA InsertarNorma(SI_NORMA objNorma)
        {
            NormaTarifarioDA objDA = new NormaTarifarioDA();
            SI_NORMA objNormaBE = new SI_NORMA();
           

            Int16 intOficinaConsularId = objNorma.OficinaConsultar;
            Int16 intUsuarioId = (Int16)objNormaBE.norm_sUsuarioCreacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objNormaBE = objDA.InsertarNorma(objNorma);

                    if (objNormaBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objNormaBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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
            return objNormaBE;
        }

        public SI_NORMA ActualizarNorma(SI_NORMA objNorma)
        {
            NormaTarifarioDA objDA = new NormaTarifarioDA();
            SI_NORMA objNormaBE = new SI_NORMA();


            Int16 intOficinaConsularId = objNorma.OficinaConsultar;
            Int16 intUsuarioId = (Int16)objNorma.norm_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objNormaBE = objDA.ActualizarNorma(objNorma);

                    if (objNormaBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objNormaBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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
            return objNormaBE;
        }

        public SI_NORMA AnularNorma(SI_NORMA objNorma)
        {
            NormaTarifarioDA objDA = new NormaTarifarioDA();
            SI_NORMA objNormaBE = new SI_NORMA();


            Int16 intOficinaConsularId = objNorma.OficinaConsultar;
            Int16 intUsuarioId = (Int16)objNorma.norm_sUsuarioModificacion;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    objNormaBE = objDA.AnularNorma(objNorma);

                    if (objNormaBE.Error)
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = intOficinaConsularId,
                            audi_vComentario = "",
                            audi_vMensaje = objNormaBE.Message,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = intUsuarioId,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });
                    }
                    else
                        scope.Complete();
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
            return objNormaBE;
        }

        public DataTable ListaTitulosArticulosNorma(short intNormaId, short intTipoNormaId=0, short intObjetoNormaId=0, short intEstadoId=0, short intGrupoNormaId=0)
        {
            DA.NormaTarifarioDA objDA = new NormaTarifarioDA();

            try
            {
                return objDA.ListaTitulosArticulosNorma(intNormaId, intTipoNormaId, intObjetoNormaId, intEstadoId, intGrupoNormaId);
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


        #endregion


    }
}
