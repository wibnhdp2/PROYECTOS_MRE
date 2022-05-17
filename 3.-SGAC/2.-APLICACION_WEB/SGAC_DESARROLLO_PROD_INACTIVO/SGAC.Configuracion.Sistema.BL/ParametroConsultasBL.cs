using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class ParametroConsultasBL
    {
        public BE.MRE.SI_PARAMETRO Obtener(BE.MRE.SI_PARAMETRO parametro) {
            ParametroConsultasDA lParametroConsultasDA = new ParametroConsultasDA();
            return lParametroConsultasDA.Obtener(parametro);
        }

        public DataTable Consultar(string StrGrupo,
                                   string StrEstado,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages)
        {
            DA.ParametroConsultasDA objDA = new DA.ParametroConsultasDA();      

            try
            {
                          
                return objDA.Consultar(StrGrupo,                                      
                                       StrEstado,
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
        public DataTable ConsultarParametro(int bPrecarga, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            DA.ParametroConsultasDA objDA = new ParametroConsultasDA();
            try
            {
                return objDA.ConsultarParametro(bPrecarga, StrCurrentPage, IntPageSize,ref IntTotalCount,ref IntTotalPages);
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

        public DataTable ConsultarParametroPorValor(string vGrupo,
                                               string vValor, string vItemInicial = "- SELECCIONAR -",string cEstado = "A",
                                                string vDescripcion = "")
        {
            DA.ParametroConsultasDA objDA = new ParametroConsultasDA();
            try
            {
                return objDA.ConsultarParametroPorValor(vGrupo, vValor, vItemInicial, cEstado,vDescripcion);
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
        public DataTable ConsultarParametroMRE(Int16 intParametroId, string strGrupo, string strDescripcion,
                                                       int intPreCarga, string strEstado, int IntPageSize, int IntPageNumber, string strCurrentPage,
                                                       string strContar, ref int IntTotalPages, 
                                                        Int16 intOficinaConsularId, string strTodos)
        {
            DA.ParametroConsultasDA objDA = new ParametroConsultasDA();
            try
            {
                return objDA.ConsultarParametroMRE(intParametroId, strGrupo, strDescripcion, intPreCarga, strEstado, IntPageSize, IntPageNumber,
                                                   strCurrentPage, strContar, ref IntTotalPages, intOficinaConsularId, strTodos);
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
        public DataTable ConsultarOficinasActivasCargaInicial(int intOficinaConsularId)
        {
            DA.ParametroConsultasDA objDA = new ParametroConsultasDA();
            try
            {
                return objDA.ConsultarOficinasActivasCargaInicial(intOficinaConsularId);
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
//----------------------------------------
    }
}
