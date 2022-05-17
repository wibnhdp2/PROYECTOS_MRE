using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;

namespace SGAC.Configuracion.Maestro.BL
{
    public class TablaMaestraConsultaBL
    {
        public DataTable Consultar(int IntTablaMaestraId, 
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   string strEstado = "A")
        {
            TablaMaestraConsultaDA objDA = new TablaMaestraConsultaDA();

            try
            {
                return objDA.Consultar(IntTablaMaestraId,
                                    StrCurrentPage,
                                    IntPageSize,
                                    ref IntTotalCount,
                                    ref IntTotalPages,
                                    strEstado);
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
      
        public int Existe(int IntTablaId, int IntOperacion, int IntId, string StrCodigo)
        {
            TablaMaestraConsultaDA objDA = new TablaMaestraConsultaDA();    

            try
            {
                return objDA.Existe(IntTablaId, IntOperacion, IntId, StrCodigo);
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

        public int ObtenerMonedaId(int intOficinaConsularId)
        {
            TablaMaestraConsultaDA Estado = new TablaMaestraConsultaDA();
            return Estado.ObtenerMonedaId(intOficinaConsularId);
        }

        //OBTENER_DOCUMENTO_IDENTIDAD//
        public SGAC.BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD DOCUMENTO_IDENTIDAD_OBTENER(SGAC.BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD documento, Int16 doid_bCui = 0)
        {
            TablaMaestraConsultaDA lTablaMaestraConsultaDA = new TablaMaestraConsultaDA();
            return lTablaMaestraConsultaDA.OBTENER_DOCUMENTO_IDENTIDAD(documento, doid_bCui);            
        }


        public DataTable ConsultarMargenesDocumento(Int16 mado_sOficinaConsular,
                                   Int16 mado_sTipDocImpresion)
        {
            TablaMaestraConsultaDA objDA = new TablaMaestraConsultaDA();

            try
            {
                return objDA.ConsultarMargenesDocumento(mado_sOficinaConsular,
                                    mado_sTipDocImpresion);
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

        public DataTable ObtenerMargenesDocumento(long mado_iCorrelativo)
        {
            TablaMaestraConsultaDA objDA = new TablaMaestraConsultaDA();

            try
            {
                return objDA.ObtenerMargenesDocumento(mado_iCorrelativo);
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

        public DataTable ConsultarDocumentoIdentidad_MRE(Int16 intTipoDocumentoId, string strDescCorta,
                                    string strEstado, Int16 IntPageSize, Int16 intPageActual,
                                    string strContar, ref Int16 IntTotalPages)
        {
            TablaMaestraConsultaDA objDA = new TablaMaestraConsultaDA();

            try
            {
                return objDA.ConsultarDocumentoIdentidad_MRE(intTipoDocumentoId, strDescCorta, strEstado,
                                                        IntPageSize, intPageActual, strContar, ref IntTotalPages);
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
        public DataTable ConsultarTablasMaestrasCargaInicial()
        {
            TablaMaestraConsultaDA objDA = new TablaMaestraConsultaDA();

            try
            {
                return objDA.ConsultarTablasMaestrasCargaInicial();
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
//--------------------------------------
    }
}
