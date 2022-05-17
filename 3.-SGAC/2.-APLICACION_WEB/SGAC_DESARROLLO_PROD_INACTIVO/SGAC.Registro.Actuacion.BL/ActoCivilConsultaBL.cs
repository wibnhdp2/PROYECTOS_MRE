using System;
using System.Data;
using SGAC.Registro.Actuacion.DA;
using System.Collections.Generic;
using SGAC.BE.MRE;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoCivilConsultaBL
    {


        public RE_REGISTROCIVIL USP_REGISTROCIVIL_OBTENER_ID(Int64 iPersonaID) {
            DA.ActoCivilConsultaDA objDA = new ActoCivilConsultaDA();
            return objDA.USP_REGISTROCIVIL_OBTENER_ID(iPersonaID);
        }
        public List<RE_PARTICIPANTE> USP_RE_REGISTROCIVIL_PARTICIPANTE_OBTENER(Int64 iActuacionDetalleID) {
            DA.ActoCivilConsultaDA objDA = new ActoCivilConsultaDA();
            return objDA.USP_RE_REGISTROCIVIL_PARTICIPANTE_OBTENER(iActuacionDetalleID);
        }


        public DataTable Consultar(long LonActuacionDetalleId,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages)
        {
            DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

            try
            {
                return objDA.Consultar(LonActuacionDetalleId,
                                       StrCurrentPage,
                                       IntPageSize,
                                       ref IntTotalCount,
                                       ref IntTotalPages);
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

        public DataTable Obtener(long? LonRegCivId = null, long? LonRegCivIdDet = null)
        {
            DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

            try
            {
                return objDA.Obtener(LonRegCivId, LonRegCivIdDet);
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

        public DataTable ObtenerPorCUI(string strNumeroCUI)
        {
            DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

            try
            {
                return objDA.ObtenerPorCUI(strNumeroCUI);
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

        public Int32 ExisteCui(String reci_vNumeroCUI, Int64 peid_iPersonaId, Int16 IOperacion, Int16 Usuario, String IP, Int16 iOficinaConsularID)
        {
            ActoCivilConsultaDA oActoCivilConsultaDA = new ActoCivilConsultaDA();
            return oActoCivilConsultaDA.ExisteCui(reci_vNumeroCUI, peid_iPersonaId, IOperacion,Usuario,IP,iOficinaConsularID);
        }
        public DataSet ObtenerDatosCivil(long? LonRegCivId = null, long? LonRegCivIdDet = null)
        {
            ActoCivilConsultaDA objDA = new ActoCivilConsultaDA();
            try
            {
                return objDA.ObtenerDatosCivil(LonRegCivId, LonRegCivIdDet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable Imprimir_Acta(Int64 iActuacionDetalleId)
        {
            DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

            try
            {
                return objDA.Imprimir_Acta(iActuacionDetalleId);
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
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 11/11/2019
        // Objetivo: Se ha comentado método sin uso.
        //------------------------------------------------------------------------
        #region Comentada
        //public DataTable Consultar_Acta(Int64 iActuacionDetalleId)
        //{
        //    DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

        //    try
        //    {
        //        return objDA.Consultar_Acta(iActuacionDetalleId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex.InnerException);
        //    }
        //    finally
        //    {
        //        if (objDA != null)
        //        {
        //            objDA = null;
        //        }
        //    }
        //}
#endregion

        public DataTable Consultar_Actas_Titulares(int intCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages, int intRegistroCivilId = 0, string strNumeroActa = "", string strApPaterno = "", string strApMaterno = "", string strNombres = "", Int16 intTipoActaId = 0)
        {
            DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

            try
            {
                return objDA.Consultar_Actas_Titulares(intCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages, intRegistroCivilId, strNumeroActa, strApPaterno, strApMaterno, strNombres, intTipoActaId);
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

        public DataTable Consultar_Formato(long LonActuacionDetalleId, long LonRegistroCivilId)
        {
            DA.ActoCivilConsultaDA objDA = new DA.ActoCivilConsultaDA();

            try
            {
                return objDA.Consultar_Formatos(LonActuacionDetalleId, LonRegistroCivilId);
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